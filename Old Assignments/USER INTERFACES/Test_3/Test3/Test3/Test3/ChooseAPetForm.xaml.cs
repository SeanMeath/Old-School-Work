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
    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // Part 3 : Modify code (ChooseAPetForm and PetDetailForm) 
    //          a) When user clicks an image (it's actually a button)
    //          b) Show the PetDetailForm with all the pet information in the fields
    //          c) If user then clicks "Adopt", close window and 
    //             say "Congratulations - you have adopted (pet name)" in
    //             txtChosenPet textBlock
    //          d) If user clicked Cancel, say "you have not chosen a pet yet" in
    //             txtChosenPet textBlock
    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    // ========================================================
    // enums
    // ========================================================
    public enum Genders { Female, Male };

    // ========================================================
    // Pet Class
    // ========================================================
    public class Pet
    {
        // Properties
        public int Age { get; set; }
        public String Name { get; set; }
        public Genders Gender { get; set; }
        public bool Neutered { get; set; }
        public Image Picture { get; set; }

        // Constructor 
        public Pet(int age, String name, Genders gender, bool neutered, String pictureFile)
        {
            Age = age;
            Name = name;
            Gender = gender;
            Neutered = neutered;
            Picture = new Image();
            Picture.Source = new BitmapImage(new Uri(pictureFile, UriKind.Relative)); 

        }

    }

    // ========================================================
    // Choose a Pet Form
    // ========================================================
    public partial class ChooseAPetForm : Window
    {

        // ========================================================
        // global variables
        // ========================================================
        Pet Pet1 = new Pet(1, "Blue Eyes", Genders.Male, true, "Images/cat1.jpeg");
        Pet Pet2 = new Pet(4, "Gemini", Genders.Female, true, "Images/cat2.jpeg");
        Pet Pet3 = new Pet(2, "Tigger", Genders.Female, false, "Images/cat3.jpeg");
        Pet Pet4 = new Pet(8, "Ghost", Genders.Male, true, "Images/cat4.jpeg");

        // ========================================================
        // constructor
        // ========================================================
        public ChooseAPetForm()
        {
            InitializeComponent();

            // add Pet pictures to buttons
            btnPet1.Content = Pet1.Picture;
            btnPet1.Width = 150;
            btnPet2.Content = Pet2.Picture;
            btnPet2.Width = 150;
            btnPet3.Content = Pet3.Picture;
            btnPet3.Width = 150;
            btnPet4.Content = Pet4.Picture;
            btnPet4.Width = 150;

        }
        
        //The clicked event for all pet buttons

        private void btnPet_Click(object sender, RoutedEventArgs e)
        {
            //creates the form
            PetDetailForm pf = new PetDetailForm();
            //gets the button used
            Button b = sender as Button;
            //creates a pet variable
            Pet p;

            //Checks which button was pressed
            if (b.Name == "btnPet1")
            {
                //sets the pet to the correct one
                p = Pet1;

            }
            else if (b.Name == "btnPet2")
            {
                //sets the pet to the correct one
                p = Pet2;
            }
            else if (b.Name == "btnPet3")
            {
                //sets the pet to the correct one
                p = Pet3;
            }
            else
            {
                //sets the pet to the correct one
                p = Pet4;
            }

            //Sets all the text boxes in the info form to the correct ones depending on the pet chosen
            pf.txtName.Text = p.Name;
            pf.txtAge.Text = p.Age.ToString();
            pf.txtGender.Text = p.Gender.ToString();
            pf.txtNeutered.Text = p.Neutered.ToString();

            //Shows the info form
            pf.ShowDialog();

            //Figures out which button was pressed using the property created
            if (pf.Adopted == true)
            {
                //Display the status of the chosen pet
                txtChosenPet.Text = "Congradulations! - you have adopted " + p.Name;
            }
            else
            {
                //Displays the status of not yet decided
                txtChosenPet.Text = "You have not chosen a pet yet";
            }
        }

        // =====================================================
        // Display details for your pet
        // =====================================================


    }
}
