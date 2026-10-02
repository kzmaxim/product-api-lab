using AutoMapper;
using ProductApiLab.Dtos;
using ProductApiLab.Models;

namespace ProductApiLab.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
    }
}
