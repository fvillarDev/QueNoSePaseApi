using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Http;
using Newtonsoft.Json;
using QueNoSePase.API.Models;
using QueNoSePase.API.WebDataClient;

namespace QueNoSePase.API.Controllers
{
    public class HorariosController : ApiController
    {
        //0| 22,MARQUES A KENNEDY12:53//22,MARQUES A KENNEDY13:08//22,MARQUES A KENNEDY13:23//22,MARQUES A KENNEDY13:38//22,MARQUES A KENNEDY13:53//
        [Route("api/horarios/{parada}")]
        public string Get(string parada)
        {
            try
            {
                ServicioWebHorariosProximos a = new ServicioWebHorariosProximos();
                var aux = a.RecuperarHorarioCuandoLlegaDosAndroid(parada, Constants.USER_ID, "TODOS", Constants.USUARIO,
                    Constants.CLAVE, "-1");

                var list = new List<Horario>();

                if (aux.StartsWith("0|"))
                {
                    if(aux.Contains("Sin datos"))
                    {
                        return JsonConvert.SerializeObject(new Exception("No se encontraron horarios para esta parada"));
                    }
                    var data = aux.Split('|')[1];
                    foreach (string s in data.Split(new[] { "//" }, StringSplitOptions.None))
                    {
                        if (string.IsNullOrEmpty(s)) continue;

                        var horario = new Horario();

                        var spl = s.Split(',');
                        horario.Linea = spl[0];
                        string ax = spl[1];
                        if (ax.Contains(":"))
                        {
                            int index = ax.IndexOf(":");
                            horario.Hora = ax.Substring((index - 2));
                            horario.Direccion = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(ax.Replace(horario.Hora, "").ToLower());
                        }
                        else
                        {
                            horario.Direccion = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(spl[1].ToLower());
                            if (spl[2] == "0")
                            {
                                horario.Llegando = true;
                                horario.Minutos = 0;
                            }
                            else
                                horario.Minutos = Int32.Parse(spl[2]);
                        }
                        list.Add(horario);
                    }
                }

                return JsonConvert.SerializeObject(list);
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(ex);
            }
        }
    }
}
