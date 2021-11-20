using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoiseLibrary
{
    public class TrigNoise
    {
        private static Random RNG = new Random();

        private static List<double>[] constants = new List<double>[4];
        private static List<double>[] mults = new List<double>[4];

        private static int octaves;

        /// <summary>
        /// Generate the axes of noise using sin functions and constants
        /// </summary>
        /// <param name="octavesIn">Octaves to create</param>
        public static void Generate(int octavesIn)
        {
            octaves = octavesIn;

            constants = new List<double>[4];
            mults = new List<double>[4];

            for (int i = 0; i < constants.Length; i++)
            {
                constants[i] = new List<double>();
                mults[i] = new List<double>();

                for (int j = 0; j < octaves; j++)
                {
                    constants[i].Add(RNG.NextDouble() * 2 * Math.PI);
                    mults[i].Add(0.1 + (RNG.NextDouble() * 20.0));
                }
            }
        }

        public static double Evaluate(double x, double y, double persistence, double initialFreq)
        {
            double frequency = initialFreq;
            double amplitude = 1.0;

            double val = 0.0;
            double maxVal = 0.0;

            for (int i = 0; i < octaves; i++)
            {
                val += Noise(x * frequency, y * frequency, i) * amplitude;
                maxVal += amplitude;

                amplitude *= persistence;
                frequency *= 2;
            }

            return val / maxVal;
        }
        public static double Evaluate(double x, double y, double z, double persistence, double initialFreq)
        {
            double frequency = initialFreq;
            double amplitude = 1.0;

            double val = 0.0;
            double maxVal = 0.0;

            for (int i = 0; i < octaves; i++)
            {
                val += Noise(x * frequency, y * frequency, i) * amplitude;
                maxVal += amplitude;

                amplitude *= persistence;
                frequency *= 2;
            }

            return val / maxVal;
        }
        public static double Evaluate(double x, double y, double z, double w, double persistence, double initialFreq)
        {
            double frequency = initialFreq;
            double amplitude = 1.0;

            double val = 0.0;
            double maxVal = 0.0;

            for (int i = 0; i < octaves; i++)
            {
                val += Noise(x * frequency, y * frequency, i) * amplitude;
                maxVal += amplitude;

                amplitude *= persistence;
                frequency *= 2;
            }

            return val / maxVal;
        }

        // Evaluate axes with respect to input axes
        // Fade function used to stop the results of multiple axes trending towards 0 too easily
        public static double Noise(double x, double y, int octave)
        {
            double xVal = Math.Sin((x * mults[0][octave]) + constants[0][octave]);
            double yVal = Math.Sin((y * mults[1][octave]) + constants[1][octave]);

            return Fade((xVal + yVal) / 2.0);
        }
        public static double Noise(double x, double y, double z, int octave)
        {
            double xVal = Math.Sin((x * mults[0][octave]) + constants[0][octave]);
            double yVal = Math.Sin((y * mults[1][octave]) + constants[1][octave]);
            double zVal = Math.Sin((z * mults[2][octave]) + constants[2][octave]);

            return Fade((xVal + yVal + zVal) / 3.0);
        }
        public static double Noise(double x, double y, double z, double w, int octave)
        {
            double xVal = Math.Sin((x * mults[0][octave]) + constants[0][octave]);
            double yVal = Math.Sin((y * mults[1][octave]) + constants[1][octave]);
            double zVal = Math.Sin((z * mults[2][octave]) + constants[2][octave]);
            double wVal = Math.Sin((w * mults[3][octave]) + constants[3][octave]);

            return Fade((xVal + yVal + zVal + wVal) / 4.0);
        }

        // Taken from Adrian's Soapbox at https://adrianb.io/2014/08/09/perlinnoise.html
        public static double Fade(double t)
        {
            // Fade function as defined by Ken Perlin.  This eases coordinate values
            // so that they will ease towards integral values.  This ends up smoothing
            // the final output.
            return t * t * t * (t * (t * 6 - 15) + 10);         // 6t^5 - 15t^4 + 10t^3
        }
    }
}
