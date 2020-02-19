using System.Collections.Generic;
using TRMDataManager.Library.Internal.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Library.DataAccess
{
    public class InventoryData
    {
        public List<InventoryModel> GetInventory()
        {
            SqlDataAccess sql = new SqlDataAccess("TRMData");

            var output = sql.LoadData<InventoryModel>("dbo.spInventory_GetAll", new { });

            return output;
        }

        public void SaveInventoryRecord(InventoryModel item)
        {
            SqlDataAccess sql = new SqlDataAccess("TRMData");

            sql.SaveData("dbo.spInventory_Insert", item);
        }
    }
}
