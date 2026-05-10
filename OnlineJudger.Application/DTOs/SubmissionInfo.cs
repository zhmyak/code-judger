using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineJudger.Application.DTOs
{
    public class SubmissionInfo
    {
        public string Status { get; set; } = null!;
        public string? ErrorMessage { get; set; }
    }
}
