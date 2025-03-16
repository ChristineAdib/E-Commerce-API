using AutoMapper;
using Core;
using Core.Entities;
using Core.Repositories;
using Core.Specifications;
using E_CommerceAPI.DTOs;
using E_CommerceAPI.Errors;
using E_CommerceAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Data.Configuration;

namespace E_CommerceAPI.Controllers
{
    public class ProductsController : APIBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductsController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [CachedAttribute(900)]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductDTO>>> ListingProducts([FromQuery]ProductSpecParams Params)
        {
            var Spec = new ProductWithSpec(Params);
            var Products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(Spec);
            var MappedProducts =  _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDTO>>(Products);
            var CountSpec = new ProductForCountAsync(Params);
            var Count = await _unitOfWork.Repository<Product>().GetCountWithSpecAsync(CountSpec);
            return Ok(new Pagination<ProductDTO>(Params.PageSize, Params.PageIndex, MappedProducts,Count));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductAsync(int id)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
            //var MappedProduct = await _mapper.Map<Product,ProductDTO>(product);
            return Ok(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto Product)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400));

            var product = new CreateProductDto
            {
                Name = Product.Name,
                Description = Product.Description,
                Price = Product.Price,
                CategoryId = Product.CategoryId,
                Stock = Product.Stock,
                ImageUrl = Product.ImageUrl
            };

            var MappedProduct = _mapper.Map<CreateProductDto, Product>(product);
            await _unitOfWork.Repository<Product>().AddAsync(MappedProduct);
            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return Ok(new { message = "Product created failed" });

            return Ok(new { message = "Product created successfully" });
        }

    }
}
