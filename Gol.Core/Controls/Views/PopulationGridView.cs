using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Gol.Core.Controls.Models;
using GoL.Core.Controls.Models;

namespace GoL.Core.Controls.Views
{
    /// <summary>
    /// Представление грида популяции.
    /// </summary>
    public class PopulationGridView : StackPanel
    {
        #region Вспомогательные типы

        /// <summary>
        /// Точка с целочисленными координатами.
        /// </summary>
        private struct IntPoint
        {
            /// <summary>
            /// Конструктор.
            /// </summary>
            /// <param name="x">Абсцисса.</param>
            /// <param name="y">Ордината.</param>
            public IntPoint(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            /// <summary>
            /// Абсцисса.
            /// </summary>
            public readonly int X;

            /// <summary>
            /// Ордината.
            /// </summary>
            public readonly int Y;
        }

        #endregion

        #region Поля и свойства

        /// <summary>
        /// Cell brush color.
        /// </summary>
        public Brush CellBrush
        {
            get
            {
                return (Brush)GetValue(CellBrushProperty);
            }

            set
            {
                SetValue(CellBrushProperty, value);
            }
        }

        /// <summary>
        /// Только чтение.
        /// </summary>
        /// <remarks>Запрещает редактировать холст мышью.</remarks>
        public bool ReadOnly
        {
            get
            {
                return (bool)GetValue(IsReadOnlyProperty);
            }

            set
            {
                SetValue(IsReadOnlyProperty, value);
            }
        }

        /// <summary>
        /// Цвет линии.
        /// </summary>
        public Brush LineBrush
        {
            get
            {
                return (Brush)GetValue(LineBrushProperty);
            }

            set
            {
                SetValue(LineBrushProperty, value);
            }
        }

        /// <summary>
        /// Размер ячейки.
        /// </summary>
        public int CellSize
        {
            get
            {
                return (int)GetValue(CellSizeProperty);
            }

            set
            {
                SetValue(CellSizeProperty, value);
            }
        }

        /// <summary>
        /// Mono grid model.
        /// </summary>
        public SingleGenerationGrid<bool> GenerationGrid
        {
            get
            {
                return (SingleGenerationGrid<bool>)GetValue(SingleGenerationGridProperty);
            }

            set
            {
                SetValue(SingleGenerationGridProperty, value);
            }
        }

        /// <summary>
        /// Зависимое свойство для режима "только чтение".
        /// </summary>
        public static readonly DependencyProperty IsReadOnlyProperty;

        /// <summary>
        /// Зависимое свойство Цвета линий.
        /// </summary>
        public static readonly DependencyProperty LineBrushProperty;

        /// <summary>
        /// Зависимое свойство размера ячейки.
        /// </summary>
        public static readonly DependencyProperty CellSizeProperty;

        /// <summary>
        /// Зависимое свойство для одиночного поколения.
        /// </summary>
        public static readonly DependencyProperty SingleGenerationGridProperty;

        /// <summary>
        /// Зависимое свойство цвета ячейки.
        /// </summary>
        public static readonly DependencyProperty CellBrushProperty;

        /// <summary>
        /// Холст отображения поколения.
        /// </summary>
        private readonly Canvas generationCanvas;

        /// <summary>
        /// Массив дааных ячеек.
        /// </summary>
        private CellData[,] cellGrid;

        #endregion

        #region Методы

        /// <summary>
        /// Расчертить поле.
        /// </summary>
        /// <remarks>Отрисовывается сетка поля(сетка для ячеек).</remarks>
        protected virtual void DrawFieldInnerLines()
        {
            var mainColor = LineBrush;
            var tenthLine = new SolidColorBrush(ChangeColorBrightness(((SolidColorBrush)mainColor).Color, -0.6f));

            for (int i = 0; i <= this.GenerationGrid.Height; i++)
            {
                var horizontalLine = CreateLine(0, i * CellSize, this.GenerationGrid.Width * CellSize, i * CellSize);
                horizontalLine.Stroke = i % 10 == 0 ? LineBrush : tenthLine;
                this.generationCanvas.Children.Add(horizontalLine);
            }

            for (int j = 0; j <= this.GenerationGrid.Width; j++)
            {
                var verticalLine = CreateLine(j * CellSize, 0, j * CellSize, this.GenerationGrid.Height * CellSize);
                verticalLine.Stroke = j % 10 == 0 ? LineBrush : tenthLine;
                this.generationCanvas.Children.Add(verticalLine);
            }
        }

