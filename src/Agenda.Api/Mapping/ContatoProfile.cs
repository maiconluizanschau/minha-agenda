using Agenda.Api.Domain;
using Agenda.Api.Models;
using AutoMapper;

namespace Agenda.Api.Mapping;

public class ContatoProfile : Profile
{
    public ContatoProfile()
    {
        CreateMap<Contato, ContatoDto>();
    }
}
