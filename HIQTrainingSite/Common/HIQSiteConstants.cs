using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIQTrainingSite.Common
{
    public class HIQSiteConstants
    {
        public class General
        {
            public const int PAGE_SIZE = 10;
            public const int PAGE_SIZE_SHORT = 5;
        }

        public class RegularExpression
        {
            public const string PHONE = @"^\d{9}$";

            //public const string PAYROLL = @"^?\d+(\,\d+)?$";
            public const string PAYROLL = @"\d+(\.\d{1,2})?";
        }
    }
}