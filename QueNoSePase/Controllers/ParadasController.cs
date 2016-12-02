using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.Http;
using Newtonsoft.Json;
using QueNoSePase.API.Models;

namespace QueNoSePase.API.Controllers
{
    public class ParadasController : ApiController
    {
        //api/paradas/162001/18745/19534
        [Route("api/paradas/{linea}/{calle}/{interseccion}")]
        public string Get(string linea, string calle, string interseccion)
        {
            try
            {
                var paradas = Helper.ParseParadasAspx(Helper.GetParadasAspx(linea, calle, interseccion));

                return JsonConvert.SerializeObject(paradas);
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(ex);
            }
        }

        private string GetParadasAspx(string linea, string calle, string interseccion)
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

        private List<Parada> ParseParadasAspx(string aspx)
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
    }
}
