using FoodTotem.Domain.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FoodTotem.Gateways.Catalog.ViewModels;
using FoodTotem.Gateways.Catalog.Services;

namespace FoodTotem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly ILogger<CatalogController> _logger;
        private readonly ICatalogService _catalogService;

        public CatalogController(ILogger<CatalogController> logger, ICatalogService catalogService)
        {
            _logger = logger;
            _catalogService = catalogService;
        }
        #region GET Endpoints
        /// <summary>
        /// Get foods by the specified category. Categories: Meal, SideDish, Drink, Dessert
        /// </summary>
        /// <param name="category">Represents the category food that should be returned</param>
        /// <returns>Returns the foods with the specified category</returns>
        /// <response code="400">Invalid category.</response>
        [HttpGet(Name = "Get foods by category")]
        public async Task<ActionResult<IEnumerable<CatalogViewModel>>> GetFoodsByCategory(string category)
        {
            try
            {
                var foods = await _catalogService.GetCatalogByCategory(category);
                if (foods is null)
                    return NoContent();
                return Ok(foods);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving foods.");
            }
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
        [Authorize("Bearer")]
        public async Task<ActionResult<CatalogViewModel>> AddNewFood(CreateCatalogViewModel foodViewModel)
        {
            try
            { 
                return Ok(await _catalogService.CreateCatalogItem(foodViewModel));
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while adding food.");
            }
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
        [Authorize("Bearer")]
        public async Task<ActionResult<CatalogViewModel>> UpdateFood(Guid id, CreateCatalogViewModel foodViewModel)
        {
            try
            {
                return Ok(await _catalogService.UpdateCatalogItem(id, foodViewModel));
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something wrong happened when updating food.");
            }
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
        [Authorize("Bearer")]
        public async Task<IActionResult> DeleteFood(Guid id)
        {
            try
            {
                await _catalogService.DeleteCatalogItem(id);

                return Ok("Food deleted");
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting food");
            }
        }
        #endregion
    }
}
