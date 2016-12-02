using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json;
using QueNoSePase.API.Models;

namespace QueNoSePase.API.Controllers
{
    public class LineasController : ApiController
    {
        private static List<Linea> _lineas;
        public LineasController()
        {
            var l =
                    "10;11;12;13;14;15;16;17;18;19;20;21;22;23;24;25;26;27;28;29;30;31;32;33;34;35;36;40;41;42;43;44;45;50;51;52;53;54;60;61;62;63;64;65;66;67;68;71;72;73;74;75;80;81;82;83;84;85;86;600;601;B30;B50;B60;B61;L70";
            var c = "1145;2145;3145;4145;5145;6145;7145;8145;9145;10145;160001;161001;162001;163001;164001;165001;166001;167001;168001;169001;172001;173001;174001;175001;176001;177001;178001;119023;120023;121023;122023;123023;124023;125023;126023;157023;147023;148023;11145;12145;13145;14145;15145;16145;17145;18145;19145;200001;201001;202001;203001;204001;180001;181001;182001;183001;184001;185001;171001;134023;135023;179001;131023;20145;21145;199001";

            var lineas = l.Split(';').ToList();
            var codigos = c.Split(';').ToList();

            _lineas = lineas.Select((t, i) => new Linea
            {
                Nombre = t,
                Codigo = codigos[i]
            }).ToList();
        }

        // GET: api/Lineas/
        public string Get()
        {
            try
            {
                //ar.com.efibus.servicioswebsms.ServicioWebHorariosProximos a = new ar.com.efibus.servicioswebsms.ServicioWebHorariosProximos();
                //var a1 = a.RecuperarLineasPorUserIDYGrupo(Constants.USER_ID, 1);
                ////devuelve todas menos los 10 (los valores estan actualizados pero no estan todas)
                
                var lineas = ParseLineasAspx(GetLineasAspx());
                if(lineas != null)
                    return JsonConvert.SerializeObject(lineas);

                return JsonConvert.SerializeObject(_lineas);
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(_lineas);
            }
        }

        private List<Linea> ParseLineasAspx(string aspx)
        {
            List<Linea> aux = new List<Linea>();
            if (aspx.StartsWith("0"))
            {
                var spl = aspx.Split('|');
                foreach (string s in spl)
                {
                    if (!string.IsNullOrEmpty(s) && s.Contains(";"))
                    {
                        var spl2 = s.Split(';');
                        aux.Add(new Linea
                        {
                            Codigo = spl2[0],
                            Nombre = spl2[1]
                        });
                    }
                }

                return aux;
            }
            return null;
        }

        private string GetLineasAspx()
        {
            var p = new NameValueCollection()
               {
                   { "uWeb", "usuarioefisat" },
                   { "cWeb", "efisat" },
                   { "funcion", "Lineas"},
                   { "userId", Constants.USER_ID},
                   { "fecha", new DateTime(1981, 08, 16).Millisecond.ToString()}
               };
            return Helper.HttpPost(p);
        }
        
        public static List<Linea> GetLineasBackup()
        {
            var l =
                    "10;11;12;13;14;15;16;17;18;19;20;21;22;23;24;25;26;27;28;29;30;31;32;33;34;35;36;40;41;42;43;44;45;50;51;52;53;54;60;61;62;63;64;65;66;67;68;71;72;73;74;75;80;81;82;83;84;85;86;600;601;B30;B50;B60;B61;L70";
            var c = "1145;2145;3145;4145;5145;6145;7145;8145;9145;10145;160001;161001;162001;163001;164001;165001;166001;167001;168001;169001;172001;173001;174001;175001;176001;177001;178001;119023;120023;121023;122023;123023;124023;125023;126023;157023;147023;148023;11145;12145;13145;14145;15145;16145;17145;18145;19145;200001;201001;202001;203001;204001;180001;181001;182001;183001;184001;185001;171001;134023;135023;179001;131023;20145;21145;199001";

            var lineas = l.Split(';').ToList();
            var codigos = c.Split(';').ToList();

            return lineas.Select((t, i) => new Linea
            {
                Nombre = t,
                Codigo = codigos[i]
            }).ToList();
        }
    }
}
