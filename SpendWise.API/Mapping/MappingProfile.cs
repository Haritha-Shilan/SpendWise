using AutoMapper;
using SpendWise.API.Features.CategoryMaster.DTOs;

namespace SpendWise.API.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<CategoryMasterCreateDto, CategoryMaster>();
            CreateMap<CategoryMasterUpdateDto, CategoryMaster>();
            CreateMap<CategoryMaster, CategoryMasterResponseDto>()
                .ForMember(dest => dest.TypeName,
                opt => opt.MapFrom(src =>
                            src.CategoryTypeMaster != null ?
                                src.CategoryTypeMaster.Name : string.Empty));
        }
    }
}
