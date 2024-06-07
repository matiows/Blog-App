using AutoMapper;
using Blog_App.DTO.Blogs;
using Blog_App.DTO.UserAccounts;

namespace Blog_App.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<UserAccount, UserAccountReturnDTO>();
            CreateMap<Blog, BlogReturnDTO>();
        }
        
    }
}
