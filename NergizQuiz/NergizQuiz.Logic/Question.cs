using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace NergizQuiz.Logic
{
    public class Question
    {
        #region Construction
        public Question(XElement data)
        {

            if (data == null)
                throw new ArgumentNullException("data");

            //TODO: attribute and tag names should not be case sensitive
            AllAnswers = new List<Answer>();
            Title = data.Element("Title").Value;

            int index = 0;
            foreach (XElement answer in data.Elements("Answers").Descendants())
            {
                var newAnswer = new Answer(answer.Value, index);

                if (answer.Attribute("correct") != null &&
                    answer.Attribute("correct").Value == "true")
                    newAnswer.IsTheCorrectAnswer = true;

                AllAnswers.Add(newAnswer);

                index++;
            }
        }
        #endregion // Construction

        #region Public Properties
        public string Title { get; set; }
        public List<Answer> AllAnswers { get; set; }
        public int CorrectAnswer
        {
            get
            {
                foreach (var ans in AllAnswers)
                {
                    if (ans.IsTheCorrectAnswer)
                        return ans.Index;
                }

                throw new Exception("No answer is marked as correct.");
            }

        }
        public string Index { get; set; }
        #endregion // Public Properties
    }
}
