using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyManager
{
    public partial class Form2 : Form
    {
        public string searchRes;
        public Form2()
        {
            InitializeComponent();
            searchRes = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 main = this.Owner as Form1;
            if (main != null) { searchRes = textBox1.Text; }
            Close();
        }
    }
}
