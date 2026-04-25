using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace record_management_system
{
    public partial class dashboard : Form
    {

        // Fields
        private Button currentbtn;
        public Form activeform;

        // Constructor
        public dashboard()
        {
            InitializeComponent();
            this.Text = string.Empty;
            this.ControlBox = false;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        // UI Methods
        private Color SelectThemeColor()
        {
            string color = "#2980b9";
            return ColorTranslator.FromHtml(color);
        }

        private void ActiveButton(object btnSender)
        {
            if (btnSender != null)
            {
                DisableButton();
                Color color = SelectThemeColor();
                currentbtn = (Button)btnSender;
                currentbtn.BackColor = color;
                currentbtn.ForeColor = Color.White;
                currentbtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }
        }

        public void OpenForm(Form childform, object btnsender)
        {
            if (activeform != null)
            {
                activeform.Close();
            }
            ActiveButton(btnsender);
            activeform = childform;
            childform.TopLevel = false;
            childform.FormBorderStyle = FormBorderStyle.None;
            childform.Dock = DockStyle.Fill;
            this.panelmain.Controls.Add(childform);
            this.panelmain.Tag = childform;
            childform.BringToFront();
            childform.Show();

            string formNameWithSpaces = childform.Text.Replace("_", " ");
            lbltitle.Text = formNameWithSpaces;
        }

        private void DisableButton()
        {
            foreach (Control previousBtn in pnlmenu.Controls)
            {
                if (previousBtn.GetType() == typeof(Button))
                {
                    previousBtn.BackColor = Color.FromArgb(37, 41, 88);
                    previousBtn.ForeColor = Color.White;
                    previousBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
            }
        }

        private void Reset()
        {
            lbltitle.Text = "DASHBOARD";
            currentbtn = null;
        }

        // --- Sidebar Button Click Events ---

        private void btndash_Click(object sender, EventArgs e)
        {
            ActiveButton(sender);

            // I-close lang ang form nga nagtabon sa dashboard
            if (activeform != null)
            {
                activeform.Close();
                activeform = null; // Importante ni para dili mag-error sunod click
            }
            this.panelmain.Controls.Clear(); // Limpyohan ang panel

            // Kon ang imong cards (Total Residents, etc.) gi-design nimo sa Form mismo,
            // ang pinakasimple nga paagi para mobalik sila kay i-call ang InitializeComponent
            // Pero basin mag-double ang buttons, so mas maayo nga i-load nimo ang dashboard content balik.

            // Pananglitan, i-refresh ang tibuok form:
            dashboard refresh = new dashboard();
            refresh.Show();
            this.Hide();

            Reset();
        }

        private void btnbrgyservice_Click(object sender, EventArgs e)
        {
            OpenForm(new FORMS.USERS(), sender);
            lbltitle.Text = "BARANGAY SERVICES";
        }

        private void btnwaterlink_Click(object sender, EventArgs e)
        {
            // GIDUGANG: Kini para sa sidebar 'Water Linkage' button
            OpenForm(new FORMS.WaterLinkage(), sender);
            lbltitle.Text = "WATER LINKAGE";
        }

        // --- Form Controls & Load ---

        private void dashboard_Load(object sender, EventArgs e)
        {
            lblUsername.Text = "Welcome, " + Login.username;
            {
                using (MySqlConnection connection = new MySqlConnection(Login.connectstring))
                {
                    try
                    {
                        connection.Open();

                        // 1. Pag-ihap sa Users
                        string queryUsers = "SELECT COUNT(*) FROM users";
                        MySqlCommand command = new MySqlCommand(queryUsers, connection);
                        lbluser.Text = command.ExecuteScalar().ToString();

                        // 2. Pag-ihap sa Residents (Gidugangan og 'FROM' ug gi-fix ang spelling)
                        string queryResidents = "SELECT COUNT(*) FROM client_list";
                        MySqlCommand command2 = new MySqlCommand(queryResidents, connection);
                        lblskbene.Text = command2.ExecuteScalar().ToString();

                        // 3. Pag-ihap sa Services/Transactions (Ilisdi ang 'transaction' kung unsay bag-ong ngalan sa table)
                        // Pananglitan: 'brgy_services' o 'requests'
                        string queryTrans = "SELECT COUNT(*) FROM client_list"; // temporaryo lang ni, ilisdi sa saktong table name
                        MySqlCommand command3 = new MySqlCommand(queryTrans, connection);
                        label4.Text = command3.ExecuteScalar().ToString();

                        // Role-based Access Control (Kadtong kaganina)
                        if (Login.userole == "1")
                        {
                            btnbrgyservice.Visible = true;
                            panel1.Visible = true;
                            btnreports.Visible = true;
                        }
                        else
                        {
                            panel1.Visible = false;
                            btnbrgyservice.Visible = false;
                            btnreports.Visible = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Dashboard Load Error: " + ex.Message);
                    }
                }
            }

            // Role-based Access Control sa dashboard_Load
            if (Login.userole == "1") // Admin
            {
                btnbrgyservice.Visible = true;
                btnreports.Visible = true;  // I-show ang reports para sa Admin
                panel1.Visible = true;
            }
            else
            {
                // Dinhi nimo i-control kung unsa gyud ang itago
                btnreports.Visible = true;

                // Siguroha nga ang Barangay Service makita gihapon
                btnbrgyservice.Visible = true;
                panel1.Visible = true;

            }
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Exit?", "Exit Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to Logout?", "LOGOUT CONFIRMATION", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                Login.loadform(new Login());
                this.Hide();
            }
        }

        private void pnltitlebar_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void btnmini_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }


        private void btnmaximize_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void btnprofiling_Click_1(object sender, EventArgs e)
        {
            // Kini ang mag-open sa imong Residents list form sulod sa panelmain
            // Siguroha nga ang 'RESIDENTS' mao ang ngalan sa imong Form class
            OpenForm(new FORMS.Residents(), sender);

            // I-update ang text sa title bar sa babaw
            lbltitle.Text = "RESIDENT PROFILING";
        }

        private void btnwaterlink_Click_1(object sender, EventArgs e)
        {
            // GIDUGANG: Kini para sa shortcut button/card sa dashboard
            OpenForm(new FORMS.WaterLinkage(), sender);
            lbltitle.Text = "WATER LINKAGE";
        }

        private void btnbrgyservice_Click_1(object sender, EventArgs e)
        {
            // Gamiton ang OpenForm function para isulod ang BarangayService sa panel
            OpenForm(new FORMS.BarangayService(), sender);
        }

        private void btnreports_Click_1(object sender, EventArgs e)
        {
            // 'FORMS.Reports' ang gigamit nato kay naa man sa FORMS folder imong Reports.cs
            // Siguroha nga ang 'sender' mao ang button nga gi-click
            OpenForm(new FORMS.Reports(), sender);
        }

    }
}