using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyShopManagement.Class
{
    class CommandData
    {
        public const string DATA_ERROR_MESSAGESS = "{0} is required.";
        public const string SOFTWARE_NAME = "Easy Shop Management";
        public static String[] ERROR_MESSAGE = new String[] 
        {
            "Are you sure you want to Update ?","Update successfully done.","Insert Successfully.","Are you sure are you want to delete ?","Delete Successfully","You missing some thing\nPlease recheck all inputs"
            //              0                           1                            2                          3                                  4                                        5
        };
    }
}
