using AutoMapper;
using Cqrs.Tests.Model;
using System.Linq;

namespace Cqrs.Tests
{
    class TestMappingsProfile : Profile
    {
        public TestMappingsProfile()
        {
            CreateMap<Author, AuthorDto>();


            CreateMap<Blog, BlogDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Author, o => o.MapFrom(s => s.Author))
                .ForMember(d => d.Comments, o => o.MapFrom(s => s.Comments == null? null : s.Comments.Select(c => new CommentDto { Id = c.Id.ToString(), Content = c.Content })))
                .ForMember(d => d.Type, o => o.Ignore());

            CreateMap<BlogDto, Blog>();
        }
    }
}
