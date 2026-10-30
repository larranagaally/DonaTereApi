namespace DoñaTere.Core.DTOs
{
    public class ReporteVentasMesDTO
    {
        public int Año { get; set; }
        public int Mes { get; set; }
        public decimal TotalVentas { get; set; }
        public int CantidadPedidos { get; set; }
    }
}
