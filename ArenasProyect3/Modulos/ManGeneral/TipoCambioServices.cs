using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ArenasProyect3.Modulos.ManGeneral
{
    public class TipoCambioServices
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<JObject> ObtenerTipoCambioAsync(string fecha)
        {
            string apiKey = "SsPENeVLVLSRy3z//qWjyQ=="; // Coloca aquí tu clave de API
            string url = $"https://api.sunat.gob.pe/v1/tipo_cambio?fecha={fecha}";

            try
            {
                // Elimina cualquier encabezado 'Authorization' existente
                if (client.DefaultRequestHeaders.Contains("Authorization"))
                {
                    client.DefaultRequestHeaders.Remove("Authorization");
                }

                // Añade el encabezado de autorización correcto
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
                var response = await client.GetAsync(url);

                // Verificar si la respuesta es exitosa
                response.EnsureSuccessStatusCode();

                // Leer el cuerpo de la respuesta
                string responseBody = await response.Content.ReadAsStringAsync();
                return JObject.Parse(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Mensaje: {0}", e.Message);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine("\nOcurrió un error inesperado:");
                Console.WriteLine("Mensaje: {0}", e.Message);
                throw;
            }
        }

    }
}
