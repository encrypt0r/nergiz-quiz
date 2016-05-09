using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Net;
using System.Diagnostics;

namespace NergizQuiz.Logic
{
    public static class DataLayer
    {
        #region Fields
        public static bool IS_DEBUGGING = false;

        private static Random randomGenerator;
        private static List<Question> listOfQuestions;
        public const string API_PASSWORD = "HowAreYou?";
        public const string API_INSERT = "insert_person";
        public const string LOCALHOST_URL = "http://localhost/nergiz-quiz-web/";
        public const string API_URL = "/api/v1/api.php";
        public const string SITE_URL = "http://nergiz-quiz.ueuo.com/";
        public const int NUMBER_OF_LEVELS = 5;
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
            NumberOfQuestions = 10;
            randomGenerator = new Random();
            listOfQuestions = new List<Question>();
            LoadQuestions();
        }
        #endregion // Construction

        #region Public Methods
        static public string GetComment(float accuracy)
        {
            int level = HelperMethods.GetLevel(accuracy);

            switch (level)
            {
                default:
                    return level1[randomGenerator.Next(0, level1.Length)];
                case 2:
                    return level2[randomGenerator.Next(0, level2.Length)];
                case 3:
                    return level3[randomGenerator.Next(0, level3.Length)];
                case 4:
                    return level4[randomGenerator.Next(0, level4.Length)];
                case 5:
                    return level5[randomGenerator.Next(0, level5.Length)];
            }
        }
        static public void SendPersonToDatabase(Person cp, UploadValuesCompletedEventHandler callback)
        {
            var nvc = new System.Collections.Specialized.NameValueCollection();
            nvc.Add("name", cp.Name);
            nvc.Add("accuracy", cp.Accuracy.ToString());
            nvc.Add("time", cp.Time.ToString());
            nvc.Add("action", API_INSERT);
            nvc.Add("password", API_PASSWORD);
            nvc.Add("gender", cp.IsMale.ToString());
            nvc.Add("age", cp.Age.ToString());

            var wb = new WebClient();
            wb.Headers.Add("user-agent", "Nergiz Quiz Desktop Client");
            wb.Headers.Add("content-type", "application/x-www-form-urlencoded");
            string apiPath;

            if (IS_DEBUGGING)
                apiPath = LOCALHOST_URL + API_URL;
            else
                apiPath = SITE_URL + API_URL;

            wb.UploadValuesAsync(new Uri(apiPath), "POST", nvc);
            wb.UploadValuesCompleted += callback;
        }
        static public ObservableCollection<Person> ParseLeaderboard(string data)
        {
            var leaderboard = new ObservableCollection<Person>();
            char[] seperators = { '|' };
            string[] dataParts = data.Split(seperators, StringSplitOptions.RemoveEmptyEntries);

            foreach (string part in dataParts)
            {
                string[] properties = part.Split(',');
                var person = new Person();

                person.Rank = int.Parse(properties[0]);
                person.Name = properties[1];
                person.Accuracy = float.Parse(properties[2]);
                person.Time = int.Parse(properties[3]);

                leaderboard.Add(person);
            }

            return leaderboard;
        }
        static public List<Question> GetNewListOfQuestions(int max)
        {
            var returnList = new List<Question>(max);

            for (int i = 1; i <= NUMBER_OF_LEVELS; i++)
            {
                int numQuestionsForThisLevel = max / NUMBER_OF_LEVELS;
                int extraQuestions = max % NUMBER_OF_LEVELS;
                if (i <= extraQuestions && max % NUMBER_OF_LEVELS != 0)
                {
                    numQuestionsForThisLevel++;
                }

                var list = GetLevel(i, numQuestionsForThisLevel);
                returnList.AddRange(list);
            }
            returnList.Shuffle();

            // reset the questions, because they are passed by reference
            foreach (var q in returnList)
                foreach (var a in q.AllAnswers)
                    a.IsChosenByUser = false;

            return returnList;
        }
        #endregion // Public Methods

        #region Public Properties
        public static int NumberOfQuestions { get; set; }
        #endregion

        #region Private Methods
        private static void LoadQuestions()
        {
            listOfQuestions.Clear();
            for (int i = 1; i <= NUMBER_OF_LEVELS; i++)
            {
                List<XElement> level_i_Questions = new List<XElement>();
                XElement data = XElement.Load("Data\\Level" + i + ".xml");
                level_i_Questions = data.Elements().ToList();

                foreach (var item in level_i_Questions)
                {
                    var q = new Question(item, i);
                    listOfQuestions.Add(q);
                }
            }
        }
        private static int GetNewRandNum(int[] oldNums, int max)
        {
            int randomNumber;
            do
            {
                randomNumber = randomGenerator.Next(0, max);
            }
            while (oldNums.Contains(randomNumber));

            return randomNumber;
        }
        private static List<Question> GetLevel(int level, int max)
        {
            List<Question> returnList = new List<Question>();

            if (listOfQuestions.Count <= 0)
                LoadQuestions();

            // This gets all of the questions of this level in the database
            var database = listOfQuestions.Where(q => q.Level == level).ToList();

            if (database.Count() < max)
                throw new ArgumentException("There are not enough questions of level " + level);

            // Get max Random Questions from database
            int[] oldNums = new int[max];
            for (int i = 0; i < max; i++)
            {
                oldNums[i] = -1; // so that GetRandomNumber() does not get stuck in the loop
                int randomNumber = GetNewRandNum(oldNums, database.Count);
                Question q = database[randomNumber];
                returnList.Add(q);
                oldNums[i] = randomNumber;
            }

            return returnList;
        }
        /// <summary>
        /// Shuffles a list pseudo-randomly. 
        /// Credit: Eric J. (http://stackoverflow.com/questions/273313/randomize-a-listt-in-c-sharp)
        /// </summary>
        /// <typeparam name="T">Type of the items in the list</typeparam>
        /// <param name="list">The list to be shuffled</param>
        private static void Shuffle<T>(this List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = randomGenerator.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        #endregion // Private Methods

    }
}
