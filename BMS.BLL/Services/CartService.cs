using AutoMapper;
using Azure.Core;
using BMS.BLL.Models;
using BMS.BLL.Models.Requests.Basic;
using BMS.BLL.Models.Requests.Cart;
using BMS.BLL.Models.Responses.Cart;
using BMS.BLL.Models.Responses.Users;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.Core.Domains.Entities;
using BMS.Core.Helpers;
using BMS.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services
{
    public class CartService : BaseService, ICartService
    {
        private readonly UserManager<User> _userManager;
        public CartService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager) : base(unitOfWork, mapper)
        {
            _userManager = userManager;
        }

        public async Task<ServiceActionResult> AddCartDetail(Guid userId, Guid shopId, CartDetailRequest request)
        {
            Expression<Func<Cart, bool>> filter = cart => (cart.CustomerId == userId && cart.ShopId == shopId);
            var cart = await _unitOfWork.CartRepository.FindAsyncAsQueryable(filter);
            if (cart == null || cart.FirstOrDefault() == null)
            {
                Cart newCart = new Cart();
                newCart.CustomerId = userId;
                newCart.ShopId = shopId;
                newCart.IsGroup = false;
                await _unitOfWork.CartRepository.AddAsync(newCart);
                request.CartId = newCart.Id;
                CartDetail cartDetails = _mapper.Map<CartDetail>(request);
                cartDetails.CartId = newCart.Id;
                await _unitOfWork.CartDetailRepository.AddAsync(cartDetails);
            } else
            {
                request.CartId = cart.FirstOrDefault().Id;
                CartDetail cartDetails = _mapper.Map<CartDetail>(request);
                await _unitOfWork.CartDetailRepository.AddAsync(cartDetails);
            }
            return new ServiceActionResult(true)
            {
                Detail = "Add Product to Cart Successfully",
                //Data = _mapper.Map<CartResponse>((await _unitOfWork.CartRepository.FindAsyncAsQueryable(filter)).Include(x => x.CartDetails).FirstOrDefault())
                Data = request.CartId
            };
        }

        public async Task<ServiceActionResult> ChangeCartToGroup(Guid userId, Guid shopId)
        {
            Expression<Func<Cart, bool>> filter = cart => (cart.CustomerId == userId && cart.ShopId == shopId);
            var cart = (await _unitOfWork.CartRepository.FindAsyncAsQueryable(filter)).FirstOrDefault();
            if (cart == null)
            {
                Cart newCart = new Cart();
                newCart.CustomerId = userId;
                newCart.ShopId = shopId;
                newCart.IsGroup = true;
                await _unitOfWork.CartRepository.AddAsync(newCart);
                return new ServiceActionResult() 
                {
                    Data = GenerateShareLink(newCart.Id)
                };
            } else
            {
                cart.IsGroup = true;
                cart.LastUpdateDate = DateTime.Now;
                await _unitOfWork.CartRepository.UpdateAsync(cart);
                return new ServiceActionResult() 
                {
                    Data = GenerateShareLink(cart.Id)
                };
            }
        }

        public async Task<ServiceActionResult> DeleteCart(Guid cartId)
        {
            var cart = await _unitOfWork.CartRepository.FindAsync(cartId);
            if (cart == null)
            {
                return new ServiceActionResult(false, "Cart is not exits or deleted");
            } else
            {
                await _unitOfWork.CartRepository.DeleteAsync(cart);
                var cartDetails = (await _unitOfWork.CartDetailRepository.GetAllAsyncAsQueryable()).Where(x => x.CartId == cartId).AsEnumerable();
                await _unitOfWork.CartDetailRepository.DeleteRangeAsync(cartDetails);
                return new ServiceActionResult(true, "Delete Successfully");
            }
            
        }

        public async Task<ServiceActionResult> DeleteCartItem(Guid cartItemId)
        {
            var cartDetail = _unitOfWork.CartDetailRepository.FindAsync(cartItemId);
            if (cartDetail == null)
            {
                return new ServiceActionResult(false, "CartDetail is not exits or deleted");
            }
            else
            {
                await _unitOfWork.CartDetailRepository.DeleteAsync(cartItemId);
                return new ServiceActionResult(true, "Delete Successfully");
            }
        }

        public async Task<ServiceActionResult> GetAllCartForUser(Guid userId, PagingRequest request)
        {
            var carts = (await _unitOfWork.CartRepository.GetAllAsyncAsQueryable()).Where(x => x.CustomerId == userId).Include(y => y.CartDetails).ThenInclude(z => z.Product);
            var paginationResult = PaginationHelper
            .BuildPaginatedResult<Cart, CartResponse>(_mapper, carts, request.PageSize, request.PageIndex);

            return new ServiceActionResult(true) { Data = paginationResult };
        }

        public async Task<ServiceActionResult> GetAllCartItemInCart(Guid cartId, PagingRequest request)
        {
            var cartDetails = (await _unitOfWork.CartDetailRepository.GetAllAsyncAsQueryable()).Where(x => x.CartId == cartId).Include(y => y.Product);
            var paginationResult = PaginationHelper
            .BuildPaginatedResult<CartDetail, CartDetailResponse>(_mapper, cartDetails, request.PageSize, request.PageIndex);

            return new ServiceActionResult(true) { Data = paginationResult };
        }

        public async Task<ServiceActionResult> GetCartByID(Guid cartId)
        {
            var cart = (await _unitOfWork.CartRepository.GetAllAsyncAsQueryable()).Where(x => x.Id == cartId).Include(y => y.CartDetails).ThenInclude(a => a.Product).Include(z => z.CartGroupUsers).FirstOrDefault();
            if (cart == null)
            {
                return new ServiceActionResult(false, "Cart is not exits or deleted");
            }
            else
            {
                return new ServiceActionResult()
                {
                    Data = _mapper.Map<CartResponse>(cart)
                };
            }
        }

        public async Task<ServiceActionResult> GetCartDetail(Guid cartDetailId)
        {
            var cartDetail = (await _unitOfWork.CartDetailRepository.GetAllAsyncAsQueryable()).Where(x => x.Id == cartDetailId).Include(y => y.Product).FirstOrDefault();

            return new ServiceActionResult(true) { Data = cartDetail };
        }

        public async Task<ServiceActionResult> GetCartInShopForUser(Guid userId, Guid shopId)
        {
            Expression<Func<Cart, bool>> filter = cart => (cart.CustomerId == userId && cart.ShopId == shopId);
            var cart = (await _unitOfWork.CartRepository.FindAsyncAsQueryable(filter)).Include(x => x.CartDetails).ThenInclude(y => y.Product).FirstOrDefault();
            return new ServiceActionResult(true)
            {
                Data = _mapper.Map<CartResponse>(cart)
            };
        }

        public async Task<ServiceActionResult> UpdateCartDetail(Guid userId, Guid shopId, CartDetailRequest request)
        {
            Expression<Func<Cart, bool>> filter = cart => (cart.CustomerId == userId && cart.ShopId == shopId);
            var cart = (await _unitOfWork.CartRepository.FindAsyncAsQueryable(filter)).FirstOrDefault();
            if(cart == null)
            {
                return new ServiceActionResult(true) { Detail = "Cart is not exits or deleted " };
            }
            else
            {
                if(cart.Id != request.CartId)
                {
                    return new ServiceActionResult(true) { Detail = "Product is not in this Cart" };
                } else
                {
                    Expression<Func<CartDetail, bool>> filter1 = cartDetail => (cartDetail.CartId == request.CartId && cartDetail.ProductId == request.ProductId);
                    var cartDetail = (await _unitOfWork.CartDetailRepository.FindAsyncAsQueryable(filter1)).FirstOrDefault();
                    cartDetail.Quantity = request.Quantity;
                    cartDetail.Price = request.Price;
                    cartDetail.LastUpdateDate = DateTime.Now;
                    await _unitOfWork.CartDetailRepository.UpdateAsync(cartDetail);

                }
            }
            return new ServiceActionResult(true)
            {
                Detail = "Update Product to Cart Successfully",
                Data = _mapper.Map<CartResponse>((await _unitOfWork.CartRepository.FindAsyncAsQueryable(filter)).Include(x => x.CartDetails).FirstOrDefault())
            };
        }

        private string GenerateShareLink(Guid cartId)
        {
            var baseUrl = "https://localhost:7039/api/Cart/GetCartByID";
            return $"{baseUrl}{cartId}";
        }
    }
}
