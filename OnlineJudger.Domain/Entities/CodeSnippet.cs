using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineJudger.Domain.Entities
{
    public class CodeSnippet
    {
        public int ProblemId { get; set; }
        public int LanguageId { get; set; }
        public string Code {  get; set; } = string.Empty;

        public Problem Problem { get; set; } = null!;
        public Language Language { get; set; } = null!;
    }
}
