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
using Npgsql;

namespace FinalProjectAircraftFleetManager
{
    /// <summary>
    /// Interaction logic for EngineerRegistration.xaml
    /// </summary>
    public partial class EngineerRegistration : Window
    {
        public EngineerRegistration()
        {
            InitializeComponent();
        }


        //Connections

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

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Step 1: Establish connection
                establishConnect();

                // Step 2: Open the connection
                con.Open();

                // Step 3: Query Generation
                string Query = "INSERT INTO workers (worker_name, worker_surname, worker_department, worker_contact, worker_username, worker_password) VALUES (@worker_name, @worker_surname, @worker_department, @worker_contact, @worker_username, @worker_password)";


                // Step 4: Initialize the command adapter of the database
                cmd = new NpgsqlCommand(Query, con);

                string workerPlainPassword = registerPassword.Password;

                // Step 4.1: Initialize the parameters in the variables of the command
                cmd.Parameters.AddWithValue("@worker_name", registerFirstName.Text);
                cmd.Parameters.AddWithValue("@worker_surname", registerLastName.Text);
                cmd.Parameters.AddWithValue("@worker_department", registerDepartment.Text);
                cmd.Parameters.AddWithValue("@worker_contact", long.Parse(registerContact.Text));
                cmd.Parameters.AddWithValue("@worker_username", registerUsername.Text);
                cmd.Parameters.AddWithValue("@worker_password", workerPlainPassword);

                // Step 5: Execute the query    
                cmd.ExecuteNonQuery();

                // Step 6: Successful Message
                MessageBox.Show("Your profile was created successfully");

                // Step 7: Close the connection
                con.Close();

                // Go back to main window
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();

            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void Window_MouseLeftButtonDown2(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void buttonCloseClick2(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void buttonMinimizeClick2(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void TextBox_GotFocus3(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "First Name")
            {
                textBox.Text = string.Empty;
            }
        }

        private void TextBox_GotFocus4(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "Last Name")
            {
                textBox.Text = string.Empty;
            }
        }

        private void TextBox_GotFocus5(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "Department")
            {
                textBox.Text = string.Empty;
            }
        }

        private void TextBox_GotFocus6(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "Contact")
            {
                textBox.Text = string.Empty;
            }
        }

        private void TextBox_GotFocus7(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "Username")
            {
                textBox.Text = string.Empty;
            }
        }

        private void PasswordBox_GotFocus8(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox && passwordBox.Password == "password")
            {
                passwordBox.Clear();
            }
        }

        private void registerGoBack_Click(Object sender, RoutedEventArgs e)
        {
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
            this.Close();
        }


    }
}
