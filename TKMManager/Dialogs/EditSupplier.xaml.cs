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
    /// Interaction logic for AddPayment.xaml
    /// </summary>
    public partial class EditSupplier : Window
    {
        public EditSupplier()
        {
            InitializeComponent();
        }

        private void btnEditSupp_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection myConnection = new SqlConnection("user id=tkmmanagerdb;" +
                                       "password=Oz781It_2!30;server=mssql3.gear.host;" +
                                       "Trusted_Connection=no;" +
                                       "database=tkmmanagerdb; " +
                                       "connection timeout=30");
            try
            {
                myConnection.Open();

                SqlCommand myCommand = new SqlCommand("UPDATE Suppliers SET [Name]=@sname, [SAddress]=@addr WHERE SuppID=@sid", myConnection);
                myCommand.Parameters.AddWithValue("@sname", txtSuppName.Text);
                myCommand.Parameters.AddWithValue("@addr", txtSuppAddr.Text);
                myCommand.Parameters.AddWithValue("@sid", txtSuppID.Text);
                myCommand.ExecuteNonQuery();

                myConnection.Close();

                this.Close();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
        }

        private void btnCancelEditSupp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
