using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
namespace EasyShopManagement.Class.Models
{
   public class ModelPaymentHistory : INotifyPropertyChanged
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

        public DateTime PaymentDate
        {
            get { return this._PaymentDate; }
            set
            {
                if (this._PaymentDate != value)
                {
                    this._PaymentDate = value;
                    this.onPropertyChanged("PaymentDate");
                }
            }
        }

        public decimal Amount
        {
            get { return this._Amount; }
            set
            {
                if (this._Amount != value)
                {
                    this._Amount = value;
                    this.onPropertyChanged("Amount");
                }
            }
        }

        #region Private Variable
        private Int64 _ID;
        private string _CustomerName;
        private DateTime _PaymentDate;
        private decimal _Amount;
        #endregion

        #region On Property Chnageed
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
