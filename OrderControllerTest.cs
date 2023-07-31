using AutoMapper;
using Moq;
using TMS.Api.Models.Dto;
using TMS.Api.Models;
using TMS.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using TMS.Api.Controllers;
using TMS.Api.Exceptions;

namespace TMS.UnitTests
{
    [TestClass]
    public class OrderControllerTest
    {
        Mock<IOrderRepository> _orderRepositoryMock;
        Mock<IMapper> _mapperMoq;
        List<Order> _moqList;
        List<OrderDto> _dtoMoq;

        [TestInitialize]
        public void SetupMoqData()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _mapperMoq = new Mock<IMapper>();
            _moqList = new List<Order>


     {
                new Order {OrderId = 1,
                    NumberOfTickets = 2,
                    OrderedAt = DateTime.Now,
                    TotalPrice = 1500,
                    Customer = new Customer {CustomerId = 1},
                    CustomerId = 1,
                    TicketCategory = new TicketCategory {TicketCategoryId = 1},
                    TicketCategoryId = 1
                }
            };
            _dtoMoq = new List<OrderDto>
            {
                new OrderDto
                {
                    OrderId = 1,
                     NumberOfTickets = 2,

                    OrderedAt = DateTime.Now,

                   Customer = new Customer {CustomerId = 1,CustomerName="Mock customer",Email="mock@yahoo.com"}.CustomerName,

                    TicketCategoryId = new TicketCategory {TicketCategoryId = 1,Description="Mock description",Price=1500,EventId=1}.TicketCategoryId
                }
            };
        }
        [TestMethod]
        public async Task GetAllOrdersReturnListOfOrders()
        {
            //Arrange

            IReadOnlyList<Order> moqOrders = _moqList;
            Task<IReadOnlyList<Order>> moqTask = Task.Run(() => moqOrders);
            _orderRepositoryMock.Setup(moq => moq.GetAll()).Returns(_moqList);

            _mapperMoq.Setup(moq => moq.Map<IEnumerable<OrderDto>>(It.IsAny<Order>())).Returns(_dtoMoq);

            var controller = new OrderController(_orderRepositoryMock.Object, _mapperMoq.Object);

            //Act
            var orders = controller.GetAll();
            var orderResult = orders.Result as OkObjectResult;
            var orderDtoList = orderResult.Value as IEnumerable<OrderDto>;

            //Assert

            Assert.AreEqual(_moqList.Count, orderDtoList.Count());
        }
        [TestMethod]
        public async Task GetOrderByIdReturnNotFoundWhenNoRecordFound()
        {
            //Arrange
            int findOrderWithId = 11;
            _orderRepositoryMock.Setup(moq => moq.GetById(findOrderWithId)).ThrowsAsync(new EntityNotFoundException(findOrderWithId, nameof(Order)));

            var controller = new OrderController(_orderRepositoryMock.Object, _mapperMoq.Object);
            //Act

            var result = await controller.GetById(findOrderWithId);
            var orderResult = result.Result as NotFoundObjectResult;


            //Assert

            Assert.IsTrue(orderResult.StatusCode == 404);
        }

        [TestMethod]
        public async Task GetOrderByIdReturnFirstRecord()
        {
            //Arrange
            _orderRepositoryMock.Setup(moq => moq.GetById(It.IsAny<int>())).Returns(Task.Run(() => _moqList.First()));
            _mapperMoq.Setup(moq => moq.Map<OrderDto>(It.IsAny<Order>())).Returns(_dtoMoq.First());
            var controller = new OrderController(_orderRepositoryMock.Object, _mapperMoq.Object);

            //Act
            var result = await controller.GetById(1);
            var orderResult = result.Result as OkObjectResult;
            var orderCount = orderResult.Value as OrderDto;

            //Assert
            Assert.IsFalse(int.Equals(0, orderCount.OrderId));
            Assert.AreEqual(1, orderCount.OrderId);
        }

    }
}
