﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspen.Api.DbModels;
using Aspen.Api.DtoModels;
using AutoMapper;
using dotnet.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventRepository eventRepository;
        private readonly IMapper mapper;

        public EventController(IEventRepository eventRepository, IMapper mapper)
        {
            this.mapper = mapper;
            this.eventRepository = eventRepository;
        }

        [HttpGet("all")]
        public async Task<IEnumerable<DtoEvent>> GetAllEvents()
        {
            var events =  await eventRepository.GetEventsAsync();
            return mapper.Map<IEnumerable<DtoEvent>>(events);
        }

        [HttpGet]
        public async Task<ActionResult<DtoEvent>> GetEventByID(string eventID)
        {

            if (eventRepository.EventExists(eventID))
            {
                var dbEvent = await eventRepository.GetEventAsync(eventID);

                return mapper.Map<DtoEvent>(dbEvent);
            }
            else
            {
                return BadRequest("Event id does not exist");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddEvent([FromBody] DtoEvent e)
        {

            if (ModelState.IsValid)
            {
                if (!eventRepository.EventExists(e.ID))
                {   var dbEvent = mapper.Map<DbEvent>(e);
                    await eventRepository.AddEventAsync(dbEvent);
                    return Ok();
                }
                else
                {
                    return BadRequest("Event already exists");
                }
            }
            return BadRequest("Event object is not valid");
        }

        [HttpPut]
        public  async Task<IActionResult> EditEvent([FromBody] DtoEvent e)
        {
            if (ModelState.IsValid)
            {
                var dbEvent = mapper.Map<DbEvent>(e);
                await eventRepository.EditEventAsync(dbEvent);
                return Ok();
            }
            return BadRequest();
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteEvent(string eventID)
        {
            if (eventRepository.EventExists(eventID))
            {
                 await eventRepository.DeleteEventAsync(eventID);
                return Ok();
            }
            else
            {
                return BadRequest("Event id does not exist");
            }
            
        }

    }
}
