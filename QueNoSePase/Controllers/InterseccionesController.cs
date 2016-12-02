using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.Http;
using Newtonsoft.Json;
using QueNoSePase.API.Models;

namespace QueNoSePase.API.Controllers
{
    public class InterseccionesController : ApiController
    {
        //GET  api/intersecciones/LINEA/CODIGO_CALLE
        //api/intersecciones/162001/18745
        [Route("api/intersecciones/{linea}/{calle}")]
        public string Get(string linea, string calle)
        {
            try
            {
                var intersecciones = Helper.ParseInterseccionAspx(Helper.GetInterseccionesAspx(linea, calle));

                return JsonConvert.SerializeObject(intersecciones);
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(ex);
            }
        }

        //private string GetInterseccionesAspx(string linea, string calle)
        //{
        //    var p = new NameValueCollection()
        //       {
        //           { "uWeb", "usuarioefisat" },
        //           { "cWeb", "efisat" },
        //           { "funcion", "CalleInterseccionCallePrincipal"},
        //           { "userId", Constants.USER_ID},
        //           { "codigoLineaGrupo", linea },
        //           { "Calle", calle}
        //       };
        //    return Helper.HttpPost(p);
        //}

        //private List<Interseccion> ParseInterseccionAspx(string aspx)
        //{
        //    List<Interseccion> aux = new List<Interseccion>();
        //    if (aspx.StartsWith("0"))
        //    {
        //        var spl = aspx.Split('|');
        //        foreach (string s in spl)
        //        {
        //            if (!string.IsNullOrEmpty(s) && s.Contains(";"))
        //            {
        //                var spl2 = s.Split(';');
        //                aux.Add(new Interseccion
        //                {
        //                    Codigo = spl2[0],
        //                    Nombre = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(spl2[1].ToLower())
        //                });
        //            }
        //        }

        //        return aux;
        //    }
        //    return null;
        //}
    }
}
