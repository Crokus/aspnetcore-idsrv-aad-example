using System.Net.Http;
using System.Threading.Tasks;

namespace Wolnik.Client.Services {
    public interface ISensorDataHttpClient {
        Task<HttpClient> GetClientAsync();
    }
}
