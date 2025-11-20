using Agenda.Api.Application.Dtos;
using Agenda.Api.Domain.Entities;
using AutoMapper;

namespace Agenda.Api.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Contact, ContactDto>().ReverseMap();
            CreateMap<CreateContactDto, Contact>();
            CreateMap<UpdateContactDto, Contact>();
        }
    }
}
