using Grpc.Auth;
using NAudio.Extras;
using NAudio.Wave;
using WebRtcVadSharp;
using Google.Cloud.Speech.V1;
using Google.Apis.Auth.OAuth2;


namespace MotoCap
{
    public partial class Form1 : Form
    {
        private WaveInEvent waveInEvent;
        private WaveFileWriter writer;
        private bool isRecording;
        private SpeechClient speechClient;
        private string outputFolderPath;
        //private Grpc.Core.Channel channel;
        private Equalizer equalizer;
        private bool speechDetected;
        private WebRtcVad vad;

        private int hertz_rate = 48000;



        // Define the equalizer bands (adjust as needed)
        private EqualizerBand[] bands = new EqualizerBand[]
        {
            new EqualizerBand { Frequency = 100, Bandwidth = 1.0f, Gain = 0 },
            new EqualizerBand { Frequency = 1000, Bandwidth = 1.0f, Gain = 0 },
            new EqualizerBand { Frequency = 5000, Bandwidth = 1.0f, Gain = 0 }
        };

        public Form1()
        {
            InitializeComponent();

            // Load Google Cloud service account credentials from the JSON file
            string jsonFilePath = "C:\\Users\\user\\source\\repos\\MotoCap\\MotoCap\\credentials\\api.json";
            GoogleCredential credential;

            try
            {
                credential = GoogleCredential.FromFile(jsonFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load credentials from file: {ex.Message}");
                return;
            }

            // Create the speech client using the credentials
            speechClient = new SpeechClientBuilder
            {
                ChannelCredentials = credential.ToChannelCredentials()
            }.Build();

        }


        private void Form1_Load(object sender, EventArgs e)
        {
            //set initial output path
            outputFolderPath = "C:/MotoRecOutput/";

            // Initialize the equalizer
            equalizer = new Equalizer((ISampleProvider)waveInEvent, bands);

            // Subscribe to the Scroll event of each trackbar/slider
            trackBar1.Scroll += EqualizerBand_Scroll;
            trackBar2.Scroll += EqualizerBand_Scroll;
            trackBar3.Scroll += EqualizerBand_Scroll;
        }

        private void EqualizerBand_Scroll(object sender, EventArgs e)
        {
            // Update the equalizer band gain based on the trackbar/slider values
            bands[0].Gain = trackBar1.Value;
            bands[1].Gain = trackBar2.Value;
            bands[2].Gain = trackBar3.Value;

            // Apply the changes
            equalizer.Update();
        }

        private void btnRecord_Click(object sender, EventArgs e)
        {
            if (outputFolderPath != null)
            {
                if (!isRecording)
                {

                    // Update UI
                    btnRecord.Enabled = false;
                    btnStop.Enabled = true;
                    record_status_label.Text = "recording";
                    voice_status_label.Enabled = true;

                    waveInEvent = new WaveInEvent
                    {
                        WaveFormat = new WaveFormat(hertz_rate, 16, 1) // Update sample rate and bit depth here
                    };

                    
                    waveInEvent.DataAvailable += WaveInEvent_DataAvailable;
                    writer = new WaveFileWriter($"{outputFolderPath}{DateTime.Now:yyyyMMdd_HHmmss}.wav", waveInEvent.WaveFormat);


                 

                    waveInEvent.StartRecording();
                    isRecording = true;
                }
            }
            else
            {
                MessageBox.Show("Please select where to save your recordings first!");
            }
        }


        private void WaveInEvent_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (isRecording)
            {
                // Write the recorded audio data to the output file
                writer.Write(e.Buffer, 0, e.BytesRecorded);

                speechDetected = DoesFrameContainSpeech(e.Buffer);

                // Update the UI control using Invoke
                voice_status_label.Invoke(new Action(() =>
                {
                    voice_status_label.Text = speechDetected ? "voice detected" : "silence";
                }));
            }
        }

         bool DoesFrameContainSpeech(byte[] audioFrame)
        {
            using var vad = new WebRtcVad();
            return vad.HasSpeech(audioFrame, SampleRate.Is48kHz, FrameLength.Is10ms);
        }


        private async void btnStop_Click(object sender, EventArgs e)
        {
            if (waveInEvent != null)
            {
                // Stop recording
                waveInEvent.StopRecording();
                waveInEvent.Dispose();
                writer.Close();

                // Update UI
                btnRecord.Enabled = true;
                btnStop.Enabled = false;
                record_status_label.Text = "stooped";
                voice_status_label.Enabled = false;

                // Save recorded audio file
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string outputFileName = $"Recording_{timestamp}.wav";
                string outputFilePath = Path.Combine(outputFolderPath, outputFileName);
                File.Move(writer.Filename, outputFilePath);

                ConvertAudioToSpeech(outputFilePath);

                isRecording = false;

                // Perform speech-to-text conversion
                //  ConvertSpeechToText();
            }
        }

        private void ConvertAudioToSpeech(string outputFilePath)
        {
            if (File.Exists(outputFilePath))
            {
                //var speech = SpeechClient.Create();
                RecognizeResponse response = speechClient.Recognize(new RecognitionConfig()
                {
                    Encoding = RecognitionConfig.Types.AudioEncoding.Linear16,
                    SampleRateHertz = hertz_rate,
                    LanguageCode = "uk-UA",
                }, RecognitionAudio.FromFile(outputFilePath));


                foreach (var result in response.Results)
                {
                    foreach (var alternatives in result.Alternatives)
                    {
                        richTextBox1.Text = richTextBox1.Text + " " + alternatives.Transcript;
                    }
                }
            }
            else
            {
                richTextBox1.Text = "Your recording was missing";
            }
        }

        // its still questionable, should I use this approach or abondon it...
        private void btnSelectJson_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "JSON Files (*.json)|*.json";
                openFileDialog.Title = "Select JSON Credentials File";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = openFileDialog.FileName;

                    // Load Google Cloud service account credentials from the selected file
                    GoogleCredential credential = null;
                    try
                    {
                        credential = GoogleCredential.FromFile(selectedFilePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to load credentials from file: {ex.Message}");
                        return;
                    }

                    // Create a channel using the credentials
                    //channel = new Grpc.Core.Channel(SpeechClient.DefaultEndpoint.ToString(), credential.ToChannelCredentials());

                    // Create the speech client using the custom channel
                    SpeechClientBuilder builder = new SpeechClientBuilder
                    {
                        ChannelCredentials = credential.ToChannelCredentials()
                    };

                    speechClient = builder.Build();

                    // Update the status label to show the selected file path
                    key_selection_label.Text = selectedFilePath;
                }
            }
        }


        private void btnPath_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "Select Output Folder";
                folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    // Set the selected folder as the output folder path
                    outputFolderPath = folderBrowserDialog.SelectedPath;
                    path_selection_label.Text = outputFolderPath;
                }
            }
        }






    }
}
