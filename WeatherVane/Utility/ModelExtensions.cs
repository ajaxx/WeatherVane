using System;

using WeatherVane.Model;

namespace WeatherVane.Utility
{
    public static class ModelExtensions
    {
        /// <summary>
        /// Parses a temperature.
        /// </summary>
        /// <param name="valueAsString">The value as string.</param>
        /// <returns></returns>
        public static Temperature ParseTemperature(string valueAsString)
        {
            try
            {
                return new Temperature(Double.Parse(valueAsString));
            }
            catch (FormatException)
            {
                return null;
            }
        }

        /// <summary>
        /// Parses a velocity.
        /// </summary>
        /// <param name="valueAsString">The value as string.</param>
        /// <returns></returns>
        public static Velocity ParseVelocity(string valueAsString)
        {
            try
            {
                return new Velocity(Decimal.Parse(valueAsString));
            }
            catch (FormatException)
            {
                return null;
            }
        }
    }
}
