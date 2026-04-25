using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace record_management_system.FORMS
{
    public partial class Reports : Form
    {

        string connStr = ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString;

        public Reports()
        {
            InitializeComponent();
        }

        private void Reports_Load(object sender, EventArgs e)
        {
            LoadStats();
        }

        public void LoadStats()
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();

                    // 1. Count Total Residents
                    MySqlCommand cmd1 = new MySqlCommand("SELECT COUNT(*) FROM client_list", conn);
                    lblTotalRes.Text = cmd1.ExecuteScalar().ToString();

                    // 2. Count Female (Panel 2)
                    MySqlCommand cmd2 = new MySqlCommand("SELECT COUNT(*) FROM client_list WHERE gender = 'Female'", conn);
                    lblFemale.Text = cmd2.ExecuteScalar().ToString();

                    // 3. Count Male (Panel 2)
                    MySqlCommand cmd3 = new MySqlCommand("SELECT COUNT(*) FROM client_list WHERE gender = 'Male'", conn);
                    lblMale.Text = cmd3.ExecuteScalar().ToString();

                  
                    // 1. Gamiton nato ang 'total + arrears' (kay mao nay naa sa imong DB)
                    // 2. Gamiton nato ang 'status = 1' (kay 1 man ang 'Paid' sa imong screenshot)
                    string queryWater = "SELECT SUM(total + arrears) FROM billing_list WHERE status = 1";

                    MySqlCommand cmd4 = new MySqlCommand(queryWater, conn);
                    object income = cmd4.ExecuteScalar();

                    if (income != DBNull.Value && income != null)
                    {
                        // Kini mo-gawas na dapat ang ₱ 150.00 base sa record ni Zenmar
                        lblIncome.Text = "₱ " + Convert.ToDouble(income).ToString("N2");
                    }
                    else
                    {
                        lblIncome.Text = "₱ 0.00";
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading stats: " + ex.Message);
                }
            }
        }
    }
}
