﻿using System;
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
    public partial class AddSupplier : Window
    {
        public AddSupplier()
        {
            InitializeComponent();
        }

        private void btnAddSupp_Click(object sender, RoutedEventArgs e)
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
                SqlCommand myCommand = new SqlCommand("INSERT INTO Suppliers (Name, SAddress) VALUES (@name, @addr)", myConnection);
                myCommand.Parameters.AddWithValue("@Name", txtSuppName.Text);
                myCommand.Parameters.AddWithValue("@addr", txtSuppAddr.Text);
                myCommand.ExecuteNonQuery();

                myConnection.Close();

                this.Close();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
        }

        private void btnCancelAddSupp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
