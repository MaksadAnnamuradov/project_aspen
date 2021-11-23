﻿using Api;
using Api.DataAccess;
using Api.DbModels;
using Api.DtoModels;
using Api.Exceptions;
using Api.Models.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.DataAccess
{
    public interface IEventRepository
    {
        Task<Event> AddAsync(DateTime date, string title, string description, string primaryImageUrl, string location);
        Task DeleteAsync(long id);
        Task EditAsync(Event e);
        public Task<bool> ExistsAsync(long id);
        Task<Event> GetByIdAsync(long id);
        Task<IEnumerable<Event>> GetAllAsync();
    }

    public class EventRepository : IEventRepository
    {
        private readonly AspenContext context;
        private readonly IMapper mapper;

        public EventRepository(AspenContext context, IMapper mapper)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper;
        }

        public async Task<bool> ExistsAsync(long id)
        {
            return await context.Events.AnyAsync(e => e.ID == id);
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            var eventList = await EntityFrameworkQueryableExtensions.ToListAsync(context.Events);
            return mapper.Map<IEnumerable<DbEvent>, IEnumerable<Event>>(eventList);
        }

        public async Task<Event> GetByIdAsync(long id)
        {
            var e = await context.Events.FindAsync(id);

            return mapper.Map<Event>(e);
        }
        public async Task<Event> AddAsync(DateTime date, string title, string description, string primaryImageUrl, string location)
        {

            var newEvent = new DbEvent {
                Date = date,
                Title = title,
                Description = description,
                PrimaryImageUrl = primaryImageUrl,
                Location = location
            };
            context.Events.Add(newEvent);
            await context.SaveChangesAsync();

            return mapper.Map<Event>(newEvent);
        }

        public async Task EditAsync(Event e)
        {
            var dbEvent = mapper.Map<DbEvent>(e);
            context.Update(dbEvent);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var e = await context.Events.FindAsync(id);
            if(e == null)
                throw new NotFoundException<Event>("Event id does not exist");
            context.Events.Remove(e);
            await context.SaveChangesAsync();
        }
    }
}