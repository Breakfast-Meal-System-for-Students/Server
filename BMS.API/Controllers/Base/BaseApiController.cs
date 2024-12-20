﻿using BMS.API.Constants;
using BMS.BLL.Models;
using BMS.Core.Exceptions.IExceptions;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace BMS.API.Controllers.Base
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        protected BaseService _baseService;
        private IActionResult BuildSuccessResult(ServiceActionResult result)
        {
            var successResult = new ApiResponse(true)
            {
                Data = result.Data,
                StatusCode = StatusCodes.Status200OK,
                IsSuccess = result.IsSuccess,
            };

            var detail = result.Detail ?? ApiMessageConstants.SUCCESS;
            successResult.AddSuccessMessage(detail);
            return base.Ok(successResult);
        }

        private IActionResult BuildErrorResult(Exception ex)
        {
            var errorResult = new ApiResponse(false);
            errorResult.AddErrorMessage(ex.Message);

            var statusCode = StatusCodes.Status500InternalServerError;
            if (ex.GetType().IsAssignableTo(typeof(INotFoundException)))
                statusCode = StatusCodes.Status404NotFound;
            else if (ex.GetType().IsAssignableTo(typeof(IBusinessException)))
                statusCode = StatusCodes.Status409Conflict;
            else if (ex.GetType().IsAssignableTo(typeof(IForbiddenException)))
                statusCode = StatusCodes.Status403Forbidden;
            errorResult.StatusCode = statusCode;
            return base.Ok(errorResult);
        }

        protected async Task<IActionResult> ExecuteServiceLogic(Func<Task<ServiceActionResult>> serviceLogicFunc)
        {
            return await ExecuteServiceLogic(serviceLogicFunc, null);

        }
        protected async Task<IActionResult> ExecuteServiceLogic(Func<Task<ServiceActionResult>> serviceLogicFunc, Func<Task<ServiceActionResult>>? errorHandler)
        {
            try
            {
                var result = await serviceLogicFunc();
                if (result.IsSuccess)
                {
                    _baseService.Commit();
                }
                else
                {
                    _baseService.Rollback();
                }
                //_baseService.Commit();
                return BuildSuccessResult(result);

            }
            catch (Exception ex)
            {
                if (errorHandler is not null)
                    await errorHandler();
                _baseService.Rollback();
                return BuildErrorResult(ex);
            }
        }
    }
}
