using StudentGrade.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentGrade.Application.Interfaces.IRepositories
{
    public interface IAssessmentRepository
    {
        Task<Assessment?> GetByCodeAndSubjectAsync(string code, Guid subjectId);
        Task<List<Assessment>> GetBySubjectIdAsync(Guid subjectId);
        Task AddAsync(Assessment assessment);
    }
}
