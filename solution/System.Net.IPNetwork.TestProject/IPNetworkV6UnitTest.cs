using System.Net.Sockets;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Net.TestProject
{
    [TestClass]
    public class IPNetworkV6UnitTest
    {
        #region ParseIP

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void TestParseIPAddressNetmaskANE8()
        {
            var ipnet = IPNetwork.Parse("xxxx::", "xxxx::");
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void TestParseIPAddressNetmaskANE9()
        {
            var ipnet = IPNetwork.Parse("::", "xxxx::");
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
        public void TestParsev6_128()
        {
            IPNetwork ipnetwork = null;
            var ipaddress = "2001:db8::";
            var netmask = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff";

            var network = "2001:db8::";
            var netmask2 = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff";
            var broadcast = "2001:db8::";
            var firstUsable = "2001:db8::";
            var lastUsable = "2001:db8::";
            byte cidr = 128;
            BigInteger usable = 1;

            var parsed = IPNetwork.TryParse(ipaddress, netmask, out ipnetwork);
            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(network, ipnetwork.Network.ToString(), "Network");
            Assert.AreEqual(netmask2, ipnetwork.Netmask.ToString(), "Netmask");
            Assert.AreEqual(null, ipnetwork.Broadcast, "Broadcast");
            Assert.AreEqual(cidr, ipnetwork.Cidr, "Cidr");
            Assert.AreEqual(usable, ipnetwork.Usable, "Usable");
            Assert.AreEqual(firstUsable, ipnetwork.FirstUsable.ToString(), "FirstUsable");
            Assert.AreEqual(lastUsable, ipnetwork.LastUsable.ToString(), "LastUsable");
        }

        [TestMethod]
        public void TestParsev6_127()
        {
            IPNetwork ipnetwork = null;
            var ipaddress = "2001:db8::";
            var netmask = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:fffe";

            var network = "2001:db8::";
            var netmask2 = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:fffe";
            var broadcast = "2001:db8::1";
            var firstUsable = "2001:db8::";
            var lastUsable = "2001:db8::1";
            byte cidr = 127;
            BigInteger usable = 2;

            var parsed = IPNetwork.TryParse(ipaddress, netmask, out ipnetwork);
            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(network, ipnetwork.Network.ToString(), "Network");
            Assert.AreEqual(netmask2, ipnetwork.Netmask.ToString(), "Netmask");
            Assert.AreEqual(null, ipnetwork.Broadcast, "Broadcast");
            Assert.AreEqual(cidr, ipnetwork.Cidr, "Cidr");
            Assert.AreEqual(usable, ipnetwork.Usable, "Usable");
            Assert.AreEqual(firstUsable, ipnetwork.FirstUsable.ToString(), "FirstUsable");
            Assert.AreEqual(lastUsable, ipnetwork.LastUsable.ToString(), "LastUsable");
        }

        [TestMethod]
        public void TestParsev6_126()
        {
            IPNetwork ipnetwork = null;
            var ipaddress = "2001:db8::";
            var netmask = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:fffc";

            var network = "2001:db8::";
            var netmask2 = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:fffc";
            var broadcast = "2001:db8::3";
            var firstUsable = "2001:db8::";
            var lastUsable = "2001:db8::3";
            byte cidr = 126;
            BigInteger usable = 4;

            var parsed = IPNetwork.TryParse(ipaddress, netmask, out ipnetwork);
            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(network, ipnetwork.Network.ToString(), "Network");
            Assert.AreEqual(netmask2, ipnetwork.Netmask.ToString(), "Netmask");
            Assert.AreEqual(null, ipnetwork.Broadcast, "Broadcast");
            Assert.AreEqual(cidr, ipnetwork.Cidr, "Cidr");
            Assert.AreEqual(usable, ipnetwork.Usable, "Usable");
            Assert.AreEqual(firstUsable, ipnetwork.FirstUsable.ToString(), "FirstUsable");
            Assert.AreEqual(lastUsable, ipnetwork.LastUsable.ToString(), "LastUsable");
        }

        [TestMethod]
        public void TestParsev6_125()
        {
            IPNetwork ipnetwork = null;
            var ipaddress = "2001:db8::";
            var netmask = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:fff8";

            var network = "2001:db8::";
            var netmask2 = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:fff8";
            var broadcast = "2001:db8::7";
            var firstUsable = "2001:db8::";
            var lastUsable = "2001:db8::7";
            byte cidr = 125;
            BigInteger usable = 8;

            var parsed = IPNetwork.TryParse(ipaddress, netmask, out ipnetwork);
            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(network, ipnetwork.Network.ToString(), "Network");
            Assert.AreEqual(netmask2, ipnetwork.Netmask.ToString(), "Netmask");
            Assert.AreEqual(null, ipnetwork.Broadcast, "Broadcast");
            Assert.AreEqual(cidr, ipnetwork.Cidr, "Cidr");
            Assert.AreEqual(usable, ipnetwork.Usable, "Usable");
            Assert.AreEqual(firstUsable, ipnetwork.FirstUsable.ToString(), "FirstUsable");
            Assert.AreEqual(lastUsable, ipnetwork.LastUsable.ToString(), "LastUsable");
        }

        [TestMethod]
        public void TestParsev6_124()
        {
            IPNetwork ipnetwork = null;
            var ipaddress = "2001:db8::";
            var netmask = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:fff0";

            var network = "2001:db8::";
            var netmask2 = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:fff0";
            var broadcast = "2001:db8::f";
            var firstUsable = "2001:db8::";
            var lastUsable = "2001:db8::f";
            byte cidr = 124;
            BigInteger usable = 16;

            var parsed = IPNetwork.TryParse(ipaddress, netmask, out ipnetwork);
            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(network, ipnetwork.Network.ToString(), "Network");
            Assert.AreEqual(netmask2, ipnetwork.Netmask.ToString(), "Netmask");
            Assert.AreEqual(null, ipnetwork.Broadcast, "Broadcast");
            Assert.AreEqual(cidr, ipnetwork.Cidr, "Cidr");
            Assert.AreEqual(usable, ipnetwork.Usable, "Usable");
            Assert.AreEqual(firstUsable, ipnetwork.FirstUsable.ToString(), "FirstUsable");
            Assert.AreEqual(lastUsable, ipnetwork.LastUsable.ToString(), "LastUsable");
        }


        [TestMethod]
        public void TestParsev6_123()
        {
            IPNetwork ipnetwork = null;
            var ipaddress = "2001:0db8::";
            var netmask = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffe0";

            var network = "2001:db8::";
            var netmask2 = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffe0";
            var broadcast = "2001:db8::1f";
            var firstUsable = "2001:db8::";
            var lastUsable = "2001:db8::1f";
            byte cidr = 123;
            BigInteger usable = 32;

            var parsed = IPNetwork.TryParse(ipaddress, netmask, out ipnetwork);
            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(network, ipnetwork.Network.ToString(), "Network");
            Assert.AreEqual(netmask2, ipnetwork.Netmask.ToString(), "Netmask");
            Assert.AreEqual(null, ipnetwork.Broadcast, "Broadcast");
            Assert.AreEqual(cidr, ipnetwork.Cidr, "Cidr");
            Assert.AreEqual(usable, ipnetwork.Usable, "Usable");
            Assert.AreEqual(firstUsable, ipnetwork.FirstUsable.ToString(), "FirstUsable");
            Assert.AreEqual(lastUsable, ipnetwork.LastUsable.ToString(), "LastUsable");
        }


        [TestMethod]
        public void TestParsev6_112()
        {
            IPNetwork ipnetwork = null;
            var ipaddress = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff";
            var netmask = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:0000";

            var network = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:0";
            var netmask2 = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:0";
            var broadcast = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff";
            var firstUsable = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:0";
            var lastUsable = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff";
            byte cidr = 112;
            uint usable = 65536;

            var parsed = IPNetwork.TryParse(ipaddress, netmask, out ipnetwork);
            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(network, ipnetwork.Network.ToString(), "Network");
            Assert.AreEqual(netmask2, ipnetwork.Netmask.ToString(), "Netmask");
            Assert.AreEqual(null, ipnetwork.Broadcast, "Broadcast");
            Assert.AreEqual(cidr, ipnetwork.Cidr, "Cidr");
            Assert.AreEqual(usable, ipnetwork.Usable, "Usable");
            Assert.AreEqual(firstUsable, ipnetwork.FirstUsable.ToString(), "FirstUsable");
            Assert.AreEqual(lastUsable, ipnetwork.LastUsable.ToString(), "LastUsable");
        }

        [TestMethod]
        public void TestParsev6_64()
        {
            IPNetwork ipnetwork = null;
            var ipaddress = "ffff:ffff:ffff:ffff:1234:1234:1234:1234";
            var netmask = "ffff:ffff:ffff:ffff:0000:0000:0000:0000";

            var network = "ffff:ffff:ffff:ffff::";
            var netmask2 = "ffff:ffff:ffff:ffff::";
            var broadcast = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff";
            var firstUsable = "ffff:ffff:ffff:ffff::";
            var lastUsable = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff";
            byte cidr = 64;
            var usable = BigInteger.Pow(2, 128 - cidr);

            var parsed = IPNetwork.TryParse(ipaddress, netmask, out ipnetwork);
            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(network, ipnetwork.Network.ToString(), "Network");
            Assert.AreEqual(netmask2, ipnetwork.Netmask.ToString(), "Netmask");
            Assert.AreEqual(null, ipnetwork.Broadcast, "Broadcast");
            Assert.AreEqual(cidr, ipnetwork.Cidr, "Cidr");
            Assert.AreEqual(usable, ipnetwork.Usable, "Usable");
            Assert.AreEqual(firstUsable, ipnetwork.FirstUsable.ToString(), "FirstUsable");
            Assert.AreEqual(lastUsable, ipnetwork.LastUsable.ToString(), "LastUsable");
        }

        [TestMethod]
        public void TestParsev6_16()
        {
            IPNetwork ipnetwork = null;
            var ipaddress = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff";
            var netmask = "ffff:0000:0000:0000:0000:0000:0000:0000";

            var network = "ffff::";
            var netmask2 = "ffff::";
            var broadcast = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff";
            var firstUsable = "ffff::";
            var lastUsable = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff";
            byte cidr = 16;
            var usable = BigInteger.Pow(2, 128 - cidr);

            var parsed = IPNetwork.TryParse(ipaddress, netmask, out ipnetwork);
            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(network, ipnetwork.Network.ToString(), "Network");
            Assert.AreEqual(netmask2, ipnetwork.Netmask.ToString(), "Netmask");
            Assert.AreEqual(null, ipnetwork.Broadcast, "Broadcast");
            Assert.AreEqual(cidr, ipnetwork.Cidr, "Cidr");
            Assert.AreEqual(usable, ipnetwork.Usable, "Usable");
            Assert.AreEqual(firstUsable, ipnetwork.FirstUsable.ToString(), "FirstUsable");
            Assert.AreEqual(lastUsable, ipnetwork.LastUsable.ToString(), "LastUsable");
        }


        [TestMethod]
        public void TestParsev6_EDGE()
        {
            IPNetwork ipnetwork = null;
            var ipaddress = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff";
            var netmask = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff";

            var network = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff";
            var firstUsable = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff";
            var lastUsable = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff";
            byte cidr = 128;
            uint usable = 1;

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
        public void TestParseString1()
        {
            var ipaddress = "2001:0db8:: ffff:ffff:ffff:ffff:ffff:ffff:ffff:fff0";

            var network = "2001:db8::";
            var netmask = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:fff0";
            var firstUsable = "2001:db8::";
            var lastUsable = "2001:db8::f";
            byte cidr = 124;
            uint usable = 16;

            var ipnetwork = IPNetwork.Parse(ipaddress);
            Assert.AreEqual(network, ipnetwork.Network.ToString(), "Network");
            Assert.AreEqual(netmask, ipnetwork.Netmask.ToString(), "Netmask");
            Assert.AreEqual(null, ipnetwork.Broadcast, "Broadcast");
            Assert.AreEqual(cidr, ipnetwork.Cidr, "Cidr");
            Assert.AreEqual(usable, ipnetwork.Usable, "Usable");
            Assert.AreEqual(firstUsable, ipnetwork.FirstUsable.ToString(), "FirstUsable");
            Assert.AreEqual(lastUsable, ipnetwork.LastUsable.ToString(), "LastUsable");
        }

        [TestMethod]
        public void TestParseString3()
        {
            var ipaddress = ":: ::";

            var network = "::";
            var netmask = "::";
            var firstUsable = "::";
            var lastUsable = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff";
            byte cidr = 0;
            var usable = BigInteger.Pow(2, 128 - cidr);

            var ipnetwork = IPNetwork.Parse(ipaddress);
            Assert.AreEqual(network, ipnetwork.Network.ToString(), "Network");
            Assert.AreEqual(netmask, ipnetwork.Netmask.ToString(), "Netmask");
            Assert.AreEqual(null, ipnetwork.Broadcast, "Broadcast");
            Assert.AreEqual(cidr, ipnetwork.Cidr, "Cidr");
            Assert.AreEqual(usable, ipnetwork.Usable, "Usable");
            Assert.AreEqual(firstUsable, ipnetwork.FirstUsable.ToString(), "FirstUsable");
            Assert.AreEqual(lastUsable, ipnetwork.LastUsable.ToString(), "LastUsable");
        }

        [TestMethod]
        public void TestParseString4()
        {
            var ipaddress = "::/0";

            var network = "::";
            var netmask = "::";
            var firstUsable = "::";
            var lastUsable = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff";
            byte cidr = 0;
            var usable = BigInteger.Pow(2, 128 - cidr);

            var ipnetwork = IPNetwork.Parse(ipaddress);
            Assert.AreEqual(network, ipnetwork.Network.ToString(), "Network");
            Assert.AreEqual(netmask, ipnetwork.Netmask.ToString(), "Netmask");
            Assert.AreEqual(null, ipnetwork.Broadcast, "Broadcast");
            Assert.AreEqual(cidr, ipnetwork.Cidr, "Cidr");
            Assert.AreEqual(usable, ipnetwork.Usable, "Usable");
            Assert.AreEqual(firstUsable, ipnetwork.FirstUsable.ToString(), "FirstUsable");
            Assert.AreEqual(lastUsable, ipnetwork.LastUsable.ToString(), "LastUsable");
        }


        [TestMethod]
        public void TestParseString5()
        {
            var ipaddress = "::/32";

            var network = "::";
            var netmask = "ffff:ffff::";
            var firstUsable = "::";
            var lastUsable = "::ffff:ffff:ffff:ffff:ffff:ffff";
            byte cidr = 32;
            var usable = BigInteger.Pow(2, 128 - cidr);

            var ipnetwork = IPNetwork.Parse(ipaddress);
            Assert.AreEqual(network, ipnetwork.Network.ToString(), "Network");
            Assert.AreEqual(netmask, ipnetwork.Netmask.ToString(), "Netmask");
            Assert.AreEqual(null, ipnetwork.Broadcast, "Broadcast");
            Assert.AreEqual(cidr, ipnetwork.Cidr, "Cidr");
            Assert.AreEqual(usable, ipnetwork.Usable, "Usable");
            Assert.AreEqual(firstUsable, ipnetwork.FirstUsable.ToString(), "FirstUsable");
            Assert.AreEqual(lastUsable, ipnetwork.LastUsable.ToString(), "LastUsable");
        }


        [TestMethod]
        public void TestParseString6()
        {
            var ipaddress = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff/128";

            var network = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff";
            var netmask = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff";
            var firstUsable = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff";
            var lastUsable = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff";
            byte cidr = 128;
            uint usable = 1;

            var ipnetwork = IPNetwork.Parse(ipaddress);
            Assert.AreEqual(network, ipnetwork.Network.ToString(), "Network");
            Assert.AreEqual(netmask, ipnetwork.Netmask.ToString(), "Netmask");
            Assert.AreEqual(null, ipnetwork.Broadcast, "Broadcast");
            Assert.AreEqual(cidr, ipnetwork.Cidr, "Cidr");
            Assert.AreEqual(usable, ipnetwork.Usable, "Usable");
            Assert.AreEqual(firstUsable, ipnetwork.FirstUsable.ToString(), "FirstUsable");
            Assert.AreEqual(lastUsable, ipnetwork.LastUsable.ToString(), "LastUsable");
        }


        [TestMethod]
        public void TestParseIPAddressNoNetmask1()
        {
            var ipaddress = "2001:0db8::";
            var ipnetwork = IPNetwork.Parse(ipaddress);


            var network = "2001:db8::";
            var netmask = "ffff:ffff:ffff:ffff::";
            var firstUsable = "2001:db8::";
            var lastUsable = "2001:db8::ffff:ffff:ffff:ffff";
            byte cidr = 64;
            var usable = BigInteger.Pow(2, 64);

            Assert.AreEqual(network, ipnetwork.Network.ToString(), "Network");
            Assert.AreEqual(netmask, ipnetwork.Netmask.ToString(), "Netmask");
            Assert.AreEqual(null, ipnetwork.Broadcast, "Broadcast");
            Assert.AreEqual(cidr, ipnetwork.Cidr, "Cidr");
            Assert.AreEqual(usable, ipnetwork.Usable, "Usable");
            Assert.AreEqual(firstUsable, ipnetwork.FirstUsable.ToString(), "FirstUsable");
            Assert.AreEqual(lastUsable, ipnetwork.LastUsable.ToString(), "LastUsable");
        }

        [TestMethod]
        public void TestParseIPAddressNoNetmask4()
        {
            var ipaddress = "::";
            var ipnetwork = IPNetwork.Parse(ipaddress);


            var network = "::";
            var netmask = "ffff:ffff:ffff:ffff::";
            var firstUsable = "::";
            var lastUsable = "::ffff:ffff:ffff:ffff";
            byte cidr = 64;
            var usable = BigInteger.Pow(2, 64);

            Assert.AreEqual(network, ipnetwork.Network.ToString(), "Network");
            Assert.AreEqual(netmask, ipnetwork.Netmask.ToString(), "Netmask");
            Assert.AreEqual(null, ipnetwork.Broadcast, "Broadcast");
            Assert.AreEqual(cidr, ipnetwork.Cidr, "Cidr");
            Assert.AreEqual(usable, ipnetwork.Usable, "Usable");
            Assert.AreEqual(firstUsable, ipnetwork.FirstUsable.ToString(), "FirstUsable");
            Assert.AreEqual(lastUsable, ipnetwork.LastUsable.ToString(), "LastUsable");
        }

        [TestMethod]
        public void TestParseIPAddressNoNetmask5()
        {
            var ipaddress = "2001:0db8::1";
            var ipnetwork = IPNetwork.Parse(ipaddress);

            var network = "2001:db8::";
            var netmask = "ffff:ffff:ffff:ffff::";
            var firstUsable = "2001:db8::";
            var lastUsable = "2001:db8::ffff:ffff:ffff:ffff";
            byte cidr = 64;
            var usable = BigInteger.Pow(2, 64);

            Assert.AreEqual(network, ipnetwork.Network.ToString(), "Network");
            Assert.AreEqual(netmask, ipnetwork.Netmask.ToString(), "Netmask");
            Assert.AreEqual(null, ipnetwork.Broadcast, "Broadcast");
            Assert.AreEqual(cidr, ipnetwork.Cidr, "Cidr");
            Assert.AreEqual(usable, ipnetwork.Usable, "Usable");
            Assert.AreEqual(firstUsable, ipnetwork.FirstUsable.ToString(), "FirstUsable");
            Assert.AreEqual(lastUsable, ipnetwork.LastUsable.ToString(), "LastUsable");
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
            var ipaddress = "2001:0db8::";
            var netmask = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:fff0";

            var ipnetwork = IPNetwork.Parse(ipaddress, netmask);
            Assert.AreEqual("2001:db8::/124", ipnetwork.ToString(), "network");
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void TestParseStringString3()
        {
            var ipaddress = "2001:0db8::";
            string netmask = null;

            var ipnetwork = IPNetwork.Parse(ipaddress, netmask);
        }


        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void TestParseStringString5()
        {
            var ipaddress = "2001:0db8::";
            var netmask = "";

            var ipnetwork = IPNetwork.Parse(ipaddress, netmask);
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
            var ipaddress = "2001:0db8::";
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
        public void TestToCidr128()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff");
            byte cidr = 128;
            int result = IPNetwork.ToCidr(mask);

            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestToCidr127()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:fffe");
            byte cidr = 127;
            int result = IPNetwork.ToCidr(mask);

            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestToCidr126()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:fffc");
            byte cidr = 126;
            int result = IPNetwork.ToCidr(mask);

            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestToCidr125()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:fff8");
            byte cidr = 125;
            int result = IPNetwork.ToCidr(mask);

            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestToCidr124()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:fff0");
            byte cidr = 124;
            int result = IPNetwork.ToCidr(mask);

            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestToCidr123()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffe0");
            byte cidr = 123;
            int result = IPNetwork.ToCidr(mask);

            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestToCidr122()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffc0");
            byte cidr = 122;
            int result = IPNetwork.ToCidr(mask);

            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestToCidr121()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:ff80");
            byte cidr = 121;
            int result = IPNetwork.ToCidr(mask);

            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestToCidr120()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:ff00");
            byte cidr = 120;
            int result = IPNetwork.ToCidr(mask);

            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestToCidr119()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:fe00");
            byte cidr = 119;
            int result = IPNetwork.ToCidr(mask);

            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestToCidr118()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:fc00");
            byte cidr = 118;
            int result = IPNetwork.ToCidr(mask);

            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestToCidr117()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:f800");
            byte cidr = 117;
            int result = IPNetwork.ToCidr(mask);

            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestToCidr116()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:f000");
            byte cidr = 116;
            int result = IPNetwork.ToCidr(mask);

            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestToCidr115()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:e000");
            byte cidr = 115;
            int result = IPNetwork.ToCidr(mask);

            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestToCidr114()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:c000");
            byte cidr = 114;
            int result = IPNetwork.ToCidr(mask);

            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestToCidr113()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:8000");
            byte cidr = 113;
            int result = IPNetwork.ToCidr(mask);

            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestToCidr112()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:0000");
            byte cidr = 112;
            int result = IPNetwork.ToCidr(mask);

            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestToCidr111()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:fffe:0");
            byte cidr = 111;
            int result = IPNetwork.ToCidr(mask);

            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestToCidr110()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:fffc:0");
            byte cidr = 110;
            int result = IPNetwork.ToCidr(mask);

            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestToCidr109()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:fff8:0");
            byte cidr = 109;
            int result = IPNetwork.ToCidr(mask);

            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestToCidr108()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:fff0:0");
            byte cidr = 108;
            int result = IPNetwork.ToCidr(mask);

            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestToCidr001()
        {
            var mask = IPAddress.Parse("8000::");
            byte cidr = 1;
            int result = IPNetwork.ToCidr(mask);

            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestToCidr000()
        {
            var mask = IPAddress.Parse("::");
            byte cidr = 0;
            int result = IPNetwork.ToCidr(mask);

            Assert.AreEqual(cidr, result, "cidr");
        }

        #endregion

        #region TryToCidr

        [TestMethod]
        public void TestTryToCidr128()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff");
            byte cidr = 128;
            byte? result = 0;
            var parsed = IPNetwork.TryToCidr(mask, out result);

            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestTryToCidr127()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:fffe");
            byte cidr = 127;
            byte? result = 0;
            var parsed = IPNetwork.TryToCidr(mask, out result);

            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestTryToCidr126()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:fffc");
            byte cidr = 126;
            byte? result = 0;
            var parsed = IPNetwork.TryToCidr(mask, out result);

            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestTryToCidr125()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:fff8");
            byte cidr = 125;
            byte? result = 0;
            var parsed = IPNetwork.TryToCidr(mask, out result);

            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestTryToCidr124()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:fff0");
            byte cidr = 124;
            byte? result = 0;
            var parsed = IPNetwork.TryToCidr(mask, out result);

            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestTryToCidr123()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffe0");
            byte cidr = 123;
            byte? result = 0;
            var parsed = IPNetwork.TryToCidr(mask, out result);

            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestTryToCidr122()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffc0");
            byte cidr = 122;
            byte? result = 0;
            var parsed = IPNetwork.TryToCidr(mask, out result);

            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestTryToCidr121()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:ff80");
            byte cidr = 121;
            byte? result = 0;
            var parsed = IPNetwork.TryToCidr(mask, out result);

            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestTryToCidr120()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:ff00");
            byte cidr = 120;
            byte? result = 0;
            var parsed = IPNetwork.TryToCidr(mask, out result);

            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestTryToCidr119()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:fe00");
            byte cidr = 119;
            byte? result = 0;
            var parsed = IPNetwork.TryToCidr(mask, out result);

            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestTryToCidr118()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:fc00");
            byte cidr = 118;
            byte? result = 0;
            var parsed = IPNetwork.TryToCidr(mask, out result);

            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestTryToCidr117()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:f800");
            byte cidr = 117;
            byte? result = 0;
            var parsed = IPNetwork.TryToCidr(mask, out result);

            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestTryToCidr116()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:f000");
            byte cidr = 116;
            byte? result = 0;
            var parsed = IPNetwork.TryToCidr(mask, out result);

            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestTryToCidr115()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:e000");
            byte cidr = 115;
            byte? result = 0;
            var parsed = IPNetwork.TryToCidr(mask, out result);

            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestTryToCidr114()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:c000");
            byte cidr = 114;
            byte? result = 0;
            var parsed = IPNetwork.TryToCidr(mask, out result);

            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestTryToCidr113()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:8000");
            byte cidr = 113;
            byte? result = 0;
            var parsed = IPNetwork.TryToCidr(mask, out result);

            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestTryToCidr112()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:0000");
            byte cidr = 112;
            byte? result = 0;
            var parsed = IPNetwork.TryToCidr(mask, out result);

            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestTryToCidr111()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:fffe:0");
            byte cidr = 111;
            byte? result = 0;
            var parsed = IPNetwork.TryToCidr(mask, out result);

            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestTryToCidr110()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:fffc:0");
            byte cidr = 110;
            byte? result = 0;
            var parsed = IPNetwork.TryToCidr(mask, out result);

            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestTryToCidr109()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:fff8:0");
            byte cidr = 109;
            byte? result = 0;
            var parsed = IPNetwork.TryToCidr(mask, out result);

            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestTryToCidr108()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:fff0:0");
            byte cidr = 108;
            byte? result = 0;
            var parsed = IPNetwork.TryToCidr(mask, out result);

            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestTryToCidr001()
        {
            var mask = IPAddress.Parse("8000::");
            byte cidr = 1;
            byte? result = 0;
            var parsed = IPNetwork.TryToCidr(mask, out result);

            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestTryToCidr000()
        {
            var mask = IPAddress.Parse("::");
            byte cidr = 0;
            byte? result = 0;
            var parsed = IPNetwork.TryToCidr(mask, out result);

            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(cidr, result, "cidr");
        }

        #endregion

        #region ToBigInteger

        [TestMethod]
        public void TestToBigInteger32()
        {
            var mask = IPAddress.Parse("::f");
            uint uintMask = 0xf;
            var result = IPNetwork.ToBigInteger(mask);

            Assert.AreEqual(uintMask, result, "uint");
        }

        [TestMethod]
        public void TestToBigInteger24()
        {
            var mask = IPAddress.Parse("::fff");
            uint uintMask = 0xfff;
            BigInteger? result = IPNetwork.ToBigInteger(mask);

            Assert.AreEqual(uintMask, result, "uint");
        }

        [TestMethod]
        public void TestToBigInteger16()
        {
            var mask = IPAddress.Parse("::ff");
            uint uintMask = 0xff;
            BigInteger? result = IPNetwork.ToBigInteger(mask);

            Assert.AreEqual(uintMask, result, "uint");
        }

        [TestMethod]
        public void TestToBigInteger8()
        {
            var mask = IPAddress.Parse("::ff00:0");
            var uintMask = 0xff000000;
            BigInteger? result = IPNetwork.ToBigInteger(mask);

            Assert.AreEqual(uintMask, result, "uint");
        }

        [TestMethod]
        public void TestToBigInteger0()
        {
            var mask = IPAddress.Parse("::");
            uint uintMask = 0x00000000;
            BigInteger? result = IPNetwork.ToBigInteger(mask);

            Assert.AreEqual(uintMask, result, "uint");
        }

        #endregion

        #region TryToBigInteger

        [TestMethod]
        public void TestTryToBigInteger32()
        {
            var mask = IPAddress.Parse("::ffff:ffff");
            var uintMask = 0xffffffff;
            BigInteger? result = null;
            var parsed = IPNetwork.TryToBigInteger(mask, out result);

            Assert.AreEqual(uintMask, result, "uint");
            Assert.AreEqual(true, parsed, "parsed");
        }

        [TestMethod]
        public void TestTryToBigInteger24()
        {
            var mask = IPAddress.Parse("::ffff:ff00");
            var uintMask = 0xffffff00;
            BigInteger? result = null;
            var parsed = IPNetwork.TryToBigInteger(mask, out result);

            Assert.AreEqual(uintMask, result, "uint");
            Assert.AreEqual(true, parsed, "parsed");
        }

        [TestMethod]
        public void TestTryToBigInteger16()
        {
            var mask = IPAddress.Parse("::ffff:0");
            var uintMask = 0xffff0000;
            BigInteger? result = null;
            var parsed = IPNetwork.TryToBigInteger(mask, out result);

            Assert.AreEqual(uintMask, result, "uint");
            Assert.AreEqual(true, parsed, "parsed");
        }

        [TestMethod]
        public void TestTryToBigInteger8()
        {
            var mask = IPAddress.Parse("::ff00:0");
            var uintMask = 0xff000000;

            BigInteger? result = null;
            var parsed = IPNetwork.TryToBigInteger(mask, out result);

            Assert.AreEqual(uintMask, result, "uint");
            Assert.AreEqual(true, parsed, "parsed");
        }

        [TestMethod]
        public void TestTryToBigInteger0()
        {
            var mask = IPAddress.Parse("::");
            uint uintMask = 0x00000000;
            BigInteger? result = null;
            var parsed = IPNetwork.TryToBigInteger(mask, out result);

            Assert.AreEqual(uintMask, result, "uint");
            Assert.AreEqual(true, parsed, "parsed");
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
        public void ToNetmask128()
        {
            byte cidr = 128;
            var netmask = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff";
            var result = IPNetwork.ToNetmask(cidr, AddressFamily.InterNetworkV6).ToString();

            Assert.AreEqual(netmask, result, "netmask");
        }

        [TestMethod]
        public void ToNetmask31()
        {
            byte cidr = 127;
            var netmask = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:fffe";
            var result = IPNetwork.ToNetmask(cidr, AddressFamily.InterNetworkV6).ToString();

            Assert.AreEqual(netmask, result, "netmask");
        }

        [TestMethod]
        public void ToNetmask30()
        {
            byte cidr = 126;
            var netmask = "ffff:ffff:ffff:ffff:ffff:ffff:ffff:fffc";
            var result = IPNetwork.ToNetmask(cidr, AddressFamily.InterNetworkV6).ToString();

            Assert.AreEqual(netmask, result, "netmask");
        }

        [TestMethod]
        public void ToNetmask1()
        {
            byte cidr = 1;
            var netmask = "8000::";
            var result = IPNetwork.ToNetmask(cidr, AddressFamily.InterNetworkV6).ToString();

            Assert.AreEqual(netmask, result, "netmask");
        }


        [TestMethod]
        public void ToNetmask0()
        {
            byte cidr = 0;
            var netmask = "::";
            var result = IPNetwork.ToNetmask(cidr, AddressFamily.InterNetworkV6).ToString();

            Assert.AreEqual(netmask, result, "netmask");
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public void ToNetmaskOORE1()
        {
            byte cidr = 129;
            var result = IPNetwork.ToNetmask(cidr, AddressFamily.InterNetworkV6).ToString();
        }

        #endregion

        #region ValidNetmask

        [TestMethod]
        public void TestValidNetmask0()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff");
            var expected = true;
            var result = IPNetwork.ValidNetmask(mask);

            Assert.AreEqual(expected, result, "ValidNetmask");
        }

        [TestMethod]
        public void TestValidNetmask1()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:fff0");
            var expected = true;
            var result = IPNetwork.ValidNetmask(mask);

            Assert.AreEqual(expected, result, "ValidNetmask");
        }

        [TestMethod]
        public void TestValidNetmask2()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:0000");
            var expected = true;
            var result = IPNetwork.ValidNetmask(mask);

            Assert.AreEqual(expected, result, "ValidNetmask");
        }


        [TestMethod]
        public void TestValidNetmaskEAE1()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:0000:ffff");
            var expected = false;
            var result = IPNetwork.ValidNetmask(mask);

            Assert.AreEqual(expected, result, "ValidNetmask");
        }

        [TestMethod]
        public void TestValidNetmaskEAE3()
        {
            var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:0000:0001");
            var expected = false;
            var result = IPNetwork.ValidNetmask(mask);

            Assert.AreEqual(expected, result, "ValidNetmask");
        }

        #endregion

        #region BitsSet

        [TestMethod]
        public void TestBitsSet128()
        {
            var ip = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff");
            uint bits = 128;
            var result = IPNetwork.BitsSet(ip);

            Assert.AreEqual(bits, result, "BitsSet");
        }

        [TestMethod]
        public void TestBitsSet120()
        {
            var ip = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:fff0");
            uint bits = 124;
            var result = IPNetwork.BitsSet(ip);

            Assert.AreEqual(bits, result, "BitsSet");
        }

        [TestMethod]
        public void TestBitsSet16()
        {
            var ip = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:0000");
            uint bits = 112;
            var result = IPNetwork.BitsSet(ip);

            Assert.AreEqual(bits, result, "BitsSet");
        }

        [TestMethod]
        public void TestBitsSet4()
        {
            var ip = IPAddress.Parse("f0f0:f0f0:f0f0:f0f0:f0f0:f0f0:f0f0:f0f0");
            uint bits = 64;
            var result = IPNetwork.BitsSet(ip);

            Assert.AreEqual(bits, result, "BitsSet");
        }

        #endregion

        #region Contains

        [TestMethod]
        public void TestContains1()
        {
            var ipnetwork = IPNetwork.Parse("2001:0db8::/64");
            var ipaddress = IPAddress.Parse("2001:0db8::1");

            var result = IPNetwork.Contains(ipnetwork, ipaddress);
            var expected = true;

            Assert.AreEqual(expected, result, "contains");
        }

        [TestMethod]
        public void TestContains2()
        {
            var ipnetwork = IPNetwork.Parse("2001:0db8::/64");
            var ipaddress = IPAddress.Parse("2001:0db8:0:1::");

            var result = IPNetwork.Contains(ipnetwork, ipaddress);
            var expected = false;

            Assert.AreEqual(expected, result, "contains");
        }

        [TestMethod]
        public void TestContains3()
        {
            var ipnetwork = IPNetwork.Parse("2001:0db8::/64");
            var ipnetwork2 = IPNetwork.Parse("2001:0db8::/64");

            var result = IPNetwork.Contains(ipnetwork, ipnetwork2);
            var expected = true;

            Assert.AreEqual(expected, result, "contains");
        }

        [TestMethod]
        public void TestContains4()
        {
            var ipnetwork = IPNetwork.Parse("2001:0db8::/64");
            var ipnetwork2 = IPNetwork.Parse("2001:0db8::/65");

            var result = IPNetwork.Contains(ipnetwork, ipnetwork2);
            var expected = true;

            Assert.AreEqual(expected, result, "contains");
        }

        [TestMethod]
        public void TestContains5()
        {
            var ipnetwork = IPNetwork.Parse("2001:0db8::/64");
            var ipnetwork2 = IPNetwork.Parse("2001:0db8:1::/65");

            var result = IPNetwork.Contains(ipnetwork, ipnetwork2);
            var expected = false;

            Assert.AreEqual(expected, result, "contains");
        }


        [TestMethod]
        public void TestContains6()
        {
            var ipnetwork = IPNetwork.Parse("2001:0db8::/64");
            var ipnetwork2 = IPNetwork.Parse("2001:0db8::/63");

            var result = IPNetwork.Contains(ipnetwork, ipnetwork2);
            var expected = false;

            Assert.AreEqual(expected, result, "contains");
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void TestContains10()
        {
            var ipnetwork = IPNetwork.Parse("::/0");
            IPAddress ipaddress = null;

            var result = IPNetwork.Contains(ipnetwork, ipaddress);
        }

        #endregion

        #region Overlap

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void TestOverlap2()
        {
            var network1 = IPNetwork.Parse("2001:0db8::/0");
            IPNetwork network2 = null;
            IPNetwork.Overlap(network1, network2);
        }

        [TestMethod]
        public void TestOverlap3()
        {
            var network1 = IPNetwork.Parse("2001:0db8::/64");
            var network2 = IPNetwork.Parse("2001:0db8::/64");
            var result = IPNetwork.Overlap(network1, network2);
            var expected = true;

            Assert.AreEqual(expected, result, "overlap");
        }

        [TestMethod]
        public void TestOverlap4()
        {
            var network1 = IPNetwork.Parse("2001:0db8::/64");
            var network2 = IPNetwork.Parse("2001:0db8:0:0:1::/65");
            var result = IPNetwork.Overlap(network1, network2);
            var expected = true;

            Assert.AreEqual(expected, result, "overlap");
        }

        [TestMethod]
        public void TestOverlap5()
        {
            var network1 = IPNetwork.Parse("2001:0db8:0:1::/68");
            var network2 = IPNetwork.Parse("2001:0db8:0:2::/68");
            var result = IPNetwork.Overlap(network1, network2);
            var expected = false;

            Assert.AreEqual(expected, result, "overlap");
        }

        [TestMethod]
        public void TestOverlap6()
        {
            var network1 = IPNetwork.Parse("2001:0db8:0:1::/68");
            var network2 = IPNetwork.Parse("2001:0db8:0:2::/62");
            var result = IPNetwork.Overlap(network1, network2);
            var expected = true;

            Assert.AreEqual(expected, result, "overlap");
        }

        #endregion

        #region Examples

        public void Example1()
        {
            var ipnetwork = IPNetwork.Parse("2001:0db8::/64");

            Console.WriteLine("Network : {0}", ipnetwork.Network);
            Console.WriteLine("Netmask : {0}", ipnetwork.Netmask);
            Console.WriteLine("Broadcast : {0}", ipnetwork.Broadcast);
            Console.WriteLine("FirstUsable : {0}", ipnetwork.FirstUsable);
            Console.WriteLine("LastUsable : {0}", ipnetwork.LastUsable);
            Console.WriteLine("Usable : {0}", ipnetwork.Usable);
            Console.WriteLine("Cidr : {0}", ipnetwork.Cidr);
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

            Console.WriteLine("{0} contains {1} : {2}", ipnetwork, ipaddress, contains1);
            Console.WriteLine("{0} contains {1} : {2}", ipnetwork, ipaddress2, contains2);
            Console.WriteLine("{0} contains {1} : {2}", ipnetwork, ipnetwork2, contains3);
            Console.WriteLine("{0} contains {1} : {2}", ipnetwork, ipnetwork3, contains4);


            Console.WriteLine("{0} overlap {1} : {2}", ipnetwork, ipnetwork2, overlap1);
            Console.WriteLine("{0} overlap {1} : {2}", ipnetwork, ipnetwork3, overlap2);
        }

        public void Example4()
        {
            var wholeInternet = IPNetwork.Parse("::/0");
            byte newCidr = 2;
            var subneted = IPNetwork.Subnet(wholeInternet, newCidr);

            Console.WriteLine("{0} was subnetted into {1} subnets", wholeInternet, subneted.Count);
            Console.WriteLine("First: {0}", subneted[0]);
            Console.WriteLine("Last : {0}", subneted[subneted.Count - 1]);
            Console.WriteLine("All  :");

            foreach (var ipnetwork in subneted)
            {
                Console.WriteLine("{0}", ipnetwork);
            }
        }

        public void Example5()
        {
            var ipnetwork1 = IPNetwork.Parse("2001:0db8::/64");
            var ipnetwork2 = IPNetwork.Parse("2001:0db9::/64");
            var ipnetwork3 = IPNetwork.Supernet(new[] {ipnetwork1, ipnetwork2});

            Console.WriteLine("{0} + {1} = {2}", ipnetwork1, ipnetwork2, ipnetwork3[0]);
        }

        public void Example6()
        {
            var ipnetwork = IPNetwork.Parse("fe80::202:b3ff:fe1e:8329/24");

            var ipaddress = IPAddress.Parse("2001:db8::");
            var ipaddress2 = IPAddress.Parse("fe80::202:b3ff:fe1e:1");

            var contains1 = IPNetwork.Contains(ipnetwork, ipaddress);
            var contains2 = IPNetwork.Contains(ipnetwork, ipaddress2);

            Console.WriteLine("{0} contains {1} : {2}", ipnetwork, ipaddress, contains1);
            Console.WriteLine("{0} contains {1} : {2}", ipnetwork, ipaddress2, contains2);
        }


        public void Example8()
        {
            var network = IPNetwork.Parse("::/124");
            var ips = IPNetwork.Subnet(network, 128);

            foreach (var ip in ips)
            {
                Console.WriteLine("{0}", ip);
            }
        }

        #endregion

        #region ToString

        [TestMethod]
        public void TestToString()
        {
            var ipnetwork = IPNetwork.Parse("2001:0db8:0000:0000:0000:0000:0000:0000/32");
            var expected = "2001:db8::/32";
            var result = ipnetwork.ToString();

            Assert.AreEqual(expected, result, "ToString");
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
            var expected = "2001:db8:1:2::/64";
            var result = ipnetwork.ToString();

            Assert.AreEqual(expected, result, "ToString");
        }

        [TestMethod]
        public void TestToString3()
        {
            var ipnetwork = IPNetwork.Parse("2001:0db8:1:2:3:4:5:6/100");
            var expected = "2001:db8:1:2:3:4::/100";
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
            byte cidr = 129;

            var subnets = IPNetwork.Subnet(ipnetwork, cidr);
        }


        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void TestSubnet4()
        {
            var ipnetwork = IPNetwork.Parse("::");
            byte cidr = 1;

            var subnets = IPNetwork.Subnet(ipnetwork, cidr);
        }


        [TestMethod]
        public void TestSubnet5()
        {
            var ipnetwork = IPNetwork.Parse("1:1:1:1:1:1:1:1");
            byte cidr = 65;

            var subnets = IPNetwork.Subnet(ipnetwork, cidr);
            Assert.AreEqual(2, subnets.Count, "count");
            Assert.AreEqual("1:1:1:1::/65", subnets[0].ToString(), "subnet1");
            Assert.AreEqual("1:1:1:1:8000::/65", subnets[1].ToString(), "subnet2");
        }


        [TestMethod]
        public void TestSubnet6()
        {
            var ipnetwork = IPNetwork.Parse("1:1:1:1:1:1:1:1");
            byte cidr = 68;

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
            byte cidr = 72;

            var subnets = IPNetwork.Subnet(ipnetwork, cidr);
            Assert.AreEqual(256, subnets.Count, "count");
            Assert.AreEqual("1:1:1:1::/72", subnets[0].ToString(), "subnet1");
            Assert.AreEqual("1:1:1:1:ff00::/72", subnets[255].ToString(), "subnet256");
        }


        [TestMethod]
        public void TestSubnet9()
        {
            var ipnetwork = IPNetwork.Parse("2001:db08:1:1:1:1:1:1");
            byte cidr = 128;
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
            byte cidr = 128;
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
            byte cidr = 70;
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
            byte cidr = 70;
            var subnets = IPNetwork.Subnet(ipnetwork, cidr);
            var error = subnets[1000];
        }

        #endregion

        #region TrySubnet

        [TestMethod]
        public void TestTrySubnet3()
        {
            var ipnetwork = IPNetwork.Parse("2001:db08::/64");
            byte cidr = 255;

            IPNetworkCollection subnets = null;
            var subnetted = IPNetwork.TrySubnet(ipnetwork, cidr, out subnets);

            Assert.AreEqual(false, subnetted, "subnetted");
        }


        [TestMethod]
        public void TestTrySubnet4()
        {
            var ipnetwork = IPNetwork.Parse("2001:db08::/64");
            byte cidr = 63;

            IPNetworkCollection subnets = null;
            var subnetted = IPNetwork.TrySubnet(ipnetwork, cidr, out subnets);

            Assert.AreEqual(false, subnetted, "subnetted");
        }


        [TestMethod]
        public void TestTrySubnet5()
        {
            var ipnetwork = IPNetwork.Parse("2001:db8::/64");
            byte cidr = 65;

            IPNetworkCollection subnets = null;
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
            byte cidr = 68;

            IPNetworkCollection subnets = null;
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
        public void TestTrySupernet1()
        {
            var network1 = IPNetwork.Parse("2001:db8::/65");
            var network2 = IPNetwork.Parse("2001:db8:0:0:8000::/65");
            var supernetExpected = IPNetwork.Parse("2001:db8::/64");
            IPNetwork supernet;
            var supernetted = true;
            var result = IPNetwork.TrySupernet(network1, network2, out supernet);

            Assert.AreEqual(supernetted, result, "supernetted");
            Assert.AreEqual(supernetExpected, supernet, "supernet");
        }

        [TestMethod]
        public void TestTrySupernet2()
        {
            IPNetwork network1 = null;
            var network2 = IPNetwork.Parse("2001:db8::/64");
            IPNetwork supernetExpected = null;
            IPNetwork supernet;
            var parsed = false;
            var result = IPNetwork.TrySupernet(network1, network2, out supernet);

            Assert.AreEqual(supernetExpected, supernet, "supernet");
            Assert.AreEqual(parsed, result, "parsed");
        }

        [TestMethod]
        public void TestTrySupernet3()
        {
            var network1 = IPNetwork.Parse("2001:db8::/64");
            IPNetwork network2 = null;
            IPNetwork supernetExpected = null;
            IPNetwork supernet;
            var parsed = false;
            var result = IPNetwork.TrySupernet(network1, network2, out supernet);

            Assert.AreEqual(supernetExpected, supernet, "supernet");
            Assert.AreEqual(parsed, result, "parsed");
        }

        [TestMethod]
        public void TestTrySupernet4()
        {
            var network1 = IPNetwork.Parse("2001:db8::/64");
            var network2 = IPNetwork.Parse("2001:db9::/65");
            IPNetwork supernetExpected = null;
            IPNetwork supernet;
            var parsed = false;
            var result = IPNetwork.TrySupernet(network1, network2, out supernet);

            Assert.AreEqual(supernetExpected, supernet, "supernet");
            Assert.AreEqual(parsed, result, "parsed");
        }

        [TestMethod]
        public void TestTrySupernet5()
        {
            var network1 = IPNetwork.Parse("2001:db8::/64");
            var network2 = IPNetwork.Parse("2001:dba::/64");
            IPNetwork supernetExpected = null;
            IPNetwork supernet;
            var parsed = false;
            var result = IPNetwork.TrySupernet(network1, network2, out supernet);

            Assert.AreEqual(supernetExpected, supernet, "supernet");
            Assert.AreEqual(parsed, result, "parsed");
        }

        [TestMethod]
        public void TestTrySupernet6()
        {
            var network1 = IPNetwork.Parse("2001:db8::/64");
            var network2 = IPNetwork.Parse("2001:db8::1/65");
            var supernetExpected = IPNetwork.Parse("2001:db8::/64");
            IPNetwork supernet;
            var parsed = true;
            var result = IPNetwork.TrySupernet(network1, network2, out supernet);

            Assert.AreEqual(supernetExpected, supernet, "supernet");
            Assert.AreEqual(parsed, result, "parsed");
        }

        [TestMethod]
        public void TestTrySupernet7()
        {
            var network1 = IPNetwork.Parse("2001:db8::/64");
            var network2 = IPNetwork.Parse("2001:db8::1/65");
            var supernetExpected = IPNetwork.Parse("2001:db8::/64");
            IPNetwork supernet;
            var parsed = true;
            var result = IPNetwork.TrySupernet(network1, network2, out supernet);

            Assert.AreEqual(supernetExpected, supernet, "supernet");
            Assert.AreEqual(parsed, result, "parsed");
        }

        [TestMethod]
        public void TestTrySupernet8()
        {
            var network1 = IPNetwork.Parse("2001:db0::/64");
            var network2 = IPNetwork.Parse("2001:dbf::/64");
            IPNetwork supernetExpected = null;
            IPNetwork supernet;
            var parsed = false;
            var result = IPNetwork.TrySupernet(network1, network2, out supernet);

            Assert.AreEqual(supernetExpected, supernet, "supernet");
            Assert.AreEqual(parsed, result, "parsed");
        }

        [TestMethod]
        public void TestTrySupernet9()
        {
            var network1 = IPNetwork.Parse("192.168.1.1/24");
            var network2 = IPNetwork.Parse("192.168.2.1/24");
            IPNetwork[] network3 = {network1, network2};
            IPNetwork[] supernetExpected = {network1, network2};
            IPNetwork[] supernet;
            var parsed = true;
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
            var parsed = true;
            var result = IPNetwork.TrySupernet(network3, out supernet);

            Assert.AreEqual(supernetExpected[0], supernet[0], "supernet");
            Assert.AreEqual(parsed, result, "parsed");
        }


        [TestMethod]
        public void TestTrySupernet11()
        {
            IPNetwork[] network3 = null;
            IPNetwork[] supernetExpected = {IPNetwork.Parse("2001:db8::/64")};
            IPNetwork[] supernet;
            var parsed = false;
            var result = IPNetwork.TrySupernet(network3, out supernet);

            Assert.AreEqual(null, supernet, "supernet");
            Assert.AreEqual(parsed, result, "parsed");
        }

        #endregion

        #region TryGuessCidr

        [TestMethod]
        public void TestTryGuessCidrNull()
        {
            byte cidr;
            var parsed = IPNetwork.TryGuessCidr(null, out cidr);

            Assert.AreEqual(false, parsed, "parsed");
            Assert.AreEqual(0, cidr, "cidr");
        }

        [TestMethod]
        public void TestTryGuessCidr1()
        {
            byte cidr;
            var parsed = IPNetwork.TryGuessCidr("::", out cidr);

            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(64, cidr, "cidr");
        }

        [TestMethod]
        public void TestTryGuessCidr2()
        {
            byte cidr;
            var parsed = IPNetwork.TryGuessCidr("2001:0db8::", out cidr);

            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(64, cidr, "cidr");
        }

        #endregion

        #region Count

        [TestMethod]
        public void Total32()
        {
            var network = IPNetwork.Parse("::/128");
            var total = 1;
            Assert.AreEqual(total, network.Total, "Total");
        }

        [TestMethod]
        public void Total31()
        {
            var network = IPNetwork.Parse("::/127");
            var total = 2;
            Assert.AreEqual(total, network.Total, "Total");
        }

        [TestMethod]
        public void Total30()
        {
            var network = IPNetwork.Parse("::/126");
            var total = 4;
            Assert.AreEqual(total, network.Total, "Total");
        }

        [TestMethod]
        public void Total24()
        {
            var network = IPNetwork.Parse("::/120");
            var total = 256;
            Assert.AreEqual(total, network.Total, "Total");
        }

        [TestMethod]
        public void Total16()
        {
            var network = IPNetwork.Parse("::/112");
            var total = 65536;
            Assert.AreEqual(total, network.Total, "Total");
        }

        [TestMethod]
        public void Total8()
        {
            var network = IPNetwork.Parse("::/104");
            var total = 16777216;
            Assert.AreEqual(total, network.Total, "Total");
        }

        [TestMethod]
        public void Total0()
        {
            var network = IPNetwork.Parse("::/0");
            var total = BigInteger.Pow(2, 128);
            Assert.AreEqual(total, network.Total, "Total");
        }

        #endregion

        #region Usable

        [TestMethod]
        public void Usable32()
        {
            var network = IPNetwork.Parse("::/128");
            uint usable = 1;
            Assert.AreEqual(usable, network.Usable, "Usable");
        }

        [TestMethod]
        public void Usable31()
        {
            var network = IPNetwork.Parse("::/127");
            uint usable = 2;
            Assert.AreEqual(usable, network.Usable, "Usable");
        }

        [TestMethod]
        public void Usable30()
        {
            var network = IPNetwork.Parse("::/126");
            uint usable = 4;
            Assert.AreEqual(usable, network.Usable, "Usable");
        }

        [TestMethod]
        public void Usable24()
        {
            var network = IPNetwork.Parse("::/120");
            uint usable = 256;
            Assert.AreEqual(usable, network.Usable, "Usable");
        }

        [TestMethod]
        public void Usable16()
        {
            var network = IPNetwork.Parse("::/112");
            uint usable = 65536;
            Assert.AreEqual(usable, network.Usable, "Usable");
        }

        [TestMethod]
        public void Usable8()
        {
            var network = IPNetwork.Parse("::/104");
            uint usable = 16777216;
            Assert.AreEqual(usable, network.Usable, "Usable");
        }

        [TestMethod]
        public void Usable0()
        {
            var network = IPNetwork.Parse("::/0");
            var usable = BigInteger.Pow(2, 128);
            Assert.AreEqual(usable, network.Usable, "Usable");
        }

        #endregion

        #region TryParseCidr

        [TestMethod]
        public void TryParseCidr1()
        {
            var sidr = "0";
            byte? cidr;
            byte? result = 0;
            var parsed = IPNetwork.TryParseCidr(sidr, AddressFamily.InterNetworkV6, out cidr);

            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(result, cidr, "cidr");
        }

        [TestMethod]
        public void TryParseCidr2()
        {
            var sidr = "sadsd";
            byte? cidr;
            byte? result = null;

            var parsed = IPNetwork.TryParseCidr(sidr, AddressFamily.InterNetworkV6, out cidr);

            Assert.AreEqual(false, parsed, "parsed");
            Assert.AreEqual(result, cidr, "cidr");
        }

        [TestMethod]
        public void TryParseCidr33()
        {
            var sidr = "33";
            byte? cidr;
            byte result = 33;

            var parsed = IPNetwork.TryParseCidr(sidr, AddressFamily.InterNetworkV6, out cidr);

            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(result, cidr, "cidr");
        }

        [TestMethod]
        public void TryParseCidr128()
        {
            var sidr = "128";
            byte? cidr;
            byte result = 128;

            var parsed = IPNetwork.TryParseCidr(sidr, AddressFamily.InterNetworkV6, out cidr);

            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(result, cidr, "cidr");
        }

        [TestMethod]
        public void TryParseCidr129()
        {
            var sidr = "129";
            byte? cidr;
            byte? result = null;

            var parsed = IPNetwork.TryParseCidr(sidr, AddressFamily.InterNetworkV6, out cidr);

            Assert.AreEqual(false, parsed, "parsed");
            Assert.AreEqual(result, cidr, "cidr");
        }

        #endregion
    }
}