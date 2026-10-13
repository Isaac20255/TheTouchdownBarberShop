using TheTouchdownBarberShop.Application.DTOs;
using TheTouchdownBarberShop.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheTouchdownBarberShop.Application.Services.Interfaces
{
    /// <summary>
    /// Contrato de interfaz para el servicio de gestión de productos.
    /// Define las operaciones de lectura asíncronas para desacoplar la lógica de negocio
    /// de los detalles de la infraestructura y acceso a datos.
    /// </summary>
    /// <remarks>
    /// - ¿Cómo funciona?: Se implementa en un servicio específico (ej. ServiceProducto) inyectando los repositorios o el DbContext necesario.
    /// - ¿Por qué se utiliza?: Sigue los principios SOLID (Inversión de Dependencias y Responsabilidad Única) y permite el uso de Dependency Injection (DI) y pruebas unitarias (Mocks/Stubs).
    /// - ¿Para qué sirve?: Expone los métodos necesarios para la consulta global de productos y la búsqueda individual por identificador único.
    /// </remarks>
    public interface IServiceProducto
    {
        /// <summary>
        /// Obtiene de forma asíncrona la lista completa de productos.
        /// </summary>
        /// <returns>Una colección de objetos <see cref="ProductoDTO"/>.</returns>
        Task<ICollection<ProductoDTO>> ListAsync();

        /// <summary>
        /// Busca un producto específico por su identificador único de forma asíncrona.
        /// </summary>
        /// <param name="id">Identificador numérico del producto.</param>
        /// <returns>El objeto <see cref="ProductoDTO"/> correspondiente, o <c>null</c> si no se encuentra.</returns>
        Task<ProductoDTO?> FindByIdAsync(int id);
    }
}