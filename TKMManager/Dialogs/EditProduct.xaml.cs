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
    /// Interaction logic for EditProduct.xaml
    /// </summary>
    public partial class EditProduct : Window
    {
        public EditProduct()
        {
            InitializeComponent();
        }

        private void btnEditProduct_Click(object sender, RoutedEventArgs e)
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
                SqlCommand myCommand = new SqlCommand("UPDATE Products SET [ProductName]=@prodname, [CreatedBy]=@created, [Created]=@date, [WarehouseAmount] = @amount WHERE ProductID=@prodid", myConnection);
                myCommand.Parameters.AddWithValue("@prodname", txtEditProdName.Text);
                myCommand.Parameters.AddWithValue("@created", "Andrzej");
                myCommand.Parameters.AddWithValue("@date", dateTimeVariable);
                myCommand.Parameters.AddWithValue("@amount", int.Parse(txtEditProdAmount.Text.ToString()));
                myCommand.Parameters.AddWithValue("@prodid", int.Parse(txtProdID.Text.ToString()));
                myCommand.ExecuteNonQuery();

                myConnection.Close();

                this.Close();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
        }

        private void btnCancelEditProd_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
