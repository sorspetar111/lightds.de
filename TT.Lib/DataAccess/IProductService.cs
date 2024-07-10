﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Lib.Entities;

namespace TT.Lib.DataAccess
{
    //public interface IKeyable<T>
    //{
    //    T Key { get; }
    //}


    /// <summary>
    /// Defines methods for a read only data access layer repository.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public interface IReadRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Returns all <typeparamref name="TEntity"/>s.
        /// </summary>
        /// <returns>All <typeparamref name="TEntity"/>s.</returns>
        Task<IEnumerable<TEntity>> Get();

        /// <summary>
        /// Get the <typeparamref name="TEntity"/> by the given <paramref name="key"/> parameter.
        /// </summary>
        /// <param name="keyValues">The <typeparamref name="object[]"/>.</param>
        /// <returns>The <typeparamref name="TEntity"/> for the given <paramref name="key"/></returns>
        Task<TEntity> GetByKey(params object[] keyValues);

        /// <summary>
        /// Returns <see cref="IQueryable{TEntity}"/> for query the <typeparamref name="TEntity"/>s.
        /// </summary>
        /// <returns>The <typeparamref name="TEntity"/>s.</returns>
        IQueryable<TEntity> Query();
    }

    /// <summary>
    /// Defines methods for a read write data access layer repository.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public interface IReadWriteRepository<TEntity> : IReadRepository<TEntity>
        where TEntity : class, IId
    {
        /// <summary>
        /// Adds an <typeparamref name="TEntity"/> to the <see cref="IReadWriteRepository{TEntity}"/>.
        /// </summary>
        /// <param name="entity">The <typeparamref name="TEntity"/> to  add to the <see cref="IReadWriteRepository{TEntity}"/></param>
        /// <returns>The added <typeparamref name="TEntity"/>.</returns>
        Task<TEntity> Add(TEntity entity);

        /// <summary>
        /// Adds <typeparamref name="TEntity"/>s to the <see cref="IReadWriteRepository{TEntity}"/>.
        /// </summary>
        /// <param name="entities">The <typeparamref name="TEntity"/>s to add to the <see cref="IReadWriteRepository{TEntity}"/>.</param>
        /// <returns>The added <typeparamref name="TEntity"/>s.</returns>
        Task<IEnumerable<TEntity>> AddRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Update an <typeparamref name="TEntity"/> to the <see cref="IReadWriteRepository{TEntity}"/>.
        /// </summary>
        /// <param name="entity">The <typeparamref name="TEntity"/> to update to the <see cref="IReadWriteRepository{TEntity}"/>.</param>
        /// <returns>The updated <typeparamref name="TEntity"/></returns>
        Task Update(TEntity entity);

        //void ClearChangeTracker();

        //Task Clear();

        /// <summary>
        /// Remove a <typeparamref name="TEntity"/> from the <see cref="IReadWriteRepository{TEntity}"/>.
        /// </summary>
        /// <param name="entity">The <typeparamref name="TEntity"/> to remove from the <see cref="IReadWriteRepository{TEntity}"/>.</param>
        Task Remove(TEntity entity);

        /// <summary>
        /// Remos <typeparamref name="TEntity"/>s from the <see cref="IReadWriteRepository{TEntity}"/>.
        /// </summary>
        /// <param name="entities">The <typeparamref name="TEntity"/>s to remove from the <see cref="IReadWriteRepository{TEntity}"/>.</param>
        Task RemoveRange(IEnumerable<TEntity> entities);
    }

    /// <summary>
    /// Defines methods for a read only business logic layer service.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public interface IReadService<TEntity>
    {
        /// <summary>
        /// Returns all <typeparamref name="TEntity"/>s.
        /// </summary>
        /// <returns>All <typeparamref name="TEntity"/>s.</returns>
        Task<IEnumerable<TEntity>> Get();

        /// <summary>
        /// Get the <typeparamref name="TEntity"/> by the given <paramref name="key"/> parameter.
        /// </summary>
        /// <param name="keyValues">The keys.</param>
        /// <returns>The <typeparamref name="TEntity"/> for the given <paramref name="key"/></returns>
        Task<TEntity> GetByKey(params object[] keyValues);
    }

    /// <summary>
    /// Defines methods for a read write business logic layer service.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public interface IReadWriteService<TEntity> : IReadService<TEntity>
    {
        Task<IQueryable<TEntity>> Query();

        /// <summary>
        /// Adds an <typeparamref name="TEntity"/> to the <see cref="IReadWriteService{TEntity}"/>.
        /// </summary>
        /// <param name="entity">The <typeparamref name="TEntity"/> to  add to the <see cref="IReadWriteService{TEntity}"/></param>
        /// <returns>The added <typeparamref name="TEntity"/>.</returns>
        Task<TEntity> Add(TEntity entity);

        /// <summary>
        /// Adds <typeparamref name="TEntity"/>s to the <see cref="IReadWriteService{TEntity}"/>.
        /// </summary>
        /// <param name="entities">The <typeparamref name="TEntity"/>s to add to the <see cref="IReadWriteService{TEntity}"/>.</param>
        /// <returns>The added <typeparamref name="TEntity"/>s.</returns>
        Task<IEnumerable<TEntity>> AddRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Update an <typeparamref name="TEntity"/> to the <see cref="IReadWriteService{TEntity}"/>.
        /// </summary>
        /// <param name="entity">The <typeparamref name="TEntity"/> to update to the <see cref="IReadWriteService{TEntity}"/>.</param>
        /// <returns>The updated <typeparamref name="TEntity"/></returns>
        Task Update(TEntity entity);

        /// <summary>
        /// Remove a <typeparamref name="TEntity"/> from the <see cref="IReadWriteService{TEntity}"/>.
        /// </summary>
        /// <param name="entity">The <typeparamref name="TEntity"/> to remove from the <see cref="IReadWriteService{TEntity}"/>.</param>
        Task Remove(TEntity entity);

        /// <summary>
        /// Remove a <typeparamref name="TEntity"/> from the <see cref="IReadWriteService{TEntity}"/> by the given key / keys/>.
        /// </summary>
        Task RemoveByKey(params object[] keyValues);
    }

    /// <summary>
    /// Provides an <see cref="IReadRepository{TEntity, TKey}"/> implementation with EntityFramework data access.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TContext">The context type.</typeparam>
    public abstract class EntityFrameworkReadRepository<TEntity, TContext> : IReadRepository<TEntity>
        where TEntity : class, IId
        where TContext : DbContext
    {
        /// <summary>
        /// Returns the <see cref="DbSet{TEntity}"/> by the given <typeparamref name="TContext"/>.
        /// </summary>
        /// <param name="context">The Entity Framework context.</param>
        /// <returns>The <see cref="DbSet{TEntity}"/>.</returns>
        protected DbSet<TEntity> GetEntities(TContext context) => context.Set<TEntity>();

        /// <summary>
        /// Returns an initalized <typeparamref name="TContext"/>.
        /// </summary>
        /// <returns>The initalized <typeparamref name="TContext"/>.</returns>
        protected abstract TContext GetDbContext();

        /// <inheritdoc/>
        public virtual async Task<IEnumerable<TEntity>> Get() => await this.GetEntities(this.GetDbContext()).ToListAsync();

        /// <inheritdoc/>
        public virtual async Task<TEntity> GetByKey(params object[] keyValues) => await this.GetEntities(this.GetDbContext()).FindAsync(keyValues);

        /// <inheritdoc/>
        public virtual IQueryable<TEntity> Query() => this.GetEntities(this.GetDbContext()).AsQueryable();
    }

    /// <summary>
    /// Provides an <see cref="IReadWriteRepository{TEntity}"/> implementation with EntityFramework data access.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TContext">The context type.</typeparam>
    public abstract class EntityFrameworkReadWriteRepository<TEntity, TContext> : EntityFrameworkReadRepository<TEntity, TContext>, IReadWriteRepository<TEntity>
        where TEntity : class, IId
        where TContext : DbContext
    {
        /// <inheritdoc/>
        public virtual async Task<TEntity> Add(TEntity entity)
        {
            var dbContext = this.GetDbContext();
            this.GetEntities(dbContext).Add(entity);

            await dbContext.SaveChangesAsync();

            return entity;
        }

        /// <inheritdoc/>
        public virtual async Task<IEnumerable<TEntity>> AddRange(IEnumerable<TEntity> entities)
        {
            var dbContext = this.GetDbContext();
            this.GetEntities(dbContext).AddRange(entities);
            await dbContext.SaveChangesAsync();

            return entities;
        }

        /// <inheritdoc/>
        public virtual async Task Clear()
        {
            var dbContext = this.GetDbContext();
            var dbSet = this.GetEntities(dbContext);
            dbSet.RemoveRange(dbSet);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public virtual async Task Remove(TEntity entity)
        {
            var dbContext = this.GetDbContext();
            this.GetEntities(dbContext).Remove(entity);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public virtual async Task RemoveRange(IEnumerable<TEntity> entities)
        {
            var dbContext = this.GetDbContext();
            this.GetEntities(dbContext).RemoveRange(entities);
            await dbContext.SaveChangesAsync();
        }

        public virtual void ClearChangeTracker()
        {
            this.GetDbContext().ChangeTracker.Clear();
        }

        /// <inheritdoc/>
        public virtual async Task Update(TEntity entity)
        {
            var dbContext = this.GetDbContext();
            dbContext.Attach(entity).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
        }
    }

    public interface IService<TEntity>
    {
        Task<List<TEntity>> GetAll();
    }

    public interface IPropertyService : IReadWriteService<TT.Lib.Entities.Property>, IService<TT.Lib.Entities.Property>
    {
        Task<List<TT.Lib.Entities.Property>> GetAll();

        Task<List<TT.Lib.Entities.Property>> GetAllProperty_View();        

    }

    public interface IProductPropertyService : IReadWriteService<ProductProperty> { }

    public interface IProductService : IReadWriteService<Product>, IService<Product>
    {
        Task<List<Product>> GetAllProducts_EF();
        Task<List<Product>> GetAllProducts_SP();
        Task<List<ProductDto>> GetAllProductDTOs_SP();
    }

    // TODO: new service??? At the moment is nor requred
    // public interface IDataService : IReadWriteService<Data> { }
    // public interface IDataRepository<T> : IReadWriteRepository<T> where T : class, IId { }

    public interface IBrandService : IReadWriteService<Brand> { }

    public interface IProductRepository<T> : IReadWriteRepository<T> where T : class, IId { }

    public interface IPropertyRepository<T> : IReadWriteRepository<T> where T : class, IId { }

    public interface IProductPropertyRepository<T> : IReadWriteRepository<T> where T : class, IId { }

    public interface IBrandRepository<T> : IReadWriteRepository<T> where T : class, IId { }

    public class ReadWriteService<T, TKey> : IReadWriteService<T>
        where T : class, IId
    {
        IReadWriteRepository<T> repo;

        public ReadWriteService(IReadWriteRepository<T> repo)
        {
            this.repo = repo;
        }

        public async Task<IQueryable<T>> Query() => this.repo.Query();

        public async Task<IEnumerable<T>> Get() => await this.repo.Get();

        public async Task Remove(T entity) => await this.repo.Remove(entity);

        public async Task<T> Add(T entity) => await this.repo.Add(entity);

        public async Task Update(T entity) => await this.repo.Update(entity);

        public async Task<IEnumerable<T>> AddRange(IEnumerable<T> entities) => await this.repo.AddRange(entities);

        public async Task<T> GetByKey(params object[] keyValues)
        {
            return await this.repo.Query()
                .FirstOrDefaultAsync(x => x.Id == int.Parse(keyValues[0].ToString()));
        }

        public async Task RemoveByKey(params object[] keyValues)
        {
            var existing = await this.repo.GetByKey(keyValues);

            if (existing == null)
            {
                throw new ArgumentNullException("Product does not exist!");
            }

            await this.repo.Remove(existing);
        }
    }


    public class ReadWriteRepository<TEntity, TKey>
        where TEntity : class
        where TKey : IComparable<TKey>
    {
        protected readonly DbContext dbContext;

        public ReadWriteRepository(TTDbContext dbContext) : base()
        {
            this.dbContext = dbContext;
        }

        protected DbContext GetDbContext() => this.dbContext;

        /// <summary>
        /// Returns the <see cref="DbSet{TEntity}"/> by the given <typeparamref name="TContext"/>.
        /// </summary>
        /// <param name="context">The Entity Framework context.</param>
        /// <returns>The <see cref="DbSet{TEntity}"/>.</returns>
        protected DbSet<TEntity> GetEntities() => this.dbContext.Set<TEntity>();

        public IQueryable<TEntity> Query() => this.GetEntities().AsQueryable();

        public async Task<IEnumerable<TEntity>> Get() => await this.GetEntities().ToListAsync();

        public async Task<TEntity> GetByKey(params object[] keyValues) => await this.GetEntities().FindAsync(keyValues);

        /// <inheritdoc/>
        public async Task Remove(TEntity entity)
        {
            var dbContext = this.GetDbContext();
            this.GetEntities().Remove(entity);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task Update(TEntity entity)
        {
            var dbContext = this.GetDbContext();
            dbContext.Attach(entity).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task RemoveRange(IEnumerable<TEntity> entities)
        {
            var dbContext = this.GetDbContext();
            this.GetEntities().RemoveRange(entities);
            await dbContext.SaveChangesAsync();
        }

        public async Task<TEntity> Add(TEntity entity)
        {
            var dbContext = this.GetDbContext();

            this.GetEntities().Add(entity);

            await dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<TEntity>> AddRange(IEnumerable<TEntity> entities)
        {
            var dbContext = this.GetDbContext();

            this.GetEntities().AddRange(entities);

            await dbContext.SaveChangesAsync();

            return entities;
        }
    }

    public class BrandRepository : ReadWriteRepository<Brand, int>, IBrandRepository<Brand>
    {
        public BrandRepository(TTDbContext dbContext) : base(dbContext) { }
    }

    public class ProductRepository : ReadWriteRepository<Product, int>, IProductRepository<Product>
    {
        public ProductRepository(TTDbContext dbContext) : base(dbContext) { }
    }

    public class PropertyRepository : ReadWriteRepository<TT.Lib.Entities.Property, int>, IPropertyRepository<TT.Lib.Entities.Property>
    {
        public PropertyRepository(TTDbContext dbContext) : base(dbContext) { }
    }

    public class ProductPropertyRepository : ReadWriteRepository<ProductProperty, int>, IProductPropertyRepository<ProductProperty>
    {
        public ProductPropertyRepository(TTDbContext dbContext) : base(dbContext) { }
    }


    public class BrandService : ReadWriteService<Brand, int>, IBrandService
    {
        private readonly TTDbContext dbContext;
        private readonly IBrandRepository<Brand> mainRepository;

        public BrandService(TTDbContext dbContext, IBrandRepository<Brand> mainRepository) : base(mainRepository)
        {
            this.dbContext = dbContext;
            this.mainRepository = mainRepository ?? throw new ArgumentNullException(nameof(mainRepository));
        }
    }

    public class ProductService : ReadWriteService<Product, int>, IProductService
    {
        private readonly TTDbContext dbContext;
        private readonly IProductRepository<Product> mainRepository;
        private readonly string _connectionString;

        public ProductService(TTDbContext dbContext, IProductRepository<Product> mainRepository) : base(mainRepository)
        {
            this.dbContext = dbContext;
            this.mainRepository = mainRepository ?? throw new ArgumentNullException(nameof(mainRepository));
        }

        /// <summary>
        /// This is made due to task condition rerquretments to not use EF. 
        /// EF can call stored procedure. This mean, this aprouch is bad and not best practice
        /// For that reasone I am going to create a new overload constructor and inject configuration        
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mainRepository"></param>
        /// <param name="configuration"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ProductService(TTDbContext dbContext, IProductRepository<Product> mainRepository, IConfiguration configuration) : base(mainRepository)
        {
            this.dbContext = dbContext;
            this.mainRepository = mainRepository ?? throw new ArgumentNullException(nameof(mainRepository));
            _connectionString = configuration.GetConnectionString("TTDbContext");
        }

        public async Task<List<Product>> GetAll()
        {
            return await GetAllProducts_SP();
        }

        public async Task<List<Product>> GetAllProducts_EF()
        {
            new NotImplementedException("Calling SP from EF is not requred. Must update context ->  lightds.de TT.Lib TTDbContext.cs");
            return null;
        }

        // This is not best practice. Must use EF direct call of SP. Still use Product model from db context
        public async Task<List<Product>> GetAllProducts_SP()
        {
            var products = new Dictionary<int, Product>();

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("GetAllDataProducts", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var productId = reader.GetInt32(reader.GetOrdinal("ProductId"));
                            if (!products.TryGetValue(productId, out var product))
                            {
                                product = new Product
                                {
                                    Id = productId,
                                    Name = reader.GetString(reader.GetOrdinal("ProductName")),
                                    BrandId = reader.GetInt32(reader.GetOrdinal("BrandId")),
                                    Brand = new Brand
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("BrandId")),
                                        Name = reader.GetString(reader.GetOrdinal("BrandName"))
                                    },
                                    Properties = new List<ProductProperty>()
                                };
                                products[productId] = product;
                            }

                            if (!reader.IsDBNull(reader.GetOrdinal("PropertyName")) && !reader.IsDBNull(reader.GetOrdinal("PropertyValue")))
                            {
                                var property = new ProductProperty
                                {
                                    ProductId = productId,
                                    Property = new TT.Lib.Entities.Property
                                    {
                                        Name = reader.GetString(reader.GetOrdinal("PropertyName"))
                                    },
                                    Value = reader.GetString(reader.GetOrdinal("PropertyValue"))
                                };
                                product.Properties.Add(property);
                            }
                        }
                    }
                }
            }

            return new List<Product>(products.Values);
        }

        // This is not best practice. Must use EF direct call of SP
        public async Task<List<ProductDto>> GetAllProductDTOs_SP()
        {
            var products = new List<ProductDto>();

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("GetAllDataProducts", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var product = new ProductDto
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                Name = reader.GetString(reader.GetOrdinal("ProductName")),
                                BrandName = reader.GetString(reader.GetOrdinal("BrandName")),
                                PropertyName = reader.IsDBNull(reader.GetOrdinal("PropertyName")) ? null : reader.GetString(reader.GetOrdinal("PropertyName")),
                                PropertyValue = reader.IsDBNull(reader.GetOrdinal("PropertyValue")) ? null : reader.GetString(reader.GetOrdinal("PropertyValue"))
                            };
                            products.Add(product);
                        }
                    }
                }
            }

            return products;
        }


    }

    public class ProductPropertyService : ReadWriteService<ProductProperty, int>, IProductPropertyService
    {
        private readonly TTDbContext dbContext;
        private readonly IProductPropertyRepository<ProductProperty> mainRepository;

        public ProductPropertyService(TTDbContext dbContext, IProductPropertyRepository<ProductProperty> mainRepository) : base(mainRepository)
        {
            this.dbContext = dbContext;
            this.mainRepository = mainRepository ?? throw new ArgumentNullException(nameof(mainRepository));
        }
    }

    public class PropertyService : ReadWriteService<TT.Lib.Entities.Property, int>, IPropertyService
    {
        private readonly TTDbContext dbContext;
        private readonly IPropertyRepository<TT.Lib.Entities.Property> mainRepository;

        public PropertyService(TTDbContext dbContext, IPropertyRepository<TT.Lib.Entities.Property> mainRepository) : base(mainRepository)
        {
            this.dbContext = dbContext;
            this.mainRepository = mainRepository ?? throw new ArgumentNullException(nameof(mainRepository));
        }




        public async Task<List<TT.Lib.Entities.Property>> GetAll()
        {
            return await GetAllProperty_View();
        }

        

        /// <summary>
        ///  // call T-SQL view by name  [AllProperties_View]
        /// </summary>
        /// <returns></returns>
        public async Task<List<TT.Lib.Entities.Property>> GetAllProperty_View()
        {
            var properties = new List<TT.Lib.Entities.Property>();

            try
            {
                // Prepare the SQL query to select from the view
                var sql = "SELECT PropertyId, PropertyName, PropertyValue FROM AllProperties_View";

                // Execute the SQL query using DbContext
                using (var command = dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = sql;
                    command.CommandType = CommandType.Text;

                    await dbContext.Database.OpenConnectionAsync();

                    using (var result = await command.ExecuteReaderAsync())
                    {
                        while (await result.ReadAsync())
                        {
                            var property = new TT.Lib.Entities.Property
                            {
                                Id = result.GetInt32(0), // Assuming PropertyId is the first column
                                Name = result.GetString(1), // Assuming PropertyName is the second column
                                Value = result.GetString(2) // Assuming PropertyValue is the third column
                            };
                            properties.Add(property);
                        }
                    }
                }
            }
            finally
            {
                dbContext.Database.CloseConnection();
            }

            return properties;
        }


        //public async Task<List<Product>> IService<Product>.GetAll()
        //{
        //    return await GetAllProperty_View();
        //}
    }
}
