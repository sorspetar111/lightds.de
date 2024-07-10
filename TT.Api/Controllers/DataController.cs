using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TT.Lib.DataAccess;
using TT.Lib.Entities;
using TT.Lib.Mvc;

namespace TT.Api.Controllers
{
   

    public class DataFactoryController<TEntity, TService> : ReadWriteController<TEntity, int>
        where TEntity : class, IId, new()
        where TService : IService<TEntity>
    {
        private readonly TService service;

        public DataFactoryController(TService service) : base((IReadWriteService<TEntity>)service)
        {
            this.service = service;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var entities = await service.GetAll();
            return Ok(entities);
        }
    }





    [Route("[controller]")]
    public class DataProductController : DataFactoryController<Product, IProductService>
    {
        private readonly IProductService productService;

        public DataProductController(IProductService productService) : base(productService)
        {
            this.productService = productService;
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await productService.GetAllProducts_SP();
            return Ok(products);
        }
    }

    [Route("[controller]")]
    public class DataPropertiesController : DataFactoryController<Property, IPropertyService>
    {
        private readonly IPropertyService propertyService;

        public DataPropertiesController(IPropertyService propertyService) : base(propertyService)
        {
            this.propertyService = propertyService;
        }

        [HttpGet("properties")]
        public async Task<IActionResult> GetAllProperties()
        {            
            var properties = await this.propertyService.GetAll();
            return Ok(properties);
        }
    }





    /*
   public class DataBaseController : ReadWriteController<Product, int>
   {
       private readonly IProductService productService;
       private readonly IPropertyService propertyService;

       public DataBaseController(IProductService productService, IPropertyService propertyService) : base((IReadWriteService<Product>)productService)
       {
           this.productService = productService;
           this.propertyService = propertyService;
       }


   }

   [Route("[controller]")]
   public class DataProductController : DataBaseController
   {
       private readonly IProductService productService;


       public DataProductController(IProductService productService) : base((IReadWriteService<Product>)productService)
       {
           this.productService = productService;

       }

       [HttpGet("products")]
       public async Task<IActionResult> GetAllProducts()
       {
           var products = await this.productService.GetAllProducts_SP();
           return Ok(products);
       }


   }


   [Route("[controller]")]
   public class DataPropertiesController : DataBaseController
   {

       private readonly IPropertyService propertyService;

       public DataPropertiesController(IProductService productService) : base((IReadWriteService<Property>)propertyService)
       {
           this.propertyService = propertyService;
       }


       [HttpGet("properties")]
       public async Task<IActionResult> GetAllProperties()
       {
           new NotImplementedException("Task 2 is not implement.");
           return null;

           //var properties = await this.propertyService.GetAllProperies_View();
           // return Ok(properties);
       }
   }


   */







}
