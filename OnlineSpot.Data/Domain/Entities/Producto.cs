using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Producto
{
    public int Productold { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public string ImagenUrl { get; set; }
    public int Categoriald { get; set; }

    // Navigation properties
    //public Categoria Categoria { get; set; }
    public ICollection<DetallePedido> DetallesPedido { get; set; }
    public ICollection<DetalleCarrito> DetallesCarrito { get; set; }
}