using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace EasyShopManagement.Class.Models
{
   public class ModelSalesHistory :INotifyPropertyChanged
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

        public DateTime SaleDate
        {
            get { return this._SaleDate; }
            set
            {
                if (this._SaleDate != value)
                {
                    this._SaleDate = value;
                    this.onPropertyChanged("SaleDate");
                }
            }
        }

        public string PropductName
        {
            get { return this._PropductName; }
            set
            {
                if (this._PropductName != value)
                {
                    this._PropductName = value;
                    this.onPropertyChanged("PropductName");
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

        public decimal Quantity
        {
            get { return this._Quantity; }
            set
            {
                if (this._Quantity != value)
                {
                    this._Quantity = value;
                    this.onPropertyChanged("Quantity");
                }
            }
        }

        public decimal Amount
        {
            get { return this._Amount; }
            set
            {
                this._Amount = this.Quantity * this.Rate;
                this.onPropertyChanged("Amount");
            }
        }

        #region Private Variable
        private Int64 _ID;
        private DateTime _SaleDate;
        private string _PropductName;
        private string _CustomerName;
        private decimal _Rate;
        private decimal _Quantity;
        private decimal _Amount;
        #endregion

        #region On Property Change
        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged!=null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
