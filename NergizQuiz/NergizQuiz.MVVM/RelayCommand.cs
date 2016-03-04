using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
namespace NergizQuiz.MVVM
{
    /// <summary>
    /// A command whose only purpose is to relay functionality to other
    /// objects by invoking delegates. The default value for CanExecute
    /// method is 'true'.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RelayCommand<T> : ICommand
    {
        #region Fields

        Predicate<T> _canExecute;
        Action<T> _execute;

        #endregion

        #region Constructor(s)
        /// <summary>
        /// Initializes a new instance of <see cref="RelayCommand&lt;T&gt;"/>
        /// class that can always be executed.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public RelayCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="RelayCommand&lt;T&gt;"/> class
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
        }
        #endregion

        #region ICommand members

        bool ICommand.CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute((T)parameter);
        }

        event EventHandler ICommand.CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        void ICommand.Execute(object parameter)
        {
            _execute((T)parameter);
        }

        #endregion

    }


    /// <summary>
    /// A command whose only purpose is to relay functionality to other
    /// objects by invoking delegates. The default value for CanExecute
    /// method is 'true'.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RelayCommand : ICommand
    {
        #region Fields

        Func<Boolean> _canExecute;
        Action _execute;

        #endregion

        #region Constructor(s)
        /// <summary>
        /// Initializes a new instance of <see cref="RelayCommand&lt;T&gt;"/>
        /// class that can always be executed.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public RelayCommand(Action execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="RelayCommand&lt;T&gt;"/> class
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action execute, Func<Boolean> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
        }
        #endregion

        #region ICommand members

        bool ICommand.CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute();
        }

        event EventHandler ICommand.CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        void ICommand.Execute(object parameter)
        {
            _execute();
        }

        #endregion

    }
}
