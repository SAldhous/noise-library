using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoiseLibrary;

namespace LibraryTesting
{
    [TestClass]
    public class WorleyTests
    {
        [TestMethod]
        public void Evaluate4D()
        {
            bool flag = true;

            try
            {
                SimplexNoise.Init();

                for (int w = 0; w < 10; w++)
                {
                    for (int z = 0; z < 10; z++)
                    {
                        for (int y = 0; y < 10; y++)
                        {
                            for (int x = 0; x < 10; x++)
                            {
                                double val = WorleyNoise.Evaluate(x, y, z, w, 5, 0.3, 1.0);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                flag = false;
            }

            Assert.IsTrue(flag);
        }

        [TestMethod]
        public void Evaluate3D()
        {
            bool flag = true;

            try
            {
                SimplexNoise.Init();

                for (int z = 0; z < 10; z++)
                {
                    for (int y = 0; y < 10; y++)
                    {
                        for (int x = 0; x < 10; x++)
                        {
                            double val = WorleyNoise.Evaluate(x, y, z, 5, 0.3, 1.0);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                flag = false;
            }

            Assert.IsTrue(flag);
        }

        [TestMethod]
        public void Evaluate2D()
        {
            bool flag = true;

            try
            {
                SimplexNoise.Init();

                for (int y = 0; y < 10; y++)
                {
                    for (int x = 0; x < 10; x++)
                    {
                        double val = WorleyNoise.Evaluate(x, y, 5, 0.3, 1.0);
                    }
                }
            }
            catch (Exception e)
            {
                flag = false;
            }

            Assert.IsTrue(flag);
        }
    }
}
