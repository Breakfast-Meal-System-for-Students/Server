using BMS.BLL.Models.Requests.Admin;
using BMS.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMS.Core.Domains.Enums;
using BMS.Core.Domains.Entities;

namespace BMS.BLL.Services.IServices
{
    public interface IOrderService
    {
        Task<ServiceActionResult> GetListOrders(SearchOrderRequest request);
        Task<ServiceActionResult> GetOrderByID(Guid id);
        Task<ServiceActionResult> GetOrderByUser(Guid id, SearchOrderRequest request);
        Task<ServiceActionResult> GetOrderByShop(Guid id, SearchOrderRequest request);
        Task<ServiceActionResult> ChangeOrderStatus(Guid id, OrderStatus status);
        Task<ServiceActionResult> GetTotalOrder(TotalOrdersRequest request);

        Task<ServiceActionResult> CreateOrder(Guid cartId, Guid voucherId);
        Task<ServiceActionResult> GetStatusOrder(Guid orderId);
    }
}
