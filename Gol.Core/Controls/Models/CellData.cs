using System;
using System.Windows.Controls;
using System.Windows.Shapes;
using GoL.Core.Controls.Views;

namespace GoL.Core.Controls.Models
{
    /// <summary>
    /// Класс дынных ячейки.
    /// </summary>
    internal class CellData
    {
        #region Поля и свойства

        private readonly Canvas canvas;

        private readonly PopulationGridView parent;

        /// <summary>
        /// Абсцисса.
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Ордината.
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// Ссылка на прямоуголик данной ячейки.
        /// </summary>
        public Rectangle Rectangle { get; private set; }

        /// <summary>
        /// Являетсяли ячейка черной.
        /// </summary>
        public bool IsBlack => this.canvas.Children.Contains(Rectangle);

        #endregion

        #region Методы

        /// <summary>
        /// Раскрасить ячейку.
        /// </summary>
        public virtual void ColorizeCell()
        {
            if (this.Rectangle == null)
            {
                this.Rectangle = new Rectangle()
                {
                    Fill = this.parent.CellBrush,
                    Width = this.parent.CellSize,
                    Height = this.parent.CellSize
                };
            }
            else if (this.canvas.Children.Contains(Rectangle))
            {
                return;
            }

            this.canvas.Children.Add(Rectangle);
            Canvas.SetLeft(Rectangle, this.X * this.parent.CellSize);
            Canvas.SetTop(Rectangle, this.Y * this.parent.CellSize);
            Panel.SetZIndex(Rectangle, -1);
        }

        /// <summary>
        /// Очистить прямоугольник.
        /// </summary>
        public void ClearRectangle()
        {
            if (Rectangle == null)
            {
                throw new ArgumentException("Rectangle is not drawed here");
            }

            this.canvas.Children.Remove(Rectangle);
        }

        #endregion

        #region Конструкторы

        /// <summary>
        /// Конструктор.
        /// </summary>
        public CellData(int x, int y, Canvas canvas, PopulationGridView parent)
        {
            this.canvas = canvas;
            this.parent = parent;
            this.X = x;
            this.Y = y;
        }

        #endregion
    }
}