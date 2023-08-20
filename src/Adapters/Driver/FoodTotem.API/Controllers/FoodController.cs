using Demand.Application.ViewModels;
using Demand.Domain.Models;
using Demand.Domain.Models.Enums;
using Demand.Domain.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FoodTotem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly ILogger<FoodController> _logger;
        private readonly IFoodService _foodService;
        private readonly IValidator<Food> _foodValidator;

        public FoodController(ILogger<FoodController> logger,
            IFoodService foodService,
            IValidator<Food> foodValidator)
        {
            _logger = logger;
            _foodService = foodService;
            _foodValidator = foodValidator;
        }

        #region GET Endpoints
        /// <summary>
        /// Get foods by the specified category
        /// </summary>
        /// <param name="category">Represents the category food that should be returned</param>
        /// <returns>Returns the foods with the specified category</returns>
        /// <response code="400">Invalid category.</response>
        [HttpGet(Name = "Get foods by category")]
        public async Task<IActionResult> GetFoodsByCategory(string category)
        {
            var existentCategory = Enum.TryParse(category, false, out FoodCategoryEnum categoryEnum);
            if (!existentCategory) return BadRequest("Invalid category");
            return Ok(await _foodService.GetFoodsByCategory(categoryEnum));
        }
        #endregion

        #region POST Endpoints
        /// <summary>
        /// Add a food with the specified details
        /// </summary>
        /// <param name="foodViewModel"></param>
        /// <returns>Returns 200 when successfully added the food.</returns>
        /// <response code="400">An error occurred. Model validation errors will be prompted when necessary.</response>
        /// <response code="500">Something wrong happened when trying to add food. Could be some error on the database or internet connection.</response>
        [HttpPost(Name = "Add new food")]
        public async Task<IActionResult> AddNewFood(FoodViewModel foodViewModel)
        {
            var food = new Food(
                foodViewModel.Name,
                foodViewModel.Description,
                foodViewModel.ImageUrl,
                foodViewModel.Price,
                foodViewModel.Category
                );
            var validationResult = _foodValidator.Validate(food);

            if (validationResult.IsValid)
            {
                var successful = await _foodService.AddFood(food);
                if (successful) return Ok("Food added successfully.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while adding food.");
            }
            return BadRequest(validationResult.ToString());
        }
        #endregion

        #region PUT Endpoints
        /// <summary>
        /// Update a food with the specified id using the specified details
        /// </summary>
        /// <param name="id">Represents the id of the food that should be updated</param>
        /// <param name="foodViewModel">Represents the food details to update the food on the database</param>
        /// <returns>Returns 200 when successful or 400 with when errors were found.</returns>
        /// <response code="400">Food in invalid format. Model validations are prompted when necessary.</response>
        /// <response code="204">No food with the specified id was found.</response>
        /// <response code="500">Something wrong happened when updating food. Could be internet connection or database error.</response>
        [HttpPut("{id:Guid}", Name = "Update a food")]
        public async Task<IActionResult> UpdateFood(Guid id, FoodViewModel foodViewModel)
        {
            var food = await _foodService.GetFood(id);
            if (food is null) return StatusCode(StatusCodes.Status204NoContent);
            food.UpdateName(foodViewModel.Name);
            food.UpdateDescription(foodViewModel.Description);
            food.UpdateImageUrl(foodViewModel.ImageUrl);
            food.UpdatePrice(foodViewModel.Price);
            food.UpdateCategory(foodViewModel.Category);
            var validationResult = _foodValidator.Validate(food);
            if (validationResult.IsValid)
            {
                var successful = await _foodService.UpdateFood(food);
                if (successful) return Ok(food);
                return StatusCode(StatusCodes.Status500InternalServerError, "Something wrong happened when updating food.");
            }
            
            return BadRequest("Could not update the food. Check the parameters");
        }
        #endregion

        #region DELETE Endpoints
        /// <summary>
        /// Delete a food with the specified id
        /// </summary>
        /// <param name="id">Represents the id of the food that should be deleted</param>
        /// <returns>Returns 200 when successfully deleted the food</returns>
        /// <response code="404">No food with the specified id was found.</response>
        [HttpDelete("{id:Guid}", Name = "Delete a food")]
        public async Task<IActionResult> DeleteFood(Guid id)
        {
            var successful = await _foodService.DeleteFood(id);
            if (successful) return Ok("Food deleted");
            return NotFound("Could not found food with the specified id");
        }
        #endregion
    }
}
