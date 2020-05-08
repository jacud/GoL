using System;
using System.Configuration;
using System.IO;
using System.Windows;
using Gol.Core.Common;
using Gol.Core.Controls.Models;
using Gol.Core.Template;
using GoL.Extras;
using GoL.Model;
using GoL.Utils;

namespace GoL.ViewModel
{
    internal class MainWindowViewModel : NotificationObject
    {
        #region Поля и свойства

        private GameTemplate game;

        /// <summary>
        /// Игра.
        /// </summary>
        public GameTemplate Game
        {
            get
            {
                return this.game;

            }

            private set
            {
                this.game = value;
                this.OnPropertyChanged(nameof(this.Game));
            }
        }

        private DelegateCommand startGameCommand;

        /// <summary>
        /// Команда остановки игры.
        /// </summary>
        public DelegateCommand StartGameCommand
        {
            get { return this.startGameCommand ?? (this.startGameCommand = new DelegateCommand(this.StartGame, this.StartGameCanExecute)); }
        }


        private DelegateCommand stopGameCommand;

        /// <summary>
        /// Команда остановки игры.
        /// </summary>
        public DelegateCommand StopGameCommand
        {
            get { return this.stopGameCommand ?? (this.stopGameCommand = new DelegateCommand(this.StopGame, this.StopGameCanExecute)); }
        }

        private DelegateCommand openFileCommand;

        /// <summary>
        /// Команда остановки игры.
        /// </summary>
        public DelegateCommand OpenFileCommand
        {
            get
            {
                return this.openFileCommand ?? (this.openFileCommand = new DelegateCommand((this.OpenFile)));

            }
        }

        private DelegateCommand exitCommand;

        /// <summary>
        /// Команда остановки игры.
        /// </summary>
        public DelegateCommand ExitCommand
        {
            get { return this.exitCommand ?? (this.exitCommand = new DelegateCommand(this.Exit)); }
        }

        private DelegateCommand rewindBackCommand;

        /// <summary>
        /// Команда отмотки на один шаг назад.
        /// </summary>
        public DelegateCommand RewindBackCommand
        {
            get { return this.rewindBackCommand ?? (this.rewindBackCommand = new DelegateCommand(this.RewindBack, this.RewindBackCanExecute)); }
        }

        private DelegateCommand createUniverseCommand;

        /// <summary>
        /// Команда создания вселенной.
        /// </summary>
        public DelegateCommand CreateUniverseCommand
        {
            get { return this.createUniverseCommand ?? (this.createUniverseCommand = new DelegateCommand(this.CreateUniverse)); }
        }

        private DelegateCommand rewindForwardCommand;

        /// <summary>
        /// Команда перемотки на один шаг вперед.
        /// </summary>
        public DelegateCommand RewindForwardCommand
        {
            get { return this.rewindForwardCommand ?? (this.rewindForwardCommand = new DelegateCommand(this.RewindForward, this.RewindForwardCanExecute)); }
        }

        /// <summary>
        /// Ширина грида по умолчанию.
        /// </summary>
        private readonly int defaultFieldWidth = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultFieldWidth"]);

        /// <summary>
        /// Высота грида по умолчанию.
        /// </summary>
        private readonly int defaultFieldHeight = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultFieldHeight"]);

        #endregion

        #region Методы

        /// <summary>
        /// Cохдать вселенную.
        /// </summary>
        private void CreateUniverse(object obj)
        {
            var grid = new SingleGenerationGrid<bool>(new bool[this.defaultFieldWidth, this.defaultFieldHeight], Guid.NewGuid());
            this.Game = new ClassicGameOfLife(grid);
        }

        /// <summary>
        /// Остановить игру.
        /// </summary>
        /// <param name="obj">Аргументы.</param>
        private void StopGame(object obj)
        {
            this.game.StopGame();
        }

        /// <summary>
        /// Возможность остановить игру.
        /// </summary>
        /// <param name="obj">Агрументы.</param>
        private bool StopGameCanExecute(object obj)
        {
            return this.game.IsEvalutionRunning;
        }

        /// <summary>
        /// Начать игру.
        /// </summary>
        /// <param name="obj">Агрументы.</param>
        private void StartGame(object obj)
        {
            this.game.StartGame();
        }

        /// <summary>
        /// Возможность начать игру.
        /// </summary>
        /// <param name="obj">Агрументы.</param>
        private bool StartGameCanExecute(object obj)
        {
            return !this.game.IsEvalutionRunning;
        }

        /// <summary>
        /// Перемотать к следующему поколению.
        /// </summary>
        /// <param name="obj">Агрументы.</param>
        private void RewindForward(object obj)
        {
            this.game.RewindGenerationForward();
        }

        /// <summary>
        /// Возможность перемотать к следующему поколению.
        /// </summary>
        /// <param name="obj">Агрументы.</param>
        private bool RewindForwardCanExecute(object obj)
        {
            return !this.game.IsLastGeneration;
        }

        /// <summary>
        /// Перемотать в предыдущему поколению
        /// </summary>
        /// <param name="obj">Агрументы.</param>
        private void RewindBack(object obj)
        {
            this.game.RewindGenerationBack();
        }

        /// <summary>
        /// Возможность перемотать в предыдущему поколению
        /// </summary>
        /// <param name="obj">Агрументы.</param>
        private bool RewindBackCanExecute(object obj)
        {
            return !this.game.IsFirstGeneration;
        }

        /// <summary>
        /// Открыть файл.
        /// </summary>
        /// <param name="obj">Агрументы.</param>
        private void OpenFile(object obj)
        {
            Stream fileStream;
            if (FileUtils.TryOpenFile(out fileStream))
            {
                using (fileStream)
                {
                    var grid = SerializationUtils.Read<SingleGenerationGrid<bool>>(fileStream);
                    this.Game = new ClassicGameOfLife(grid);
                }
            }
        }

        private void Exit(object obj)
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region Конструкторы

        /// <summary>
        /// Конструктор
        /// </summary>
        public MainWindowViewModel()
        {
            this.CreateUniverse(new object());
        }

        #endregion
    }
}
