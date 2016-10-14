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

namespace TKMManager.Dialogs
{
    /// <summary>
    /// Interaction logic for EditPayment.xaml
    /// </summary>
    public partial class EditPayment : Window
    {
        public EditPayment()
        {
            InitializeComponent();
        }

        private void btnEditPaym_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection myConnection = new SqlConnection("user id=tkmmanagerdb;" +
                                      "password=Oz781It_2!30;server=mssql3.gear.host;" +
                                      "Trusted_Connection=no;" +
                                      "database=tkmmanagerdb; " +
                                      "connection timeout=30");
            try
            {
                myConnection.Open();

                DateTime dateTimeVariable = DateTime.Now;
                SqlCommand myCommand = new SqlCommand("UPDATE Payments SET [Comment]=@comment WHERE PaymentID=@paymid", myConnection);
                myCommand.Parameters.AddWithValue("@comment", txtPaymComm.Text);
                myCommand.Parameters.AddWithValue("@paymid", int.Parse(txtPaymID.Text.ToString()));
                myCommand.ExecuteNonQuery();

                myConnection.Close();

                this.Close();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
        }

        private void btnCancelEditPaym_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
