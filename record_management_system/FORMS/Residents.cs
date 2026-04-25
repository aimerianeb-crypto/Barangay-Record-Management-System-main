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
using System.Net.Http; // I-add ni sa taas
using Newtonsoft.Json; // I-install ni via NuGet (Newtonsoft.Json)

namespace record_management_system.FORMS
{
    public partial class Residents : Form
    {
        

        public Residents()
        {
            InitializeComponent();
        }

        public async void LoadResidents()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // --- KINI ANG KINAHANGLAN NGA DUGANG ---
                    // Isulod ang token nga nakuha nimo pag-login
                    client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Login.authToken);

                    string url = "http://localhost:3000/api/clients";
                    string response = await client.GetStringAsync(url);

                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(response);
                    dgvResidents.DataSource = dt;

                    // ... (ang imong formatting logic)
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error connecting to API: " + ex.Message);
                }
            }
        }

        private async void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                LoadResidents();
                return;
            }

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // --- DUGANGI SAB NI OG AUTHORIZATION ---
                    client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Login.authToken);

                    string searchText = Uri.EscapeDataString(txtSearch.Text);
                    string url = "http://localhost:3000/api/clients/search/" + searchText;

                    string response = await client.GetStringAsync(url);
                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(response);
                    dgvResidents.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Search Error: " + ex.Message);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddResidents frm = new AddResidents();
            frm.SetMode("ADD");

            frm.TopLevel = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;

            if (this.Parent is Panel p)
            {
                p.Controls.Clear(); // Clear daan para dili magsapaw
                p.Controls.Add(frm);
                frm.BringToFront();
                frm.Show();

                if (this.FindForm() is dashboard mainDash)
                {
                    mainDash.activeform = frm;
                }
            }
        }

        private void Residents_Load(object sender, EventArgs e)
        {
            LoadResidents();
        }

        private void btnRefresh_Click_1(object sender, EventArgs e)
        {
            LoadResidents();
            MessageBox.Show("Data Refreshed!", "System", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dgvResidents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvResidents.Rows[e.RowIndex];

                AddResidents editForm = new AddResidents();
                editForm.SetMode("EDIT");

                // --- FIXED: Gidugangan og row.Cells["category_id"] sa pagpasa ---
                // Kinahanglan 11 na kabuok ang parameters dinhi
                editForm.FillData(
                    row.Cells["id"].Value.ToString(),            // 1
                    row.Cells["code"].Value.ToString(),          // 2
                    row.Cells["category_id"].Value.ToString(),   // 3. <--- KINI ANG GI-INSERT
                    row.Cells["firstname"].Value.ToString(),     // 4
                    row.Cells["lastname"].Value.ToString(),      // 5
                    row.Cells["middlename"].Value.ToString(),    // 6
                    row.Cells["gender"].Value.ToString(),        // 7
                    row.Cells["birthdate"].Value.ToString(),     // 8
                    row.Cells["contact"].Value.ToString(),       // 9
                    row.Cells["address"].Value.ToString(),       // 10
                    row.Cells["purok"].Value.ToString()          // 11 (Kini na ang ika-11)
                );

                editForm.TopLevel = false;
                editForm.FormBorderStyle = FormBorderStyle.None;
                editForm.Dock = DockStyle.Fill;

                if (this.Parent is Panel p)
                {
                    p.Controls.Clear();
                    p.Controls.Add(editForm);
                    editForm.Show();
                }
            }
        }
    }
}