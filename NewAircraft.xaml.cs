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
    /// Interaction logic for NewAircraft.xaml
    /// </summary>
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

        //database connections

        public static NpgsqlConnection con;
        public static NpgsqlCommand cmd;

        private string get_ConnectingString()
        {
            //To connect with postgreSQL we need values: host, port, dbName, userName, password
            string host = "Host=localhost;";
            String port = "Port=5432;";
            string dbName = "Database=FleetManagementApp;";
            string userName = "Username=postgres;";
            string password = "Password=atuaprima;";

            string connectionString = string.Format("{0}{1}{2}{3}{4}", host, port, dbName, userName, password);
            return connectionString;
        }


        //After having the info we need we establish the connection
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


        //Add values to the database (doctor regist)

        private void registerAircraftButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Step 1: Establish connection
                establishConnect();

                // Step 2: Open the connection
                con.Open();

                // Step 3: Query Generation
                string Query = "INSERT INTO groundaircraft (arrivaltime, supplier, id, flight, origin) VALUES (@arrivaltime, @supplier, @id, @flight, @origin)";


                // Step 4: Initialize the command adapter of the database
                cmd = new NpgsqlCommand(Query, con);


                // Step 4.1: Initialize the parameters in the variables of the command
                cmd.Parameters.AddWithValue("@arrivaltime", arrivalTimeRegistration.Text);
                cmd.Parameters.AddWithValue("@supplier", airSupplierRegistration.Text);
                cmd.Parameters.AddWithValue("@id", aircraftModelRegistration.Text);
                cmd.Parameters.AddWithValue("@flight", aircrafFlighRegistration.Text);
                cmd.Parameters.AddWithValue("@origin", aircraftOriginRegistration.Text);

                // Step 5: Execute the query    
                cmd.ExecuteNonQuery();

                // Step 6: Successful Message
                MessageBox.Show("The aircraft was created successfully");

                // Step 7: Close the connection
                con.Close();

                // Go back to main window
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
