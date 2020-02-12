using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TRMDataManager.Models;

namespace TRMDataManager.Controllers
{

    [Authorize]
    public class SaleController : ApiController
    {
        public void Post(SaleModel sale)
        {
            Console.WriteLine();
            
        }
    }
}
