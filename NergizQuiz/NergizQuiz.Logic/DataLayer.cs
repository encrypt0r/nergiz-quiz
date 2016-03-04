using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace NergizQuiz.Logic
{
    public static class DataLayer
    {
        #region Members
        private static Random randomGenerator;
        private static List<XElement> listOfQuestions;
        #endregion // Members
        #region Construction
        static DataLayer()
        {
            randomGenerator = new Random();
            XElement data = XElement.Load("Data\\Questions.xml");
            
            listOfQuestions = data.Elements().ToList();
        }
        #endregion // Construction
        #region Public Functions
        static public XElement GetNextQuestion()
        {
            XElement question;
            int randomNumber = randomGenerator.Next(0, 3);
            question = listOfQuestions[randomNumber];

            return question;
        }
        #endregion // Public Function
    }
}
