/************************************************** ID BLOCK ************************************************************
* Due Date: October 1st, 2018
* Student:              Sean Meath
* Course:               
* Deliverance:          
* Description:          This program calculates the gap 'n' twins between two values
*                       The starting point (odd), ending point as well as the gap (even) are entered in by the user.
*                       The program then loops through the odd numbers (p) checking if each is prime
*                       Once the program finds a prime it sets a switch variable on and increases p by the gap
*                       The program then checkes the new p to see if its prime
*                           -If it is prime then:
*                               -Print both the first p and the second p (gapped 'n' twin primes)
*                               -Reset the switch variable
*                               -Remove the gap from p
*                               -Go to the next p
*                           -If it isn't prime then:
*                               -Check if the switch is activated
*                                   -If it is then
*                                       -Reset Switch
*                                       -Remove gap from p
*                               -Go to the next p
*                       When p exceeds the ending point close
*
*                       Outputs the twin primes between the two points
* 
************************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace meatSeTwins
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {   /************************************************** Declarations **************************************************/
            listBox1.Items.Clear();                                                         //Reset output
            int gap = int.Parse(textBox1.Text);                                             //Get gap
            int lo = int.Parse(textBox2.Text);                                              //Get lower limit
            int hi = int.Parse(textBox3.Text);                                              //Get upper limit
            int p = lo;                                                                     //Starting prime candidate
            int t;                                                                          //Initialize trial devisor
            int f;                                                                          //Initialize prime switch
            int sw = 0;                                                                       //Initialize switch

            do
            {
                t = 3;                                                                      //Reset trial devisor
                f = 1;                                                                      //Reset prime switch
                while (t * t <= p && f == 1)                                                //Trial devisor remain?
                {
                    if (p % t == 0)                                                         //Devisible?
                        f = t;                                                              //Devisor found
                    else
                        t += 2;                                                             //Next trial devisor
                }
                if (f == 1)                                                                 //Prime?
                {
                    if (sw == 1)                                                            //Second prime?
                    {
                        listBox1.Items.Add(p - gap + ", " + p + " Are Prime");              //Output gaped primes
                        p -= gap;                                                           //Remove gap
                        p += 2;                                                             //Next candidate
                        sw = 0;                                                             //Reset switch
                    }
                    else                                                                    //First prime
                    {
                        p += gap;                                                           //Jump gap
                        sw = 1;                                                             //Set switch
                    }
                }
                else                                                                        //Not prime
                {
                    if (sw == 1)                                                            //Second prime?
                    {
                        sw = 0;                                                             //Reset switch
                        p -= gap;                                                           //Remove gap
                    }
                    p += 2;                                                                 //Next Candidate
                }
            }
            while (p <= hi);                                                                //Valid candidate?
        }
    }
}
