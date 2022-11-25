using Akosta.BusinessLogic.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Akosta.BusinessLogic.Core.Interfaces
{
    public interface IStudyService
    {
        Task<StudyInformationBlo> AddStudy(StudyAddBlo studyAddBlo);
        Task<StudyInformationBlo> UpdateStudy(int studyId, int store);
        Task<StudyInformationBlo> GetStudy(int studyId);
        Task<List<StudyInformationBlo>> GetAllStudy(int userId, int count, int skipCount);
        Task<bool> DoesExistStudy(int studyId);
        Task<bool> DeleteStudy(int studyId);
        Task<bool> DeleteAllStudy(int userId);
    }
}
