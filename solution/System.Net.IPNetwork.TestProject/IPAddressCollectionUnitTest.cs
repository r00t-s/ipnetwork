﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections;
using System.Numerics;

namespace System.Net.TestProject {
    /// <summary>
    /// IPNetworkUnitTest test every single method
    /// </summary>
    [TestClass]
    public class IPAddressCollectionUnitTest {

        #region Parse

        [TestMethod]
        public void TestAtIndexIPAddress() {

            var ipn = IPNetwork.Parse("192.168.1.0/29");
            using (var ips = IPNetwork.ListIPAddress(ipn)) {
                Assert.AreEqual("192.168.1.0", ips[0].ToString(), "0");
                Assert.AreEqual("192.168.1.1", ips[1].ToString(), "1");
                Assert.AreEqual("192.168.1.2", ips[2].ToString(), "2");
                Assert.AreEqual("192.168.1.3", ips[3].ToString(), "3");
                Assert.AreEqual("192.168.1.4", ips[4].ToString(), "4");
                Assert.AreEqual("192.168.1.5", ips[5].ToString(), "5");
                Assert.AreEqual("192.168.1.6", ips[6].ToString(), "6");
                Assert.AreEqual("192.168.1.7", ips[7].ToString(), "7");
            }

        }

        [TestMethod]
        public void TestIterateIPAddress() {

            var ipn = IPNetwork.Parse("192.168.1.0/29");
            IPAddress last = null;
            IPAddress fisrt = null;
            var count = 0;
            using (var ips = IPNetwork.ListIPAddress(ipn)) {
                foreach (var ip in ips) {
                    if (fisrt == null) fisrt = ip;
                    last = ip;
                    count++;
                }
                Assert.IsNotNull(last, "last is null");
                Assert.IsNotNull(fisrt, "fisrt is null");
                Assert.AreEqual("192.168.1.0", fisrt.ToString(), "first");
                Assert.AreEqual("192.168.1.7", last.ToString(), "last");
                Assert.AreEqual(8, count, "count");
            }

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestOutOfRangeIPAddress() {

            var ipn = IPNetwork.Parse("192.168.1.0/29");
            using (var ips = IPNetwork.ListIPAddress(ipn)) {
                Console.Write("This is out of range : {0} ", ips[8]);
            }

        }

        [TestMethod]
        public void TestCountIPAddress() {

            var ipn = IPNetwork.Parse("192.168.1.0/29");
            using (var ips = IPNetwork.ListIPAddress(ipn)) {
                Assert.AreEqual(8, ips.Count, "Count");
            }
        }

        [TestMethod]
        public void TestReset() {

            var ipn = IPNetwork.Parse("192.168.1.0/29");
            using (var ips = IPNetwork.ListIPAddress(ipn)) {
                ips.Reset();
            }
        }

        [TestMethod]
        public void TestResetEnumerator() {

            var ipn = IPNetwork.Parse("192.168.1.0/29");
            using (IEnumerator<IPAddress> ips = IPNetwork.ListIPAddress(ipn)) {
                ips.Reset();
                while (ips.MoveNext()) {
                    Assert.IsNotNull(ips.Current);
                }
                ips.Reset();
                while (ips.MoveNext()) {
                    Assert.IsNotNull(ips.Current);
                }

            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestEnumeratorFailed() {

            var ipn = IPNetwork.Parse("192.168.1.0/29");
            using (IEnumerator<IPAddress> ips = IPNetwork.ListIPAddress(ipn)) {
                ips.Reset();
                while (ips.MoveNext()) {
                    Assert.IsNotNull(ips.Current);
                }
                Console.WriteLine("This is out of range : {0}", ips.Current);

            }
        }

        [TestMethod]
        public void TestEnumeratorMoveNext() {

            var ipn = IPNetwork.Parse("192.168.1.0/29");
            using (IEnumerator<IPAddress> ips = IPNetwork.ListIPAddress(ipn)) {
                Assert.IsTrue(ips.MoveNext());
                Assert.IsTrue(ips.MoveNext());
                Assert.IsTrue(ips.MoveNext());
                Assert.IsTrue(ips.MoveNext());
                Assert.IsTrue(ips.MoveNext());
                Assert.IsTrue(ips.MoveNext());
                Assert.IsTrue(ips.MoveNext());
                Assert.IsTrue(ips.MoveNext());
                Assert.IsFalse(ips.MoveNext());
                Assert.IsFalse(ips.MoveNext());


            }
        }

        [TestMethod]
        public void TestEnumeratorMoveNext2() {

            var ipn = IPNetwork.Parse("192.168.1.0/31");
            using (IEnumerator<IPAddress> ips = IPNetwork.ListIPAddress(ipn)) {
                Assert.IsTrue(ips.MoveNext());
                Assert.IsTrue(ips.MoveNext());
                Assert.IsFalse(ips.MoveNext());
                ips.Reset();
                Assert.IsTrue(ips.MoveNext());
                Assert.IsTrue(ips.MoveNext());
                Assert.IsFalse(ips.MoveNext());


            }
        }

        [TestMethod]
        public void TestEnumeratorCurrent() {

            var ipn = IPNetwork.Parse("192.168.1.0/31");
            IEnumerator ips = IPNetwork.ListIPAddress(ipn);
            Assert.IsNotNull(ips.Current);
            Assert.IsTrue(ips.MoveNext());
            Assert.IsNotNull(ips.Current);
            Assert.IsTrue(ips.MoveNext());
            Assert.IsFalse(ips.MoveNext());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestEnumeratorCurrentOor() {

            var ipn = IPNetwork.Parse("192.168.1.0/31");
            IEnumerator ips = IPNetwork.ListIPAddress(ipn);
            Assert.IsNotNull(ips.Current);
            Assert.IsTrue(ips.MoveNext());
            Assert.IsNotNull(ips.Current);
            Assert.IsTrue(ips.MoveNext());
            Assert.IsFalse(ips.MoveNext());
            Console.WriteLine("This is out of range : {0} ", ips.Current);
        }

        [TestMethod]
        public void TestEnumeratorIterate() {

            var ipn = IPNetwork.Parse("192.168.1.0/31");
            IEnumerator ips = IPNetwork.ListIPAddress(ipn);
            while (ips.MoveNext()) {
                Assert.IsNotNull(ips.Current);
            }
        }


        #endregion

        #region IPv6


        [TestMethod]
        public void Test_ipv6_AtIndexIPAddress() {

            var ipn = IPNetwork.Parse("::/125");
            using (var ips = IPNetwork.ListIPAddress(ipn)) {
                Assert.AreEqual("::", ips[0].ToString(), "0");
                Assert.AreEqual("::1", ips[1].ToString(), "1");
                Assert.AreEqual("::2", ips[2].ToString(), "2");
                Assert.AreEqual("::3", ips[3].ToString(), "3");
                Assert.AreEqual("::4", ips[4].ToString(), "4");
                Assert.AreEqual("::5", ips[5].ToString(), "5");
                Assert.AreEqual("::6", ips[6].ToString(), "6");
                Assert.AreEqual("::7", ips[7].ToString(), "7");
            }

        }

        [TestMethod]
        public void Test_ipv6_IterateIPAddress() {

            var ipn = IPNetwork.Parse("::/125");
            IPAddress last = null;
            IPAddress fisrt = null;
            var count = 0;
            using (var ips = IPNetwork.ListIPAddress(ipn)) {
                foreach (var ip in ips) {
                    if (fisrt == null) fisrt = ip;
                    last = ip;
                    count++;
                }
                Assert.IsNotNull(last, "last is null");
                Assert.IsNotNull(fisrt, "fisrt is null");
                Assert.AreEqual("::", fisrt.ToString(), "first");
                Assert.AreEqual("::7", last.ToString(), "last");
                Assert.AreEqual(8, count, "count");
            }

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Test_ipv6_OutOfRangeIPAddress() {

            var ipn = IPNetwork.Parse("::/125");
            using (var ips = IPNetwork.ListIPAddress(ipn)) {
                Console.Write("This is out of range : {0} ", ips[8]);
            }

        }

        [TestMethod]
        public void Test_ipv6_CountIPAddress() {

            var ipn = IPNetwork.Parse("::/125");
            using (var ips = IPNetwork.ListIPAddress(ipn)) {
                Assert.AreEqual(8, ips.Count, "Count");
            }
        }

        [TestMethod]
        public void Test_ipv6_CountIPAddress2() {

            var ipn = IPNetwork.Parse("::/0");
            var max = BigInteger.Pow(2, 128);
            using (var ips = IPNetwork.ListIPAddress(ipn)) {
                Assert.AreEqual(max, ips.Count, "Count");
            }
        }

        [TestMethod]
        public void Test_ipv6_Reset() {

            var ipn = IPNetwork.Parse("::/125");
            using (var ips = IPNetwork.ListIPAddress(ipn)) {
                ips.Reset();
            }
        }

        [TestMethod]
        public void Tes_ipv6_tResetEnumerator() {

            var ipn = IPNetwork.Parse("::/125");
            using (IEnumerator<IPAddress> ips = IPNetwork.ListIPAddress(ipn)) {
                ips.Reset();
                while (ips.MoveNext()) {
                    Assert.IsNotNull(ips.Current);
                }
                ips.Reset();
                while (ips.MoveNext()) {
                    Assert.IsNotNull(ips.Current);
                }

            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Test_ipv6_EnumeratorFailed() {

            var ipn = IPNetwork.Parse("::/125");
            using (IEnumerator<IPAddress> ips = IPNetwork.ListIPAddress(ipn)) {
                ips.Reset();
                while (ips.MoveNext()) {
                    Assert.IsNotNull(ips.Current);
                }
                Console.WriteLine("This is out of range : {0}", ips.Current);

            }
        }

        [TestMethod]
        public void Test_ipv6_EnumeratorMoveNext() {

            var ipn = IPNetwork.Parse("::/125");
            using (IEnumerator<IPAddress> ips = IPNetwork.ListIPAddress(ipn)) {
                Assert.IsTrue(ips.MoveNext());
                Assert.IsTrue(ips.MoveNext());
                Assert.IsTrue(ips.MoveNext());
                Assert.IsTrue(ips.MoveNext());
                Assert.IsTrue(ips.MoveNext());
                Assert.IsTrue(ips.MoveNext());
                Assert.IsTrue(ips.MoveNext());
                Assert.IsTrue(ips.MoveNext());
                Assert.IsFalse(ips.MoveNext());
                Assert.IsFalse(ips.MoveNext());


            }
        }

        [TestMethod]
        public void Test_ipv6_EnumeratorMoveNext2() {

            var ipn = IPNetwork.Parse("::/127");
            using (IEnumerator<IPAddress> ips = IPNetwork.ListIPAddress(ipn)) {
                Assert.IsTrue(ips.MoveNext());
                Assert.IsTrue(ips.MoveNext());
                Assert.IsFalse(ips.MoveNext());
                ips.Reset();
                Assert.IsTrue(ips.MoveNext());
                Assert.IsTrue(ips.MoveNext());
                Assert.IsFalse(ips.MoveNext());


            }
        }

        [TestMethod]
        public void Test_ipv6_EnumeratorCurrent() {

            var ipn = IPNetwork.Parse("::/127");
            IEnumerator ips = IPNetwork.ListIPAddress(ipn);
            Assert.IsNotNull(ips.Current);
            Assert.IsTrue(ips.MoveNext());
            Assert.IsNotNull(ips.Current);
            Assert.IsTrue(ips.MoveNext());
            Assert.IsFalse(ips.MoveNext());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Test_ipv6_EnumeratorCurrentOor() {

            var ipn = IPNetwork.Parse("::/127");
            IEnumerator ips = IPNetwork.ListIPAddress(ipn);
            Assert.IsNotNull(ips.Current);
            Assert.IsTrue(ips.MoveNext());
            Assert.IsNotNull(ips.Current);
            Assert.IsTrue(ips.MoveNext());
            Assert.IsFalse(ips.MoveNext());
            Console.WriteLine("This is out of range : {0} ", ips.Current);
        }

        [TestMethod]
        public void Test_ipv6_EnumeratorIterate() {

            var ipn = IPNetwork.Parse("::/127");
            IEnumerator ips = IPNetwork.ListIPAddress(ipn);
            while (ips.MoveNext()) {
                Assert.IsNotNull(ips.Current);
            }
        }

        #endregion

    }
}
