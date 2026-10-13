using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TheTouchdownBarberShop.Infraestructure.Data;
using TheTouchdownBarberShop.Infraestructure.Models;
using TheTouchdownBarberShop.Infraestructure.Repository.Interfaces;



/// <summary>
/// QUÉ HACE:
/// Implementa el repositorio de productos (IRepositoryProducto). Es la única clase de este
/// módulo que consulta la base de datos: obtiene el listado de productos activos y el
/// detalle de un producto, junto con el nombre de su categoría.
///
/// CÓMO LO HACE:
/// Usa Entity Framework Core a través del contexto (TheTouchdownBarberShopContext). Las
/// consultas se escriben con LINQ y EF Core las traduce a sentencias SQL para SQL Server.
/// Los métodos son asincrónicos (async/await) para no bloquear el hilo de la aplicación
/// mientras la base de datos responde.
///
/// POR QUÉ LO HACE ASÍ:
/// - Implementa una interfaz: la capa Application depende de IRepositoryProducto y no de
///   esta clase concreta, por lo que el acceso a datos puede cambiarse o probarse sin
///   modificar los servicios (arquitectura en capas).
/// - Todos los datos que se muestran provienen de consultas a la BD.
/// </summary>
/// 


namespace TheTouchdownBarberShop.Infraestructure.Repository.Implementations
{
    public class RepositoryProducto : IRepositoryProducto
    {
        private readonly TheTouchdownBarberShopContext _context;

        public RepositoryProducto(TheTouchdownBarberShopContext context)
        {
            _context = context;
        }

        public async Task<Productos?> FindByIdAsync(int id)
        {
            // SELECT TOP 1 ... FROM productos p
            // INNER JOIN categorias_productos c ON ... WHERE p.id_producto = @id AND p.activo = 1
            var producto = await _context.Set<Productos>()
                .Include(p => p.IdCategoriaProductoNavigation)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.IdProducto == id && p.Activo == true);

            return producto;
        }

        public async Task<ICollection<Productos>> ListAsync()
        {
            // SELECT * FROM productos p
            // INNER JOIN categorias_productos c ON ... WHERE p.activo = 1 ORDER BY p.nombre
            var collection = await _context.Set<Productos>()
                .Include(p => p.IdCategoriaProductoNavigation)
                .AsNoTracking()
                .Where(p => p.Activo == true)
                .OrderBy(p => p.Nombre)
                .ToListAsync();

            return collection;
        }
    }
}