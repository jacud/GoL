using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Gol.Core.Controls.Models
{
    /// <summary>
    /// Грид для одного поколения.
    /// </summary>
    /// <typeparam name="T">Тип значения.</typeparam>
    [DataContract]
    public class SingleGenerationGrid<T>
    {
        #region Поля и свойства

        /// <summary>
        /// Индексатор.
        /// </summary>
        /// <param name="x">Х- координата.</param>
        /// <param name="y">У - координата.</param>
        public T this[int x, int y]
        {
            get
            {
                return this.sourceArray[x][y];
            }

            set
            {
                this.sourceArray[x][y] = value;
            }
        }

        /// <summary>
        /// Идентификатор  зародившейся жизни.
        /// </summary>
        [DataMember]
        public Guid LifeId { get; private set; }

        /// <summary>
        /// Ширина грида.
        /// </summary>
        public int Width => this.sourceArray.GetLength(0);

        /// <summary>
        /// Высота грида.
        /// </summary>
        public int Height => this.sourceArray[0].GetLength(0);

        /// <summary>
        /// Массив, в котором хранится состояние текущей популяции.
        /// </summary>
        [DataMember]
        private T[][] sourceArray;

        #endregion

        #region Методы

        /// <summary>
        /// Копировать.
        /// </summary>
        /// <returns>Новый экземпляр класса после копирования данных.</returns>
        public SingleGenerationGrid<T> Clone()
        {
            var copy = new T[this.Width][];
            for (int i = 0; i < this.Width; i++)
            {
                var second = new T[this.Height];
                Array.Copy(this.sourceArray[i], second, this.Height);
                copy[i] = second;
            }

            return new SingleGenerationGrid<T>(copy, this.LifeId);
        }

        /// <summary>
        /// Инициализировать грид.
        /// </summary>
        /// <param name="array">массив даных.</param>
        /// <param name="lifeId">Идентификатор жизни.</param>
        private void Initialize(T[][] array, Guid lifeId)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (lifeId == Guid.Empty)
            {
                throw new ArgumentException(nameof(lifeId));
            }

            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == null)
                {
                    throw new ArgumentNullException($"sourceArray [{i}] is null");
                }
            }

            this.sourceArray = array;
            this.LifeId = lifeId;
        }

        #endregion

        #region Базовый класс

        public override bool Equals(object ob)
        {
            var grid = (SingleGenerationGrid<T>)ob;
            for (int i = 0; i < this.sourceArray.Length - 1; i++)
            {
                if (!this.sourceArray[i].SequenceEqual(grid.sourceArray[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            return this.sourceArray.GetHashCode();
        }

        #endregion

        #region Конструкторы

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="array">Массив ячеек с жизнью.</param>
        /// <param name="lifeId">Идентификатор жизни.</param>
        public SingleGenerationGrid(T[][] array, Guid lifeId)
        {
            this.Initialize(array, lifeId);
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="array">Массив ячеек с жизнью.</param>
        /// <param name="lifeId">Идентификатор жизни.</param>
        public SingleGenerationGrid(T[,] array, Guid lifeId)
        {
            this.Initialize(array.ToJaggedArray(), lifeId);
        }

        #endregion
    }
}