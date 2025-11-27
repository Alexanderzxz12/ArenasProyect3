using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArenasProyect3.Modulos.Resourses
{

    public class ClassResourses
    {
        private static readonly HttpClient client = new HttpClient();


        public static void RegistrarAuditora(int idAccion, string mantenimiento, int idProceso,int? idUsuario = null, string descripcion = null, int? idGeneral = null)
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

                if (!string.IsNullOrEmpty(descripcion)){cmd.Parameters.AddWithValue("@descripcion", descripcion);}
                else{cmd.Parameters.AddWithValue("@descripcion", DBNull.Value);}

                cmd.Parameters.AddWithValue("@idTipoAccion", idAccion);
                cmd.Parameters.AddWithValue("@nombreUsuarioSesion", usuarioWindows);

                if (idGeneral.HasValue){cmd.Parameters.AddWithValue("@idGeneral", idGeneral.Value);}
                else{cmd.Parameters.AddWithValue("@idGeneral", DBNull.Value);}

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
        public static void RefrescarRequerimientos(string tipoOperacion,DataGridView DGV, Action metodoCarga)
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

        //METODO PARA LA REDACCIÓN DE TEXTO USANDO GEMINI
        public async Task<string> RedactarTexto(string texto, string modeloseleccionado)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(texto)) return "ERROR: EL TEXTO DE ENTRADA ESTÁ VACÍO.";

                // --- LIMPIEZA PREVIA INTELIGENTE (INPUT) ---
                string textoLimpio = Regex.Replace(texto, @"^\s*\d+[\.\)]?\s*$", "", RegexOptions.Multiline);

                // Normalización estándar
                textoLimpio = textoLimpio.Replace("\"", "'").Trim();

                string apiKey = ConfigurationManager.AppSettings["GeminiApiKey"];
                if (string.IsNullOrEmpty(apiKey)) return "ERROR: NO SE ENCONTRÓ LA API KEY.";

                string[] modelosGemini = {
                "gemini-2.5-flash",
                "gemini-1.5-pro",
                "gemini-1.5-flash",
                "gemini-2.0-flash-exp"
            };

                IEnumerable<string> modelosAUsar = modeloseleccionado == "gemini"
                                                   ? modelosGemini
                                                   : new[] { modeloseleccionado };

                string textoFinal = "";
                bool solicitudExitosa = false;
                Exception ultimoError = null;

                // --- PROMPT ANTI-ALUCINACIONES NUMÉRICAS ---
                string promptSistema = @"
                Actúa como un experto redactor técnico.
                TU TAREA: Reestructurar la información en un informe profesional limpio y secuencial.

                REGLAS CRÍTICAS DE NUMERACIÓN:
                1. IGNORA ABSOLUTAMENTE cualquier numeración del texto original (como '37.', '01.', números de página).
                2. Ten CUIDADO con nombres de eventos que incluyen números (ej: 'PERUMIN 37'). NO uses ese '37' como viñeta.
                3. GENERA TU PROPIA numeración secuencial lógica empezando estrictamente desde 1 (1., 2., 3...).
                
                ESTRUCTURA DE SALIDA:
                - Primer Párrafo: Resumen ejecutivo (TEXTO CORRIDO, SIN NÚMEROS AL INICIO).
                - Cuerpo: Lista numerada ordenada (1., 2., 3...).
                - Detalles: Usa viñetas (-) para sub-puntos.

                Devuelve SOLO el texto plano reestructurado.
            ";

                foreach (var modeloActual in modelosAUsar)
                {
                    try
                    {
                        string url = $"https://generativelanguage.googleapis.com/v1beta/models/{modeloActual}:generateContent?key={apiKey}";

                        var payload = new
                        {
                            contents = new[]
                            {
                                new {
                                        parts = new[] {
                                            new { text = promptSistema + "\n\nTEXTO FUENTE:\n" + textoLimpio }
                                        }
                                    }
                            },
                            generationConfig = new
                            {
                                temperature = 0.3,
                                maxOutputTokens = 2000,
                                candidateCount = 1
                            }
                        };

                        string jsonPayload = JsonConvert.SerializeObject(payload);
                        StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                        var response = await client.PostAsync(url, content).ConfigureAwait(false);

                        if (!response.IsSuccessStatusCode) throw new Exception($"Error HTTP {response.StatusCode}");

                        string jsonResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        dynamic data = JsonConvert.DeserializeObject(jsonResponse);

                        string respuestaTexto = data?.candidates?[0]?.content?.parts?[0]?.text;

                        if (!string.IsNullOrWhiteSpace(respuestaTexto))
                        {
                            textoFinal = respuestaTexto;
                            solicitudExitosa = true;
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        ultimoError = ex;
                    }
                }

                if (!solicitudExitosa) return $"ERROR: {ultimoError?.Message ?? "Sin respuesta"}";

                // --- LIMPIEZA FINAL ---

                // 1. Normalizar saltos de línea
                textoFinal = textoFinal.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", Environment.NewLine);

                // 2. Eliminar Markdown residual
                textoFinal = textoFinal.Replace("**", "").Replace("##", "").Replace("`", "");

                // 3. LIMPIEZA DE "RUIDO" POST-GENERACIÓN (SOLUCIÓN AL 37 SUELTO)
                // Borra cualquier línea que haya quedado conteniendo solo números y puntos (ej: una línea que sea solo "37.")
                textoFinal = Regex.Replace(textoFinal, @"(?m)^\s*\d+[\.\)]\s*(\r\n|\r|\n|$)", "");

                // 4. Limpieza de espacios dobles
                textoFinal = Regex.Replace(textoFinal, @" +", " ");

                // 5. Formato de lista: Asegurar espacio después del punto ("1.Texto" -> "1. Texto")
                textoFinal = Regex.Replace(textoFinal, @"(\d+\.)([^\s])", "$1 $2");

                textoFinal = Regex.Replace(textoFinal, @"(?<=[\.\:\?!]|\r|\n|^)\s+(\d+\.)", $"{Environment.NewLine}{Environment.NewLine}$1");

                // 7. Eliminar saltos de línea excesivos (más de 3 seguidos se vuelven 2)
                textoFinal = Regex.Replace(textoFinal, @"(\r\n){3,}", $"{Environment.NewLine}{Environment.NewLine}");

                textoFinal = textoFinal.ToUpper().Trim();

                return textoFinal;
            }
            catch (Exception ex)
            {
                return "EXCEPCIÓN: " + ex.Message.ToUpper();
            }
        }

        //-----------------------------------------------------------------------
        //----------------------------
        //METODO PARA TRANSCRIBIR AUDIO A TEXTO USANDO GEMINI   
        public async Task<string> TranscribirAudioATexto(string rutaArchivoaudio)
        {
            try
            {
                string apikey = ConfigurationManager.AppSettings["GeminiApiKey"];

                if (!File.Exists(rutaArchivoaudio))
                {
                    return "ERROR: EL ARCHIVO DE AUDIO NO EXISTE";
                }

                //EXTENSION DEL ARCHIVO EN MINUSCULAS
                string extension = Path.GetExtension(rutaArchivoaudio).ToLower();
                string mimeType = "";

                //CAPTURA DE LA EXTENSIÓN Y ASIGNACIÓN DEL MIME TYPE
                switch (extension)
                {
                    case ".mp3": mimeType = "audio/mp3"; break;
                    case ".wav": mimeType = "audio/wav"; break;
                    case ".ogg": mimeType = "audio/ogg"; break;
                    case ".opus": mimeType = "audio/ogg"; break;
                    case ".m4a": mimeType = "audio/m4a"; break;
                    default: return "ERROR: FORMATO NO SOPORTADO";
                }

                byte[] audioBytes = File.ReadAllBytes(rutaArchivoaudio);
                string audioBase64 = Convert.ToBase64String(audioBytes);

                // USO DEL MODELO GEMINI DE GOOGLE 2.5 FLASH
                string modelo = "gemini-2.5-flash";
                string url = $"https://generativelanguage.googleapis.com/v1beta/models/{modelo}:generateContent?key={apikey}";

                //CUERPO DEL JSON A ENVIAR
                var jsondata = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new object[]
                            {
                                new { inlineData = new { mimeType = mimeType, data = audioBase64 } },
                                new { text = "Transcribe esto textualmente." }
                            }
                        }
                    }
                };

                using (HttpClient client = new HttpClient())
                {
                    string jsonString = JsonConvert.SerializeObject(jsondata);
                    StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    // Llamada única a la API
                    var response = await client.PostAsync(url, content);

                    // Verificación estándar de errores (400, 401, 404, 500, etc.)
                    if (!response.IsSuccessStatusCode)
                    {
                        string errorMsg = await response.Content.ReadAsStringAsync();
                        return $"ERROR API: {response.StatusCode} - {errorMsg}";
                    }

                    dynamic data = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

                    if (data["candidates"] == null)
                    {
                        return "ERROR: LA IA NO PUDO ESCUCHAR.";
                    }


                    return data["candidates"][0]["content"]["parts"][0]["text"].ToString();
                }
            }
            catch (Exception ex)
            {
                return "ERROR CRÍTICO: " + ex.Message;
            }
        }
    }
}
