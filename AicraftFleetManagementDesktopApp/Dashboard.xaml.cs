using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
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
    
    public partial class Dashboard : Window
    {
        public Dashboard()
        {
            InitializeComponent();
            LoadData();
            LoadData2();
            LoadData3();
            LoadData4();
            LoadData5();
            LoadData6();
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
            string Query = "select * from flightaircraft";
            cmd = new NpgsqlCommand(Query, con);

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            flightAircraftDatagrid.ItemsSource = dt.DefaultView; 
            con.Close();
        }

        private void LoadData2()
        {
            establishConnect();
            con.Open();
            string Query = "select * from maintenance";
            cmd = new NpgsqlCommand(Query, con);

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            maintenanceDash.ItemsSource = dt.DefaultView;
            con.Close();
        }

        private void LoadData3()
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


        

        private void buttonAircraftManagSection_Click(object sender, EventArgs e)
        {
            aircraftManagement aircraftManag = new aircraftManagement();
            aircraftManag.Show();
            this.Close();
        }

        private void LoadData4()
        {
            
            using (var connection = new NpgsqlConnection(Get_ConnectionString()))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT COUNT(*) FROM flightaircraft";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        int count = Convert.ToInt32(command.ExecuteScalar());

                        inflightCount.Text = count.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void LoadData5()
        {
            using (var connection = new NpgsqlConnection(Get_ConnectionString()))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT COUNT(*) FROM groundaircraft";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        int count = Convert.ToInt32(command.ExecuteScalar());

                        ongroundCount.Text = count.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void LoadData6()
        {
            int inflight = 0;
            int onGround = 0;

            using (var connection = new NpgsqlConnection(Get_ConnectionString()))
            {
                try
                {
                    connection.Open();

                    string queryInflight = "SELECT COUNT(*) FROM flightaircraft";

                    using (var commandInflight = new NpgsqlCommand(queryInflight, connection))
                    {
                        inflight = Convert.ToInt32(commandInflight.ExecuteScalar());
                    }

                    string queryOnGround = "SELECT COUNT(*) FROM groundaircraft";

                    using (var commandOnGround = new NpgsqlCommand(queryOnGround, connection))
                    {
                        onGround = Convert.ToInt32(commandOnGround.ExecuteScalar());
                    }

                    int total = inflight + onGround;

                    totalaircraftCount.Text = total.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void flightSchedule_Click(object sender, EventArgs e)
        {
            FlightSchedule flightSchedule = new FlightSchedule();
            flightSchedule.Show();
            this.Close();
        }

        private void Maintenance_Click(object sender, EventArgs e)
        {
            Maintenance mainten = new Maintenance();
            mainten.Show();
            this.Close();
        }

        private void alerts_Click(object sender, EventArgs e)
        {
            Alerts alerts = new Alerts();
            alerts.Show();
            this.Close();
        }

        private void logOut_Click(object sender, EventArgs e)
        {
            MainWindow Login = new MainWindow();
            Login.Show();
            this.Close();
        }

        private void aircraftTracker_Click(object sender, EventArgs e)
        {
            AircraftTracker aircraftTracker = new AircraftTracker();
            aircraftTracker.Show();
            this.Close();
        }

    }
}
