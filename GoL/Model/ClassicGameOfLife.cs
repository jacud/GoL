using System.Linq;
using Gol.Core.Controls.Models;
using Gol.Core.Template;
using GoL.Extras;

namespace GoL.Model
{
    public class ClassicGameOfLife : GameTemplate
    {
        #region Вспомогательные структуры

        private readonly Offset[] offsets =
        {
            // Line 1
            new Offset(-1, -1),
            new Offset(0, -1),
            new Offset(1, -1),

            // Line 2
            new Offset(-1, 0),
            new Offset(1, 0),

            // Line 3
            new Offset(-1, 1),
            new Offset(0, 1),
            new Offset(1, 1),
        };

        #endregion

        #region Методы

        /// <summary>
        /// Получить количество "живых соседей".
        /// </summary>
        /// <param name="x">Абсцисса точки в гриде.</param>
        /// <param name="y">Ордината точки в гриде.</param>
        /// <param name="grid">Грид.</param>
        /// <returns>Количество "живых" соседей.</returns>
        private int GetNeighborCellsCount(int x, int y, SingleGenerationGrid<bool> grid)
        {
            return offsets.Sum(offset => IsCellAlive(x + offset.Dx, y + offset.Dy, grid) ? 1 : 0);
        }


        /// <summary>
        /// Является ли ячейка живой.
        /// </summary>
        /// <param name="x">Абсцисса точки в гриде.</param>
        /// <param name="y">Ордината точки в гриде.</param>
        /// <param name="grid">Грид.</param>
        /// <returns>Признак того жива ячейка или нет.</returns>
        private static bool IsCellAlive(int x, int y, SingleGenerationGrid<bool> grid)
        {
            if ((x >= 0 && x < grid.Width) && (y >= 0 && y < grid.Height))
            {
                return grid[x, y];
            }

            return false;
        }

        #endregion

        #region Базовый класс

        protected override bool CheckIsLoopInGenerationsPresent()
        {
            for (var i = this.PreviousGenerations.Count - 1; i >= 0; i--)
            {
                if (!this.IsFirstGeneration && this.CurrentGeneration.Equals(this.PreviousGenerations[i]))
                {
                    return true;
                }
            }
            return false;
        }

        protected override void GenerateNewGeneration()
        {
            var newGeneration = this.CurrentGeneration.Clone();
            for (int i = 0; i < newGeneration.Width; i++)
            {
                for (int j = 0; j < newGeneration.Height; j++)
                {
                    var nearCells = GetNeighborCellsCount(i, j, this.CurrentGeneration);
                    bool isCreature = this.CurrentGeneration[i, j];

                    if (isCreature)
                    {
                        // Если у живой клетки есть две или три живые соседки, то эта клетка продолжает жить;
                        newGeneration[i, j] = nearCells == 2 || nearCells == 3;
                    }
                    else
                    {
                        // В пустой (мёртвой) клетке, рядом с которой ровно три живые клетки, зарождается жизнь;
                        newGeneration[i, j] = nearCells == 3;
                    }
                }
            }

            this.SetNextGeneration(newGeneration);
        }

        #endregion

        #region Конструкторы

        public ClassicGameOfLife(SingleGenerationGrid<bool> current) : base(current) { }

        #endregion
    }
}
