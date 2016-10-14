using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace TKMManager.Dialogs
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection myConnection = new SqlConnection("user id=tkmmanagerdb;" +
                            "password=Oz781It_2!30;server=mssql3.gear.host;" +
                            "Trusted_Connection=no;" +
                            "database=tkmmanagerdb; " +
                            "connection timeout=30");

                myConnection.Open();
                //SqlCommand myCommand = new SqlCommand("CREATE TABLE Persons_" + DateTime.Today.ToString("yyyyMMdd") + "(PersonID int, LastName varchar(255), FirstName varchar(255), Address varchar(255), City varchar(255));", myConnection);
                SqlCommand myCommand = new SqlCommand("SELECT userID, ulogin, Name, urole FROM Users", myConnection);
                SqlDataAdapter sda = new SqlDataAdapter(myCommand);
                DataTable dt = new DataTable("Users");
                sda.Fill(dt);
                dgUsers.ItemsSource = dt.DefaultView;
                //myCommand.ExecuteNonQuery();

                myConnection.Close();

            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
        }

        private void LoginCancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
