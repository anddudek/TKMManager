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
    public partial class AddUser : Window
    {
        public AddUser()
        {
            InitializeComponent();
        }

        private void AddNewUser(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection myConnection = new SqlConnection("user id=tkmmanagerdb;" +
                            "password=Oz781It_2!30;server=mssql3.gear.host;" +
                            "Trusted_Connection=no;" +
                            "database=tkmmanagerdb; " +
                            "connection timeout=30");

                myConnection.Open();

                SqlCommand myCommand = new SqlCommand("INSERT INTO Users ([ulogin], [pw], [Name], [urole]) VALUES (@login, @pw, @name, @role)", myConnection);
                myCommand.Parameters.AddWithValue("@login", txtLogin.Text);
                myCommand.Parameters.AddWithValue("@pw", txtPassword.Text);
                myCommand.Parameters.AddWithValue("@name", txtName.Text);
                myCommand.Parameters.AddWithValue("@role", txtRole.Text);
                myCommand.ExecuteNonQuery();

                myConnection.Close();
                this.Close();

            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
        }

        private void AddCancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
