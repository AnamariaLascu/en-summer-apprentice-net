using Moq;
using System;
using System.Collections;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TMS.Api.Repositories;
using TMS.Api.Models;
using TMS.Api.Models.Dto;
using TMS.Api.Controllers;
using TMS.Api.Exceptions;

namespace TMS.UnitTests
{
    [TestClass]
    public class EventControllerTest
    {
        Mock<IEventRepository> _eventRepositoryMock;
        Mock<IMapper> _mapperMoq;
        List<Event> _moqList;
        List<EventDto> _dtoMoq;



        [TestInitialize]
        public void SetupMoqData()
        {
            _eventRepositoryMock = new Mock<IEventRepository>();
            _mapperMoq = new Mock<IMapper>();
            _moqList = new List<Event>
             {
                new Event {EventId = 1,
                    EventName = "Moq name",
                    EventDescription = "Moq description",
                    EndDate = DateTime.Now,
                    StartDate = DateTime.Now,
                    EventType = new EventType {EventTypeId = 1,EventTypeName="test event type"},
                    EventTypeId = 1,
                    Venue = new Venue {VenueId = 1,Capacity = 15, Location = "Mock location",Type = "mock type"},
                    VenueId = 1
                }
            };
            _dtoMoq = new List<EventDto>
            {
                new EventDto
                {                
                    EventDescription = "Moq description",
                    EventId = 1,
                    EventName = "Moq name",
                    EventTypeId = new EventType {EventTypeId = 1,EventTypeName="test event type"}.EventTypeId,
                    VenueId = new Venue {VenueId = 1,Capacity = 15, Location = "Mock location",Type = "mock type"}.VenueId
                }
            };
        }

        [TestMethod]
        public void GetAllEventsReturnListOfEvents()
        {
            // Arrange
            _eventRepositoryMock.Setup(moq => moq.GetAll()).Returns(_moqList);
            _mapperMoq.Setup(moq => moq.Map<IEnumerable<EventDto>>(It.IsAny<IEnumerable<Event>>())).Returns(_dtoMoq);

            var controller = new EventController(_eventRepositoryMock.Object, _mapperMoq.Object);

            // Act
            var events = controller.GetAll();
            var eventResult = events.Result as OkObjectResult;
            var eventDtoList = eventResult.Value as IEnumerable<EventDto>;

            // Assert
            Assert.AreEqual(_moqList.Count, eventDtoList.Count());
        }



        [TestMethod]
        public async Task GetEventByIdReturnNotFoundWhenNoRecordFound()
        {
            // Arrange
            int findEventWithId = 11;
            _eventRepositoryMock.Setup(moq => moq.GetById(findEventWithId)).ThrowsAsync(new EntityNotFoundException(findEventWithId, nameof(Event)));
            var controller = new EventController(_eventRepositoryMock.Object, _mapperMoq.Object);

            // Act
            var result = await controller.GetById(findEventWithId);
            var eventResult = result.Result as NotFoundObjectResult;

            // Assert
            Assert.AreEqual(404, eventResult.StatusCode);
        }



        [TestMethod]
        public async Task GetEventByIdReturnFirstRecord()
        {
            //Arrange
            _eventRepositoryMock.Setup(moq => moq.GetById(It.IsAny<int>())).Returns(Task.Run(() => _moqList.First()));
            _mapperMoq.Setup(moq => moq.Map<EventDto>(It.IsAny<Event>())).Returns(_dtoMoq.First());
            var controller = new EventController(_eventRepositoryMock.Object, _mapperMoq.Object);
            //Act

            var result = await controller.GetById(1);
            var eventResult = result.Result as OkObjectResult;
            var eventCount = eventResult.Value as EventDto;

            //Assert

            Assert.IsFalse(string.IsNullOrEmpty(eventCount.EventName));
            Assert.AreEqual(1, eventCount.EventId);
        }



       
    }
}