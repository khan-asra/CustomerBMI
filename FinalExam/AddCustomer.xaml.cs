using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
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
    /// Add Customer page will take users input and add the data to the database.
    /// </summary>
    public sealed partial class AddCustomer : Page
    {
        SqlConnection sqlConnection;
        SqlCommand sqlCommand;
        string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=CustomerDB;Integrated Security=True";
        public AddCustomer()
        {
            this.InitializeComponent();
        }
        /// <summary>
        /// On Click validate the data and add it into the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnClickAdd(object sender, RoutedEventArgs e)
        {
            int age;
            int.TryParse(txtAge.Text, out age);
            if (Validation())
            {
                var dialog1 = new MessageDialog("Please check your information!\n" +
                    "Integer value for ID, Age, Height\n" +
                    "Double value for weight\n" +
                    "Age should be greater than 18." +
                    "");
                     await dialog1.ShowAsync();
            }
            else
            {
                try
                {
                    using (sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        if (sqlConnection.State == System.Data.ConnectionState.Open)
                        {                
                                sqlCommand = new SqlCommand("Insert into Customers (ID,FullName,Age,Weight,Height) VALUES(" + Convert.ToInt32(txtID.Text) + " ,'" + txtFirstName.Text + " " + txtLastName.Text + "'," + Convert.ToInt32(txtAge.Text) + "  , " + Convert.ToDouble(txtWeight.Text) + "  , " + Convert.ToInt32(txtHeight.Text) + " )", sqlConnection);
                                sqlCommand.ExecuteNonQuery();
                                var dialog4 = new MessageDialog(" Customer added to the database !");
                                await dialog4.ShowAsync();
                                Clear();
                        }
                    }
                }
                catch (Exception )
                {
                    var dialog4 = new MessageDialog(" Customer already exist  !");
                    await dialog4.ShowAsync();
                }
            }
        }
        /// <summary>
        /// On click navigate to view customer page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClickBack(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ViewCustomers));
        }

        /// <summary>
        /// validation method is use to validate all the values entered by the user
        /// </summary>
        /// <returns></returns>
        public bool Validation()
        {
            int age;
            double weight;
            int height;
            if (string.IsNullOrEmpty(txtID.Text) || string.IsNullOrEmpty(txtFirstName.Text) || string.IsNullOrEmpty(txtLastName.Text)
                || string.IsNullOrEmpty(txtWeight.Text) || string.IsNullOrEmpty(txtHeight.Text) || string.IsNullOrEmpty(txtAge.Text))
            {

                return true;
            }
            if (!int.TryParse(txtID.Text, out int id) || !int.TryParse(txtAge.Text, out age) || !int.TryParse(txtHeight.Text, out  height) || !double.TryParse(txtWeight.Text, out  weight )|| height<0 || weight<0 )
            {
                return true;
            }
            if(age<18)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// will clear all the text boxes
        /// </summary>
        public void Clear()
        {
            txtID.Text = string.Empty;
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtHeight.Text = string.Empty;
            txtWeight.Text = string.Empty;
            txtAge.Text = string.Empty;
        }
    }
    

    
}
