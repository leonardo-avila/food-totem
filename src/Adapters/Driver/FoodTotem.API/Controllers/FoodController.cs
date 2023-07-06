using Demand.Application.ViewModels;
using Demand.Domain.Models;
using Demand.Domain.Models.Enums;
using Demand.Domain.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FoodTotem.API.Controllers
{
    [Route("[controller]")]
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

        [HttpPost(Name = "Add new food")]
        public IActionResult AddNewFood(FoodViewModel foodViewModel)
        {
            var food = new Food(
                foodViewModel.Name,
                foodViewModel.Description,
                foodViewModel.ImageUrl,
                foodViewModel.Price,
                (FoodCategoryEnum)Enum.Parse(typeof(FoodCategoryEnum), foodViewModel.Category)
                );
            var validationResult = _foodValidator.Validate(food);

            if (validationResult.IsValid)
            {
                return Ok(_foodService.AddFood(food));
            }
            else
            {
                return BadRequest(validationResult.ToString());
            }
        }

        [HttpPut("{id:Guid}", Name = "Update a food")]
        public async Task<IActionResult> UpdateFood(Guid id, FoodViewModel foodViewModel)
        {
            var food = await _foodService.GetFood(id);
            food.UpdateName(foodViewModel.Name);
            food.UpdateDescription(foodViewModel.Category);
            food.UpdateImageUrl(foodViewModel.ImageUrl);
            food.UpdatePrice(foodViewModel.Price);
            food.UpdateCategory((FoodCategoryEnum)Enum.Parse(typeof(FoodCategoryEnum), foodViewModel.Category));

            var successful = await _foodService.UpdateFood(food);
            if (successful) return Ok(food);
            return BadRequest("Could not update the food. Check the parameters");
        }

        [HttpDelete("{id:Guid}", Name = "Delete a food")]
        public async Task<IActionResult> DeleteFood(Guid id)
        {
            var successful = await _foodService.DeleteFood(id);
            if (successful) return Ok("Food deleted");
            return NotFound("Could not found food with the specified id");
        }

        [HttpGet(Name = "Get foods by category")]
        public async Task<IActionResult> GetFoodsByCategory(string category)
        {
            FoodCategoryEnum categoryEnum;
            var existentCategory = Enum.TryParse(category, false, out categoryEnum);
            if (!existentCategory) return BadRequest("Invalid category");
            return Ok(await _foodService.GetFoodsByCategory(categoryEnum));
        }
    }
}
