namespace GoL.Extras
{
    /// <summary>
    /// Вспомогательный класс смещений.
    /// </summary>
    internal class Offset
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public Offset(int dx, int dy)
        {
            this.Dx = dx;
            this.Dy = dy;
        }

        /// <summary>
        /// Смещение по оси Х.
        /// </summary>
        public readonly int Dx;

        /// <summary>
        /// Смещение по оси У.
        /// </summary>
        public readonly int Dy;
    }
}
