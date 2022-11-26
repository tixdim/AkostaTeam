using Akosta.API.Models;
using Akosta.BusinessLogic.Core.Models;
using Akosta.DataAccess.Core.Models;
using AutoMapper;

namespace Akosta.BusinessLogic.AutoMapperProfile
{
    public class BusinessLogicProfile: Profile
    {
        public BusinessLogicProfile()
        {
            CreateMap<StudyRto, StudyInformationBlo>()
                .ForMember(x => x.Id, x => x.MapFrom(m => m.Id))
                // .ForMember(x => x.UserId, x => x.MapFrom(m => m.UserId))
                .ForMember(x => x.SkillsInCource, x => x.MapFrom(m => m.SkillsInCource))
                .ForMember(x => x.Store, x => x.MapFrom(m => m.Store));

            CreateMap<UserRto, UserInformationBlo>()
                .ForMember(x => x.Id, x => x.MapFrom(m => m.Id))
                .ForMember(x => x.Telegram, x => x.MapFrom(m => m.Telegram))
                .ForMember(x => x.Name, x => x.MapFrom(m => m.Name))
                .ForMember(x => x.Surname, x => x.MapFrom(m => m.Surname))
                .ForMember(x => x.IsWorker, x => x.MapFrom(m => m.IsWorker))
                .ForMember(x => x.Skill, x => x.MapFrom(m => m.Skill));

            CreateMap<UserRegistrDto, UserRegistrBlo>();

            CreateMap<UserIdentityDto, UserIdentityBlo>();

            CreateMap<UserCritetiaDto, UserCritetiaBlo>();

            CreateMap<UserInformationBlo, UserInformationDto>();

            CreateMap<StudyAddDto, StudyAddBlo>();

            CreateMap<StudyInformationBlo, StudyInformationDto>();
        }
    }
}
