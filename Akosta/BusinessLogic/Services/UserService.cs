using System;
using System.Threading.Tasks;
using Akosta.BusinessLogic.Core.Interfaces;
using Akosta.BusinessLogic.Core.Models;
using Akosta.DataAccess.Core.Interfaces.DBContext;
using Akosta.DataAccess.Core.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Share.Exceptions;

namespace Akosta.BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IDbContext _context;
        public UserService(IMapper mapper, IDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<UserInformationBlo> RegistrationUser(UserRegistrBlo userRegistrBlo)
        {
            bool result = await _context.Users.AnyAsync(y => y.Telegram == userRegistrBlo.Telegram);

            if (!userRegistrBlo.Telegram.Contains("@")) throw new BadRequestException($"Вы неправильно ввели Телеграм");

            if (result == true) throw new BadRequestException($"Пользователь с телеграмом {userRegistrBlo.Telegram} уже зарегистрирован");

            if (userRegistrBlo.Telegram == null || userRegistrBlo.Name == null || userRegistrBlo.Surname == null ||
                userRegistrBlo.FirstPassword == null || userRegistrBlo.SecondPassword == null || userRegistrBlo.Skill == null ||
                userRegistrBlo.Telegram == "" || userRegistrBlo.Name == "" || userRegistrBlo.Surname == "" ||
                userRegistrBlo.FirstPassword == "" || userRegistrBlo.SecondPassword == "" || userRegistrBlo.Skill == "") 
                throw new BadRequestException($"Вы заполнили не все поля");

            if (userRegistrBlo.FirstPassword != userRegistrBlo.SecondPassword) throw new BadRequestException($"Пароли не совпадают");

            if (userRegistrBlo.FirstPassword.Length < 6) throw new BadRequestException("Длина пароля должна быть не менее 6 символов");

            UserRto user = new UserRto()
            {
                Telegram = userRegistrBlo.Telegram,
                Name = userRegistrBlo.Name,
                Surname = userRegistrBlo.Surname,
                Password = userRegistrBlo.FirstPassword,
                IsWorker = userRegistrBlo.IsWorker,
                Skill = userRegistrBlo.Skill
            };
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return ConvertToUserInformationBlo(user);
        }

        public async Task<UserInformationBlo> AuthenticationUser(UserIdentityBlo userIdentityBlo)
        {
            if (userIdentityBlo.Telegram == null || userIdentityBlo.Password == null) throw new BadRequestException("Вы заполнили не все поля");

            UserRto user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Telegram == userIdentityBlo.Telegram && p.Password == userIdentityBlo.Password);

            if (user == null) throw new BadRequestException("Неверное имя пользователя или пароль");
            
            return ConvertToUserInformationBlo(user);
        }

        public async Task<UserInformationBlo> GetUser(int userId)
        {
            UserRto user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null) throw new NotFoundException($"Пользователя с id {userId} нет");

            return ConvertToUserInformationBlo(user);
        }

        public async Task<bool> DoesExistUser(int userId)
        {
            bool result = await _context.Users.AnyAsync(y => y.Id == userId);
            return result;
        }

        public async Task<bool> DeleteUser(int userId)
        {
            bool result = await _context.Users.AnyAsync(y => y.Id == userId);
            if (result == false) throw new NotFoundException($"Пользователя с id {userId} нет");

            var userIsDelete = await _context.Users
                .FindAsync(userId);

            if (userIsDelete != null)
            {
                _context.Users.Remove(userIsDelete);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }


        private UserInformationBlo ConvertToUserInformationBlo(UserRto userRto)
        {
            if (userRto == null) throw new ArgumentNullException(nameof(userRto));

            UserInformationBlo userInformationBlo = _mapper.Map<UserInformationBlo>(userRto);

            return userInformationBlo;
        }
    }
}
