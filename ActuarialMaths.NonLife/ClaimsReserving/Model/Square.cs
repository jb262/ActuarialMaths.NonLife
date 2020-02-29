using System.Collections.Generic;
using System.Linq;

namespace ActuarialMaths.NonLife.ClaimsReserving.Model
{
    /// <summary>
    /// Run-off square for the claims projection of a run-off triangle according to a reserving method.
    /// </summary>
    public class Square : SquareBase, ISquare
    {
        /// <summary>
        /// Indexer of the run-off square.
        /// </summary>
        /// <param name="row">Row where the value to be accessed resides.</param>
        /// <param name="column">Column where the value to be accessed resides.</param>
        /// <returns>Value that resides at the address provided by the given row and column.</returns>
        public new decimal this[int row, int column]
        {
            get => base[row, column];

            set
            {
                if (row < 0 || column < 0)
                {
                    throw new NegativePeriodException();
                }

                if (row > Periods - 1 || column > Periods - 1)
                {
                    throw new ObservationPeriodExceedenceException();
                }

                _claims[row, column] = value;
            }
        }

        /// <summary>
        /// Constructor of the run-off square.
        /// </summary>
        /// <param name="periods">Number of periods under observation.</param>
        public Square(int periods)
        {
            Periods = periods;
            _claims = new decimal[periods, periods];
        }

        /// <summary>
        /// Sets a specific row of the object.
        /// </summary>
        /// <param name="values">Values the row is to be set to.</param>
        /// <param name="row">Index of the row to be set.</param>
        /// <exception cref="DimensionMismatchException">Thrown when the element count of the new values does not match the length of the square's row.</exception>
        /// <exception cref="NegativePeriodException">Thrown when the row, representing an accident period, is smaller than zero.</exception>
        /// <exception cref="ObservationPeriodExceedenceException">Thrown when the row, representing an accident period, is greater than the last period under observation.</exception>
        public void SetRow(IEnumerable<decimal> values, int row)
        {
            int n = values.Count();

            if (n != Periods)
            {
                throw new DimensionMismatchException(Periods, n);
            }

            if (row < 0)
            {
                throw new NegativePeriodException();
            }

            if (row > Periods - 1)
            {
                throw new ObservationPeriodExceedenceException();
            }

            int column = 0;

            foreach (decimal val in values)
            {
                _claims[row, column] = val;
                column++;
            }
        }

        /// <summary>
        /// Sets a specific column of the object.
        /// </summary>
        /// <param name="values">Values the column is to be set to.</param>
        /// <param name="column">Index of the column to be set.</param>
        /// <exception cref="DimensionMismatchException">Thrown when the element count of the new values does not match the length of the square's column.</exception>
        /// <exception cref="NegativePeriodException">Thrown when the column, representing a relative setlement period, is smaller than zero.</exception>
        /// <exception cref="ObservationPeriodExceedenceException">Thrown when the column, representing a relative settlement period, is greater than the last period under observation.</exception>
        public void SetColumn(IEnumerable<decimal> values, int column)
        {
            int n = values.Count();

            if (n != Periods)
            {
                throw new DimensionMismatchException(Periods, n);
            }

            if (column < 0)
            {
                throw new NegativePeriodException();
            }

            if (column > Periods - 1)
            {
                throw new ObservationPeriodExceedenceException();
            }

            int row = 0;

            foreach (decimal val in values)
            {
                _claims[row, column] = val;
                row++;
            }
        }

        /// <summary>
        /// Sets the main diagonal of the object.
        /// </summary>
        /// <param name="values">Values the diagonal is to be set to.</param>
        public void SetDiagonal(IEnumerable<decimal> values)
        {
            SetDiagonal(values, Periods - 1);
        }

        /// <summary>
        /// Sets a specific diagonal of the object.
        /// </summary>
        /// <param name="values">Values the diagonal is to be set to.</param>
        /// <param name="diagonal">Index of the diagonal to be set.</param>
        /// <exception cref="DimensionMismatchException">Thrown when the element count of the new values does not match the length of the triangle's diagonal.</exception>
        /// <exception cref="NegativePeriodException">Thrown when the diagonal, representing an absolute settlement period, is smaller than zero.</exception>
        /// <exception cref="ObservationPeriodExceedenceException">Thrown when the diagonal, representing an absolute settlement period, is greater than the last period under observation.</exception>
        public void SetDiagonal(IEnumerable<decimal> values, int diagonal)
        {
            int n = values.Count();
            int expectedLen;
            int row;
            int column;

            if (diagonal < 0)
            {
                throw new NegativePeriodException();
            }

            if (diagonal > 2 * (Periods - 1))
            {
                throw new ObservationPeriodExceedenceException();
            }

            if (diagonal < Periods)
            {
                row = diagonal;
                column = 0;
                expectedLen = diagonal + 1;
            }
            else
            {
                row = Periods - 1;
                column = diagonal - Periods + 1;
                expectedLen = 2 * Periods - diagonal - 1;
            }

            if (n != expectedLen)
            {
                throw new DimensionMismatchException(expectedLen, n);
            }

            foreach (decimal val in values)
            {
                _claims[row, column] = val;
                row--;
                column++;
            }
        }

        /// <summary>
        /// Initializes the values of the square from a triangle.
        /// </summary>
        /// <param name="triangle">Triangle the square is to be initialized from.</param>
        public void InitFromTriangle(IReadOnlyTriangle triangle)
        {
            if (triangle.Periods != Periods)
            {
                throw new DimensionMismatchException(Periods, triangle.Periods);
            }

            for (int i = 0; i < triangle.Periods; i++)
            {
                int col = 0;

                foreach (decimal val in triangle.GetRow(i))
                {
                    _claims[i, col] = val;
                    col++;
                }
            }
        }

        /// <summary>
        /// Creates a read only copy of the current run-off square.
        /// </summary>
        /// <returns>An immutable instance of the run-off square.</returns>
        public IReadOnlySquare AsReadOnly()
        {
            return new ReadOnlySquare(this);
        }
    }
}
