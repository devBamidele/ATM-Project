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
    public partial class TransOptions : Form
    {

        public class BalanceChecker 
        {
            private DatabaseConnector databaseConnector;

            public BalanceChecker()
            {
                databaseConnector = new DatabaseConnector();
            }


            public void checkBalance()
            {
                databaseConnector.OpenConnection();

                int accNo = Form1.user_id;

                string checkQuery = $"SELECT Balance FROM accounts WHERE Account_Number = {accNo}";

                MySqlCommand cmd = databaseConnector.CreateCommand(checkQuery);
                object result = cmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    int balance = Convert.ToInt32(result);
                    string report = $"Your Account Balance is {balance}";
                    MessageBox.Show(report);
                }
                else
                {
                    MessageBox.Show("Balance not found for the account");
                }

                databaseConnector.CloseConnection();
            }

        }

        public TransOptions()
        {
            InitializeComponent();
        }

        private void TransOptions_Load(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            handleButton1Click();
        }

        private void handleButton1Click()
        {
            this.Hide();
            Withdraw withdraw = new Withdraw();
            withdraw.Show();
        }

        private void handleButton2Click()
        {
            BalanceChecker balanceChecker = new BalanceChecker();
            balanceChecker.checkBalance();
        }


        private void button3_Click(object sender, EventArgs e)
        {
            handleButton2Click();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 login = new Form1();
            login.Show();
        }
    }
}
