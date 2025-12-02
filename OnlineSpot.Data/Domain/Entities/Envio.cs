using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Envio
{
    public int Enviold { get; set; }
    public int Pedidold { get; set; }
    public string? DireccionEnvio { get; set; }
    public string? Ciudad { get; set; }
    public string? Pais { get; set; }
    public string? CodigoPostal { get; set; }
    public string? MetodoEnvio { get; set; }
    public DateTime FechaEnvio { get; set; }
    public DateTime FechaEntregaEstimada { get; set; }
    public string? EstadoEnvio { get; set; }

    // Navigation property
    public Pedido Pedido { get; set; }
}