using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Test3
{

    public partial class PetDetailForm : Window
    {
        //Backing Field for the buttons to send back
        private bool _adopted;

        //Property to send back to the other windows
        public bool Adopted
        {
            get { return _adopted; }
        }


        public PetDetailForm()
        {
            InitializeComponent();
            
        }

        //Sets the Adopted property to true and closes the window
        private void btnAdopt_Click(object sender, RoutedEventArgs e)
        {
            _adopted = true;
            this.Close();
        }

        //Sets the Adopted property to false and closes the window
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            _adopted = false;
            this.Close();
        }
    }
}
