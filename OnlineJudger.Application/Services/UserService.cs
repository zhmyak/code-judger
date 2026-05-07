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

namespace OnlineJudger.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly EncryptionService _encryptionService;
        private readonly JwtProvider _jwtProvider;
        private readonly IUnitOfWork _unitOfWork;
        public UserService(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            EncryptionService encryptionService,
            JwtProvider jwtProvider
            )
        {
            _userRepository = userRepository;
            _encryptionService = encryptionService;
            _jwtProvider = jwtProvider;
            _unitOfWork = unitOfWork;
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
    }
}
