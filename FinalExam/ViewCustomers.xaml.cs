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
    /// This page shows customer list. If you click on a customer name it pull the information of the 
    /// customer and display 
    /// </summary>
    public sealed partial class ViewCustomers : Page
    {
        SqlConnection sqlConnection;
        SqlCommand sqlCommand;
        string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=CustomerDB;Integrated Security=True";
        SqlDataReader dreader;
        Customer selectedItem = new Customer();
        ObservableCollection<Customer> list = new ObservableCollection<Customer>();
        /// <summary>
        /// CustomerList will be uploaded as the page is loaded, and the listView is binded
        /// </summary>
        public ViewCustomers()
        {
            this.InitializeComponent();
            CustomerList.ItemsSource = list;
            LoadCustomers();
        }
        /// <summary>
        /// LoadCustomer method will select all the customer from the database.
        /// It will reader the data from the Database 
        /// </summary>
            private void LoadCustomers()
            {
                string selectCommand = "SELECT * from Customers ORDER BY id";
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                sqlCommand = new SqlCommand(selectCommand, sqlConnection);
                list.Clear();
                dreader = sqlCommand.ExecuteReader();
                while(dreader.Read())
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
        /// View Selection method will selected Item and the text boxes will display the item selected. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
            private void ViewSelection(object sender, SelectionChangedEventArgs e)
            {
                txtFName.Text = selectedItem.FirstName;
                txtLName.Text = selectedItem.LastName;
                txtAge.Text = selectedItem.Age.ToString();
                txtWeight.Text = selectedItem.Weight.ToString();
                txtHeight.Text = selectedItem.Height.ToString();
                txtBMI.Text = Math.Round(selectedItem.CalculateBMI(),2).ToString();
            }
        }
}

