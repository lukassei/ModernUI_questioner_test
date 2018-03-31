using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernUI_questioner_test
{
    class Question
    {
        public string QuestionTask { get; set; }
        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }
        public int ChoosenAnswer { get; set; }
        public int CorrectAnswer { get; set; }

        public Question(string _questionTask, string _answer1, string _answer2, string _answer3, int _correctAnswer)
        {
            QuestionTask = _questionTask;
            Answer1 = _answer1;
            Answer2 = _answer2;
            Answer3 = _answer3;
            CorrectAnswer = _correctAnswer;
        }
        public bool Evaluate()
        {
            if (CorrectAnswer == ChoosenAnswer)
                return true;
            else
                return false;
        }
        public override string ToString()
        {
            return QuestionTask;
        }
    }
}
