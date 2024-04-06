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
    
    public partial class MaintenanceChecked : Window
    {
        public MaintenanceChecked()
        {
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDownDashboard(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }


        public static NpgsqlConnection con;
        public static NpgsqlCommand cmd;

        private string get_ConnectingString()
        {
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
                con = new NpgsqlConnection(get_ConnectingString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void TextBox_GotFocus1(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "Aircraft code")
            {
                textBox.Text = string.Empty;
            }
        }

        private void buttonMaintenanceClose_Button(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void searchMaintenanceButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                establishConnect();

                con.Open();

                string query = "SELECT * FROM maintenance WHERE aircraft = @aircraft";
                cmd = new NpgsqlCommand(query, con);

                string aircraftCode = aircraftCodeChecked.Text.Trim();
                if (!string.IsNullOrEmpty(aircraftCode))
                {
                    cmd.Parameters.AddWithValue("@aircraft", aircraftCode);

                    using (NpgsqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            ProblemChecked.Text = dr["problem"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("No data found for the aircraft code.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Aircraft code cannot be empty. Please enter a valid code.");
                }

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void checkMaintenanceButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                establishConnect();

                con.Open();

                string Query = "DELETE FROM maintenance WHERE aircraft = @Id";

                cmd = new NpgsqlCommand(Query, con);

                cmd.Parameters.AddWithValue("@Id", aircraftCodeChecked.Text);

                int rowsAffected = cmd.ExecuteNonQuery();

               
                con.Close();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Aircraft checked successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    aircraftCodeChecked.Text = string.Empty;
                    ProblemChecked.Text = string.Empty;

                    this.Close();

                }
                else
                {
                    MessageBox.Show("Aircraft code not found or deletion failed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
