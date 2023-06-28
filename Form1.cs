using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATM
{

    // Form1 Class
    public partial class Form1 : Form
    {
        public static int user_id;
        private FormHandler formHandler;

        //public static String 

        public Form1()
        {
            InitializeComponent();
            formHandler = new FormHandler();
        }

        private void Continue_Click(object sender, EventArgs e)
        {
            formHandler.HandleContinueClick(accNo, password, this);
        }

        private void label6_Click(object sender, EventArgs e)
        {
            formHandler.HandleCreateAccountClick(this);
        }

        private void Form1_Load(object sender, EventArgs e) { }

        private void textBox1_TextChanged(object sender, EventArgs e) { }

        private void groupBox1_Enter(object sender, EventArgs e) { }
    }


    // Database Connection Class
    public class DatabaseConnector
    {
        private string server;
        private string username;
        private string server_password;
        private string database;
        private MySqlConnection connection;

        public DatabaseConnector()
        {
            server = "localhost";
            username = "root";
            server_password = "";
            database = "project";
        }

        public void OpenConnection()
        {
            string connectionString = $"SERVER={server};DATABASE={database};UID={username};PASSWORD={server_password};";
            connection = new MySqlConnection(connectionString);
            connection.Open();
        }

        public void CloseConnection()
        {
            if (connection != null && connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        public MySqlCommand CreateCommand(string query)
        {
            return new MySqlCommand(query, connection);
        }
    }

    // Account Authentication Class
    public class AccountAuthenticator
    {
        private DatabaseConnector databaseConnector;

        public AccountAuthenticator()
        {
            databaseConnector = new DatabaseConnector();
        }

        public bool ValidateFields(string accountNumber, string password)
        {
            return !string.IsNullOrWhiteSpace(accountNumber.Trim()) && !string.IsNullOrWhiteSpace(password);
        }

        public bool CheckCredentials(string accountNumber, string password, Form1 form)
        {
            databaseConnector.OpenConnection();

            string checkquery = $"SELECT * FROM users WHERE Account_Number = '{accountNumber}' AND Password = '{password}'";
            MySqlCommand cmd = databaseConnector.CreateCommand(checkquery);

            try
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Form1.user_id = reader.GetInt32("Account_Number");
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                databaseConnector.CloseConnection();
            }
        }
    }

    // Form Navigation and Event Handling Class
    public class FormHandler
    {
        private AccountAuthenticator accountAuthenticator;

        public FormHandler()
        {
            accountAuthenticator = new AccountAuthenticator();
        }

        public void HandleContinueClick(TextBox accNoTextBox, TextBox passwordTextBox, Form1 form)
        {
            string accountNumber = accNoTextBox.Text;
            string password = passwordTextBox.Text;

            if (!accountAuthenticator.ValidateFields(accountNumber, password))
            {
                MessageBox.Show("Please fill in all the fields");
                return;
            }

            if (accountAuthenticator.CheckCredentials(accountNumber, password, form))
            {
                MessageBox.Show("Login Successful");

                form.Hide();
                TransOptions transactions = new TransOptions();
                transactions.Show();
            }
            else
            {
                MessageBox.Show("Invalid Username or Password");
            }
        }


        public void HandleCreateAccountClick(Form1 form)
        {
            form.Hide();
            Create_Account create = new Create_Account();
            create.Show();
        }
    }

}