        /// <summary>
        /// Обработчик события изменения грида.
        /// </summary>
        /// <param name="grid">Грид поколения.</param>
        private void SingleGenerationModelChanged(SingleGenerationGrid<bool> grid)
        {
            if (this.generationCanvas == null)
            {
                return;
            }

            Guid lifeId;
            if (grid != null)
            {
                lifeId = grid.LifeId;
            }
            else
            {
                return;
            }

            if (Dispatcher.CheckAccess())
            {
                this.RenderGrid(lifeId);
            }
            else
            {
                Dispatcher.Invoke(new Action(() => this.RenderGrid(lifeId)));
            }
        }

        /// <summary>
        /// Отрисовать грид.
        /// </summary>
        /// <param name="lifeId"></param>
        private void RenderGrid(Guid? lifeId)
        {
            if (this.GenerationGrid == null || this.GenerationGrid.Height == 0 || this.GenerationGrid.Width == 0)
            {
                return;
            }

            bool needRecreate = this.cellGrid == null;
            bool isNewGrid = !(lifeId.HasValue && this.GenerationGrid != null && lifeId == this.GenerationGrid.LifeId);
            if (isNewGrid || needRecreate)
            {
                this.generationCanvas.Children.Clear();
                this.cellGrid = new CellData[GenerationGrid.Width, GenerationGrid.Height];
                this.DrawFieldInnerLines();
                Width = this.GenerationGrid.Width * CellSize;
                Height = this.GenerationGrid.Height * CellSize;
            }

            for (int i = 0; i < this.GenerationGrid.Width; i++)
            {
                for (int j = 0; j < this.GenerationGrid.Height; j++)
                {
                    CellData cell;
                    if (isNewGrid || needRecreate)
                    {
                        this.cellGrid[i, j] = cell = new CellData(i, j, this.generationCanvas, this);
                    }
                    else
                    {
                        cell = this.cellGrid[i, j];
                    }

                    if (this.GenerationGrid[i, j] && !cell.IsBlack)
                    {
                        DrawCell(i, j);
                    }
                    else if (!this.GenerationGrid[i, j] && cell.IsBlack)
                    {
                        cell.ClearRectangle();
                    }
                }
            }
        }

        /// <summary>
        /// Нарисовать ячейку.
        /// </summary>
        /// <param name="x">Х - индекс ячейки грида.</param>
        /// <param name="y">У - индекс ячейки грида.</param>
        private void DrawCell(int x, int y)
        {
            this.cellGrid[x, y].ColorizeCell();
        }

        /// <summary>
        /// Создать линию.
        /// </summary>
        /// <param name="x1">Абсцисса начала.</param>
        /// <param name="y1">Ордината начала.</param>
        /// <param name="x2">Абсцисса конца.</param>
        /// <param name="y2">Ордината конца.</param>
        /// <returns>Созданная линия.</returns>
        private Line CreateLine(int x1, int y1, int x2, int y2)
        {
            return new Line { X1 = x1, X2 = x2, Y1 = y1, Y2 = y2, StrokeThickness = 1 };
        }

        /// <summary>
        /// Скорректировать яркость цвета.
        /// </summary>
        /// <param name="color">Цвет для корректировки.</param>
        /// <param name="correctionFactor">Коэффициент корректировки.</param>
        /// <returns>
        /// скорректированный цвет.
        /// </returns>
        /// <remarks>Если correctionFactor отрицателен то цвет становится темнее и наоборот.</remarks>
        private static Color ChangeColorBrightness(Color color, float correctionFactor)
        {
            float red = color.R;
            float green = color.G;
            float blue = color.B;

            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }

            return Color.FromArgb(color.A, (byte)red, (byte)green, (byte)blue);
        }

        #endregion

        #region Базовый класс

