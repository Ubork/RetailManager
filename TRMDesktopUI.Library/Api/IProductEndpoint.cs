using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.Library.Api
{
    public interface IProductEndpoint
    {
        HttpClient ApiClient { get; }

        Task<List<ProductModel>> GetAll();
    }
}