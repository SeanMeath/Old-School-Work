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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;

namespace Test3
{
    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // Test 3
    //
    // Part 2: Finish the code for copying one file from one place to another
    //         a) User clicks on the "btnOriginalFile" to select which file to copy
    //            i) use OpenFileDialog, use filter option properly
    //            ii) the name of this file is entered into the tbOriginalFile TextBox
    //            iii) clear txtStatus (set text to an empty string)
    //         b) User clicks on the "btnSaveToFile" to select which file they want
    //            to copy to
    //            i) use OpenFileDialog, use filter option properly
    //            ii) the name of this file is entered into the tbSaveToFile TextBox
    //            iii) clear txtStatus (set text to an empty string)
    //         c) User clicks the "btn_CopyFile" and
    //            i) if the btnOriginalFile and btnSaveToFile are not empty then
    //               1) Read the original file
    //               2) Write what you read to the saveto file
    //               3) txtStatus text says "Successfully copied"
    //            ii) else
    //               1) MessageBox... display appropriate message
    //
    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //flags to verify the filepaths are filled
        private bool ofFlag = false;
        private bool sfFlag = false;

        // ==================================================================
        // Select "original File" event handler
        // ==================================================================
        private void btnOriginalFile_Click(object sender, RoutedEventArgs e)
        {
            //creates the openfiledialog box
            OpenFileDialog of = new OpenFileDialog();
            //sets the filters of said box
            of.Filter = "Text Files|*.txt|All Files|*.*";
            //opens and checks to see if a file was chosen
            if (of.ShowDialog() == true)
            {
                //sets the tb in main's text to the filepath
                tbOriginalFile.Text = of.FileName;
            }
            //resets the status bar
            txtStatus.Text = "";
            //sets the flag
            ofFlag = true;
        }

        // ==================================================================
        // Select "SaveTo File" event handler
        // ==================================================================
        private void btnSaveToFile_Click(object sender, RoutedEventArgs e)
        {
            //creates a savefiledialog box
            SaveFileDialog sf = new SaveFileDialog();
            //sets the filter of said box
            sf.Filter = "Text Files|*.txt|All Files|*.*";
            //opens and checks to see if a file was chosen
            if (sf.ShowDialog() == true)
            {
                //sets the tb in main's text to the filepath
                tbSaveToFile.Text = sf.FileName;
            }
            //resets the status bar
            txtStatus.Text = "";
            //sets the flag
            sfFlag = true;
        }

        // ==================================================================
        // "Now copy files" event handler
        // ==================================================================
        private void btnCopyFile_Click(object sender, RoutedEventArgs e)
        {
            //Verifies to check if both boxes have a filepath designated to them
            if (ofFlag && sfFlag)
            {
                //Writes the text from one file to another
                File.WriteAllText(tbSaveToFile.Text, File.ReadAllText(tbOriginalFile.Text));
                //Sets the status to Successfull
                txtStatus.Text = "Successfully Copied";
            }
            else
            {
                //Displays an error message
                MessageBox.Show("Error! Please verify that the files chosen are not corrupted and exist");
            }
        }


        // ==================================================================
        // Part 3 - Adopt a pet - see ChooseAPetForm for instructions
        // ==================================================================
        private void btnAdopt_Click(object sender, RoutedEventArgs e)
        {
            ChooseAPetForm capf = new ChooseAPetForm();
            capf.ShowDialog();
        }

    }
}
