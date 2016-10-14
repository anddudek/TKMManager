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
    /// Interaction logic for AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window
    {
        public AddProduct()
        {
            InitializeComponent();
        }

        private void btnCancelAddProd_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
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
                SqlCommand myCommand = new SqlCommand("INSERT INTO Products ([ProductName], [CreatedBy], [Created], [LastOrder], [LastSuppy], [WarehouseAmount]) VALUES (@name, 'Andrzej', @date, null, null, 0)", myConnection);
                myCommand.Parameters.AddWithValue("@name", txtAddProdName.Text);
                myCommand.Parameters.AddWithValue("@date", dateTimeVariable);
                myCommand.ExecuteNonQuery();

                myConnection.Close();

                this.Close();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
        }
    }
}
