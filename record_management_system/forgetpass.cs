using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace record_management_system
{
    public partial class forgetpass : Form
    {
        public forgetpass()
        {
            InitializeComponent();
        }

        private void panelusernm_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Login.loadform(new Login());
            this.Hide();
        }
        public void getdata(TextBox txtbxuserN,string find,TextBox txtbxans)
        {
            try
            {
                databaseconnection.Open();
                string queryfind = "SELECT " + find + " FROM users WHERE username = '" + txtbxuserN.Text + "'";
                
                MySqlCommand Commanddatabase = new MySqlCommand(queryfind, databaseconnection);

                object result = Commanddatabase.ExecuteScalar();
                //string questions = Convert.ToString(result);

                if (result != null)
                {


                    //decrypt ung nakuhang answer sa result then save it to "questions" need mo ilagay ung questions dun sa result sa if sta
                    string questions = Cryptography.Decrypt(result.ToString());


                    string username1 = "";
                    string password = "";


                    if (txtbxans.Text.Equals(Convert.ToString(questions), StringComparison.OrdinalIgnoreCase))
                    {


                        if (txtbxpass.Text.Trim() == txtbxCP.Text.Trim())
                        {
                            username1 = txtbxusername.Text;
                            password = Cryptography.Encrypt(txtbxpass.Text.ToString());

                            databaseconnection.Close();
                            string updatepass = "UPDATE users SET password='" + password + "'WHERE username = '" + username1 + "'";
                            executeQuery(updatepass);

                            Login f = new Login();
                            f.Show();

                            this.Close();

                        }
                        else
                        {
                            MessageBox.Show("The Password is not the same", "FORGET PASSWORD", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Verification Failed!", "Verification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtbxpass.Clear();
                        txtbxCP.Clear();

                    }
                }
                else
                {
                    MessageBox.Show("User not found!", "User Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);

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
        private void btnlogin_Click(object sender, EventArgs e)
        {
            if (txtbxusername.Text == "" || txtbxpass.Text == "" || txtbxCP.Text == "" || textBox1.Text == "")
            {

                MessageBox.Show("Please Fill up Form", "Login Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                if (txtbxusername.Text == "")
                {
                    usererror.Visible = true;
                }
                if (txtbxpass.Text == "")
                {
                    passerror1.Visible = true;
                }
                if (txtbxCP.Text == "")
                {
                    passerror2.Visible = true;
                }
                if (textBox1.Text == "")
                {
                    label6.Visible = true;
                }

                errormsg.Text = "Please Fill up form!";
            }
            else
            {

                try
                {

                    if (comboBox1.Text == "The name of the City you were born?")
                    {
                        getdata(txtbxusername,"question1",textBox1);

                    }
                    else if (comboBox1.Text == "The name of the first school you attended?")
                    {

                        getdata(txtbxusername, "question2", textBox1);
                    }
                    else if (comboBox1.Text == "Your childhood nickname?")
                    {
                        getdata(txtbxusername, "question3", textBox1);
                    }
                    else
                    {
                        MessageBox.Show("Please Answer the Verification");
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
                    MessageBox.Show("Password Updated","Forget Password",MessageBoxButtons.OK,MessageBoxIcon.Information);
                   
                }
                else
                {
                    //MessageBox.Show("Query Not Executed");
                    MessageBox.Show("User not Found", "Forget Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtbxpass.Clear();
                    txtbxCP.Clear();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                databaseconnection.Close();
            }
           
         
        }

        private void txtbxusername_TextChanged(object sender, EventArgs e)
        {
            if (txtbxusername.Text == "")
            {
                usererror.Visible = true;
                errormsg.Text = "Please Fill up form!";
            }
            else
            {
                usererror.Visible = false;
                errormsg.Text = "";
            }
        }

        private void txtbxpass_TextChanged(object sender, EventArgs e)
        {
            if (txtbxpass.Text == "")
            {
                passerror1.Visible = true;
                errormsg.Text = "Please Fill up form!";
            }
            else
            {
                passerror1.Visible = false;
                errormsg.Text = "";
            }
        }

        private void txtbxusername_Click(object sender, EventArgs e)
        {
            Login.colorcontrol(txtbxusername, txtbxpass);
            Login.colorcontrol(txtbxusername, txtbxCP);
            Login.colorcontrol(txtbxusername, textBox1);

            Login.colorcontrol2(panelusernm, panelpass);
            Login.colorcontrol2(panelusernm, panelpass2);
            Login.colorcontrol2(panelusernm, panel3);

            if (txtbxpass.Text == "" || txtbxCP.Text == ""|| textBox1.Text == "")
            {

                if (txtbxpass.Text == "")
                {
                    passerror1.Visible = true;
                }
                if (txtbxCP.Text == "")
                {
                    passerror2.Visible = true;
                }
                if (textBox1.Text == "")
                {
                    label6.Visible = true;
                }
                errormsg.Text = "Please Fill up form!";
            }
        }

        private void txtbxpass_Click(object sender, EventArgs e)
        {
            Login.colorcontrol(txtbxpass, txtbxusername);
            Login.colorcontrol(txtbxpass, txtbxCP);
            Login.colorcontrol(txtbxpass, textBox1);

            Login.colorcontrol2(panelpass, panelusernm);
            Login.colorcontrol2(panelpass, panelpass2);
            Login.colorcontrol2(panelpass, panel3);



            if (txtbxusername.Text == "" || txtbxCP.Text == "" || textBox1.Text == "")
            {
                if (txtbxusername.Text == "")
                {
                    usererror.Visible = true;
                }
                if (txtbxCP.Text == "")
                {
                    passerror2.Visible = true;
                }
                if (textBox1.Text == "")
                {
                    label6.Visible = true;
                }
                errormsg.Text = "Please Fill up form!";
            }
        }

        private void txtbxCP_Click(object sender, EventArgs e)
        {
            Login.colorcontrol(txtbxCP, txtbxusername);
            Login.colorcontrol(txtbxCP, txtbxpass);
            Login.colorcontrol(txtbxCP, textBox1);

            Login.colorcontrol2(panelpass2, panelusernm);
            Login.colorcontrol2(panelpass2, panelpass);
            Login.colorcontrol2(panelpass2, panel3);


            if (txtbxusername.Text == "" || txtbxpass.Text == "" || textBox1.Text == "")
            {

                if (txtbxusername.Text == "")
                {
                    usererror.Visible = true;
                }
                if (txtbxpass.Text == "")
                {
                    passerror1.Visible = true;
                }
                if(textBox1.Text == "")
                {
                    label6.Visible = true;
                }
                errormsg.Text = "Please Fill up form!";


            }
        }

        private void panelpass2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtbxCP_TextChanged(object sender, EventArgs e)
        {
            if (txtbxCP.Text == "")
            {
                passerror2.Visible = true;
                errormsg.Text = "Please Fill up form!";
            }
            else
            {
                passerror2.Visible = false;
                errormsg.Text = "";
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                label6.Visible = true;
                errormsg.Text = "Please Fill up form!";
            }
            else
            {
                label6.Visible = false;
                errormsg.Text = "";
            }
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            Login.colorcontrol(textBox1, txtbxusername);
            Login.colorcontrol(textBox1, txtbxpass);
            Login.colorcontrol(textBox1, txtbxCP);

            Login.colorcontrol2(panel3, panelusernm);
            Login.colorcontrol2(panel3, panelpass);
            Login.colorcontrol2(panel3, panelpass2);


            if (txtbxusername.Text == "" || txtbxpass.Text == ""||txtbxCP.Text == "")
            {

                if (txtbxusername.Text == "")
                {
                    usererror.Visible = true;
                }
                if (txtbxpass.Text == "")
                {
                    passerror1.Visible = true;
                }
                if (txtbxCP.Text == "")
                {
                    passerror2.Visible = true;
                }
                errormsg.Text = "Please Fill up form!";


            }

        }
       


      
    }
}
