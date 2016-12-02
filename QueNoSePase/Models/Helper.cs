using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using QueNoSePase.API.Controllers;

namespace QueNoSePase.API.Models
{
    public class Helper
    {
        public static string HttpPost(NameValueCollection param)
        {
            using (var client = new WebClient())
            {
                var response =
                client.UploadValues("http://movilesparadas.efibus.com.ar/Receiver.aspx", param);

                return Encoding.UTF8.GetString(response);
            }
        }

        

        #region Calles
        public static List<Calle> ParseCallesAspx(string aspx)
        {
            List<Calle> aux = new List<Calle>();
            if (aspx.StartsWith("0"))
            {
                var spl = aspx.Split('|');
                foreach (string s in spl)
                {
                    if (!string.IsNullOrEmpty(s) && s.Contains(";"))
                    {
                        var spl2 = s.Split(';');
                        aux.Add(new Calle
                        {
                            Codigo = spl2[0],
                            Nombre = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(spl2[1].ToLower())
                        });
                    }
                }

                return aux;
            }
            return null;
        }
        public static string GetCallesAspx(string codigo)
        {
            var p = new NameValueCollection()
               {
                   { "uWeb", "usuarioefisat" },
                   { "cWeb", "efisat" },
                   { "funcion", "CallePrincipalLinea"},
                   { "userId", Constants.USER_ID},
                   { "codigoLineaGrupo", codigo }
               };
            return Helper.HttpPost(p);
        }

        #endregion

        #region Intersecciones

        public static string GetInterseccionesAspx(string linea, string calle)
        {
            var p = new NameValueCollection()
               {
                   { "uWeb", "usuarioefisat" },
                   { "cWeb", "efisat" },
                   { "funcion", "CalleInterseccionCallePrincipal"},
                   { "userId", Constants.USER_ID},
                   { "codigoLineaGrupo", linea },
                   { "Calle", calle}
               };
            return Helper.HttpPost(p);
        }

        public static List<Interseccion> ParseInterseccionAspx(string aspx)
        {
            List<Interseccion> aux = new List<Interseccion>();
            if (aspx.StartsWith("0"))
            {
                var spl = aspx.Split('|');
                foreach (string s in spl)
                {
                    if (!string.IsNullOrEmpty(s) && s.Contains(";"))
                    {
                        var spl2 = s.Split(';');
                        aux.Add(new Interseccion
                        {
                            Codigo = spl2[0],
                            Nombre = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(spl2[1].ToLower())
                        });
                    }
                }

                return aux;
            }
            return null;
        }

        #endregion

        #region Paradas

        public static string GetParadasAspx(string linea, string calle, string interseccion)
        {
            var p = new NameValueCollection()
               {
                   { "uWeb", "usuarioefisat" },
                   { "cWeb", "efisat" },
                   { "funcion", "ParadaLineaCalles"},
                   { "userId", Constants.USER_ID},
                   { "codigoLineaGrupo", linea },
                   { "calle", calle},
                   { "calleInterseccion", interseccion}
               };
            return Helper.HttpPost(p);
        }

        public static List<Parada> ParseParadasAspx(string aspx)
        {
            List<Parada> aux = new List<Parada>();
            if (aspx.StartsWith("0"))
            {
                var spl = aspx.Split('|');
                foreach (string s in spl)
                {
                    if (!string.IsNullOrEmpty(s) && s.Contains(";"))
                    {
                        var spl2 = s.Split(';');
                        aux.Add(new Parada
                        {
                            Codigo = spl2[0],
                            Nombre = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(spl2[1].ToLower()),
                            NumeroParada = spl2[2],
                            Latitud = spl2[3],
                            Longitud = spl2[4],
                            Fecha = spl2[5]
                        });
                    }
                }

                return aux;
            }
            return null;
        }

        #endregion

        #region Paradas Cercanas

        public static List<ParadaCercana> ParseParadasCercanasAspx(string aspx)
        {
            aspx = aspx.Split(new[] { Environment.NewLine }, StringSplitOptions.None)[0];
            List<ParadaCercana> aux = new List<ParadaCercana>();
            if (aspx.StartsWith("0"))
            {
                var spl = aspx.Split('|');
                foreach (string s in spl)
                {
                    if (!string.IsNullOrEmpty(s) && s.Contains(";"))
                    {
                        var spl2 = s.Split(';');
                        var parada = new ParadaCercana
                        {
                            Direccion = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(spl2[1].ToLower()),
                            Calles = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(spl2[2].ToLower()),
                            Lineas = ParseLineas(spl2),
                        };
                        parada.Parada = Helper.GetParadaPosition(parada.Lineas[0], parada.Calles, spl2[0]);
                        aux.Add(parada);
                    }
                }

                return aux;
            }
            return null;
        }

        private static List<string> ParseLineas(string[] aux)
        {
            var lineas = new List<string>();
            if (aux == null) return lineas;

            for (int i = 3; i < aux.Length; i++)
            {
                lineas.Add(aux[i]);
            }

            return lineas;
        }

        private static Parada GetParadaPosition(string linea, string calle_EntreCalle, string numParada)
        {
            try
            {
                if (string.IsNullOrEmpty(linea) || string.IsNullOrEmpty(calle_EntreCalle))
                    return null;

                var lineas = LineasController.GetLineasBackup();
                var lineaObj = lineas.Find(item => item.Nombre == linea);
                if (lineaObj == null)
                    return null;//JsonConvert.SerializeObject("Error. No se encontro una linea para el codigo ingresado");

                var calles = ParseCallesAspx(GetCallesAspx(lineaObj.Codigo));
                var splCalle = calle_EntreCalle.Split(new[] { " Y " }, StringSplitOptions.None);
                var calleObj = calles.Find(item => item.Nombre == splCalle[0]);
                if (calleObj == null)
                    return null;

                var intersecciones = ParseInterseccionAspx(GetInterseccionesAspx(lineaObj.Codigo, calleObj.Codigo));
                var interObj = intersecciones.Find(item => item.Nombre == splCalle[1]);
                if (interObj == null)
                    return null;

                var paradas = ParseParadasAspx(GetParadasAspx(lineaObj.Codigo, calleObj.Codigo, interObj.Codigo));
                var parada = paradas.Find(item => item.NumeroParada == numParada);
                if (parada == null)
                    return null;

                return parada;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion
        internal class UrlShortener
        {
            private const string Apikey = "AIzaSyDVR1ojllzrQlJ-YuVXWrR19Sl_1y7n6Oc";
            public static string ShortUrl(string url)
            {
                string shortUrl = url;
                try
                {
                    string post = "{\"longUrl\": \"" + url + "\"}";
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.googleapis.com/urlshortener/v1/url?key=" + Apikey);

                    request.ServicePoint.Expect100Continue = false;
                    request.Method = "POST";
                    request.ContentLength = post.Length;
                    request.ContentType = "application/json";
                    request.Headers.Add("Cache-Control", "no-cache");

                    using (Stream requestStream = request.GetRequestStream())
                    {
                        byte[] postBuffer = Encoding.ASCII.GetBytes(post);
                        requestStream.Write(postBuffer, 0, postBuffer.Length);
                    }

                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (Stream responseStream = response.GetResponseStream())
                        {
                            using (StreamReader responseReader = new StreamReader(responseStream))
                            {
                                string json = responseReader.ReadToEnd();
                                shortUrl = Regex.Match(json, @"""id"": ?""(?<id>.+)""").Groups["id"].Value;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // if Google's URL Shortner is down...
                    //System.Diagnostics.Debug.WriteLine(ex.Message);
                    //System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                }
                return shortUrl;
            }
        }
    }
}