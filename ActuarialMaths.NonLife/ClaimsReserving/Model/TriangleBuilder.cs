using System;
using System.Collections.Generic;

namespace ActuarialMaths.NonLife.ClaimsReserving.Model
{
    /// <summary>
    /// Static factory class for the creation of triangles.
    /// </summary>
    /// <typeparam name="TTriangle">Explicit type of the triangle.</typeparam>
    public static class TriangleBuilder<TTriangle> where TTriangle : ITriangle
    {
        /// <summary>
        /// Creates a triangle of the specified type from a given triangle.
        /// </summary>
        /// <param name="triangle">Triangle the output triangle is to be created from.</param>
        /// <returns>Triangle created from the input triangle.</returns>
        public static ITriangle CreateFrom(ITriangle triangle)
        {
            switch (triangle)
            {
                case ITriangle t when t.GetType().Equals(typeof(TTriangle)):
                    return t;
                case CumulativeTriangle cumulative when typeof(TTriangle).Equals(typeof(IncrementalTriangle)):
                    return cumulative.ToIncrementalTriangle();
                case IncrementalTriangle incremental when typeof(TTriangle).Equals(typeof(CumulativeTriangle)):
                    return incremental.ToCumulativeTriangle();
                default:
                    throw new NotSupportedException("A triangle of the desired type cannot be created from the given triangle.");
            }
        }

        /// <summary>
        /// Creates an empty triangle.
        /// </summary>
        /// <returns>A completely empty triangle with zero periods.</returns>
        public static ITriangle CreateEmpty()
        {
            return (ITriangle)Activator.CreateInstance(typeof(TTriangle));
        }

        /// <summary>
        /// Creates a triangle with the given number of periods.
        /// </summary>
        /// <param name="periods">Number of periods the triangle is supposed to have.</param>
        /// <returns>An empty triangle with the given number of periods.</returns>
        public static ITriangle Create(int periods)
        {
            return (ITriangle)Activator.CreateInstance(typeof(TTriangle), periods);
        }

        /// <summary>
        /// Creates a triangle with the given claims on the diagonals.
        /// </summary>
        /// <param name="claims">Claims to be developed in the run-off-triangle.</param>
        /// <returns>Run-off triangle of the specified type with the given claims.</returns>
        public static ITriangle Create(params IEnumerable<decimal>[] claims)
        {
            ITriangle triangle = CreateEmpty();

            foreach (IEnumerable<decimal> claim in claims)
            {
                triangle.AddClaims(claim);
            }

            return triangle;
        }
    }
}
