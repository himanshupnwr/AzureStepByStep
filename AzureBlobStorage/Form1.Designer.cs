namespace AzureBlobStorage
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnBlockBlob = new System.Windows.Forms.Button();
            this.btnAppendblob = new System.Windows.Forms.Button();
            this.btnPageBlob = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnBlockBlob
            // 
            this.btnBlockBlob.Location = new System.Drawing.Point(50, 47);
            this.btnBlockBlob.Name = "btnBlockBlob";
            this.btnBlockBlob.Size = new System.Drawing.Size(207, 104);
            this.btnBlockBlob.TabIndex = 0;
            this.btnBlockBlob.Text = "Block Blob";
            this.btnBlockBlob.UseVisualStyleBackColor = true;
            this.btnBlockBlob.Click += new System.EventHandler(this.btnBlockBlob_Click);
            // 
            // btnAppendblob
            // 
            this.btnAppendblob.Location = new System.Drawing.Point(316, 47);
            this.btnAppendblob.Name = "btnAppendblob";
            this.btnAppendblob.Size = new System.Drawing.Size(195, 109);
            this.btnAppendblob.TabIndex = 1;
            this.btnAppendblob.Text = "Append Blob";
            this.btnAppendblob.UseVisualStyleBackColor = true;
            this.btnAppendblob.Click += new System.EventHandler(this.btnAppendblob_Click);
            // 
            // btnPageBlob
            // 
            this.btnPageBlob.Location = new System.Drawing.Point(547, 47);
            this.btnPageBlob.Name = "btnPageBlob";
            this.btnPageBlob.Size = new System.Drawing.Size(207, 104);
            this.btnPageBlob.TabIndex = 2;
            this.btnPageBlob.Text = "Page Blob";
            this.btnPageBlob.UseVisualStyleBackColor = true;
            this.btnPageBlob.Click += new System.EventHandler(this.btnPageBlob_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1277, 834);
            this.Controls.Add(this.btnPageBlob);
            this.Controls.Add(this.btnAppendblob);
            this.Controls.Add(this.btnBlockBlob);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBlockBlob;
        private System.Windows.Forms.Button btnAppendblob;
        private System.Windows.Forms.Button btnPageBlob;
    }
}