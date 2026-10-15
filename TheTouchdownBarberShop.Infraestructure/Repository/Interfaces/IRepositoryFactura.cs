using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheTouchdownBarberShop.Infraestructure.Models;

namespace TheTouchdownBarberShop.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryFactura
    {

        Task<ICollection<Facturas>> ListAsync();
        Task<Facturas?> FindByIdAsync(int id);

    }
}
