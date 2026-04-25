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

namespace record_management_system.FORMS
{
    public partial class USERS : Form
    {

        public string  primaryid;
        MySqlCommand cm;
        MySqlDataReader dr;


        public USERS()
        {
            InitializeComponent();
            loadrecord();
            txtSearch.TextChanged += textBox1_TextChanged;
        }
        public void loadrecord()
        {
            MySqlConnection dbconnect = new MySqlConnection(Login.connectstring);
            try
            {

                dataGridView1.Rows.Clear();
                dbconnect.Open();

                if (comboBox1.Text == "SHOW ADMINS")
                {
                    cm = new MySqlCommand("SELECT * FROM users WHERE user_role = 1", dbconnect);
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        dataGridView1.Rows.Add(dr["ID"].ToString(), dr["username"].ToString(), dr["password"].ToString());
                    }
                 
                    this.dataGridView1.Columns["btnuserdel"].Visible = false;

                }else if (comboBox1.Text == "SHOW USERS" || comboBox1.Text == "ROLE")
                {
                    cm = new MySqlCommand("SELECT * FROM users WHERE user_role = 2", dbconnect);
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        dataGridView1.Rows.Add(dr["ID"].ToString(), dr["username"].ToString(), dr["password"].ToString());
                    }
                    this.dataGridView1.Columns["btnuseredit"].Visible = true;
                    this.dataGridView1.Columns["btnuserdel"].Visible = true;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                dbconnect.Close();
            }
        }
        private void USERS_Load(object sender, EventArgs e)
        {
            loadrecord();
        }

      

        private void btnadd_Click(object sender, EventArgs e)
        {
            user_add form2 = new user_add(this);
            form2.Show();
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadrecord();
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string searchQuery = txtSearch.Text.ToLower();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                bool isVisible = false;
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null && cell.Value.ToString().ToLower().Contains(searchQuery))
                    {
                        isVisible = true;
                        break;
                    }
                }
                row.Visible = isVisible;
            }
        }
    }
}
