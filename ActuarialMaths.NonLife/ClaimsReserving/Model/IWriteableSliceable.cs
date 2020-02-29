using System.Collections.Generic;

namespace ActuarialMaths.NonLife.ClaimsReserving.Model
{
    /// <summary>
    /// Interface to model data that can be indexed and sliced by row, column and diagonal both to get and set values.
    /// </summary>
    /// <typeparam name="T">Type of the data to be stored in the IWriteableSliceable object.</typeparam>
    public interface IWriteableSliceable<T> : ISliceable<T>
    {
        /// <summary>
        /// Indexer of the ISliceable.
        /// </summary>
        /// <param name="row">Row where the value to be accessed resides.</param>
        /// <param name="column">Column where the value to be accessed resides.</param>
        /// <returns>Value that resides at the address provided by the given row and column.</returns>
        new T this[int row, int column] { get; set; }

        /// <summary>
        /// Sets a specific row of the object.
        /// </summary>
        /// <param name="values">Values the row is to be set to.</param>
        /// <param name="row">Index of the row to be set.</param>
        void SetRow(IEnumerable<T> values, int row);

        /// <summary>
        /// Sets a specific column of the object.
        /// </summary>
        /// <param name="values">Values the column is to be set to.</param>
        /// <param name="column">Index of the column to be set.</param>
        void SetColumn(IEnumerable<T> values, int column);

        /// <summary>
        /// Sets the main diagonal of the object.
        /// </summary>
        /// <param name="values">Values the diagonal is to be set to.</param>
        void SetDiagonal(IEnumerable<T> values);

        /// <summary>
        /// Sets a specific diagonal of the object.
        /// </summary>
        /// <param name="values">Values the diagonal is to be set to.</param>
        /// <param name="diagonal">Index of the diagonal to be set.</param>
        void SetDiagonal(IEnumerable<T> values, int diagonal);
    }
}
