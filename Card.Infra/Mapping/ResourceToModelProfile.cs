using AutoMapper;
using Cards.Infra.Models;
using Cards.Infra.Resources;

namespace Cards.Infra.Mapping
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<SaveCardResource, Card>();
        }
    }
}
