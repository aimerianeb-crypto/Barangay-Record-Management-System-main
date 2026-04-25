using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Drawing.Printing;

namespace record_management_system.FORMS
{
    public partial class WaterLinkage : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString;
        PrintDocument printReceipt = new PrintDocument();
        PrintPreviewDialog previewDialog = new PrintPreviewDialog();

        public WaterLinkage()
        {
            InitializeComponent();
            printReceipt.PrintPage += new PrintPageEventHandler(PrintReceipt_PrintPage);
        }

        private void LoadWaterLinkageData(string searchTerm = "")
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // SQL Query FIX: 
                    // Gikuha na nato ang original values gikan sa database para dili mag-zero ang display.
                    string query = @"SELECT 
                                CONCAT(c.firstname, ' ', c.lastname) AS 'CLIENT NAME', 
                                b.meter_code AS 'METER', 
                                CONCAT(b.previous, ' -> ', b.reading) AS 'READING (M³)', 
                                ((b.reading - b.previous) * b.rate) AS 'CURRENT',
                                (b.total - ((b.reading - b.previous) * b.rate)) AS 'ARREARS',
                                b.total AS 'GRAND TOTAL',
                                CASE WHEN b.status = 1 THEN 'PAID' ELSE 'PENDING' END AS 'STATUS'
                             FROM wbms_db.billing_list b
                             INNER JOIN wbms_db.client_list c ON b.client_id = c.id";

                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        query += " WHERE (c.firstname LIKE @search OR c.lastname LIKE @search OR b.meter_code LIKE @search)";
                    }

                    query += " ORDER BY b.id DESC";

                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@search", "%" + searchTerm + "%");
                    }

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvLinkage.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupDataGridView()
        {
            if (dgvLinkage.Columns.Count > 0)
            {
                dgvLinkage.ReadOnly = true;
                dgvLinkage.AllowUserToAddRows = false;
                dgvLinkage.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvLinkage.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                var phCulture = new System.Globalization.CultureInfo("en-PH");
                string[] currencyColumns = { "CURRENT", "ARREARS", "GRAND TOTAL" };

                foreach (string col in currencyColumns)
                {
                    if (dgvLinkage.Columns.Contains(col))
                    {
                        dgvLinkage.Columns[col].DefaultCellStyle.Format = "C2";
                        dgvLinkage.Columns[col].DefaultCellStyle.FormatProvider = phCulture;
                    }
                }
            }
        }

        private void WaterLinkage_Load_1(object sender, EventArgs e)
        {
            LoadWaterLinkageData();
            SetupDataGridView();
        }

        private void txtSearch_TextChanged_1(object sender, EventArgs e)
        {
            LoadWaterLinkageData(txtSearch.Text.Trim());
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dgvLinkage.SelectedRows.Count > 0)
            {
                previewDialog.Document = printReceipt;
                previewDialog.WindowState = FormWindowState.Maximized;
                previewDialog.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a specific resident from the list.", "System", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void PrintReceipt_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (dgvLinkage.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvLinkage.SelectedRows[0];
                Font titleFont = new Font("Arial", 18, FontStyle.Bold);
                Font headerFont = new Font("Arial", 12, FontStyle.Bold);
                Font bodyFont = new Font("Arial", 12, FontStyle.Regular);
                int y = 80;
                int leftMargin = 50;

                e.Graphics.DrawString("BARANGAY WATER BILLING", titleFont, Brushes.Black, leftMargin, y);
                y += 40;
                e.Graphics.DrawString("Official Billing Statement", bodyFont, Brushes.Black, leftMargin, y);
                y += 30;
                e.Graphics.DrawString("----------------------------------------------------------", bodyFont, Brushes.Black, leftMargin, y);
                y += 40;

                e.Graphics.DrawString("CLIENT NAME:  " + row.Cells["CLIENT NAME"].Value.ToString(), headerFont, Brushes.Black, leftMargin, y);
                y += 30;
                e.Graphics.DrawString("METER NO:     " + row.Cells["METER"].Value.ToString(), bodyFont, Brushes.Black, leftMargin, y);
                y += 30;
                e.Graphics.DrawString("READING:      " + row.Cells["READING (M³)"].Value.ToString(), bodyFont, Brushes.Black, leftMargin, y);
                y += 40;

                decimal current = Convert.ToDecimal(row.Cells["CURRENT"].Value);
                decimal arrears = Convert.ToDecimal(row.Cells["ARREARS"].Value);
                decimal total = Convert.ToDecimal(row.Cells["GRAND TOTAL"].Value);

                e.Graphics.DrawString("CURRENT BILL: ₱" + current.ToString("N2"), bodyFont, Brushes.Black, leftMargin, y);
                y += 30;
                e.Graphics.DrawString("ARREARS:      ₱" + arrears.ToString("N2"), bodyFont, Brushes.Black, leftMargin, y);
                y += 40;

                e.Graphics.DrawString("GRAND TOTAL:  ₱" + total.ToString("N2"), titleFont, Brushes.Black, leftMargin, y);
                y += 50;

                e.Graphics.DrawString("STATUS:       " + row.Cells["STATUS"].Value.ToString(), headerFont, Brushes.Black, leftMargin, y);
                y += 60;

                e.Graphics.DrawString("----------------------------------------------------------", bodyFont, Brushes.Black, leftMargin, y);
                y += 30;
                e.Graphics.DrawString("Date Generated: " + DateTime.Now.ToString("MMMM dd, yyyy"), bodyFont, Brushes.Gray, leftMargin, y);
            }
        }
    }
}