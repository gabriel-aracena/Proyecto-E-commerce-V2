using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class DetalleCarrito
{
    public int DetalleCarritold { get; set; }
    public int Cartitold { get; set; }
    public int Productold { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }

    // Navigation properties
    //public CarritoCompra CarritoCompra { get; set; }
    public Producto Producto { get; set; }
}