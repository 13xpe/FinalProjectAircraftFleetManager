using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FinalProjectAircraftFleetManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void buttonCloseClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void buttonMinimizeClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private string get_ConnectionString()
        {

            string host = "Host=localhost;";
            string port = "Port=5432;";
            string dbName = "Database=FleetManagementApp;";
            string userName = "Username=postgres;";
            string password = "Password=atuaprima;";

            string connectionString = string.Format("{0}{1}{2}{3}{4}", host, port, dbName, userName, password);
            return connectionString;

        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string inputUsername = username.Text;
            string inputPassword = password.Password;

            NpgsqlConnection con = new NpgsqlConnection(get_ConnectionString());
            con.Open();

            string query = "SELECT worker_username, worker_password FROM workers WHERE worker_username = @username";
            NpgsqlCommand cmd = new NpgsqlCommand(query, con);
            cmd.Parameters.AddWithValue("@username", inputUsername);

            NpgsqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string storedPassword = reader["worker_password"].ToString();

                if (inputPassword == storedPassword)
                {
                    Dashboard dashboard = new Dashboard();
                    dashboard.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Wrong Password");
                }
            }
            else
            {
                MessageBox.Show("User not found");
            }

            con.Close();
        }

        private void ForgotPassword_Click(Object sender, RoutedEventArgs e)
        {
            ForgotPassword forgotpassword = new ForgotPassword();
            forgotpassword.Show();
            this.Close();
        }

        private void CreateAccountButton_Click(Object sender, RoutedEventArgs e)
        {
            EngineerRegistration engineerRegistration = new EngineerRegistration();
            engineerRegistration.Show();
            this.Close();
        }


        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "aircraftEngineer")
            {
                textBox.Text = string.Empty;
            }
        }

        private void PasswordBox_GotFocus2(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox && passwordBox.Password == "password")
            {
                passwordBox.Clear();
            }
        }
    }
}
