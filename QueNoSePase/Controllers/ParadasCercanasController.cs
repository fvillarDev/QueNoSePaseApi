using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.Http;
using Newtonsoft.Json;
using QueNoSePase.API.Models;

namespace QueNoSePase.API.Controllers
{
    public class ParadasCercanasController : ApiController
    {
        //api/paradas/-31,3650534;-64,2365634
        [Route("api/paradascercanas/{posicion}")]
        public string Get(string posicion)
        {
            try
            {
                var pos = posicion.Split(';');
                var parameters = new NameValueCollection
                   {
                       { "funcion", "paradasCercanas" },
                       { "userId", Constants.USER_ID },
                       { "uWeb", Constants.USUARIO },
                       { "cWeb", Constants.CLAVE },
                       { "latitud", pos[0] },
                       { "longitud", pos[1] }
                   };
                var res = Helper.HttpPost(parameters);
                var paradas = Helper.ParseParadasCercanasAspx(res);

                return JsonConvert.SerializeObject(paradas);
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(ex);
            }
        }
    }
}
