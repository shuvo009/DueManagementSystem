using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using EasyShopManagement.Class;
namespace EasyShopManagement.Class.Models
{
   public class ModelCustomerIno : INotifyPropertyChanged,IDataErrorInfo
   {
       public Int64 ID
       {
           get { return this._ID; }
           set
           {
               if (this._ID != value)
               {
                   this._ID = value;
                   this.onPropertyChanged("ID");
               }
           }
       }

       public string CustomerName
       {
           get { return this._CustomerName; }
           set
           {
               if (this._CustomerName != value)
               {
                   this._CustomerName = value;
                   this.onPropertyChanged("CustomerName");
               }
           }
       }

       public string Mobile
       {
           get { return this._Mobile; }
           set
           {
               if (this._Mobile != value)
               {
                   this._Mobile = value;
                   this.onPropertyChanged("Mobile");
               }
           }
       }

       public string Address
       {
           get { return this._Address; }
           set
           {
               if (this._Address != value)
               {
                   this._Address = value;
                   this.onPropertyChanged("Address");
               }
           }
       }

       public string ShopName
       {
           get { return this._ShopName; }
           set
           {
               if (this._ShopName != value)
               {
                   this._ShopName = value;
                   this.onPropertyChanged("ShopName");
               }
           }
       }

       public decimal DueAmount
       {
           get { return this._DueAmount; }
           set
           {
               if (this._DueAmount != value)
               {
                   this._DueAmount = value;
                   this.onPropertyChanged("DueAmount");
               }
           }
       }

       public string Remark
       {
           get { return this._Remark; }
           set
           {
               if (this._Remark != value)
               {
                   this._Remark = value;
                   this.onPropertyChanged("Remark");
               }
           }
       }

       public DateTime CreateDate
       {
           get { return this._CreateDate; }
           set
           {
               if (this._CreateDate != value)
               {
                   this._CreateDate = value;
                   this.onPropertyChanged("CreateDate");
               }
           }
       }

       #region Private Variable
       private Int64 _ID;
       private string _CustomerName;
       private string _Mobile;
       private string _Address;
       private string _ShopName;
       private decimal _DueAmount;
       private string _Remark;
       private DateTime _CreateDate;
       #endregion

       #region On Property Chnage
       public event PropertyChangedEventHandler PropertyChanged;
       private void onPropertyChanged(string propertyName)
       {
           if (this.PropertyChanged!=null)
           {
               this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
           }
       }
       #endregion

       #region Data Error
       public string Error
        {
            get { return string.Empty; }
        }

        public string this[string columnName]
        {
            get 
                {
                    string errorMessages = string.Empty;
                    switch (columnName)
                    {
                        case "CustomerName":
                            if (String.IsNullOrEmpty(this.CustomerName))
                            {
                                errorMessages = String.Format(CommandData.DATA_ERROR_MESSAGESS, "CustomerName");
                            }
                            break;
                        default:
                            break;
                    }
                    return errorMessages;
                }
       }
       #endregion
    }
}
