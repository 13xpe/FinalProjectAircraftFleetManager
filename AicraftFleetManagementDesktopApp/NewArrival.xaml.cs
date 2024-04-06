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
    /// Interaction logic for NewArrival.xaml
    /// </summary>
    public partial class NewArrival : Window
    {
        public NewArrival()
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

        private void TextBox_GotFocus1(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "Arrival time")
            {
                textBox.Text = string.Empty;
            }
        }

        private void TextBox_GotFocus2(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "Arrival flight")
            {
                textBox.Text = string.Empty;
            }
        }

        private void TextBox_GotFocus3(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "Arriving from")
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



        private void addArrivalButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                establishConnect();

                con.Open();

                string Query = "INSERT INTO arrivals (time, flight, originfrom) VALUES (@time, @flight, @originfrom)";


                cmd = new NpgsqlCommand(Query, con);


                cmd.Parameters.AddWithValue("@time", arrivalTime.Text);
                cmd.Parameters.AddWithValue("@flight", arrivalFlight.Text);
                cmd.Parameters.AddWithValue("@originfrom", arrivalFrom.Text);

                cmd.ExecuteNonQuery();

                MessageBox.Show("The arrival was added successfully");

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

        private void buttonArrivalClose_Button(object sender, RoutedEventArgs e)
        {
            FlightSchedule flightschedule1 = new FlightSchedule();
            flightschedule1.Show();
            this.Close();
        }
    }
}
