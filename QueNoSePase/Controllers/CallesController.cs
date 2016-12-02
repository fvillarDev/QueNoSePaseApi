using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.Http;
using Newtonsoft.Json;
using QueNoSePase.API.Models;
using QueNoSePase.API.WebDataClient;

namespace QueNoSePase.API.Controllers
{
    public class CallesController : ApiController
    {
        //GET  api/calles/162001
        public string Get(string id)
        {
            try
            {
                var lineas = LineasController.GetLineasBackup();
                var linea = lineas.Find(item => item.Codigo == id);
                if(linea == null)
                    return JsonConvert.SerializeObject("Error. No se encontro una linea para el codigo ingresado");

                var calles = Helper.ParseCallesAspx(Helper.GetCallesAspx(linea.Codigo));

                return JsonConvert.SerializeObject(calles);
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(ex);
            }
        }

        //private string GetCallesAspx(string codigo)
        //{
        //    var p = new NameValueCollection()
        //       {
        //           { "uWeb", "usuarioefisat" },
        //           { "cWeb", "efisat" },
        //           { "funcion", "CallePrincipalLinea"},
        //           { "userId", Constants.USER_ID},
        //           { "codigoLineaGrupo", codigo }
        //       };
        //    return Helper.HttpPost(p);
        //}

        //private List<Calle> ParseCallesAspx(string aspx)
        //{
        //    List<Calle> aux = new List<Calle>();
        //    if (aspx.StartsWith("0"))
        //    {
        //        var spl = aspx.Split('|');
        //        foreach (string s in spl)
        //        {
        //            if (!string.IsNullOrEmpty(s) && s.Contains(";"))
        //            {
        //                var spl2 = s.Split(';');
        //                aux.Add(new Calle
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
