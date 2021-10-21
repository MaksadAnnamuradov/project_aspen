﻿using Api.DbModels;
using Api.Models.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.DataAccess
{
    public interface IRegistrationRepository
    {
        Task<Registration> AddRegistrationAsync(long teamId, long ownerId);
        Task DeleteRegistrationAsync(long registrationID);
        Task<Registration> EditRegistrationAsync(Registration registration);
        Task<IEnumerable<Registration>> GetRegistrationsAsync();
    }

    public class RegistrationRepository : IRegistrationRepository
    {
        private readonly AspenContext context;
        private readonly IMapper mapper;

        public RegistrationRepository(AspenContext context, IMapper mapper)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper;
        }

        private bool RegistrationExists(long registrationID)
        {
            return context.Registrations.Any(e => e.ID == registrationID);
        }

        public async Task<IEnumerable<Registration>> GetRegistrationsAsync()
        {
            var dbRegistrations = await EntityFrameworkQueryableExtensions
                .ToListAsync(context.Registrations);
            return mapper.Map<List<Registration>>(dbRegistrations);

        }

        public async Task<Registration> AddRegistrationAsync(long teamId, long ownerId)
        {
            var dbRegistration = new DbRegistration
            {
                TeamID = teamId,
                OwnerID = ownerId
            };
            context.Registrations.Add(dbRegistration);
            await context.SaveChangesAsync();
            return mapper.Map<Registration>(dbRegistration);
        }

        public async Task<Registration> EditRegistrationAsync(Registration registration)
        {
            var dtoRegistration = mapper.Map<DbRegistration>(registration);
            context.Update(dtoRegistration);
            await context.SaveChangesAsync();
            return mapper.Map<Registration>(dtoRegistration);
        }

        public async Task DeleteRegistrationAsync(long registrationID)
        {
            var registration = await context.Registrations.FindAsync(registrationID);

            context.Registrations.Remove(registration);
            await context.SaveChangesAsync();
        }

    }
}
