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

namespace Exercise

{
    // ==================================================================================
    // Workout class
    // ==================================================================================
    public class Workout
    {
        // properties
        public double Calories { get; set; }
        public String Exercise { get; set; }
        public String Description { get; set; }

        // constructor
        public Workout(double workout, string exercise, string description)
        {
            Calories = workout;
            Exercise = exercise;
            Description = description;
        }

        // override to string propery
        public override string ToString()
        {
            return this.Exercise;
        }
    }

    // ==================================================================================
    // MAIN WINDOW
    // ==================================================================================
    public partial class MainWindow : Window
    {
        // ------------------------------------------------------------------------------
        // global variables
        // ------------------------------------------------------------------------------
        double total = 0;

        // ------------------------------------------------------------------------------
        // Main Window Constructor
        // ------------------------------------------------------------------------------
        public MainWindow()
        {
            InitializeComponent();

            // populate our vending machine with some yummy food
            cmbExercise.Items.Add(new Workout(150, "1/2 Hour Jog", "Moderate speed on flat surface"));
            cmbExercise.Items.Add(new Workout(300, "Tae Kwon Do", "1 1/2 class - cardio and stretching"));
            cmbExercise.Items.Add(new Workout(200, "Yoga", "Mostly stretching"));
            cmbExercise.Items.Add(new Workout(900, "WPF", "Writing Sandy's test is hard work!"));
            cmbExercise.Items.Add(new Workout(30, "Watching TV", "Star Wars - Yeah!"));

            // start with no selection
            cmbExercise.SelectedIndex = -1;
        }

        // ---------------------------------------------------------------------------
        // TODO:
        // selection has changed, so update the exercise/description/calories information
        // for this exercise
        // ---------------------------------------------------------------------------
        // NOTE: <TextBlock> names are: 
        //       txtDescription
        //       txtExercise
        //       txtCalories
        // ---------------------------------------------------------------------------
        private void cmbExercise_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // NOTE: USE ONLY IF YOU CAN"T GET THE COMBO BOX TO GIVE YOU WHAT YOU NEED - USE THIS AND CARRY ON!
            //       Workout wo = new Workout(111,"Stuff","I couldn't get the combobox to work");
            if (cmbExercise.SelectedIndex != -1)
            {
                Workout selected = cmbExercise.SelectedItem as Workout;
                txtCalories.Text = selected.Calories.ToString();
                txtDescription.Text = selected.Description;
                txtExercise.Text = selected.Exercise;
            }
        }


        // ---------------------------------------------------------------------------
        // TODO:
        // User has accepted their choice, so 
        // 1) add the calories of their selection to the total calories burned
        // 2) Display new total (txtTotalCalories)
        // 3) clear the selection
        // 4) clear information about the selection
        // ---------------------------------------------------------------------------
        private void btnAcceptCurrentSelection_Click(object sender, RoutedEventArgs e)
        {
            Workout selected = null;
            if (cmbExercise.SelectedIndex != -1)
            {
                selected = cmbExercise.SelectedItem as Workout;
                double sum = double.Parse(txtTotalCalories.Text);
                sum += selected.Calories;
                txtTotalCalories.Text = sum.ToString();
                cmbExercise.SelectedIndex = -1;
                txtCalories.Text = "";
                txtDescription.Text = "";
                txtExercise.Text = "";
            }
        }

        // ---------------------------------------------------------------------------
        // TODO:
        // User has cancelled their choice, so 
        // 1) clear the selection 
        // 2) clear information about the selection
        // ---------------------------------------------------------------------------
        private void btnCancelCurrentSelection_Click(object sender, RoutedEventArgs e)
        {
            if (cmbExercise.SelectedIndex != -1)
            {
                cmbExercise.SelectedIndex = -1;
                txtCalories.Text = "";
                txtDescription.Text = "";
                txtExercise.Text = "";
            }
        }

        // ---------------------------------------------------------------------------
        // TODO:
        // User has submitted their calories, 
        // 1) pop up a Message box, saying "You have burned xx calories. Congratulations!"
        // 2) clear total
        // 3) update display
        // ---------------------------------------------------------------------------
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Congradulations you have burned " + txtTotalCalories.Text + " calories in this workout.");
            cmbExercise.SelectedIndex = -1;
            txtCalories.Text = "";
            txtDescription.Text = "";
            txtExercise.Text = "";
            txtTotalCalories.Text = "";
        }

        // ---------------------------------------------------------------------------
        // Cancel submission, reset total to zero
        // ---------------------------------------------------------------------------
        private void btnSubmitCancel_Click(object sender, RoutedEventArgs e)
        {
            total = 0;
            txtTotalCalories.Text = "0";
        }

    }
}
