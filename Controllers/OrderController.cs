using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using TMS.Api.Models.Dto;
using TMS.Api.Repositories;

namespace TMS.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public OrderController(IOrderRepository orderRepository, IMapper mapper, ILogger<OrderController> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<List<OrderDto>> GetAll()
        {
            var orders = _orderRepository.GetAll();

            /*var dtoOrders = orders.Select(o => new OrderDto()
            {
               OrderId = o.OrderId,
               OrderedAt = o.OrderedAt,
                NumberOfTickets = o.NumberOfTickets,
                TotalPrice = o.TotalPrice,
                Customer = o.Customer?.CustomerName ?? string.Empty,
                TicketCategory = o.TicketCategory?.Description ?? string.Empty
            });*/
            var ordersDto = orders.Select(o => _mapper.Map<OrderDto>(o));

            return Ok(ordersDto);
        }


        [HttpGet]
        public async Task<ActionResult<OrderDto>> GetById(int id)
        {
            var @order = await _orderRepository.GetById(id);

            if (@order == null)
            {
                return NotFound();
            }

            /* var dtoOrder = new OrderDto()
             {
                 OrderId = @order.OrderId,
                 OrderedAt = @order.OrderedAt,
                 NumberOfTickets = @order.NumberOfTickets,
                 TotalPrice = @order.TotalPrice,
                 Customer = @order.Customer?.CustomerName ?? string.Empty,
                 TicketCategory = @order.TicketCategory?.Description ?? string.Empty
             };*/
            var orderDto =_mapper.Map<OrderDto>(@order);

            return Ok(orderDto);


        }
        [HttpPatch]
        public async Task<ActionResult<OrderPatchDto>> Patch(OrderPatchDto orderPatch)
        {
            if (orderPatch == null) throw new ArgumentNullException(nameof(orderPatch));
            var orderEntity = await _orderRepository.GetById(orderPatch.OrderId);
            if (orderEntity == null)
            {
                return NotFound();
            }
            
            if (orderPatch.NumberOfTickets!=0) orderEntity.NumberOfTickets = orderPatch.NumberOfTickets;
            _orderRepository.Update(orderEntity);
            return NoContent();
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var orderEntity = await _orderRepository.GetById(id);
            if (orderEntity == null)
            {
                return NotFound();
            }
            _orderRepository.Delete(orderEntity);
            return NoContent();
        }
    }
}
