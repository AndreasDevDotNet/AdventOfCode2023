namespace AoCToolbox
{
    public static class ArrayExtensions
    {
        public static T[,] Reshape<T>(this T[,] array, int rows, int columns)
        {
            T[,] result = new T[rows, columns];
            int index = 0;

            foreach (T element in array)
            {
                result[index / columns, index % columns] = element;
                index++;
            }

            return result;
        }

        public static T[,] Transpose<T>(this T[,] array)
        {
            int rows = array.GetLength(0);
            int columns = array.GetLength(1);

            T[,] result = new T[columns, rows];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    result[j, i] = array[i, j];
                }
            }

            return result;
        }

        public static T[,] Append<T>(this T[,] array, T[] elements)
        {
            int rows = array.GetLength(0);
            int columns = array.GetLength(1);
            int newRows = rows + 1;

            T[,] result = new T[newRows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    result[i, j] = array[i, j];
                }
            }

            for (int j = 0; j < columns; j++)
            {
                result[rows, j] = elements[j];
            }

            return result;
        }
    }
}
