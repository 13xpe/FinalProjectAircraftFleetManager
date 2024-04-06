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
    /// <summary>
    /// Interaction logic for NewMaintenance.xaml
    /// </summary>
    public partial class NewMaintenance : Window
    {
        public NewMaintenance()
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
            String port = "Port=5432;";
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


        private void requestMaintenanceButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                establishConnect();

                con.Open();

                string Query = "INSERT INTO maintenance (date, aircraft, problem) VALUES (@date, @aircraft, @problem)";


                cmd = new NpgsqlCommand(Query, con);


                cmd.Parameters.AddWithValue("@date", dateMaintenance.Text);
                cmd.Parameters.AddWithValue("@aircraft", aircraftMaintenance.Text);
                cmd.Parameters.AddWithValue("@problem", problemMaintenance.Text);

                cmd.ExecuteNonQuery();

                MessageBox.Show("The aircraft was created successfully");

                con.Close();

                Maintenance maintenance = new Maintenance();
                maintenance.Show();
                this.Close();

            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TextBox_GotFocus1(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "Date")
            {
                textBox.Text = string.Empty;
            }
        }

        private void TextBox_GotFocus2(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "Aircraft")
            {
                textBox.Text = string.Empty;
            }
        }

        private void TextBox_GotFocus3(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "Problem detected")
            {
                textBox.Text = string.Empty;
            }
        }

        private void buttonMaintenanceClose_Button(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
