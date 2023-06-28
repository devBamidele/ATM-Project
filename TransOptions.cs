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
            TransOptions transOptions = new TransOptions();
            transOptions.Hide();
            Withdraw withdraw = new Withdraw();
            withdraw.Show();
        }

        private void handleButton2Click()
        {
            MessageBox.Show("Hello World");
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
