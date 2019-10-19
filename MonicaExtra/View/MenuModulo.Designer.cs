namespace MonicaExtra.View
{
    partial class MenuModulo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MenuModulo));
            this.btnMenuControlCajaChica = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnMenuControlCajaChica
            // 
            this.btnMenuControlCajaChica.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMenuControlCajaChica.Image = ((System.Drawing.Image)(resources.GetObject("btnMenuControlCajaChica.Image")));
            this.btnMenuControlCajaChica.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMenuControlCajaChica.Location = new System.Drawing.Point(0, 0);
            this.btnMenuControlCajaChica.Name = "btnMenuControlCajaChica";
            this.btnMenuControlCajaChica.Size = new System.Drawing.Size(225, 81);
            this.btnMenuControlCajaChica.TabIndex = 0;
            this.btnMenuControlCajaChica.Text = "Control de Caja Chica";
            this.btnMenuControlCajaChica.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnMenuControlCajaChica.UseVisualStyleBackColor = true;
            // 
            // MenuModulo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 593);
            this.Controls.Add(this.btnMenuControlCajaChica);
            this.Name = "MenuModulo";
            this.Text = "MenuModulo";
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Button btnMenuControlCajaChica;
    }
}