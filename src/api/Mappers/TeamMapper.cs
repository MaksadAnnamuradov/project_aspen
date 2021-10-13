using Aspen.Api.DbModels;
using Aspen.Api.DtoModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aspen.Api.Mappers
{
    public class TeamMapper : Profile
    {
        public TeamMapper()
        {
            CreateMap<DbTeam, DtoTeam>();
            CreateMap<DtoTeam, DbTeam>();
            CreateMap<IEnumerable<DbTeam>, IEnumerable<DtoTeam>>();
        }
    }
}
