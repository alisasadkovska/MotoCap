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
        private Grpc.Core.Channel channel;
        private Equalizer equalizer;
        private readonly WebRtcVad vad;
        private bool isVoiceDetected;

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

            // Initialize the Voice Activity Detector
            vad = new WebRtcVad
            {
                SampleRate = SampleRate.Is48kHz // Adjust the sample rate to match your audio input
            };

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

                    waveInEvent = new WaveInEvent
                    {
                        WaveFormat = new WaveFormat(44100, 16, 1) // Update sample rate and bit depth here
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

                // Perform voice activity detection
                var buffer = new byte[e.BytesRecorded];
                Buffer.BlockCopy(e.Buffer, 0, buffer, 0, e.BytesRecorded);

                // Calculate the root mean square (RMS) value of the buffer
                double rms = CalculateRMS(buffer);

                // Adjust the threshold value as needed
                double threshold = 0.5; // Example threshold value

                // Determine if the buffer contains voice or silence based on the threshold
                var isSpeech = rms > threshold;

                if (isSpeech)
                {
                    isVoiceDetected = true;
                    voice_status_label.Text = "voice detected";
                }
                else
                {
                    isVoiceDetected = false;
                    voice_status_label.Text = "silence";
                }

              
                // Write the recorded audio data to the output file
                writer.Write(e.Buffer, 0, e.BytesRecorded);


            }
        }


        private double CalculateRMS(byte[] buffer)
        {
            double sum = 0;

            for (int i = 0; i < buffer.Length; i += 2)
            {
                short sample = (short)((buffer[i + 1] << 8) | buffer[i]);
                sum += sample * sample;
            }

            double rms = Math.Sqrt(sum / (buffer.Length / 2));
            return rms;
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

                // Save recorded audio file
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string outputFileName = $"Recording_{timestamp}.wav";
                string outputFilePath = Path.Combine(outputFolderPath, outputFileName);
                File.Move(writer.Filename, outputFilePath);
                ConvertAudioToSpeech(outputFilePath);

                // Perform speech-to-text conversion
                //  ConvertSpeechToText();
            }
        }

        private void ConvertAudioToSpeech(string outputFilePath)
        {
            if (File.Exists(outputFilePath))
            {
                //var speech = SpeechClient.Create();
                var response = speechClient.Recognize(new RecognitionConfig()
                {
                    Encoding = RecognitionConfig.Types.AudioEncoding.Linear16,
                    SampleRateHertz = 44100,
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
                    channel = new Grpc.Core.Channel(SpeechClient.DefaultEndpoint.ToString(), credential.ToChannelCredentials());

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
