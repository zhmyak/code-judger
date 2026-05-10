using OnlineJudger.Domain.Stores;
using OnlineJudger.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineJudger.Application.DTOs;
using OnlineJudger.Application.Exceptions;

namespace OnlineJudger.Application.Services
{
    public class ProblemService
    {
        private readonly ISubmissionRepository _submissionRepository;
        private readonly IProblemRepository _problemRepository;

        public ProblemService(IProblemRepository problemRepository, ISubmissionRepository submissionRepository)
        {
            _problemRepository = problemRepository;
            _submissionRepository = submissionRepository;
        }

        public async Task<List<ProblemDTO>> GetAllByUserIdAsync(int userId)
        {
            var problems = await _problemRepository.GetAllAsync();
            var result = new List<ProblemDTO>();
            foreach(var problem in problems)
            {
                var isSolved = await IsProblemSolvedByUserAsync(userId, problem.Id);
                result.Add(new ProblemDTO
                {
                    Id = problem.Id,
                    Title = problem.Title,
                    Description = problem.Description,
                    Difficulty = problem.Difficulty.ToString(),
                    IsSolved = isSolved
                });
            }
            return result;
        }

        public async Task<List<ProblemDTO>> GetAllSolvedByUserIdAsync(int userId)
        {
            var problems = await _problemRepository.GetAllAsync();
            var result = new List<ProblemDTO>();
            foreach (var problem in problems)
            {
                var isSolved = await IsProblemSolvedByUserAsync(userId, problem.Id);
                if (isSolved)
                {
                    result.Add(new ProblemDTO
                    {
                        Id = problem.Id,
                        Title = problem.Title,
                        Description = problem.Description,
                        Difficulty = problem.Difficulty.ToString(),
                        IsSolved = isSolved
                    });
                }
            }
            return result;
        }
        public async Task<ProblemDTO> GetByIdAsync(int id, int userId)
        {
            var problem = await _problemRepository.GetByIdAsync(id);
            if (problem == null)
            {
                throw new ProblemNotFoundException();
            }
            var result = new ProblemDTO
            {
                Id = problem.Id,
                Title = problem.Title,
                Description = problem.Description,
                Difficulty = problem.Difficulty.ToString(),
                IsSolved = await IsProblemSolvedByUserAsync(userId, problem.Id)
            };
            return result;
        }
        public async Task<bool> IsProblemSolvedByUserAsync(int userId, int problemId)
        {
            var submissions = await _submissionRepository.GetAllByUserIdAsync(userId);
            return submissions.Any(s => s.ProblemId == problemId && s.Status == Domain.Enums.SubmissionStatus.Accepted);
        }
    }
}
