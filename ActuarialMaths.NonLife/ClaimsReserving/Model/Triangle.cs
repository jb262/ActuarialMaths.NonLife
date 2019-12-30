using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActuarialMaths.NonLife.ClaimsReserving.Model
{
    /// <summary>
    /// Abstract base class for run-off triangles, implements ISliceable for decimal types and ICloneable
    /// </summary>
    public abstract class Triangle : ITriangle, ICloneable
    {
        /// <summary>
        /// Initial capacity of the jagged two dimensional array the triangle's values are to be stored in.
        /// </summary>
        private const int initialCapacity = 8;

        /// <summary>
        /// Jagged two dimensional array containing the existing claims of the model.
        /// </summary>
        protected decimal[][] claims;

        /// <summary>
        /// The capacity of the jagged tow dimensional array containing the existing claims of the model.
        /// </summary>
        private int capacity;

        /// <summary>
        /// Number of periods under observation.
        /// </summary>
        public int Periods { get; private set; }

        /// <summary>
        /// Constructor for an empty run-off triangle.
        /// </summary>
        protected Triangle() : this(0) { }

        /// <summary>
        /// Constructor for a run-off triangle containing only zeroes for a given number of periods.
        /// </summary>
        /// <param name="periods">Initial number of periods to be modeled.</param>
        /// <remarks>
        /// The two dimensional array containing the exosting claims is created in a jagged form to represent the triangluar "shape" of the data.
        /// </remarks>
        protected Triangle(int periods)
        {
            capacity = initialCapacity;

            while (capacity < periods)
            {
                capacity *= 2;
            }

            claims = new decimal[capacity][];

            for (int i = 0; i < capacity; i++)
            {
                claims[i] = new decimal[capacity - i];
            }

            Periods = periods;
        }

        /// <summary>
        /// Indexer of the ISliceable.
        /// </summary>
        /// <param name="row">Row where the value to be accessed resides.</param>
        /// <param name="column">Column where the value to be accessed resides.</param>
        /// <returns>Value that resides at the address provided by the given row and column.</returns>
        /// <exception cref="NegativePeriodException">Thrown when the row or column, representing accident and relative settlement period, is negative.</exception>
        /// <exception cref="ObservationPeriodExceedenceException">Thrown when the sum of the row and column, representing the absolute settlement period, exceeds the observation period.</exception>
        public decimal this[int row, int column]
        {
            get
            {
                if (row < 0 || column < 0)
                {
                    throw new NegativePeriodException();
                }

                if (row + column > Periods - 1)
                {
                    throw new ObservationPeriodExceedenceException();
                }

                return claims[row][column];
            }

            set
            {
                if (row < 0 || column < 0)
                {
                    throw new NegativePeriodException();
                }

                if (row + column > Periods - 1)
                {
                    throw new ObservationPeriodExceedenceException();
                }

                claims[row][column] = value;
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

            for (int i = 0; i < Periods - row; i++)
            {
                yield return claims[row][i];
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

            for (int i = 0; i < Periods - column; i++)
            {
                yield return claims[i][column];
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
        /// </remarks>
        public IEnumerable<decimal> GetDiagonal(int diagonal)
        {
            if (diagonal < 0)
            {
                throw new NegativePeriodException();
            }

            if (diagonal > Periods - 1)
            {
                throw new ObservationPeriodExceedenceException();
            }

            for (int i = 0; i < diagonal + 1; i++)
            {
                yield return claims[diagonal - i][i];
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

            if (n != Periods - row)
            {
                throw new DimensionMismatchException(Periods - row, n);
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
                claims[row][column] = val;
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

            if (n != Periods - column)
            {
                throw new DimensionMismatchException(Periods - column, n);
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
                claims[row][column] = val;
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

            if (n != diagonal + 1)
            {
                throw new DimensionMismatchException(diagonal + 1, n);
            }

            if (diagonal < 0)
            {
                throw new NegativePeriodException();
            }

            if (diagonal > Periods - 1)
            {
                throw new ObservationPeriodExceedenceException();
            }

            int row = diagonal;
            int column = 0;

            foreach (decimal val in values)
            {
                claims[row][column] = val;
                row--;
                column++;
            }
        }

        /// <summary>
        /// Creates a deep copy of the triangle.
        /// </summary>
        /// <returns>Deep copy of the triangle.</returns>
        public object Clone()
        {
            Triangle cloned = (Triangle)Activator.CreateInstance(this.GetType(), Periods);
            for (int i = 0; i < Periods; i++)
            {
                for (int j = 0; j < Periods - i; j++)
                {
                    cloned.claims[i][j] = claims[i][j];
                }
            }
            return cloned;
        }

        /// <summary>
        /// Adds a sequence of claims to the run-off triangle.
        /// </summary>
        /// <param name="values">Claims to be appended to the triangle.</param>
        /// <exception cref="DimensionMismatchException">Thrown when the element count of the new claims does not match the expected number of new claims.</exception>
        /// <remarks>
        /// While this method is virtual and could technically not be overriden in a derived class, it does not provide the functionality to actually append new claims
        /// to the run-off triangle, but rather only takes care of the validation, eventually neccessary resizing of the claims array and increment of the periods count.
        /// </remarks>
        public virtual void AddClaims(IEnumerable<decimal> values)
        {
            Periods++;

            int n = values.Count();

            if (n != Periods)
            {
                throw new DimensionMismatchException(Periods, n);
            }

            if (capacity < n)
            {
                capacity *= 2;

                Array.Resize(ref claims, capacity);

                for (int i = 0; i < capacity; i++)
                {
                    Array.Resize(ref claims[i], capacity - i);
                }
            }
        }

        /// <summary>
        /// Creates a string representation of the run-off triangle.
        /// </summary>
        /// <returns>String representation of the run-off triangle.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < Periods; i++)
            {
                for (int j = 0; j < Periods - i; j++)
                {
                    sb.Append(claims[i][j].ToString("0.00"));
                    if (j < Periods - i - 1)
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