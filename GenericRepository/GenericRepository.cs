using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> _dbEntity;

        /// <summary> 
        /// Creates an instance of a generic repository to be used with EntityFrameworkCore
        /// which implements CRUD operations with different options.
        /// </summary>
        /// <remarks>
        /// To commit changes on the database you have to call the SaveChanges method
        /// of your DbContext by yourself.
        /// </remarks>
        /// <param name="context">Your application dbContext instance.</param>
        public GenericRepository(DbContext context)
        {
            _dbEntity = context.Set<TEntity>();
        }

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
        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> where = null,
                                           Func<TEntity, object> orderBy = null,
                                           string includeProperties = null,
                                           bool descendingOrder = false)
        {
            IQueryable<TEntity> query = _dbEntity;

            if (where != null)
            {
                query = query.Where(where);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                if (descendingOrder)
                    return query.OrderByDescending(orderBy).AsEnumerable();
                else
                    return query.OrderByDescending(orderBy).AsEnumerable();
            }

            if (descendingOrder)
                return query.OrderByDescending(x => x).AsEnumerable();
            else
                return query.AsEnumerable();
        }

        /// <summary>
        /// Gets an instance of <typeparamref name="TEntity"/> by the specified filter.
        /// </summary>
        /// <remarks>
        /// Returns a null result if no entity meets the specified filter.
        /// </remarks>
        /// <param name="where">A lambda expression to filter the query.</param>
        /// <param name="includeProperties">The Foreign Key properties of the entity to be included in the query separated by commas (,).</param>
        /// <returns>A new instance of <typeparamref name="TEntity"/>.</returns>
        public TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> where,
                                         string includeProperties = null)
        {
            IQueryable<TEntity> query = _dbEntity;

            if (where == null)
            {
                throw new ArgumentNullException(nameof(where));
            }

            query = query.Where(where);

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return query.FirstOrDefault();
        }

        /// <summary>
        /// Asynchronously gets an instance <typeparamref name="TEntity"/> by the specified filter.
        /// </summary>
        /// <remarks>
        /// Returns a null result if no entity meets the specified filter.
        /// </remarks>
        /// <param name="where">A lambda expression to filter the query.</param>
        /// <param name="includeProperties">The Foreign Key properties of the entity to be included in the query separated by commas (,).</param>
        /// <returns>A new instance of <typeparamref name="TEntity"/>.</returns>
        public async Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> where = null,
                                                          string includeProperties = null)
        {
            IQueryable<TEntity> query = _dbEntity;

            if (where == null)
            {
                throw new ArgumentNullException(nameof(where));
            }

            query = query.Where(where);

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Asynchronously gets an instance of <typeparamref name="TEntity"/> by the specified primary key.
        /// </summary>
        /// <remarks>
        /// Returns a null result if no entity meets the specified primary key.
        /// </remarks>
        /// <param name="pk">Your entity primary key.</param>
        /// <returns>A new instance of <typeparamref name="TEntity"/>.</returns>
        public async Task<TEntity> GetByPrimaryKeyAsync(object pk)
        {
            return await _dbEntity.FindAsync(pk);
        }

        /// <summary>
        /// Gets an instance of <typeparamref name="TEntity"/> by the specified primary key.
        /// </summary>
        /// <remarks>
        /// Returns a null result if no entity meets the specified primary key.
        /// </remarks>
        /// <param name="pk">Your entity primary key.</param>
        /// <returns>A new instance of <typeparamref name="TEntity"/>.</returns>
        public TEntity GetByPrimaryKey(object pk)
        {
            return _dbEntity.Find(pk);
        }

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
        public async Task CreateAsync(TEntity entity)
        {
            await _dbEntity.AddAsync(entity);
        }

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
        public async Task CreateAsync(TEntity[] entities)
        {
            await _dbEntity.AddRangeAsync(entities);
        }

        /// <summary>
        /// Begins tracking to the parameterized entity and will be inserted
        /// when SaveChanges method gets called.
        /// </summary>
        /// <param name="entity">Your entity instance.</param>
        /// <returns></returns>
        public void Create(TEntity entity)
        {
            _dbEntity.Add(entity);
        }

        /// <summary>
        /// Begins tracking to the parameterized entities and will be inserted
        /// when SaveChanges method gets called.
        /// </summary>
        /// <param name="entities">Your entities instances array.</param>
        /// <returns></returns>
        public void Create(TEntity[] entities)
        {
            _dbEntity.AddRange(entities);
        }

        /// <summary>
        /// Begins tracking to the parameterized entity and will be updated
        /// when SaveChanges method gets called.
        /// </summary>
        /// <param name="entity">Your entity instance.</param>
        /// <returns></returns>
        public void Update(TEntity entity)
        {
            _dbEntity.Update(entity);
        }

        /// <summary>
        /// Begins tracking to the parameterized entities and will be updated
        /// when SaveChanges method gets called.
        /// </summary>
        /// <param name="entities">Your entities instances array.</param>
        /// <returns></returns>
        public void Update(TEntity[] entities)
        {
            _dbEntity.UpdateRange(entities);
        }

        /// <summary>
        /// Begins tracking to the parameterized entity and will be deleted
        /// when SaveChanges method gets called.
        /// </summary>
        /// <param name="entity">Your entity instance.</param>
        /// <returns></returns>
        public void Delete(TEntity entity)
        {
            _dbEntity.Remove(entity);
        }

        /// <summary>
        /// Begins tracking to the parameterized entities and will be deleted
        /// when SaveChanges method gets called.
        /// </summary>
        /// <param name="entities">Your entities instances array.</param>
        /// <returns></returns>
        public void Delete(TEntity[] entities)
        {
            _dbEntity.RemoveRange(entities);
        }
    }
}
