using NergizQuiz.MVVM;
using NergizQuiz.Logic;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System;

namespace NergizQuiz.UI
{
    /// <summary>
    /// This is the Facade of a Person object.
    /// It wraps all the properties of the objet to
    /// be MVVM-friendly.
    /// </summary>
    class PersonFacade : ObservableObject, IDataErrorInfo
    {

        #region Fields
        private Person person = new Person();
        #endregion

        #region Public Properties
        public string Name
        {
            get { return person.Name; }
            set
            {
                if (value != person.Name)
                {
                    person.Name = value;
                    RaisePropertyChanged("Name");
                    RaisePropertyChanged("FirstName");
                }
            }
        }
        public string FirstName
        {
            get { return person.FirstName; }
        }
        public float Accuracy
        {
            get { return person.Accuracy; }
            set
            {
                if (value != person.Accuracy)
                {
                    if (float.IsNaN(person.Accuracy))
                        person.Accuracy = 1f;

                    person.Accuracy = value;
                    RaisePropertyChanged("Accuracy");
                    RaisePropertyChanged("Level");
                    RaisePropertyChanged("Comment");
                }
            }
        }
        public int Rank
        {
            get { return person.Rank; }
            set
            {
                if (value != person.Rank)
                {
                    person.Rank = value;
                    RaisePropertyChanged("Rank");
                }
            }
        }
        public int Time
        {
            get { return person.Time; }
            set
            {
                if (value != person.Time)
                {
                    System.Diagnostics.Debug.WriteLine(value);
                    person.Time = value;
                    RaisePropertyChanged("Time");
                    RaisePropertyChanged("TimeForHumans");
                }
            }
        }
        public int Age
        {
            get { return person.Age; }
            set
            {
                if (value != person.Age)
                {
                    person.Age = value;
                    RaisePropertyChanged("Age");
                }
            }
        }
        public bool IsMale
        {
            get { return person.IsMale; }
            set
            {
                if (value != person.IsMale)
                {
                    person.IsMale = value;
                    RaisePropertyChanged("IsMale");
                }
            }
        }
        public string TimeForHumans
        {
            get
            {
                return person.TimeForHumans;
            }
        }
        public string Level
        {
            get { return person.Level; }
        }
        public string Comment
        {
            get { return person.Comment; }
        }
        #endregion

        #region Public Methods
        public Person GetPerson()
        {
            return person;
        }
        #endregion

        #region IDataErrorInfo members
        // accept a-z, space and dot(.)
        private Regex nameRegex = new Regex("^[a-zA-Z\\s.]+$");
        private string error = string.Empty;
        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string this[string columnName]
        {
            get
            {
                error = string.Empty;
                if (columnName == "Name")
                    error = ValidateName();

                return error;
            }
        }

        private string ValidateName()
        {
            if (Name.Length < 1)
                return Strings.NameEmptyError;
            else if (!nameRegex.IsMatch(Name))
                return Strings.NameInvalidCharacterError;
            else if (Name.Length < 3)
                return Strings.NameMinLetters;
            else return string.Empty;
        }
        #endregion
    }
}
