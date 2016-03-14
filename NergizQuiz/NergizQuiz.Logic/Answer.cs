using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NergizQuiz.Logic
{
    public class Answer
    {
        public Answer(string text, int index, bool isTheRightAnswer = false)
        {
            Text = text;
            Index = index;
            IsTheCorrectAnswer = isTheRightAnswer;
        }

        public bool IsTheCorrectAnswer { get; set; }
        public int Index { get; set; }
        public string Text { get; set; }
        public bool IsChosenByUser { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
