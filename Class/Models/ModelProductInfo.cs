using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using EasyShopManagement.Class;
namespace EasyShopManagement.Class.Models
{
    class ModelProductInfo : INotifyPropertyChanged,IDataErrorInfo
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

        public string ProductName
        {
            get { return this._ProductName; }
            set
            {
                if (this._ProductName != value)
                {
                    this._ProductName = value;
                    this.onPropertyChanged("ProductName");
                }
            }
        }

        public decimal Rate
        {
            get { return this._Rate; }
            set
            {
                if (this._Rate != value)
                {
                    this._Rate = value;
                    this.onPropertyChanged("Rate");
                }
            }
        }

        #region Private Variable
        private Int64 _ID;
        private string _ProductName;
        private decimal _Rate;
        #endregion

        #region On Property Changed
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
                string errorMessagess = string.Empty;
                switch (columnName)
                {
                    case "ProductName":
                        if (String.IsNullOrEmpty(this.ProductName))
                        {
                            errorMessagess = String.Format(CommandData.DATA_ERROR_MESSAGESS, "ProductName");
                        }
                        break;
                    default:
                        break;
                }
                return errorMessagess;
            }
        }
        #endregion
    }
}
