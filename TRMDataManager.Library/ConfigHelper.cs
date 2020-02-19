using System;
using System.Configuration;
using System.Globalization;

namespace TRMDataManager.Library
{
    public static class ConfigHelper
    {
        public static decimal GetTaxRate()
        {
            string rateText = ConfigurationManager.AppSettings["taxRate"];

            bool IsValidTaxRate = Decimal.TryParse(rateText, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal output);

            if (!IsValidTaxRate)
            {
                throw new ConfigurationErrorsException("The tax rate is not set up properly");
            }

            return output;
        }
    }
}
