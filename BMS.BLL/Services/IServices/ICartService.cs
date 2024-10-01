using BMS.BLL.Models.Requests.Category;
using BMS.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMS.Core.Domains.Entities;
using BMS.BLL.Models.Requests.Cart;
using BMS.BLL.Models.Requests.Basic;

namespace BMS.BLL.Services.IServices
{
    public interface ICartService
    {
        Task<ServiceActionResult> GetAllCartForUser(Guid userId, PagingRequest request);
        Task<ServiceActionResult> GetAllCartItemInCart(Guid cartId, PagingRequest request);

        Task<ServiceActionResult> AddCartDetail(Guid userId, Guid shopId, CartDetailRequest request);
        Task<ServiceActionResult> UpdateCartDetail(Guid userId, Guid shopId, CartDetailRequest request);
        Task<ServiceActionResult> GetCartDetail(Guid cartDetailId);
        Task<ServiceActionResult> DeleteCart(Guid cartId);
        Task<ServiceActionResult> DeleteCartItem(Guid cartItemId);
        Task<ServiceActionResult> GetCartInShopForUser(Guid userId, Guid shopId);
        Task<ServiceActionResult> GetCartByID(Guid cartId);
        Task<ServiceActionResult> ChangeCartToGroup(Guid userId, Guid shopId);
    }
}
