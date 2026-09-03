namespace SpendWise.API.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            //CategoryMaster
            CreateMap<CategoryMasterCreateDto, CategoryMaster>();
            CreateMap<CategoryMasterUpdateDto, CategoryMaster>();
            CreateMap<CategoryMaster, CategoryMasterResponseDto>()
                .ForMember(dest => dest.TypeName,
                opt => opt.MapFrom(src =>
                            src.CategoryTypeMaster != null ?
                                src.CategoryTypeMaster.Name : string.Empty));

            //PaymentMethod
            CreateMap<PaymentMethodCreateDto, PaymentMethod>();
            CreateMap<PaymentMethodUpdateDto, PaymentMethod>();
            CreateMap<PaymentMethod, PaymentMethodResponseDto>();
        }
    }
}
