using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeCBot.Dominio
{
    public class ResultadoBusca
    {
        public string Titulo { get; set; }
        public string Area { get; set; }
        public string Autor { get; set; }
        public string Descricao { get; set; }
        public string Data { get; set; }
    }
    public class ResultadoDatas
    {
        public string Data { get; set; }
    }
    public class ResultadoAutor
    { 
        public string Autor { get; set; } 
    }
}
