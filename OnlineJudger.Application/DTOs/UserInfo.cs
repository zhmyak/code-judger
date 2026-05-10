using OnlineJudger.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineJudger.Application.DTOs
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int AllSolvedProblems {  get; set; }
        public int EasySolvedProblems { get; set; }
        public int MediumSolvedProblems { get; set; }
        public int HardSolvedProblems { get; set; }
        public DateTime LastSubmitted {  get; set; }
        
        public int SubmittedLastWeek {  get; set; }
        public int SubmittedLastMonth {  get; set; }
        public int SubmittedLastYear {  get; set; }
        public int Points {  get; set; }
        public int TopPlace {  get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
