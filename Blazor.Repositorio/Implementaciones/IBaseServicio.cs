using Blazor.BD.Entidades;
using Blazor.Shared.DTO_Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Repositorios.Implementaciones
{
    public interface IBaseServicio<T, TKey> where T : class
    {
        Task<T> InsertarNuevoRegistro(T entidad);
        Task<List<T>> BuscarTodo();
        Task<T?> BuscarPorId(TKey id);
        Task<bool> Actualizar(TKey id, T entidad);
        Task<bool> Eliminar(TKey id);
    }
}
