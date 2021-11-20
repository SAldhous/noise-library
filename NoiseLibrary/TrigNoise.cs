using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace NoiseLibrary
{
    public class TrigNoise
    {
        private static Random RNG = new Random();

        private static List<double>[] constants = new List<double>[4];
        private static List<double>[] mults = new List<double>[4];
        private static List<double>[] amplitudes = new List<double>[4];
        private static List<int>[] signs = new List<int>[4];
        private static List<int>[] functionType = new List<int>[4];
        private static List<double> frequencies = new List<double>();

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
            amplitudes = new List<double>[4];
            signs = new List<int>[4];
            functionType = new List<int>[4];
            frequencies = new List<double>();

            double frequency = 1.0;

            for (int j = 0; j < octaves; j++)
            {
                frequencies.Add(frequency);
                frequency *= RandomDouble(1.93, 2.07);
                frequency += RandomDouble(0.0, 0.1);
            }

            for (int i = 0; i < constants.Length; i++)
            {
                constants[i] = new List<double>();
                mults[i] = new List<double>();
                amplitudes[i] = new List<double>();
                signs[i] = new List<int>();
                functionType[i] = new List<int>();

                for (int j = 0; j < octaves; j++)
                {
                    constants[i].Add(RandomDouble(2 * -Math.PI, 2 * Math.PI));
                    mults[i].Add(RandomDouble(1.5, 3.0));
                    amplitudes[i].Add(RandomDouble(0.2, 1.0));
                    signs[i].Add(RNG.Next(0, 2));
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

            Vector2 v = new Vector2((float)x, (float)y);
            int seed = v.ToString().GetHashCode();

            Random rng = new Random(seed);

            for (int i = 0; i < octaves; i++)
            {
                double valX = SampleFunction(x * frequency, 0, i) * amplitude;
                double valY = SampleFunction(y * frequency, 1, i) * amplitude;

                val += (valX + valY) / 2.0;

                double maxAmp = amplitudes[0][i];
                for (int axis = 0; axis < 2; axis++)
                {
                    if (amplitudes[axis][i] > maxAmp) maxAmp = amplitudes[axis][i];
                }

                maxVal += amplitude * maxAmp;

                amplitude *= persistence;

                frequency *= RandomDouble(rng, 1.5, 2.5);
                frequency += RandomDouble(rng, -0.2, 0.2);
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

        public static double SampleFunction(double position, int axis, int octave)
        {
            double sign = 1.0;
            if (signs[axis][octave] != 0) sign = -1.0;

            if (functionType[axis][octave] == 0) return Math.Sin((position * mults[axis][octave]) - (constants[axis][octave] * mults[axis][octave])) * amplitudes[axis][octave] * sign;
            else return Math.Cos((position * mults[axis][octave]) - (constants[axis][octave] * mults[axis][octave])) * amplitudes[axis][octave] * sign;
        }

        // Evaluate axes with respect to input axes
        public static double Noise(double[] components, int octave)
        {
            double val = 0.0;
            double maxVal = 0.0;

            for (int i = 0; i < components.Length; i++)
            {
                if (functionType[i][octave] == 0) val += Math.Sin((components[i] * mults[i][octave]) - (constants[i][octave] * mults[i][octave])) * amplitudes[i][octave];
                else val += Math.Cos((components[i] * mults[i][octave]) - (constants[i][octave] * mults[i][octave])) * amplitudes[i][octave];

                maxVal += amplitudes[i][octave];
            }

            return val / maxVal;
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

        public static double RandomDouble(double min, double max)
        {
            double randomVal = RNG.NextDouble();
            return min + randomVal * (max - min);
        }
        public static double RandomDouble(Random rng, double min, double max)
        {
            double randomVal = rng.NextDouble();
            return min + randomVal * (max - min);
        }
    }
}
