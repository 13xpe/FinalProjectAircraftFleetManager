using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
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

    public partial class aircraftManagement : Window
    {
        public aircraftManagement()
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

        private void flightSchedule_Click(object sender, EventArgs e)
        {
            FlightSchedule flightSchedule = new FlightSchedule();
            flightSchedule.Show();
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
            string Query = "select * from flightaircraft";
            cmd = new NpgsqlCommand(Query, con);

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            inFlighAircraft.ItemsSource = dt.DefaultView; 
            con.Close();
        }

        private void LoadData2()
        {
            establishConnect();
            con.Open();
            string Query = "select * from groundaircraft";
            cmd = new NpgsqlCommand(Query, con);

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            onGroundAircraft.ItemsSource = dt.DefaultView; 
            con.Close();
        }

        private void newAircraft_Click(object sender, RoutedEventArgs e)
        {
            NewAircraft newAircraftPage = new NewAircraft();
            newAircraftPage.Show();
            this.Close();
        }

        private void TextBox_GotFocus1(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "Aircraft code")
            {
                textBox.Text = string.Empty;
            }
        }

        private void deleteAircraft_Click(object sender, RoutedEventArgs e)
        {
            DeleteAircraft deleteAircraft = new DeleteAircraft();
            deleteAircraft.Show();
            this.Close();
        }


    }
}
