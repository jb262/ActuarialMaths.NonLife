using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActuarialMaths.NonLife.ClaimsReserving.Model
{
    /// <summary>
    /// "Run-off square" for the claims projection of a run-off triangle according to a reserving method.
    /// </summary>
    public class Square : ISquare, ICloneable
    {
        /// <summary>
        /// Square array containing the existing and projected claims.
        /// </summary>
        private decimal[,] claims;

        /// <summary>
        /// Number of periods under observation.
        /// </summary>
        public int Periods { get; }

        /// <summary>
        /// Constructor of the "run-off square".
        /// </summary>
        /// <param name="periods">Number of periods under observation.</param>
        public Square(int periods)
        {
            Periods = periods;
            claims = new decimal[periods, periods];
        }

        /// <summary>
        /// Indexer of the ISliceable.
        /// </summary>
        /// <param name="row">Row where the value to be accessed resides.</param>
        /// <param name="column">Column where the value to be accessed resides.</param>
        /// <returns>Value that resides at the address provided by the given row and column.</returns>
        /// <exception cref="NegativePeriodException">Thrown when the row or column, representing accident and relative settlement period, is negative.</exception>
        /// <exception cref="ObservationPeriodExceedenceException">Thrown when the row or column, representing accident and relative settlement period, exceeds the observation period.</exception>
        public decimal this[int row, int column]
        {
            get
            {
                if (row < 0 ||column < 0)
                {
                    throw new NegativePeriodException();
                }

                if (row > Periods - 1 || column > Periods - 1)
                {
                    throw new ObservationPeriodExceedenceException();
                }

                return claims[row, column];
            }

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

                claims[row, column] = value;
            }
        }

        /// <summary>
        /// Provides a specific row of the object.
        /// </summary>
        /// <param name="row">Index of the row to be accessed.</param>
        /// <returns>Specified row of the object.</returns>
        /// <exception cref="NegativePeriodException">Thrown when the row, representing an accident period, is smaller than zero.</exception>
        /// <exception cref="ObservationPeriodExceedenceException">Thrown when the row, representing an accident period, is greater than the last period under observation.</exception>
        public IEnumerable<decimal> GetRow(int row)
        {
            if (row < 0)
            {
                throw new NegativePeriodException();
            }

            if (row > Periods - 1)
            {
                throw new ObservationPeriodExceedenceException();
            }

            for (int i = 0; i < Periods; i++)
            {
                yield return claims[row, i];
            }
        }

        /// <summary>
        /// Provides a specific column of the object.
        /// </summary>
        /// <param name="column">Index of the column to be accessed.</param>
        /// <returns>Specified column of the object.</returns>
        /// <exception cref="NegativePeriodException">Thrown when the column, representing a relative settlement period, is smaller than zero.</exception>
        /// <exception cref="ObservationPeriodExceedenceException">Thrown when the column, representing arelative settlement period, is greater than the last period under observation.</exception>
        public IEnumerable<decimal> GetColumn(int column)
        {
            if (column < 0)
            {
                throw new NegativePeriodException();
            }

            if (column > Periods - 1)
            {
                throw new ObservationPeriodExceedenceException();
            }

            for (int i = 0; i < Periods; i++)
            {
                yield return claims[i, column];
            }
        }

        /// <summary>
        /// Provides the main diagonal of the object.
        /// </summary>
        /// <returns>Main diagonal of the object.</returns>
        public IEnumerable<decimal> GetDiagonal()
        {
            return GetDiagonal(Periods - 1);
        }

        /// <summary>
        /// Provides a specific diagonal of the object.
        /// </summary>
        /// <param name="diagonal">Index of the diagonal to be accessed.</param>
        /// <returns>Specified diagonal of the object.</returns>
        /// <exception cref="NegativePeriodException">Thrown when the diagonal, representing an absolute settlement period, is smaller than zero.</exception>
        /// <exception cref="ObservationPeriodExceedenceException">Thrown when the diagonal, representing an absolute settlement period, is greater than the last period under observation.</exception>
        /// <remarks>
        /// In this model it is assumed that a diagonal is accessed from the lower left to the upper right corner of the triangle.
        /// This is the result of the assumption that claims are first ordered by accident period in ascending order (= diagonal index),
        /// then by relative settlement period in ascending order (= element order inside the diagonal).
        public IEnumerable<decimal> GetDiagonal(int diagonal)
        {
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
                for (int i = 0; i < diagonal + 1; i++)
                {
                    yield return claims[diagonal - i, i];
                }
            }
            else
            {
                int j = diagonal - Periods + 1;
                for (int i = 0; i < 2 * Periods - diagonal - 1; i++)
                {
                    yield return claims[Periods - 1 - i, j];
                    j++;
                }
            }
        }

        /// <summary>
        /// Sets a specific row of the object.
        /// </summary>
        /// <param name="values">Values the row is to be set to.</param>
        /// <param name="row">Index of the row to be set.</param>
        /// <exception cref="DimensionMismatchException">Thrown when the element count of the new values does not match the length of the triangle's row.</exception>
        /// <exception cref="NegativePeriodException">Thrown when the row, representing an accident period, is smaller than zero.</exception>
        /// <exception cref="ObservationPeriodExceedenceException">Thrown when the row, representing an accident period, is greater than the last period under observation.</exception>
        public void SetRow(IEnumerable<decimal> values, int row)
        {
            int n = values.Count();

            if (n != Periods)
            {
                throw new DimensionMismatchException(Periods, n);
            }

            int column = 0;

            foreach (decimal val in values)
            {
                claims[row, column] = val;
                column++;
            }
        }

        /// <summary>
        /// Sets a specific column of the object.
        /// </summary>
        /// <param name="values">Values the column is to be set to.</param>
        /// <param name="column">Index of the column to be set.</param>
        /// <exception cref="DimensionMismatchException">Thrown when the element count of the new values does not match the length of the triangle's column.</exception>
        /// <exception cref="NegativePeriodException">Thrown when the column, representing a relative setlement period, is smaller than zero.</exception>
        /// <exception cref="ObservationPeriodExceedenceException">Thrown when the column, representing a relative settlement period, is greater than the last period under observation.</exception>
        public void SetColumn(IEnumerable<decimal> values, int column)
        {
            int n = values.Count();

            if (n != Periods)
            {
                throw new DimensionMismatchException(Periods, n);
            }

            int row = 0;

            foreach (decimal val in values)
            {
                claims[row, column] = val;
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

            if (diagonal < Periods)
            {
                row = diagonal;
                column = 0;
                expectedLen = diagonal + 1;
            }
            else
            {
                row = Periods - 1;
                column = diagonal - Periods;
                expectedLen = 2 * Periods - diagonal;
            }

            if (n != expectedLen)
            {
                throw new DimensionMismatchException(expectedLen, n);
            }

            foreach (decimal val in values)
            {
                claims[row, column] = val;
                row--;
                column++;
            }
        }

        /// <summary>
        /// Creates a deep copy of the sqaure.
        /// </summary>
        /// <returns>Deep copy of the square.</returns>
        public object Clone()
        {
            Square cloned = new Square(Periods);
            for (int i = 0; i < Periods; i++)
            {
                for (int j = 0; j < Periods; j++)
                {
                    cloned.claims[i, j] = claims[i, j];
                }
            }

            return cloned;
        }

        /// <summary>
        /// Creates a string representation of the square.
        /// </summary>
        /// <returns>String representation of the square.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < Periods; i++)
            {
                for (int j = 0; j < Periods; j++)
                {
                    sb.Append(claims[i, j].ToString("0.00"));

                    if (j < Periods - 1)
                    {
                        sb.Append("\t");
                    }
                }

                if (i < Periods - 1)
                {
                    sb.Append("\n");
                }
            }

            return sb.ToString();
        }
    }
}