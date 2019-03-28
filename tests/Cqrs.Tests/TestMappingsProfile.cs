using AutoMapper;
using Cqrs.Tests.Model;
using System.Linq;

namespace Cqrs.Tests
{
    class TestMappingsProfile : Profile
    {
        public TestMappingsProfile()
        {
            CreateMap<Blog, BlogDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Comments, o => o.MapFrom(s => s.Comments.Select(c => c.Content)));
        }
    }
}
