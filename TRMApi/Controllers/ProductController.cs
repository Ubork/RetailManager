using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TRMDataManager.Library.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        public List<ProductModel> Get()
        {
            ProductData data = new ProductData();

            return data.GetProducts();
        }
    }
}