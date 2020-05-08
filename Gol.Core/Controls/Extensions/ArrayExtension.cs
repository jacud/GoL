namespace Gol.Core
{
    /// <summary>
    /// Расширение массивов.
    /// </summary>
    /// <remarks>Служит для превращение многомерных массивов в зубчаный. т.е.массив массивов.</remarks>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Сконвертировать многомерный массив в зубчаный.
        /// </summary>
        /// <typeparam name="T">Тип значения.</typeparam>
        /// <param name="twoDimensionalArray">Двумерный массив.</param>
        /// <returns>Зубчатый массив.</returns>
        public static T[][] ToJaggedArray<T>(this T[,] twoDimensionalArray)
        {
            int rowsFirstIndex = twoDimensionalArray.GetLowerBound(0);
            int rowsLastIndex = twoDimensionalArray.GetUpperBound(0);
            int numberOfRows = rowsLastIndex + 1;

            int columnsFirstIndex = twoDimensionalArray.GetLowerBound(1);
            int columnsLastIndex = twoDimensionalArray.GetUpperBound(1);
            int numberOfColumns = columnsLastIndex + 1;

            T[][] jaggedArray = new T[numberOfRows][];
            for (int i = rowsFirstIndex; i <= rowsLastIndex; i++)
            {
                jaggedArray[i] = new T[numberOfColumns];

                for (int j = columnsFirstIndex; j <= columnsLastIndex; j++)
                {
                    jaggedArray[i][j] = twoDimensionalArray[i, j];
                }
            }
            return jaggedArray;
        }
    }
}