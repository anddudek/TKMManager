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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace TKMManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SqlConnection myConnection;
        public MainWindow()
        {
           myConnection = new SqlConnection("user id=tkmmanagerdb;" +
                                       "password=Oz781It_2!30;server=mssql3.gear.host;" +
                                       "Trusted_Connection=no;" +
                                       "database=tkmmanagerdb; " +
                                       "connection timeout=30");
            InitializeComponent();
        }

        private void GetProductsList(object sender, RoutedEventArgs e)
        {
            try
            {
                myConnection.Open();           

                //SqlCommand myCommand = new SqlCommand("CREATE TABLE Persons_" + DateTime.Today.ToString("yyyyMMdd") + "(PersonID int, LastName varchar(255), FirstName varchar(255), Address varchar(255), City varchar(255));", myConnection);
                SqlCommand myCommand = new SqlCommand("SELECT * FROM Products", myConnection);
                SqlDataAdapter sda = new SqlDataAdapter(myCommand);
                DataTable dt = new DataTable("Products");
                sda.Fill(dt);
                dgProducts.ItemsSource = dt.DefaultView;
                //myCommand.ExecuteNonQuery();

                myConnection.Close();

            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
        }

        private void AddNewProduct(object sender, RoutedEventArgs e)
        {
            Dialogs.AddProduct addNewProduct = new Dialogs.AddProduct();
            addNewProduct.Show();
        }

        private void EditProduct(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)dgProducts.SelectedItem;
            Dialogs.EditProduct editProdWindow = new Dialogs.EditProduct();
            editProdWindow.txtProdID.Text = row["ProductID"].ToString();
            editProdWindow.txtEditProdName.Text = row["ProductName"].ToString();
            editProdWindow.txtEditProdAmount.Text = row["WarehouseAmount"].ToString();
            editProdWindow.Show();

            //Console.WriteLine(row["ProductID"].ToString());
        }

        private void DeleteProduct(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)dgProducts.SelectedItem;

            myConnection.Open();

            //SqlCommand myCommand = new SqlCommand("CREATE TABLE Persons_" + DateTime.Today.ToString("yyyyMMdd") + "(PersonID int, LastName varchar(255), FirstName varchar(255), Address varchar(255), City varchar(255));", myConnection);
            SqlCommand myCommand = new SqlCommand("DELETE FROM Products WHERE ProductID=@prodID", myConnection);
            myCommand.Parameters.AddWithValue("@prodID", int.Parse(row["ProductID"].ToString()));
            myCommand.ExecuteNonQuery();

            myConnection.Close();
        }

    }
}
