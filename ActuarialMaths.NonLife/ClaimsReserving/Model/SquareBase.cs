using System;
using System.Collections.Generic;
using System.Text;

namespace ActuarialMaths.NonLife.ClaimsReserving.Model
{
    /// <summary>
    /// Basic implementation of a run-off square with methods and periods shared by both mutable and immutable squares.
    /// </summary>
    public abstract class SquareBase : IReadOnlySquare, ICloneable
    {
        /// <summary>
        /// Square array containing the existing and projected claims.
        /// </summary>
        protected decimal[,] _claims;

        /// <summary>
        /// Number of periods under observation.
        /// </summary>
        public int Periods { get; protected set; }

        /// <summary>
        /// Indexer of the square.
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
                if (row < 0 || column < 0)
                {
                    throw new NegativePeriodException();
                }

                if (row > Periods - 1 || column > Periods - 1)
                {
                    throw new ObservationPeriodExceedenceException();
                }

                return _claims[row, column];
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
                yield return _claims[row, i];
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
                yield return _claims[i, column];
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
                    yield return _claims[diagonal - i, i];
                }
            }
            else
            {
                int j = diagonal - Periods + 1;
                for (int i = 0; i < 2 * Periods - diagonal - 1; i++)
                {
                    yield return _claims[Periods - 1 - i, j];
                    j++;
                }
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
                    cloned._claims[i, j] = _claims[i, j];
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
                    sb.Append(_claims[i, j].ToString("0.00"));

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
