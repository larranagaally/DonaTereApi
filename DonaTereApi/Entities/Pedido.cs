using System;
using System.Collections.Generic;

namespace DoñaTereApi.Entities;

public partial class Pedido
{
    public int Id { get; set; }

    public int IdCliente { get; set; }

    public DateTime FechaEntrega { get; set; }

    public decimal Total { get; set; }

    public bool Pagado { get; set; }

    public string Estado { get; set; } = null!;

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual ICollection<Pedidodetalles> Pedidodetalles { get; set; } = new List<Pedidodetalles>();
}
