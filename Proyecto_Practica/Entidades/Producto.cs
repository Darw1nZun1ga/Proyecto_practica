using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    //Clase debe ser publica
    public class Producto
    {
        //Propieades de clase producto
        public int Codigo { get; set; }
        public string Descripcion { get; set; }
        public int Existencia { get; set; }
        public decimal Precio { get; set; }
        public DateTime FechaCreacion { get; set; }
        public byte[] Imagen { get; set; }
    }
}
