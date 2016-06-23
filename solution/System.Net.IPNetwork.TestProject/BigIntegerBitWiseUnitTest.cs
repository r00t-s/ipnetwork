﻿using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Net.TestProject
{
    [TestClass]
    public class BigIntegerBitWiseUnitTest
    {
        [TestMethod]
        public void Test1()
        {
            var bytes = new byte[] {0xFF, 0xFF, 0xFF, 0xFF, 0x00};
            var reverseme = new BigInteger(bytes);
            var reversed = reverseme.PositiveReverse(4);

            Assert.AreEqual(0, (int) reversed);
        }

        [TestMethod]
        public void Test2()
        {
            byte[] bytes = {0xFF, 0xFF, 0xFF, 0xFF, 0x00};
            var reverseme = new BigInteger(bytes);
            var reversed = reverseme.PositiveReverse(8);
            var result = reversed.ToByteArray();

            Assert.AreEqual(0x0, result[0]);
            Assert.AreEqual(0x0, result[1]);
            Assert.AreEqual(0x0, result[2]);
            Assert.AreEqual(0x0, result[3]);
            Assert.AreEqual(0xFF, result[4]);
            Assert.AreEqual(0xFF, result[5]);
            Assert.AreEqual(0xFF, result[6]);
            Assert.AreEqual(0xFF, result[7]);
        }
    }
}