using System;
using System.IO;
using System.Net;
using System.Threading.Channels;
using System.Windows.Forms;
using System.Speech.Recognition;
using Grpc.Auth;
using Grpc.Core;
using NAudio.CoreAudioApi;
using NAudio.Extras;
using NAudio.Wave;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace MotoCap
{
    public partial class Form1 : Form
    {

        private WaveInEvent waveInEvent;
        private WaveFileWriter writer;
        private bool isRecording;
        //private SpeechClient speechClient;
        private SpeechRecognitionEngine recognizer;
        private string outputFolderPath;
        private Grpc.Core.Channel channel;
        private Equalizer equalizer;

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
            InitializeRecognizer();
        }

        private void InitializeRecognizer()
        {
            recognizer = new SpeechRecognitionEngine();

            recognizer.SetInputToDefaultAudioDevice();

            // Set the event handler for speech recognition
            recognizer.SpeechRecognized += Recognizer_SpeechRecognized;

            // Enable continuous recognition
            recognizer.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            richTextBox1.Text = e.Result.Text;
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

                    recognizer.RecognizeAsync(RecognizeMode.Multiple);

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
                // Write the recorded audio data to the output file
                writer.Write(e.Buffer, 0, e.BytesRecorded);
            }
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

                // Perform speech-to-text conversion
                //  ConvertSpeechToText();
            }
        }

        /*
        private async Task SpeechRecognition(string filepath)
        {
            var speechClient = SpeechClient.Create();
            var response = await speechClient.RecognizeAsync(new RecognitionConfig
            {
                Encoding = RecognitionConfig.Types.AudioEncoding.Linear16,
                SampleRateHertz = 16000,
                LanguageCode = LanguageCodes.English.UnitedStates
            }, RecognitionAudio.FromFile(filepath));

            foreach (var result in response.Results)
            {
                foreach (var alternative in result.Alternatives)
                {
                    string speech = alternative.Transcript;
                    // Speech recognized, do something with the recognized speech
                    // For example, display the speech in a textbox
                    richTextBox1.Text = speech;
                }
            }
        }*/


        /*
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
                    GoogleCredential credential = GoogleCredential.FromFile(selectedFilePath);
                    channel = new Grpc.Core.Channel(SpeechClient.DefaultEndpoint.ToString(),
                        credential.ToChannelCredentials());

                    // Update the status label to show the selected file path
                    key_selection_label.Text = selectedFilePath;
                }
            }
        }*/

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



        /*
        private GoogleCredential GetGoogleCredentialsFromChannel(Grpc.Core.Channel channel)
        {
            var credentialKey = channel.Target.Replace(':', '_').Replace('/', '_');
            var credentialsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".credentials", $"{credentialKey}.json");

            if (File.Exists(credentialsPath))
            {
                try
                {
                    var json = File.ReadAllText(credentialsPath);
                    return GoogleCredential.FromJson(json);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to read credentials from file: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Credentials file not found.");
            }

            return null;
        }
        */

    }
}
