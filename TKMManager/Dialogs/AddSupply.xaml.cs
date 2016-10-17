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
    public partial class AddSupply : Window
    {
        List<string> productsList = new List<string>();
        List<string> suppliersList = new List<string>();

        public AddSupply()
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

                myCommand = new SqlCommand("SELECT * FROM Suppliers", myConnection);
                sda = new SqlDataAdapter(myCommand);
                ds = new DataSet();
                sda.Fill(ds, "Suppliers");
                cmbSuppliers.Items.Clear();
                foreach (DataRow row in ds.Tables["Suppliers"].Rows)
                {
                    //productsList.Add(row["ProductName"].ToString());
                    cmbSuppliers.Items.Add(row["Name"].ToString());
                }

                myConnection.Close();


            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
        }

        private void supplyAddSupplier(object sender, RoutedEventArgs e)
        {
            Dialogs.AddSupplier windowAddSupply = new AddSupplier();
            windowAddSupply.Show();
        }

        private void supplyAddProduct(object sender, RoutedEventArgs e)
        {
            AddProduct windowAddProduct = new AddProduct();
            windowAddProduct.Show();
        }

        private void supplyAddSupplyToList(object sender, RoutedEventArgs e)
        {
            var dataPoint = new SupplyProduct(cmbProducts.SelectedItem.ToString(), supplyAmount.Text, supplyCost.Text);
            supplyList.Items.Add(dataPoint);
            txtSum.Text = CalculateSum().ToString();
        }

        private void supplyEditSupplyFromList(object sender, RoutedEventArgs e)
        {
            SupplyProduct selected = (SupplyProduct)supplyList.SelectedItem;
            SupplyProduct sp = new SupplyProduct(cmbProducts.SelectedItem.ToString(), supplyAmount.Text, supplyCost.Text);
            int index = supplyList.SelectedIndex;
            supplyList.Items.RemoveAt(index);
            supplyList.Items.Insert(index, sp);
            txtSum.Text = CalculateSum().ToString();
        }

        private void supplyDelSupplyFromList(object sender, RoutedEventArgs e)
        {
            supplyList.Items.RemoveAt(supplyList.SelectedIndex);
            txtSum.Text = CalculateSum().ToString();
        }

        private void supplySaveSupply(object sender, RoutedEventArgs e)
        {
            SqlConnection myConnection = new SqlConnection("user id=tkmmanagerdb;" +
                                       "password=Oz781It_2!30;server=mssql3.gear.host;" +
                                       "Trusted_Connection=no;" +
                                       "database=tkmmanagerdb; " +
                                       "connection timeout=30");
            try
            {
                myConnection.Open();

                SqlCommand myCommand = new SqlCommand("SELECT COUNT(*) FROM Supplies", myConnection);
                int supplyID = (int)myCommand.ExecuteScalar()+1;
                DateTime curr = DateTime.Now;
                myCommand = new SqlCommand("INSERT INTO Payments (Registered, Cost, Comment) VALUES (@date, @cost, @comment)", myConnection);
                myCommand.Parameters.Add("@date", curr);
                myCommand.Parameters.Add("@cost", -CalculateSum());
                myCommand.Parameters.Add("@comment", "Dostawa nr " + (supplyID).ToString());
                myCommand.ExecuteNonQuery();

                myCommand = new SqlCommand("SELECT suppID FROM Suppliers WHERE Name=@name", myConnection);
                myCommand.Parameters.Add("@name", cmbSuppliers.SelectedItem.ToString());
                int supplierID = (int)myCommand.ExecuteScalar();

                myCommand = new SqlCommand("INSERT INTO Supplies (supplyDate, supplierID, supplyCost, supplyComment) VALUES (@date, @sid, @cost, @comm)", myConnection);
                myCommand.Parameters.Add("@date", curr);
                myCommand.Parameters.Add("@cost", -CalculateSum());
                myCommand.Parameters.Add("@sid", supplierID);
                myCommand.Parameters.Add("@comm", txtComment.Text);
                myCommand.ExecuteNonQuery();

                myCommand = new SqlCommand("CREATE TABLE [dbo].[Supply_" + supplyID.ToString() + "] (ProductID int not null, Amount int not null, Cost float not null)", myConnection);
                //myCommand.Parameters.Add("@table", "Supply_" + supplyID.ToString());
                myCommand.ExecuteNonQuery();
                int pid = 0;

                foreach (SupplyProduct row in supplyList.Items)
                {
                    myCommand = new SqlCommand("SELECT ProductID FROM Products WHERE ProductName=@name", myConnection);
                    myCommand.Parameters.Add("@name", row.ProductName);
                    pid = (int)myCommand.ExecuteScalar();
                    myCommand = new SqlCommand("INSERT INTO [dbo].[Supply_" + supplyID.ToString() + "] (ProductID, Amount, Cost) VALUES (@pid, @amount, @cost)", myConnection);
                    myCommand.Parameters.Add("@table", "Supply_" + supplyID.ToString());
                    myCommand.Parameters.Add("@pid", pid);
                    myCommand.Parameters.Add("@amount", int.Parse(row.Amount));
                    myCommand.Parameters.Add("@cost", double.Parse(row.Cost.Replace(',', '.')));
                    myCommand.ExecuteNonQuery();
                }
                //
                //TRZEBA DODAC UPDATE W PRODUKTACH ( DATA ZAMOWIENIA I STAN)
                //
                myConnection.Close();


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
        private double CalculateSum()
        {
            double sum = 0;
            foreach (SupplyProduct row in supplyList.Items)
            {
                sum += double.Parse(row.Cost.Replace(',', '.'));
            }
            return sum;
        }

        public class SupplyProduct
        {
            public string ProductName {get; set; }
            public string Amount { get; set; }
            public string Cost { get; set; }

            public SupplyProduct(string _name, string _amnt, string _cost)
            {
                ProductName = _name;
                Amount = _amnt;
                Cost = _cost;
            }
        }
    }
}
