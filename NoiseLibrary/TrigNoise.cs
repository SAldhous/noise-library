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
        private static List<int>[] functionType = new List<int>[4];

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
            functionType = new List<int>[4];

            for (int i = 0; i < constants.Length; i++)
            {
                constants[i] = new List<double>();
                mults[i] = new List<double>();
                functionType[i] = new List<int>();

                for (int j = 0; j < octaves; j++)
                {
                    constants[i].Add(RNG.NextDouble() * 2 * Math.PI);
                    mults[i].Add(0.5 + (RNG.NextDouble() * 40.0));
                    functionType[i].Add(RNG.Next(0, 2));
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
                val += Noise(new double[2]{x * frequency, y * frequency}, i) * amplitude;
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
                val += Noise(new double[3]{x * frequency, y * frequency, z * frequency}, i) * amplitude;
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
                val += Noise(new double[4]{x * frequency, y * frequency, z * frequency, w * frequency}, i) * amplitude;
                maxVal += amplitude;

                amplitude *= persistence;
                frequency *= 2;
            }

            return val / maxVal;
        }

        // Evaluate axes with respect to input axes
        public static double Noise(double[] components, int octave)
        {
            double val = 0.0;

            for (int i = 0; i < components.Length; i++)
            {
                if (functionType[i][octave] == 0) val += Math.Sin((components[i] * mults[i][octave]) + constants[i][octave]);
                else val += Math.Cos((components[i] * mults[i][octave]) + constants[i][octave]);
            }

            return val / components.Length;
        }
        /*public static double Noise(double x, double y, double z, int octave)
        {
            double xVal = Math.Sin((x * mults[0][octave]) + constants[0][octave]);
            double yVal = Math.Sin((y * mults[1][octave]) + constants[1][octave]);
            double zVal = Math.Sin((z * mults[2][octave]) + constants[2][octave]);

            return (xVal + yVal + zVal) / 3.0;
        }
        public static double Noise(double x, double y, double z, double w, int octave)
        {
            double xVal = Math.Sin((x * mults[0][octave]) + constants[0][octave]);
            double yVal = Math.Sin((y * mults[1][octave]) + constants[1][octave]);
            double zVal = Math.Sin((z * mults[2][octave]) + constants[2][octave]);
            double wVal = Math.Sin((w * mults[3][octave]) + constants[3][octave]);

            return (xVal + yVal + zVal + wVal) / 4.0;
        }*/

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
