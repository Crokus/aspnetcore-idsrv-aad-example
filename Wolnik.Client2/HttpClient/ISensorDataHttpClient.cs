using System.Net.Http;
using System.Threading.Tasks;

namespace Wolnik.Client2.Services {
    public interface ISensorDataHttpClient {
        Task<HttpClient> GetClientAsync();
    }
}
