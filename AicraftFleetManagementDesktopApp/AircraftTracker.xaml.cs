using Npgsql;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FinalProjectAircraftFleetManager
{
    public partial class AircraftTracker : Window
    {
        public AircraftTracker()
        {
            InitializeComponent();
            this.Loaded += AircraftTracker_Loaded;
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

        private void buttonAircraftManagSection_Click(object sender, EventArgs e)
        {
            aircraftManagement aircraftManag = new aircraftManagement();
            aircraftManag.Show();
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

        private void AircraftTracker_Loaded(object sender, RoutedEventArgs e)
        {
            GenerateStaticPointsWithDifferentCoordinates();
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

        private void GenerateStaticPointsWithDifferentCoordinates()
        {
            try
            {
                establishConnect();

                con.Open();

                string query = "SELECT flight, aircraft, COUNT(*) FROM flightaircraft GROUP BY flight, aircraft";

                
                cmd = new NpgsqlCommand(query, con);
                NpgsqlDataReader reader = cmd.ExecuteReader();

                
                List<Point> staticCoordinates = new List<Point>()
                {
                    new Point(100, 75),
                    new Point(200, 200),
                    new Point(280, 50),
                    new Point(300, 189),
                    new Point(350, 100),
                    new Point(130, 130),
                    
                };

                int coordinateIndex = 0; 

                while (reader.Read())
                {
                    string flight = reader.GetString(0); 
                    string aircraft = reader.GetString(1); 
                    int aircraftCount = reader.GetInt32(2);

                    
                    for (int i = 0; i < aircraftCount; i++)
                    {
                        
                        Point currentCoordinate = staticCoordinates[coordinateIndex];

                        double x = currentCoordinate.X;
                        double y = currentCoordinate.Y;

                       
                        coordinateIndex = (coordinateIndex + 1) % staticCoordinates.Count;

                      
                        Ellipse point = new Ellipse();
                        point.Width = 10;
                        point.Height = 10;
                        point.Fill = Brushes.Red;
                        point.Stroke = Brushes.Black;
                        point.StrokeThickness = 1;

                       
                        Canvas.SetLeft(point, x);
                        Canvas.SetTop(point, y);

                        canvas.Children.Add(point);

                        Border border = new Border();
                        border.BorderBrush = Brushes.Black;
                        border.BorderThickness = new Thickness(1);
                        border.Background = Brushes.White; 

                        TextBlock textBlock = new TextBlock();
                        textBlock.Text = aircraft.ToLower(); 
                        textBlock.FontSize = 10;
                        textBlock.Foreground = Brushes.Black;

                        border.Child = textBlock;
                        double textBlockX = x + 12; 
                        double textBlockY = y; 

                        if (textBlockX + textBlock.ActualWidth > canvas.ActualWidth)
                            textBlockX = canvas.ActualWidth - textBlock.ActualWidth - 2;
                        if (textBlockY + textBlock.ActualHeight > canvas.ActualHeight)
                            textBlockY = canvas.ActualHeight - textBlock.ActualHeight - 2;

                        Canvas.SetLeft(border, textBlockX);
                        Canvas.SetTop(border, textBlockY);

                        canvas.Children.Add(border);
                    }
                }

                reader.Close(); 
                con.Close(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void newAircraftTracker_Click(object sender, EventArgs e)
        {
            aircraftManagement aircraftManagement = new aircraftManagement();
            aircraftManagement.Show();
            this.Close();
        }
    }
}