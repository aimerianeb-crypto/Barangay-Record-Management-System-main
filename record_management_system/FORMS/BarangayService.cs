using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace record_management_system.FORMS
{
    public partial class BarangayService : Form
    {

        PrintDocument printDoc = new PrintDocument();
        PrintPreviewDialog previewDlg = new PrintPreviewDialog();
        string connectionString = ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString;

        // Variable para mailhan kung unsa nga docs ang i-print
        string serviceType = "";

        public BarangayService()
        {
            InitializeComponent();
            printDoc.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);
        }

       

        private void LoadResidents(string search = "")
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // GIDUGANGAN OG "WHERE delete_flag = 0"
                    // Kini aron sigurado nga ang aktibo ra nga residente ang mugawas sa Brgy Services
                    string query = "SELECT id, code, firstname, lastname, address, purok FROM client_list WHERE delete_flag = 0";

                    // Kon naay search, idugang ang filter gamit ang AND
                    if (!string.IsNullOrEmpty(search))
                    {
                        query += " AND (firstname LIKE @s OR lastname LIKE @s OR code LIKE @s)";
                    }

                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    adapter.SelectCommand.Parameters.AddWithValue("@s", "%" + search + "%");

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvResidentsServices.DataSource = dt;

                    // Opsyonal: I-hide ang ID column para limpyo ang grid
                    if (dgvResidentsServices.Columns.Contains("id"))
                    {
                        dgvResidentsServices.Columns["id"].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void dgvResidentsServices_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnClearance_Click(object sender, EventArgs e)
        {
            if (dgvResidentsServices.SelectedRows.Count > 0)
            {
                // KINI ANG NAWALA: Kinahanglan i-set ang serviceType 
                // para makahibalo ang PrintPage unsay i-sulat
                serviceType = "CLEARANCE";

                previewDlg.Document = printDoc;
                previewDlg.WindowState = FormWindowState.Maximized;
                previewDlg.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please choose a resident on the lists.", "System", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadResidents(txtSearch.Text.Trim());
        }

        private void BarangayService_Load(object sender, EventArgs e)
        {
            LoadResidents();
        }

        // 4. Ang hitsura sa i-print nga Barangay Clearance
        // --- PRINTING LOGIC ---

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (dgvResidentsServices.SelectedRows.Count > 0)
            {
                var row = dgvResidentsServices.SelectedRows[0];
                string fullname = row.Cells["firstname"].Value.ToString() + " " + row.Cells["lastname"].Value.ToString();
                string address = row.Cells["address"].Value.ToString() + ", " + row.Cells["purok"].Value.ToString();

                Graphics g = e.Graphics;
                Font titleFont = new Font("Arial", 20, FontStyle.Bold);
                Font headerFont = new Font("Arial", 12, FontStyle.Regular);
                Font boldBody = new Font("Arial", 12, FontStyle.Bold);
                Font contentFont = new Font("Arial", 12, FontStyle.Regular);

                int y = 100;
                // Header (Common for all documents)
                g.DrawString("REPUBLIC OF THE PHILIPPINES", headerFont, Brushes.Black, 300, y); y += 25;
                g.DrawString("PROVINCE OF SOUTHERN LEYTE", headerFont, Brushes.Black, 310, y); y += 25;
                g.DrawString("MUNICIPALITY OF SAINT BERNARD", headerFont, Brushes.Black, 290, y); y += 25;
                g.DrawString("BARANGAY HIMOS-ONAN", boldBody, Brushes.Black, 340, y); y += 60;

                g.DrawString("OFFICE OF THE BARANGAY CAPTAIN", titleFont, Brushes.Black, 150, y); y += 80;

                // Dinhi mag-usab ang Title ug Content base sa serviceType
                string docTitle = "";
                string content = "";

                if (serviceType == "CLEARANCE")
                {
                    docTitle = "BARANGAY CLEARANCE";
                    content = $"TO WHOM IT MAY CONCERN:\n\n" +
                              $"This is to certify that {fullname.ToUpper()}, of legal age, " +
                              $"is a bona fide resident of {address.ToUpper()}.\n\n" +
                              $"This certification is being issued upon the request of the above-named person " +
                              $"for whatever legal purpose it may serve.\n\n" +
                              $"Issued this {DateTime.Now.ToString("dd")} day of {DateTime.Now.ToString("MMMM, yyyy")}.";
                }
                else if (serviceType == "INDIGENCY")
                {
                    docTitle = "CERTIFICATE OF INDIGENCY";
                    content = $"TO WHOM IT MAY CONCERN:\n\n" +
                              $"This is to certify that {fullname.ToUpper()}, of legal age, resident of {address.ToUpper()}, " +
                              $"is one of the INDIGENTS/LOW-INCOME earners in this Barangay.\n\n" +
                              $"This certification is being issued upon the request of the above-named person " +
                              $"to support his/her application for MEDICAL/FINANCIAL ASSISTANCE or whatever legal purpose it may serve.\n\n" +
                              $"Issued this {DateTime.Now.ToString("dd")} day of {DateTime.Now.ToString("MMMM, yyyy")}.";
                }

                else if (serviceType == "RESIDENCY")
                {
                    docTitle = "CERTIFICATE OF RESIDENCY";
                    content = $"TO WHOM IT MAY CONCERN:\n\n" +
                              $"This is to certify that {fullname.ToUpper()}, of legal age, " +
                              $"is a PERMANENT RESIDENT of {address.ToUpper()}, Saint Bernard, Southern Leyte.\n\n" +
                              $"Based on our records, he/she has been residing in this barangay for several years and " +
                              $"is known to be of good moral character.\n\n" +
                              $"Issued this {DateTime.Now.ToString("dd")} day of {DateTime.Now.ToString("MMMM, yyyy")}.";
                }

                // Draw Title
                g.DrawString(docTitle, new Font("Arial", 22, FontStyle.Bold | FontStyle.Underline), Brushes.Black, 200, y); y += 100;

                // Draw Body Content
                g.DrawString(content, contentFont, Brushes.Black, new RectangleF(100, y, 650, 400));

                // Footer / Signatures
                y += 350;
                g.DrawString("__________________________", boldBody, Brushes.Black, 500, y); y += 20;
                g.DrawString("BARANGAY CAPTAIN", boldBody, Brushes.Black, 530, y);
            }
        }

        private void btnIndigency_Click(object sender, EventArgs e)
        {
            if (dgvResidentsServices.SelectedRows.Count > 0)
            {
                serviceType = "INDIGENCY"; // I-set ang mode
                ShowPreview();
            }
            else
            {
                MessageBox.Show("Please choose a resident on the lists.", "System", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ShowPreview()
        {
            previewDlg.Document = printDoc;
            previewDlg.WindowState = FormWindowState.Maximized;
            previewDlg.ShowDialog();
        }

        private void btnResidency_Click(object sender, EventArgs e)
        {
            if (dgvResidentsServices.SelectedRows.Count > 0)
            {
                serviceType = "RESIDENCY"; // Mao ni ang atong bag-ong mode
                ShowPreview();
            }
            else
            {
                MessageBox.Show("Please choose a resident on the lists.", "System", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}


