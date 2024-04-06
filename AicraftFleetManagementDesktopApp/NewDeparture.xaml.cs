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
    /// Interaction logic for NewDeparture.xaml
    /// </summary>
    public partial class NewDeparture : Window
    {
        public NewDeparture()
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

        private void buttonDepartureClose_Button(object sender, RoutedEventArgs e)
        {
            FlightSchedule flightschedulee = new FlightSchedule();
            flightschedulee.Show();
            this.Close();
        }

        private void TextBox_GotFocus1(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "Departure time")
            {
                textBox.Text = string.Empty;
            }
        }

        private void TextBox_GotFocus2(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "Departure flight")
            {
                textBox.Text = string.Empty;
            }
        }

        private void TextBox_GotFocus3(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "Departure from")
            {
                textBox.Text = string.Empty;
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



        private void addDepartureButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                establishConnect();

                con.Open();

                string Query = "INSERT INTO departures (time, flight, destinationto) VALUES (@time, @flight, @destinationto)";


                cmd = new NpgsqlCommand(Query, con);


                cmd.Parameters.AddWithValue("@time", departureTime.Text);
                cmd.Parameters.AddWithValue("@flight", departureFlight.Text);
                cmd.Parameters.AddWithValue("@destinationto", departureTo.Text);

                cmd.ExecuteNonQuery();

                MessageBox.Show("The departure was added successfully");

                con.Close();

                FlightSchedule flightSchedule = new FlightSchedule();
                flightSchedule.Show();
                this.Close();

            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
