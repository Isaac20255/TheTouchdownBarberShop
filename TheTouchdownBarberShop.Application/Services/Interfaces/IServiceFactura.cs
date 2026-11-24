using TheTouchdownBarberShop.Application.DTO;

namespace TheTouchdownBarberShop.Application.Services.Interfaces
{
    public interface IServiceFactura
    {
        // Existentes
        Task<ICollection<FacturaDTO>> ListAsync();
        Task<FacturaDTO?> FindByIdAsync(int id);

        // Historial y detalle por encargado
        Task<ICollection<FacturaDTO>> ListAsync(int idEncargado, string? cliente = null, DateTime? fecha = null);
        Task<FacturaDTO?> FindByIdAsync(int id, int idEncargado);
        Task<List<(int IdUsuario, string Descripcion)>> ListEncargadosAsync();

        // Registro de factura (nueva o desde una proforma)
        Task<FacturaDTO?> PrepararNuevaAsync(int idEncargado);
        Task<FacturaDTO?> PrepararDesdeProformaAsync(int idFactura, int idEncargado);
        Task<int?> RegistrarAsync(FacturaDTO dto, int idEncargado);

        // Catálogos del formulario
        Task<List<(int Id, string Nombre, string Correo)>> ListClientesAsync();
        Task<List<(int Id, string Nombre, decimal Precio)>> ListProductosAsync();
        Task<List<(int Id, string Nombre, decimal Precio)>> ListServiciosAsync();
    }
}