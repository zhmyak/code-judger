using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineJudger.Application.Exceptions
{
    public class NotFoundException: Exception
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }
    public class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException() : base("Пользователь не найден")
        {
        }
    }
    public class ProblemNotFoundException : NotFoundException
    {
        public ProblemNotFoundException() : base("Задача не найдена")
        {
        }
    }
}
