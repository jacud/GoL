using System;
using System.IO;
using Microsoft.Win32;

namespace GoL.Utils
{
    /// <summary>
    /// Класс - утилита для работы с фалами.
    /// </summary>
    public static class FileUtils
    {
        #region Константы

        /// <summary>
        /// Директория по умолчанию.
        /// </summary>
        private const string DefaultDirectoryName = "Example";

        /// <summary>
        /// Фильтр для диалога открытия файлов.
        /// </summary>
        private const string FilesFilter = "Life saves (*.life)|*.life|All files (*.*)|*.*";

        #endregion

        #region Методы

        /// <summary>
        /// Получить начальную директорию.
        /// </summary>
        /// <returns>Return default initial directory path.</returns>
        public static string GetInitialDirectory()
        {
            var initialDirectory = Environment.CurrentDirectory;
            var examplePath = Path.Combine(Environment.CurrentDirectory, DefaultDirectoryName);
            if (Directory.Exists(examplePath))
            {
                initialDirectory = examplePath;
            }

            return initialDirectory;
        }

        /// <summary>
        /// Попытаться сохранить файл.
        /// </summary>
        /// <param name="saveStream">Поток для записи.</param>
        /// <returns>Индикатор успеха.</returns>
        public static bool TrySaveFile(out Stream saveStream)
        {
            saveStream = null;
            var saveDialog = new SaveFileDialog()
            {
                InitialDirectory = GetInitialDirectory(),
                Filter = FilesFilter,
                RestoreDirectory = true
            };

            var dialogResult = saveDialog.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                saveStream = saveDialog.OpenFile();
                return true;
            }

            return false;
        }


        /// <summary>
        /// Попытаться прочитать файл.
        /// </summary>
        /// <param name="openStream">Поток для чтения.</param>
        /// <returns>Индикатор успеха.</returns>
        public static bool TryOpenFile(out Stream openStream)
        {
            openStream = null;
            var openDialog = new OpenFileDialog()
            {
                InitialDirectory = GetInitialDirectory(),
                Filter = FilesFilter,
                RestoreDirectory = true
            };

            var dialogResult = openDialog.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                openStream = openDialog.OpenFile();
                return true;
            }

            return false;
        }

        #endregion
    }
}