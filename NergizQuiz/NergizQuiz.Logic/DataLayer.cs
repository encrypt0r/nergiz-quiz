using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            listOfQuestions = new List<XElement>();
            LoadQuestions();
        }
        #endregion // Construction

        #region Public Methods
        static public XElement GetNextQuestion()
        {
            if (listOfQuestions.Count <= 0)
                LoadQuestions();

            XElement question;
            int randomNumber = randomGenerator.Next(0, listOfQuestions.Count);
            question = listOfQuestions[randomNumber];
            listOfQuestions.RemoveAt(randomNumber);

            return question;
        }
        static public List<CoolPerson> GetLeaderboard()
        {
            var list = new List<CoolPerson>();
            var leaderBoard = XElement.Load("Data\\Leaderboard.xml");
            foreach (var person in leaderBoard.Elements())
            {
                var cp = new CoolPerson();
                cp.Name = person.Element("Name").Value;
                cp.Accuracy = float.Parse(person.Element("Accuracy").Value);
                cp.DeciSecondsElapsed = int.Parse(person.Element("DeciSecondsElapsed").Value);

                list.Add(cp);
            }
            List<CoolPerson> sortedList = list.OrderByDescending(p => p.Accuracy).ThenBy(p => p.DeciSecondsElapsed).ToList();

            for (int i = 1; i <= sortedList.Count; i++)
            {
                sortedList[i - 1].Index = i;
            }

            return sortedList;
        }
        static public void AddToLeaderBoard(CoolPerson cp)
        {
            // check if he is really a cool person
            var leaderboard = GetLeaderboard();

            if (leaderboard.Count < 10)
                leaderboard.Add(cp);
            else
            {
                for (int i = 0; i < leaderboard.Count; i++)
                {
                    if (cp.Accuracy > leaderboard[i].Accuracy)
                    {
                        leaderboard.Insert(i, cp);
                        break;
                    }
                    else if (cp.Accuracy == leaderboard[i].Accuracy)
                    {
                        if (cp.DeciSecondsElapsed < leaderboard[i].DeciSecondsElapsed)
                        {
                            leaderboard.Insert(i, cp);
                            break;
                        }
                        else
                        {
                            leaderboard.Insert(i + 1, cp);
                            break;
                        }
                    }
                }
            }

            WriteListToDataBase(leaderboard);
        }
        private static void WriteListToDataBase(List<CoolPerson> list)
        {
            XElement leaderboardx = new XElement("Leaderboard");

            for (int i = 0, m = list.Count; i < m && i < 10; i++)
            {
                CoolPerson cp = list[i];
                XElement cpx = new XElement("Person");
                XElement namex = new XElement("Name", cp.Name);
                XElement accuracyx = new XElement("Accuracy", cp.Accuracy);
                XElement timex = new XElement("DeciSecondsElapsed", cp.DeciSecondsElapsed);

                cpx.Add(namex);
                cpx.Add(accuracyx);
                cpx.Add(timex);

                leaderboardx.Add(cpx);
            }

            leaderboardx.Save("Data\\Leaderboard.xml");
        }
        #endregion // Public Methods

        #region Private Methods
        private static void LoadQuestions()
        {
            listOfQuestions.Clear();
            XElement data = XElement.Load("Data\\Questions.xml");
            listOfQuestions = data.Elements().ToList();
        }
        #endregion // Private Methods

    }
}
