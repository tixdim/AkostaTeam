using Akosta.BusinessLogic.Core.Models;
using System.Threading.Tasks;

namespace Akosta.BusinessLogic.Core.Interfaces
{
    public interface IUserService
    {
        Task<UserInformationBlo> RegistrationUser(UserRegistrBlo userRegistrBlo);
        Task<UserInformationBlo> AuthenticationUser(UserIdentityBlo userIdentityBlo);
        Task<UserInformationBlo> GetUser(int userId);
        Task<bool> DoesExistUser(int userId);
        Task<bool> DeleteUser(int userId);
    }
}
