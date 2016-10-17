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

        private string GetMonthByInt(int month)
        {
            switch (month)
            {
                case 1:
                    return "Styczeń";
                case 2:
                    return "Luty";
                case 3: 
                    return "Marzec";
                case 4:
                    return "Kwiecień";
                case 5:
                    return "Maj";
                case 6:
                    return "Czerwiec";
                case 7:
                    return "Lipiec";
                case 8:
                    return "Sierpień";
                case 9:
                    return "Wrzesień";
                case 10:
                    return "Październik";
                case 11:
                    return "Listopad";
                case 12:
                    return "Grudzień";
                default:
                    return "BłędnyMiesiąc";
            }
        }

        private void GetPaymentsList(object sender, RoutedEventArgs e)
        {
            try
            {
                myConnection.Open();
                //SqlCommand myCommand = new SqlCommand("CREATE TABLE Persons_" + DateTime.Today.ToString("yyyyMMdd") + "(PersonID int, LastName varchar(255), FirstName varchar(255), Address varchar(255), City varchar(255));", myConnection);
                SqlCommand myCommand = new SqlCommand("SELECT * FROM Payments WHERE DATEPART(month, Registered) = @month", myConnection);
                myCommand.Parameters.AddWithValue("@month", 10);
                SqlDataAdapter sda = new SqlDataAdapter(myCommand);
                DataTable dt = new DataTable("Payments");
                sda.Fill(dt);
                dgPayments.ItemsSource = dt.DefaultView;
                //myCommand.ExecuteNonQuery();

                SqlCommand CurrMonth = new SqlCommand("Select MONTH(getdate())", myConnection);
                int currMonthIndex = int.Parse(CurrMonth.ExecuteScalar().ToString());

                
                txtCurrMonth.Text = GetMonthByInt(currMonthIndex);

                SqlCommand Balance = new SqlCommand("Select SUM(Cost) from dbo.Payments Where MONTH(Registered)=@month", myConnection);
                Balance.Parameters.AddWithValue("@month", currMonthIndex);
                txtPaymentsSaldo.Text = Balance.ExecuteScalar().ToString();

                myConnection.Close();

            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
        }

        private void AddPayment(object sender, RoutedEventArgs e)
        {
            Dialogs.AddPayment addNewPayment = new Dialogs.AddPayment();
            addNewPayment.Show();
        }

        private void EditPayment(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)dgPayments.SelectedItem;
            Dialogs.EditPayment editPaymWindow = new Dialogs.EditPayment();
            editPaymWindow.txtPaymID.Text = row["PaymentID"].ToString();
            editPaymWindow.txtPaymDate.Text = row["Registered"].ToString();
            editPaymWindow.txtPaymCost.Text = row["Cost"].ToString();

            editPaymWindow.Show();
        }

        private void GetUserList(object sender, RoutedEventArgs e)
        {
            try
            {
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

        private void GetSuppliersList(object sender, RoutedEventArgs e)
        {
            try
            {
                myConnection.Open();
                //SqlCommand myCommand = new SqlCommand("CREATE TABLE Persons_" + DateTime.Today.ToString("yyyyMMdd") + "(PersonID int, LastName varchar(255), FirstName varchar(255), Address varchar(255), City varchar(255));", myConnection);
                SqlCommand myCommand = new SqlCommand("SELECT * FROM Suppliers", myConnection);
                SqlDataAdapter sda = new SqlDataAdapter(myCommand);
                DataTable dt = new DataTable("Suppliers");
                sda.Fill(dt);
                dgSupliers.ItemsSource = dt.DefaultView;
                //myCommand.ExecuteNonQuery();

                myConnection.Close();

            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
        }

        private void AddSupplier(object sender, RoutedEventArgs e)
        {
            Dialogs.AddSupplier addNewSupp = new Dialogs.AddSupplier();
            addNewSupp.Show();
        }

        private void EditSupplier(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)dgSupliers.SelectedItem;
            Dialogs.EditSupplier editSuppWindow = new Dialogs.EditSupplier();
            editSuppWindow.txtSuppID.Text = row["SuppID"].ToString();
            editSuppWindow.txtSuppName.Text = row["Name"].ToString();
            editSuppWindow.txtSuppAddr.Text = row["SAddress"].ToString();

            editSuppWindow.Show();
        }

        private void DelSupplier(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)dgSupliers.SelectedItem;

            myConnection.Open();

            //SqlCommand myCommand = new SqlCommand("CREATE TABLE Persons_" + DateTime.Today.ToString("yyyyMMdd") + "(PersonID int, LastName varchar(255), FirstName varchar(255), Address varchar(255), City varchar(255));", myConnection);
            SqlCommand myCommand = new SqlCommand("DELETE FROM Suppliers WHERE suppID=@sID", myConnection);
            myCommand.Parameters.AddWithValue("@sID", int.Parse(row["suppID"].ToString()));
            myCommand.ExecuteNonQuery();

            myConnection.Close();
        }
    }
}
