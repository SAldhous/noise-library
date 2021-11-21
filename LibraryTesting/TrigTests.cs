using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoiseLibrary;

namespace LibraryTesting
{
    [TestClass]
    public class TrigTests
    {
        [TestMethod]
        public void Evaluate4D()
        {
            bool flag = true;

            try
            {
                TrigNoise.Generate(5);

                for (int w = 0; w < 10; w++)
                {
                    for (int z = 0; z < 10; z++)
                    {
                        for (int y = 0; y < 10; y++)
                        {
                            for (int x = 0; x < 10; x++)
                            {
                                double val = TrigNoise.Evaluate(x, y, z, w, 0.3, 1.0);
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
                TrigNoise.Generate(5);

                for (int z = 0; z < 10; z++)
                {
                    for (int y = 0; y < 10; y++)
                    {
                        for (int x = 0; x < 10; x++)
                        {
                            double val = TrigNoise.Evaluate(x, y, z, 0.3, 1.0);
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
                TrigNoise.Generate(5);

                for (int y = 0; y < 10; y++)
                {
                    for (int x = 0; x < 10; x++)
                    {
                        double val = TrigNoise.Evaluate(x, y, 0.3, 1.0);
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
