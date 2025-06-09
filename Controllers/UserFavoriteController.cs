using EcommerceAPI.DTOs.UserFavoriteDTOs;
using EcommerceAPI.Services.UserManagement.UserFavoriteManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserFavoriteController : ControllerBase
    {
        private readonly IUserFavorite userFavorite;

        public UserFavoriteController(IUserFavorite userFavorite)
        {
            this.userFavorite = userFavorite;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<FavoriteResponseDTO>>> GetFavoritesAsync()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdClaim, out int userId) || userId <= 0)
            {
                return Unauthorized("User is not authorized");
            }
            var GetFavoriteResponse = await userFavorite.GetFavoritesAsync(userId);
            
            return Ok(GetFavoriteResponse.Data);
        }

        [HttpPost("AddtoFavorite/{ProductId}")]
        [Authorize]
        public async Task<ActionResult<List<FavoriteResponseDTO>>> AddToFavoritesAsync(int ProductId)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdClaim, out int userId) || userId <= 0)
            {
                return Unauthorized("User is not authorized");
            }
            var AddtoFavoriteResponse = await userFavorite.AddToFavoritesAsync(userId, ProductId);

            if(AddtoFavoriteResponse.Success == false)
            {
                return BadRequest(AddtoFavoriteResponse.Message);
            }
            return Ok(AddtoFavoriteResponse.Data);

        }

        [HttpDelete("RemoveFavorite/{ProductId}")]
        [Authorize]
        public async Task<ActionResult<List<FavoriteResponseDTO>>> RemoveFromFavoritesAsync(int ProductId)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdClaim, out int userId) || userId <= 0)
            {
                return Unauthorized("User is not authorized");
            }
            var RemoveFavoriteResponse = await userFavorite.RemoveFromFavoritesAsync(userId, ProductId);

            if(RemoveFavoriteResponse.Success == false)
            {
                return BadRequest(RemoveFavoriteResponse.Message);
            }
            return Ok(RemoveFavoriteResponse.Data);
        }
    }
}
