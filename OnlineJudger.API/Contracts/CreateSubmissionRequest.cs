using System.ComponentModel.DataAnnotations;

namespace OnlineJudger.API.Contracts
{
    public class CreateSubmissionRequest
    {
        [Required]
        public int ProblemId { get; set; }
        [Required]
        public int LanguageId {  get; set; }
        
        public string SourceCode { get; set; } = string.Empty;
    }
}
