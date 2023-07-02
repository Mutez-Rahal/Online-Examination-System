using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Online_Examination_System
{
    public partial class ExamForm : Form
    {

        private SqlConnection connection;
        private int currentQuestionIndex;
        private int totalQuestions;
        private int points;

        public ExamForm()
        {
            InitializeComponent();
            connection = new SqlConnection("data source = DESKTOP-AFFK6S9\\SQLEXPRESS; Initial Catalog = ExaminationSystem; integrated security = true;");
            currentQuestionIndex = 0;
            totalQuestions = 10;
            points = 0;
        }
        private void LoadQuestionsFromDatabase()
        {
            
                try
            {
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT QuestionText, OptionA, OptionB, OptionC, OptionD, Answer FROM Questions WHERE QuestionID = @QuestionID", connection);
                command.Parameters.AddWithValue("@QuestionID", currentQuestionIndex + 1);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    string question = reader.GetString(0);
                    string optionA = reader.GetString(1);
                    string optionB = reader.GetString(2);
                    string optionC = reader.GetString(3);
                    string optionD = reader.GetString(4);

                    questionText.Text = question;
                    radioOptionA.Text = optionA;
                    radioOptionB.Text = optionB;
                    radioOptionC.Text = optionC;
                    radioOptionD.Text = optionD;
                }

                reader.Close();
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
        private void CheckAnswer()
        {
            
                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("SELECT Answer FROM Questions WHERE QuestionID = @QuestionID", connection);
                    command.Parameters.AddWithValue("@QuestionID", currentQuestionIndex + 1);

                    string correctAnswer = command.ExecuteScalar().ToString();

                    if (radioOptionA.Checked && correctAnswer == "A")
                    {
                        points += 10;
                    }
                    else if (radioOptionB.Checked && correctAnswer == "B")
                    {
                        points += 10;
                    }
                    else if (radioOptionC.Checked && correctAnswer == "C")
                    {
                        points += 10;
                    }
                    else if (radioOptionD.Checked && correctAnswer == "D")
                    {
                        points += 10;
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

        public void changeColor(int i)
        {
            switch (i)
            {
                case 1:
                    que1.BackColor = Color.FromArgb(120, 17, 61);
                    que1.ForeColor = Color.White;
                    break;

                case 2:
                    que2.BackColor = Color.FromArgb(120, 17, 61);
                    que2.ForeColor = Color.White;
                    break;

                case 3:
                    que3.BackColor = Color.FromArgb(120, 17, 61);
                    que3.ForeColor = Color.White;
                    break;

                case 4:
                    que4.BackColor = Color.FromArgb(120, 17, 61);
                    que4.ForeColor = Color.White;
                    break;
                case 5:
                    que5.BackColor = Color.FromArgb(120, 17, 61);
                    que5.ForeColor = Color.White;
                    break;
                case 6:
                    que6.BackColor = Color.FromArgb(120, 17, 61);
                    que6.ForeColor = Color.White;
                    break;

                case 7:
                    que7.BackColor = Color.FromArgb(120, 17, 61);
                    que7.ForeColor = Color.White;
                    break;

                case 8:
                    que8.BackColor = Color.FromArgb(120, 17, 61);
                    que8.ForeColor = Color.White;
                    break;

                case 9:
                    que9.BackColor = Color.FromArgb(120, 17, 61);
                    que9.ForeColor = Color.White;
                    break;
                case 10:
                    que10.BackColor = Color.FromArgb(120, 17, 61);
                    que10.ForeColor = Color.White;
                    break;
            }
        }

        private void SavePointsToDatabase(int points)
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("UPDATE Students SET Points = @Points WHERE StudentNo = @StudentNo", connection);
                command.Parameters.AddWithValue("@Points", Convert.ToString(points));
                command.Parameters.AddWithValue("@StudentNo", Login_UI.student);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Your ExamHas Finished, Thank You :)");
                }
                else
                {
                    MessageBox.Show("Failed to save points!");
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


        private void ExamForm_Load(object sender, EventArgs e)
        {
            LoadQuestionsFromDatabase();
            changeColor(currentQuestionIndex + 1);

        }

        private void btnNextQue_Click(object sender, EventArgs e)
        {
            CheckAnswer();
            currentQuestionIndex++;
            changeColor(currentQuestionIndex + 1);
            if(currentQuestionIndex == 9)
            {
                btnNextQue.Text = "Finish";
            }
            if (currentQuestionIndex < totalQuestions)
            {
                LoadQuestionsFromDatabase();
            }
            else
            {
                SavePointsToDatabase(points);
                Login_UI l = new Login_UI();
                MessageBox.Show("You have answered all the questions. Your total points: " + points);
                this.Hide();
                l.Show();
            }
        }
    }
}
