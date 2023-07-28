using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TMS.Api.Models.Dto;
using TMS.Api.Repositories;

namespace TMS.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public EventController(IEventRepository eventRepository, IMapper mapper, ILogger<EventController> logger)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<List<EventDto>> GetAll()
        {
            var events = _eventRepository.GetAll();

            /* var dtoEvents = events.Select(e => new EventDto()
             {
                 EventId = e.EventId,
                 EventDescription = e.EventDescription,
                 EventName = e.EventName,
                 EventType = e.EventType?.EventTypeName ?? string.Empty,
                 Venue = e.Venue?.Location ?? string.Empty
             });*/
            // var eventsDto = _mapper.Map<EventDto>(@event);
            var eventsDto = events.Select(e => _mapper.Map<EventDto>(e));

            return Ok(eventsDto);
        }


        [HttpGet]
        public async  Task<ActionResult<EventDto>> GetById(int id)
        {
           
                var @event = await _eventRepository.GetById(id);

            if (@event == null)
            {
                return NotFound();
            }

            //var dtoEvent = new EventDto()
            //{
            //    EventId = @event.EventId,
            //    EventDescription = @event.EventDescription,
            //    EventName = @event.EventName,
            //    EventType = @event.EventType?.EventTypeName ?? string.Empty,
            //    Venue = @event.Venue?.Location ?? string.Empty
            //};

            var eventDto = _mapper.Map<EventDto>(@event);
                return Ok(eventDto);
            
        }
        [HttpPatch]
        public async Task<ActionResult<EventPatchDto>> Patch(EventPatchDto eventPatch)
        {

            if (eventPatch == null) throw new ArgumentNullException(nameof(eventPatch));
            var eventEntity = await _eventRepository.GetById(eventPatch.EventId);
            if (eventEntity == null)
            {
                return NotFound();
            }
            if (!eventPatch.EventName.IsNullOrEmpty()) eventEntity.EventName = eventPatch.EventName;
            if (!eventPatch.EventDescription.IsNullOrEmpty()) eventEntity.EventDescription = eventPatch.EventDescription;
            _eventRepository.Update(eventEntity);
            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var eventEntity = await _eventRepository.GetById(id);
            if (eventEntity == null)
            {
                return NotFound();
            }
            _eventRepository.Delete(eventEntity);
            return NoContent();
        }
    }
}
