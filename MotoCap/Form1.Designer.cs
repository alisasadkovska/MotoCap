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
            btnSelectJson = new Button();
            key_selection_label = new Label();
            btnPath = new Button();
            path_selection_label = new Label();
            SuspendLayout();
            // 
            // btnRecord
            // 
            btnRecord.Location = new Point(12, 27);
            btnRecord.Name = "btnRecord";
            btnRecord.Size = new Size(75, 23);
            btnRecord.TabIndex = 0;
            btnRecord.Text = "Record";
            btnRecord.UseVisualStyleBackColor = true;
            btnRecord.Click += btnRecord_Click;
            // 
            // btnStop
            // 
            btnStop.Enabled = false;
            btnStop.Location = new Point(93, 27);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(75, 23);
            btnStop.TabIndex = 1;
            btnStop.Text = "Stop";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // record_status_label
            // 
            record_status_label.AutoSize = true;
            record_status_label.Location = new Point(12, 9);
            record_status_label.Name = "record_status_label";
            record_status_label.Size = new Size(92, 15);
            record_status_label.TabIndex = 2;
            record_status_label.Text = "recording status";
            // 
            // btnSelectJson
            // 
            btnSelectJson.Location = new Point(12, 56);
            btnSelectJson.Name = "btnSelectJson";
            btnSelectJson.Size = new Size(75, 23);
            btnSelectJson.TabIndex = 3;
            btnSelectJson.Text = "Select key";
            btnSelectJson.UseVisualStyleBackColor = true;
            btnSelectJson.Click += btnSelectJson_Click;
            // 
            // key_selection_label
            // 
            key_selection_label.AutoSize = true;
            key_selection_label.Location = new Point(12, 82);
            key_selection_label.Name = "key_selection_label";
            key_selection_label.Size = new Size(106, 15);
            key_selection_label.TabIndex = 4;
            key_selection_label.Text = "key is not selected!";
            // 
            // btnPath
            // 
            btnPath.Location = new Point(12, 100);
            btnPath.Name = "btnPath";
            btnPath.Size = new Size(75, 23);
            btnPath.TabIndex = 5;
            btnPath.Text = "Path";
            btnPath.UseVisualStyleBackColor = true;
            btnPath.Click += btnPath_Click;
            // 
            // path_selection_label
            // 
            path_selection_label.AutoSize = true;
            path_selection_label.Location = new Point(12, 126);
            path_selection_label.Name = "path_selection_label";
            path_selection_label.Size = new Size(112, 15);
            path_selection_label.TabIndex = 6;
            path_selection_label.Text = "path is not selected!";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(457, 263);
            Controls.Add(path_selection_label);
            Controls.Add(btnPath);
            Controls.Add(key_selection_label);
            Controls.Add(btnSelectJson);
            Controls.Add(record_status_label);
            Controls.Add(btnStop);
            Controls.Add(btnRecord);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "MotoCapture";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnRecord;
        private Button btnStop;
        private Label record_status_label;
        private Button btnSelectJson;
        private Label key_selection_label;
        private Button btnPath;
        private Label path_selection_label;
    }
}