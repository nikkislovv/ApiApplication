using ApiApplication.Contracts;
using ApiApplication.Models;
using ApiApplication.ModelsDTO.OrderDTO;
using ApiApplication.RequestFeatures;
using AutoMapper;
using Contracts;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using Repository;
using Server.Filters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HomeController : ControllerBase
    {
        readonly UserManager<User> _userManager;
        readonly ILoggerManager _logger;
        readonly IMapper _mapper;
        readonly IRepositoryManager _repositoryManager;
        readonly IPhoneHelper _phoneHelper;

        public HomeController(UserManager<User> userManager, IMapper mapper, ILoggerManager logger, IRepositoryManager repositoryManager, IPhoneHelper phoneHelper)
        {
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
            _repositoryManager = repositoryManager;
            _phoneHelper = phoneHelper;
        }

        /// <summary>
        /// Return all acces requests for this url
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns all acces requests for this url</response>
        /// <response code="500">Server error</response>
        [HttpOptions]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public IActionResult GetCompaniesOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS,PUT, POST,DELETE");
            return Ok();
        }

        /// <summary>
        /// Get all phones
        /// </summary>
        /// <param name="phoneParameters"></param>
        /// <returns>The phones list</returns>
        /// <response code="200">Returns the list of all phones</response>
        /// <response code="400">Request parameters are not valid</response>
        /// <response code="500">Server error</response>
        [HttpGet("ShowAllPhones")]
        [HttpHead("ShowAllPhones")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllPhonesAsync([FromQuery] PhoneParameters phoneParameters)
        {
            if (!phoneParameters.ValidPriceRange)
                return BadRequest("MaxPrice can notbe less then MinPrice.");
            var _phones = await _repositoryManager.Phones.GetAllPhonesAsync(phoneParameters,true);//через lazyLoading подгружаются данные через навиг св-во
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(_phones.MetaData));//add to header some information about our paging
            var _phonesDto = _mapper.Map<IEnumerable<PhonesToShowDto>>(_phones);
            return Ok(_phonesDto);
        }

        /// <summary>
        /// Create one new order
        /// </summary>
        /// <param name="orderDto"></param>
        /// <returns></returns>
        /// <response code="201">Order created</response>
        /// <response code="400">New order is null</response>
        /// <response code="401">You are not authorized</response>
        /// <response code="403">You have no rules</response>
        /// <response code="422">New order is not valid</response>
        /// <response code="500">Server error</response>
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPost("CreateOrder")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateOrderAsync([FromBody] OrderToCreateDto orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);
            order.UserId =User.FindFirst(e => e.Type == "Id").Value;
            if (!await _phoneHelper.AddPhoneCollection(order, orderDto))
            {
                _logger.LogWarn($"{nameof(CreateOrderAsync)}: AddModifiedPhoneCollection failed");
                return StatusCode(500, "Internal server error");
            }
            _repositoryManager.Orders.CreateOrder(order);
            await _repositoryManager.SaveAsync();
            return StatusCode(201);
        }

        /// <summary>
        /// Get all orders
        /// </summary>
        /// <param name="orderParameters"></param>
        /// <returns>The orders list</returns>
        /// <response code="200">Returns the list of all orders</response>
        /// <response code="401">You are not authorized</response>
        /// <response code="403">You have no rules</response>
        /// <response code="500">Server error</response>
        [HttpGet("ShowAllOrders")]
        [HttpHead]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllOrderInfoAsync([FromQuery] OrderParameters orderParameters)
        {
            var _orders = await _repositoryManager.Orders.GetAllOrdersAsync(orderParameters,true);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(_orders.MetaData));
            var _ordersDto = _mapper.Map<IEnumerable<OrdersToShowDto>>(_orders);          
            return Ok(_ordersDto);      
        }

        /// <summary>
        /// Update order by id 
        /// </summary>
        /// <param name="orderDto"></param>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        /// <response code="204">Order updated</response>
        /// <response code="400">New order is null</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">You have no rules</response>
        /// <response code="404">Order with this id not found</response>
        /// <response code="422">New event not valid</response>
        /// <response code="500">Server error</response>
        [HttpPut("UpdateAnyOrder/{OrderId}")]
        [Authorize(Roles = "Administrator")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateAnyOrderAsync([FromBody] OrderToUpdateDto orderDto, [FromRoute] string OrderId)//изменить string на guid
        {
            var _order = await _repositoryManager.Orders.GetOrderByIdAsync(Guid.Parse(OrderId), true);

            if (_order == null)
            {
                _logger.LogInfo($"Order with id: {OrderId} doesn't exist in the database.");
                return NotFound("There is no order with such Id");

            }
            _mapper.Map(orderDto, _order);//действует наложение
            //_order = _mapper.Map<Order>(orderDto);//создается новый объект
            _phoneHelper.ClearPhonesInCollection(_order);
            if (!await _phoneHelper.AddPhoneCollection(_order, orderDto))
            {
                _logger.LogInfo($"{nameof(UpdateAnyOrderAsync)}: AddModifiedPhoneCollection failed");
                return StatusCode(500, "Internal server error");
            }
            await _repositoryManager.SaveAsync();
            return NoContent();
        }

        /// <summary>
        /// Delete order by id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        /// <response code="204">Order deleted</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">You have no rules</response>
        /// <response code="404">Order with this id not found</response>
        /// <response code="500">Server error</response>
        [HttpDelete("DeleteAnyOrder/{orderId}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteAnyOrderAsync([FromRoute] string orderId)
        {
            var _order = await _repositoryManager.Orders.GetOrderByIdAsync(Guid.Parse(orderId), false);
            if (_order == null)
            {
                _logger.LogInfo($"Order with id: {orderId} doesn't exist in the database.");
                return NotFound("There is no order with such Id");
            }
            _repositoryManager.Orders.DeleteOrder(_order);
            await _repositoryManager.SaveAsync();   
            return NoContent();
        }

        /// <summary>
        /// Delete phone by id
        /// </summary>
        /// <param name="phoneId"></param>
        /// <returns></returns>
        /// <response code="204">Phone deleted</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">You have no rules</response>
        /// <response code="404">Phone with this id not found</response>
        /// <response code="500">Server error</response>
        [HttpDelete("DeleteAnyPhone/{phoneId}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteAnyPhoneAsync([FromRoute] string phoneId)
        {
            var phone = await _repositoryManager.Phones.GetPhoneByIdAsync(Guid.Parse(phoneId), false);
            if (phone == null)
            {
                _logger.LogWarn($"{nameof(DeleteAnyOrderAsync)}: Phone was null, wrong phoneId");
                return NotFound("There is no order with such Id");
            }
            _repositoryManager.Phones.DeletePhone(phone);
            await _repositoryManager.SaveAsync();       
            return NoContent();
        }
    }
}
