using TheTouchdownBarberShop.Infraestructure.Models;

namespace TheTouchdownBarberShop.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryFactura
    {
        // Existentes
        Task<ICollection<Facturas>> ListAsync();
        Task<Facturas?> FindByIdAsync(int id);

        // Historial y detalle por encargado (solo facturas de su sucursal)
        Task<ICollection<Facturas>> ListByEncargadoAsync(int idEncargado);
        Task<Facturas?> FindByIdAsync(int id, int idEncargado);

        // Encargados y datos del encargado actual
        Task<ICollection<Usuario>> ListEncargadosAsync();
        Task<Usuario?> FindEncargadoAsync(int idEncargado);
        Task<int?> ObtenerSucursalEncargadoAsync(int idEncargado);

        // Catálogos del formulario de factura
        Task<ICollection<Usuario>> ListClientesAsync();
        Task<ICollection<Producto>> ListProductosAsync();
        Task<ICollection<Servicios>> ListServiciosAsync();

        // Registro
        Task AddAsync(Facturas factura);
        Task<bool> UpdateAsync(Facturas factura);   // convierte una proforma en factura
    }
}