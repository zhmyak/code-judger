using Microsoft.AspNetCore.Http;
using OnlineJudger.Application.Exceptions;
using OnlineJudger.Domain.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineJudger.Application.Services
{
    public class CodeService
    {
        private readonly ICodeSnippetRepository _codeSnippetRepository;
        private readonly ISubmissionRepository _submissionRepository;
        public CodeService(ICodeSnippetRepository codeSnippetRepository, ISubmissionRepository submissionRepository)
        {
            _codeSnippetRepository = codeSnippetRepository;
            _submissionRepository = submissionRepository;
        }

        public async Task<string> GetSavedCodeOrTemplate(int problemId, int languageId, int userId)
        {
            var submissions = await _submissionRepository.GetAllByUserAndProblemIdAsync(userId, problemId);   
            var submission = submissions.LastOrDefault(s => s.LanguageId == languageId && s.Status == Domain.Enums.SubmissionStatus.Accepted)
                ?? submissions.LastOrDefault(s => s.LanguageId == languageId);
            if(submission == null)
            {
                var codeSnippet = await _codeSnippetRepository.GetByIdAsync(problemId, languageId);
                if (codeSnippet == null)
                {
                    throw new CodeSnippetNotFoundException();
                }
                return codeSnippet.Code;
            }
            return submission.SourceCode;
        }
    }
}
