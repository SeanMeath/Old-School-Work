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

// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================

namespace Budget
{

    public partial class MainWindow : Window
    {
        // ====================================================================
        // global vars
        // ====================================================================
        HomeBudget budget;
        Boolean savedStatus;
        DateTime lastUsedDate = DateTime.Now;

        // ====================================================================
        // setup the gui
        // ====================================================================
        public MainWindow()
        {
            InitializeComponent();
            SetBudgetRequiredControls(false);
        }

        #region Data Grid Stuff


        // ====================================================================
        // Update the Data Grid View
        // ====================================================================
        private void UpdateDataGridView()
        {
            // don't do anything if budget is not yest defined
            if (budget == null)
            {
                return;

            }
            // filter options
            bool FilterFlag = cbFilterCategories.IsChecked == true;
            int id = -1;
            if (cmbCategories.SelectedItem != null)
            {
                id = ((Category)cmbCategories.SelectedItem).Id;
            }

            // ----------------------------------------------------------------
            // standard display
            // ----------------------------------------------------------------
            if (cbByCategory.IsChecked != true && cbByMonth.IsChecked != true)
            {
                // create columns for standard display
                CreateDataGridStandardDisplay();

                // set the ItemSource
                dataBudget.ItemsSource = null;
                dataBudget.ItemsSource = budget.GetExpenses(dpStartDate.SelectedDate, dpEndDate.SelectedDate, FilterFlag, id);
            }
            // ----------------------------------------------------------------
            // total by month
            // ----------------------------------------------------------------
            if (cbByCategory.IsChecked != true && cbByMonth.IsChecked == true)
            {
                // create columns for by Month display
                CreateDataGridByMonthDisplay();

                // set the ItemSource
                dataBudget.ItemsSource = null;
                dataBudget.ItemsSource = budget.GetExpensesByMonth(dpStartDate.SelectedDate, dpEndDate.SelectedDate, FilterFlag, id);
            }

            // ----------------------------------------------------------------
            // total by category
            // ----------------------------------------------------------------
            if (cbByCategory.IsChecked == true && cbByMonth.IsChecked != true)
            {
                // create columns for by Category display
                CreateDataGridByCategoryDisplay();

                // set the ItemSource
                dataBudget.ItemsSource = null;
                dataBudget.ItemsSource = budget.GetExpensesByCategory(dpStartDate.SelectedDate, dpEndDate.SelectedDate, FilterFlag, id);
            }

            // ----------------------------------------------------------------
            // total by category and month
            // ----------------------------------------------------------------
            if (cbByCategory.IsChecked == true && cbByMonth.IsChecked == true)
            {
                // create columns for by Category display
                CreateDataGridByCategoryAndMonthDisplay();

                // set the ItemSource
                dataBudget.ItemsSource = null;
                dataBudget.ItemsSource = budget.GetExpensesByCategoryAndMonth(dpStartDate.SelectedDate, dpEndDate.SelectedDate, FilterFlag, id);
            }


        }

        // ====================================================================
        // Create the Columns for the DataGrid Standard Display mode
        // ====================================================================
        private void CreateDataGridStandardDisplay()
        {
            // create columns to display
            dataBudget.Columns.Clear();
            var col = new DataGridTextColumn();
            col.Header = "Date";
            col.Binding = new Binding("Date");
            col.Binding.StringFormat = "dd/MM/yyyy";
            dataBudget.Columns.Add(col);

            col = new DataGridTextColumn();
            col.Header = "Category";
            col.Binding = new Binding("Category");
            dataBudget.Columns.Add(col);

            col = new DataGridTextColumn();
            col.Header = "Description";
            col.Binding = new Binding("ShortDescription");
            dataBudget.Columns.Add(col);

            col = new DataGridTextColumn();
            col.Header = "Amount";
            col.Binding = new Binding("Amount");
            col.Binding.StringFormat = "F2";
            Style s = new Style();
            s.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Right));
            col.CellStyle = s;
            dataBudget.Columns.Add(col);

