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
    
    public partial class NewAircraft : Window
    {
        public NewAircraft()
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


        private void registerAircraftButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                establishConnect();

                con.Open();

                string Query = "INSERT INTO groundaircraft (arrivaltime, supplier, id, flight, origin) VALUES (@arrivaltime, @supplier, @id, @flight, @origin)";


                cmd = new NpgsqlCommand(Query, con);


                cmd.Parameters.AddWithValue("@arrivaltime", arrivalTimeRegistration.Text);
                cmd.Parameters.AddWithValue("@supplier", airSupplierRegistration.Text);
                cmd.Parameters.AddWithValue("@id", aircraftModelRegistration.Text);
                cmd.Parameters.AddWithValue("@flight", aircrafFlighRegistration.Text);
                cmd.Parameters.AddWithValue("@origin", aircraftOriginRegistration.Text);

                cmd.ExecuteNonQuery();

                MessageBox.Show("The aircraft was created successfully");

                con.Close();

                aircraftManagement aircraftManag = new aircraftManagement();
                aircraftManag.Show();
                this.Close();

            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
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
            if (sender is TextBox textBox && textBox.Text == "Air supplier group")
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

        private void TextBox_GotFocus5(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "Aircraft flight number")
            {
                textBox.Text = string.Empty;
            }
        }

        private void backRegisterAircraftButton_Click(object sender, RoutedEventArgs e)
        {
            aircraftManagement aircraftManagem = new aircraftManagement();
            aircraftManagem.Show();
            this.Close();

        }
    }
}
