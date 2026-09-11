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
                opt => opt.MapFrom(src => src.CategoryTypeMaster != null 
                                        ? src.CategoryTypeMaster.Name 
                                        : string.Empty));

            CreateMap<CategoryMaster, UserCategory>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.ApplicationUser, opt => opt.Ignore())
                .ForMember(dest => dest.CategoryTypeMaster, opt => opt.Ignore());

            //PaymentMethod
            CreateMap<PaymentMethodCreateDto, PaymentMethod>();
            CreateMap<PaymentMethodUpdateDto, PaymentMethod>();
            CreateMap<PaymentMethod, PaymentMethodResponseDto>();

            //Notification
            CreateMap<NotificationCreateDto, Notification>();
            CreateMap<Notification,NotificationResponseDto>();

            //UserCategory
            CreateMap<UserCategoryCreateDto, UserCategory>();
            CreateMap<UserCategoryUpdateDto, UserCategory>();
            CreateMap<UserCategory, UserCategoryResponseDto>()
                .ForMember(dest=>dest.TypeName,
                opt=>opt.MapFrom(src=>src.CategoryTypeMaster!=null
                                     ?src.CategoryTypeMaster.Name 
                                     :string.Empty));


            //Transaction
            CreateMap<TransactionCreateDto, Transaction>();
            CreateMap<TransactionUpdateDto,Transaction>();
            CreateMap<Transaction, TransactionResponseDto>()
                .ForMember(
                    dest => dest.UserCategoryName,
                    opt => opt.MapFrom(src => src.UserCategory != null
                                            ? src.UserCategory.Name
                                            : string.Empty)
                )
                .ForMember(
                    dest => dest.PaymentMethodName,
                    opt => opt.MapFrom(src => src.PaymentMethod != null
                                            ? src.PaymentMethod.Name
                                            : string.Empty)
                )
                .ForMember(
                    dest => dest.CategoryTypeId,
                    opt => opt.MapFrom(src => src.UserCategory != null
                                            ? src.UserCategory.TypeId
                                            : -1)
                )
                .ForMember(
                    dest => dest.CategoryTypeName,
                    opt => opt.MapFrom(src => src.UserCategory != null && src.UserCategory!.CategoryTypeMaster != null
                                            ? src.UserCategory.CategoryTypeMaster.Name
                                            : string.Empty)
                )
                .ForMember(
                    dest => dest.HasAttachment,
                    opt => opt.MapFrom(src =>
                        src.TransactionAttachment != null)
                )
                .ForMember(
                    dest => dest.AttachmentFileName,
                    opt => opt.MapFrom(src =>
                        src.TransactionAttachment != null
                            ? src.TransactionAttachment.FileName
                            : null));

            //UserDashboard
            CreateMap<Transaction, RecentTransactionDto>()
                .ForMember(
                    dest => dest.CategoryName,
                    opt => opt.MapFrom(src =>
                        src.UserCategory != null
                            ? src.UserCategory.Name
                            : string.Empty))
                .ForMember(
                    dest => dest.Type,
                    opt => opt.MapFrom(src =>
                        src.UserCategory != null &&
                        src.UserCategory.TypeId == (int)CategoryType.Income
                            ? "Income"
                            : "Expense"));

            //AdminDashboard
            CreateMap<ApplicationUser, RecentRegistrationDto>();
 
        }
    }
}
