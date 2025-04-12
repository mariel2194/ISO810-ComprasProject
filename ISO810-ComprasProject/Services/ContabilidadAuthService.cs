using ISO810_ComprasProject.Services.Models;
using Newtonsoft.Json;
using System.Text;

namespace ISO810_ComprasProject.Services
{
    public class ContabilidadAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ContabilidadConfig _config;

        public ContabilidadAuthService(HttpClient httpClient, ContabilidadConfig config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<(string Token, int SistemaAuxiliarId)> ObtenerTokenAsync(string user, string password)
        {
            var loginData = new
            {
                email = user,
                password = password
            };

            var content = new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_config.LoginUrl, content);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Error al obtener el token del sistema contable.");

            var json = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(json);

            return (result.token.ToString(), (int)result.usuario.sistemaAuxiliarId);
        }
    }

}
