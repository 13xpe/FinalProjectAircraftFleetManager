using Npgsql;
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

namespace FinalProjectAircraftFleetManager
{
    /// <summary>
    /// Interaction logic for NewAlert.xaml
    /// </summary>
    public partial class NewAlert : Window
    {
        public NewAlert()
        {
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDownDashboard(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void TextBox_GotFocus2(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "Report date")
            {
                textBox.Text = string.Empty;
            }
        }

        private void TextBox_GotFocus3(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "Aircraft model")
            {
                textBox.Text = string.Empty;
            }
        }

        private void TextBox_GotFocus4(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "Aircraft section")
            {
                textBox.Text = string.Empty;
            }
        }

        private void TextBox_GotFocus5(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "Alert message")
            {
                textBox.Text = string.Empty;
            }
        
        }

        private void buttonAlertsClose_Button(object sender, RoutedEventArgs e)
        {
            Alerts alerts = new Alerts();
            alerts.Show();
            this.Close();
        }

        public static NpgsqlConnection con;
        public static NpgsqlCommand cmd;

        private string get_ConnectingString()
        {
            string host = "Host=localhost;";
            String port = "Port=5432;";
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



        private void addAlertButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                establishConnect();

                con.Open();

                string Query = "INSERT INTO alerts (report_date, aircraft, report_message, report_details, report_grade) VALUES (@report_date, @aircraft, @report_message, @report_details, @report_grade)";


                cmd = new NpgsqlCommand(Query, con);


                cmd.Parameters.AddWithValue("@report_date", alertDate.Text);
                cmd.Parameters.AddWithValue("@aircraft", alertAicraft.Text);
                cmd.Parameters.AddWithValue("@report_message", alertMessage.Text);
                cmd.Parameters.AddWithValue("@report_details", alertSection.Text);
                cmd.Parameters.AddWithValue("@report_grade", alertGrade.Text);

                cmd.ExecuteNonQuery();

                MessageBox.Show("The alert was created successfully");

                con.Close();

                Alerts alerts = new Alerts();
                alerts.Show();
                this.Close();

            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void alertGrade_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (alertGrade.SelectedItem != null && (alertGrade.SelectedItem as ComboBoxItem).Content.ToString() != "Select grade")
            {
                string selectedGrade = (alertGrade.SelectedItem as ComboBoxItem).Content.ToString();
                MessageBox.Show($"Selected grade: {selectedGrade}");
            }
        }

    }
}
