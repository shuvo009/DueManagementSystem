using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Controls;
using EasyShopManagement.Class.Models;
namespace EasyShopManagement.Class.Searchs
{
    class ProcuctSearch
    {
        public ProcuctSearch(ICollectionView filteredList, TextBox textEdit)
        {
            string filterText = string.Empty;
            
            filteredList.Filter = delegate(object obj)
            {
                if (String.IsNullOrEmpty(filterText))
                {
                    return true;
                }
                ModelProductInfo str = obj as ModelProductInfo;
                if (str.ProductName==null)
                {
                    return true;
                }
                if (str.ProductName.ToUpper().Contains(filterText.ToUpper()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            };
            textEdit.TextChanged += delegate
            {
                filterText = textEdit.Text;
                filteredList.Refresh();
            };
        }
    }
}
