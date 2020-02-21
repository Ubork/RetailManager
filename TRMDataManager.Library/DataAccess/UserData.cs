using System.Collections.Generic;
using TRMDataManager.Library.Internal.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Library.DataAccess
{
    public class UserData
    {
        public List<UserModel> GetUserById(string Id)
        {
            SqlDataAccess sql = new SqlDataAccess("TRMData");

            var output = sql.LoadData<UserModel>("dbo.spUserLookup", new { Id = Id });

            return output;
        }
    }
}