            col = new DataGridTextColumn();
            col.Header = "Balance";
            col.Binding = new Binding("Balance");
            col.Binding.StringFormat = "F2";
            col.CellStyle = s;
            dataBudget.Columns.Add(col);

        }

        // ====================================================================
        // Create the Columns for the DataGrid by Month Display mode
        // ====================================================================
        private void CreateDataGridByMonthDisplay()
        {
            Style s = new Style();
            s.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Right));

            // create columns to display
            dataBudget.Columns.Clear();
            var col = new DataGridTextColumn();
            col.Header = "Month";
            col.Binding = new Binding("Month");
            dataBudget.Columns.Add(col);

            col = new DataGridTextColumn();
            col.Header = "Total";
            col.Binding = new Binding("Total");
            col.Binding.StringFormat = "F2";
            col.CellStyle = s;
            dataBudget.Columns.Add(col);

        }

        // ====================================================================
        // Create the Columns for the DataGrid by Category Display mode
        // ====================================================================
        private void CreateDataGridByCategoryDisplay()
        {
            Style s = new Style();
            s.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Right));

            dataBudget.Columns.Clear();

            // create columns to display
            var col = new DataGridTextColumn();
            col.Header = "Category";
            col.Binding = new Binding("Category");
            dataBudget.Columns.Add(col);

            col = new DataGridTextColumn();
            col.Header = "Total";
            col.Binding = new Binding("Total");
            col.Binding.StringFormat = "F2";
            col.CellStyle = s;
            dataBudget.Columns.Add(col);

        }
        // ====================================================================
        // Create the Columns for the DataGrid by Category And Month Display mode
        // ====================================================================
        private void CreateDataGridByCategoryAndMonthDisplay()
        {
            Style s = new Style();
            s.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Right));
            dataBudget.Columns.Clear();

            var col1 = new DataGridTextColumn();
            col1.Header = "Month";
            col1.Binding = new Binding("Month");
            dataBudget.Columns.Add(col1);

            foreach (Category c in budget.categories.List().OrderBy(c => c.Description))
            {
                var col = new DataGridTextColumn();
                col.Header = c.Description;
                col.Binding = new Binding(c.Description);
                col.Binding.StringFormat = "F2";
                col.CellStyle = s;
                dataBudget.Columns.Add(col);
            }
            var col2 = new DataGridTextColumn();
            col2.Header = "Total";
            col2.Binding = new Binding("Total");
            col2.Binding.StringFormat = "F2";
            col2.CellStyle = s;
            dataBudget.Columns.Add(col2);

        }
        #endregion

        #region setting controls and stuff
        // ====================================================================
        // Set the controls (enabled/disabled) that require a 
        // budget object to be defined before these controls can be used
        // and sets the menu buttons to be enabled or disabled
        // ====================================================================
        private void SetBudgetRequiredControls(bool isDirty)
        {
            // saved status is opposite of dirty
            savedStatus = !isDirty;

            // can only add an expense if budget is defined
            if (budget != null)
            {
                btnAddExpense.IsEnabled = true;
                toolAdd.IsEnabled = true;
                toolAdd.Opacity = 1;
                menuModify.IsEnabled = true;
                toolModify.IsEnabled = true;
                toolModify.Opacity = 1;
            }
            else
            {
                btnAddExpense.IsEnabled = false;
                toolAdd.IsEnabled = false;
                toolAdd.Opacity = .25;
                menuModify.IsEnabled = false;
                toolModify.IsEnabled = false;
                toolModify.Opacity = .25;
            }
            

            // set info on status bar
            if (savedStatus == true)
            {
                txtSavedStatus.Text = "Saved";
            }
            else if (savedStatus == false)
            {
                txtSavedStatus.Text = "Not Saved";
            }

            // -----------------------------------------------------------
            // if budget is defined, then enable/disable certain controls
            // AND update the categories list
            if (budget != null)
            {
                txbFileName.Text = budget.PathName;
            }

            // -----------------------------------------------------------
            // menu and taskbar update
            //Added the ToolBar buttons to be Enabled/Dissabled and change the opacity as needed
            if (budget != null && budget.FileName != null && savedStatus != true)
            {
                menuSave.IsEnabled = true;
                toolSave.IsEnabled = true;
                toolSave.Opacity = 1;
                menuSaveAs.IsEnabled = true;
                toolSaveAs.IsEnabled = true;
                toolSaveAs.Opacity = 1;

            }
            else
            {
                menuSave.IsEnabled = false;
                toolSave.IsEnabled = false;
                toolSave.Opacity = .25;

                if (budget != null)
                {
                    menuSaveAs.IsEnabled = true;
                    toolSaveAs.IsEnabled = true;
                    toolSaveAs.Opacity = 1;
                }
                else
                {
                    menuSaveAs.IsEnabled = false;
                    toolSaveAs.IsEnabled = false;
                    toolSaveAs.Opacity = .25;
                }
            }
        }

        // ====================================================================
        // set categories
        // ====================================================================
        private void SetCategories()
        {
            cmbCategories.ItemsSource = null;
            cmbCategories.ItemsSource = budget.categories.List().OrderBy(c => c.Description);
        }
        #endregion

        #region New / Open
        // ====================================================================
        // New budget
        // ====================================================================
        private void menuNew_Click(object sender, RoutedEventArgs e)
        {
            // Always use a try catch, just in case
            try
            {
                // bail out if we do have not saved changed changes, and do not
                // want to continue
                if (savedStatus == false && ContinueAndLoseChanges() == false)
                {
                    return;
                }

                // create a new budget
                budget = new HomeBudget();
                UpdateDataGridView();

                // update controls
                SetBudgetRequiredControls(false);

                // reset the categories
                SetCategories();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Creating New Budget", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ====================================================================
        // Open budget
        // ====================================================================
        private void menuOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // bail out if we have not saved changes, and do not
                // want to continue
                if (savedStatus == false && ContinueAndLoseChanges() == false)
                {
                    return;
                }

                // open file dialog box
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Budget Files|*.budget|All Files|*.*";

                // if user selects a file to read
                if (openFileDialog.ShowDialog() == true)
                {
                    // create a new budget
                    budget = new HomeBudget(openFileDialog.FileName);
                    UpdateDataGridView();

                    // update controls
                    SetBudgetRequiredControls(false);

                    // reset the categories
                    SetCategories();
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Openning Budget", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        #endregion

        #region Save and SaveAs

        // ====================================================================
        // Save
        // ====================================================================
        private void menuSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // if we have a pathname, save, else call SaveAs
                if (budget.PathName != null)
                {
                    budget.SaveToFile(budget.PathName);
                    SetBudgetRequiredControls(false);
                }

                else
                {
                    menuSaveAs_Click(new object(), new RoutedEventArgs());
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Saving Budget", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }


        // ====================================================================
        // Save As
        // ====================================================================
        private void menuSaveAs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Budget Files|*.budget|All Files|*.*";

                // if user selects a file to save text to
                if (saveFileDialog.ShowDialog() == true)
                {
                    budget.SaveToFile(saveFileDialog.FileName);
                }

                // update Controls
                SetBudgetRequiredControls(false);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Saving Budget", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        // ====================================================================
        // Do we continue and lose existing changes
        // ====================================================================
        private bool ContinueAndLoseChanges()
        {
            MessageBoxResult r = MessageBox.Show("Current Budget is not saved\nOpening a new one will erase all changes\nDo you want to Continue?", "Budget not saved", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No);
            return r == MessageBoxResult.Yes;
        }


        #endregion

        #region closing the app

        // ====================================================================
        // Are we sure we want to close this window?
        // ====================================================================
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (savedStatus == false)
            {
                e.Cancel = !ContinueAndLoseChanges();
            }
            else
            {
                System.Windows.Application.Current.Shutdown();
            }
        }

        // ====================================================================
        // Exit the program
        // ====================================================================
        private void menuExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion


        #region event handlers for requiring DataGridView refresh 
        // ====================================================================
        // Refresh DataGrid because display options have changed
        // ====================================================================
        private void cmbCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbFilterCategories.IsChecked == true)
            {
                UpdateDataGridView();
            }
        }

        private void cbFilterCategories_Click(object sender, RoutedEventArgs e)
        {
            UpdateDataGridView();
        }

        private void dpStartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDataGridView();
        }

        private void dpEndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDataGridView();
        }

        private void cbByMonth_Click(object sender, RoutedEventArgs e)
        {
            UpdateDataGridView();
        }

        private void cbByCategory_Click(object sender, RoutedEventArgs e)
        {
            UpdateDataGridView();
        }
        #endregion

        #region add expense
        // ====================================================================
        // Add Expense
        // ====================================================================
        private void btnAddExpense_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Creates the expense form and sets the budget to the current budget , the theme to add expense and the last used date
                ExpenseForm expenses = new ExpenseForm();
                expenses.Budget = budget;
                expenses.Theme = Themes.Add;
                expenses.Day = lastUsedDate;
                //Opens the window
                expenses.ShowDialog();
                lastUsedDate = (DateTime)expenses.txtDate.SelectedDate;
                UpdateDataGridView();
                // Update budget required controls
                SetBudgetRequiredControls(true);

                // set the focus on the last element of the budget
                ResetFocusAfterUpdate(dataBudget.Items.Count - 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Adding Expense", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region modify expense
        private void menuUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Creates the expense form , sets the budget to the current budget , the theme to modify expense and the expense id to the selected item's id
                ExpenseForm expenses = new ExpenseForm();
                expenses.Budget = budget;
                expenses.Theme = Themes.Modify;
                BudgetItem bi = dataBudget.SelectedItem as BudgetItem;
                expenses.ExpenseID = bi.ExpenseID;
                //Opens the window
                expenses.ShowDialog();
                UpdateDataGridView();
                // Update budget required controls
                SetBudgetRequiredControls(true);

                // set the focus on the last element of the budget
                ResetFocusAfterUpdate(dataBudget.Items.Count - 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Adding Expense", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region ResetFocusAfterUpdate 

        // ==========================================================================
        // call this after modifying or adding an expense, it sets up the focus
        // of the datagrid
        // ==========================================================================
        private void ResetFocusAfterUpdate( int index)
        {
            try
            {
                // set the index of what we want to be selected, make sure that 
                // it is not larger than the number of items
                if (index >= dataBudget.Items.Count)
                {
                    index = dataBudget.Items.Count - 1;
                }

                // set the selected index, and then set focus onto the datagrid
                dataBudget.SelectedIndex = index;
                dataBudget.Focus();

                // if we have a "real" selected index, set the current cell (puts the 
                // focus truly on that row)
                if (index != -1)
                {
                    dataBudget.CurrentCell = new DataGridCellInfo(dataBudget.Items[index], dataBudget.Columns[0]);
                }
            }
            catch
           {
            }

        }
        #endregion

    }

}

