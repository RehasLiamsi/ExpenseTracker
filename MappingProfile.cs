using AutoMapper;
using ExpenseTracker.DTO;
using ExpenseTracker.Models;

namespace ExpenseTracker
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>()
            .ForMember(dest => dest.ExpenseIds,
               opt => opt.MapFrom(src => src.Expenses != null
                   ? src.Expenses.Select(e => e.Id).ToList()
                   : new List<int>()));

            CreateMap<UserDTO, User>()
            .ForMember(dest => dest.Expenses, opt => opt.Ignore());

            CreateMap<Expense, ExpenseDTO>();

            CreateMap<ExpenseDTO, Expense>();
        }
    }
}
