using AutoMapper;
using WebApplication1.Data;
using WebApplication1.ViewModels;

namespace WebApplication1.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() {
            CreateMap<RegisterVM, KhachHang>();
        }
    }
}
