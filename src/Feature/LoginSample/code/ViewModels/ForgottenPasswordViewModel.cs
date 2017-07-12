using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.LoginSample
{
    public class ForgottenPasswordViewModel
    {
        public string UserName { get; set; }
        public string AnswerText { get; set; }
        public string QuestionText { get; set; }
        public bool AnswerProvided { get; set; }
        public string NewPassword { get; set; }
    }
}