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
    /// Interaction logic for AddSupply.xaml
    /// </summary>
    public partial class AddOrder : Window
    {
        List<string> productsList = new List<string>();
        List<string> suppliersList = new List<string>();

        public AddOrder()
        {
            InitializeComponent();
            PopulateComboBoxes();
            
        }

        public void PopulateComboBoxes()
        {
            SqlConnection myConnection = new SqlConnection("user id=tkmmanagerdb;" +
                                       "password=Oz781It_2!30;server=mssql3.gear.host;" +
                                       "Trusted_Connection=no;" +
                                       "database=tkmmanagerdb; " +
                                       "connection timeout=30");
            try
            {
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand("SELECT * FROM Products", myConnection);
                SqlDataAdapter sda = new SqlDataAdapter(myCommand);
                DataSet ds = new DataSet();
                sda.Fill(ds, "Products");
                cmbProducts.Items.Clear();
                foreach (DataRow row in ds.Tables["Products"].Rows)
                {
                    //productsList.Add(row["ProductName"].ToString());
                    cmbProducts.Items.Add(row["ProductName"].ToString());
                }

                myCommand = new SqlCommand("SELECT * FROM Users", myConnection);
                sda = new SqlDataAdapter(myCommand);
                ds = new DataSet();
                sda.Fill(ds, "Orderers");
                cmbOrderer.Items.Clear();
                foreach (DataRow row in ds.Tables["Orderers"].Rows)
                {
                    //productsList.Add(row["ProductName"].ToString());
                    cmbOrderer.Items.Add(row["Name"].ToString());
                    cmbMaker.Items.Add(row["Name"].ToString());
                }

                myConnection.Close();


            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
        }

        private void orderAddOrderToList(object sender, RoutedEventArgs e)
        {
            var dataPoint = new OrderProduct(cmbProducts.SelectedItem.ToString(), orderAmount.Text);
            orderList.Items.Add(dataPoint);
        }

        private void orderEditOrderFromList(object sender, RoutedEventArgs e)
        {
            OrderProduct selected = (OrderProduct)orderList.SelectedItem;
            OrderProduct sp = new OrderProduct(cmbProducts.SelectedItem.ToString(), orderAmount.Text);
            int index = orderList.SelectedIndex;
            orderList.Items.RemoveAt(index);
            orderList.Items.Insert(index, sp);
        }

        private void orderDelOrderFromList(object sender, RoutedEventArgs e)
        {
            orderList.Items.RemoveAt(orderList.SelectedIndex);
        }

        private void orderSaveOrder(object sender, RoutedEventArgs e)
        {
            SqlConnection myConnection = new SqlConnection("user id=tkmmanagerdb;" +
                                       "password=Oz781It_2!30;server=mssql3.gear.host;" +
                                       "Trusted_Connection=no;" +
                                       "database=tkmmanagerdb; " +
                                       "connection timeout=30");
            try
            {
                myConnection.Open();

                SqlCommand myCommand = new SqlCommand("SELECT COUNT(*) FROM Orders", myConnection);
                int orderID = (int)myCommand.ExecuteScalar()+1;
                DateTime curr = DateTime.Now;

                myCommand = new SqlCommand("INSERT INTO Orders (orderBy, doneBy, orderDate, comment) VALUES (@oBy, @dBy, @date, @comm)", myConnection);
                myCommand.Parameters.AddWithValue("@date", curr);
                myCommand.Parameters.AddWithValue("@oBy", cmbOrderer.SelectedItem.ToString());
                myCommand.Parameters.AddWithValue("@dBy", cmbMaker.SelectedItem.ToString());
                myCommand.Parameters.AddWithValue("@comm", txtComment.Text);
                myCommand.ExecuteNonQuery();

                myCommand = new SqlCommand("CREATE TABLE [dbo].[Order_" + orderID.ToString() + "] (ProductID int not null, Amount int not null)", myConnection);
                myCommand.ExecuteNonQuery();
                int pid = 0;

                foreach (OrderProduct row in orderList.Items)
                {
                    myCommand = new SqlCommand("SELECT ProductID FROM Products WHERE ProductName=@name", myConnection);
                    myCommand.Parameters.AddWithValue("@name", row.ProductName);
                    pid = (int)myCommand.ExecuteScalar();
                    myCommand = new SqlCommand("INSERT INTO [dbo].[Order_" + orderID.ToString() + "] (ProductID, Amount) VALUES (@pid, @amount)", myConnection);
                    
                    myCommand.Parameters.AddWithValue("@pid", pid);
                    myCommand.Parameters.AddWithValue("@amount", int.Parse(row.Amount));
                    myCommand.ExecuteNonQuery();

                    myCommand = new SqlCommand("update Products Set LastOrder=@date, WarehouseAmount=WarehouseAmount-@amount Where ProductID = @pid", myConnection);
                    myCommand.Parameters.AddWithValue("@date", curr);
                    myCommand.Parameters.AddWithValue("@amount", int.Parse(row.Amount));
                    myCommand.Parameters.AddWithValue("@pid", pid);
                    myCommand.ExecuteNonQuery();

                }
                //
                //TRZEBA DODAC UPDATE W PRODUKTACH ( DATA ZAMOWIENIA I STAN)
                //
                myConnection.Close();
                
                this.Close();


            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
        }

        private void refresh(object sender, RoutedEventArgs e)
        {
            PopulateComboBoxes();
        }

        public class OrderProduct
        {
            public string ProductName {get; set; }
            public string Amount { get; set; }

            public OrderProduct(string _name, string _amnt)
            {
                ProductName = _name;
                Amount = _amnt;
            }
        }
    }
}
