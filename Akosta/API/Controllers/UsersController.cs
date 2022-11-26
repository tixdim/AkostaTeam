using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Akosta.API.Models;
using Akosta.BusinessLogic.Core.Interfaces;
using Akosta.BusinessLogic.Core.Models;
using Share.Exceptions;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Akosta.API.Controllers
{
    /// <summary>
    /// User's controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UsersController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        /// <summary>
        /// Регистрирует пользователя в приложение и возвращает информацию о нём
        /// </summary>
        /// <param name="Telegram">Telegram пользователя</param>
        /// <param name="Name">Ник пользователя</param>
        /// <param name="Surname">Фамилия пользователя</param>
        /// <param name="FirstPassword">Первый пароль</param>
        /// <param name="SecondPassword">Второй пароль</param>
        /// <param name="IsWorker">Работник</param>
        /// <param name="Skill">Какая сфера деятельности</param>

        [ProducesResponseType(typeof(UserInformationDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPost("Registration")]
        public async Task<ActionResult<UserInformationDto>> RegistrationUser(UserRegistrDto userRegistrDto)
        {
            UserRegistrBlo userRegistrBlo = _mapper.Map<UserRegistrBlo>(userRegistrDto);
            UserInformationBlo userInformationBlo;

            try
            {
                userInformationBlo = await _userService.RegistrationUser(userRegistrBlo);
            }
            catch (BadRequestException e)
            {
                return BadRequest(e.Message);
            }

            return Created("", ConvertToUserInformationDto(userInformationBlo));
        }

        /// <summary>
        /// Аутентифицирует пользователя в приложение и возвращает информацию о нём
        /// </summary>
        /// <param name="Telegram">Telegram пользователя</param>
        /// <param name="Password">Пароль</param>
        [ProducesResponseType(typeof(UserInformationDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPost("Authentication")]
        public async Task<ActionResult<UserInformationDto>> AuthenticationUser(UserIdentityDto userIdentityDto)
        {
            UserIdentityBlo userIdentityBlo = _mapper.Map<UserIdentityBlo>(userIdentityDto);
            UserInformationBlo userInformationBlo;

            try
            {
                userInformationBlo = await _userService.AuthenticationUser(userIdentityBlo);
            }
            catch (BadRequestException e)
            {
                return BadRequest(e.Message);
            }

            return Ok(ConvertToUserInformationDto(userInformationBlo));
        }

        /// <summary>
        /// Возвращает информацию о пользователе приложения
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        [ProducesResponseType(typeof(UserInformationDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("[action]/{userId}")]
        public async Task<ActionResult<UserInformationDto>> GetUser(int userId)
        {
            UserInformationBlo userInformationBlo;

            try
            {
                userInformationBlo = await _userService.GetUser(userId);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }

            return Ok(ConvertToUserInformationDto(userInformationBlo));
        }

        /// <summary>
        /// Возвращает всех пользователей по фильтрам
        /// </summary>
        /// <param name="critetia">Кретерии через пробел</param>
        [ProducesResponseType(typeof(List<UserInformationDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpPost("[action]")]
        public async Task<ActionResult<List<UserInformationDto>>> GetAllUserOfCriteria(UserCritetiaDto userCritetiaDto)
        {
            UserCritetiaBlo userCritetiaBlo = _mapper.Map<UserCritetiaBlo>(userCritetiaDto);
            List<UserInformationBlo> userInformationBlos = new List<UserInformationBlo>();

            try
            {
                userInformationBlos = await _userService.GetAllUserOfCriteria(userCritetiaBlo);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }

            return Ok(ConvertToListUserInformationDto(userInformationBlos));
        }

        /// <summary>
        /// Проверяет, существует ли пользователь с указанным id
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [HttpGet("[action]/{userId}")]
        public async Task<ActionResult<bool>> DoesExistUser(int userId)
        {
            return Ok(await _userService.DoesExistUser(userId));
        }

        /// <summary>
        /// Возвращает получилось ли удалить юзера с указанным id или нет
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpDelete("[action]/{userId}")]
        public async Task<ActionResult<bool>> DeleteUser(int userId)
        {
            try
            {
                return Ok(await _userService.DeleteUser(userId));
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }


        private UserInformationDto ConvertToUserInformationDto(UserInformationBlo userInformationBlo)
        {
            if (userInformationBlo == null)
                throw new ArgumentNullException(nameof(userInformationBlo));

            UserInformationDto userInformationDto = _mapper.Map<UserInformationDto>(userInformationBlo);
            return userInformationDto;
        }

        private List<UserInformationDto> ConvertToListUserInformationDto(List<UserInformationBlo> userInformationBlos)
        {
            if (userInformationBlos == null)
                throw new ArgumentNullException(nameof(userInformationBlos));

            List<UserInformationDto> userInformationDtos = new List<UserInformationDto>();
            for (int i = 0; i < userInformationBlos.Count; i++)
            {
                userInformationDtos.Add(ConvertToUserInformationDto(userInformationBlos[i]));
            }
            return userInformationDtos;
        }
    }
}
