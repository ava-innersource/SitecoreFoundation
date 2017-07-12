using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.LoginSample
{
    public class RegisterViewModel
    {
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string QuestionText { get; set; }
        public string AnswerText { get; set; }

        public List<string> Questions { get; set; }
    }
}