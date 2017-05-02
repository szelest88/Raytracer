namespace Raytracer
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.renderButton = new System.Windows.Forms.Button();
            this.renderAntialiased = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 22);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(948, 831);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.PictureBox1_Click);
            // 
            // renderButton
            // 
            this.renderButton.Location = new System.Drawing.Point(1011, 203);
            this.renderButton.Name = "renderButton";
            this.renderButton.Size = new System.Drawing.Size(75, 23);
            this.renderButton.TabIndex = 1;
            this.renderButton.Text = "button1";
            this.renderButton.UseVisualStyleBackColor = true;
            this.renderButton.Click += new System.EventHandler(this.RenderButton_Click);
            // 
            // renderAntialiased
            // 
            this.renderAntialiased.Location = new System.Drawing.Point(1011, 271);
            this.renderAntialiased.Name = "renderAntialiased";
            this.renderAntialiased.Size = new System.Drawing.Size(75, 23);
            this.renderAntialiased.TabIndex = 2;
            this.renderAntialiased.Text = "button1";
            this.renderAntialiased.UseVisualStyleBackColor = true;
            this.renderAntialiased.Click += new System.EventHandler(this.RenderAntialiased_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1169, 919);
            this.Controls.Add(this.renderAntialiased);
            this.Controls.Add(this.renderButton);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button renderButton;
        private System.Windows.Forms.Button renderAntialiased;
    }
}

