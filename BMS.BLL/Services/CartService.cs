using AutoMapper;
using Azure.Core;
using BMS.BLL.Models;
using BMS.BLL.Models.Requests.Basic;
using BMS.BLL.Models.Requests.Cart;
using BMS.BLL.Models.Responses.Cart;
using BMS.BLL.Models.Responses.Shop;
using BMS.BLL.Models.Responses.Users;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.BLL.Utilities;
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
        private readonly ITokenService _tokenService;
        private readonly IShopService _shopService;
        private IProductService _productService;
        public CartService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager, ITokenService tokenService, IShopService shopService, IProductService productService) : base(unitOfWork, mapper)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _shopService = shopService;
            _productService = productService;
        }

        public async Task<ServiceActionResult> AddCartDetail(Guid userId, Guid shopId, CartDetailRequest request)
        {
            int inventory = 0;
            var productInShop = (await _unitOfWork.ProductRepository.GetAllAsyncAsQueryable()).Where(x => x.Id == request.ProductId && x.ShopId == shopId).SingleOrDefault();
            if (productInShop == null)
            {
                return new ServiceActionResult(false)
                {
                    Detail = "Product is not in This Shop"
                };
            }
            else if(productInShop.isOutOfStock)
            {
                return new ServiceActionResult(false)
                {
                    Detail = "Product is out of stock"
                };
            }
            /*else
            {

                inventory = await _productService.GetInventoryOfProductInDay(request.ProductId);
                if (request.Quantity > inventory)
                {
                    return new ServiceActionResult(false)
                    {
                        Detail = $"The Inventory Of This Product In Shop is had already {inventory} now. Please booking this product less than or equals {inventory}"
                    };
                }
            }*/
            Expression<Func<Cart, bool>> filter = cart => (cart.CustomerId == userId && cart.ShopId == shopId);
            var cart = (await _unitOfWork.CartRepository.GetAllAsyncAsQueryable()).Include(a => a.CartDetails).Where(filter);
            if (cart == null || cart.FirstOrDefault() == null)
            {
                Cart newCart = new Cart();
                newCart.CustomerId = userId;
                newCart.ShopId = shopId;
                newCart.IsGroup = false;
                await _unitOfWork.CartRepository.AddAsync(newCart);
                await _unitOfWork.CommitAsync();
                request.CartId = newCart.Id;
                CartDetail cartDetails = _mapper.Map<CartDetail>(request);
                cartDetails.CartId = newCart.Id;
                await _unitOfWork.CartDetailRepository.AddAsync(cartDetails);
                await _unitOfWork.CommitAsync();
            } else
            {
                /*int x = cart.FirstOrDefault().CartDetails.Where(a => a.ProductId == request.ProductId).Sum(x => x.Quantity);
                if (request.Quantity > (inventory - x))
                {
                    return new ServiceActionResult(false)
                    {
                        Detail = $"The Inventory Of This Product In Shop is had already {inventory} now and you added {x} to your Cart. Please booking this product less than or equals {inventory - x}"
                    };
                }*/
                request.CartId = cart.FirstOrDefault().Id;
                CartDetail cartDetails = _mapper.Map<CartDetail>(request);
                var cartDetailInDB = (await _unitOfWork.CartDetailRepository.GetAllAsyncAsQueryable()).Where(x => x.ProductId.Equals(cartDetails.ProductId) && x.CartId == cartDetails.CartId && x.Note == cartDetails.Note).FirstOrDefault();
                if (cartDetailInDB != null && cartDetailInDB.Note.Equals(cartDetails.Note))
                {
                    cartDetailInDB.Quantity += cartDetails.Quantity;
                    //cartDetailInDB.Price += cartDetails.Price;
                    await _unitOfWork.CartDetailRepository.UpdateAsync(cartDetailInDB);
                } else
                {
                    await _unitOfWork.CartDetailRepository.AddAsync(cartDetails);
                    await _unitOfWork.CommitAsync();
                }
            }
            return new ServiceActionResult(true)
            {
                Detail = "Add Product to Cart Successfully",
                //Data = _mapper.Map<CartResponse>((await _unitOfWork.CartRepository.FindAsyncAsQueryable(filter)).Include(x => x.CartDetails).FirstOrDefault())
                Data = request.CartId
            };
        }

        public async Task<ServiceActionResult> AddCartDetailForGroup(Guid userId, Guid cartId, CartGroupDetailRequest request)
        {
            var shop = await _unitOfWork.ShopRepository.FindAsync(request.ShopId);
            if (shop == null || shop.IsDeleted == true)
            {
                return new ServiceActionResult(false)
                {
                    Detail = "Shop is not valid or delete"
                };
            }
            var cart = (await _unitOfWork.CartRepository.GetAllAsyncAsQueryable()).Include(a => a.CartDetails).Where(x => x.Id == cartId).FirstOrDefault();
            if (cart == null)
            {
                return new ServiceActionResult(false, "Cart is not exits or deleted");
            } else
            {
                if(cart.IsGroup == false)
                {
                    return new ServiceActionResult(false, "Cart is not group");
                }
                var productInShop = (await _unitOfWork.ProductRepository.GetAllAsyncAsQueryable()).Where(x => x.Id == request.ProductId && x.ShopId == request.ShopId).SingleOrDefault();
                if (productInShop == null)
                {
                    return new ServiceActionResult(false)
                    {
                        Detail = "Product is not in This Shop"
                    };
                }
                else if (productInShop.isOutOfStock)
                {
                    return new ServiceActionResult(false)
                    {
                        Detail = "Product is out of stock"
                    };
                }
                /*else
                {
                    var inventory = await _productService.GetInventoryOfProductInDay(request.ProductId);
                    if (request.Quantity > inventory)
                    {
                        return new ServiceActionResult(false)
                        {
                            Detail = $"The Inventory Of This Product In Shop is had already {inventory} now. Please booking this product less than or equals {inventory}"
                        };
                    }
                    int x = cart.CartDetails.Where(a => a.ProductId == request.ProductId).Sum(x => x.Quantity);
                    if (request.Quantity > (inventory - x))
                    {
                        return new ServiceActionResult(false)
                        {
                            Detail = $"The Inventory Of This Product In Shop is had already {inventory} now and the cart group added {x} to this Cart. Please booking this product less than or equals {inventory - x}"
                        };
                    }

                }*/
                request.CartId = cart.Id;
                CartDetail cartDetail = _mapper.Map<CartDetail>(request);
                
                var cartGroupUser = (await _unitOfWork.CartGroupUserRepository.GetAllAsyncAsQueryable()).Where(x => x.CartId == cartId && x.UserId == userId).FirstOrDefault();
                if(cartGroupUser != null)
                {
                    cartDetail.CartGroupUserId = cartGroupUser.Id;
                } else
                {
                    CartGroupUser newCartGroupUser = new CartGroupUser()
                    {
                        UserId = userId,
                        CartId = cartId
                    };
                    await _unitOfWork.CartGroupUserRepository.AddAsync(newCartGroupUser);
                    cartDetail.CartGroupUserId = newCartGroupUser.Id;
                }
                
                await _unitOfWork.CartDetailRepository.AddAsync(cartDetail);
                return new ServiceActionResult()
                {
                    Data = cartId,
                    Detail = "Add CartDetail to GroupCart Successfully"
                };
            }
        }

        public async Task<ServiceActionResult> ChangeCartToGroup(Guid userId, Guid shopId)
        {
            var shop = await _shopService.GetShop(shopId);
            if (shop.Data == null || ((ShopResponse)shop.Data).IsDeleted == true)
            {
                throw new ArgumentNullException("Shop does not exist or has been deleted");
            }
            Expression<Func<Cart, bool>> filter = cart => (cart.CustomerId == userId && cart.ShopId == shopId);
            var cart = (await _unitOfWork.CartRepository.GetAllAsyncAsQueryable()).Where(filter).Include(x => x.CartDetails).FirstOrDefault();
            if (cart == null)
            {
                Cart newCart = new Cart();
                newCart.CustomerId = userId;
                newCart.ShopId = shopId;
                newCart.IsGroup = true;
                await _unitOfWork.CartRepository.AddAsync(newCart);
                return new ServiceActionResult()
                {
                    Data = await GenerateShareLink(newCart.Id)
                };
            } 
            else
            {
                if (cart.IsGroup == false)
                {
                    cart.IsGroup = true;
                    cart.LastUpdateDate = DateTimeHelper.GetCurrentTime();
                    await _unitOfWork.CartRepository.UpdateAsync(cart);
                }
                var cartGroupUser = (await _unitOfWork.CartGroupUserRepository.GetAllAsyncAsQueryable()).Where(x => x.CartId == cart.Id && x.UserId == userId).FirstOrDefault();
                if (cartGroupUser != null)
                {
                    foreach (var cartDetail in cart.CartDetails)
                    {
                        if (cartDetail.CartGroupUserId == null)
                        {
                            cartDetail.CartGroupUserId = cartGroupUser.Id;
                        }
                    }
                }
                else
                {
                    CartGroupUser newCartGroupUser = new CartGroupUser()
                    {
                        UserId = userId,
                        CartId = cart.Id,
                    };
                    await _unitOfWork.CartGroupUserRepository.AddAsync(newCartGroupUser);
                    foreach (var cartDetail in cart.CartDetails)
                    {
                        if (cartDetail.CartGroupUserId == null)
                        {
                            cartDetail.CartGroupUserId = newCartGroupUser.Id;
                        }
                    }
                }
                return new ServiceActionResult()
                {
                    Data = await GenerateShareLink(cart.Id)
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
                
                var cartDetails = (await _unitOfWork.CartDetailRepository.GetAllAsyncAsQueryable()).Where(x => x.CartId == cartId).AsEnumerable();
                await _unitOfWork.CartDetailRepository.DeleteRangeAsync(cartDetails);
                var cartGroupUsers = (await _unitOfWork.CartGroupUserRepository.GetAllAsyncAsQueryable()).Where(x => x.CartId == cartId).AsEnumerable();
                await _unitOfWork.CartGroupUserRepository.DeleteRangeAsync(cartGroupUsers);
                await _unitOfWork.CartRepository.DeleteAsync(cart);
                return new ServiceActionResult(true, "Delete Successfully");
            }
            
        }

        public async Task<ServiceActionResult> DeleteCartItem(Guid cartItemId)
        {
            var cartDetail = await _unitOfWork.CartDetailRepository.FindAsync(cartItemId);
            if (cartDetail == null)
            {
                return new ServiceActionResult(false, "CartDetail is not exits or deleted");
            }
            else
            {
                await _unitOfWork.CartDetailRepository.DeleteAsync(cartDetail);
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
            var cartDetails = (await _unitOfWork.CartDetailRepository.GetAllAsyncAsQueryable()).Where(x => x.CartId == cartId).Include(y => y.Product).ThenInclude(z => z.Images)
                                                                                                                            .Include(a => a.CartGroupUser).ThenInclude(a => a.User);
            var paginationResult = PaginationHelper
            .BuildPaginatedResult<CartDetail, CartDetailResponse>(_mapper, cartDetails, request.PageSize, request.PageIndex);

            return new ServiceActionResult(true) { Data = paginationResult };
        }

        public async Task<ServiceActionResult> GetCartByID(Guid cartId)
        {
            var cart = (await _unitOfWork.CartRepository.GetAllAsyncAsQueryable()).Where(x => x.Id == cartId).Include(y => y.CartDetails).ThenInclude(a => a.Product).ThenInclude(b => b.Images)
                                                                                          .Include(y => y.CartDetails).Include(z => z.CartGroupUsers).ThenInclude(a => a.User).FirstOrDefault();
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
            var cartDetail = (await _unitOfWork.CartDetailRepository.GetAllAsyncAsQueryable()).Where(x => x.Id == cartDetailId).Include(y => y.Product).ThenInclude(z => z.Images)
                                                                                                            .Include(a => a.CartGroupUser).ThenInclude(a => a.User).FirstOrDefault();

            return new ServiceActionResult(true) { Data = cartDetail };
        }

        public async Task<ServiceActionResult> GetCartInShopForUser(Guid userId, Guid shopId)
        {
            Expression<Func<Cart, bool>> filter = cart => (cart.CustomerId == userId && cart.ShopId == shopId);
            var cart = (await _unitOfWork.CartRepository.FindAsyncAsQueryable(filter)).Include(x => x.CartDetails).ThenInclude(y => y.Product).ThenInclude(z => z.Images)
                                                                                       .Include(x => x.CartDetails).ThenInclude(y => y.CartGroupUser).ThenInclude(a => a.User).FirstOrDefault();
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
                    cartDetail.LastUpdateDate = DateTimeHelper.GetCurrentTime();
                    await _unitOfWork.CartDetailRepository.UpdateAsync(cartDetail);

                }
            }
            return new ServiceActionResult(true)
            {
                Detail = "Update Product to Cart Successfully",
                Data = _mapper.Map<CartResponse>((await _unitOfWork.CartRepository.FindAsyncAsQueryable(filter)).Include(x => x.CartDetails).FirstOrDefault())
            };
        }

        public async Task<ServiceActionResult> CountCartItemInShop(Guid userId, Guid shopId)
        {
            var cartItem = (await _unitOfWork.CartDetailRepository.GetAllAsyncAsQueryable())
                .Include(x => x.Cart).Where(y => y.Cart.CustomerId == userId && y.Cart.ShopId == shopId)
                .Select(z => new
                {
                    Id = z.Id,
                    Total = z.Quantity
                });
            return new ServiceActionResult()
            {
                Data = cartItem.Sum(x => x.Total)
            };
        }

    private async Task<LinkResponse> GenerateShareLink(Guid cartId)
        {
            //var baseUrl = "https://bms-fs-api.azurewebsites.net/api/Cart/GetCartBySharing/";
            string tokenString = await _tokenService.GenerateTokenForShareLink(cartId);
            //return $"{baseUrl}{cartId}" + $"?access_token={tokenString}";
            return new LinkResponse()
            {
                CartId = cartId,
                AccessToken = tokenString,
            };
        }
    }
}
