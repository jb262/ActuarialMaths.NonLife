using System;
using System.Linq;

namespace ActuarialMaths.NonLife.ClaimsReserving.Model
{
    /// <summary>
    /// Static converter class for the creation of triangles.
    /// </summary>
    /// <typeparam name="TTriangle">Explicit type of the triangle.</typeparam>
    public static class TriangleConverter<TTriangle> where TTriangle : ITriangle
    {
        /// <summary>
        /// Converts a given triangle to a triangle of the specified type.
        /// </summary>
        /// <param name="triangle">Triangle the output triangle is to be created from.</param>
        /// <returns>Triangle created from the input triangle.</returns>
        public static ITriangle Convert(ITriangle triangle)
        {
            switch (triangle)
            {
                case ITriangle t when t.GetType().Equals(typeof(TTriangle)):
                    return t;

                case CumulativeTriangle cumulative when typeof(TTriangle).Equals(typeof(IncrementalTriangle)):
                    ITriangle retIncr = new IncrementalTriangle(cumulative.Periods);
                    retIncr.SetColumn(cumulative.GetColumn(0), 0);
                    for (int i = 0; i < retIncr.Periods - 1; i++)
                    {
                        retIncr.SetColumn(cumulative
                            .GetColumn(i + 1)
                            .Zip(cumulative.GetColumn(i), (x, y) => x - y), i + 1);
                    }
                    return retIncr;

                case IncrementalTriangle incremental when typeof(TTriangle).Equals(typeof(CumulativeTriangle)):
                    ITriangle retCumul = new CumulativeTriangle(incremental.Periods);
                    retCumul.SetColumn(incremental.GetColumn(0), 0);
                    for (int i = 0; i < retCumul.Periods - 1; i++)
                    {
                        retCumul.SetColumn(retCumul.GetColumn(i)
                            .Take(retCumul.Periods - 1 - i)
                            .Zip(incremental.GetColumn(i + 1), (x, y) => x + y), i + 1);
                    }
                    return retCumul;

                default:
                    throw new NotSupportedException("A triangle of the desired type cannot be created from the given triangle.");
            }
        }
    }
}
