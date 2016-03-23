using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Net;

namespace NergizQuiz.Logic
{
    public static class DataLayer
    {
        #region Fields
        private static Random randomGenerator;
        private static List<XElement> listOfQuestions;
        public const string API_PASSWORD =  "Pass";
        public const string API_INSERT = "insert";
        public const string API_PATH = SITE_URL + "api.php";
        public const string SITE_URL = "http://localhost/nergiz-quiz-web/";
        public const string LEADERBOARD_URL = SITE_URL;

        // comments
        private static string[] level1 = { "Needs More Work", "Unsatsifactory" };
        private static string[] level2 = { "Good", "Satisfactory" };
        private static string[] level3 = { "Very Good!", "Well done!", "Good Job!" };
        private static string[] level4 = { "Excellent", "Fantastic", "Superb" };
        private static string[] level5 = { "Incredible!", "Marvelous", "Legendary" };
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
                cp.TimeElapsed = int.Parse(person.Element("DeciSecondsElapsed").Value);

                list.Add(cp);
            }
            List<CoolPerson> sortedList = list.OrderByDescending(p => p.Accuracy).ThenBy(p => p.TimeElapsed).ToList();

            for (int i = 1; i <= sortedList.Count; i++)
            {
                sortedList[i - 1].Index = i;
            }

            return sortedList;
        }
        static public string GetComment(float accuracy)
        {
            int level = HelperMethods.GetLevel(accuracy);

            switch (level)
            {
                default:
                    return level1[randomGenerator.Next(0, level1.Length)];
                case 2:
                    return level2[randomGenerator.Next(0, level2.Length )];
                case 3:
                    return level3[randomGenerator.Next(0, level3.Length)];
                case 4:
                    return level4[randomGenerator.Next(0, level4.Length)];
                case 5:
                    return level5[randomGenerator.Next(0, level5.Length)];
            }
        }
        static public void UploadPersonIntoLeaderboard(CoolPerson cp, UploadValuesCompletedEventHandler callback)
        {
            var nvc = new System.Collections.Specialized.NameValueCollection();
            nvc.Add("name", cp.Name);
            nvc.Add("accuracy", cp.Accuracy.ToString());
            nvc.Add("time", cp.TimeElapsed.ToString());
            nvc.Add("operation", API_INSERT);
            nvc.Add("password", API_PASSWORD);

            var wb = new WebClient();
            wb.Headers.Add("user-agent", "Nergiz Quiz Desktop Client");
            wb.Headers.Add("content-type", "application/x-www-form-urlencoded");

            wb.UploadValuesAsync(new Uri(API_PATH), "POST", nvc);
            wb.UploadValuesCompleted += callback;
        }
        #endregion // Public Methods

        #region Private Methods
        private static void LoadQuestions()
        {
            listOfQuestions.Clear();
            XElement data = XElement.Load("Data\\Questions.xml");
            listOfQuestions = data.Elements().ToList();
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
                XElement timex = new XElement("DeciSecondsElapsed", cp.TimeElapsed);

                cpx.Add(namex);
                cpx.Add(accuracyx);
                cpx.Add(timex);

                leaderboardx.Add(cpx);
            }

            leaderboardx.Save("Data\\Leaderboard.xml");
        }
        #endregion // Private Methods

    }
}
