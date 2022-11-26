using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Akosta.API.Models;
using Share.Exceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Akosta.BusinessLogic.Core.Interfaces;
using Akosta.BusinessLogic.Core.Models;

namespace Akosta.API.Controllers
{
    /// <summary>
    /// Study's controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class StudysController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IStudyService _studyService;

        public StudysController(IMapper mapper, IStudyService studyService)
        {
            _mapper = mapper;
            _studyService = studyService;
        }

        /// <summary>
        /// Добавляет обучение и возвращает информацию о нём
        /// </summary>
        /// <param name="UserId">Id пользователя, к которому надо добавить обучение</param>
        /// <param name="SkillsInCource">Как называется</param>
        /// <param name="Store">На сколько процентов сделано</param>
        [ProducesResponseType(typeof(StudyInformationDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPost("[action]")]
        public async Task<ActionResult<StudyInformationDto>> AddStudy(StudyAddDto studyAddDto)
        {
            StudyAddBlo studyAddBlo = _mapper.Map<StudyAddBlo>(studyAddDto);
            StudyInformationBlo studyInformationBlo = new StudyInformationBlo();
            try
            {
                studyInformationBlo = await _studyService.AddStudy(studyAddBlo);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (ArgumentNullException e)
            {
                return BadRequest(e.Message);
            }
            catch (BadRequestException e)
            {
                return BadRequest(e.Message);
            }
            return Created("", ConvertToStudyInformationDto(studyInformationBlo));
        }

        /// <summary>
        /// Обновляет рейтинг прохождения курса у юзера
        /// </summary>
        /// <param name="studyId">Идентификатор курса у чела</param>
        /// <param name="store">На сколько процентов юзер прошёл его</param>
        [ProducesResponseType(typeof(StudyInformationDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpPatch("[action]/{studyId}/{store}")]
        public async Task<ActionResult<StudyInformationDto>> UpdateStudy(int studyId, int store)
        {
            StudyInformationBlo studyInformationBlo;

            try
            {
                studyInformationBlo = await _studyService.UpdateStudy(studyId, store);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }

            return Ok(ConvertToStudyInformationDto(studyInformationBlo));

        }

        /// <summary>
        /// Возвращает обучение по id
        /// </summary>
        /// <param name="studyId">Идентификатор обучения</param>
        [ProducesResponseType(typeof(StudyInformationDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("[action]/{studyId}")]
        public async Task<ActionResult<StudyInformationDto>> GetStudy(int studyId)
        {
            StudyInformationBlo studyInformationBlo;

            try
            {
                studyInformationBlo = await _studyService.GetStudy(studyId);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }

            return Ok(ConvertToStudyInformationDto(studyInformationBlo));
        }

        /// <summary>
        /// Возвращает все обучения по id юзера
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="count">Сколько обучений требуется</param>
        /// <param name="skipCount">Сколько обучений уже есть</param>
        [ProducesResponseType(typeof(List<StudyInformationDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("[action]/{userId}/{count}/{skipCount}")]
        public async Task<ActionResult<List<StudyInformationDto>>> GetAllStudy(int userId, int count, int skipCount)
        {
            List<StudyInformationBlo> studyInformationBlos = new List<StudyInformationBlo>();

            try
            {
                studyInformationBlos = await _studyService.GetAllStudy(userId, count, skipCount);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }

            return Ok(ConvertToListWorkoutInformationDto(studyInformationBlos));
        }

        /// <summary>
        /// Проверяет, существует ли обучение с указанным id
        /// </summary>
        /// <param name="studyId">Идентификатор обучения</param>
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [HttpGet("[action]/{studyId}")]
        public async Task<ActionResult<bool>> DoesExistStudy(int studyId)
        {
            return Ok(await _studyService.DoesExistStudy(studyId));
        }

        /// <summary>
        /// Возвращает получилось ли удалить обучение с указанным id или нет
        /// </summary>
        /// <param name="studyId">Идентификатор обучения</param>
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpDelete("[action]/{studyId}")]
        public async Task<ActionResult<bool>> DeleteStudy(int studyId)
        {
            try
            {
                return Ok(await _studyService.DeleteStudy(studyId));
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Возвращает получилось ли удалить все обучения у пользователя с указанным id или нет
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpDelete("[action]/{userId}")]
        public async Task<ActionResult<bool>> DeleteAllStudy(int userId)
        {
            try
            {
                return Ok(await _studyService.DeleteAllStudy(userId));
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }


        private StudyInformationDto ConvertToStudyInformationDto(StudyInformationBlo studyInformationBlo)
        {
            if (studyInformationBlo == null)
                throw new ArgumentNullException(nameof(studyInformationBlo));

            StudyInformationDto studyInformationDto = _mapper.Map<StudyInformationDto>(studyInformationBlo);
            return studyInformationDto;
        }

        private List<StudyInformationDto> ConvertToListWorkoutInformationDto(List<StudyInformationBlo> studyInformationBlos)
        {
            if (studyInformationBlos == null)
                throw new ArgumentNullException(nameof(studyInformationBlos));

            List<StudyInformationDto> studyInformationDtos = new List<StudyInformationDto>();
            for (int i = 0; i < studyInformationBlos.Count; i++)
            {
                studyInformationDtos.Add(ConvertToStudyInformationDto(studyInformationBlos[i]));
            }
            return studyInformationDtos;
        }
    }
}
