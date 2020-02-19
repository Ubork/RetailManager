using System.Configuration;
using System.Globalization;

namespace TRMDataManager.Library
{
    public static class ConfigHelper
    {
        public static decimal GetTaxRate()
        {
            string taxRate = ConfigurationManager.AppSettings["taxRate"];

            if (decimal.TryParse(taxRate, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal validTaxRate))
            {
                return validTaxRate;
            }

            throw new ConfigurationErrorsException("The tax rate is not set up properly");
        }
    }
}
