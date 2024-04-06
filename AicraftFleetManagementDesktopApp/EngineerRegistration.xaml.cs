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

    public partial class EngineerRegistration : Window
    {
        public EngineerRegistration()
        {
            InitializeComponent();
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



        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                establishConnect();

                con.Open();

                string Query = "INSERT INTO workers (worker_name, worker_surname, worker_department, worker_contact, worker_username, worker_password) VALUES (@worker_name, @worker_surname, @worker_department, @worker_contact, @worker_username, @worker_password)";

                cmd = new NpgsqlCommand(Query, con);

                string workerPlainPassword = registerPassword.Password;

                cmd.Parameters.AddWithValue("@worker_name", registerFirstName.Text);
                cmd.Parameters.AddWithValue("@worker_surname", registerLastName.Text);
                cmd.Parameters.AddWithValue("@worker_department", registerDepartment.Text);

                if (long.TryParse(registerContact.Text, out long workerContact))
                {
                    cmd.Parameters.AddWithValue("@worker_contact", workerContact);
                }
                else
                {
                    MessageBox.Show("Invalid contact number. Please enter a valid long integer.");
                    con.Close(); 
                    return;
                }

                cmd.Parameters.AddWithValue("@worker_username", registerUsername.Text);
                cmd.Parameters.AddWithValue("@worker_password", workerPlainPassword);

                cmd.ExecuteNonQuery();

                con.Close();

                MessageBox.Show("Your profile was created successfully");
                MainWindow mainw = new MainWindow();
                mainw.Show();

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);

                try
                {
                    con.Close(); 
                }
                catch (Exception)
                {
                    
                }
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
