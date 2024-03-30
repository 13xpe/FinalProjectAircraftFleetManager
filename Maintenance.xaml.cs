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
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;
using System.Collections.ObjectModel;

namespace FinalProjectAircraftFleetManager
{
    /// <summary>
    /// Interaction logic for Maintenance.xaml
    /// </summary>
    public partial class Maintenance : Window
    {
        public Maintenance()
        {
            InitializeComponent();
            LoadData1();
            LoadData2();
            LoadData3();
            LoadData4();

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

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow Login = new MainWindow();
            Login.Show();
            this.Close();
        }

        public static NpgsqlConnection con;
        public static NpgsqlCommand cmd;

        private string Get_ConnectionString()
        {
            //Connect with database to get the inFlight aircraft
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

        private void LoadData1()
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

        private void LoadData2()
        {
            // Establish connection
            using (var connection = new NpgsqlConnection(Get_ConnectionString()))
            {
                try
                {
                    connection.Open();

                    // Create query to count inflight aircraft
                    string query = "SELECT COUNT(*) FROM maintenancepending";

                    // Create command
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        // Execute scalar command to get count
                        int count = Convert.ToInt32(command.ExecuteScalar());

                        // Update the TextBlock with the count
                        maintenancePending.Text = count.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void LoadData3()
        {
            // Establish connection
            using (var connection = new NpgsqlConnection(Get_ConnectionString()))
            {
                try
                {
                    connection.Open();

                    // Create query to count inflight aircraft
                    string query = "SELECT COUNT(*) FROM maintenanceprogress";

                    // Create command
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        // Execute scalar command to get count
                        int count = Convert.ToInt32(command.ExecuteScalar());

                        // Update the TextBlock with the count
                        InProgress.Text = count.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void LoadData4()
        {
            int inflight = 0;
            int onGround = 0;

            // Establish connection
            using (var connection = new NpgsqlConnection(Get_ConnectionString()))
            {
                try
                {
                    connection.Open();

                    // Create query to count inflight aircraft
                    string queryInflight = "SELECT COUNT(*) FROM maintenancepending";

                    // Create command for inflight count
                    using (var commandInflight = new NpgsqlCommand(queryInflight, connection))
                    {
                        // Execute scalar command to get inflight count
                        inflight = Convert.ToInt32(commandInflight.ExecuteScalar());
                    }

                    // Create query to count on ground aircraft
                    string queryOnGround = "SELECT COUNT(*) FROM maintenanceprogress";

                    // Create command for on ground count
                    using (var commandOnGround = new NpgsqlCommand(queryOnGround, connection))
                    {
                        // Execute scalar command to get on ground count
                        onGround = Convert.ToInt32(commandOnGround.ExecuteScalar());
                    }

                    // Calculate total count
                    int total = inflight + onGround;

                    // Update the TextBlock with the total count
                    maintenanceCreated.Text = total.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void DrawCartesianGraph()
        {
            // Clear existing children of myCanvas
            myCanvas.Children.Clear();

            // Define the canvas dimensions
            double canvasWidth = myCanvas.ActualWidth;
            double canvasHeight = myCanvas.ActualHeight;

            // Create X and Y axes for value 3
            Line yAxis3 = new Line
            {
                X1 = canvasWidth / 4, // Shifted towards the left by dividing canvasWidth by 4
                Y1 = 0,
                X2 = canvasWidth / 4, // Shifted towards the left by dividing canvasWidth by 4
                Y2 = canvasHeight,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };

            // Add axes for value 3 to the canvas
            myCanvas.Children.Add(yAxis3);

            // Create X and Y axes for value 2
            Line xAxis2 = new Line
            {
                X1 = 0,
                Y1 = canvasHeight / 2 + 30, // Adjust position for value 2
                X2 = canvasWidth,
                Y2 = canvasHeight / 2 + 30, // Adjust position for value 2
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };

            // Add axes for value 2 to the canvas
            myCanvas.Children.Add(xAxis2);

            // Add new gold points with adjusted x values
            PlotPoint(-3.3, 0, Brushes.Gold);
            PlotPoint(-1, 0, Brushes.Gold);
            PlotPoint(2, 2, Brushes.Gold);
            PlotPoint(7, 1, Brushes.Gold);
            PlotPoint(9, 2, Brushes.Gold);
            PlotPoint(10.5, 0, Brushes.Gold);

            // Add new green points
            PlotPoint(-3.3, 3, Brushes.Green);
            PlotPoint(-1, 1, Brushes.Green);
            PlotPoint(2, 1, Brushes.Green);
            PlotPoint(7, 3, Brushes.Green);
            PlotPoint(9, -1, Brushes.Green);
            PlotPoint(10.5, -1, Brushes.Green);

            // Create polyline to connect gold points
            Polyline goldPolyline = new Polyline
            {
                Stroke = Brushes.Gold,
                StrokeThickness = 2
            };

            // Add points to the polyline (from lowest to highest x value)
            goldPolyline.Points.Add(new Point(canvasWidth / 4 - 3.3 * 20, canvasHeight / 2 - 0 * 20)); // (-5, 0)
            goldPolyline.Points.Add(new Point(canvasWidth / 4 - 1 * 20, canvasHeight / 2 - 0 * 20)); // (-2, 0)
            goldPolyline.Points.Add(new Point(canvasWidth / 4 + 2 * 20, canvasHeight / 2 - 2 * 20)); // (1, 2)
            goldPolyline.Points.Add(new Point(canvasWidth / 4 + 7 * 20, canvasHeight / 2 - 1 * 20)); // (6, 1)
            goldPolyline.Points.Add(new Point(canvasWidth / 4 + 9 * 20, canvasHeight / 2 - 2 * 20)); // (8, 2)
            goldPolyline.Points.Add(new Point(canvasWidth / 4 + 10.5 * 20, canvasHeight / 2 - 0 * 20)); // (10, 0)

            // Add polyline to the canvas
            myCanvas.Children.Add(goldPolyline);

            // Create green polyline to connect green points
            Polyline greenPolyline = new Polyline
            {
                Stroke = Brushes.Green,
                StrokeThickness = 2
            };

            // Add points to the green polyline (from lowest to highest x value)
            greenPolyline.Points.Add(new Point(canvasWidth / 4 - 3.3 * 20, canvasHeight / 2 - 3 * 20)); // (-5, 3)
            greenPolyline.Points.Add(new Point(canvasWidth / 4 - 1 * 20, canvasHeight / 2 - 1 * 20)); // (-2, 1)
            greenPolyline.Points.Add(new Point(canvasWidth / 4 + 2 * 20, canvasHeight / 2 - 1 * 20)); // (1, 1)
            greenPolyline.Points.Add(new Point(canvasWidth / 4 + 7 * 20, canvasHeight / 2 - 3 * 20)); // (6, 3)
            greenPolyline.Points.Add(new Point(canvasWidth / 4 + 9 * 20, canvasHeight / 2 + 1 * 20)); // (8, -1)
            greenPolyline.Points.Add(new Point(canvasWidth / 4 + 10.5 * 20, canvasHeight / 2 + 1 * 20)); // (10, -1)

            // Add green polyline to the canvas
            myCanvas.Children.Add(greenPolyline);

            // Add "new week" text at the intersection of the axes
            TextBlock newTextBlock = new TextBlock
            {
                Text = "new week",
                Foreground = Brushes.Black,
                FontSize = 10,
                FontWeight = FontWeights.Bold
            };

            Canvas.SetLeft(newTextBlock, canvasWidth / 4 + 5); // Adjust position of text to the right
            Canvas.SetTop(newTextBlock, canvasHeight / 1 - 55); // Adjust position of text higher

            // Add text to the canvas
            myCanvas.Children.Add(newTextBlock);
        }

        private void PlotPoint(double x, double y, Brush pointColor)
        {
            Ellipse point = new Ellipse
            {
                Width = 6,
                Height = 6,
                Fill = pointColor,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            // Calculate canvas coordinates based on Cartesian coordinates
            double canvasX = myCanvas.ActualWidth / 4 + x * 20; // Adjust multiplier for scale and shifted X axis
            double canvasY = myCanvas.ActualHeight / 2 - y * 20; // Adjust multiplier for scale

            // Set position of the point on the canvas
            Canvas.SetLeft(point, canvasX - point.Width / 2);
            Canvas.SetTop(point, canvasY - point.Height / 2);

            // Add the point to the canvas
            myCanvas.Children.Add(point);
        }

        private void Maintenance_Loaded(object sender, RoutedEventArgs e)
        {
            DrawCartesianGraph();
        }

    }
}

