namespace App
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
            openFileDialog1 = new OpenFileDialog();
            ImportProfile = new Button();
            dataGridView1 = new DataGridView();
            Profile = new DataGridViewTextBoxColumn();
            QuickSwitchEnabled = new DataGridViewTextBoxColumn();
            Programs = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // ImportProfile
            // 
            ImportProfile.Location = new Point(12, 12);
            ImportProfile.Name = "ImportProfile";
            ImportProfile.Size = new Size(98, 23);
            ImportProfile.TabIndex = 0;
            ImportProfile.Text = "Import profile";
            ImportProfile.UseVisualStyleBackColor = true;
            ImportProfile.Click += ButtonClicked;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Profile, QuickSwitchEnabled, Programs });
            dataGridView1.Location = new Point(12, 135);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(776, 303);
            dataGridView1.TabIndex = 1;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // Profile
            // 
            Profile.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Profile.HeaderText = "Profile";
            Profile.Name = "Profile";
            // 
            // QuickSwitchEnabled
            // 
            QuickSwitchEnabled.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            QuickSwitchEnabled.HeaderText = "Quick switch enabled";
            QuickSwitchEnabled.Name = "QuickSwitchEnabled";
            QuickSwitchEnabled.Width = 133;
            // 
            // Programs
            // 
            Programs.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            Programs.HeaderText = "Programs";
            Programs.Name = "Programs";
            Programs.Width = 83;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(dataGridView1);
            Controls.Add(ImportProfile);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private OpenFileDialog openFileDialog1;
        private Button ImportProfile;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn Profile;
        private DataGridViewTextBoxColumn QuickSwitchEnabled;
        private DataGridViewTextBoxColumn Programs;
    }
}
