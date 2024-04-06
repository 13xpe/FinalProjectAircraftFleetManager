using Npgsql;
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

namespace FinalProjectAircraftFleetManager
{
    
    public partial class DeleteAlert : Window
    {
        public DeleteAlert()
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

        private void searchAlertseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                establishConnect();

                
                con.Open();

                string query = "SELECT * FROM alerts WHERE aircraft = @aircraft";
                cmd = new NpgsqlCommand(query, con);

                string aircraftCode = aircraftCodeAlerts.Text.Trim(); 
                if (!string.IsNullOrEmpty(aircraftCode))
                {
                    cmd.Parameters.AddWithValue("@aircraft", aircraftCode);

                    using (NpgsqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            alertMessage.Text = dr["report_message"].ToString();
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

        private void deleteAlertsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                establishConnect();

                con.Open();

                string Query = "DELETE FROM alerts WHERE aircraft = @Id";

                cmd = new NpgsqlCommand(Query, con);

                cmd.Parameters.AddWithValue("@Id", aircraftCodeAlerts.Text);

                int rowsAffected = cmd.ExecuteNonQuery();

                con.Close();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Alert deleted successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    aircraftCodeAlerts.Text = string.Empty;
                    aircraftCodeAlerts.Text = string.Empty;

                    Alerts alerts = new Alerts();
                    alerts.Show();
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
