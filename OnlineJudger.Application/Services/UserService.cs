using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineJudger.Domain.Stores;
using OnlineJudger.Domain.Entities;
using OnlineJudger.Application.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using OnlineJudger.Application.Exceptions;
using System.Numerics;
using OnlineJudger.Application.DTOs;
using Microsoft.AspNetCore.SignalR;

namespace OnlineJudger.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISubmissionRepository _submissionRepository;
        private readonly EncryptionService _encryptionService;
        private readonly JwtProvider _jwtProvider;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ProblemService _problemService;
        public UserService(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            EncryptionService encryptionService,
            JwtProvider jwtProvider,
            ProblemService problemService,
            ISubmissionRepository submissionRepository
            )
        {
            _userRepository = userRepository;
            _encryptionService = encryptionService;
            _jwtProvider = jwtProvider;
            _unitOfWork = unitOfWork;
            _problemService = problemService;
            _submissionRepository = submissionRepository;
        }
        public async Task<string> RegistrUser(string username, string password, string email)
        {
            if(await _userRepository.GetByUsernameAsync(username) != null)
            {
                throw new UserNameIsAlreadyUsedException();
            }
            if(await _userRepository.GetByEmailAsync(email) != null)
            {
                throw new EmailIsAlreadyUsedException();
            }
            var user = new User
            {
                Username = username,
                PasswordHash = _encryptionService.Encrypt(password),
                Email = email,
                CreatedAt = DateTime.UtcNow
            };
            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
            string accessToken = _jwtProvider.GenerateAccessToken(user);
            return accessToken;
        }
        public async Task<User> GetUserInfoById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if(user == null)
            {
                throw new UserNotFoundException();
            }
            return user;
        }
        public async Task<string> AuthUser(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null)
            {
                throw new InvalidLoginException();
            }
            if(user.PasswordHash != _encryptionService.Encrypt(password))
            {
                throw new InvalidPasswordException();
            }
            string accessToken = _jwtProvider.GenerateAccessToken(user);
            return accessToken;
        }
        public async Task<List<UserDTO>> GetTopList()
        {
            var users = await _userRepository.GetAllOrderByPointsDesc();
            var topList = new List<UserDTO>();
            for(int i  = 0; i < users.Count; i++)
            {
                topList.Add(new UserDTO
                {
                    Id = users[i].Id,
                    Username = users[i].Username,
                    Points = users[i].Points,
                    PlaceInTop = i + 1
                });
            }
            return topList;
        }
        public async Task<UserInfo> GetUserInfo(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new UserNotFoundException();
            }
            var solvedProblems = await _problemService.GetAllSolvedByUserIdAsync(userId);
            var easySolvedProblems = solvedProblems.Where(p => p.Difficulty == Domain.Enums.Difficulty.Easy.ToString());
            var mediumSolvedProblems = solvedProblems.Where(p => p.Difficulty == Domain.Enums.Difficulty.Medium.ToString());
            var hardSolvedProblems = solvedProblems.Where(p => p.Difficulty == Domain.Enums.Difficulty.Hard.ToString());
            var submissions = await _submissionRepository.GetAllByUserIdAsync(userId);
            var lastSubmission = submissions.LastOrDefault();
            var submissionsOfLastYear = submissions.Where(s => s.CreatedAt >= DateTime.Now.AddMonths(-12));
            var submissionsOfLastMonth = submissions.Where(s => s.CreatedAt >= DateTime.Now.AddDays(-30));
            var submissionsOfLastWeek = submissions.Where(s => s.CreatedAt >= DateTime.Now.AddDays(-7));
            var topList = await GetTopList();
            var userInTop = topList.FirstOrDefault(u => u.Id == userId);
            if (userInTop == null)
            {
                throw new UserNotFoundException();
            }
            return new UserInfo
            {
                Id = userId,
                Username = user.Username,
                Email = user.Email,
                AllSolvedProblems = solvedProblems.Count(),
                EasySolvedProblems = easySolvedProblems.Count(),
                MediumSolvedProblems = mediumSolvedProblems.Count(),
                HardSolvedProblems = hardSolvedProblems.Count(),
                LastSubmitted = lastSubmission?.CreatedAt,
                SubmittedLastWeek = submissionsOfLastWeek.Count(),
                SubmittedLastMonth = submissionsOfLastMonth.Count(),
                SubmittedLastYear = submissionsOfLastYear.Count(),
                Points = user.Points,
                PlaceInTop = userInTop.PlaceInTop,
                CreatedAt = user.CreatedAt
            };
        }
    }
}
