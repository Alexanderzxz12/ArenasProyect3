using DocumentFormat.OpenXml.Office2010.Word;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ArenasProyect3.Modulos.Resourses
{

    public class ClassResourses
    {

        //CREACION DEL OBJETO CLIENTE PARA EL LLAMADO A LAS APIS
        //TIMEOUT DE 30 MINUTOS PARA QUE SOPORTE UAIDOS LARGOS SIN QUE SE CORTE
        private static readonly HttpClient client = new HttpClient { Timeout = TimeSpan.FromMinutes(30) };

        //CLAVE API KEY 
        //private static string ApiKeyGemini => ConfigurationManager.AppSettings["GeminiApiKey"];

        private static readonly string apiKey = ConfigurationManager.AppSettings["GeminiApiKey"];


        public static void RegistrarAuditora(int idAccion, string mantenimiento, int idProceso, int? idUsuario = null, string descripcion = null, int? idGeneral = null)
        {
            try
            {
                string usuarioWindows = Environment.UserName;
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("Auditoria_Registro", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                cmd.Parameters.AddWithValue("@mantenimiento", mantenimiento);

                if (!string.IsNullOrEmpty(descripcion)) { cmd.Parameters.AddWithValue("@descripcion", descripcion); }
                else { cmd.Parameters.AddWithValue("@descripcion", DBNull.Value); }

                cmd.Parameters.AddWithValue("@idTipoAccion", idAccion);
                cmd.Parameters.AddWithValue("@nombreUsuarioSesion", usuarioWindows);

                if (idGeneral.HasValue) { cmd.Parameters.AddWithValue("@idGeneral", idGeneral.Value); }
                else { cmd.Parameters.AddWithValue("@idGeneral", DBNull.Value); }

                cmd.Parameters.AddWithValue("@idProceso", idProceso);

                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //MÉTODO PARA ENVIAR CORREOS POR LA ANULACIÓN DE UN REQUERIMIENTO
        public static void Enviar(string para, string asunto, string mensaje)
        {
            var outlokkApp = new Microsoft.Office.Interop.Outlook.Application();
            var mailItem = (Microsoft.Office.Interop.Outlook.MailItem)outlokkApp.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
            mailItem.To = para;
            mailItem.Subject = asunto;
            mailItem.Body = mensaje;

            mailItem.Send();
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(mailItem);
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(outlokkApp);
        }

        //METODO PARA LIMPIAR EL TEXTO SI CONTIENE ESPACIOS EN BLANCO O SALTOS DE LINEA
        public static void LimpiarTexto_EspaciosEnBlanco(TextBox txt)
        {
            string limpiartexto = Regex.Replace(txt.Text.Trim(), @"\s+", " ");

            txt.Text = limpiartexto;
        }

        //METODO QUE REFRESCA LOS REQUERIMIENTOS GUARDANDO EL SCROLL
        public static void RefrescarRequerimientos(string tipoOperacion, DataGridView DGV, Action metodoCarga)
        {
            //VARIABLES QUE ALMACENARAN EL SCROLL Y LA FILA SELECCIONADA POR EL USUARIO
            int indiceScroll = 0;
            int filaseleccionada = -1;

            //SI EXISTEN FILAS O SI LA FILA DE PUEDE LEER
            if (DGV.RowCount > 0 && DGV.DisplayedRowCount(true) > 0)
            {
                //ASIGNA EL NUMERO LA PRIMERA FILA QUE SE PUEDE VISUALIZAR EN EL CONTROL
                indiceScroll = DGV.FirstDisplayedScrollingRowIndex;

                //ASIGNAR LA FILA SELECCIONADA POR EL USUARIO
                filaseleccionada = DGV.CurrentRow?.Index ?? -1;  //SI NO SE SELECCIONO NINGUNA SE GUARDARA COMO -1
            }

            //METODO QUE CARGA LOS REGISTROS NUEVAMENTE DE LA BASE DE DATOS
            metodoCarga();

            //SI LA OPERACION QUE SE REALICE ES GUARDAR Y
            if (tipoOperacion == "guardar" && DGV.Rows.Count > 0)
            {
                //SE AGARRA LA ULTIMA FILA
                int ultimaFila = DGV.Rows.Count - 1;

                //SE DIRIGE A LA ULTIMA FILA
                DGV.FirstDisplayedScrollingRowIndex = ultimaFila;

                // Y SE SELECCIONA
                DGV.Rows[ultimaFila].Selected = true;
            }
            else
            {
                //SI ES UN TIPO DE OPERACION COMO APROBAR,ANULAR,LIBERAR,GENERAR,ETC

                //SE LE PASA EL SCROLL
                if (indiceScroll >= 0 && indiceScroll < DGV.Rows.Count)
                {
                    DGV.FirstDisplayedScrollingRowIndex = indiceScroll;
                }

                //Y SE SELECCIONA LA FILA SELECCIONADA
                if (filaseleccionada >= 0 && filaseleccionada < DGV.Rows.Count)
                {
                    DGV.ClearSelection();
                    DGV.Rows[filaseleccionada].Selected = true;
                    DGV.CurrentCell = DGV.Rows[filaseleccionada].Cells[0];
                }
            }
        }

        //--------------------------------------------------------------
        //------------------ MÉTODOS PARA GEMINI -------------------------

        public async Task<string> RedactarTexto(string texto,string IAutilizada)
        {
            try
            {
                if (string.IsNullOrEmpty(apiKey))
                {
                    return "Error: No se encontro la API KEY";
                }

                if (string.IsNullOrWhiteSpace(texto))
                {
                    return "Error: El texto de entrada esta vacio";
                }

                //MODELOS V1beta (DE TESTING Y PRUEBA)
                //string[] modelosGemini = {"gemini-2.5-flash","gemini-1.5-pro","gemini-1.5-flash","gemini-2.0-flash-exp"};

                //MODELOS V1 (OFICIALES)
                string[] modelosGemini = { "gemini-2.5-flash-lite", "gemini-2.5-flash", "gemini-2.5-pro" };

                string prompt = @"
                                1.ROL: Actua como un experto redactor técnico especializado en informes de ingenieria y mantenimiento.

                                2.CONTEXTO: El texto que recibiras proviene de notas rápidas, Contiene errores gramaticales, ideas repetidas, muletillas y oraciones inconexas.

                                3.TAREA: Tu objetivo es reescribir y estructurar este contenido para convertirlo en un INFORME PROFESIONAL. Debes:
                                - Corregir ortografia y gramática.
                                - Eliminar redundancias y muletillas.
                                - Sintetizar las ideas principales.
                                - Agrupar temas relacionados aunque estén separados en el texto original.
                                - Mantener nombres propios (como 'Lucas', 'Sistemas') y datos técnicos exactos.

                                4.FORMATO DE SALIDA (Estructura visual): Organiza la respuesta estrictamente con esta estructura:
                                
                                RESUMEN EJECUTIVO:
                                (Un párrafo breve de 3 líneas resumiendo la situación general)
                              
                                PUNTOS CLAVE:
                                - (Lista con viñetas sobre los temas tratados).
                                - (Usa lenguaje técnico apropiado).

                                ACUERDOS Y TAREAS:
                                1. (Lista numerada con las acciones a realizar o conclusiones, si las hay).
                                2. (Si no hay acuerdos claros, omite esta sección).

                                5. RESTRICCIONES (Lo prohibido)
                                - NO incluyas saludos, despedidas ni frases como 'Aqui esta tu reporte'.
                                - NO inventes información que no esté en el texto fuente
                                - NO uses Markdown complejo (negritas o cursivas). usa solo texto plano.
                                - Escribe todo en ESPAÑOL NEUTRO Y FORMAL
                                ";

                string textofinal = "";
                bool solicitudexitosa = false;
                string ultimoerror = "";

                foreach (var modeloactual in modelosGemini)
                {
                    try
                    {
                        //VARIABLES PARA LA VALIDACIÓN DE REINTENTOS DEL MISMO MODELO SI HAY ERROR 429 - SATURACIÓN
                        int intentosmaximos = 3;
                        int tiempoesperasegundos = 2;

                        for (int intentos = 1; intentos <= intentosmaximos; intentos++)
                        {
                            //LLAMADO A LA API DE MODELOS DE PRUEBA
                            //string url = $"https://generativelanguage.googleapis.com/v1beta/models/{modeloActual}:generateContent?key={apiKey}";

                            //LLAMADO A LA API DE MODELOS DE OFICIALES
                            string url = $"https://generativelanguage.googleapis.com/v1/models/{modeloactual}:generateContent?key={apiKey}";

                            //CUERPO DEL JSON
                            var json = new
                            {
                                contents = new[]
                                {
                                    new
                                    {
                                        parts = new[]
                                        {
                                            new { text = prompt + "\n\nTEXTO FUENTE:\n" + texto }
                                        }
                                    }
                                },
                                generationConfig = new
                                {
                                    temperature = 0.1,  //LO MENOS POSIBLE PARA RESPUESTAS MÁS PRECISAS
                                    maxOutputTokens = 10000, //CANTIDAD DE TOKENS A USAR PARA LA REDACCIÓN
                                    candidateCount = 1  // NÚMERO DE RESPUESTAS A GENERAR
                                }
                            };

                            var jsonAenviar = new StringContent(JsonConvert.SerializeObject(json), Encoding.UTF8, "application/json");
                            var response = await client.PostAsync(url, jsonAenviar);

                            // ERROR 429 Y ERROR 503
                            if(response.StatusCode == (System.Net.HttpStatusCode)429 || response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
                            {
                                if(intentos < intentosmaximos)
                                {
                                    //ESPERA DE 2 SEGUNDOS
                                    await Task.Delay(tiempoesperasegundos * 1000);

                                    //TIEMPO DE ESPERA PARA EL SIGUEINTE INTENTO
                                    tiempoesperasegundos = tiempoesperasegundos * 2;

                                    continue;
                                }
                                else
                                {
                                    string errorMsg = await response.Content.ReadAsStringAsync();
                                    ultimoerror = $"Saturación {response.StatusCode} - {errorMsg}";
                                    break;
                                }
                            }

                            //OTROS ERRORES HTTP (PASA AL SIGUIENTE MODELO)
                            if (response.IsSuccessStatusCode == false)
                            {
                                string errordetalle = await response.Content.ReadAsStringAsync();
                                ultimoerror = $"Error HTTP {response.StatusCode} - {errordetalle}";
                                break;
                            }

                            string jsonRespuesta = await response.Content.ReadAsStringAsync();
                            dynamic data = JsonConvert.DeserializeObject(jsonRespuesta);
                            string respuestaTexto = data?.candidates?[0]?.content?.parts?[0]?.text;

                            if (!string.IsNullOrWhiteSpace(respuestaTexto))
                            {
                                textofinal = respuestaTexto.Trim();
                                solicitudexitosa = true;
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ultimoerror = ex.Message;
                        await Task.Delay(1000);
                    }

                    if(solicitudexitosa == true)
                    {
                        break;
                    }
                }

                if(solicitudexitosa == true)
                {                  
                    string textolimpio = LimpiezaTexto(textofinal);

                    return textolimpio;
                }
                else
                {
                    return $"Error al momento de llamar a la IA: {ultimoerror}";
                }
            }
            catch (Exception ex)
            {
                return $"Error Critico: {ex.Message}";
            }
        }

        //METODO PARA LIMPIAR EL TEXTO DEVUELTO POR LA IA
        private string LimpiezaTexto(string texto)
        {

            //NORMALIZAR SALTOS DE LÍNEA DE LINUX A WINDOWS (PARA QUE NO SE VEA EL TEXTO SEGUIDO)
            texto = texto.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", Environment.NewLine);

            //ELIMINAR MARKDOWN RESIDUAL (NEGRITA ** Y ENCABEZADOS ##)
            texto = texto.Replace("**", "").Replace("##", "").Replace("###", "").Replace("`","");

            //ELIMINACIÓN POR SI 1 LINEA SOLO ES UN NÚMERO
            texto = Regex.Replace(texto, @"(?m)^\s*\d+[\.\)]\s*(\r\n|\r|\n|$)", "");

            //ELMINACIÓN DE ESPACIOS DOBLES INNECESARIOS
            texto = System.Text.RegularExpressions.Regex.Replace(texto, @" +", " ");

            // ESPACIO LUEGO DE DOS PUNTOS (TITULO:TEXTO)
            texto = texto.Replace(":", ": ");

            //FORMATO DE LISTA: ASEGURAR ESPACIO DESPUÉS DEL PUNTO (1.Texto -> 1. Texto)
            texto = Regex.Replace(texto, @"(\d+\.)([^\s])", "$1 $2");

            //SALTO DE LÍNEA ANTES DE CADA NÚMERO DE LISTA
            texto = Regex.Replace(texto, @"(?<=[\.\:\?!]|\r|\n|^)\s+(\d+\.)", $"{Environment.NewLine}{Environment.NewLine}$1");

            //ELIMINAR SALTOS DE LÍNEA EXCESIVOS (MÁS DE 3 SEGUIDOS SE VUELVEN 2)
            texto = Regex.Replace(texto, @"(\r\n){3,}", $"{Environment.NewLine}{Environment.NewLine}");

            return texto.ToUpper();
        }

        //-----------------------------------------------------------------------------
        //TRANSCRIPCION AUDIO A TEXTO
        // NUEVO ESTRUCTURA DE METODOS PARA LA GENERACIÓN DE TRANSCRIPCION DE AUDIO A TEXTO

        //METODO PARA SUBIR EL ARCHIVO COMO BYTES
        public async Task<string[]> SubirComoBytes(string rutaarchivo, string mimetype)
        {
            //URL PARA LA SUBIDA DEL ARCHIVO
            string url = $"https://generativelanguage.googleapis.com/upload/v1beta/files?key={apiKey}";

            //SEPARADOR DE MULTIPART PARA LOS METADATOS Y LA URI
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");

            string nombrearchivo = Path.GetFileName(rutaarchivo);

            // CREAR EL FORMULARIO MULTIPART CON LOS DATOS (PASANDOLE EL SEPARADOR boundary)
            using (var form = new MultipartFormDataContent(boundary))
            {
                // FORMACION DEL JSON DE METADATOS
                var metadata = new { file = new { display_name = nombrearchivo } };  //NOMBRE DEL ARCHIVO A SUBIR

                //SERIALIZACION DEL JSON Y CAMBIO EN LA CABECERA PARA QUE SE LLAME metadata
                var jsonContent = new StringContent(JsonConvert.SerializeObject(metadata), Encoding.UTF8, "application/json");
                jsonContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = "\"metadata\"" };

                //AGREGADO AL FORMULARIO
                form.Add(jsonContent);

                //LECUTRA DEL ARCHIVO EN BYTES
                byte[] archivoEnBytes = File.ReadAllBytes(rutaarchivo);

                //CREACION DEL CONTENT DEL ARCHIVO
                var fileContent = new ByteArrayContent(archivoEnBytes);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(mimetype);
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "\"file\"",
                    FileName = $"\"{nombrearchivo}\""
                };

                //AGREGADO AL FORMULARIO : JSON + BYTES
                form.Add(fileContent);

                
                form.Headers.Remove("Content-Type");
                form.Headers.TryAddWithoutValidation("Content-Type", "multipart/form-data; boundary=" + boundary);

                //ENVIO DEL FORMULARIO
                var response = await client.PostAsync(url, form);

                if (response.IsSuccessStatusCode == false)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error al Subir: {error}");
                }

                var jsonResp = await response.Content.ReadAsStringAsync();
                dynamic data = JsonConvert.DeserializeObject(jsonResp);

                string uri = data["file"]["uri"].ToString();
                string name = data["file"]["name"].ToString();

                return new string[] { uri, name };
            }
        }       

        //METODO DE TRANSCRIPCION DE AUDIO A TEXTO 
        public async Task<string> TranscribirAudioATexto(string fileUri, string mimeTypeRecibido)
        {
            int intentosmaximos = 3;
            int esperarPorSaturacion = 10;
            int pausaFinalExito = 2;

            string modelo = "gemini-2.5-flash";

            string url = $"https://generativelanguage.googleapis.com/v1beta/models/{modelo}:generateContent?key={apiKey}";

            //string prompt = "Por favor, transcribe este audio textualmente." + "Escribe todo de corrido sin agregar timestamps (00:00) ni comentarios extra.";

            //string prompt = "Por favor, transcribe este audio textualmente." + "Escribe todo de corrido agregando timestamps en cada palabra que se diga (00:00) ni comentarios extra.";

            string prompt = "Transcribe el audio textualmente. Agrega timestamps (00:00) solo al inicio de cada frase o cambio de orador. No repitas texto y no agregues comentarios extra, no escribas el prompt dentro de la transcripción.";

            var payload = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new object[]
                        {
                            new
                            {
                                fileData = new
                                {
                                    mimeType = mimeTypeRecibido,  
                                    fileUri = fileUri
                                }
                            },
                            new { text = prompt }
                        }
                    }
                }
            };

            for (int intento = 1; intento <= intentosmaximos; intento++)
            {
                try
                {
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(url, jsonContent);

                    if(response.StatusCode == (System.Net.HttpStatusCode)429 || response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
                    {
                        if(intento == intentosmaximos)
                        {
                            string errorMsg = await response.Content.ReadAsStringAsync();
                            throw new Exception($"Google sigue saturado tras {intentosmaximos} intentos. {errorMsg}");
                        }

                        await Task.Delay((esperarPorSaturacion * intento) * 1000);
                        continue;
                    }

                    if (response.IsSuccessStatusCode == false)
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        throw new Exception($"Error al transcribir: {error}");
                    }

                    string responseString = await response.Content.ReadAsStringAsync();
                    dynamic data = JsonConvert.DeserializeObject(responseString);

                    await Task.Delay(pausaFinalExito * 1000);

                    if (data["candidates"] != null && data["candidates"].Count > 0)
                    {
                        string textotranscrito = data["candidates"][0]["content"]["parts"][0]["text"].ToString();

                        string textoformateado = Regex.Replace(textotranscrito, @"\(\d{2}:\d{2}\)", Environment.NewLine + "$0");

                        return textoformateado;
                    }

                    return "No se pudo generar texto (Respuesta vacía).";
                }
                catch (Exception ex)
                {
                    if (intento == intentosmaximos) throw; 
                    await Task.Delay(5000); 
                }
            }
            return "Error desconocido despues de reintentos";
        }

        //ELIMINACIÓN DEL ARCHIVO SUBIDO
        public async Task<bool> BorrarArchivoGoogle(string fileName)
        {
            try
            {
                string url = $"https://generativelanguage.googleapis.com/v1beta/{fileName}?key={apiKey}";

                var response = await client.DeleteAsync(url);

                if (response.IsSuccessStatusCode == true)
                {
                    return true;
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    return false;
                }
            }
            catch (Exception )
            {
                return false;
            }
        }

        //CAPTURA DEL MIMETYPE SEGÚN LA EXTENSIÓN DEL ARCHIVO
        public string obtenerMimeType(string rutaarchivo)
        {
            string extension = Path.GetExtension(rutaarchivo).ToLower();

            switch (extension)
            {
                // --- AUDIOS ---
                case ".mp3": return "audio/mp3";
                case ".wav": return "audio/wav";
                case ".aac": return "audio/aac";
                case ".flac": return "audio/flac";
                case ".wma": return "audio/wma";
                case ".aiff": return "audio/aiff";

                // --- AUDIOS MÓVILES --
                case ".ogg": return "audio/ogg";
                case ".opus": return "audio/ogg"; 
                case ".m4a": return "audio/m4a";
                case ".amr": return "audio/amr";

                // --- VIDEOS 
                case ".mp4": return "video/mp4";
                case ".mpeg": return "video/mpeg";
                case ".mov": return "video/mov"; 
                case ".avi": return "video/avi";
                case ".webm": return "video/webm";
                case ".flv": return "video/x-flv";

                default: return "application/octet-stream";

            }
        }
    }
}



