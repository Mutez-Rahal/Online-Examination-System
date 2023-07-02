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

namespace Online_Examination_System
{
    public partial class Login_UI : Form
    {
        public static string student;
        private string connectionString = "data source = DESKTOP-AFFK6S9\\SQLEXPRESS; Initial Catalog=ExaminationSystem; integrated security = true;";
        public Login_UI()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Login_UI_Load(object sender, EventArgs e)
        {
            panelLogin.Visible = true;
            PanelRegister.Visible = false;
            wrongLabel.Visible = false;
            btnGeri.Visible = false;
        }
        private void btnRegister_Click(object sender, EventArgs e)
        {
            panelLogin.Visible = false;
            PanelRegister.Visible = true;
            wrongLabel.Visible = false;
            btnGeri.Visible = true;
        }

        private void btnGeri_Click(object sender, EventArgs e)
        {
            panelLogin.Visible = true;
            PanelRegister.Visible = false;
            wrongLabel.Visible = false;
            btnGeri.Visible = false;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)//  Hide,Show Password
        {
            if (checkBox2.Checked)
            {
                txtPassword.UseSystemPasswordChar = false;
                checkBox2.Text = "Hide Password";
            }
            else
            {
                txtPassword.UseSystemPasswordChar = true;
                checkBox2.Text = "Show Password";

            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)//  Hide,Show Password
        {
            if (checkBox1.Checked)
            {
                txtLoginPass.UseSystemPasswordChar = false;
                checkBox1.Text = "Hide Password";
            }
            else
            {
                txtLoginPass.UseSystemPasswordChar = true;
                checkBox1.Text = "Show Password";

            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string studentNo = txtloginStdNo.Text;
            string password = txtLoginPass.Text;
            student = txtloginStdNo.Text;


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Check if the student exists and has not taken the exam
                    SqlCommand checkCommand = new SqlCommand("SELECT COUNT(*) FROM Students WHERE StudentNo = @StudentNo AND Passwords = @Passwords AND HasTakenExam = 0", connection);
                    checkCommand.Parameters.AddWithValue("@StudentNo", studentNo);
                    checkCommand.Parameters.AddWithValue("@Passwords", password);

                    int count = (int)checkCommand.ExecuteScalar();

                    if (count == 1)
                    {
                        // Update the database to mark the student as having taken the exam
                        SqlCommand updateCommand = new SqlCommand("UPDATE Students SET HasTakenExam = 1 WHERE StudentNo = @StudentNo", connection);
                        updateCommand.Parameters.AddWithValue("@StudentNo", studentNo);
                        updateCommand.ExecuteNonQuery();

                        MessageBox.Show("Login successful!");
                        ExamForm ex = new ExamForm();
                        ex.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Sorry!! You have already taken the exam!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }


        private void register_Click(object sender, EventArgs e)
        {
            string firstName = txtFirstName.Text;
            string lastName = txtLastName.Text;
            string studentNo = txtStudentNo.Text;
            string password = txtPassword.Text;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Students WHERE StudentNo = @StudentNo", connection);
                    command.Parameters.AddWithValue("@StudentNo", studentNo);

                    int count = (int)command.ExecuteScalar();

                    if (count == 0)
                    {
                        // Student number does not exist, can proceed with registration

                        command = new SqlCommand("INSERT INTO Students (StudentNo, FirstName, LastName, Passwords, HasTakenExam) VALUES (@StudentNo, @FirstName, @LastName, @Passwords, @HasTakenExam)", connection);
                        command.Parameters.AddWithValue("@StudentNo", studentNo);
                        command.Parameters.AddWithValue("@FirstName", firstName);
                        command.Parameters.AddWithValue("@LastName", lastName);
                        command.Parameters.AddWithValue("@Passwords", password);
                        command.Parameters.AddWithValue("@HasTakenExam", 0);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Registration successful! Now you can login");
                            panelLogin.Visible = true;
                            PanelRegister.Visible = false;
                            wrongLabel.Visible = false;
                            btnGeri.Visible = false;
                        }
                        else
                        {
                            MessageBox.Show("Registration failed!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Student number already exists!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }

            }
        }
    }
}
