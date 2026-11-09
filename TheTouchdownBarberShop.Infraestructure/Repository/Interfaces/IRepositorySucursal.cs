using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheTouchdownBarberShop.Infraestructure.Models;

namespace TheTouchdownBarberShop.Infraestructure.Repository.Interfaces
{
    public interface IRepositorySucursal
    {
        Task<ICollection<Sucursal>> ListAsync();
        Task<Sucursal?> FindByIdAsync(int id);

        Task AddAsync(Sucursal sucursal);
        Task UpdateAsync(Sucursal sucursal);
        Task DeleteAsync(int id);
    }
}