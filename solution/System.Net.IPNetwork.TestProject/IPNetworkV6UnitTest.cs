using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Net.TestProject
{
    [TestClass]
    public class IPNetworkV6UnitTest
    {
        #region TryToCidr

        [TestMethod]
        public void TestTryToCidr128To0()
        {
            var mask = new List<string>
            {
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:fffe",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:fffc",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:fff8",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:fff0",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffe0",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffc0",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ff80",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ff00",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:fe00",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:fc00",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:f800",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:f000",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:e000",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:c000",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:8000",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:0000",
                "ffff:ffff:ffff:ffff:ffff:ffff:fffe:0",
                "ffff:ffff:ffff:ffff:ffff:ffff:fffc:0",
                "ffff:ffff:ffff:ffff:ffff:ffff:fff8:0",
                "ffff:ffff:ffff:ffff:ffff:ffff:fff0:0",
                "8000::",
                "::"
            };
            var expectedcidr = new List<byte>
            {
                128,
                127,
                126,
                125,
                124,
                123,
                122,
                121,
                120,
                119,
                118,
                117,
                116,
                115,
                114,
                113,
                112,
                111,
                110,
                109,
                108,
                1,
                0
            };

            for (var i = 0; i < mask.Count; i++)
            {
                byte? result;
                var parsed = IPNetwork.TryToCidr(IPAddress.Parse(mask[i]), out result);
                Assert.AreEqual(true, parsed, "parsed");
                Assert.AreEqual(expectedcidr[i], result, "cidr");
            }
        }

        #endregion

        #region ToBigInteger

        [TestMethod]
        public void TestToBigInteger10()
        {
            var mask = new List<string> {"::f", "::fff", "::ff", "::ff00:0", "::"};
            var uintMask = new List<uint> {0xf, 0xfff, 0xff, 0xff000000, 0x00000000};
            var index = 0;
            foreach (var result in mask.Select(masktotest => IPNetwork.ToBigInteger(IPAddress.Parse(mask[index]))))
            {
                Assert.AreEqual(uintMask[index], result, "uint");
                index++;
            }
        }

        #endregion

        #region TryToBigInteger

        [TestMethod]
        public void TestTryToBigInteger()
        {
            var mask = new List<string> {"::ffff:ffff", "::ffff:ff00", "::ffff:0", "::ff00:0", "::"};
            var uintMask = new List<uint> {0xffffffff, 0xffffff00, 0xffff0000, 0xff000000, 0x00000000};

            for (var i = 0; i < mask.Count; i++)
            {
                BigInteger? result;
                var parsed = IPNetwork.TryToBigInteger(IPAddress.Parse(mask[i]), out result);
                Assert.AreEqual(uintMask[i], result, "uint");
                Assert.AreEqual(true, parsed, "parsed");
            }
        }

        #endregion

        #region ValidNetmask

        [TestMethod]
        public void TestValidNetmask()
        {
            var mask = new List<string>
            {
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:fff0",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:0000",
                "ffff:ffff:ffff:ffff:ffff:ffff:0000:ffff",
                "ffff:ffff:ffff:ffff:ffff:ffff:0000:0001"
            };
            var expectedvalue = new List<bool> {true, true, true, false, false};
            var index = 0;
            foreach (var result in mask.Select(masktotest => IPNetwork.ValidNetmask(IPAddress.Parse(masktotest))))
            {
                Assert.AreEqual(expectedvalue[index], result, "ValidNetmask");
                index++;
            }
        }

        #endregion

        #region BitsSet

        [TestMethod]
        public void TestBitsSet()
        {
            var ip = new List<string>
            {
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:fff0",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:0000",
                "f0f0:f0f0:f0f0:f0f0:f0f0:f0f0:f0f0:f0f0"
            };
            var bits = new List<uint> {128, 124, 112, 64};
            var index = 0;
            foreach (var result in ip.Select(iptotest => IPNetwork.BitsSet(IPAddress.Parse(iptotest))))
            {
                Assert.AreEqual(bits[index], result, "BitsSet");
                index++;
            }
        }

        #endregion

        #region TryGuessCidr

        [TestMethod]
        public void TestTryGuessCidr()
        {
            var toparse = new List<string> {null, "::", "2001:0db8::"};
            var diditparse = new List<bool> {false, true, true};
            var expectedresult = new List<int> {0, 64, 64};
            for (var i = 0; i < toparse.Count; i++)
            {
                byte cidr;
                var parsed = IPNetwork.TryGuessCidr(toparse[i], out cidr);
                Assert.AreEqual(diditparse[i], parsed, "parsed");
                Assert.AreEqual(expectedresult[i], cidr, "cidr");
            }
        }

        #endregion

        #region Count

        [TestMethod]
        public void TestTotals()
        {
            var network = new List<string> {"::/128", "::/127", "::/126", "::/120", "::/112", "::/104", "::/0"};
            var total = new List<BigInteger> {1, 2, 4, 256, 65536, 16777216, BigInteger.Pow(2, 128)};
            var index = 0;
            foreach (var testtotal in network.Select(IPNetwork.Parse))
            {
                Assert.AreEqual(total[index], testtotal.Total, "Total");
                index++;
            }
        }

        #endregion

        #region Usable

        [TestMethod]
        public void TestUsable()
        {
            var network = new List<string> {"::/128", "::/127", "::/126", "::/120", "::/112", "::/104", "::/0"};
            var usable = new List<BigInteger> {1, 2, 4, 256, 65536, 16777216, BigInteger.Pow(2, 128)};
            for (var i = 0; i < network.Count; i++)
            {
                var networktotest = IPNetwork.Parse(network[i]);
                Assert.AreEqual(usable[i], networktotest.Usable, "Usable");
            }
        }

        #endregion

        #region TryParseCidr

        [TestMethod]
        public void TryParseCidr()
        {
            var sidr = new List<string> {"0", "sadsd", "33", "128", "129"};
            var result = new List<byte?> {0, null, 33, 128, null};

            for (var i = 9; i < sidr.Count; i++)
            {
                byte? cidr;
                var parsed = IPNetwork.TryParseCidr(sidr[i], AddressFamily.InterNetworkV6, out cidr);
                Assert.AreEqual(result[i], cidr, "cidr");
                switch (result[i])
                {
                    case null:
                        Assert.AreEqual(false, parsed, "parsed");
                        break;
                    default:
                        Assert.AreEqual(true, parsed, "parsed");
                        break;
                }
            }
        }

        #endregion

        #region ParseIP

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void TestParseIPAddressNetmaskANE8()
        {
            var ipnetworkstotest = new List<string[]>
            {
                new[] {"xxxx::", "xxxx::"},
                new[] {"::", "xxxx::"}
            };
            foreach (var ipnet in ipnetworkstotest.Select(t => IPNetwork.Parse(t[0], t[1])))
            {
            }
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void TestParseIPAddressNetmaskANE10()
        {
            var ipnet = IPNetwork.Parse("xxxx::", 0);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void TestParseIPAddressNetmaskANE11()
        {
            var ipnet = IPNetwork.Parse("::", 129);
        }

        [TestMethod]
        public void TestParsev6_CIDR()
        {
            var ipaddress = new List<string>
            {
                "2001:db8::",
                "2001:db8::",
                "2001:db8::",
                "2001:db8::",
                "2001:db8::",
                "2001:0db8::",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff",
                "ffff:ffff:ffff:ffff:1234:1234:1234:1234",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff"
            };

            var netmask = new List<string>
            {
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:fffe",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:fffc",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:fff8",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:fff0",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffe0",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:0000",
                "ffff:ffff:ffff:ffff:0000:0000:0000:0000",
                "ffff:0000:0000:0000:0000:0000:0000:0000"
            };

            var network = new List<string>
            {
                "2001:db8::",
                "2001:db8::",
                "2001:db8::",
                "2001:db8::",
                "2001:db8::",
                "2001:db8::",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:0",
                "ffff:ffff:ffff:ffff::",
                "ffff::"
            };

            var netmask2 = new List<string>
            {
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:fffe",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:fffc",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:fff8",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:fff0",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffe0",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:0",
                "ffff:ffff:ffff:ffff::",
                "ffff::"
            };

            var firstUsable = new List<string>
            {
                "2001:db8::",
                "2001:db8::",
                "2001:db8::",
                "2001:db8::",
                "2001:db8::",
                "2001:db8::",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:0",
                "ffff:ffff:ffff:ffff::",
                "ffff::"
            };

            var lastUsable = new List<string>
            {
                "2001:db8::",
                "2001:db8::1",
                "2001:db8::3",
                "2001:db8::7",
                "2001:db8::f",
                "2001:db8::1f",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff"
            };

            var cidr = new List<byte>
            {
                128,
                127,
                126,
                125,
                124,
                123,
                112,
                64,
                16
            };

            var usable = new List<BigInteger>
            {
                1,
                2,
                4,
                8,
                16,
                32,
                65536,
                BigInteger.Pow(2, 128 - cidr[7]),
                BigInteger.Pow(2, 128 - cidr[8])
            };

            for (var i = 0; i < ipaddress.Count; i++)
            {
                IPNetwork ipnetwork;
                var parsed = IPNetwork.TryParse(ipaddress[i], netmask[i], out ipnetwork);
                Assert.AreEqual(true, parsed, "parsed");
                Assert.AreEqual(network[i], ipnetwork.Network.ToString(), "Network");
                Assert.AreEqual(netmask2[i], ipnetwork.Netmask.ToString(), "Netmask");
                Assert.AreEqual(null, ipnetwork.Broadcast, "Broadcast");
                Assert.AreEqual(cidr[i], ipnetwork.Cidr, "Cidr");
                Assert.AreEqual(usable[i], ipnetwork.Usable, "Usable");
                Assert.AreEqual(firstUsable[i], ipnetwork.FirstUsable.ToString(), "FirstUsable");
                Assert.AreEqual(lastUsable[i], ipnetwork.LastUsable.ToString(), "LastUsable");
            }
        }

        [TestMethod]
        public void TestParsev6_EDGE()
        {
            IPNetwork ipnetwork;
            const string ipaddress = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff";
            const string netmask = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff";
            const string network = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff";
            const string firstUsable = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff";
            const string lastUsable = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff";
            const byte cidr = 128;
            const uint usable = 1;

            var parsed = IPNetwork.TryParse(ipaddress, netmask, out ipnetwork);

            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(network, ipnetwork.Network.ToString(), "Network");
            Assert.AreEqual(netmask, ipnetwork.Netmask.ToString(), "Netmask");
            Assert.AreEqual(null, ipnetwork.Broadcast, "Broadcast");
            Assert.AreEqual(cidr, ipnetwork.Cidr, "Cidr");
            Assert.AreEqual(usable, ipnetwork.Usable, "Usable");
            Assert.AreEqual(firstUsable, ipnetwork.FirstUsable.ToString(), "FirstUsable");
            Assert.AreEqual(lastUsable, ipnetwork.LastUsable.ToString(), "LastUsable");
        }

        #endregion

        #region ParseString

        [TestMethod]
        public void TestParseString()
        {
            var ipaddress = new List<string>
            {
                "2001:0db8:: ffff:ffff:ffff:ffff:ffff:ffff:ffff:fff0",
                ":: ::",
                "::/0",
                "::/32",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff/128",
                "2001:0db8::",
                "::",
                "2001:0db8::1"
            };

            var network = new List<string>
            {
                "2001:db8::",
                "::",
                "::",
                "::",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff",
                "2001:db8::",
                "::",
                "2001:db8::"
            };

            var netmask = new List<string>
            {
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:fff0",
                "::",
                "::",
                "ffff:ffff::",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff",
                "ffff:ffff:ffff:ffff::",
                "ffff:ffff:ffff:ffff::",
                "ffff:ffff:ffff:ffff::"
            };

            var firstUsable = new List<string>
            {
                "2001:db8::",
                "::",
                "::",
                "::",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff",
                "2001:db8::",
                "::",
                "2001:db8::"
            };

            var lastUsable = new List<string>
            {
                "2001:db8::f",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff",
                "::ffff:ffff:ffff:ffff:ffff:ffff",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff",
                "2001:db8::ffff:ffff:ffff:ffff",
                "::ffff:ffff:ffff:ffff",
                "2001:db8::ffff:ffff:ffff:ffff"
            };

            var cidr = new List<byte>
            {
                124,
                0,
                0,
                32,
                128,
                64,
                64,
                64
            };

            var usable = new List<BigInteger>
            {
                16,
                BigInteger.Pow(2, 128 - cidr[1]),
                BigInteger.Pow(2, 128 - cidr[2]),
                BigInteger.Pow(2, 128 - cidr[3]),
                1,
                BigInteger.Pow(2, 64),
                BigInteger.Pow(2, 64),
                BigInteger.Pow(2, 64)
            };

            for (var i = 0; i < ipaddress.Count; i++)
            {
                var ipnetwork = IPNetwork.Parse(ipaddress[i]);
                Assert.AreEqual(network[i], ipnetwork.Network.ToString(), "Network");
                Assert.AreEqual(netmask[i], ipnetwork.Netmask.ToString(), "Netmask");
                Assert.AreEqual(null, ipnetwork.Broadcast, "Broadcast");
                Assert.AreEqual(cidr[i], ipnetwork.Cidr, "Cidr");
                Assert.AreEqual(usable[i], ipnetwork.Usable, "Usable");
                Assert.AreEqual(firstUsable[i], ipnetwork.FirstUsable.ToString(), "FirstUsable");
                Assert.AreEqual(lastUsable[i], ipnetwork.LastUsable.ToString(), "LastUsable");
            }
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void TestParseStringAE1()
        {
            var ipaddress = "garbage";
            var ipnetwork = IPNetwork.Parse(ipaddress);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void TestParseStringAE2()
        {
            var ipaddress = "0:0:0:0:0:0:1:0:0 0:1:2:3:4:5:6:7:8";
            var ipnetwork = IPNetwork.Parse(ipaddress);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void TestParseStringANE1()
        {
            string ipaddress = null;
            var ipnetwork = IPNetwork.Parse(ipaddress);
        }

        #endregion

        #region ParseStringString

        [TestMethod]
        public void TestParseStringString1()
        {
            const string ipaddress = "2001:0db8::";
            const string netmask = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:fff0";

            var ipnetwork = IPNetwork.Parse(ipaddress, netmask);
            Assert.AreEqual("2001:db8::/124", ipnetwork.ToString(), "network");
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void TestParseStringString3()
        {
            const string ipaddress = "2001:0db8::";
            var netmask = new List<string> {null, ""};

            foreach (var ipnetwork in netmask.Select(parsenetmask => IPNetwork.Parse(ipaddress, parsenetmask)))
            {
            }
        }

        #endregion

        #region ParseIpIp

        [TestMethod]
        public void ParseIpIp1()
        {
            var ipaddress = "2001:0db8::";
            var netmask = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:fff0";
            var ip = IPAddress.Parse(ipaddress);
            var netm = IPAddress.Parse(netmask);
            var ipnetwork = IPNetwork.Parse(ip, netm);
            Assert.AreEqual("2001:db8::/124", ipnetwork.ToString(), "network");
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ParseIpIp3()
        {
            const string ipaddress = "2001:0db8::";
            var ip = IPAddress.Parse(ipaddress);
            IPAddress netm = null;
            var ipnetwork = IPNetwork.Parse(ip, netm);
        }

        #endregion

        #region ToCidr

        [TestMethod]
        public void TestToCidrAE()
        {
            var cidr = IPNetwork.ToCidr(IPAddress.IPv6Any);
            Assert.AreEqual(0, cidr, "cidr");
        }


        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void TestToCidrAE2()
        {
            IPNetwork.ToCidr(IPAddress.Parse("2001:db8:3:4:5:6:7:8"));
        }


        [TestMethod]
        public void TestToCidr128To0()
        {
            var mask = new List<string>
            {
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:fffe",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:fffc",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:fff8",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:fff0",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffe0",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffc0",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ff80",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ff00",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:fe00",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:fc00",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:f800",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:f000",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:e000",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:c000",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:8000",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:0000",
                "ffff:ffff:ffff:ffff:ffff:ffff:fffe:0",
                "ffff:ffff:ffff:ffff:ffff:ffff:fffc:0",
                "ffff:ffff:ffff:ffff:ffff:ffff:fff8:0",
                "ffff:ffff:ffff:ffff:ffff:ffff:fff0:0",
                "8000::",
                "::"
            };
            var expectedcidr = new List<byte>
            {
                128,
                127,
                126,
                125,
                124,
                123,
                122,
                121,
                120,
                119,
                118,
                117,
                116,
                115,
                114,
                113,
                112,
                111,
                110,
                109,
                108,
                1,
                0
            };

            for (var i = 0; i < mask.Count; i++)
            {
                int result = IPNetwork.ToCidr(IPAddress.Parse(mask[i]));
                Assert.AreEqual(expectedcidr[i], result, "cidr");
            }
        }

        #endregion

        #region TryToNetmask

        [TestMethod]
        public void TryToNetmask1()
        {
            IPAddress result = null;
            var parsed = IPNetwork.TryToNetmask(0, AddressFamily.InterNetworkV6, out result);
            var expected = IPAddress.Parse("::");

            Assert.AreEqual(expected, result, "Netmask");
            Assert.AreEqual(true, parsed, "parsed");
        }

        [TestMethod]
        public void TryToNetmask2()
        {
            IPAddress result = null;
            var parsed = IPNetwork.TryToNetmask(33, AddressFamily.InterNetworkV6, out result);
            var expected = IPAddress.Parse("ffff:ffff:8000::");

            Assert.AreEqual(expected, result, "Netmask");
            Assert.AreEqual(true, parsed, "parsed");
        }

        #endregion

        #region ToNetmask

        [TestMethod]
        public void ToNetmask()
        {
            var cidr = new List<byte> {128, 127, 126, 1, 0};
            var netmask = new List<string>
            {
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:fffe",
                "ffff:ffff:ffff:ffff:ffff:ffff:ffff:fffc",
                "8000::",
                "::"
            };
            for (var i = 0; i < netmask.Count; i++)
            {
                var result = IPNetwork.ToNetmask(cidr[i], AddressFamily.InterNetworkV6).ToString();
                Assert.AreEqual(netmask[i], result, "netmask");
            }
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public void ToNetmaskOORE1()
        {
            byte cidr = 129;
            var result = IPNetwork.ToNetmask(cidr, AddressFamily.InterNetworkV6).ToString();
        }

        #endregion

        #region Contains

        [TestMethod]
        public void TestContains()
        {
            var ipnetwork = IPNetwork.Parse("2001:0db8::/64");
            var ipaddress = new List<string>
            {
                "2001:0db8::1",
                "2001:0db8:0:1::",
                "2001:0db8::/64",
                "2001:0db8::/65",
                "2001:0db8:1::/65",
                "2001:0db8::/63"
            };
            var expectedvalue = new List<bool> {true, false, true, true, false, false};
            for (var i = 0; i < ipaddress.Count; i++)
            {
                bool result;
                if (i < 2)
                {
                    result = IPNetwork.Contains(ipnetwork, IPAddress.Parse(ipaddress[i]));
                }
                else
                {
                    result = IPNetwork.Contains(ipnetwork, IPNetwork.Parse(ipaddress[i]));
                }
                Assert.AreEqual(expectedvalue[i], result, "contains");
            }
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void TestContainsError()
        {
            var ipnetwork = IPNetwork.Parse("::/0");
            IPAddress ipaddress = null;

            var result = IPNetwork.Contains(ipnetwork, ipaddress);
        }

        #endregion

        #region Overlap

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void TestOverlap1()
        {
            var network1 = IPNetwork.Parse("2001:0db8::/0");
            IPNetwork network2 = null;
            IPNetwork.Overlap(network1, network2);
        }

        [TestMethod]
        public void TestOverlap2()
        {
            var network1 = new List<string>
            {
                "2001:0db8::/64",
                "2001:0db8::/64",
                "2001:0db8:0:1::/68",
                "2001:0db8:0:1::/68"
            };
            var network2 = new List<string>
            {
                "2001:0db8::/64",
                "2001:0db8:0:0:1::/65",
                "2001:0db8:0:2::/68",
                "2001:0db8:0:2::/62"
            };
            var valueexpected = new List<bool> {true, true, false, true};

            for (var i = 0; i < network1.Count; i++)
            {
                var result = IPNetwork.Overlap(IPNetwork.Parse(network1[i]), IPNetwork.Parse(network2[i]));
                Assert.AreEqual(valueexpected[i], result, "overlap");
            }
        }

        #endregion

        #region Examples

        public void Example1()
        {
            var ipnetwork = IPNetwork.Parse("2001:0db8::/64");

            Console.WriteLine($"Network : {ipnetwork.Network}");
            Console.WriteLine($"Netmask : {ipnetwork.Netmask}");
            Console.WriteLine($"Broadcast : {ipnetwork.Broadcast}");
            Console.WriteLine($"FirstUsable : {ipnetwork.FirstUsable}");
            Console.WriteLine($"LastUsable : {ipnetwork.LastUsable}");
            Console.WriteLine($"Usable : {ipnetwork.Usable}");
            Console.WriteLine($"Cidr : {ipnetwork.Cidr}");
        }

        public void Example2()
        {
            var ipnetwork = IPNetwork.Parse("2001:0db8::/64");

            var ipaddress = IPAddress.Parse("2001:0db8::1");
            var ipaddress2 = IPAddress.Parse("2001:0db9::1");

            var ipnetwork2 = IPNetwork.Parse("2001:0db8::1/128");
            var ipnetwork3 = IPNetwork.Parse("2001:0db9::1/64");

            var contains1 = IPNetwork.Contains(ipnetwork, ipaddress);
            var contains2 = IPNetwork.Contains(ipnetwork, ipaddress2);
            var contains3 = IPNetwork.Contains(ipnetwork, ipnetwork2);
            var contains4 = IPNetwork.Contains(ipnetwork, ipnetwork3);

            var overlap1 = IPNetwork.Overlap(ipnetwork, ipnetwork2);
            var overlap2 = IPNetwork.Overlap(ipnetwork, ipnetwork3);

            Console.WriteLine($"{ipnetwork} contains {ipaddress} : {contains1}");
            Console.WriteLine($"{ipnetwork} contains {ipaddress2} : {contains2}");
            Console.WriteLine($"{ipnetwork} contains {ipnetwork2} : {contains3}");
            Console.WriteLine($"{ipnetwork} contains {ipnetwork3} : {contains4}");


            Console.WriteLine("{0} overlap {1} : {2}", ipnetwork, ipnetwork2, overlap1);
            Console.WriteLine("{0} overlap {1} : {2}", ipnetwork, ipnetwork3, overlap2);
        }

        public void Example4()
        {
            var wholeInternet = IPNetwork.Parse("::/0");
            byte newCidr = 2;
            var subneted = IPNetwork.Subnet(wholeInternet, newCidr);

            Console.WriteLine($"{wholeInternet} was subnetted into {subneted.Count} subnets");
            Console.WriteLine($"First: {subneted[0]}");
            Console.WriteLine($"Last : {subneted[subneted.Count - 1]}");
            Console.WriteLine("All  :");

            foreach (var ipnetwork in subneted)
            {
                Console.WriteLine(ipnetwork);
            }
        }

        public void Example5()
        {
            var ipnetwork1 = IPNetwork.Parse("2001:0db8::/64");
            var ipnetwork2 = IPNetwork.Parse("2001:0db9::/64");
            var ipnetwork3 = IPNetwork.Supernet(new[] {ipnetwork1, ipnetwork2});

            Console.WriteLine($"{ipnetwork1} + {ipnetwork2} = {ipnetwork3[0]}");
        }

        public void Example6()
        {
            var ipnetwork = IPNetwork.Parse("fe80::202:b3ff:fe1e:8329/24");

            var ipaddress = IPAddress.Parse("2001:db8::");
            var ipaddress2 = IPAddress.Parse("fe80::202:b3ff:fe1e:1");

            var contains1 = IPNetwork.Contains(ipnetwork, ipaddress);
            var contains2 = IPNetwork.Contains(ipnetwork, ipaddress2);

            Console.WriteLine($"{ipnetwork} contains {ipaddress} : {contains1}");
            Console.WriteLine($"{ipnetwork} contains {ipaddress2} : {contains2}");
        }


        public void Example8()
        {
            var network = IPNetwork.Parse("::/124");
            var ips = IPNetwork.Subnet(network, 128);

            foreach (var ip in ips)
            {
                Console.WriteLine(ip);
            }
        }

        #endregion

        #region ToString

        [TestMethod]
        public void TestToString()
        {
            var ipnetwork = new List<string>
            {
                "2001:0db8:0000:0000:0000:0000:0000:0000/32",
                "2001:0db8:1:2:3:4:5:6/32",
                "2001:0db8:1:2:3:4:5:6/64",
                "2001:0db8:1:2:3:4:5:6/100"
            };
            var expected = new List<string>
            {
                "2001:db8::/32",
                "2001:db8::/32",
                "2001:db8:1:2::/64",
                "2001:db8:1:2:3:4::/100"
            };
            var index = 0;
            foreach (var result in ipnetwork.Select(ipnetworktotest => IPNetwork.Parse(ipnetwork[index]).ToString()))
            {
                Assert.AreEqual(expected[index], result, "ToString");
                index++;
            }
        }

        [TestMethod]
        public void TestToString1()
        {
            var ipnetwork = IPNetwork.Parse("2001:0db8:1:2:3:4:5:6/32");
            var expected = "2001:db8::/32";
            var result = ipnetwork.ToString();

            Assert.AreEqual(expected, result, "ToString");
        }

        [TestMethod]
        public void TestToString2()
        {
            var ipnetwork = IPNetwork.Parse("2001:0db8:1:2:3:4:5:6/64");
            const string expected = "2001:db8:1:2::/64";
            var result = ipnetwork.ToString();

            Assert.AreEqual(expected, result, "ToString");
        }

        [TestMethod]
        public void TestToString3()
        {
            var ipnetwork = IPNetwork.Parse("2001:0db8:1:2:3:4:5:6/100");
            const string expected = "2001:db8:1:2:3:4::/100";
            var result = ipnetwork.ToString();

            Assert.AreEqual(expected, result, "ToString");
        }

        #endregion

        #region Subnet

        [TestMethod]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public void TestSubnet3()
        {
            var ipnetwork = IPNetwork.Parse("::");
            const byte cidr = 129;

            var subnets = IPNetwork.Subnet(ipnetwork, cidr);
        }


        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void TestSubnet4()
        {
            var ipnetwork = IPNetwork.Parse("::");
            const byte cidr = 1;

            var subnets = IPNetwork.Subnet(ipnetwork, cidr);
        }


        [TestMethod]
        public void TestSubnet5()
        {
            var ipnetwork = IPNetwork.Parse("1:1:1:1:1:1:1:1");
            const byte cidr = 65;

            var subnets = IPNetwork.Subnet(ipnetwork, cidr);
            Assert.AreEqual(2, subnets.Count, "count");
            Assert.AreEqual("1:1:1:1::/65", subnets[0].ToString(), "subnet1");
            Assert.AreEqual("1:1:1:1:8000::/65", subnets[1].ToString(), "subnet2");
        }


        [TestMethod]
        public void TestSubnet6()
        {
            var ipnetwork = IPNetwork.Parse("1:1:1:1:1:1:1:1");
            const byte cidr = 68;

            var subnets = IPNetwork.Subnet(ipnetwork, cidr);
            Assert.AreEqual(16, subnets.Count, "count");
            Assert.AreEqual("1:1:1:1::/68", subnets[0].ToString(), "subnet1");
            Assert.AreEqual("1:1:1:1:1000::/68", subnets[1].ToString(), "subnet2");
            Assert.AreEqual("1:1:1:1:2000::/68", subnets[2].ToString(), "subnet3");
            Assert.AreEqual("1:1:1:1:3000::/68", subnets[3].ToString(), "subnet4");
            Assert.AreEqual("1:1:1:1:4000::/68", subnets[4].ToString(), "subnet5");
            Assert.AreEqual("1:1:1:1:5000::/68", subnets[5].ToString(), "subnet6");
            Assert.AreEqual("1:1:1:1:6000::/68", subnets[6].ToString(), "subnet7");
            Assert.AreEqual("1:1:1:1:7000::/68", subnets[7].ToString(), "subnet8");
            Assert.AreEqual("1:1:1:1:8000::/68", subnets[8].ToString(), "subnet9");
            Assert.AreEqual("1:1:1:1:9000::/68", subnets[9].ToString(), "subnet10");
            Assert.AreEqual("1:1:1:1:a000::/68", subnets[10].ToString(), "subnet11");
            Assert.AreEqual("1:1:1:1:b000::/68", subnets[11].ToString(), "subnet12");
            Assert.AreEqual("1:1:1:1:c000::/68", subnets[12].ToString(), "subnet13");
            Assert.AreEqual("1:1:1:1:d000::/68", subnets[13].ToString(), "subnet14");
            Assert.AreEqual("1:1:1:1:e000::/68", subnets[14].ToString(), "subnet15");
            Assert.AreEqual("1:1:1:1:f000::/68", subnets[15].ToString(), "subnet16");
        }


        [TestMethod]
        public void TestSubnet7()
        {
            var ipnetwork = IPNetwork.Parse("1:1:1:1:1:1:1:1");
            const byte cidr = 72;

            var subnets = IPNetwork.Subnet(ipnetwork, cidr);
            Assert.AreEqual(256, subnets.Count, "count");
            Assert.AreEqual("1:1:1:1::/72", subnets[0].ToString(), "subnet1");
            Assert.AreEqual("1:1:1:1:ff00::/72", subnets[255].ToString(), "subnet256");
        }


        [TestMethod]
        public void TestSubnet9()
        {
            var ipnetwork = IPNetwork.Parse("2001:db08:1:1:1:1:1:1");
            const byte cidr = 128;
            var count = BigInteger.Pow(2, ipnetwork.Cidr);
            var subnets = IPNetwork.Subnet(ipnetwork, cidr);
            Assert.AreEqual(count, subnets.Count, "count");
            Assert.AreEqual("2001:db08:1:1::/128", subnets[0].ToString(), "subnet1");
            Assert.AreEqual("2001:db08:1:1::ff/128", subnets[255].ToString(), "subnet256");
            Assert.AreEqual("2001:db08:1:1:ffff:ffff:ffff:ffff/128", subnets[count - 1].ToString(), "last");
        }


        [TestMethod]
        public void TestSubnet10()
        {
            var ipnetwork = IPNetwork.Parse("2001:db08::/0");
            const byte cidr = 128;
            var count = BigInteger.Pow(2, 128 - ipnetwork.Cidr);

            // Here I spawm a OOM dragon ! beware of the beast !
            var subnets = IPNetwork.Subnet(ipnetwork, cidr);
            Assert.AreEqual(count, subnets.Count, "count");
            Assert.AreEqual("::/128", subnets[0].ToString(), "subnet1");
            Assert.AreEqual("ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff/128", subnets[count - 1].ToString(), "last");
        }


        [TestMethod]
        public void TestSubnet12()
        {
            var ipnetwork = IPNetwork.Parse("2001:db08::/64");
            const byte cidr = 70;
            var i = -1;
            var subnets = IPNetwork.Subnet(ipnetwork, cidr);
            foreach (var ipn in subnets)
            {
                i++;
                Assert.AreEqual(subnets[i], ipn, "subnet");
            }
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public void TestSubnet13()
        {
            var ipnetwork = IPNetwork.Parse("2001:db08::/64");
            const byte cidr = 70;
            var subnets = IPNetwork.Subnet(ipnetwork, cidr);
            var error = subnets[1000];
        }

        #endregion

        #region TrySubnet

        [TestMethod]
        public void TestTrySubnet3()
        {
            var ipnetwork = IPNetwork.Parse("2001:db08::/64");
            const byte cidr = 255;

            IPNetworkCollection subnets;
            var subnetted = IPNetwork.TrySubnet(ipnetwork, cidr, out subnets);

            Assert.AreEqual(false, subnetted, "subnetted");
        }


        [TestMethod]
        public void TestTrySubnet4()
        {
            var ipnetwork = IPNetwork.Parse("2001:db08::/64");
            const byte cidr = 63;

            IPNetworkCollection subnets;
            var subnetted = IPNetwork.TrySubnet(ipnetwork, cidr, out subnets);

            Assert.AreEqual(false, subnetted, "subnetted");
        }


        [TestMethod]
        public void TestTrySubnet5()
        {
            var ipnetwork = IPNetwork.Parse("2001:db8::/64");
            const byte cidr = 65;

            IPNetworkCollection subnets;
            var subnetted = IPNetwork.TrySubnet(ipnetwork, cidr, out subnets);

            Assert.AreEqual(true, subnetted, "subnetted");
            Assert.AreEqual(2, subnets.Count, "count");
            Assert.AreEqual("2001:db8::/65", subnets[0].ToString(), "subnet1");
            Assert.AreEqual("2001:db8:0:0:8000::/65", subnets[1].ToString(), "subnet2");
        }


        [TestMethod]
        public void TestTrySubnet6()
        {
            var ipnetwork = IPNetwork.Parse("2001:db8::/64");
            const byte cidr = 68;

            IPNetworkCollection subnets;
            var subnetted = IPNetwork.TrySubnet(ipnetwork, cidr, out subnets);

            Assert.AreEqual(true, subnetted, "subnetted");
            Assert.AreEqual(16, subnets.Count, "count");
            Assert.AreEqual("2001:db8::/68", subnets[0].ToString(), "subnet1");
            Assert.AreEqual("2001:db8:0:0:1000::/68", subnets[1].ToString(), "subnet2");
            Assert.AreEqual("2001:db8:0:0:2000::/68", subnets[2].ToString(), "subnet3");
            Assert.AreEqual("2001:db8:0:0:3000::/68", subnets[3].ToString(), "subnet4");
            Assert.AreEqual("2001:db8:0:0:4000::/68", subnets[4].ToString(), "subnet5");
            Assert.AreEqual("2001:db8:0:0:5000::/68", subnets[5].ToString(), "subnet6");
            Assert.AreEqual("2001:db8:0:0:6000::/68", subnets[6].ToString(), "subnet7");
            Assert.AreEqual("2001:db8:0:0:7000::/68", subnets[7].ToString(), "subnet8");
            Assert.AreEqual("2001:db8:0:0:8000::/68", subnets[8].ToString(), "subnet9");
            Assert.AreEqual("2001:db8:0:0:9000::/68", subnets[9].ToString(), "subnet10");
            Assert.AreEqual("2001:db8:0:0:a000::/68", subnets[10].ToString(), "subnet11");
            Assert.AreEqual("2001:db8:0:0:b000::/68", subnets[11].ToString(), "subnet12");
            Assert.AreEqual("2001:db8:0:0:c000::/68", subnets[12].ToString(), "subnet13");
            Assert.AreEqual("2001:db8:0:0:d000::/68", subnets[13].ToString(), "subnet14");
            Assert.AreEqual("2001:db8:0:0:e000::/68", subnets[14].ToString(), "subnet15");
            Assert.AreEqual("2001:db8:0:0:f000::/68", subnets[15].ToString(), "subnet16");
        }

        #endregion

        #region TrySupernet

        [TestMethod]
        public void TestTrySupernet2()
        {
            var network2 = IPNetwork.Parse("2001:db8::/64");
            IPNetwork supernet;
            const bool parsed = false;
            var result = IPNetwork.TrySupernet(null, network2, out supernet);

            Assert.AreEqual(null, supernet, "supernet");
            Assert.AreEqual(parsed, result, "parsed");
        }

        [TestMethod]
        public void TestTrySupernet()
        {
            var network1 = new List<IPNetwork>
            {
                IPNetwork.Parse("2001:db8::/65"),
                null,
                IPNetwork.Parse("2001:db8::/64"),
                IPNetwork.Parse("2001:db8::/64"),
                IPNetwork.Parse("2001:db8::/64"),
                IPNetwork.Parse("2001:db8::/64"),
                IPNetwork.Parse("2001:db8::/64"),
                IPNetwork.Parse("2001:db0::/64")
            };
            var network2 = new List<IPNetwork>
            {
                IPNetwork.Parse("2001:db8:0:0:8000::/65"),
                IPNetwork.Parse("2001:db8::/64"),
                null,
                IPNetwork.Parse("2001:db9::/65"),
                IPNetwork.Parse("2001:dba::/64"),
                IPNetwork.Parse("2001:db8::1/65"),
                IPNetwork.Parse("2001:db8::1/65"),
                IPNetwork.Parse("2001:dbf::/64")
            };
            var supernetExpected = new List<IPNetwork>
            {
                IPNetwork.Parse("2001:db8::/64"),
                null,
                null,
                null,
                null,
                IPNetwork.Parse("2001:db8::/64"),
                IPNetwork.Parse("2001:db8::/64"),
                null
            };
            var parsed = new List<bool> {true, false, false, false, false, true, true, false};
            var message = new List<string>
            {
                "supernetted",
                "parsed",
                "parsed",
                "parsed",
                "parsed",
                "parsed",
                "parsed",
                "parsed"
            };
            for (var i = 0; i < network1.Count; i++)
            {
                IPNetwork supernet;
                var result = IPNetwork.TrySupernet(network1[i], network2[i], out supernet);
                Assert.AreEqual(parsed[i], result, message[i]);
                Assert.AreEqual(supernetExpected[i], supernet, "supernet");
            }
        }

        [TestMethod]
        public void TestTrySupernet9()
        {
            var network1 = IPNetwork.Parse("192.168.1.1/24");
            var network2 = IPNetwork.Parse("192.168.2.1/24");
            IPNetwork[] network3 = {network1, network2};
            IPNetwork[] supernetExpected = {network1, network2};
            IPNetwork[] supernet;
            const bool parsed = true;
            var result = IPNetwork.TrySupernet(network3, out supernet);

            Assert.AreEqual(supernetExpected[0], supernet[0], "supernet");
            Assert.AreEqual(supernetExpected[1], supernet[1], "supernet");
            Assert.AreEqual(parsed, result, "parsed");
        }


        [TestMethod]
        public void TestTrySupernet10()
        {
            var network1 = IPNetwork.Parse("2001:db8:0000::/65");
            var network2 = IPNetwork.Parse("2001:db8:0:0:8000::/65");
            IPNetwork[] network3 = {network1, network2};
            IPNetwork[] supernetExpected = {IPNetwork.Parse("2001:db8::/64")};
            IPNetwork[] supernet;
            const bool parsed = true;
            var result = IPNetwork.TrySupernet(network3, out supernet);

            Assert.AreEqual(supernetExpected[0], supernet[0], "supernet");
            Assert.AreEqual(parsed, result, "parsed");
        }


        [TestMethod]
        public void TestTrySupernet11()
        {
            IPNetwork[] supernet;
            const bool parsed = false;
            var result = IPNetwork.TrySupernet(null, out supernet);

            Assert.AreEqual(null, supernet, "supernet");
            Assert.AreEqual(parsed, result, "parsed");
        }

        #endregion
    }
}