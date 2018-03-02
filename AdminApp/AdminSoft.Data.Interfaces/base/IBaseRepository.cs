using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AdminSoft.Data.Interfaces.Base
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Encuentra un objeto por su id.
        /// </summary>
        /// <param name="keyValues">Identificador de la entidad.</param>
        /// <returns></returns>
        TEntity Find(params object[] keyValues);
        
        IQueryable<TEntity> GetAllQuery();
        /// <summary>
        /// Consulta todos los datos de una tabla, realiza la consulta.
        /// </summary>
        /// <returns>Colección de objetos</returns>
        ICollection<TEntity> GetAll();
        /// <summary>
        /// Consulta que acepta una expresión. La expresión puede ser nulo sino se desea realizar ningun filtro.
        /// </summary>
        /// <param name="where">Expresión, acepta null.</param>
        /// <param name="orders">Criterio de ordenamiento.</param>
        /// <param name="includes">Arraeglo de inclusiones.</param>
        /// <returns>Colección de objetos</returns>
        ICollection<TEntity> Query(Expression<Func<TEntity, bool>> where, string orders = "", params Expression<Func<TEntity, object>>[] includes);
        /// <summary>
        /// Consulta que acepta la entidad como criterio de busqueda. La entidad no puede ser nula.
        /// </summary>
        /// <param name="criteria">Entidad con las propiedades que se toman como criterio para realizar la consulta, no debe ser nulo.</param>
        /// <param name="orders">Criterio de ordenamiento.</param>
        /// <param name="includes">Arraeglo de inclusiones.</param>
        /// <returns>Colección de objetos.</returns>
        ICollection<TEntity> Query(TEntity criteria, string orders = "", params Expression<Func<TEntity, object>>[] includes);
        /// <summary>
        /// Consulta que devuelve los resultados de forma página. Acepta acepta una expresión para filtrar la información, puede ser nulo sino se requiere filtrar información.
        /// </summary>
        /// <param name="where">Expresión con condiciones</param>
        /// <param name="totalPages">Páginas totales encontradas.</param>
        /// <param name="totalRows">Número de filas encontradas.</param>
        /// <param name="page">Página que se quiere consultar, inicia en. </param>
        /// <param name="pageSize">Tamaño de página que se requiere.</param>
        /// <param name="orders">Criterio de ordenamiento.</param>
        /// <param name="includes">Includes.</param>
        /// <returns>Colección de objetos.</returns>
        ICollection<TEntity> QueryPage(Expression<Func<TEntity, bool>> where, out int totalPages, out int totalRows, int page = 0, int pageSize = 10, string orders = "", params Expression<Func<TEntity, object>>[] includes);
        /// <summary>
        /// Consulta que devuelve los resultados de forma página. Acepta la entidad como criterio de busqueda por lo tanto no puede ser nulo.
        /// </summary>
        /// <param name="entity">Entidad con las propiedades que se toman como criterio para realizar la consulta, no debe ser nulo.</param>
        /// <param name="totalPages">Páginas totales encontradas.</param>
        /// <param name="totalRows">Número de filas encontradas.</param>
        /// <param name="page">Página que se quiere consultar. </param>
        /// <param name="pageSize">Tamaño de página que se requiere.</param>
        /// <param name="orders">Criterio de ordenamiento.</param>
        /// <param name="includes">Includes.</param>
        /// <returns>Colección de objetos.</returns>
        ICollection<TEntity> QueryPage(TEntity criteria, out int totalPages, out int totalRows, int page = 0, int pageSize = 10, string orders = "", params Expression<Func<TEntity, object>>[] includes);


        /// <summary>
        /// Inserta una entidad.
        /// </summary>
        /// <param name="entity">Entidad que se va  insertar.</param>
        void Insert(TEntity entity);
        /// <summary>
        /// Inserta un conjunto de entidades
        /// </summary>
        /// <param name="entities">Colección de entidades.</param>
        void Insert(IEnumerable<TEntity> entities);
        /// <summary>
        /// Marca una entidad como actualizada.
        /// </summary>
        /// <param name="entity">Entidad a actualizar.</param>
        void Update(TEntity entity);
        /// <summary>
        /// Marca una entidad como eliminada
        /// </summary>
        /// <param name="entity"></param>
        void Delete(TEntity entity);
        /// <summary>
        /// Marca una o varias entidades como eliminado utilizando la condición indicada.
        /// </summary>
        /// <param name="entities"></param>
        void Delete(IEnumerable<TEntity> entities);
    }
}
