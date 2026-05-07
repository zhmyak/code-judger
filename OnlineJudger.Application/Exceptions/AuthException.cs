using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineJudger.Application.Exceptions
{
    public class AuthException : Exception
    {
        public AuthException(string message) : base(message)
        {
        }
    }
    public class InvalidLoginException : AuthException
    {
        public InvalidLoginException() : base("Неверный логин")
        {
        }
    }
    public class EmailIsAlreadyUsedException : AuthException
    {
        public EmailIsAlreadyUsedException() : base("Пользователь с таким email уже существует")
        {
        }
    }
    public class InvalidPasswordException : AuthException
    {
        public InvalidPasswordException() : base("Неверный пароль")
        {

        }
    }
    public class UserNameIsAlreadyUsedException : AuthException
    {
        public UserNameIsAlreadyUsedException() : base("Пользователь с таким именем уже существует")
        {
        }
    }
}
