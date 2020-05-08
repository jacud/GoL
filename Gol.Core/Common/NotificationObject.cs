using System;
using System.ComponentModel;

namespace Gol.Core.Common
{
    /// <summary>
    /// Базовый класс для элементов с оповещением.
    /// </summary>
    [Serializable]
    public abstract class NotificationObject : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged   

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Методы

        /// <summary>
        /// Сгенерировать событие на изменение свойства.
        /// </summary>
        /// <param name="propertyName">Имя изменённого свойства.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Сгенерировать событие на изменение свойства.
        /// </summary>
        /// <param name="propertyNames">Массив имен изменённых свойств.</param>
        protected void RaisePropertyChanged(params string[] propertyNames)
        {
            if (this.PropertyChanged == null)
            {
                return;
            }

            if (propertyNames == null)
            {
                throw new ArgumentNullException(nameof(propertyNames));
            }
            string[] strArrays = propertyNames;
            foreach (string propertyName in strArrays)
            {
                this.RaisePropertyChanged(propertyName);
            }
        }

        #endregion
    }
}