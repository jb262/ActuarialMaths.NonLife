using System.Collections.Generic;

namespace ActuarialMaths.NonLife.ClaimsReserving.Model
{
    /// <summary>
    /// Interface to model data that can be indexed and sliced by row, column and diagonal to get values.
    /// </summary>
    /// <typeparam name="T">Type of the data to be stored in the ISliceable object.</typeparam>
    public interface ISliceable<T>
    {
        /// <summary>
        /// Indexer of the ISliceable.
        /// </summary>
        /// <param name="row">Row where the value to be accessed resides.</param>
        /// <param name="column">Column where the value to be accessed resides.</param>
        /// <returns>Value that resides at the address provided by the given row and column.</returns>
        T this[int row, int column] { get; }

        /// <summary>
        /// Provides a specific row of the object.
        /// </summary>
        /// <param name="row">Index of the row to be accessed.</param>
        /// <returns>Specified row of the object.</returns>
        IEnumerable<T> GetRow(int row);

        /// <summary>
        /// Provides a specific column of the object.
        /// </summary>
        /// <param name="colum">Index of the column to be accessed.</param>
        /// <returns>Specified column of the object.</returns>
        IEnumerable<T> GetColumn(int column);

        /// <summary>
        /// Provides the main diagonal of the object.
        /// </summary>
        /// <returns>Main diagonal of the object.</returns>
        IEnumerable<T> GetDiagonal();

        /// <summary>
        /// Provides a specific diagonal of the object.
        /// </summary>
        /// <param name="diagonal">Index of the diagonal to be accessed.</param>
        /// <returns>Specified diagonal of the object.</returns>
        IEnumerable<T> GetDiagonal(int diagonal);
    }
}
