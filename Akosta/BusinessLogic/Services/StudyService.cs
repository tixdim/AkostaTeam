using Akosta.BusinessLogic.Core.Interfaces;
using Akosta.BusinessLogic.Core.Models;
using Akosta.DataAccess.Core.Interfaces.DBContext;
using Akosta.DataAccess.Core.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Share.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Akosta.BusinessLogic.Services
{
    public class StudyService : IStudyService
    {
        private readonly IMapper _mapper;
        private readonly IDbContext _context;
        public StudyService(IMapper mapper, IDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<StudyInformationBlo> AddStudy(StudyAddBlo studyAddBlo)
        {
            if (studyAddBlo == null)
                throw new ArgumentNullException(nameof(studyAddBlo));


            UserRto userRto = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == studyAddBlo.UserId);

            if (userRto == null) 
                throw new NotFoundException($"Пользователя с id {studyAddBlo.UserId} нет");

            StudyRto study = new StudyRto()
            {
                UserId = studyAddBlo.UserId,
                SkillsInCource = studyAddBlo.SkillsInCource,
                Store = studyAddBlo.Store
            };

            _context.Studys.Add(study);
            await _context.SaveChangesAsync();

            study.User = userRto;
            return ConvertToStudyInfoBlo(study);
        }

        public async Task<StudyInformationBlo> UpdateStudy(int studyId, int store)
        {
            StudyRto study = await _context.Studys.FirstOrDefaultAsync(x => x.Id == studyId);
            if (study == null) throw new NotFoundException($"Тренировки с id {studyId} нет");

            study.Store = store;

            await _context.SaveChangesAsync();

            return ConvertToStudyInfoBlo(study);
        }

        public async Task<StudyInformationBlo> GetStudy(int studyId)
        {
            StudyRto study = await _context.Studys.FirstOrDefaultAsync(x => x.Id == studyId);

            if (study == null) throw new NotFoundException($"Тренировки с id {studyId} нет");

            return ConvertToStudyInfoBlo(study);
        }

        public async Task<List<StudyInformationBlo>> GetAllStudy(int userId, int count, int skipCount)
        {
            bool doesExsist = await _context.Users
                .AnyAsync(x => x.Id == userId);

            if (doesExsist == false)
                throw new NotFoundException($"Пользователя с id {userId} нет");

            List<StudyRto> studys = await _context.Studys
                .Where(e => e.UserId == userId)
                .Skip(skipCount)
                .Take(count)
                .ToListAsync();

            if (studys.Count == 0)
                throw new NotFoundException($"У пользователя с id {userId} нет тренировок");

            studys.Reverse();

            return ConvertToStudyInfoBloList(studys);
        }

        public async Task<bool> DoesExistStudy(int studyId)
        {
            bool result = await _context.Studys.AnyAsync(y => y.Id == studyId);
            return result;
        }

        public async Task<bool> DeleteStudy(int studyId)
        {
            bool result = await _context.Studys.AnyAsync(y => y.Id == studyId);
            if (result == false) throw new NotFoundException($"Тренировки с id {studyId} нет");

            var studyWhoIsDelete = await _context.Studys
                .FindAsync(studyId);

            if (studyWhoIsDelete != null)
            {
                _context.Studys.Remove(studyWhoIsDelete);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteAllStudy(int userId)
        {
            bool doesExsist = await _context.Users
                .AnyAsync(x => x.Id == userId);

            if (doesExsist == false)
                throw new NotFoundException($"Пользователя с id {userId} нет");

            List<StudyRto> studys = await _context.Studys
                .Where(e => e.UserId == userId)
                .ToListAsync();

            if (studys.Count == 0 || studys == null)
                throw new NotFoundException($"У пользователя с id {userId} нет тренировок");

            foreach (StudyRto study in studys)
            {
                _context.Studys.Remove(study);
            }

            await _context.SaveChangesAsync();

            List<StudyRto> newStudys = await _context.Studys
                .Where(e => e.UserId == userId)
                .ToListAsync();

            if (newStudys.Count == 0 || newStudys == null)
                return true;

            return false;
        }


        private List<StudyInformationBlo> ConvertToStudyInfoBloList(List<StudyRto> studyRtos)
        {
            if (studyRtos == null)
                throw new ArgumentNullException(nameof(studyRtos));

            List<StudyInformationBlo> studyInformationBlos = new List<StudyInformationBlo>();
            for (int i = 0; i < studyRtos.Count; i++)
            {
                studyInformationBlos.Add(_mapper.Map<StudyInformationBlo>(studyRtos[i]));
            }

            return studyInformationBlos;
        }

        private StudyInformationBlo ConvertToStudyInfoBlo(StudyRto studyRto)
        {
            if (studyRto == null) throw new ArgumentNullException(nameof(studyRto));

            StudyInformationBlo studyInformationBlo = _mapper.Map<StudyInformationBlo>(studyRto);

            return studyInformationBlo;
        }
    }
}
