using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BucketBox
{
    public class Base
    {
        public static void exceptionHandle(Exception ex)
        {
            if (ex != null)
            {
                throw (ex);
            }
        }
    }
}
