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
using System.Net.Http; 
using Newtonsoft.Json; 

namespace record_management_system
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        public static void loadform(object Form)
        {
            Form load = Form as Form;
            load.Show();
        }

        // ---  GLOBAL VARIABLES ---
        public static string authToken; //  token
        public static int userid;
        public static string userole, username;
        public static string rolecheck;

        //  connectstring direct MySQL 
        public static string connectstring = "server=localhost;port=3306;username=root;password=;database=wbms_db";

        // Gihimong 'async' para dili mo-freeze ang UI
        public async void login()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var loginData = new
                    {
                        username = txtbxusername.Text,
                        password = txtbxpass.Text
                    };

                    string json = JsonConvert.SerializeObject(loginData);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    string url = "http://localhost:3000/api/login";

                    HttpResponseMessage response = await client.PostAsync(url, content);
                    string responseString = await response.Content.ReadAsStringAsync();

                    // I-convert ang JSON gikan sa Node.js
                    var result = JsonConvert.DeserializeObject<dynamic>(responseString);

                    if (result.status == "success")
                    {
                        // 1. I-save ang Token (Kinahanglan ni sa Residents.cs unya)
                        authToken = (string)result.token;

                        // 2. I-save ang user info (Dili na ni mo-error kay naa nay 'user' sa JSON)
                        userid = (int)result.user.id;
                        userole = (string)result.user.role;
                        string rawUsername = (string)result.user.username;

                        // I-format ang username (First letter capital)
                        username = rawUsername.Substring(0, 1).ToUpper() + rawUsername.Substring(1);

                        MessageBox.Show("Welcome: " + username, "Login Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        loadform(new dashboard());
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Invalid Username or Password", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("API Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // --- UI EVENTS (DESIGN & COLORS) ---

        private void btnclose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public static void colorcontrol(TextBox tx1, TextBox tx2)
        {
            tx1.BackColor = Color.White;
            tx2.BackColor = SystemColors.Control;
        }

        public static void colorcontrol2(Panel pnl1, Panel pnl2)
        {
            pnl1.BackColor = Color.White;
            pnl2.BackColor = SystemColors.Control;
        }

        private void txtbxusername_Click(object sender, EventArgs e)
        {
            colorcontrol(txtbxusername, txtbxpass);
            colorcontrol2(panelusernm, panelpass);
        }

        private void txtbxpass_Click(object sender, EventArgs e)
        {
            colorcontrol(txtbxpass, txtbxusername);
            colorcontrol2(panelpass, panelusernm);
        }

        private void pictureBox3_MouseDown(object sender, MouseEventArgs e)
        {
            txtbxpass.UseSystemPasswordChar = false; // Ipakita ang password
        }

        private void pictureBox3_MouseUp(object sender, MouseEventArgs e)
        {
            txtbxpass.UseSystemPasswordChar = true; // Itago ang password
        }

        private void txtbxusername_TextChanged(object sender, EventArgs e)
        {
            usererror.Visible = string.IsNullOrEmpty(txtbxusername.Text);
        }

        private void txtbxpass_TextChanged(object sender, EventArgs e)
        {
            passerror.Visible = string.IsNullOrEmpty(txtbxpass.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtbxusername.Text) || string.IsNullOrEmpty(txtbxpass.Text))
            {
                MessageBox.Show("Please input username and Password", "Login Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                login(); // Tawgon ang API Login function
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnFP_Click(object sender, EventArgs e)
        {
            loadform(new forgetpass());
            this.Hide();
        }
    }
}