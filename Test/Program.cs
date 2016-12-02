using System;
using System.Collections.Specialized;
using System.Text;
using QueNoSePase.API;
using QueNoSePase.API.Models;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string posicion = "-31,368277;-64,233481";
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

            int index = 0;
            StringBuilder sb = new StringBuilder();
            string baseurl = "map.quenosepase.com.ar?user=" + posicion.Replace(",", ".") + "&paradas=";
            foreach (ParadaCercana cercana in paradas)
            {
                if(cercana.Parada == null) continue;

                var url = index + ",Lineas " + string.Join("-", cercana.Lineas) + "," +
                          cercana.Parada.NumeroParada + "," + cercana.Parada.Latitud.Replace(",", ".") + ";" + cercana.Parada.Longitud.Replace(",", ".");
                sb.Append(url + "|");
                index++;
            }
            string result = baseurl + sb.ToString();

            Console.ReadLine();
        }
    }
}
