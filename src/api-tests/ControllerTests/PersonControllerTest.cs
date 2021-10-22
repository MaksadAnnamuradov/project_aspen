using System;
using System.Threading.Tasks;
using Api.Controllers;
using Api.DataAccess;
using Api.DtoModels;
using Api.Mappers;
using Api.Models;
using Api.Models.Entities;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;

namespace Tests.Controller
{
    public class PersonControllerTest
    {
        public static PersonController GetPersonController()
        {
            var context = TestHelpers.CreateContext();
            var personRepository = new PersonRepository(context, TestHelpers.AspenMapper);
            return new PersonController(personRepository, TestHelpers.AspenMapper);
        }

        [Test]
        public async Task CanCreatePerson()
        {
            var newPerson = new DtoPerson { Name = "George" };
            var dtoPerson = (await GetPersonController().Add(newPerson)).Value;
            dtoPerson.ID.Should().NotBe(0);
            dtoPerson.Name.Should().Be("George");
        }

        [Test]
        public async Task CanGetCreatedPerson()
        {
            var newPerson = new DtoPerson { Name = "George" };
            var createdPerson = (await GetPersonController().Add(newPerson)).Value;
            var returnedPerson = (await GetPersonController().GetByID(createdPerson.ID)).Value;
            returnedPerson.Name.Should().Be("George");
        }

        [Test]
        public async Task CanGetDifferentPerson()
        {
            var newPerson = new DtoPerson { Name = "Ben" };
            var createdPerson = (await GetPersonController().Add(newPerson)).Value;
            var returnedPerson = (await GetPersonController().GetByID(createdPerson.ID)).Value;
            returnedPerson.Name.Should().Be("Ben");
        }

        [Test]
        public async Task CanDeletePerson()
        {
            var newPerson = new DtoPerson { Name = "Ben" };
            var createdPerson = (await GetPersonController().Add(newPerson)).Value;
            await GetPersonController().Delete(createdPerson.ID);
            var badPersonRequests = await GetPersonController().GetByID(createdPerson.ID);

            var actual = badPersonRequests.Result as NotFoundObjectResult;
            actual.StatusCode.Should().Be(404);
        }

        [Test]
        public async Task CanEditPerson()
        {
            var newPerson = new DtoPerson { Name = "Ben" };
            var createdPerson = (await GetPersonController().Add(newPerson)).Value;

            var person = TestHelpers.AspenMapper.Map<Person>(createdPerson);
            var editedPerson = person.WithName("notBen").WithBio("new bio");
            await GetPersonController().Edit(TestHelpers.AspenMapper.Map<DtoPerson>(editedPerson));

            var returnedPerson = (await GetPersonController().GetByID(createdPerson.ID)).Value;
            returnedPerson.Name.Should().Be(editedPerson.Name);
        }

        [Test]
        public async Task PersonCanBeCreatedWithAuthId()
        {
            var newPerson = new DtoPerson
            {
                Name = "Bill",
                AuthID = Guid.NewGuid().ToString()
            };
            var createdPerson = (await GetPersonController().Add(newPerson)).Value;
            var returnedPerson = (await GetPersonController().GetByID(createdPerson.ID)).Value;
            returnedPerson.AuthID.Should().Be(newPerson.AuthID);
        }

        [Test]
        public async Task CanAddAuthIdToPersonAfterCreation()
        {
            var newPerson = new DtoPerson { Name = "Mike" };
            var createdPerson = (await GetPersonController().Add(newPerson)).Value;
            var authIdPerson = createdPerson.WithAuthID(Guid.NewGuid().ToString());
            await GetPersonController().Edit(authIdPerson);

            var returnedPerson = (await GetPersonController().GetByID(createdPerson.ID)).Value;
            returnedPerson.AuthID.Should().Be(authIdPerson.AuthID);
        }
    }
}