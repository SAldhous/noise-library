using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace NoiseLibrary
{
    public class WorleyNoise
    {
        private static Random RNG = new Random();

        private static Vector<double>[] grad4 = new Vector<double>[32];

        private static short[] p = {151,160,137,91,90,15,
        131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
        190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
        88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
        77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
        102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
        135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
        5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
        223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
        129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
        251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
        49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
        138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180};
        // To remove the need for index wrapping, double the permutation table length
        private static short[] perm = new short[512];
        private static short[] permMod12 = new short[512];

        public static void Init()
        {
            for (int i = 0; i < 512; i++)
            {
                perm[i] = p[i & 255];
                permMod12[i] = (short)(perm[i] % 12);
            }

            for (int i = 0; i < 32; i++)
            {
                double x = RNG.NextDouble();
                double y = RNG.NextDouble();
                double z = RNG.NextDouble();
                double w = RNG.NextDouble();

                grad4[i] = new Vector<double>(new double[4] { x, y, z, w } );
            }
        }

        public static double Evaluate(double x, double y, int octaves, double persistence, double initialFreq)
        {
            double frequency = initialFreq;
            double amplitude = 1.0;

            double val = 0.0;
            double maxVal = 0.0;

            for (int i = 0; i < octaves; i++)
            {
                val += Math.Clamp(Noise(x * frequency, y * frequency), 0.0, 1.0) * amplitude;
                maxVal += amplitude;

                amplitude *= persistence;
                frequency *= 2;
            }

            return val / maxVal;
        }

        public static double Evaluate(double x, double y, double z, int octaves, double persistence, double initialFreq)
        {
            double frequency = initialFreq;
            double amplitude = 1.0;

            double val = 0.0;
            double maxVal = 0.0;

            for (int i = 0; i < octaves; i++)
            {
                val += Math.Clamp(Noise(x * frequency, y * frequency, z * frequency), 0.0, 1.0) * amplitude;
                maxVal += amplitude;

                amplitude *= persistence;
                frequency *= 2;
            }

            return val / maxVal;
        }

        public static double Evaluate(double x, double y, double z, double w, int octaves, double persistence, double initialFreq)
        {
            double frequency = initialFreq;
            double amplitude = 1.0;

            double val = 0.0;
            double maxVal = 0.0;

            for (int i = 0; i < octaves; i++)
            {
                val += Math.Clamp(Noise(x * frequency, y * frequency, z * frequency, w * frequency), 0.0, 1.0) * amplitude;
                maxVal += amplitude;

                amplitude *= persistence;
                frequency *= 2;
            }

            return val / maxVal;
        }

        public static double Noise(double x, double y)
        {
            Vector<double> sampleCoords = new Vector<double>(new double[4] { x, y, 0.0, 0.0 });

            double shortest = double.MaxValue;

            for (int Y = -1; Y <= 1; Y++)
            {
                for (int X = -1; X <= 1; X++)
                {
                    double flooredX = Math.Floor(x + X);
                    double flooredY = Math.Floor(y + Y);

                    int i = (int)flooredX & 255;
                    int j = (int)flooredY & 255;

                    int hash = (perm[i] + perm[j]) % 32;
                    Vector<double> v = grad4[hash] + new Vector<double>(new double[4] { flooredX, flooredY, 0.0, 0.0} );

                    Vector<double> dist = v - sampleCoords;
                    double distSqr = (dist[0] * dist[0]) + (dist[1] * dist[1]);

                    if (distSqr < shortest) shortest = distSqr;
                }
            }

            return Math.Sqrt(shortest);
        }

        public static double Noise(double x, double y, double z)
        {
            Vector<double> sampleCoords = new Vector<double>(new double[4] { x, y, z, 0.0 });

            double shortest = double.MaxValue;

            for (int Z = -1; Z <= 1; Z++)
            {
                for (int Y = -1; Y <= 1; Y++)
                {
                    for (int X = -1; X <= 1; X++)
                    {
                        double flooredX = Math.Floor(x + X);
                        double flooredY = Math.Floor(y + Y);
                        double flooredZ = Math.Floor(z + Z);

                        int i = (int)flooredX & 255;
                        int j = (int)flooredY & 255;
                        int k = (int)flooredZ & 255;

                        int hash = (perm[i] + perm[j] + perm[k]) % 32;
                        Vector<double> v = grad4[hash] + new Vector<double>(new double[4] { flooredX, flooredY, flooredZ, 0.0 });

                        Vector<double> dist = v - sampleCoords;
                        double distSqr = (dist[0] * dist[0]) + (dist[1] * dist[1]) + (dist[2] * dist[2]);

                        if (distSqr < shortest) shortest = distSqr;
                    }
                }
            }

            return Math.Sqrt(shortest);
        }

        public static double Noise(double x, double y, double z, double w)
        {
            Vector<double> sampleCoords = new Vector<double>(new double[4] { x, y, z, w });

            double shortest = double.MaxValue;

            for (int W = -1; W <= 1; W++)
            {
                for (int Z = -1; Z <= 1; Z++)
                {
                    for (int Y = -1; Y <= 1; Y++)
                    {
                        for (int X = -1; X <= 1; X++)
                        {
                            double flooredX = Math.Floor(x + X);
                            double flooredY = Math.Floor(y + Y);
                            double flooredZ = Math.Floor(z + Z);
                            double flooredW = Math.Floor(w + W);

                            int i = (int)flooredX & 255;
                            int j = (int)flooredY & 255;
                            int k = (int)flooredZ & 255;
                            int l = (int)flooredW & 255;

                            int hash = (perm[i] + perm[j] + perm[k] + perm[l]) % 32;
                            Vector<double> v = grad4[hash] + new Vector<double>(new double[4] { flooredX, flooredY, flooredZ, flooredW });

                            Vector<double> dist = v - sampleCoords;
                            double distSqr = (dist[0] * dist[0]) + (dist[1] * dist[1]) + (dist[2] * dist[2]) + (dist[3] * dist[3]);

                            if (distSqr < shortest) shortest = distSqr;
                        }
                    }
                }
            }

            return Math.Sqrt(shortest);
        }
    }
}
