using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GenericRepository
{
    /// <summary>
    /// Generic repository to be used with EntityFrameworkCore
    /// which implements CRUD operations with different options.
    /// </summary>
    /// <remarks>
    /// To commit changes on the database you have to call the SaveChanges method
    /// of your DbContext by yourself.
    /// </remarks>
    /// <typeparam name="TEntity">The entity of your application dbContext.</typeparam>
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Gets a IEnumerable collection of the entity.
        /// </summary>
        /// <remarks>
        /// The query will be made to the database once you loop through collection
        /// or executes LINQ ToList method.
        /// </remarks>
        /// <param name="where">A lambda expression to filter the query.</param>
        /// <param name="orderBy">A lambda expression to order the query by the specified propertie.</param>
        /// <param name="includeProperties">The Foreign Key properties of the entity to be included in the query separated by commas (,).</param>
        /// <param name="descendingOrder">Specifies if the query should be in descending order.</param>
        /// <returns>A collection of <typeparamref name="TEntity"/>.</returns>
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> where = null,
                                    Func<TEntity, object> orderBy = null,
                                    string includeProperties = null,
                                    bool descendingOrder = false);

        /// <summary>
        /// Gets an instance of <typeparamref name="TEntity"/> by the specified filter.
        /// </summary>
        /// <remarks>
        /// Returns a null result if no entity meets the specified filter.
        /// </remarks>
        /// <param name="where">A lambda expression to filter the query.</param>
        /// <param name="includeProperties">The Foreign Key properties of the entity to be included in the query separated by commas (,).</param>
        /// <returns>A new instance of <typeparamref name="TEntity"/>.</returns>
        TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> where,
                                  string includeProperties = null);

        /// <summary>
        /// Asynchronously gets an instance <typeparamref name="TEntity"/> by the specified filter.
        /// </summary>
        /// <remarks>
        /// Returns a null result if no entity meets the specified filter.
        /// </remarks>
        /// <param name="where">A lambda expression to filter the query.</param>
        /// <param name="includeProperties">The Foreign Key properties of the entity to be included in the query separated by commas (,).</param>
        /// <returns>A new instance of <typeparamref name="TEntity"/>.</returns>
        Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> where = null,
                                             string includeProperties = null);

        /// <summary>
        /// Asynchronously gets an instance of <typeparamref name="TEntity"/> by the specified primary key.
        /// </summary>
        /// <remarks>
        /// Returns a null result if no entity meets the specified primary key.
        /// </remarks>
        /// <param name="pk">Your entity primary key.</param>
        /// <returns>A new instance of <typeparamref name="TEntity"/>.</returns>
        Task<TEntity> GetByPrimaryKeyAsync(object pk);

        /// <summary>
        /// Gets an instance of <typeparamref name="TEntity"/> by the specified primary key.
        /// </summary>
        /// <remarks>
        /// Returns a null result if no entity meets the specified primary key.
        /// </remarks>
        /// <param name="pk">Your entity primary key.</param>
        /// <returns>A new instance of <typeparamref name="TEntity"/>.</returns>
        TEntity GetByPrimaryKey(object pk);

        /// <summary>
        /// Begins tracking to the parameterized entity and will be inserted
        /// when SaveChanges method gets called.
        /// </summary>
        /// <remarks>
        /// This method is async only to allow special value generators,
        /// such as the one used by 'Microsoft.EntityFrameworkCore.Metadata.SqlServerValueGenerationStrategy.SequenceHiLo',
        /// to access the database asynchronously. For all other cases the non async method should be used.
        /// </remarks>
        /// <param name="entity">Your entity instance.</param>
        /// <returns></returns>
        Task CreateAsync(TEntity entity);

        /// <summary>
        /// Begins tracking to the parameterized entities and will be inserted
        /// when SaveChanges method gets called.
        /// </summary>
        /// <remarks>
        /// This method is async only to allow special value generators,
        /// such as the one used by 'Microsoft.EntityFrameworkCore.Metadata.SqlServerValueGenerationStrategy.SequenceHiLo',
        /// to access the database asynchronously. For all other cases the non async method should be used.
        /// </remarks>
        /// <param name="entities">Your entities instances array.</param>
        /// <returns></returns>
        Task CreateAsync(TEntity[] entities);

        /// <summary>
        /// Begins tracking to the parameterized entity and will be inserted
        /// when SaveChanges method gets called.
        /// </summary>
        /// <param name="entity">Your entity instance.</param>
        /// <returns></returns>
        void Create(TEntity entity);

        /// <summary>
        /// Begins tracking to the parameterized entities and will be inserted
        /// when SaveChanges method gets called.
        /// </summary>
        /// <param name="entities">Your entities instances array.</param>
        /// <returns></returns>
        void Create(TEntity[] entities);

        /// <summary>
        /// Begins tracking to the parameterized entity and will be updated
        /// when SaveChanges method gets called.
        /// </summary>
        /// <param name="entity">Your entity instance.</param>
        /// <returns></returns>
        void Update(TEntity entity);

        /// <summary>
        /// Begins tracking to the parameterized entities and will be updated
        /// when SaveChanges method gets called.
        /// </summary>
        /// <param name="entities">Your entities instances array.</param>
        /// <returns></returns>
        void Update(TEntity[] entities);

        /// <summary>
        /// Begins tracking to the parameterized entity and will be deleted
        /// when SaveChanges method gets called.
        /// </summary>
        /// <param name="entity">Your entity instance.</param>
        /// <returns></returns>
        void Delete(TEntity entity);

        /// <summary>
        /// Begins tracking to the parameterized entities and will be deleted
        /// when SaveChanges method gets called.
        /// </summary>
        /// <param name="entities">Your entities instances array.</param>
        /// <returns></returns>
        void Delete(TEntity[] entities);
    }
}