        /// <summary>
        /// Признак того, движется мышь или нет.
        /// </summary>
        private bool isMouseMoving = false;

        /// <summary>
        /// Текущее положение мыши по оси Х.
        /// </summary>
        private int currentMousePositionX = -1;

        /// <summary>
        /// Текущее положение мыши по оси Y.
        /// </summary>
        private int currentMousePositionY = -1;

        /// <inheritdoc/>
        protected override void OnMouseUp(MouseButtonEventArgs args)
        {
            base.OnMouseUp(args);
            if (args.LeftButton == MouseButtonState.Released && !this.isMouseMoving && !this.ReadOnly)
            {
                var cell = GetMouseCell(args);
                this.ProcessMouseSelection(cell);
            }
        }

        /// <inheritdoc/>
        protected override void OnMouseMove(MouseEventArgs args)
        {
            base.OnMouseMove(args);
            if (args.LeftButton == MouseButtonState.Pressed && !this.ReadOnly)
            {
                this.isMouseMoving = true;
                var cell = this.GetMouseCell(args);
                if (cell.X == currentMousePositionX && cell.Y == this.currentMousePositionY)
                {
                    return;
                }

                this.ProcessMouseSelection(cell, true);
                this.currentMousePositionX = cell.X;
                this.currentMousePositionY = cell.Y;
            }

            if (args.LeftButton == MouseButtonState.Released)
            {
                this.currentMousePositionY = this.currentMousePositionX = -1;
                this.isMouseMoving = false;
            }
        }

        /// <summary>
        /// Обработать веделение мышью.
        /// </summary>
        /// <param name="cell">Выделенная ячейка.</param>
        /// <param name="isOnlyDraw">Режим отрисовки.</param>
        private void ProcessMouseSelection(IntPoint cell, bool isOnlyDraw = false)
        {
            if (cell.X < 0 || cell.X >= this.GenerationGrid.Width || cell.Y < 0 ||
                cell.Y >= this.GenerationGrid.Height)
                return;

            var current = this.GenerationGrid[cell.X, cell.Y];
            bool setValue = true;

            if (current)
            {
                if (!isOnlyDraw)
                {
                    this.cellGrid[cell.X, cell.Y].ClearRectangle();
                }
                else
                {
                    setValue = false;
                }
            }
            else
            {
                this.DrawCell(cell.X, cell.Y);
            }

            if (setValue)
            {
                this.GenerationGrid[cell.X, cell.Y] = !current;
            }
        }

        private IntPoint GetMouseCell(MouseEventArgs args)
        {
            var position = args.GetPosition(this);
            int x = (int)(position.X / CellSize), y = (int)(position.Y / CellSize);
            return new IntPoint(x, y);
        }

        #endregion

        #region Конструкторы

        /// <summary>
        /// Конструктор.
        /// </summary>
        public PopulationGridView()
        {
            this.generationCanvas = new Canvas();
            Children.Add(this.generationCanvas);
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        static PopulationGridView()
        {
            CellBrushProperty = DependencyProperty.Register(
                "CellBrush",
                typeof(Brush),
                typeof(PopulationGridView),
                new PropertyMetadata(Brushes.GreenYellow));

            IsReadOnlyProperty = DependencyProperty.Register(
                "(IsReadOnly)",
                typeof(bool),
                typeof(PopulationGridView),
                new PropertyMetadata(default(bool)));

            LineBrushProperty = DependencyProperty.Register(
                "LineBrush",
                typeof(Brush),
                typeof(PopulationGridView),
                new PropertyMetadata(new BrushConverter().ConvertFrom("#2d2d2d")));

            CellSizeProperty = DependencyProperty.Register(
                "CellSize",
                typeof(int),
                typeof(PopulationGridView),
                new PropertyMetadata(10));

            SingleGenerationGridProperty = DependencyProperty.Register(
                "GenerationGrid",
                typeof(SingleGenerationGrid<bool>),
                typeof(PopulationGridView),
                new PropertyMetadata(
                    default(SingleGenerationGrid<bool>),
                    (source, args) => ((PopulationGridView)source).SingleGenerationModelChanged((SingleGenerationGrid<bool>)args.NewValue)));
        }

        #endregion
    }
}
