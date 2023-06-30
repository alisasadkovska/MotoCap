using System;
using System.IO;
using System.Threading.Channels;
using System.Windows.Forms;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Speech.V1;
using Grpc.Auth;
using Grpc.Core;
using NAudio.Wave;



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



        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //set initial output path
            outputFolderPath = "C:/MotoRecOutput/";
        }

        private void btnRecord_Click(object sender, EventArgs e)
        {
            if (channel != null)
            {
                if (!isRecording)
                {
                    // Update UI
                    btnRecord.Enabled = false;
                    btnStop.Enabled = true;

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
                MessageBox.Show("Please select a JSON credentials file first.");
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

        private void btnStop_Click(object sender, EventArgs e)
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

                // Save recorded audio file
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string outputFileName = $"Recording_{timestamp}.wav";
                string outputFilePath = Path.Combine(outputFolderPath, outputFileName);
                File.Move(writer.Filename, outputFilePath);

                // Perform speech-to-text conversion
                //  ConvertSpeechToText();
            }
        }

        private void ConvertSpeechToText()
        {
            throw new NotImplementedException();
        }

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


    }
}
