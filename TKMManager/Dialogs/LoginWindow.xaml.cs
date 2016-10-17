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

                SqlCommand login = new SqlCommand("SELECT COUNT(*) FROM dbo.Users WHERE UPPER(ulogin)=UPPER(@login) AND pw=@pw", myConnection);
                login.Parameters.AddWithValue("@login", txtLogin.Text);
                login.Parameters.AddWithValue("@pw", pwPassword.Password);
                var loginSucceeded = login.ExecuteScalar().ToString();

                myConnection.Close();


                if (loginSucceeded == "1")
                {
                    MainWindow mainWndw = new MainWindow();
                    mainWndw.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Błędny login lub hasło");
                }

                

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
