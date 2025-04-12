using ISO810_ComprasProject.Services.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace ISO810_ComprasProject.Services
{
    public class ContabilidadApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ContabilidadConfig _config;

        public ContabilidadApiService(HttpClient httpClient, ContabilidadConfig config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<(int,DateTime)> EnviarAsientoAsync(string token, object asientoContable)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var content = new StringContent(JsonConvert.SerializeObject(asientoContable), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_config.AsientosUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al enviar el asiento contable: {error}");
            }
            else
            {
                var json = await response.Content.ReadAsStringAsync();
                dynamic result = JsonConvert.DeserializeObject(json);
                return (result.id,result.fechaAsiento);
            }
        }
    }
}
