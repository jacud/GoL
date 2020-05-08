using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Timers;
using Gol.Core.Common;
using Gol.Core.Controls.Models;

namespace Gol.Core.Template
{
    /// <summary>
    /// Шаблонный класс игры.
    /// </summary>
    /// <remarks>Содержит базовые шаги игры. которые могут быть переопределены в наследнике.</remarks>
    public abstract class GameTemplate : NotificationObject
    {
        #region Поля и свойства

        private bool isEvalutionRunning;

        /// <summary>
        /// Признак того идет ли эволция в данный момент.
        /// </summary>
        public bool IsEvalutionRunning
        {
            get
            {
                return this.isEvalutionRunning;
            }

            private set
            {
                this.isEvalutionRunning = value;
                this.OnPropertyChanged(nameof(this.IsEvalutionRunning));
            }
        }

        /// <summary>
        /// Признак того, что текущее поколениие является последним из посчитанных.
        /// </summary>
        public bool IsLastGeneration => this.PreviousGenerations.Count == this.currentGenerationIndex;

        /// <summary>
        /// Признак того, что текущее поколениие является первым из посчитанных.
        /// </summary>
        public bool IsFirstGeneration => this.currentGenerationIndex == 0;

        private SingleGenerationGrid<bool> currentGeneration;

        /// <summary>
        /// Текущее поколение.
        /// </summary>
        public SingleGenerationGrid<bool> CurrentGeneration
        {
            get
            {
                return this.currentGeneration;
            }

            private set
            {
                this.currentGeneration = value;
                this.OnPropertyChanged(nameof(this.CurrentGeneration));
            }
        }

        /// <summary>
        /// Период смены поколений.
        /// </summary>
        protected virtual int СhangeGenerationInterval => 50;

        /// <summary>
        /// Таймер смены поколений.
        /// </summary>
        private Timer changeGenerationTimer;

        private int currentGenerationIndex;

        /// <summary>
        /// Индекс текущего поколения.
        /// </summary>
        public int CurrentGenerationIndex
        {
            get
            {
                return this.currentGenerationIndex;
            }

            set
            {
                this.currentGenerationIndex = value;
                this.OnPropertyChanged(nameof(this.CurrentGenerationIndex));
            }
        }

        /// <summary>
        /// Предыдущие поколения.
        /// </summary>
        protected readonly List<SingleGenerationGrid<bool>> PreviousGenerations = new List<SingleGenerationGrid<bool>>();

        #endregion

        #region Методы

        /// <summary>
        /// Сдвиг на одно поколение назад.
        /// </summary>
        public void RewindGenerationBack()
        {
            var genIndex = this.CurrentGenerationIndex - 1;
            if (genIndex < 0)
            {
                genIndex = 0;
            }
            this.CurrentGeneration = this.PreviousGenerations[genIndex];
            this.CurrentGenerationIndex = genIndex;
        }

        /// <summary>
        /// Сдвиг на одно поколение вперед.
        /// </summary>
        public void RewindGenerationForward()
        {
            var genIndex = this.CurrentGenerationIndex + 1;
            if (genIndex >= this.PreviousGenerations.Count)
            {
                genIndex = this.PreviousGenerations.Count;
                this.CurrentGenerationIndex = genIndex;
                this.GenerateNewGeneration();
            }
            else
            {
                this.CurrentGeneration = this.PreviousGenerations[genIndex - 1];
                this.CurrentGenerationIndex = genIndex;
            }   
        }

        /// <summary>
        /// Стартовать игру.
        /// </summary>
        public void StartGame()
        {
            this.IsEvalutionRunning = true;
            this.changeGenerationTimer.Start();
        }

        /// <summary>
        /// Остановить игру.
        /// </summary>
        public void StopGame()
        {
            this.IsEvalutionRunning = false;
            this.changeGenerationTimer.Stop();
        }

        /// <summary>
        /// Задать следующее поколение.
        /// </summary>
        /// <param name="grid">Грид поколения.</param>
        protected void SetNextGeneration(SingleGenerationGrid<bool> grid)
        {
            if (grid == null)
            {
                this.StopGame();
            }
            else
            {
                if (this.currentGeneration != null)
                {
                    this.PreviousGenerations.Add(this.currentGeneration);
                    this.CurrentGenerationIndex = this.PreviousGenerations.Count;
                }
                else
                {
                    this.CurrentGenerationIndex = 0;
                }
                this.CurrentGeneration = grid;
            }
        }

        /// <summary>
        /// Проверить наличие зацикленностей в поколениях.
        /// </summary>
        /// <returns></returns>
        protected virtual bool CheckIsLoopInGenerationsPresent()
        {
            return false;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        protected abstract void GenerateNewGeneration();

        private void OnChangeGenerationTimerElapsed(object sender, ElapsedEventArgs args)
        {
            this.GenerateNewGeneration();
            var isLooped = this.CheckIsLoopInGenerationsPresent();
            if (isLooped)
            {
                this.StopGame();
            }
        }

        #endregion

        #region Конструкторы

        protected GameTemplate(SingleGenerationGrid<bool> current)
        {
            var timer = new Timer
            {
                Enabled = false,
                AutoReset = true,
                Interval = this.СhangeGenerationInterval
            };
            timer.Elapsed += this.OnChangeGenerationTimerElapsed;
            this.changeGenerationTimer = timer;
            this.SetNextGeneration(current);
        }

        #endregion
    }
}
