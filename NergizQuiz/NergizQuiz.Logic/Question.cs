using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace NergizQuiz.Logic
{
    public class Question
    {
        #region Construction
        public Question(XElement data, int level)
        {

            if (data == null)
                throw new ArgumentNullException("data");

            AllAnswers = new List<Answer>();
            Title = HelperMethods.ApplyMeaningfulChars(HelperMethods.SelectXElement("title", data).Value);
            
            int index = 0;
            foreach (XElement answer in HelperMethods.SelectXElement("answers", data).Descendants())
            {
                var newAnswer = new Answer(answer.Value, index);

                if (HelperMethods.SelectXAttribute("correct", answer) != null &&
                    HelperMethods.SelectXAttribute("correct", answer).Value.ToLower() == "true")
                    newAnswer.IsTheCorrectAnswer = true;

                AllAnswers.Add(newAnswer);

                index++;
            }

            Level = level;
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
        public int UserAnswer
        {
            get
            {
                foreach (var ans in AllAnswers)
                {
                    if (ans.IsChosenByUser)
                        return ans.Index;
                }

                return -1;
            }
        }
        public int Level { get; set; }
        #endregion // Public Properties

        #region Publi Overrided Methods
        public override string ToString()
        {
            return Title;
        }
        #endregion
    }
}
