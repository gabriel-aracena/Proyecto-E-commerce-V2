using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Pedido
{
    public int Pedidold { get; set; }
    public int Usuariold { get; set; }
    public DateTime FechaPedido { get; set; }

    // Pendiente, Pagado, Enviado, Cancelado
    public string Estado { get; set; } 
    public decimal Total { get; set; }
    public ICollection<DetallePedido> DetallesPedido { get; set; }

    // Navigation properties
    public Envio Envio { get; set; }
}