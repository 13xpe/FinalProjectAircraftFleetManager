using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace FinalProjectAircraftFleetManager
{
    /// <summary>
    /// Interaction logic for DeleteAircraft.xaml
    /// </summary>
    public partial class DeleteAircraft : Window
    {
        public DeleteAircraft()
        {
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDownDeleteDelete(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        public static NpgsqlConnection con;
        public static NpgsqlCommand cmd;

        private string Get_ConnectionString()
        {
            //Connect with database to get the inFlight aircraft
            string host = "Host=localhost;";
            string port = "Port=5432;";
            string dbName = "Database=FleetManagementApp;";
            string userName = "Username=postgres;";
            string password = "Password=atuaprima;";

            string connectionString = string.Format("{0}{1}{2}{3}{4}", host, port, dbName, userName, password);
            return connectionString;
        }

        private void establishConnect()
        {
            try
            {
                con = new NpgsqlConnection(Get_ConnectionString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void searchDeleteAircraftButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Establish connection
                establishConnect();

                // Open connection
                con.Open();

                // Create query with flight ID as a parameter
                string Query = "SELECT * FROM groundaircraft WHERE flight = @Id";

                // Create command
                cmd = new NpgsqlCommand(Query, con);

                // Initialize the query variable as a string parameter
                cmd.Parameters.AddWithValue("@Id", deleteAircraftFlight.Text);

                // Data Reader adapter
                NpgsqlDataReader dr = cmd.ExecuteReader();

                // Check if data exists
                if (dr.HasRows)
                {
                    dr.Read(); // Read the first row

                    // Display data in TextBoxes
                    flightGroupDelete.Text = dr["supplier"].ToString();
                    aircraftModelDelete.Text = dr["id"].ToString();
                    aircraftOriginDelete.Text = dr["origin"].ToString();
                }
                else
                {
                    MessageBox.Show("Flight ID not found", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                // Close the data reader and connection
                dr.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        
        
        
        private void backDeleteAircraftButton_Click( object sender, RoutedEventArgs e)
        {
            aircraftManagement aircraftManagement = new aircraftManagement();
            aircraftManagement.Show();
            this.Close();
        }

        private void deleteAircraftButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Establish connection
                establishConnect();

                // Open connection
                con.Open();

                // Create delete query
                string Query = "DELETE FROM groundaircraft WHERE flight = @Id";

                // Create command
                cmd = new NpgsqlCommand(Query, con);

                // Initialize the query variable as a string parameter
                cmd.Parameters.AddWithValue("@Id", deleteAircraftFlight.Text);

                // Execute the delete query
                int rowsAffected = cmd.ExecuteNonQuery();

                // Close the connection
                con.Close();

                // Check if any rows were affected (i.e., if the flight was deleted)
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Flight deleted successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Clear TextBoxes or perform any other necessary actions
                    flightGroupDelete.Text = string.Empty;
                    aircraftModelDelete.Text = string.Empty;
                    aircraftOriginDelete.Text = string.Empty;

                }
                else
                {
                    MessageBox.Show("Flight ID not found or deletion failed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TextBox_GotFocus1(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "Aircraft ID")
            {
                textBox.Text = string.Empty;
            }
        }

        private void TextBox_GotFocus2(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "Aircraft group")
            {
                textBox.Text = string.Empty;
            }
        }

        private void TextBox_GotFocus3(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "Aircraft model")
            {
                textBox.Text = string.Empty;
            }
        }

        private void TextBox_GotFocus4(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "Aircraft origin")
            {
                textBox.Text = string.Empty;
            }
        }
    }
}
