using System;
using System.Windows.Input;

namespace GoL.Extras
{
    /// <summary>
    /// Команда.
    /// </summary>
    public class DelegateCommand : ICommand
    {
        #region Поля и свойства

        /// <summary>
        /// Обработчик команды.
        /// </summary>
        public Action<object> ExecuteAction { get; private set; }

        /// <summary>
        /// Обработчик возможности выполнения команды.
        /// </summary>
        public Func<object, bool> CanExecuteFunction { get; private set; }

        #endregion

        #region ICommand

        public void Execute(object parameter)
        {
            this.ExecuteAction?.Invoke(parameter);
        }

        public bool CanExecute(object parameter)
        {
            if (this.CanExecuteFunction != null)
                return this.CanExecuteFunction(parameter);
            else
                return true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion

        #region Конструкторы

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="execute">Обработчик команды.</param>
        /// <param name="canExecute">Обработчик возможности выполнения команды.</param>
        public DelegateCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            this.ExecuteAction = execute;
            this.CanExecuteFunction = canExecute;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="execute">Обработчик команды.</param>
        public DelegateCommand(Action<object> execute) : this(execute, null) { }

        #endregion
    }
}
