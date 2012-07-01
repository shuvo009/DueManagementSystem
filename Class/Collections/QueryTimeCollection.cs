using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using EasyShopManagement.Class.Models;
namespace EasyShopManagement.Class.Collections
{
    class QueryTimeCollection : ObservableCollection<ModelCustomQueryTime>
    {
        public QueryTimeCollection()
        {
            this.Add(new ModelCustomQueryTime {  Name="1 Month",QuertyTime=30});
            this.Add(new ModelCustomQueryTime { Name = "2 Month", QuertyTime = 60 });
            this.Add(new ModelCustomQueryTime { Name = "3 Month", QuertyTime = 90 });
            this.Add(new ModelCustomQueryTime { Name = "4 Month", QuertyTime = 120 });
            this.Add(new ModelCustomQueryTime { Name = "5 Month", QuertyTime = 150 });
        }
    }
}
