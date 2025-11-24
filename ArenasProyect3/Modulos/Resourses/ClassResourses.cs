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

        //METODO PARA REDACTAR TEXTO A TRAVES DE LA API DE GEMINI
        public async Task<string> RedactarTexto(string texto, string modeloseleccionado)
        {
            try
            {
                //LIMPIEZA DE CONTENIDO
                string textoLimpio = texto
                    .Replace("\r\n", " ")
                    .Replace("\n", " ")
                    .Replace("\"", "'")
                    .Trim();

                string prompt = "";
                string url = "";
                StringContent content;
                string textoFinal = "";
                bool solicitudexitosa = false;

                //CREACIÓN DE CLIENTE PARA LAS SOLICITUD DE RED HACIA LAS APIS
                using (HttpClient client = new HttpClient())
                {
                    // USO DEL MODELO GEMINI DE GOOGLE
                    if (modeloseleccionado == "gemini")
                    {
                        // 1. OBTENER LA API KEY DE FORMA SEGURA (desde App.config)
                        string apiKey = ConfigurationManager.AppSettings["GeminiApiKey"];

                        if (string.IsNullOrEmpty(apiKey))
                        {
                            return "UPS OCURRIO UN ERROR";
                        }

                        //ARREGLO DE MODELOS SI 1 DA ERROR Y NO FUNCIONA SIGUE AL SIGUIENTE MODELO HASTA QUE 1 DE ELLOS ENVIE LA REDACCIÓN
                        string[] modelosGemini = { 
                            "gemini-2.5-pro",
                            "gemini-2.5-flash",
                            "gemini-2.5-flash-lite",
                            "gemini-2.0-flash", 
                            "gemini-2.0-flash-lite",
                            "gemini-2.0-flash-exp" };

                        Exception ultimoerror = null;

                        foreach (var modeloactual in modelosGemini)
                        {
                            try
                            {                              
                                url = $"https://generativelanguage.googleapis.com/v1beta/models/{modeloactual}:generateContent?key={apiKey}";

                                // 3. PROMPT DE REDACCIÓN
                                prompt =
                                    "Eres un asistente experto en redacción de informes técnicos. Tu tarea es reestructurar y reformular el siguiente texto en ESPAÑOL en un informe profesional. " +

                                    "**REGLAS CRÍTICAS (DEBES SEGUIRLAS):**" +
                                    "1. **Introducción:** Comienza con un párrafo de resumen. Este párrafo **NUNCA DEBE SER NUMERADO** (ej: sin '5.') y **NUNCA DEBE EMPEZAR CON GUION** (-)." +
                                    "2. **Secciones Numeradas:** Después de la introducción, las secciones DEBEN empezar con un número (ej: '1. GESTIONES...')." +
                                    "3. **Palabras Compuestas:** Palabras con guion (como 'técnico-comercial') **DEBEN PERMANECER JUNTAS**. NO las separes. 'TÉCNICO\r\n-COMERCIAL' es un error grave." +
                                    "4. **Fechas y Números:** NUNCA separes una fecha o un número (ej: '...DE 202\r\n5' es un error grave)." +
                                    "5. **Viñetas:** Usa guiones (-) solo para viñetas (puntos de acción)." +
                                    "6. **Salida Limpia:** Devuelve solo el texto reformulado. Sin saludos, sin Markdown, sin explicaciones. Mantén 100% el contexto." +
                                    "7. Prohibición de Listas Falsas: Los números presentes en el texto (años, cantidades, referencias) deben permanecer como texto corrido. ESTÁ PROHIBIDO formatearlos como viñetas o elementos de lista a menos que sean explícitamente una nueva sección." +

                                    "\nEl texto a reformular es:\n\n\"" + textoLimpio + "\"";

                                // PREPARACION DEL PAQUETE DE DATOS EN FORMATO JSON
                                var jsonData = new
                                {
                                    contents = new[]
                                    {
                                        new { parts = new[] { new { text = prompt } } }
                                    },
                                    generationConfig = new
                                    {
                                        temperature = 0.4 // CREATIVIDAD BAJA PARA LA IA (0.4) PARA QUE EL RESULTADO SEA PRECISO Y NO SE VAYA POR OTRO LADO
                                    }
                                };

                                //CONVERTIR EL OBJETO A JSON
                                string json = JsonConvert.SerializeObject(jsonData);
                                content = new StringContent(json, Encoding.UTF8, "application/json");

                                //  ENVÍO DE DATOS A LA API DE GEMINI
                                var response = await client.PostAsync(url, content);
                                response.EnsureSuccessStatusCode();

                                // LECTURA DE LA RESPUESTA QUE TRAJO LA API DE GEMINI
                                string jsonResponse = await response.Content.ReadAsStringAsync();
                                dynamic data = JsonConvert.DeserializeObject(jsonResponse);

                                //SI NO TRAE NADA EN LA RESPUESTA O ESTA BLOQUEADO
                                if (data["candidates"] == null || data["candidates"][0]["content"] == null)
                                {
                                    return "ERROR: LA RESPUESTA FUE BLOQUEADA O NO ES VALIDA.";
                                }
                                //SINO SE EXTRAE EL CONTENIDO
                                else
                                {
                                    textoFinal = data["candidates"][0]["content"]["parts"][0]["text"].ToString();
                                    solicitudexitosa = true;
                                    break;
                                }
                            }
                            catch (Exception ex)
                            {
                                //SI FALLA EL MODELO QUE SE LE HA ENVIADO LOS DATOS, SE GUARDA EL ERROR Y EL BUCLE CONTINUA AL SIGUIENTE MODELO
                                ultimoerror = ex;
                            }
                        }

                        //SI NINGUN MODELO FUNCIONO SE ENVIA EL ULTIMO ERROR
                        if (!solicitudexitosa)
                        {
                            return "OCURRIO UN ERROR: " + (ultimoerror.Message.ToUpper() ?? "ERROR DESCONOCIDO");
                        }
                    }

                    else
                    {
                        return "MODELO NO SOPORTADO.";
                    }
                }

                //SI LA IA DEVOLVIO VACIO
                if (string.IsNullOrWhiteSpace(textoFinal))
                {
                    return "ERROR NO SE GENERO TEXTO.";
                }

                //LIMPIEZA FINAL DEL TEXTO
                textoFinal = textoFinal
                    .Replace("\\r", "")
                    .Replace("\\n", "\r\n")
                    .Replace("\\t", " ")
                    .Replace("```", "")
                    .Replace("**", "")
                    .Replace("* ", "\r\n- ")
                    .Trim();

                // Asegura saltos de línea ANTES de los guiones
                textoFinal = Regex.Replace(textoFinal, @"(?<!\w)\s*-\s+", "\r\n- ");

                //// Asegura DOBLE salto de línea ANTES de los números (1. TEMA, 2. TEMA)
                textoFinal = Regex.Replace(textoFinal, @"(^|[\.\:\?!]\s+)(\d+\.)\s+", "$1\r\n\r\n$2 ");

                //// Quita espacios en blanco o saltos de línea duplicados
                textoFinal = Regex.Replace(textoFinal, @"(\r\n\s*){2,}", "\r\n\r\n").Trim();

                // Quita el primer guion si está al inicio
                if (textoFinal.StartsWith("- "))
                {
                    textoFinal = textoFinal.Substring(2);
                }


                textoFinal = textoFinal.ToUpper();

                return textoFinal;
            }
            catch (Exception ex)
            {
                return "UPS OCURRIO UN ERROR: " + ex.Message.ToUpper();
            }
        }
    }
}
