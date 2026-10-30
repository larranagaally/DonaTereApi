using System;
using System.Collections.Generic;

namespace DoñaTereApi.Entities;

public partial class Producto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal Precio { get; set; }

    public bool Disponible { get; set; }

    public int IdCategoria { get; set; }

    public virtual Categorias IdCategoriaNavigation { get; set; } = null!;

    public virtual ICollection<Pedidodetalles> Pedidodetalles { get; set; } = new List<Pedidodetalles>();
}
