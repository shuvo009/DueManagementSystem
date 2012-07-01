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
using System.Collections.ObjectModel;
using EasyShopManagement.Class;
using EasyShopManagement.Class.Models;
using EasyShopManagement.Class.Searchs;
using EasyShopManagement.EntityFramework;
using System.ComponentModel;
using EasyShopManagement.Reports;
using System.Collections;
using DevExpress.Xpf.Printing;
using System.IO;
namespace EasyShopManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            string fileName =System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),EasyShopManagement.Class.CommandData.SOFTWARE_NAME);
            AppDomain.CurrentDomain.SetData("DataDirectory", fileName);
        }

        #region public Property
        private string _SelectedRadioButton = "ByDate";
        public string SelectedRadioButton  
        {
            get { return this._SelectedRadioButton; }
            set { this._SelectedRadioButton = value; }
        }
        #endregion

        #region Main Menu
        public ICommand MainMenuCommand
        {
            get { return new ReplayCommand(new Action<object>(this.mainMenuClick)); }
        }

        /// <summary>
        /// Main Manu 
        /// </summary>
        /// <param name="obj"></param>
        private void mainMenuClick(object obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                var visiablePanels = this.PanelRootGrid.Children.OfType<Grid>().Where(x => x.Visibility.Equals(Visibility.Visible));
                foreach (var panel in visiablePanels)
                {
                    panel.Visibility = Visibility.Hidden;
                }
                using (DueManagementEntity dmDatabase = new DueManagementEntity())
                {
                    switch (obj.ToString())
                    {
                        case "Home":
                            this.PanelHome.Visibility = Visibility.Visible;
                            break;
                        case "Exit":
                            this.PanelHome.Visibility = Visibility.Visible;
                            this.Close();
                            break;
                        case "DatabaseBackup":
                            this.PanelDatabaseBackup.Visibility = Visibility.Visible;
                            break;
                        case "DatabaseRestore":
                            this.PanelDatabaseRestore.Visibility = Visibility.Visible;
                            break;
                        case "About":
                            this.PanelAbout.Visibility = Visibility.Visible;
                            break;
                        case "NewCustomer":
                            var customersInfo = new ObservableCollection<ModelCustomerIno>(from custInfo in dmDatabase.CustomerInformations
                                                                                                          select new ModelCustomerIno 
                                                                                                          {
                                                                                                              Address=custInfo.Address,
                                                                                                              CreateDate=custInfo.CreateDate,
                                                                                                              CustomerName=custInfo.CustomerName,
                                                                                                              DueAmount=custInfo.DueAmount,
                                                                                                              ID=custInfo.AutoInc,
                                                                                                              Mobile=custInfo.Mobile,
                                                                                                              Remark=custInfo.Remark,
                                                                                                              ShopName=custInfo.ShopName
                                                                                                          });

                            this.newCustomerList.ItemsSource = customersInfo;
                            ICollectionView usesInfoView = CollectionViewSource.GetDefaultView(customersInfo);
                            new CustomerSearch(usesInfoView, this.newCustomerSearch);
                            this.selectFirstItem(this.newCustomerList);
                            this.PanelNewCustomer.Visibility = Visibility.Visible;
                            break;
                        case "NewProduct":
                             var productsInfo= new ObservableCollection<ModelProductInfo>(from prodInfo in dmDatabase.ProductInformations
                                                                                                         select new ModelProductInfo 
                                                                                                         {
                                                                                                             ID=prodInfo.AutoInc,
                                                                                                             ProductName=prodInfo.ProductName,
                                                                                                             Rate=prodInfo.Rate
                                                                                                         });
                            this.NewProductList.ItemsSource = productsInfo;
                            ICollectionView productsInfoView = CollectionViewSource.GetDefaultView(productsInfo);
                            new ProcuctSearch(productsInfoView, this.NewProductSearch);
                            this.selectFirstItem(this.NewProductList);
                            this.PanelNewProduct.Visibility = Visibility.Visible;
                            break;
                        case "Invoice":
                            var invoiceProductsInfo = new ObservableCollection<ModelProductInfo>(from prodInfo in dmDatabase.ProductInformations
                                                                                          select new ModelProductInfo
                                                                                          {
                                                                                              ID = prodInfo.AutoInc,
                                                                                              ProductName = prodInfo.ProductName,
                                                                                              Rate = prodInfo.Rate
                                                                                          });
                            this.InvoiceCustomer.ItemsSource = new ObservableCollection<ModelCustomerIno>(from custInfo in dmDatabase.CustomerInformations
                                                                                                          select new ModelCustomerIno
                                                                                                          {
                                                                                                              Address = custInfo.Address,
                                                                                                              CustomerName = custInfo.CustomerName,
                                                                                                              ID = custInfo.AutoInc
                                                                                                          });
                            this.InvoiceProductList.ItemsSource = invoiceProductsInfo;
                            ICollectionView invoiceProductView = CollectionViewSource.GetDefaultView(invoiceProductsInfo);
                            new ProcuctSearch(invoiceProductView, this.InvoiceProductSearch);
                            this.newInvoiceClick(null);
                            this.selectFirstItem(this.newCustomerList);
                            this.PanelInvoice.Visibility = Visibility.Visible;
                            break;
                        case "Payment":
                            this.PaymentCustomers.ItemsSource = new ObservableCollection<ModelCustomerIno>(from custInfo in dmDatabase.CustomerInformations
                                                                                                            where custInfo.DueAmount >0
                                                                                                            select new ModelCustomerIno
                                                                                                            {
                                                                                                                Address = custInfo.Address,
                                                                                                                CustomerName = custInfo.CustomerName,
                                                                                                                ID = custInfo.AutoInc,
                                                                                                                DueAmount=custInfo.DueAmount
                                                                                                            });
                            this.PanelPayment.Visibility = Visibility.Visible;
                            break;
                        case "ProductSaleHistory":
                            this.SaleHistoryCustomerList.ItemsSource = new ObservableCollection<ModelCustomerIno>(from custInfo in dmDatabase.CustomerInformations
                                                                                                          select new ModelCustomerIno
                                                                                                          {
                                                                                                              Address = custInfo.Address,
                                                                                                              CustomerName = custInfo.CustomerName,
                                                                                                              ID = custInfo.AutoInc
                                                                                                          });
                            this.PanelProductSaleHistory.Visibility = Visibility.Visible;
                            break;
                        case "PaymentHistory":
                            this.PaymentHisCutomerList.ItemsSource = new ObservableCollection<ModelCustomerIno>(from custInfo in dmDatabase.CustomerInformations
                                                                                                                  select new ModelCustomerIno
                                                                                                                  {
                                                                                                                      Address = custInfo.Address,
                                                                                                                      CustomerName = custInfo.CustomerName,
                                                                                                                      ID = custInfo.AutoInc
                                                                                                                  });
                            this.PanelPaymentHistory.Visibility = Visibility.Visible;
                            break;
                        default:
                            this.PanelHome.Visibility = Visibility.Visible;
                            break;
                    }
                }
            }
            catch (Exception errorException)
            {
                Mouse.OverrideCursor = null;
                //System.Diagnostics.EventLog.WriteEntry("MyEventSource", errorException.StackTrace,
                //                       System.Diagnostics.EventLogEntryType.Warning); 
                MessageBox.Show(errorException.Message, CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }
        #endregion

        #region Panel Customer Information
        public ICommand NewCustomerCommand
        {
            get { return new ReplayCommand(new Action<object>(this.newCustomerClick)); }
        }
        public ICommand UpdateCustomerCommand
        {
            get { return new ReplayCommand(new Action<object>(this.updateCustomerClick)); }
        }
        public ICommand DeleteCustomerCommand
        {
            get { return new ReplayCommand(new Action<object>(this.deleteCustomerClick)); }
        }
        /// <summary>
        /// add New Customer
        /// </summary>
        /// <param name="obj"></param>
        private void newCustomerClick(object obj)
        {
            this.newCustomerNew.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                ModelCustomerIno newCustomer = new ModelCustomerIno();
                (this.newCustomerList.ItemsSource as ObservableCollection<ModelCustomerIno>).Add(newCustomer);
                this.newCustomerList.SelectedIndex = this.newCustomerList.Items.IndexOf(newCustomer);
                this.newCustomerName.Focus();
            }
            catch (NullReferenceException)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(CommandData.ERROR_MESSAGE[5], CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            catch (Exception errorException)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(errorException.Message, CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.newCustomerNew.IsEnabled = true;
            }
        }
        /// <summary>
        /// Update or Insert Customer information
        /// </summary>
        /// <param name="obj"></param>
        private void updateCustomerClick(object obj)
        {
            this.newCustomerUpdate.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                this.newCustomerName.GetBindingExpression(TextBox.TextProperty);
                this.getValidationError(this.newCustomerName);
                using (DueManagementEntity dmDatabase = new DueManagementEntity())
                {
                    ModelCustomerIno selectedCustomer = obj as ModelCustomerIno;
                    var custometIsExist = dmDatabase.CustomerInformations.FirstOrDefault(x => x.AutoInc== selectedCustomer.ID);
                    if (custometIsExist!=null)
                    {
                        Mouse.OverrideCursor = null;
                        if (MessageBox.Show(CommandData.ERROR_MESSAGE[0],CommandData.SOFTWARE_NAME,MessageBoxButton.YesNo,MessageBoxImage.Question)==MessageBoxResult.Yes)
                        {
                            Mouse.OverrideCursor = Cursors.Wait;
                            custometIsExist.Address = selectedCustomer.Address;
                            custometIsExist.CustomerName = selectedCustomer.CustomerName;
                            custometIsExist.DueAmount = selectedCustomer.DueAmount;
                            custometIsExist.Mobile = selectedCustomer.Mobile;
                            custometIsExist.Remark = selectedCustomer.Remark;
                            custometIsExist.ShopName = selectedCustomer.ShopName;
                            dmDatabase.SaveChanges();
                            Mouse.OverrideCursor = null;
                            MessageBox.Show(CommandData.ERROR_MESSAGE[1], CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        CustomerInformation newCustomerInformation = new CustomerInformation
                                                                    {
                                                                        Address = selectedCustomer.Address,
                                                                        AutoInc = default(long),
                                                                        CreateDate = DateTime.Today,
                                                                        CustomerName = selectedCustomer.CustomerName,
                                                                        DueAmount = selectedCustomer.DueAmount,
                                                                        Mobile = selectedCustomer.Mobile,
                                                                        Remark = selectedCustomer.Remark,
                                                                        ShopName = selectedCustomer.ShopName
                                                                    };
                        dmDatabase.CustomerInformations.AddObject(newCustomerInformation);
                        dmDatabase.SaveChanges();
                        selectedCustomer.ID = newCustomerInformation.AutoInc;
                        Mouse.OverrideCursor = null;
                        MessageBox.Show(CommandData.ERROR_MESSAGE[2], CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (NullReferenceException)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(CommandData.ERROR_MESSAGE[5], CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            catch (Exception errorException)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(errorException.Message, CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }
        /// <summary>
        /// delete customer information
        /// </summary>
        /// <param name="obj"></param>
        private void deleteCustomerClick(object obj)
        {
            this.newCustomerDelete.IsEnabled = false;
            try
            {
                if (MessageBox.Show(CommandData.ERROR_MESSAGE[3], CommandData.SOFTWARE_NAME, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    using (DueManagementEntity dmDatabase = new DueManagementEntity())
                    {
                        ModelCustomerIno deletedCustomer = obj as ModelCustomerIno;
                        dmDatabase.CustomerInformations.DeleteObject(dmDatabase.CustomerInformations.First(x => x.AutoInc == deletedCustomer.ID));
                        dmDatabase.SaveChanges();
                        (this.newCustomerList.ItemsSource as ObservableCollection<ModelCustomerIno>).Remove(deletedCustomer);
                        this.selectFirstItem(this.newCustomerList);
                        Mouse.OverrideCursor = null;
                        MessageBox.Show(CommandData.ERROR_MESSAGE[4], CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    return;
                }
            }
            catch (NullReferenceException)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(CommandData.ERROR_MESSAGE[5], CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            catch (Exception errorExcption)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(errorExcption.Message, CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.newCustomerDelete.IsEnabled = true;
            }
        }
        #endregion

        #region Panel New Product

        public ICommand NewProductCommnad
        {
            get { return new ReplayCommand(new Action<object>(this.newProductClick)); }
        }

        public ICommand UpdateProductCommand
        {
            get { return new ReplayCommand(new Action<object>(this.updateProductClick)); }
        }

        public ICommand DeleteProductCommand
        {
            get { return new ReplayCommand(new Action<object>(this.deleteProductClick)); }
        }
        /// <summary>
        /// new Product Information
        /// </summary>
        /// <param name="obj"></param>
        private void newProductClick(object obj)
        {
            this.NewProductNew.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                ModelProductInfo newProductInfo = new ModelProductInfo();
                (this.NewProductList.ItemsSource as ObservableCollection<ModelProductInfo>).Add(newProductInfo);
                this.NewProductList.SelectedIndex = this.NewProductList.Items.IndexOf(newProductInfo);
                this.NewProductName.Focus();
            }
            catch (NullReferenceException)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(CommandData.ERROR_MESSAGE[5], CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            catch (Exception errorMessagess)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(errorMessagess.Message, CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally 
            {
                Mouse.OverrideCursor = null;
                this.NewProductNew.IsEnabled = true;
            }
        }
        /// <summary>
        /// Update or Insert Product information
        /// </summary>
        /// <param name="obj"></param>
        private void updateProductClick(object obj)
        {
            this.NewProductUpdate.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                this.NewProductName.GetBindingExpression(TextBox.TextProperty);
                this.getValidationError(this.NewProductName);
                using (DueManagementEntity dmDatabase = new DueManagementEntity())
                {
                    ModelProductInfo selectedProductInfo = obj as ModelProductInfo;
                    var productExist = dmDatabase.ProductInformations.FirstOrDefault(x => x.AutoInc == selectedProductInfo.ID);
                    if (productExist != null)
                    {
                        Mouse.OverrideCursor = null;
                        if (MessageBox.Show(CommandData.ERROR_MESSAGE[0], CommandData.SOFTWARE_NAME, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            Mouse.OverrideCursor = Cursors.Wait;
                            productExist.ProductName = selectedProductInfo.ProductName;
                            productExist.Rate = selectedProductInfo.Rate;
                            dmDatabase.SaveChanges();
                            Mouse.OverrideCursor = null;
                            MessageBox.Show(CommandData.ERROR_MESSAGE[1], CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        ProductInformation newProductInfo = new ProductInformation
                                                                {
                                                                    AutoInc = default(long),
                                                                    ProductName = selectedProductInfo.ProductName,
                                                                    Rate = selectedProductInfo.Rate
                                                                };
                        dmDatabase.ProductInformations.AddObject(newProductInfo);
                        dmDatabase.SaveChanges();
                        selectedProductInfo.ID = newProductInfo.AutoInc;
                        Mouse.OverrideCursor = null;
                        MessageBox.Show(CommandData.ERROR_MESSAGE[2], CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (NullReferenceException)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(CommandData.ERROR_MESSAGE[5], CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            catch (Exception errorMessagess)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(errorMessagess.Message, CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }
        /// <summary>
        /// Delete Product information
        /// </summary>
        /// <param name="obj"></param>
        private void deleteProductClick(object obj)
        {
            this.NewProductDelete.IsEnabled = false;
            try
            {
                if (MessageBox.Show(CommandData.ERROR_MESSAGE[3], CommandData.SOFTWARE_NAME, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    using (DueManagementEntity dmDatabase = new DueManagementEntity())
                    {
                        ModelProductInfo deletedProductInfo = obj as ModelProductInfo;
                        dmDatabase.ProductInformations.DeleteObject(dmDatabase.ProductInformations.First(x => x.AutoInc == deletedProductInfo.ID));
                        dmDatabase.SaveChanges();
                        (this.NewProductList.ItemsSource as ObservableCollection<ModelProductInfo>).Remove(deletedProductInfo);
                        this.selectFirstItem(this.newCustomerList);
                        Mouse.OverrideCursor = null;
                        MessageBox.Show(CommandData.ERROR_MESSAGE[4], CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (NullReferenceException)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(CommandData.ERROR_MESSAGE[5], CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            catch (Exception errorMessagess)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(errorMessagess.Message, CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.NewProductDelete.IsEnabled = true;
            }
        }
        #endregion

        #region Panel Invoice

        public ICommand AddItemCommand
        {
            get { return new ReplayCommand(new Action<object>(this.addItemClick)); }
        }

        public ICommand NewInvoiceCommand
        {
            get { return new ReplayCommand(new Action<object>(this.newInvoiceClick)); }
        }

        public ICommand UpdateInvoiceCommnad
        {
            get { return new ReplayCommand(new Action<object>(this.updateInvoiceClick)); }
        }

        public ICommand PrintInvoiceCommnad
        {
            get { return new ReplayCommand(new Action<object>(this.printInvoiceClick)); }
        }

        public ICommand DeleteInvoiceItemCommnad
        {
            get { return new ReplayCommand(new Action<object>(this.deleteInvoiceItemClick)); }
        }
        /// <summary>
        /// Add item In InvoiceGrid
        /// </summary>
        /// <param name="obj">
        /// obj[0] = CustomerName
        /// obj[1] = ModelProductInfo
        /// obj[2] = Quantity
        /// </param>
        private void addItemClick(object obj)
        {
            this.InvoiceProductAdd.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {

                if (obj is ArrayList)
                {
                    ArrayList dataList = obj as ArrayList;
                    ModelProductInfo seletedProduct = dataList[1] as ModelProductInfo;
                    ModelSalesHistory newSaleItem = new ModelSalesHistory
                                                    {
                                                        ID=seletedProduct.ID,
                                                        CustomerName = dataList[0].ToString(),
                                                        PropductName = seletedProduct.ProductName,
                                                        Quantity = Convert.ToDecimal(dataList[2]),
                                                        Rate = seletedProduct.Rate,
                                                        SaleDate = DateTime.Today,
                                                        Amount=0
                                                    };
                    var invoiceGridSource=this.InvoiceGrid.ItemsSource as ObservableCollection<ModelSalesHistory>;
                    if (invoiceGridSource.FirstOrDefault(x=>x.ID == newSaleItem.ID)==null)
                    {
                        invoiceGridSource.Add(newSaleItem);
                    }
                    else
                    {
                        if (MessageBox.Show("Item already exists are you want to update ?",CommandData.SOFTWARE_NAME,MessageBoxButton.YesNo,MessageBoxImage.Question)==MessageBoxResult.Yes)
                        {
                            invoiceGridSource.Remove(invoiceGridSource.First(x => x.ID == newSaleItem.ID));
                            invoiceGridSource.Add(newSaleItem);
                        }
                        else
                        {
                            return;
                        }
                    }
                    this.InvoiceTotalAmount.Text = (this.InvoiceGrid.ItemsSource as ObservableCollection<ModelSalesHistory>).Sum(x => x.Amount).ToString();
                    this.InvoiceCustomer.IsEnabled = false;
                }
                else
                {
                    Mouse.OverrideCursor = null;
                    MessageBox.Show("Unable to add in invoice", CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Stop);
                }

            }
            catch (NullReferenceException)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(CommandData.ERROR_MESSAGE[5], CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Stop);
                this.InvoiceCustomer.IsEnabled = true;
            }
            catch (Exception errorException)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(errorException.Message, CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.InvoiceProductAdd.IsEnabled = true;
            }
        }
        /// <summary>
        /// New Invoice 
        /// </summary>
        /// <param name="obj"></param>
        private void newInvoiceClick(object obj)
        {
            this.InvoiceNew.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                this.InvoiceGrid.IsEnabled = true;
                this.InvoiceGrid.ItemsSource = null;
                this.InvoiceGrid.ItemsSource = new ObservableCollection<ModelSalesHistory>();
                this.InvoiceUpdate.IsEnabled = false;
                this.InvoiceCustomer.IsEnabled = true;
            }
            catch (NullReferenceException)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(CommandData.ERROR_MESSAGE[5], CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            catch (Exception errorException)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(errorException.Message, CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.InvoiceNew.IsEnabled = true;
            }
        }
        /// <summary>
        /// Insert invoice Reports
        /// </summary>
        /// <param name="obj">
        /// obj[0] = invoiceGrid.ItemSource
        /// obj[1] = Payment Amount
        /// obj[2] = Due Amount
        /// obj[3] = ModelCustomerInfo
        /// </param>
        private void updateInvoiceClick(object obj)
        {
            Mouse.OverrideCursor = null;
            try
            {
                if (obj is ArrayList)
                {
                    using (DueManagementEntity dmDatabase = new DueManagementEntity())
                    {
                        ArrayList dataList = obj as ArrayList;
                        ObservableCollection<ModelSalesHistory> salesItems = dataList[0] as ObservableCollection<ModelSalesHistory>;
                        ModelCustomerIno selectedCustomer = dataList[3] as ModelCustomerIno;
                        foreach (ModelSalesHistory sale in salesItems)
                        {
                            dmDatabase.AddToProductSaleHistories(new ProductSaleHistory
                                                                                      {
                                                                                          AutoInc = default(long),
                                                                                          CustomerName = sale.CustomerName,
                                                                                          ProductName = sale.PropductName,
                                                                                          Quantity = sale.Quantity,
                                                                                          Rate = sale.Rate,
                                                                                          SaleDate = sale.SaleDate,
                                                                                          CustomerID=selectedCustomer.ID
                                                                                      });
                        }
                        var customerInfo = dmDatabase.CustomerInformations.First(x => x.AutoInc == selectedCustomer.ID);
                        customerInfo.DueAmount = +Convert.ToDecimal(dataList[2]);
                        dmDatabase.AddToPaymentHistories(new PaymentHistory 
                                                                          {
                                                                              AutoInc=default(long),
                                                                              CustomerName=selectedCustomer.CustomerName,
                                                                              Amount=Convert.ToDecimal(dataList[1]),
                                                                              PaymentDate=DateTime.Today,
                                                                              CustomerID=selectedCustomer.ID
                                                                          });
                        dmDatabase.SaveChanges();
                        Mouse.OverrideCursor = null;
                        MessageBox.Show(CommandData.ERROR_MESSAGE[1], CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    Mouse.OverrideCursor = null;
                    MessageBox.Show("Unable to save in database", CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
            catch (NullReferenceException)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(CommandData.ERROR_MESSAGE[5], CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            catch (Exception errorException)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(errorException.Message, CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }
        /// <summary>
        /// Print Invoice
        /// </summary>
        /// <param name="obj">
        /// obj[0] = InvoiceGrid.ItemSource
        /// obj[1] = ModelCutomerInfo
        /// obj[2] = Total Amount
        /// obj[3] = Total payment
        /// obj[4] = Total Due
        /// </param>
        private void printInvoiceClick(object obj)
        {
            this.InvoicePrint.IsEnabled = false;
            try
            {
                if (obj is ArrayList)
                {
                    ArrayList dataList = obj as ArrayList;
                    InvoiceReport newInvoiceReport = new InvoiceReport();
                    newInvoiceReport.bindingSourceProductInfo.DataSource = (dataList[0] as ObservableCollection<ModelSalesHistory>);
                    ModelCustomerIno selectedCustomer = dataList[1] as ModelCustomerIno;
                    ModelInvoicInformation invoiceInformation = new ModelInvoicInformation
                                                                                        {
                                                                                            Address = selectedCustomer.Address,
                                                                                            CreateDate = DateTime.Now,
                                                                                            CustomerName = selectedCustomer.CustomerName,
                                                                                            DueAmount = dataList[4].ToString(),
                                                                                            Payment = dataList[3].ToString(),
                                                                                            TotalAmount = dataList[2].ToString()
                                                                                        };
                    newInvoiceReport.bindingSourceInviceInfo.DataSource = invoiceInformation;
                   PrintHelper.ShowPrintPreview(this, newInvoiceReport);
                }
                else
                {
                    MessageBox.Show("Unable to create report.", CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
            catch (NullReferenceException)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(CommandData.ERROR_MESSAGE[5], CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            catch (Exception errorException)
            {
                MessageBox.Show(errorException.Message, CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                this.InvoicePrint.IsEnabled = true;
            }
        }
        /// <summary>
        /// Delete Item Form Invoice List
        /// </summary>
        /// <param name="obj"></param>
        private void deleteInvoiceItemClick(object obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                ModelSalesHistory deletedItem = obj as ModelSalesHistory;
                (this.InvoiceGrid.ItemsSource as ObservableCollection<ModelSalesHistory>).Remove(deletedItem);
                this.InvoiceTotalAmount.Text = (this.InvoiceGrid.ItemsSource as ObservableCollection<ModelSalesHistory>).Sum(x => x.Amount).ToString();
            }
            catch (NullReferenceException)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(CommandData.ERROR_MESSAGE[5], CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            catch (Exception errorException)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(errorException.Message, CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }
        #endregion

        #region Panel Payment
        public ICommand UpdatePaymentCommnad
        {
            get { return new ReplayCommand(new Action<object>(this.updatePaymentClick)); }
        }

        public ICommand PrintPaymentCommand
        {
            get { return new ReplayCommand(new Action<object>(this.printPaymentClick)); }
        }
        /// <summary>
        /// Update Customer Account and Insert in Payment History
        /// </summary>
        /// <param name="obj">
        /// obj[0] = ModelCustomerInfo
        /// obj[1] = paymentAmount
        /// obj[2] = Remain Due
        /// </param>
        private void updatePaymentClick(object obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (obj is ArrayList)
                {
                    using (DueManagementEntity dmDatabase = new DueManagementEntity())
                    {
                        ArrayList dataList = obj as ArrayList;
                        ModelCustomerIno selectedCustomer = dataList[0] as ModelCustomerIno;
                        var customerInformation = dmDatabase.CustomerInformations.First(x => x.AutoInc == selectedCustomer.ID);
                        customerInformation.DueAmount = +Convert.ToDecimal(dataList[2]);
                        dmDatabase.AddToPaymentHistories(new PaymentHistory
                                                                            {
                                                                                AutoInc = default(long),
                                                                                Amount = Convert.ToDecimal(dataList[1]),
                                                                                CustomerName = selectedCustomer.CustomerName,
                                                                                PaymentDate = DateTime.Today,
                                                                                CustomerID=selectedCustomer.ID
                                                                            });
                        dmDatabase.SaveChanges();
                        Mouse.OverrideCursor = null;
                        MessageBox.Show(CommandData.ERROR_MESSAGE[1], CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    Mouse.OverrideCursor = null;
                    MessageBox.Show("Unable to receive payment", CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
            catch (NullReferenceException)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(CommandData.ERROR_MESSAGE[5], CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            catch (Exception errorException)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(errorException.Message, CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }
        /// <summary>
        /// Create a Payment Report 
        /// </summary>
        /// <param name="obj">
        /// obj[0] = ModelCustomerInfo
        /// obj[1] = paymentAmount
        /// obj[2] = Remain Due
        /// obj[3] = Due Amount
        /// </param>
        private void printPaymentClick(object obj)
        {
            this.PaymentPrint.IsEnabled = false;
            try
            {
                if (obj is ArrayList)
                {
                    ArrayList dataList = obj as ArrayList;
                    ModelCustomerIno selectedCustomer = dataList[0] as ModelCustomerIno;
                    ModelInvoicInformation paymentInfo = new ModelInvoicInformation
                                                                                {
                                                                                    Address = selectedCustomer.Address,
                                                                                    CreateDate = DateTime.Now,
                                                                                    CustomerName = selectedCustomer.CustomerName,
                                                                                    DueAmount = dataList[2].ToString(),
                                                                                    Payment = dataList[1].ToString(),
                                                                                    TotalAmount = dataList[3].ToString()
                                                                                };
                    PaymentReport newPaymentReport = new PaymentReport();
                    newPaymentReport.PaymentbindingSource.DataSource = paymentInfo;
                    PrintHelper.ShowPrintPreview(this, newPaymentReport);
                }
                else
                {
                    MessageBox.Show("Unable to create report.", CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
            catch (NullReferenceException)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(CommandData.ERROR_MESSAGE[5], CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            catch (Exception errorException)
            {
                MessageBox.Show(errorException.Message, CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                this.PaymentPrint.IsEnabled = true;
            }
        }
        #endregion

        #region Panel Product Sale History

        public ICommand SearchSaleHistoryCommnad
        {
            get { return new ReplayCommand(new Action<object>(this.searchSaleHistoryClick)); }
        }

        public ICommand PrintSaleHistoryCommand
        {
            get { return new ReplayCommand(new Action<object>(this.printSaleHistoryClick)); }
        }

        public ICommand AllDeleteSaleHistoryCommand
        {
            get { return new ReplayCommand(new Action<object>(this.allDeletesaleHistoryClick)); }
        }

        public ICommand DeleteSaleHistoryCommand
        {
            get { return new ReplayCommand(new Action<object>(this.DeletesaleHistoryClick)); }
        }

        /// <summary>
        /// Search Sale`s History
        /// </summary>
        /// <param name="obj">
        /// obj[0] = ModelCustomerInformation
        /// obj[1] = by Date -- Date
        /// obj[2] = Between two date : First Date
        /// obj[3] = Between Two Date : Second Date
        /// obj[4] = ModelCustomQueryTime
        /// obj[5] = With Customer (Bool)
        /// </param>
        private void searchSaleHistoryClick(object obj)
        {
            this.SaleHistorySearch.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (obj is ArrayList)
                {
                    ArrayList dataList = obj as ArrayList;
                    IQueryable<ProductSaleHistory> searchQuery=null;
                    using (DueManagementEntity dmDatabase = new DueManagementEntity())
                    {
                        if ((bool)dataList[5])
                        {
                            ModelCustomerIno selectedCustomer= dataList[0] as ModelCustomerIno;
                            switch (this.SelectedRadioButton)
                            {
                                case "ByDate":
                                    DateTime firstDate = ((DateTime)dataList[1]).Date;
                                    searchQuery = from searchedData in dmDatabase.ProductSaleHistories where searchedData.SaleDate == firstDate && searchedData.CustomerID == selectedCustomer.ID select searchedData;
                                    break;
                                case "BetweenTwoDate" :
                                    DateTime secondDate= ((DateTime)dataList[2]).Date;
                                    DateTime thardDate=((DateTime)dataList[3]).Date;
                                    searchQuery = from searchedData in dmDatabase.ProductSaleHistories where searchedData.SaleDate <= secondDate && searchedData.SaleDate >= thardDate  && searchedData.CustomerID == selectedCustomer.ID select searchedData;
                                    break;
                                case "OlderThan" :
                                     ModelCustomQueryTime selectedDate = dataList[4] as ModelCustomQueryTime;
                                     DateTime olderDate=DateTime.Today.AddDays(selectedDate.QuertyTime).Date;
                                     searchQuery = from searchedData in dmDatabase.ProductSaleHistories where searchedData.SaleDate >=olderDate  && searchedData.CustomerID == selectedCustomer.ID select searchedData;
                                    break;
                                case "All" :
                                     searchQuery = from searchedData in dmDatabase.ProductSaleHistories where searchedData.CustomerID == selectedCustomer.ID select searchedData;
                                    break;
                                default:
                                    break;
                            }
                            this.SaleHistoryGrid.ItemsSource = new ObservableCollection<ModelSalesHistory>(searchQuery.Select(x => new ModelSalesHistory
                                                                                                            {
                                                                                                                ID = x.AutoInc,
                                                                                                                PropductName = x.ProductName,
                                                                                                                Quantity = x.Quantity,
                                                                                                                Rate = x.Rate,
                                                                                                                SaleDate = x.SaleDate,
                                                                                                                Amount = 0
                                                                                                            }));
                        }
                        else
                        {
                            switch (this.SelectedRadioButton)
                            {
                                case "ByDate":
                                    DateTime firstDate = ((DateTime)dataList[1]).Date;
                                    searchQuery = from searchedData in dmDatabase.ProductSaleHistories where searchedData.SaleDate == firstDate select searchedData;
                                    break;
                                case "BetweenTwoDate":
                                    DateTime secondDate= ((DateTime)dataList[2]).Date;
                                    DateTime thardDate=((DateTime)dataList[3]).Date;
                                    searchQuery = from searchedData in dmDatabase.ProductSaleHistories where searchedData.SaleDate <= secondDate && searchedData.SaleDate >= thardDate select searchedData;
                                    break;
                                case "OlderThan":
                                    ModelCustomQueryTime selectedDate = dataList[4] as ModelCustomQueryTime;
                                    DateTime olderDate = DateTime.Today.AddDays(selectedDate.QuertyTime).Date;
                                    searchQuery = from searchedData in dmDatabase.ProductSaleHistories where searchedData.SaleDate >= olderDate select searchedData;
                                    break;
                                case "All":
                                    searchQuery = from searchedData in dmDatabase.ProductSaleHistories select searchedData;
                                    break;
                                default:
                                    break;
                            }
                            this.SaleHistoryGrid.ItemsSource = new ObservableCollection<ModelSalesHistory> (searchQuery.Select(x => new ModelSalesHistory
                                                                                                            {
                                                                                                                ID = x.AutoInc,
                                                                                                                CustomerName = x.CustomerName,
                                                                                                                PropductName = x.ProductName,
                                                                                                                Quantity = x.Quantity,
                                                                                                                Rate = x.Rate,
                                                                                                                SaleDate = x.SaleDate,
                                                                                                                Amount = 0
                                                                                                            }));
                        }
                    }
                }
                else
                {
                    Mouse.OverrideCursor = null;
                    MessageBox.Show("Unable to Search in database", CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
            catch (NullReferenceException)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(CommandData.ERROR_MESSAGE[5], CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            catch (Exception errorException)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(errorException.Message, CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.SaleHistorySearch.IsEnabled = true;
            }
        }
        /// <summary>
        /// Print Sale History
        /// </summary>
        /// <param name="obj">
        /// obj[0] = ModelCustomerInformation
        /// obj[1] = SaleHistoryGrid.ItemSource
        /// obj[2] = With Customer (Bool)
        /// </param>
        private void printSaleHistoryClick(object obj)
        {
            this.SaleHistoryReport.IsEnabled = false;
            try
            {
                if (obj is ArrayList)
                {
                    ArrayList dataList = obj as ArrayList;
                    ModelInvoicInformation saleHistoryInfo= new ModelInvoicInformation();
                    if ((bool)dataList[2])
                    {
                        ModelCustomerIno selectCustomerInfo = dataList[0] as ModelCustomerIno;
                        saleHistoryInfo.Address = selectCustomerInfo.Address;
                        saleHistoryInfo.CustomerName = selectCustomerInfo.CustomerName;
                    }
                    saleHistoryInfo.CreateDate = DateTime.Now;
                    SHestoryReport newSaleHistoryReport = new SHestoryReport();
                    newSaleHistoryReport.CutomerinfobindingSource.DataSource = saleHistoryInfo;
                    newSaleHistoryReport.SaleHistorybindingSource.DataSource = (dataList[1] as ObservableCollection<ModelSalesHistory>);
                    PrintHelper.ShowPrintPreview(this, newSaleHistoryReport);
                }
                else
                {
                    Mouse.OverrideCursor = null;
                    MessageBox.Show("Unable to create report", CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
            catch (NullReferenceException)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(CommandData.ERROR_MESSAGE[5], CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            catch (Exception errorException)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(errorException.Message, CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.SaleHistoryReport.IsEnabled = true;
            }
        }
        /// <summary>
        /// Delete selected History From Database
        /// </summary>
        /// <param name="obj"></param>
        private void DeletesaleHistoryClick(object obj)
        {
            this.SaleHistoryDelete.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                using (DueManagementEntity dmDatabase= new DueManagementEntity())
                {
                    ObservableCollection<ModelSalesHistory> saleHestoryItems =new ObservableCollection<ModelSalesHistory>();
                    foreach (var singleItem in obj as IEnumerable)
                    {
                        ModelSalesHistory singleHistory = singleItem as ModelSalesHistory;
                        saleHestoryItems.Add(singleHistory);
                    }
                    foreach (ModelSalesHistory singleHistory in saleHestoryItems)
                    {
                        dmDatabase.ProductSaleHistories.DeleteObject(dmDatabase.ProductSaleHistories.First(x=>x.AutoInc==singleHistory.ID));
                        (this.SaleHistoryGrid.ItemsSource as ObservableCollection<ModelSalesHistory>).Remove(singleHistory);
                    }
                    dmDatabase.SaveChanges();
                    Mouse.OverrideCursor = null;
                    MessageBox.Show(CommandData.ERROR_MESSAGE[4], CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (NullReferenceException)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(CommandData.ERROR_MESSAGE[5], CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            catch (Exception errorException)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(errorException.Message, CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.SaleHistoryDelete.IsEnabled = true;
            }
        }
        /// <summary>
        /// Delete Selected Items
        /// </summary>
        /// <param name="obj">DataGridSelectedItems</param>
        private void allDeletesaleHistoryClick(object obj)
        {
            this.SaleHistoryDeleteAll.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                using (DueManagementEntity dmDatabase = new DueManagementEntity())
                {
                    ObservableCollection<ModelSalesHistory> saleHestoryItems = new ObservableCollection<ModelSalesHistory>();
                    foreach (var singleItem in obj as ObservableCollection<ModelSalesHistory>)
                    {
                        saleHestoryItems.Add(singleItem);
                    }
                    foreach (var deletedItem in saleHestoryItems)
                    {
                        dmDatabase.ProductSaleHistories.DeleteObject(dmDatabase.ProductSaleHistories.First(x => x.AutoInc == deletedItem.ID));
                        (this.SaleHistoryGrid.ItemsSource as ObservableCollection<ModelSalesHistory>).Remove(deletedItem);
                    }
                    dmDatabase.SaveChanges();
                    Mouse.OverrideCursor = null;
                    MessageBox.Show(CommandData.ERROR_MESSAGE[4], CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (NullReferenceException)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(CommandData.ERROR_MESSAGE[5], CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            catch (Exception errorException)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(errorException.Message, CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.SaleHistoryDeleteAll.IsEnabled = true;
            }
        }
        #endregion

        #region Panel Payment History
        public ICommand SearchPaymentHistoryCommand
        {
            get { return new ReplayCommand(new Action<object>(this.searchPaymentHistoryClick)); }
        }

        public ICommand PrintPaymentHistoryCommand
        {
            get { return new ReplayCommand(new Action<object>(this.printPaymentHistoryClick)); }
        }

        public ICommand DeletePaymentHistoryCommand
        {
           get {return new ReplayCommand(new Action<object>(this.deletePaymentHistoryClick));}
        }

        public ICommand DeleteAllPaymentHistoryCommand
        {
            get { return new ReplayCommand(new Action<object>(this.deleteAllPaymentHistoryClick)); }
        }
        /// <summary>
        /// Search Payment History
        /// </summary>
        /// <param name="obj">
        /// obj[0] = ModelCustomerInformation
        /// obj[1] = by Date -- Date
        /// obj[2] = Between two date : First Date
        /// obj[3] = Between Two Date : Second Date
        /// obj[4] = ModelCustomQueryTime
        /// obj[5] = With Customer (Bool)
        /// </param>
        private void searchPaymentHistoryClick(object obj)
        {
            this.PaymentHisSearch.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (obj is ArrayList)
                {
                    ArrayList dataList = obj as ArrayList;
                    IQueryable<PaymentHistory> searchQuery = null;
                    using (DueManagementEntity dmDatabase = new DueManagementEntity())
                    {
                        if ((bool)dataList[5])
                        {
                            ModelCustomerIno selectedCustomer = dataList[0] as ModelCustomerIno;
                            switch (this.SelectedRadioButton)
                            {
                                case "ByDate":
                                    DateTime firstDate = ((DateTime)dataList[1]).Date;
                                    searchQuery = from searchedData in dmDatabase.PaymentHistories where searchedData.PaymentDate == firstDate && searchedData.CustomerID == selectedCustomer.ID select searchedData;
                                    break;
                                case "BetweenTwoDate":
                                    DateTime secondDate = ((DateTime)dataList[2]).Date;
                                    DateTime thardDate = ((DateTime)dataList[3]).Date;
                                    searchQuery = from searchedData in dmDatabase.PaymentHistories where searchedData.PaymentDate <= secondDate && searchedData.PaymentDate >= thardDate && searchedData.CustomerID == selectedCustomer.ID select searchedData;
                                    break;
                                case "OlderThan":
                                    ModelCustomQueryTime selectedDate = dataList[4] as ModelCustomQueryTime;
                                    DateTime olderDate = DateTime.Today.AddDays(selectedDate.QuertyTime).Date;
                                    searchQuery = from searchedData in dmDatabase.PaymentHistories where searchedData.PaymentDate >= olderDate && searchedData.CustomerID == selectedCustomer.ID select searchedData;
                                    break;
                                case "All":
                                    searchQuery = from searchedData in dmDatabase.PaymentHistories where searchedData.CustomerID == selectedCustomer.ID select searchedData;
                                    break;
                                default:
                                    break;
                            }
                            this.PaymentHisGrid.ItemsSource = new ObservableCollection<ModelPaymentHistory>(searchQuery.Select(x => new ModelPaymentHistory
                            {
                                ID = x.AutoInc,
                                PaymentDate=x.PaymentDate,
                                Amount=x.Amount
                            }));
                        }
                        else
                        {
                            switch (this.SelectedRadioButton)
                            {
                                case "ByDate":
                                    DateTime firstDate = ((DateTime)dataList[1]).Date;
                                    searchQuery = from searchedData in dmDatabase.PaymentHistories where searchedData.PaymentDate == firstDate select searchedData;
                                    break;
                                case "BetweenTwoDate":
                                    DateTime secondDate = ((DateTime)dataList[2]).Date;
                                    DateTime thardDate = ((DateTime)dataList[3]).Date;
                                    searchQuery = from searchedData in dmDatabase.PaymentHistories where searchedData.PaymentDate <= secondDate && searchedData.PaymentDate >= thardDate select searchedData;
                                    break;
                                case "OlderThan":
                                    ModelCustomQueryTime selectedDate = dataList[4] as ModelCustomQueryTime;
                                    DateTime olderDate = DateTime.Today.AddDays(selectedDate.QuertyTime).Date;
                                    searchQuery = from searchedData in dmDatabase.PaymentHistories where searchedData.PaymentDate >= olderDate select searchedData;
                                    break;
                                case "All":
                                    searchQuery = from searchedData in dmDatabase.PaymentHistories select searchedData;
                                    break;
                                default:
                                    break;
                            }
                            this.PaymentHisGrid.ItemsSource = new ObservableCollection<ModelPaymentHistory>(searchQuery.Select(x => new ModelPaymentHistory
                            {
                                ID = x.AutoInc,
                                CustomerName = x.CustomerName,
                                Amount=x.Amount,
                                PaymentDate=x.PaymentDate
                            }));
                        }
                    }
                }
                else
                {
                    Mouse.OverrideCursor = null;
                    MessageBox.Show("Unable to Search in database", CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
            catch (NullReferenceException)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(CommandData.ERROR_MESSAGE[5], CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            catch (Exception errorException)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(errorException.Message, CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.PaymentHisSearch.IsEnabled = true;
            }
        }
        /// <summary>
        /// Print Sale History
        /// </summary>
        /// <param name="obj">
        /// obj[0] = ModelCustomerInformation
        /// obj[1] = SaleHistoryGrid.ItemSource
        /// obj[2] = With Customer (Bool)
        /// </param>
        private void printPaymentHistoryClick(object obj) 
        {
            this.PaymentPrint.IsEnabled = false;
            try
            {
                if (obj is ArrayList)
                {
                    ArrayList dataList = obj as ArrayList;
                    ModelInvoicInformation paymentHistoryInfo = new ModelInvoicInformation();
                    if ((bool)dataList[2])
                    {
                        ModelCustomerIno selectCustomerInfo = dataList[0] as ModelCustomerIno;
                        paymentHistoryInfo.Address = selectCustomerInfo.Address;
                        paymentHistoryInfo.CustomerName = selectCustomerInfo.CustomerName;
                    }
                    paymentHistoryInfo.CreateDate = DateTime.Now;
                    PaymentHisReport newPaymentHistoryReport = new PaymentHisReport();
                    newPaymentHistoryReport.GeneralInfobindingSource.DataSource = paymentHistoryInfo;
                    newPaymentHistoryReport.PaymentHisbindingSource.DataSource = (dataList[1] as ObservableCollection<ModelPaymentHistory>);
                    PrintHelper.ShowPrintPreview(this, newPaymentHistoryReport);
                }
                else
                {
                    Mouse.OverrideCursor = null;
                    MessageBox.Show("Unable to create report", CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
            catch (NullReferenceException)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(CommandData.ERROR_MESSAGE[5], CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            catch (Exception errorException)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(errorException.Message, CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.PaymentHisPrint.IsEnabled = true;
            }
        }
        /// <summary>
        /// Delete Selected Items
        /// </summary>
        /// <param name="obj">DataGrid.SelectedItems</param>
        private void deletePaymentHistoryClick(object obj)
        {
            this.PaymentHisDelete.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                using (DueManagementEntity dmDatabase = new DueManagementEntity())
                {
                    ObservableCollection<ModelPaymentHistory> paymentHestoryItems = new ObservableCollection<ModelPaymentHistory>();
                    foreach (var singleItem in obj as IEnumerable)
                    {
                        ModelPaymentHistory singleHistory = singleItem as ModelPaymentHistory;
                        paymentHestoryItems.Add(singleHistory);
                    }
                    foreach (ModelPaymentHistory singleHistory in paymentHestoryItems)
                    {
                        dmDatabase.PaymentHistories.DeleteObject(dmDatabase.PaymentHistories.First(x => x.AutoInc == singleHistory.ID));
                        (this.PaymentHisGrid.ItemsSource as ObservableCollection<ModelPaymentHistory>).Remove(singleHistory);
                    }
                    dmDatabase.SaveChanges();
                    Mouse.OverrideCursor = null;
                    MessageBox.Show(CommandData.ERROR_MESSAGE[4], CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (NullReferenceException)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(CommandData.ERROR_MESSAGE[5], CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            catch (Exception errorException)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(errorException.Message, CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.PaymentHisDelete.IsEnabled = true;
            }
        }
        /// <summary>
        /// Delete All items
        /// </summary>
        /// <param name="obj">DataGrid.ItemSource</param>
        private void deleteAllPaymentHistoryClick(object obj)
        {
            this.PaymentHisDeleteAll.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                using (DueManagementEntity dmDatabase = new DueManagementEntity())
                {
                    ObservableCollection<ModelPaymentHistory> saleHestoryItems = new ObservableCollection<ModelPaymentHistory>();
                    foreach (var singleItem in obj as ObservableCollection<ModelPaymentHistory>)
                    {
                        saleHestoryItems.Add(singleItem);
                    }
                    foreach (var deletedItem in saleHestoryItems)
                    {
                        dmDatabase.PaymentHistories.DeleteObject(dmDatabase.PaymentHistories.First(x => x.AutoInc == deletedItem.ID));
                        (this.PaymentHisGrid.ItemsSource as ObservableCollection<ModelPaymentHistory>).Remove(deletedItem);
                    }
                    dmDatabase.SaveChanges();
                    Mouse.OverrideCursor = null;
                    MessageBox.Show(CommandData.ERROR_MESSAGE[4], CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (NullReferenceException)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(CommandData.ERROR_MESSAGE[5], CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            catch (Exception errorException)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(errorException.Message, CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.PaymentHisDeleteAll.IsEnabled = true;
            }
        }
        #endregion

        #region Panel Database Backup
        private string filePath=string.Empty;

        public ICommand FolderBrowseDBCommand
        {
            get { return new ReplayCommand(new Action<object>(this.folderBrowseDBClick)); }
        }

        public ICommand DatabseBackupCommand
        {
            get { return new ReplayCommand(new Action<object>(this.databseBackupClick)); }
        }

        private void folderBrowseDBClick(object obj)
        {
            this.DbackupBrowse.IsEnabled = false;
            try
            {
                System.Windows.Forms.FolderBrowserDialog folderBrowse = new System.Windows.Forms.FolderBrowserDialog();
                if (folderBrowse.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    filePath = folderBrowse.SelectedPath;
                }
            }
            catch (Exception errorException)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(errorException.Message, CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.DbackupBrowse.IsEnabled = true;
            }
        }

        private void databseBackupClick(object obj)
        {
            this.Dbackup.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (!String.IsNullOrEmpty(filePath))
                {
                    string fromDBName = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), EasyShopManagement.Class.CommandData.SOFTWARE_NAME, "DueManagementSystemDatabase.s3db");
                    File.Copy(fromDBName, System.IO.Path.Combine(filePath, DateTime.Now.ToString("ddMMyyyyHHmmssfftt") + ".s3db"));
                    Mouse.OverrideCursor = null;
                    MessageBox.Show("Database Backup Complete.", CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    Mouse.OverrideCursor = null;
                    MessageBox.Show("Please Select a folder.", CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Hand);
                }
            }
            catch (Exception errorException)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(errorException.Message, CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.Dbackup.IsEnabled = true;
            }
        }
        #endregion

        #region Panel Databse Restore
        private string selectedDatabase = string.Empty;

        public ICommand FileBrowseDRCommand
        {
            get { return new ReplayCommand(new Action<object>(this.fileBrowseDRClick)); }
        }

        public ICommand DatabseRestoreCommand
        {
            get { return new ReplayCommand(new Action<object>(this.databaseRestoreClick)); }
        }

        private void fileBrowseDRClick(object obj)
        {
            this.DRestoreBrowse.IsEnabled = false;
            try
            {
                System.Windows.Forms.OpenFileDialog openDatabseFile = new System.Windows.Forms.OpenFileDialog();
                openDatabseFile.Filter = "Database (*.s3db)|*.s3db";
                openDatabseFile.FilterIndex = 2;
                openDatabseFile.RestoreDirectory = true;
                if (openDatabseFile.ShowDialog()==System.Windows.Forms.DialogResult.OK)
                {
                   selectedDatabase = this.DRestorePath.Text = openDatabseFile.FileName;
                }
            }
            catch (Exception errorException)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(errorException.Message, CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.DRestoreBrowse.IsEnabled = true;
            }
        }

        private void databaseRestoreClick(object obj)
        {
            this.DResore.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (!String.IsNullOrEmpty(selectedDatabase))
                {
                    string toDBName = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), EasyShopManagement.Class.CommandData.SOFTWARE_NAME, "DueManagementSystemDatabase.s3db");
                    File.Copy(selectedDatabase, toDBName, true);
                    Mouse.OverrideCursor = null;
                    MessageBox.Show("Database Restore Complete.", CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    Mouse.OverrideCursor = null;
                    MessageBox.Show("Please Select a database.", CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Hand);
                }
            }
            catch (Exception errorException)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(errorException.Message, CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.DResore.IsEnabled = true;
            }
        }
        #endregion

        #region Private Methord
        private void selectFirstItem(ListBox listbox)
        {
            if (listbox.Items.Count > 0)
            {
                listbox.SelectedIndex = 0;
            }
        }

        private void getValidationError(params DependencyObject[] dp)
        {
            foreach (DependencyObject depenency in dp)
            {

                foreach (ValidationError errors in Validation.GetErrors(depenency))
                {
                    throw new Exception(errors.ErrorContent.ToString());
                }
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if ((MessageBox.Show("Are you sure you want to Exit ?", CommandData.SOFTWARE_NAME, MessageBoxButton.YesNo, MessageBoxImage.Question)) == MessageBoxResult.No)
            {
                e.Cancel = true;
                base.OnClosing(e);
            }
        }
        #endregion
    }

    #region ICommand Class
    class ReplayCommand : ICommand
    {
        private Action<object> _action;

        public ReplayCommand(Action<object> action)
        {
            this._action = action;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

#pragma warning disable 67
        public event EventHandler CanExecuteChanged;
#pragma warning restore 67

        public void Execute(object parameter)
        {
            try
            {
                if (parameter != null)
                {
                    this._action(parameter);
                }
                else
                {
                    MessageBox.Show("Invalid Command", CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Hand);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, CommandData.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
    #endregion
    
}
