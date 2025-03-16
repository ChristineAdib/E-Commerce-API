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
using System.Security.Claims;

namespace E_CommerceAPI.Controllers
{

    public class CategoriesController : APIBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoriesController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [CachedAttribute(900)]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CategoryDTO>>> ListingCategories()
        {
            var Categories = await _unitOfWork.Repository<Category>().GetAllAsync();
            var MappedCategories = _mapper.Map<IReadOnlyList<Category>, IReadOnlyList<CategoryDTO>>(Categories);
            return Ok(MappedCategories);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] Category Category)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400));

            var category = new Category
            {
                Name = Category.Name,
                Description = Category.Description,
                ParentId = Category.ParentId
            };

            await _unitOfWork.Repository<Category>().AddAsync(category);
            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return Ok(new { message = "Category created failed" });

            return Ok(new { message = "Category created successfully" });
        }
    }
}
