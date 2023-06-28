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
    public partial class Withdraw : Form
    {
        public Withdraw()
        {
            InitializeComponent();

            textBox1.KeyPress += textBox1_KeyPress;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Withdraw_Load(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {
            this.Hide();
            TransOptions transOptions = new TransOptions();
            transOptions.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string amount = textBox1.Text;
            string transactionType = comboBox1.SelectedItem as string;

            if (validateFields(amount, transactionType))
            {
                int parsedAmount = int.Parse(amount);

                Transaction transaction = new Transaction();

                if (transactionType == "Withdraw")
                {
                    transaction.Withdraw(parsedAmount);
                }
                else if (transactionType == "Deposit")
                {
                    transaction.Deposit(parsedAmount);
                }
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if the pressed key is a digit, a control character, and not a newline character
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '\n')
            {
                // Reject the input by setting the Handled property to true
                e.Handled = true;
            }
        }


        private bool validateFields(string amount, string transType)
        {
            if (string.IsNullOrEmpty(amount) || string.IsNullOrEmpty(transType))
            {
                MessageBox.Show("Please enter an amount and select a transaction type.");
                return false;
            }
            return true;
        }
    }

    public class Transaction
    {
        private DatabaseConnector databaseConnector;

        public Transaction()
        {
            databaseConnector = new DatabaseConnector();
        }

        public void Withdraw(int amount)
        {
            databaseConnector.OpenConnection();

            int accountNumber = Form1.user_id;

            // Check the current balance
            string checkBalanceQuery = $"SELECT Balance FROM accounts WHERE Account_number = {accountNumber}";
            MySqlCommand checkBalanceCmd = databaseConnector.CreateCommand(checkBalanceQuery);
            int currentBalance = Convert.ToInt32(checkBalanceCmd.ExecuteScalar());

            // Check if the amount to withdraw is greater than the current balance
            if (amount > currentBalance)
            {
                MessageBox.Show("Insufficient balance.");
                databaseConnector.CloseConnection();
                return;
            }

            // Update the balance with the withdrawn amount
            int newBalance = currentBalance - amount;
            string updateBalanceQuery = $"UPDATE accounts SET Balance = {newBalance} WHERE Account_number = {accountNumber}";
            MySqlCommand updateBalanceCmd = databaseConnector.CreateCommand(updateBalanceQuery);
            updateBalanceCmd.ExecuteNonQuery();

            MessageBox.Show($"Withdrawn amount: {amount}. New balance: {newBalance}");

            databaseConnector.CloseConnection();
        }

        public void Deposit(int amount)
        {
            databaseConnector.OpenConnection();

            int accountNumber = Form1.user_id;

            // Check the current balance
            string checkBalanceQuery = $"SELECT Balance FROM accounts WHERE Account_number = {accountNumber}";
            MySqlCommand checkBalanceCmd = databaseConnector.CreateCommand(checkBalanceQuery);
            int currentBalance = Convert.ToInt32(checkBalanceCmd.ExecuteScalar());

            // Update the balance with the deposited amount
            int newBalance = currentBalance + amount;
            string updateBalanceQuery = $"UPDATE accounts SET Balance = {newBalance} WHERE Account_number = {accountNumber}";
            MySqlCommand updateBalanceCmd = databaseConnector.CreateCommand(updateBalanceQuery);
            updateBalanceCmd.ExecuteNonQuery();

            MessageBox.Show($"Deposited amount: {amount}. New balance: {newBalance}");

            databaseConnector.CloseConnection();
        }
    }
}
