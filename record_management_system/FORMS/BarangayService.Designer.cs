namespace record_management_system.FORMS
{
    partial class BarangayService
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.dgvResidentsServices = new System.Windows.Forms.DataGridView();
            this.pnlServices = new System.Windows.Forms.Panel();
            this.btnIndigency = new System.Windows.Forms.Button();
            this.btnResidency = new System.Windows.Forms.Button();
            this.btnClearance = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResidentsServices)).BeginInit();
            this.pnlServices.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Search";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(70, 76);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(139, 20);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // dgvResidentsServices
            // 
            this.dgvResidentsServices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvResidentsServices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResidentsServices.Location = new System.Drawing.Point(82, 114);
            this.dgvResidentsServices.Name = "dgvResidentsServices";
            this.dgvResidentsServices.Size = new System.Drawing.Size(461, 224);
            this.dgvResidentsServices.TabIndex = 2;
            this.dgvResidentsServices.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvResidentsServices_CellContentClick);
            // 
            // pnlServices
            // 
            this.pnlServices.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlServices.BackColor = System.Drawing.Color.White;
            this.pnlServices.Controls.Add(this.btnClearance);
            this.pnlServices.Controls.Add(this.btnResidency);
            this.pnlServices.Controls.Add(this.btnIndigency);
            this.pnlServices.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.pnlServices.ForeColor = System.Drawing.Color.White;
            this.pnlServices.Location = new System.Drawing.Point(566, 129);
            this.pnlServices.Name = "pnlServices";
            this.pnlServices.Size = new System.Drawing.Size(222, 195);
            this.pnlServices.TabIndex = 3;
            // 
            // btnIndigency
            // 
            this.btnIndigency.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(88)))));
            this.btnIndigency.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIndigency.Location = new System.Drawing.Point(0, 66);
            this.btnIndigency.Name = "btnIndigency";
            this.btnIndigency.Size = new System.Drawing.Size(219, 61);
            this.btnIndigency.TabIndex = 1;
            this.btnIndigency.Text = "Certificate of Indigency";
            this.btnIndigency.UseVisualStyleBackColor = false;
            this.btnIndigency.Click += new System.EventHandler(this.btnIndigency_Click);
            // 
            // btnResidency
            // 
            this.btnResidency.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(88)))));
            this.btnResidency.Location = new System.Drawing.Point(0, 133);
            this.btnResidency.Name = "btnResidency";
            this.btnResidency.Size = new System.Drawing.Size(219, 59);
            this.btnResidency.TabIndex = 2;
            this.btnResidency.Text = "Certificate of Residency";
            this.btnResidency.UseVisualStyleBackColor = false;
            this.btnResidency.Click += new System.EventHandler(this.btnResidency_Click);
            // 
            // btnClearance
            // 
            this.btnClearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(88)))));
            this.btnClearance.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnClearance.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearance.Location = new System.Drawing.Point(3, 3);
            this.btnClearance.Name = "btnClearance";
            this.btnClearance.Size = new System.Drawing.Size(216, 61);
            this.btnClearance.TabIndex = 3;
            this.btnClearance.Text = "Barangay Clearance";
            this.btnClearance.UseVisualStyleBackColor = false;
            this.btnClearance.Click += new System.EventHandler(this.btnClearance_Click);
            // 
            // BarangayService
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pnlServices);
            this.Controls.Add(this.dgvResidentsServices);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.label1);
            this.Name = "BarangayService";
            this.Text = "BarangayService";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.BarangayService_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvResidentsServices)).EndInit();
            this.pnlServices.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.DataGridView dgvResidentsServices;
        private System.Windows.Forms.Panel pnlServices;
        private System.Windows.Forms.Button btnResidency;
        private System.Windows.Forms.Button btnIndigency;
        private System.Windows.Forms.Button btnClearance;
    }
}