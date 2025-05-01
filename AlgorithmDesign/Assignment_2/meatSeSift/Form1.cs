using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace meatSeSift
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {listBox1.Items.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {   listBox1.Items.Clear();
            int gap = int.Parse(textBox1.Text);
            int lo = int.Parse(textBox2.Text);
            int hi = int.Parse(textBox3.Text);

            int p = lo;
            int t;
            int f;

            do
            {   t = 3;
                f = 1;
                while (t*t<=p && f==1)
                {   if (p % t == 0)
                        f = t;
                    else
                        t += 2;
                }
                if (f == 1)
                    listBox1.Items.Add(p);
                p += 2;
            }
            while (p <= hi);
        }
    }
}
