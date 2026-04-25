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

namespace record_management_system
{
    public partial class user_add : Form
    {

        private FORMS.USERS form1;
        MySqlCommand cm;
        MySqlDataReader dr;

        public user_add(FORMS.USERS form1)
        {
            InitializeComponent();
            this.form1 = form1;
        }
        public void adminload()
        {
            MySqlConnection dbconnect = new MySqlConnection(Login.connectstring);
            try
            {
                form1.dataGridView1.Rows.Clear();
                dbconnect.Open();


                cm = new MySqlCommand("SELECT * FROM users WHERE user_role = 1", dbconnect);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    form1.dataGridView1.Rows.Add(dr["ID"].ToString(), dr["username"].ToString(), dr["password"].ToString());
                }

                form1.dataGridView1.Columns["btnuserdel"].Visible = false;
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


        public void userload()
        {
            MySqlConnection dbconnect = new MySqlConnection(Login.connectstring);
            try
            {
                form1.dataGridView1.Rows.Clear();
                dbconnect.Open();


                cm = new MySqlCommand("SELECT * FROM users WHERE user_role = 2", dbconnect);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    form1.dataGridView1.Rows.Add(dr["ID"].ToString(), dr["username"].ToString(), dr["password"].ToString());
                }

                form1.dataGridView1.Columns["btnuserdel"].Visible = false;
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

        public void loadrecord()
        {
            MySqlConnection dbconnect = new MySqlConnection(Login.connectstring);
            try
            {

                form1.dataGridView1.Rows.Clear();
                dbconnect.Open();

                if (form1.comboBox1.Text == "SHOW ADMINS")
                {
                    cm = new MySqlCommand("SELECT * FROM users WHERE user_role = 1", dbconnect);
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        form1.dataGridView1.Rows.Add(dr["ID"].ToString(), dr["username"].ToString(), dr["password"].ToString());
                    }

                    form1.dataGridView1.Columns["btnuserdel"].Visible = false;



                }
                else if (form1.comboBox1.Text == "SHOW USERS")
                {
                    cm = new MySqlCommand("SELECT * FROM users WHERE user_role = 2", dbconnect);
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        form1.dataGridView1.Rows.Add(dr["ID"].ToString(), dr["username"].ToString(), dr["password"].ToString());
                    }
                    form1.dataGridView1.Columns["btnuseredit"].Visible = true;
                    form1.dataGridView1.Columns["btnuserdel"].Visible = true;
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


        private void btnlogin_Click(object sender, EventArgs e)
        {
            if (txtbxusername.Text == "" || txtbxpass.Text == "" || txtbxCP.Text == "" || txtbxquestion1.Text == "" || txtbxquestion2.Text == "" || txtbxquestion3.Text == "")
            {

                MessageBox.Show("Please Fill up the Form!", "Login Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

               

                errormsg.Text = "Please Fill up form!";
            }
            else
            {
                MySqlConnection dbconnect = new MySqlConnection(Login.connectstring);
                if (txtbxpass.Text.Trim() == txtbxCP.Text.Trim())
                {
                

                if (useroles == "ADMIN")
                    {
                        adminload();
                        int count = 0;
                        //string insertQuery = "INSERT INTO insertupdatedelete(firstname, lastname, age) VALUES('" + textBoxfirstname.Text + "','" + textBoxlastname.Text + "'," + textBoxage.Text + ")";
                        foreach (DataGridViewRow r in form1.dataGridView1.Rows)
                        {
                            count++;
                        }
                        

                        if (count >= 3)
                        {
                            MessageBox.Show("The Admin Role is only allowed to up to 3 accounts only", "System Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                          
                            try
                            {
                                string UserName = txtbxusername.Text;
                                string Password = Cryptography.Encrypt(txtbxpass.Text.ToString());   // Passing the Password to Encrypt method and the method will return encrypted string and stored in Password variable.
                                string question1 = Cryptography.Encrypt(txtbxquestion1.Text.ToString());
                                string question2 = Cryptography.Encrypt(txtbxquestion2.Text.ToString());
                                string question3 = Cryptography.Encrypt(txtbxquestion3.Text.ToString());

                                dbconnect.Open();
                                MySqlCommand insert = new MySqlCommand("insert into users(username,password,user_role,question1,question2,question3)values('" + UserName + "','" + Password + "','1','" + question1 + "','" + question2 + "','" + question3 + "')", dbconnect);
                                insert.ExecuteNonQuery();
                                dbconnect.Close();
                                MessageBox.Show("Record inserted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                adminload();
                                this.Close();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                            finally
                            {
                                dbconnect.Close();
                            }

                            /*string addadmin = "INSERT INTO users(username,password,user_role,question1,question2,question3) VALUES('" + txtbxusername.Text + "','" + txtbxpass.Text + "','1','" +txtbxquestion1.Text +"','"+txtbxquestion2.Text+"','"+txtbxquestion3.Text+"')";
                            executeQuery(addadmin);
                            txtbxusername.Clear();
                            txtbxpass.Clear();
                            txtbxCP.Clear();
                            adminload();
                            this.Close();*/
                        }


                       
                    }
                else if (useroles == "USER")
                    {
                        userload();
                        string UserName = txtbxusername.Text;
                        string Password = Cryptography.Encrypt(txtbxpass.Text.ToString());   // Passing the Password to Encrypt method and the method will return encrypted string and stored in Password variable.
                        string question1 = Cryptography.Encrypt(txtbxquestion1.Text.ToString());
                        string question2 = Cryptography.Encrypt(txtbxquestion2.Text.ToString());
                        string question3 = Cryptography.Encrypt(txtbxquestion3.Text.ToString());


                        dbconnect.Open();
                        MySqlCommand insert = new MySqlCommand("insert into users(username,password,user_role,question1,question2,question3)values('" + UserName + "','" + Password + "','2','" + question1 + "','" + question2 + "','" + question3 + "')", dbconnect);
                        insert.ExecuteNonQuery();
                        dbconnect.Close();
                        MessageBox.Show("Record inserted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        userload();
                        this.Close();

                        /*userload();
                        string adduser = "INSERT INTO users(username,password,user_role,question1,question2,question3) VALUES('" + txtbxusername.Text + "','" + txtbxpass.Text + "','2','" + txtbxquestion1.Text + "','" + txtbxquestion2.Text + "','" + txtbxquestion3.Text + "')";
                        executeQuery(adduser);
                        txtbxusername.Clear();
                        txtbxpass.Clear();
                        txtbxCP.Clear();
                        userload();
                        this.Close();*/
                    }
                else
                    {
                        MessageBox.Show("Please choose the User Role","System Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                        
                    }

                }
                else
                {
                    MessageBox.Show("The Password is not the same", "FORGET PASSWORD", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    
                }

            }
        }

        MySqlConnection databaseconnection = new MySqlConnection(Login.connectstring);

        public void executeQuery(string query)
        {
            try
            {

                MySqlCommand Commanddatabase = new MySqlCommand(query, databaseconnection);

                databaseconnection.Open();

                if (Commanddatabase.ExecuteNonQuery() == 1)
                {
                    //MessageBox.Show("Query Executed");
                    MessageBox.Show("Added", "Forget Password", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadrecord();

                }
                else
                {
                    //MessageBox.Show("Query Not Executed");
                    MessageBox.Show("not Added", "Forget Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtbxpass.Clear();
                    txtbxCP.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                databaseconnection.Close();
            }


        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtbxusername_TextChanged(object sender, EventArgs e)
        {
            if (txtbxusername.Text == "")
            {
                
                errormsg.Text = "Please Fill up form!";
            }
            else
            {
                
                errormsg.Text = "";

                if (txtbxpass.Text == "" || txtbxCP.Text == "")
                {
                    errormsg.Text = "Please Fill up form!";
                    if (txtbxpass.Text == "")
                    {
                        
                        errormsg.Text = "Please Fill up form!";
                    }
                    if (txtbxCP.Text == "")
                    {
                        
                        errormsg.Text = "Please Fill up form!";
                    }
                }

            }
        }

        private void txtbxpass_TextChanged(object sender, EventArgs e)
        {
            if (txtbxpass.Text == "")
            {
                
                errormsg.Text = "Please Fill up form!";
            }
            else
            {
                
                errormsg.Text = "";
                if (txtbxusername.Text == "" || txtbxCP.Text == "")
                {
                    errormsg.Text = "Please Fill up form!";
                    if (txtbxusername.Text == "")
                    {
                        
                        errormsg.Text = "Please Fill up form!";
                    }
                    if (txtbxCP.Text == "")
                    {
                        
                        errormsg.Text = "Please Fill up form!";
                    }
                }
            }
        }

        private void txtbxCP_TextChanged(object sender, EventArgs e)
        {
            if (txtbxCP.Text == "")
            {
                
                errormsg.Text = "Please Fill up form!";
            }
            else
            {
                
                errormsg.Text = "";
                if (txtbxusername.Text == "" || txtbxpass.Text == "")
                {
                    errormsg.Text = "Please Fill up form!";
                    if (txtbxusername.Text == "")
                    {
                        
                        errormsg.Text = "Please Fill up form!";
                    }
                    if (txtbxpass.Text == "")
                    {
                        
                        errormsg.Text = "Please Fill up form!";
                    }
                }

            }
        }

        private void pictureBox3_MouseUp(object sender, MouseEventArgs e)
        {
            txtbxpass.UseSystemPasswordChar = true;
        }

        private void pictureBox3_MouseDown(object sender, MouseEventArgs e)
        {
            txtbxpass.UseSystemPasswordChar = false;
        }

        private void pictureBox4_MouseUp(object sender, MouseEventArgs e)
        {
            txtbxCP.UseSystemPasswordChar = true;
        }

        private void pictureBox4_MouseDown(object sender, MouseEventArgs e)
        {
            txtbxCP.UseSystemPasswordChar = false;
        }

        private void txtbxpass_Click(object sender, EventArgs e)
        {
          



            if (txtbxusername.Text == "" || txtbxCP.Text == "")
            {
                if (txtbxusername.Text == "")
                {
                    
                }
                if (txtbxCP.Text == "")
                {
                    
                }
                errormsg.Text = "Please Fill up form!";
            }
        }

        private void txtbxCP_Click(object sender, EventArgs e)
        {



            if (txtbxusername.Text == "" || txtbxpass.Text == "")
            {

                if (txtbxusername.Text == "")
                {
                    
                }
                if (txtbxpass.Text == "")
                {
                    
                }
                errormsg.Text = "Please Fill up form!";


            }
        }

        private void txtbxusername_Click(object sender, EventArgs e)
        {
 

            if (txtbxpass.Text == "" || txtbxCP.Text == "")
            {

                if (txtbxpass.Text == "")
                {
                    
                }
                if (txtbxCP.Text == "")
                {
                   
                }
                errormsg.Text = "Please Fill up form!";
            }
        }
        string useroles;
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "ADMIN")
            {
                useroles = "ADMIN";
                adminload();
            }
            else if (comboBox1.Text == "USER")
            {
                useroles = "USER";
                userload();
            }
          
        }
    }
}

