using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
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

    public partial class FlightSchedule : Window
    {
        public FlightSchedule()
        {
            InitializeComponent();
            LoadData();
            LoadData2();
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

        private void dashBoardButton_Click(object sender, EventArgs e)
        {
            Dashboard dashboardSection = new Dashboard();
            dashboardSection.Show();
            this.Close();
        }

        private void buttonAircraftManagSection_Click(object sender, EventArgs e)
        {
            aircraftManagement aircraftManag = new aircraftManagement();
            aircraftManag.Show();
            this.Close();
        }

        private void Maintenance_Click(object sender, EventArgs e)
        {
            Maintenance maint = new Maintenance();
            maint.Show();
            this.Close();
        }

        private void alerts_Click(object sender, EventArgs e)
        {
            Alerts alerts = new Alerts();
            alerts.Show();
            this.Close();
        }

        private void aircraftTraker_Click(object sender, EventArgs e)
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
            string Query = "select * from arrivals";
            cmd = new NpgsqlCommand(Query, con);

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            arrivalsAircraft.ItemsSource = dt.DefaultView; 
            con.Close();
        }

        private void LoadData2()
        {
            establishConnect();
            con.Open();
            string Query = "select * from departures";
            cmd = new NpgsqlCommand(Query, con);

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            departuresAircraft.ItemsSource = dt.DefaultView; 
            con.Close();
        }

        private void newArrival_Click(object sender, EventArgs e)
        {
            NewArrival newarrival = new NewArrival();
            newarrival.Show();
        }

        private void newDeparture_Click(object sender, EventArgs e)
        {
            NewDeparture newdepart = new NewDeparture();
            newdepart.Show();
        }
    }
}
