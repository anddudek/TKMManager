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

        private void ChangeSelection(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((string)btnChangeSel.Content == "Wszystkie")
                {
                    btnChangeSel.Content = "Miesiąc";
                }
                else
                {
                    btnChangeSel.Content = "Wszystkie";
                }

            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
        }

        private void GetPaymentsList(object sender, RoutedEventArgs e)
        {
            try
            {
                myConnection.Open();
                SqlCommand myCommand;
                SqlCommand CurrMonth = new SqlCommand("Select MONTH(getdate())", myConnection);
                int currMonthIndex = int.Parse(CurrMonth.ExecuteScalar().ToString());

                //SqlCommand myCommand = new SqlCommand("CREATE TABLE Persons_" + DateTime.Today.ToString("yyyyMMdd") + "(PersonID int, LastName varchar(255), FirstName varchar(255), Address varchar(255), City varchar(255));", myConnection);
                if (btnChangeSel.Content == "Miesiąc")
                {
                    myCommand = new SqlCommand("SELECT * FROM Payments WHERE DATEPART(month, Registered) = @month", myConnection);
                    myCommand.Parameters.AddWithValue("@month", currMonthIndex);
                }
                else
                {
                    myCommand = new SqlCommand("SELECT * FROM Payments", myConnection);
                }     
                SqlDataAdapter sda = new SqlDataAdapter(myCommand);
                DataTable dt = new DataTable("Payments");
                sda.Fill(dt);
                dgPayments.ItemsSource = dt.DefaultView;
                //myCommand.ExecuteNonQuery();
                
                txtCurrMonth.Text = GetMonthByInt(currMonthIndex);

                SqlCommand Balance = new SqlCommand("Select SUM(Cost) from dbo.Payments Where MONTH(Registered)=@month", myConnection);
                Balance.Parameters.AddWithValue("@month", currMonthIndex);
                txtPaymentsSaldo.Text = Balance.ExecuteScalar().ToString();

                myConnection.Close();

                updateFirstPage();

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

        private void GetSuppliesList(object sender, RoutedEventArgs e)
        {
            try
            {
                myConnection.Open();
                //SqlCommand myCommand = new SqlCommand("CREATE TABLE Persons_" + DateTime.Today.ToString("yyyyMMdd") + "(PersonID int, LastName varchar(255), FirstName varchar(255), Address varchar(255), City varchar(255));", myConnection);
                SqlCommand myCommand = new SqlCommand("Select p.supplyID, p.supplyDate, p.supplierID, s.Name, p.supplyCost, p.supplyComment from Supplies p inner join Suppliers s on s.suppID = p.supplierID", myConnection);
                SqlDataAdapter sda = new SqlDataAdapter(myCommand);
                DataTable dt = new DataTable("Suppliers");
                sda.Fill(dt);
                dgSuplies.ItemsSource = dt.DefaultView;
                //myCommand.ExecuteNonQuery();

                myConnection.Close();

            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
        }

        private void AddSupply(object sender, RoutedEventArgs e)
        {
            Dialogs.AddSupply addSupplyWindow = new Dialogs.AddSupply();
            addSupplyWindow.Show();
            addSupplyWindow.Closing += new System.ComponentModel.CancelEventHandler(AddSupplyClosing);  
        }

        private void AddSupplyClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GetSuppliesList(null, null);
            GetProductsList(null, null);
            GetPaymentsList(null, null);
        }

        private void ShowSupply(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView row = (DataRowView)dgSuplies.SelectedItem;
                if (row != null)
                {
                    string _id = row["supplyID"].ToString();
                    myConnection.Open();
                    SqlCommand myCommand = new SqlCommand("Select p.ProductID, a.ProductName, p.Amount, p.Cost from Supply_" + _id + " p inner join Products a on a.ProductID = p.ProductID", myConnection);
                    SqlDataAdapter sda = new SqlDataAdapter(myCommand);
                    DataTable dt = new DataTable("Supply_" + _id);
                    sda.Fill(dt);
                    dgSuplies.ItemsSource = dt.DefaultView;
                    myConnection.Close();
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        private void GetOrdersList(object sender, RoutedEventArgs e)
        {
            try
            {
                myConnection.Open();
                //SqlCommand myCommand = new SqlCommand("CREATE TABLE Persons_" + DateTime.Today.ToString("yyyyMMdd") + "(PersonID int, LastName varchar(255), FirstName varchar(255), Address varchar(255), City varchar(255));", myConnection);
                SqlCommand myCommand = new SqlCommand("Select * From Orders", myConnection);
                SqlDataAdapter sda = new SqlDataAdapter(myCommand);
                DataTable dt = new DataTable("Orders");
                sda.Fill(dt);
                dgOrders.ItemsSource = dt.DefaultView;
                //myCommand.ExecuteNonQuery();

                myConnection.Close();

            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
        }

        private void AddOrder(object sender, RoutedEventArgs e)
        {
            Dialogs.AddOrder addOrderWindow = new Dialogs.AddOrder();
            addOrderWindow.Show();
            addOrderWindow.Closing += new System.ComponentModel.CancelEventHandler(AddOrderClosing);            
        }

        private void AddOrderClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GetOrdersList(null, null);
            GetProductsList(null, null);
        }

        private void ShowOrder(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView row = (DataRowView)dgOrders.SelectedItem;
                if (row != null)
                {
                    string _id = row["oID"].ToString();
                    myConnection.Open();
                    SqlCommand myCommand = new SqlCommand("Select p.ProductID, a.ProductName, p.Amount from Order_" + _id + " p inner join Products a on a.ProductID = p.ProductID", myConnection);
                    SqlDataAdapter sda = new SqlDataAdapter(myCommand);
                    DataTable dt = new DataTable("Order_" + _id);
                    sda.Fill(dt);
                    dgOrders.ItemsSource = dt.DefaultView;
                    myConnection.Close();
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        private void DelSupply(object sender, RoutedEventArgs e)
        {
            //Not Implemented
        }

        public void populateTables()
        {
            GetOrdersList(null, null);
            GetSuppliersList(null, null);
            GetSuppliesList(null, null);
            GetUserList(null, null);
            GetProductsList(null, null);
            GetPaymentsList(null, null);
        }

        public void updateFirstPage()
        {
            myConnection.Open();
            SqlCommand CurrMonth = new SqlCommand("Select MONTH(getdate())", myConnection);
            int currMonthIndex = int.Parse(CurrMonth.ExecuteScalar().ToString());
            txtMonth.Text = GetMonthByInt(currMonthIndex);

            SqlCommand Balance = new SqlCommand("Select SUM(Cost) from dbo.Payments Where MONTH(Registered)=@month", myConnection);
            Balance.Parameters.AddWithValue("@month", currMonthIndex);
            txtSaldo.Text = Balance.ExecuteScalar().ToString();

            SqlCommand myCommand = new SqlCommand("SELECT COUNT(*) FROM Orders Where MONTH(orderDate)=@month", myConnection);
            myCommand.Parameters.AddWithValue("@month", currMonthIndex);
            int orderCount = (int)myCommand.ExecuteScalar();
            txtOrdersCount.Text = orderCount.ToString();

            myCommand = new SqlCommand("SELECT COUNT(*) FROM Supplies Where MONTH(supplyDate)=@month", myConnection);
            myCommand.Parameters.AddWithValue("@month", currMonthIndex);
            int suppliesCount = (int)myCommand.ExecuteScalar();
            txtSuppliesCount.Text = suppliesCount.ToString();

            myConnection.Close();
        }

        private void AddNewUser(object sender, RoutedEventArgs e)
        {
            Dialogs.AddUser AddUserWindow = new Dialogs.AddUser();
            AddUserWindow.Show();
            AddUserWindow.Closing += new System.ComponentModel.CancelEventHandler(AddUserClosing);
        }

        private void AddUserClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GetUserList(null, null);
        }

        private void EditUser(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)dgUsers.SelectedItem;
            myConnection.Open();

            SqlCommand myCommand = new SqlCommand("Update Users Set pw='temp' WHERE userID=@uID", myConnection);
            myCommand.Parameters.AddWithValue("@uID", int.Parse(row["userID"].ToString()));
            myCommand.ExecuteNonQuery();

            myConnection.Close();
            //Console.WriteLine(row["ProductID"].ToString());
        }

        private void DeleteUser(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)dgUsers.SelectedItem;

            myConnection.Open();

            //SqlCommand myCommand = new SqlCommand("CREATE TABLE Persons_" + DateTime.Today.ToString("yyyyMMdd") + "(PersonID int, LastName varchar(255), FirstName varchar(255), Address varchar(255), City varchar(255));", myConnection);
            SqlCommand myCommand = new SqlCommand("DELETE FROM Users WHERE userID=@uID", myConnection);
            myCommand.Parameters.AddWithValue("@uID", int.Parse(row["userID"].ToString()));
            myCommand.ExecuteNonQuery();

            myConnection.Close();
            GetUserList(null, null);
        }
    }
}
