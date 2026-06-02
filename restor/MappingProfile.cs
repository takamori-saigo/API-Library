using AutoMapper;
using Domains;
using Domains.DTO;

namespace restor;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<EmployeeForUpdateDto, Employee>().ReverseMap();
        CreateMap<CompanyForUpdateDTO, Company>();
    }
}