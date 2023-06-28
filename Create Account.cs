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
    // Create_Account Class
    public partial class Create_Account : Form
    {
        private CreateAccountFormHandler formHandler;

        public Create_Account()
        {
            InitializeComponent();
            formHandler = new CreateAccountFormHandler();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            formHandler.HandleCreateAccountClick(accName, accNum, email, password, this);
        }

        private void label7_Click(object sender, EventArgs e) 
        {
            formHandler.HandleBackButtonClick(this);
        }



        private void label1_Click(object sender, EventArgs e){ }

        private void textBox3_TextChanged(object sender, EventArgs e){ }

        private void groupBox1_Enter(object sender, EventArgs e){ }

        private void label2_Click(object sender, EventArgs e){ }

        private void label5_Click(object sender, EventArgs e){ }

        private void accName_TextChanged(object sender, EventArgs e){ }

        private void Create_Account_Load(object sender, EventArgs e)
        {

        }
    }

   


    // Account Creation Class
    public class AccountCreator
    {
        private DatabaseConnector databaseConnector;

        public AccountCreator()
        {
            databaseConnector = new DatabaseConnector();
        }

        public bool ValidateFields(string accName, string accNum, string email, string password)
        {
            return !string.IsNullOrWhiteSpace(accName.Trim()) && !string.IsNullOrWhiteSpace(accNum.Trim()) &&
                   !string.IsNullOrWhiteSpace(email.Trim()) && !string.IsNullOrWhiteSpace(password);
        }

        public bool CreateAccount(string accName, string accNum, string email, string password)
        {
            databaseConnector.OpenConnection();

            // Check if the user already exists
            string checkQuery = $"SELECT COUNT(*) FROM users WHERE Account_Number = '{accNum}'";
            MySqlCommand checkCmd = databaseConnector.CreateCommand(checkQuery);
            int userCount = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (userCount > 0)
            {
                MessageBox.Show("User already exists");
                databaseConnector.CloseConnection();
                return false;
            }

            // Create the new account
            string insertQuery = "INSERT INTO users (UserID, Account_Name, Account_Number, Email, Password) " +
                                 "VALUES ('', @accName, @accNum, @email, @password)";

            MySqlCommand insertCmd = databaseConnector.CreateCommand(insertQuery);
            insertCmd.Parameters.AddWithValue("@accName", accName);
            insertCmd.Parameters.AddWithValue("@accNum", accNum);
            insertCmd.Parameters.AddWithValue("@email", email);
            insertCmd.Parameters.AddWithValue("@password", password);

            
            // Create the new account
            string accountQuery = "INSERT INTO accounts (Account_Number, Balance) " +
                                 "VALUES (@accNum, @balance)";

            MySqlCommand accountCmd = databaseConnector.CreateCommand(accountQuery);
            accountCmd.Parameters.AddWithValue("@accNum", accNum);
            accountCmd.Parameters.AddWithValue("@balance", 0);           

            try
            {
                accountCmd.ExecuteNonQuery();
                int rowsAffected = insertCmd.ExecuteNonQuery();
                databaseConnector.CloseConnection();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
    }

    // Form Navigation and Event Handling Class
    public class CreateAccountFormHandler
    {
        private AccountCreator accountCreator;

        public CreateAccountFormHandler()
        {
            accountCreator = new AccountCreator();
        }

        public void HandleCreateAccountClick(
            TextBox accNameTxtBox, 
            TextBox accNumTxtBox, 
            TextBox emailTxtBox, 
            TextBox passwordTxtBox,
            Create_Account form
            )
        {
            string accName = accNameTxtBox.Text;
            string accNum = accNumTxtBox.Text;
            string email = emailTxtBox.Text;
            string password = passwordTxtBox.Text;

            if (!accountCreator.ValidateFields(accName, accNum, email, password))
            {
                MessageBox.Show("Please fill in all the fields");
                return;
            }

            if (accountCreator.CreateAccount(accName, accNum, email, password))
            {
                MessageBox.Show($"{accName} registered Successfully");
                form.Close();
                Form1 login = new Form1();
                login.Show();
            }
        }

        public void HandleBackButtonClick(Create_Account form)
        {
            form.Hide();
            Form1 login = new Form1();
            login.Show();
        }
    }
}
