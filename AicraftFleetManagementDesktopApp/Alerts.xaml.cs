using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    
    public partial class Alerts : Window
    {
        public Alerts()
        {
            InitializeComponent();
            LoadData();
            LoadData2();
            LoadData3();
        }

        private void Window_MouseLeftButtonDownDashboard(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void buttonCloseDashClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void buttonMinimizeDashClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Dashboard_Click(object sender, RoutedEventArgs e)
        {
            Dashboard dash = new Dashboard();
            dash.Show();
            this.Close();
        }

        private void buttonAircraftManagSection_Click(object sender, RoutedEventArgs e)
        {
            aircraftManagement manag = new aircraftManagement();
            manag.Show();
            this.Close();
        }

        private void flightSchedule_Click(object sender, RoutedEventArgs e)
        {
            FlightSchedule flightsched = new FlightSchedule();
            flightsched.Show();
            this.Close();
        }

        private void Maintenance_Click(object sender, RoutedEventArgs e)
        {
            Maintenance maintenance = new Maintenance();
            maintenance.Show();
            this.Close();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow Login = new MainWindow();
            Login.Show();
            this.Close();
        }

        private void deleteAlert_Click(object sender, RoutedEventArgs e)
        {
            DeleteAlert alert = new DeleteAlert();
            alert.Show();
        }

        private void aircraftTraker_Click(object sender, RoutedEventArgs e)
        {
            AircraftTracker aircraftTracker = new AircraftTracker();
            aircraftTracker.Show();
            this.Close();
        }

        public static NpgsqlConnection con;
        public static NpgsqlCommand cmd;

        private string Get_ConnectionString()
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
                con = new NpgsqlConnection(Get_ConnectionString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void LoadData()
        {
            establishConnect();
            con.Open();
            string Query = "select * from alerts";
            cmd = new NpgsqlCommand(Query, con);

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            alertsDash.ItemsSource = dt.DefaultView; 
            con.Close();
        }

        private void LoadData2()
        {
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(Get_ConnectionString()))
                {
                    con.Open();
                    string query = "SELECT COUNT(*) FROM alerts WHERE report_grade = 'WARN';";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, con))
                    {
                        int warningCount = Convert.ToInt32(cmd.ExecuteScalar());
                        alertsWarning.Text = warningCount.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void LoadData3()
        {
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(Get_ConnectionString()))
                {
                    con.Open();
                    string query = "SELECT COUNT(*) FROM alerts WHERE report_grade = 'CRIT';";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, con))
                    {
                        int warningCount = Convert.ToInt32(cmd.ExecuteScalar());
                        alertsCritical.Text = warningCount.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void newAlert_Click(object sender, EventArgs e)
        {
            NewAlert alert = new NewAlert();
            alert.Show();
        }
    }
}
