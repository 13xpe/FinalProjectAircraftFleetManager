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
using System.Globalization;

namespace FinalProjectAircraftFleetManager
{
  
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

        private void alerts_Click(object sender, RoutedEventArgs e)
        {
            Alerts alerts = new Alerts();
            alerts.Show();
            this.Close();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow Login = new MainWindow();
            Login.Show();
            this.Close();
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
            using (var connection = new NpgsqlConnection(Get_ConnectionString()))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT COUNT(*) FROM maintenancepending";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        int count = Convert.ToInt32(command.ExecuteScalar());

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
            using (var connection = new NpgsqlConnection(Get_ConnectionString()))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT COUNT(*) FROM maintenanceprogress";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        int count = Convert.ToInt32(command.ExecuteScalar());

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
            using (var connection = new NpgsqlConnection(Get_ConnectionString()))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT COUNT(*) FROM maintenance";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        int count = Convert.ToInt32(command.ExecuteScalar());

                        maintenanceCreated.Text = count.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void searchAlertseButton_Click(object sender, RoutedEventArgs e)
        {
            string aircraftCode = aircraftPendingCodeAlerts.Text;

            if (string.IsNullOrWhiteSpace(aircraftCode))
            {
                MessageBox.Show("Please enter an aircraft code.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using (var connection = new NpgsqlConnection(Get_ConnectionString()))
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            string getDataQuery = "SELECT date, problem FROM maintenance WHERE aircraft = @AircraftCode";
                            using (var getDataCmd = new NpgsqlCommand(getDataQuery, connection, transaction))
                            {
                                getDataCmd.Parameters.AddWithValue("@AircraftCode", aircraftCode);

                                using (var reader = getDataCmd.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        string dateString = reader.GetString(reader.GetOrdinal("date"));
                                        string problem = reader.GetString(reader.GetOrdinal("problem"));
                                        DateTime date;

                                        bool parseSuccess = DateTime.TryParse(dateString, out date);
                                        if (!parseSuccess)
                                        {
                                            string[] formats = { "yyyy-MM-dd", "MM/dd/yyyy", "dd/MM/yyyy" };
                                            parseSuccess = DateTime.TryParseExact(dateString, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
                                        }

                                        if (!parseSuccess)
                                        {
                                            MessageBox.Show("Error parsing date from the database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                            return;
                                        }

                                        reader.Close();
                                        getDataCmd.Dispose();

                                        string insertQuery = "INSERT INTO maintenancepending (aircraft, date, problem) VALUES (@AircraftCode, @Date, @Problem)";
                                        using (var insertCmd = new NpgsqlCommand(insertQuery, connection, transaction))
                                        {
                                            insertCmd.Parameters.AddWithValue("@AircraftCode", aircraftCode);
                                            insertCmd.Parameters.AddWithValue("@Date", date);
                                            insertCmd.Parameters.AddWithValue("@Problem", problem);

                                            int rowsAffected = insertCmd.ExecuteNonQuery();
                                            if (rowsAffected > 0)
                                            {
                                                string deleteQuery = "DELETE FROM maintenance WHERE aircraft = @AircraftCode";
                                                using (var deleteCmd = new NpgsqlCommand(deleteQuery, connection, transaction))
                                                {
                                                    deleteCmd.Parameters.AddWithValue("@AircraftCode", aircraftCode);
                                                    deleteCmd.ExecuteNonQuery();
                                                }

                                                transaction.Commit();

                                                MessageBox.Show("Data transferred to maintenancepending successfully and deleted from maintenance.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                                                LoadData2(); 

                                                Maintenance maintenance = new Maintenance();
                                                maintenance.Show();
                                                
                                            }
                                            else
                                            {
                                                MessageBox.Show("Failed to transfer data to maintenancepending.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("No data retrieved from maintenance table for the specified aircraft code.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void DrawCartesianGraph()
        {
            //clear canvas
            myCanvas.Children.Clear();

            double canvasWidth = myCanvas.ActualWidth;
            double canvasHeight = myCanvas.ActualHeight;

            // creatte X and Y axes
            Line yAxis3 = new Line
            {
                X1 = canvasWidth / 4, 
                Y1 = 0,
                X2 = canvasWidth / 4, 
                Y2 = canvasHeight,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };

           
            myCanvas.Children.Add(yAxis3);

            Line xAxis2 = new Line
            {
                X1 = 0,
                Y1 = canvasHeight / 2 + 30, 
                X2 = canvasWidth,
                Y2 = canvasHeight / 2 + 30, 
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };

            myCanvas.Children.Add(xAxis2);

            PlotPoint(-3.3, 0, Brushes.Gold);
            PlotPoint(-1, 0, Brushes.Gold);
            PlotPoint(2, 2, Brushes.Gold);
            PlotPoint(7, 1, Brushes.Gold);
            PlotPoint(9, 2, Brushes.Gold);
            PlotPoint(10.5, 0, Brushes.Gold);

            PlotPoint(-3.3, 3, Brushes.Green);
            PlotPoint(-1, 1, Brushes.Green);
            PlotPoint(2, 1, Brushes.Green);
            PlotPoint(7, 3, Brushes.Green);
            PlotPoint(9, -1, Brushes.Green);
            PlotPoint(10.5, -1, Brushes.Green);

            Polyline goldPolyline = new Polyline
            {
                Stroke = Brushes.Gold,
                StrokeThickness = 2
            };

            goldPolyline.Points.Add(new Point(canvasWidth / 4 - 3.3 * 20, canvasHeight / 2 - 0 * 20)); // (-5, 0)
            goldPolyline.Points.Add(new Point(canvasWidth / 4 - 1 * 20, canvasHeight / 2 - 0 * 20)); // (-2, 0)
            goldPolyline.Points.Add(new Point(canvasWidth / 4 + 2 * 20, canvasHeight / 2 - 2 * 20)); // (1, 2)
            goldPolyline.Points.Add(new Point(canvasWidth / 4 + 7 * 20, canvasHeight / 2 - 1 * 20)); // (6, 1)
            goldPolyline.Points.Add(new Point(canvasWidth / 4 + 9 * 20, canvasHeight / 2 - 2 * 20)); // (8, 2)
            goldPolyline.Points.Add(new Point(canvasWidth / 4 + 10.5 * 20, canvasHeight / 2 - 0 * 20)); // (10, 0)

            myCanvas.Children.Add(goldPolyline);

            Polyline greenPolyline = new Polyline
            {
                Stroke = Brushes.Green,
                StrokeThickness = 2
            };

            greenPolyline.Points.Add(new Point(canvasWidth / 4 - 3.3 * 20, canvasHeight / 2 - 3 * 20)); // (-5, 3)
            greenPolyline.Points.Add(new Point(canvasWidth / 4 - 1 * 20, canvasHeight / 2 - 1 * 20)); // (-2, 1)
            greenPolyline.Points.Add(new Point(canvasWidth / 4 + 2 * 20, canvasHeight / 2 - 1 * 20)); // (1, 1)
            greenPolyline.Points.Add(new Point(canvasWidth / 4 + 7 * 20, canvasHeight / 2 - 3 * 20)); // (6, 3)
            greenPolyline.Points.Add(new Point(canvasWidth / 4 + 9 * 20, canvasHeight / 2 + 1 * 20)); // (8, -1)
            greenPolyline.Points.Add(new Point(canvasWidth / 4 + 10.5 * 20, canvasHeight / 2 + 1 * 20)); // (10, -1)

            myCanvas.Children.Add(greenPolyline);

            TextBlock newTextBlock = new TextBlock
            {
                Text = "new week",
                Foreground = Brushes.Black,
                FontSize = 10,
                FontWeight = FontWeights.Bold
            };

            Canvas.SetLeft(newTextBlock, canvasWidth / 4 + 5); 
            Canvas.SetTop(newTextBlock, canvasHeight / 1 - 55); 

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

            double canvasX = myCanvas.ActualWidth / 4 + x * 20; 
            double canvasY = myCanvas.ActualHeight / 2 - y * 20; 

            Canvas.SetLeft(point, canvasX - point.Width / 2);
            Canvas.SetTop(point, canvasY - point.Height / 2);

            myCanvas.Children.Add(point);
        }

        private void Maintenance_Loaded(object sender, RoutedEventArgs e)
        {
            DrawCartesianGraph();
        }

        private void newMaintenanceRequest_Click(object sender, RoutedEventArgs e)
        {
            NewMaintenance maintenance = new NewMaintenance();
            maintenance.Show();
        }

        private void newMaintenanceChecked_Click(object sender, RoutedEventArgs e)
        {
            MaintenanceChecked  checkedmainte = new MaintenanceChecked();
            checkedmainte.Show();
        }

        private void TextBox_GotFocus1(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "Aircraft code")
            {
                textBox.Text = string.Empty;
            }
        }

    }
}

