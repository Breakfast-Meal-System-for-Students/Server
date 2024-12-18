﻿using BMS.BLL.Models;
using BMS.BLL.Models.Requests.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services.IServices
{
    public interface ICartGroupUserService
    {
        Task<ServiceActionResult> GetAllUserInGroupCart(Guid cartId, PagingRequest request);
    }
}
