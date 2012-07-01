using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyShopManagement.Class.Models
{
   public class ModelInvoicInformation
    {
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string TotalAmount { get; set; }
        public string Payment { get; set; }
        public string DueAmount { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
