using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FinalExam
{
    /// <summary>
    /// A Delete customer page will filter data.The data can be selected and deleted.
    /// </summary>
    public sealed partial class DeleteCustomer : Page
    {

        /// <summary>
        /// fields
        /// </summary>
        SqlConnection sqlConnection;
        SqlCommand sqlCommand;
        string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=CustomerDB;Integrated Security=True";
        SqlDataReader dreader;
        Customer selectedItem = new Customer();
        ObservableCollection<Customer> list = new ObservableCollection<Customer>();
        public DeleteCustomer()
        {
            this.InitializeComponent();
            CustomerList.ItemsSource = list;
            LoadCustomers();
        }
        /// <summary>
        /// LoadCustomers method will reads data fromthe database and 
        /// load it on the listview.
        /// 
        /// </summary>
        private void LoadCustomers()
        {
            string selectCommand = "SELECT * from Customers ORDER BY ID";
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            sqlCommand = new SqlCommand(selectCommand, sqlConnection);
            list.Clear();
            dreader = sqlCommand.ExecuteReader();
            while (dreader.Read())
            {
                Customer customer = new Customer();
                customer.ID = dreader.GetInt32(0);
                string[] fullname = dreader.GetString(1).Split(" ");
                customer.FirstName = fullname[0];
                customer.LastName = fullname[1];
                customer.Age = dreader.GetInt32(2);
                customer.Weight = dreader.GetDouble(3);
                customer.Height = dreader.GetInt32(4);
                list.Add(customer);
            }
        }

        /// <summary>
        /// Will fill the text fields with user selected data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ViewSelection(object sender, SelectionChangedEventArgs e)
        {
            try 
            {
                if (selectedItem != null)
                {
                    txtFName.Text = selectedItem.FirstName;
                    txtLName.Text = selectedItem.LastName;
                }
            }
            catch (System.NullReferenceException ex)
            {
                var dialog4 = new MessageDialog(ex.Message);
                await dialog4.ShowAsync();
            }
        }
        /// <summary>
        /// OnClickAddCustomer will take it back to the add customer page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClickAddCustomer(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddCustomer));
        }
        /// <summary>
        /// OnClickDelete when the item is selected. The selected customer will be deleted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnClickDelete(object sender, RoutedEventArgs e)
        {
           // string name = selectedItem.FirstName;
            using (sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                if (sqlConnection.State == System.Data.ConnectionState.Open)
                {
                    try
                    {
                        if (selectedItem != null)
                        {
                            sqlCommand = new SqlCommand("Delete  from Customers where FullName = '" + selectedItem.FirstName + " " + selectedItem.LastName + "'", sqlConnection);
                            sqlCommand.ExecuteNonQuery();

                            var dialog4 = new MessageDialog(" Customer Deleted from the database !");
                            await dialog4.ShowAsync();
                        }
                        else
                        {
                            var dialog5 = new MessageDialog(" An error occured.  !");
                            await dialog5.ShowAsync();
                        }
                    }
                    catch(System.NullReferenceException ex)
                    {
                        var dialog5 = new MessageDialog(" An error occured please try again !");
                        await dialog5.ShowAsync();
                    }
                }
                list.Clear();
                LoadCustomers();
            }
            txtFName.Text = String.Empty;
            txtLName.Text = String.Empty;
          
        }
         /// <summary>
         /// This method will navigate to the View Customer page
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
        private void OnClickViewCustomer(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ViewCustomers));
        }
        /// <summary>
        /// Filter the data accoring to the user input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void filterTxt_textChanged(object sender, TextChangedEventArgs e)
        {
            ObservableCollection<Customer> FilterList = new ObservableCollection<Customer>();
            var textSelectedFilter = list.Where(customer => Filtered(customer));
            foreach( var customer in textSelectedFilter)
            {
                FilterList.Add(customer);
                selectedItem = customer;
            }
            CustomerList.ItemsSource = FilterList;
        }
        /// <summary>
        /// will return true false if the constion is true.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        private bool Filtered(Customer customer)
        {
          return customer.FirstName.Contains(txtFName.Text, StringComparison.InvariantCultureIgnoreCase) &&
                    customer.LastName.Contains(txtLName.Text, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
