using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CSharp.RuntimeBinder;

namespace DataAccessLayer.Core.EntityFramework.Utilities
{
    public class Helpers
    {
        public static bool IsPropertyExist(dynamic dynamicObj, string propertyName)
        {
            try
            {
                dynamic property= dynamicObj.GetType().GetProperty(propertyName);
                if (property != null)
                    return true;
                else return false;
            }
            catch (RuntimeBinderException)
            {
                return false;
            }

        }
    }
}
