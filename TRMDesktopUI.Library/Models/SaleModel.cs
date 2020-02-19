using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDesktopUI.Library.Models
{
    public class SaleModel
    {
        public SaleModel()
        {
            SaleDetails = new List<SaleDetailModel>();
        }

        public List<SaleDetailModel> SaleDetails { get; set; }
    }
}
