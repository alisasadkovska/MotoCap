namespace MotoCap
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            btnRecord = new Button();
            btnStop = new Button();
            record_status_label = new Label();
            btnPath = new Button();
            path_selection_label = new Label();
            trackBar1 = new TrackBar();
            trackBar2 = new TrackBar();
            trackBar3 = new TrackBar();
            richTextBox1 = new RichTextBox();
            btn_Key = new Button();
            key_selection_label = new Label();
            voice_status_label = new Label();
            btnWavPath = new Button();
            wav_path_label = new Label();
            btn_convert_speech = new Button();
            ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar3).BeginInit();
            SuspendLayout();
            // 
            // btnRecord
            // 
            btnRecord.Location = new Point(17, 45);
            btnRecord.Margin = new Padding(4, 5, 4, 5);
            btnRecord.Name = "btnRecord";
            btnRecord.Size = new Size(107, 38);
            btnRecord.TabIndex = 0;
            btnRecord.Text = "Record";
            btnRecord.UseVisualStyleBackColor = true;
            btnRecord.Click += btnRecord_Click;
            // 
            // btnStop
            // 
            btnStop.Enabled = false;
            btnStop.Location = new Point(133, 45);
            btnStop.Margin = new Padding(4, 5, 4, 5);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(107, 38);
            btnStop.TabIndex = 1;
            btnStop.Text = "Stop";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // record_status_label
            // 
            record_status_label.AutoSize = true;
            record_status_label.Location = new Point(17, 15);
            record_status_label.Margin = new Padding(4, 0, 4, 0);
            record_status_label.Name = "record_status_label";
            record_status_label.Size = new Size(140, 25);
            record_status_label.TabIndex = 2;
            record_status_label.Text = "recording status";
            // 
            // btnPath
            // 
            btnPath.Location = new Point(17, 167);
            btnPath.Margin = new Padding(4, 5, 4, 5);
            btnPath.Name = "btnPath";
            btnPath.Size = new Size(107, 38);
            btnPath.TabIndex = 5;
            btnPath.Text = "Path";
            btnPath.UseVisualStyleBackColor = true;
            btnPath.Click += btnPath_Click;
            // 
            // path_selection_label
            // 
            path_selection_label.AutoSize = true;
            path_selection_label.Location = new Point(17, 210);
            path_selection_label.Margin = new Padding(4, 0, 4, 0);
            path_selection_label.Name = "path_selection_label";
            path_selection_label.Size = new Size(171, 25);
            path_selection_label.TabIndex = 6;
            path_selection_label.Text = "path is not selected!";
            // 
            // trackBar1
            // 
            trackBar1.Location = new Point(485, 15);
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new Size(156, 69);
            trackBar1.TabIndex = 7;
            // 
            // trackBar2
            // 
            trackBar2.Location = new Point(485, 90);
            trackBar2.Name = "trackBar2";
            trackBar2.Size = new Size(156, 69);
            trackBar2.TabIndex = 8;
            // 
            // trackBar3
            // 
            trackBar3.Location = new Point(485, 166);
            trackBar3.Name = "trackBar3";
            trackBar3.Size = new Size(156, 69);
            trackBar3.TabIndex = 9;
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(12, 238);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(629, 188);
            richTextBox1.TabIndex = 11;
            richTextBox1.Text = "";
            // 
            // btn_Key
            // 
            btn_Key.Location = new Point(17, 91);
            btn_Key.Name = "btn_Key";
            btn_Key.Size = new Size(107, 38);
            btn_Key.TabIndex = 12;
            btn_Key.Text = "Key";
            btn_Key.UseVisualStyleBackColor = true;
            btn_Key.Click += btnSelectJson_Click;
            // 
            // key_selection_label
            // 
            key_selection_label.AutoSize = true;
            key_selection_label.Location = new Point(17, 128);
            key_selection_label.Name = "key_selection_label";
            key_selection_label.Size = new Size(204, 25);
            key_selection_label.TabIndex = 13;
            key_selection_label.Text = ".json key is not selected!";
            // 
            // voice_status_label
            // 
            voice_status_label.AutoSize = true;
            voice_status_label.Location = new Point(181, 15);
            voice_status_label.Name = "voice_status_label";
            voice_status_label.Size = new Size(0, 25);
            voice_status_label.TabIndex = 14;
            // 
            // btnWavPath
            // 
            btnWavPath.Location = new Point(307, 167);
            btnWavPath.Name = "btnWavPath";
            btnWavPath.Size = new Size(107, 38);
            btnWavPath.TabIndex = 15;
            btnWavPath.Text = "WAV path";
            btnWavPath.UseVisualStyleBackColor = true;
            btnWavPath.Click += btnWavPath_Click;
            // 
            // wav_path_label
            // 
            wav_path_label.AutoSize = true;
            wav_path_label.Location = new Point(307, 208);
            wav_path_label.Name = "wav_path_label";
            wav_path_label.Size = new Size(202, 25);
            wav_path_label.TabIndex = 16;
            wav_path_label.Text = "wav path is not selected";
            // 
            // btn_convert_speech
            // 
            btn_convert_speech.Location = new Point(307, 119);
            btn_convert_speech.Name = "btn_convert_speech";
            btn_convert_speech.Size = new Size(107, 38);
            btn_convert_speech.TabIndex = 17;
            btn_convert_speech.Text = "Convert";
            btn_convert_speech.UseVisualStyleBackColor = true;
            btn_convert_speech.Click += btn_convert_speech_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(653, 438);
            Controls.Add(btn_convert_speech);
            Controls.Add(wav_path_label);
            Controls.Add(btnWavPath);
            Controls.Add(voice_status_label);
            Controls.Add(key_selection_label);
            Controls.Add(btn_Key);
            Controls.Add(richTextBox1);
            Controls.Add(trackBar3);
            Controls.Add(trackBar2);
            Controls.Add(trackBar1);
            Controls.Add(path_selection_label);
            Controls.Add(btnPath);
            Controls.Add(record_status_label);
            Controls.Add(btnStop);
            Controls.Add(btnRecord);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 5, 4, 5);
            Name = "Form1";
            Text = "MotoCapture";
            ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar2).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar3).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnRecord;
        private Button btnStop;
        private Label record_status_label;
        private Button btnPath;
        private Label path_selection_label;
        private TrackBar trackBar1;
        private TrackBar trackBar2;
        private TrackBar trackBar3;
        private RichTextBox richTextBox1;
        private Button btn_Key;
        private Label key_selection_label;
        private Label voice_status_label;
        private Button btnWavPath;
        private Label wav_path_label;
        private Button btn_convert_speech;
    }
}