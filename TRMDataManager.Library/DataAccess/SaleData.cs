using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDataManager.Library.Internal.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Library.DataAccess
{
    public class SaleData
    {
        private readonly IConfiguration _config;

        public SaleData(IConfiguration config)
        {
            _config = config;
        }
        public void SaveSale(SaleModel saleInfo, string cashierId)
        {
            //TODO: Make this SOLID / DRY / BETTER

            //Start filling in the models we sill save to the database
            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();
            ProductData products = new ProductData(_config);
            var taxRate = ConfigHelper.GetTaxRate() * (decimal)0.01;

            foreach (var item in saleInfo.SaleDetails)
            {
                var productInfo = products.GetProductById(item.ProductId);

                if (productInfo == null) { throw new Exception($"The product Id of { item.ProductId } could not be found in the database."); }

                var detail = new SaleDetailDBModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    PurchasePrice = productInfo.RetailPrice * item.Quantity,
                };

                detail.Tax = !productInfo.IsTaxable ? 0 : (productInfo.RetailPrice * item.Quantity * taxRate);

                details.Add(detail);
            }

            //Create the SaleModel
            SaleDBModel sale = new SaleDBModel
            {
                SubTotal = details.Sum(x => x.PurchasePrice),
                Tax = details.Sum(x => x.Tax),
                CashierId = cashierId
            };
            sale.Total = sale.SubTotal + sale.Tax;

            //Save the SaleModel
            using (SqlDataAccess sql = new SqlDataAccess(_config))
            {
                try
                {
                    sql.StartTransaction("TRMData");
                    sql.SaveDataInTransaction("dbo.spSale_Insert", sale);
                    sale.Id = sql.LoadDataInTransaction<int, dynamic>("spSale_Lookup", new { sale.CashierId, sale.SaleDate }).FirstOrDefault();
                    foreach (var item in details)
                    {
                        item.SaleId = sale.Id;
                        sql.SaveDataInTransaction("dbo.spSaleDetail_Insert", item);
                    }
                    sql.CommitTransaction();
                }
                catch
                {
                    sql.RollBackTransaction();
                    throw;
                }
            }
        }
        public List<SaleReportModel> GetSaleReport()
        {
            SqlDataAccess sql = new SqlDataAccess(_config);

            var output = sql.LoadData<SaleReportModel, dynamic>("dbo.spSale_SaleReport", new { }, "TRMData");

            return output;
        }

    }

    
}
