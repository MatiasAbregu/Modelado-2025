using Blazor.BD;
using Blazor.BD.Entidades;
using Blazor.Repositorios.Implementaciones;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Repositorios.Servicios
{
    public class BaseServicio<T, TKey> : IBaseServicio<T, TKey> where T : class, IEntityBase<TKey>
    {
        private readonly AppDbContext baseDeDatos;

        public BaseServicio(AppDbContext baseDeDatos)
        {
            this.baseDeDatos = baseDeDatos;
        }

        public async Task<T?> BuscarPorId(TKey id)
        {
            return await baseDeDatos.Set<T>().FindAsync(id);
        }

        public virtual async Task<List<T>> BuscarTodo()
        {
            try
            {
                return await baseDeDatos.Set<T>().ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ocurrio un error en buscar todo: {ex.Message}");
                return null;
            }
        }

        public virtual async Task<T> InsertarNuevoRegistro(T entidad)
        {
            try
            {
                baseDeDatos.Set<T>().Add(entidad);
                await baseDeDatos.SaveChangesAsync();
                return entidad;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public virtual async Task<bool> Actualizar(TKey id, T entidad)
        {
            if (!id.Equals(entidad.Id)) return false;

            var exist = await baseDeDatos.Set<T>().FirstOrDefaultAsync(e => e.Id.Equals(id));
            if (exist == null) return false;

            try
            {
                baseDeDatos.Set<T>().Update(entidad);
                await baseDeDatos.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> Eliminar(TKey id)
        {
            var entidad = await baseDeDatos.Set<T>().FirstOrDefaultAsync(e => e.Id.Equals(id));
            if (entidad == null) return false;

            try
            {
                baseDeDatos.Set<T>().Remove(entidad);
                await baseDeDatos.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}