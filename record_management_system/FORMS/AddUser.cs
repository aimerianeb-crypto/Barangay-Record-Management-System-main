using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace record_management_system.FORMS
{
    public partial class AddResidents : Form
    {
        string connStr = "server=localhost;database=wbms_db;uid=root;pwd=;";

        public AddResidents()
        {
            InitializeComponent();
        }

        private void LoadCategories()
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    // Gikuha ang tanang categories; gitangtang na ang delete_flag condition
                    string query = "SELECT id, name FROM category_list ORDER BY name ASC";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        cmbCategory.DataSource = dt;
                        cmbCategory.DisplayMember = "name";
                        cmbCategory.ValueMember = "id";
                        cmbCategory.SelectedIndex = -1;
                    }
                    else
                    {
                        MessageBox.Show("No categories found in the database.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading categories: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void GenerateAutoCode()
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT code FROM client_list ORDER BY id DESC LIMIT 1";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        long lastCode = long.Parse(result.ToString());
                        long newCode = lastCode + 1;
                        txtCode.Text = newCode.ToString();
                    }
                    else
                    {
                        txtCode.Text = "202205020001";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error generating code: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void AddResidents_Load(object sender, EventArgs e)
        {
            LoadCategories();
            txtAddress.Text = "Himos-onan, Saint Bernard, Southern Leyte";
            txtAddress.ReadOnly = true;

            if (btnUpdate.Visible == false && btnDelete.Visible == false)
            {
                btnSave.Visible = true;
                GenerateAutoCode();
            }

            txtCode.ReadOnly = true;
        }

        // SAVE VIA API (Node.js Integration)
        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbCategory.SelectedIndex == -1)
            {
                MessageBox.Show("Please choose a category!", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var clientData = new
            {
                code = txtCode.Text,
                category_id = int.Parse(cmbCategory.SelectedValue.ToString()),
                firstname = txtFirstName.Text,
                lastname = txtLastName.Text,
                middlename = txtMiddleName.Text,
                gender = cmbGender.Text,
                birthdate = dtpDate.Value.ToString("yyyy-MM-dd"),
                contact = txtContact.Text,
                address = txtAddress.Text,
                purok = cmbPurok.Text
            };

            // FIX: Ang 'using' block dapat magputos sa tanang code nga naggamit sa 'client'
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // 1. I-add ang Token Header
                    client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Login.authToken);

                    string url = "http://localhost:3000/api/clients/add";
                    string json = JsonConvert.SerializeObject(clientData);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // 2. I-send ang request (Naa na ni sa sulod sa using)
                    HttpResponseMessage response = await client.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Resident successfully added via API!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        GoBackToList();
                    }
                    else
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("API Error: " + error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Connection Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            GoBackToList();
        }

        private void GoBackToList()
        {
            Residents listForm = new Residents();
            listForm.TopLevel = false;
            listForm.FormBorderStyle = FormBorderStyle.None;
            listForm.Dock = DockStyle.Fill;

            Control container = this.Parent;
            while (container != null && !(container is Panel))
            {
                container = container.Parent;
            }

            if (container is Panel containerPanel)
            {
                containerPanel.Controls.Clear();
                containerPanel.Controls.Add(listForm);
                listForm.Show();
            }
            else
            {
                this.Close();
            }
        }

        public void SetMode(string mode)
        {
            if (mode == "ADD")
            {
                btnSave.Visible = true;
                btnUpdate.Visible = false;
                btnDelete.Visible = false;
                lblTitle.Text = "ADD RESIDENTS";
            }
            else if (mode == "EDIT")
            {
                btnSave.Visible = false;
                btnUpdate.Visible = true;
                btnDelete.Visible = true;
                lblTitle.Text = "EDIT RESIDENT PROFILE";
            }
        }

        // Gikuha na ang first_reading ug delete_flag sa FillData
        public void FillData(string id, string code, string catID, string fn, string ln, string mn, string gender, string bday, string contact, string addr, string purok)
        {
            this.Tag = id;
            txtCode.Text = code;

            if (!string.IsNullOrEmpty(catID))
            {
                cmbCategory.SelectedValue = catID;
            }

            txtFirstName.Text = fn;
            txtLastName.Text = ln;
            txtMiddleName.Text = mn;
            cmbGender.Text = gender;

            if (!string.IsNullOrEmpty(bday))
            {
                dtpDate.Value = DateTime.Parse(bday);
            }

            txtContact.Text = contact;
            txtAddress.Text = addr;
            cmbPurok.Text = purok;
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            // Validation
            if (cmbCategory.SelectedIndex == -1)
            {
                MessageBox.Show("Please choose a category!", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 1. I-prepare ang data (Dapat mag-match ang variable names sa imong Node.js req.body)
            var updateData = new
            {
                code = txtCode.Text,
                category_id = int.Parse(cmbCategory.SelectedValue.ToString()),
                firstname = txtFirstName.Text,
                lastname = txtLastName.Text,
                middlename = txtMiddleName.Text,
                gender = cmbGender.Text,
                birthdate = dtpDate.Value.ToString("yyyy-MM-dd"),
                contact = txtContact.Text,
                address = txtAddress.Text,
                purok = cmbPurok.Text
            };

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // 2. I-ADD ANG AUTH HEADER
                    // Kinahanglan ni kay naay 'verifyToken' ang imong route sa Node.js
                    client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Login.authToken);

                    // 3. I-SET ANG URL (Kinahanglan naay ID sa tumoy para sa /update/:id)
                    string url = "http://localhost:3000/api/clients/update/" + this.Tag;

                    // 4. I-serialize ang data ngadto sa JSON
                    string json = JsonConvert.SerializeObject(updateData);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // 5. I-send ang PUT request
                    HttpResponseMessage response = await client.PutAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Record updated successfully via API!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        GoBackToList();
                    }
                    else
                    {
                        // Makuha nato ang error message gikan sa server (pananglitan: 'Unauthorized')
                        string error = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("API Error: " + error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Connection Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            // 1. Siguraduhon nga naay napili nga record
            if (this.Tag == null || string.IsNullOrEmpty(this.Tag.ToString()))
            {
                MessageBox.Show("No record selected to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Confirmation gikan sa user
            var confirm = MessageBox.Show("Are you sure you want to delete this record permanently?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // 3. I-add ang Authorization Token (Importante para sa verifyToken middleware)
                        client.DefaultRequestHeaders.Authorization =
                            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Login.authToken);

                        // 4. API Endpoint (Dapat mag-match sa Node.js: /api/clients/delete/:id)
                        string url = "http://localhost:3000/api/clients/delete/" + this.Tag;

                        // 5. I-execute ang DELETE request
                        HttpResponseMessage response = await client.DeleteAsync(url);

                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show("Resident record removed via API!", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            GoBackToList();
                        }
                        else
                        {
                            // Kon naay error (pananglitan 401/403), basahon ang response gikan sa Node.js
                            string error = await response.Content.ReadAsStringAsync();
                            MessageBox.Show("API Error: " + error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Connection Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e) { }
        private void txtAddress_TextChanged(object sender, EventArgs e) { }
    }
}