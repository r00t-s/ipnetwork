using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Net.IPNetwork.TestProject
{
    /// <summary>
    ///     IPNetworkUnitTest test every single method
    /// </summary>
    [TestClass]
    public class IpNetworkUnitTest
    {
        #region Parse

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestParseIpAddressNetmaskAne2()
        {
            IPAddress ip = null;
            IpNetwork.Parse(ip, ip);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestParseIpAddressNetmaskAne3()
        {
            IpNetwork.Parse("", 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestParseIpAddressNetmaskAne4()
        {
            IpNetwork.Parse(null, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestParseIpAddressNetmaskAne5()
        {
            string n = null;
            IpNetwork.Parse(n, n);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestParseIpAddressNetmaskAne8()
        {
            var ipnet = IpNetwork.Parse("x.x.x.x", "x.x.x.x");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestParseIpAddressNetmaskAne9()
        {
            var ipnet = IpNetwork.Parse("0.0.0.0", "x.x.x.x");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestParseIpAddressNetmaskAne10()
        {
            var ipnet = IpNetwork.Parse("x.x.x.x", 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestParseIpAddressNetmaskAne11()
        {
            var ipnet = IpNetwork.Parse("0.0.0.0", 33);
        }

        [TestMethod]
        public void TestParseIpAddressNetmask()
        {
            const string ipaddress = "192.168.168.100";
            const string netmask = "255.255.255.0";

            const string network = "192.168.168.0";
            const string broadcast = "192.168.168.255";
            const string firstUsable = "192.168.168.1";
            const string lastUsable = "192.168.168.254";
            const byte cidr = 24;
            const uint usable = 254;

            var ipnetwork = IpNetwork.Parse(ipaddress, netmask);
            Assert.AreEqual(network, ipnetwork.Network.ToString(), "Network");
            Assert.AreEqual(netmask, ipnetwork.Netmask.ToString(), "Netmask");
            Assert.AreEqual(broadcast, ipnetwork.Broadcast.ToString(), "Broadcast");
            Assert.AreEqual(cidr, ipnetwork.Cidr, "Cidr");
            Assert.AreEqual(usable, ipnetwork.Usable, "Usable");
            Assert.AreEqual(firstUsable, ipnetwork.FirstUsable.ToString(), "FirstUsable");
            Assert.AreEqual(lastUsable, ipnetwork.LastUsable.ToString(), "LastUsable");
        }


        [TestMethod]
        public void TestParseString1()
        {
            const string ipaddress = "192.168.168.100 255.255.255.0";

            const string network = "192.168.168.0";
            const string netmask = "255.255.255.0";
            const string broadcast = "192.168.168.255";
            const string firstUsable = "192.168.168.1";
            const string lastUsable = "192.168.168.254";
            const int cidr = 24;
            const int usable = 254;

            var ipnetwork = IpNetwork.Parse(ipaddress);
            Assert.AreEqual(network, ipnetwork.Network.ToString(), "Network");
            Assert.AreEqual(netmask, ipnetwork.Netmask.ToString(), "Netmask");
            Assert.AreEqual(broadcast, ipnetwork.Broadcast.ToString(), "Broadcast");
            Assert.AreEqual(cidr, ipnetwork.Cidr, "Cidr");
            Assert.AreEqual(usable, ipnetwork.Usable, "Usable");
            Assert.AreEqual(firstUsable, ipnetwork.FirstUsable.ToString(), "FirstUsable");
            Assert.AreEqual(lastUsable, ipnetwork.LastUsable.ToString(), "LastUsable");
        }


        [TestMethod]
        public void TestParseString2()
        {
            const string ipaddress = "192.168.168.100/24";

            const string network = "192.168.168.0";
            const string netmask = "255.255.255.0";
            const string broadcast = "192.168.168.255";
            const string firstUsable = "192.168.168.1";
            const string lastUsable = "192.168.168.254";
            const byte cidr = 24;
            const uint usable = 254;

            var ipnetwork = IpNetwork.Parse(ipaddress);
            Assert.AreEqual(network, ipnetwork.Network.ToString(), "Network");
            Assert.AreEqual(netmask, ipnetwork.Netmask.ToString(), "Netmask");
            Assert.AreEqual(broadcast, ipnetwork.Broadcast.ToString(), "Broadcast");
            Assert.AreEqual(cidr, ipnetwork.Cidr, "Cidr");
            Assert.AreEqual(usable, ipnetwork.Usable, "Usable");
            Assert.AreEqual(firstUsable, ipnetwork.FirstUsable.ToString(), "FirstUsable");
            Assert.AreEqual(lastUsable, ipnetwork.LastUsable.ToString(), "LastUsable");
        }


        [TestMethod]
        public void TestParseString3()
        {
            const string ipaddress = "0.0.0.0/0";

            const string network = "0.0.0.0";
            const string netmask = "0.0.0.0";
            const string broadcast = "255.255.255.255";
            const string firstUsable = "0.0.0.1";
            const string lastUsable = "255.255.255.254";
            const byte cidr = 0;
            const uint usable = 4294967294;

            var ipnetwork = IpNetwork.Parse(ipaddress);
            Assert.AreEqual(network, ipnetwork.Network.ToString(), "Network");
            Assert.AreEqual(netmask, ipnetwork.Netmask.ToString(), "Netmask");
            Assert.AreEqual(broadcast, ipnetwork.Broadcast.ToString(), "Broadcast");
            Assert.AreEqual(cidr, ipnetwork.Cidr, "Cidr");
            Assert.AreEqual(usable, ipnetwork.Usable, "Usable");
            Assert.AreEqual(firstUsable, ipnetwork.FirstUsable.ToString(), "FirstUsable");
            Assert.AreEqual(lastUsable, ipnetwork.LastUsable.ToString(), "LastUsable");
        }


        [TestMethod]
        public void TestParseString4()
        {
            const string ipaddress = "0.0.0.0/32";

            const string network = "0.0.0.0";
            const string netmask = "255.255.255.255";
            const string broadcast = "0.0.0.0";
            const string firstUsable = "0.0.0.0";
            const string lastUsable = "0.0.0.0";
            const byte cidr = 32;
            const uint usable = 0;

            var ipnetwork = IpNetwork.Parse(ipaddress);
            Assert.AreEqual(network, ipnetwork.Network.ToString(), "Network");
            Assert.AreEqual(netmask, ipnetwork.Netmask.ToString(), "Netmask");
            Assert.AreEqual(broadcast, ipnetwork.Broadcast.ToString(), "Broadcast");
            Assert.AreEqual(cidr, ipnetwork.Cidr, "Cidr");
            Assert.AreEqual(usable, ipnetwork.Usable, "Usable");
            Assert.AreEqual(firstUsable, ipnetwork.FirstUsable.ToString(), "FirstUsable");
            Assert.AreEqual(lastUsable, ipnetwork.LastUsable.ToString(), "LastUsable");
        }


        [TestMethod]
        public void TestParseString5()
        {
            const string ipaddress = "255.255.255.255/32";

            const string network = "255.255.255.255";
            const string netmask = "255.255.255.255";
            const string broadcast = "255.255.255.255";
            const string firstUsable = "255.255.255.255";
            const string lastUsable = "255.255.255.255";
            const byte cidr = 32;
            const uint usable = 0;

            var ipnetwork = IpNetwork.Parse(ipaddress);
            Assert.AreEqual(network, ipnetwork.Network.ToString(), "Network");
            Assert.AreEqual(netmask, ipnetwork.Netmask.ToString(), "Netmask");
            Assert.AreEqual(broadcast, ipnetwork.Broadcast.ToString(), "Broadcast");
            Assert.AreEqual(cidr, ipnetwork.Cidr, "Cidr");
            Assert.AreEqual(usable, ipnetwork.Usable, "Usable");
            Assert.AreEqual(firstUsable, ipnetwork.FirstUsable.ToString(), "FirstUsable");
            Assert.AreEqual(lastUsable, ipnetwork.LastUsable.ToString(), "LastUsable");
        }


        [TestMethod]
        public void TestParseIpAddressNoNetmask1()
        {
            const string ipaddress = "10.0.0.0";

            const string network = "10.0.0.0";
            const string netmask = "255.0.0.0";
            const string broadcast = "10.255.255.255";
            const string firstUsable = "10.0.0.1";
            const string lastUsable = "10.255.255.254";
            const byte cidr = 8;
            const uint usable = 16777214;

            var ipnetwork = IpNetwork.Parse(ipaddress);
            Assert.AreEqual(network, ipnetwork.Network.ToString(), "Network");
            Assert.AreEqual(netmask, ipnetwork.Netmask.ToString(), "Netmask");
            Assert.AreEqual(broadcast, ipnetwork.Broadcast.ToString(), "Broadcast");
            Assert.AreEqual(cidr, ipnetwork.Cidr, "Cidr");
            Assert.AreEqual(usable, ipnetwork.Usable, "Usable");
            Assert.AreEqual(firstUsable, ipnetwork.FirstUsable.ToString(), "FirstUsable");
            Assert.AreEqual(lastUsable, ipnetwork.LastUsable.ToString(), "LastUsable");
        }


        [TestMethod]
        public void TestParseIpAddressNoNetmask2()
        {
            const string ipaddress = "172.0.0.0";

            const string network = "172.0.0.0";
            const string netmask = "255.255.0.0";
            const string broadcast = "172.0.255.255";
            const string firstUsable = "172.0.0.1";
            const string lastUsable = "172.0.255.254";
            const byte cidr = 16;
            const uint usable = 65534;

            var ipnetwork = IpNetwork.Parse(ipaddress);
            Assert.AreEqual(network, ipnetwork.Network.ToString(), "Network");
            Assert.AreEqual(netmask, ipnetwork.Netmask.ToString(), "Netmask");
            Assert.AreEqual(broadcast, ipnetwork.Broadcast.ToString(), "Broadcast");
            Assert.AreEqual(cidr, ipnetwork.Cidr, "Cidr");
            Assert.AreEqual(usable, ipnetwork.Usable, "Usable");
            Assert.AreEqual(firstUsable, ipnetwork.FirstUsable.ToString(), "FirstUsable");
            Assert.AreEqual(lastUsable, ipnetwork.LastUsable.ToString(), "LastUsable");
        }

        [TestMethod]
        public void TestParseIpAddressNoNetmask3()
        {
            const string ipaddress = "192.0.0.0";

            const string network = "192.0.0.0";
            const string netmask = "255.255.255.0";
            const string broadcast = "192.0.0.255";
            const string firstUsable = "192.0.0.1";
            const string lastUsable = "192.0.0.254";
            byte cidr = 24;
            uint usable = 254;

            var ipnetwork = IpNetwork.Parse(ipaddress);
            Assert.AreEqual(network, ipnetwork.Network.ToString(), "Network");
            Assert.AreEqual(netmask, ipnetwork.Netmask.ToString(), "Netmask");
            Assert.AreEqual(broadcast, ipnetwork.Broadcast.ToString(), "Broadcast");
            Assert.AreEqual(cidr, ipnetwork.Cidr, "Cidr");
            Assert.AreEqual(usable, ipnetwork.Usable, "Usable");
            Assert.AreEqual(firstUsable, ipnetwork.FirstUsable.ToString(), "FirstUsable");
            Assert.AreEqual(lastUsable, ipnetwork.LastUsable.ToString(), "LastUsable");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestParseIpAddressNoNetmask4()
        {
            const string ipaddress = "224.0.0.0";
            var ipnetwork = IpNetwork.Parse(ipaddress);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestParseIpAddressNoNetmask5()
        {
            const string ipaddress = "240.0.0.0";
            var ipnetwork = IpNetwork.Parse(ipaddress);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestParseStringAe1()
        {
            const string ipaddress = "garbage";
            var ipnetwork = IpNetwork.Parse(ipaddress);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestParseStringAe2()
        {
            const string ipaddress = "0.0.0.0 0.0.1.0";
            var ipnetwork = IpNetwork.Parse(ipaddress);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestParseStringAne1()
        {
            string ipaddress = null;
            var ipnetwork = IpNetwork.Parse(ipaddress);
        }

        #endregion

        #region TryParse

        [TestMethod]
        public void TestTryParseIpAddressNetmaskAne2()
        {
            IpNetwork ipnet;
            IPAddress ip = null;
            var parsed = IpNetwork.TryParse(ip, ip, out ipnet);

            Assert.AreEqual(false, parsed, "parsed");
            Assert.AreEqual(null, ipnet, "ipnet");
        }

        [TestMethod]
        public void TestTryParseIpAddressNetmaskAne3()
        {
            IpNetwork ipnet;
            var parsed = IpNetwork.TryParse("", 0, out ipnet);

            Assert.AreEqual(false, parsed, "parsed");
            Assert.AreEqual(null, ipnet, "ipnet");
        }

        [TestMethod]
        public void TestTryParseIpAddressNetmaskAne4()
        {
            IpNetwork ipnet;
            var parsed = IpNetwork.TryParse(null, 0, out ipnet);

            Assert.AreEqual(false, parsed, "parsed");
            Assert.AreEqual(null, ipnet, "ipnet");
        }

        [TestMethod]
        public void TestTryParseIpAddressNetmaskAne5()
        {
            string n = null;
            IpNetwork ipnet;
            var parsed = IpNetwork.TryParse(n, n, out ipnet);

            Assert.AreEqual(false, parsed, "parsed");
            Assert.AreEqual(null, ipnet, "ipnet");
        }

        [TestMethod]
        public void TestTryParseIpAddressNetmaskAne6()
        {
            IpNetwork ipnet;
            var parsed = IpNetwork.TryParse(IPAddress.Parse("10.10.10.10"), null, out ipnet);
            Assert.AreEqual(false, parsed, "parsed");
            Assert.AreEqual(null, ipnet, "ipnet");
        }

        [TestMethod]
        public void TestTryParseIpAddressNetmaskAne7()
        {
            IpNetwork ipnet;
            var parsed = IpNetwork.TryParse("0.0.0.0", null, out ipnet);

            Assert.AreEqual(false, parsed, "parsed");
            Assert.AreEqual(null, ipnet, "ipnet");
        }

        [TestMethod]
        public void TestTryParseIpAddressNetmaskAne8()
        {
            IpNetwork ipnet = null;
            var parsed = IpNetwork.TryParse("x.x.x.x", "x.x.x.x", out ipnet);

            Assert.AreEqual(false, parsed, "parsed");
            Assert.AreEqual(null, ipnet, "ipnet");
        }

        [TestMethod]
        public void TestTryParseIpAddressNetmaskAne9()
        {
            IpNetwork ipnet;
            var parsed = IpNetwork.TryParse("0.0.0.0", "x.x.x.x", out ipnet);

            Assert.AreEqual(false, parsed, "parsed");
            Assert.AreEqual(null, ipnet, "ipnet");
        }


        [TestMethod]
        public void TestTryParseIpAddressNetmaskAne10()
        {
            IpNetwork ipnet;
            var parsed = IpNetwork.TryParse("x.x.x.x", 0, out ipnet);

            Assert.AreEqual(false, parsed, "parsed");
            Assert.AreEqual(null, ipnet, "ipnet");
        }

        [TestMethod]
        public void TestTryParseIpAddressNetmaskAne11()
        {
            IpNetwork ipnet;
            var parsed = IpNetwork.TryParse("0.0.0.0", 33, out ipnet);

            Assert.AreEqual(false, parsed, "parsed");
            Assert.AreEqual(null, ipnet, "ipnet");
        }


        [TestMethod]
        public void TestTryParseIpAddressNetmask()
        {
            IpNetwork ipnetwork;
            const string ipaddress = "192.168.168.100";
            const string netmask = "255.255.255.0";

            const string network = "192.168.168.0";
            const string broadcast = "192.168.168.255";
            const string firstUsable = "192.168.168.1";
            const string lastUsable = "192.168.168.254";
            const byte cidr = 24;
            const uint usable = 254;

            var parsed = IpNetwork.TryParse(ipaddress, netmask, out ipnetwork);
            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(network, ipnetwork.Network.ToString(), "Network");
            Assert.AreEqual(netmask, ipnetwork.Netmask.ToString(), "Netmask");
            Assert.AreEqual(broadcast, ipnetwork.Broadcast.ToString(), "Broadcast");
            Assert.AreEqual(cidr, ipnetwork.Cidr, "Cidr");
            Assert.AreEqual(usable, ipnetwork.Usable, "Usable");
            Assert.AreEqual(firstUsable, ipnetwork.FirstUsable.ToString(), "FirstUsable");
            Assert.AreEqual(lastUsable, ipnetwork.LastUsable.ToString(), "LastUsable");
        }


        [TestMethod]
        public void TestTryParseString1()
        {
            IpNetwork ipnetwork = null;
            const string ipaddress = "192.168.168.100 255.255.255.0";

            const string network = "192.168.168.0";
            const string netmask = "255.255.255.0";
            const string broadcast = "192.168.168.255";
            const string firstUsable = "192.168.168.1";
            const string lastUsable = "192.168.168.254";
            const byte cidr = 24;
            const uint usable = 254;

            var parsed = IpNetwork.TryParse(ipaddress, out ipnetwork);
            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(network, ipnetwork.Network.ToString(), "Network");
            Assert.AreEqual(netmask, ipnetwork.Netmask.ToString(), "Netmask");
            Assert.AreEqual(broadcast, ipnetwork.Broadcast.ToString(), "Broadcast");
            Assert.AreEqual(cidr, ipnetwork.Cidr, "Cidr");
            Assert.AreEqual(usable, ipnetwork.Usable, "Usable");
            Assert.AreEqual(firstUsable, ipnetwork.FirstUsable.ToString(), "FirstUsable");
            Assert.AreEqual(lastUsable, ipnetwork.LastUsable.ToString(), "LastUsable");
        }


        [TestMethod]
        public void TestTryParseString2()
        {
            IpNetwork ipnetwork;
            const string ipaddress = "192.168.168.100/24";

            const string network = "192.168.168.0";
            const string netmask = "255.255.255.0";
            const string broadcast = "192.168.168.255";
            const string firstUsable = "192.168.168.1";
            const string lastUsable = "192.168.168.254";
            const byte cidr = 24;
            const uint usable = 254;

            var parsed = IpNetwork.TryParse(ipaddress, out ipnetwork);
            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(network, ipnetwork.Network.ToString(), "Network");
            Assert.AreEqual(netmask, ipnetwork.Netmask.ToString(), "Netmask");
            Assert.AreEqual(broadcast, ipnetwork.Broadcast.ToString(), "Broadcast");
            Assert.AreEqual(cidr, ipnetwork.Cidr, "Cidr");
            Assert.AreEqual(usable, ipnetwork.Usable, "Usable");
            Assert.AreEqual(firstUsable, ipnetwork.FirstUsable.ToString(), "FirstUsable");
            Assert.AreEqual(lastUsable, ipnetwork.LastUsable.ToString(), "LastUsable");
        }


        [TestMethod]
        public void TestTryParseString3()
        {
            IpNetwork ipnetwork;
            const string ipaddress = "0.0.0.0/0";

            const string network = "0.0.0.0";
            const string netmask = "0.0.0.0";
            const string broadcast = "255.255.255.255";
            const string firstUsable = "0.0.0.1";
            const string lastUsable = "255.255.255.254";
            const byte cidr = 0;
            const uint usable = 4294967294;

            var parsed = IpNetwork.TryParse(ipaddress, out ipnetwork);
            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(network, ipnetwork.Network.ToString(), "Network");
            Assert.AreEqual(netmask, ipnetwork.Netmask.ToString(), "Netmask");
            Assert.AreEqual(broadcast, ipnetwork.Broadcast.ToString(), "Broadcast");
            Assert.AreEqual(cidr, ipnetwork.Cidr, "Cidr");
            Assert.AreEqual(usable, ipnetwork.Usable, "Usable");
            Assert.AreEqual(firstUsable, ipnetwork.FirstUsable.ToString(), "FirstUsable");
            Assert.AreEqual(lastUsable, ipnetwork.LastUsable.ToString(), "LastUsable");
        }


        [TestMethod]
        public void TestTryParseString4()
        {
            IpNetwork ipnetwork;
            const string ipaddress = "0.0.0.0/32";

            const string network = "0.0.0.0";
            const string netmask = "255.255.255.255";
            const string broadcast = "0.0.0.0";
            const string firstUsable = "0.0.0.0";
            const string lastUsable = "0.0.0.0";
            const byte cidr = 32;
            const uint usable = 0;

            var parsed = IpNetwork.TryParse(ipaddress, out ipnetwork);
            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(network, ipnetwork.Network.ToString(), "Network");
            Assert.AreEqual(netmask, ipnetwork.Netmask.ToString(), "Netmask");
            Assert.AreEqual(broadcast, ipnetwork.Broadcast.ToString(), "Broadcast");
            Assert.AreEqual(cidr, ipnetwork.Cidr, "Cidr");
            Assert.AreEqual(usable, ipnetwork.Usable, "Usable");
            Assert.AreEqual(firstUsable, ipnetwork.FirstUsable.ToString(), "FirstUsable");
            Assert.AreEqual(lastUsable, ipnetwork.LastUsable.ToString(), "LastUsable");
        }


        [TestMethod]
        public void TestTryParseString5()
        {
            IpNetwork ipnetwork;
            const string ipaddress = "255.255.255.255/32";

            const string network = "255.255.255.255";
            const string netmask = "255.255.255.255";
            const string broadcast = "255.255.255.255";
            const string firstUsable = "255.255.255.255";
            const string lastUsable = "255.255.255.255";
            const byte cidr = 32;
            const uint usable = 0;

            var parsed = IpNetwork.TryParse(ipaddress, out ipnetwork);
            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(network, ipnetwork.Network.ToString(), "Network");
            Assert.AreEqual(netmask, ipnetwork.Netmask.ToString(), "Netmask");
            Assert.AreEqual(broadcast, ipnetwork.Broadcast.ToString(), "Broadcast");
            Assert.AreEqual(cidr, ipnetwork.Cidr, "Cidr");
            Assert.AreEqual(usable, ipnetwork.Usable, "Usable");
            Assert.AreEqual(firstUsable, ipnetwork.FirstUsable.ToString(), "FirstUsable");
            Assert.AreEqual(lastUsable, ipnetwork.LastUsable.ToString(), "LastUsable");
        }


        [TestMethod]
        public void TestTryParseStringAe1()
        {
            const string ipaddress = "garbage";
            IpNetwork ipnetwork;
            var parsed = IpNetwork.TryParse(ipaddress, out ipnetwork);
            Assert.AreEqual(false, parsed, "parsed");
        }

        [TestMethod]
        public void TestTryParseStringAe2()
        {
            const string ipaddress = "0.0.0.0 0.0.1.0";
            IpNetwork ipnetwork;
            var parsed = IpNetwork.TryParse(ipaddress, out ipnetwork);
            Assert.AreEqual(false, parsed, "parsed");
        }

        [TestMethod]
        public void TestTryParseStringAne1()
        {
            string ipaddress = null;
            IpNetwork ipnetwork;
            var parsed = IpNetwork.TryParse(ipaddress, out ipnetwork);
            Assert.AreEqual(false, parsed, "parsed");
        }

        #endregion

        #region ParseStringString

        [TestMethod]
        public void TestParseStringString1()
        {
            const string ipaddress = "192.168.168.100";
            const string netmask = "255.255.255.0";

            var ipnetwork = IpNetwork.Parse(ipaddress, netmask);
            Assert.AreEqual("192.168.168.0/24", ipnetwork.ToString(), "network");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestParseStringString2()
        {
            string ipaddress = null;
            string netmask = null;

            var ipnetwork = IpNetwork.Parse(ipaddress, netmask);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestParseStringString3()
        {
            const string ipaddress = "192.168.168.100";
            string netmask = null;

            var ipnetwork = IpNetwork.Parse(ipaddress, netmask);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestParseStringString4()
        {
            const string ipaddress = "";
            const string netmask = "";

            var ipnetwork = IpNetwork.Parse(ipaddress, netmask);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestParseStringString5()
        {
            var ipaddress = "192.168.168.100";
            var netmask = "";

            var ipnetwork = IpNetwork.Parse(ipaddress, netmask);
        }

        #endregion

        #region ParseIpIp

        [TestMethod]
        public void ParseIpIp1()
        {
            const string ipaddress = "192.168.168.100";
            const string netmask = "255.255.255.0";
            var ip = IPAddress.Parse(ipaddress);
            var netm = IPAddress.Parse(netmask);
            var ipnetwork = IpNetwork.Parse(ip, netm);
            Assert.AreEqual("192.168.168.0/24", ipnetwork.ToString(), "network");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ParseIpIp2()
        {
            IPAddress ip = null;
            IPAddress netm = null;
            var ipnetwork = IpNetwork.Parse(ip, netm);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ParseIpIp3()
        {
            const string ipaddress = "192.168.168.100";
            var ip = IPAddress.Parse(ipaddress);
            IPAddress netm = null;
            var ipnetwork = IpNetwork.Parse(ip, netm);
        }

        #endregion

        #region ToCidr

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestToCidrAne()
        {
            IpNetwork.ToCidr(null);
        }

        [TestMethod]
        public void TestToCidrAe()
        {
            var cidr = IpNetwork.ToCidr(IPAddress.IPv6Any);
            Assert.AreEqual(0, cidr, "cidr");
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestToCidrAe2()
        {
            IpNetwork.ToCidr(IPAddress.Parse("6.6.6.6"));
        }


        [TestMethod]
        public void TestToCidr32()
        {
            var mask = IPAddress.Parse("255.255.255.255");
            const byte cidr = 32;
            int result = IpNetwork.ToCidr(mask);

            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestToCidr24()
        {
            var mask = IPAddress.Parse("255.255.255.0");
            const byte cidr = 24;
            int result = IpNetwork.ToCidr(mask);

            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestToCidr16()
        {
            var mask = IPAddress.Parse("255.255.0.0");
            const byte cidr = 16;
            int result = IpNetwork.ToCidr(mask);

            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestToCidr8()
        {
            var mask = IPAddress.Parse("255.0.0.0");
            const byte cidr = 8;
            int result = IpNetwork.ToCidr(mask);

            Assert.AreEqual(cidr, result, "cidr");
        }

        [TestMethod]
        public void TestToCidr0()
        {
            var mask = IPAddress.Parse("0.0.0.0");
            const byte cidr = 0;
            int result = IpNetwork.ToCidr(mask);

            Assert.AreEqual(cidr, result, "cidr");
        }

        #endregion

        #region TryToCidr

        [TestMethod]


        public void TestTryToCidrAne()
        {
            byte? cidr;
            var parsed = IpNetwork.TryToCidr(null, out cidr);
            Assert.AreEqual(false, parsed, "parsed");
        }

        [TestMethod]
        public void TestTryToCidrAe()
        {
            byte? cidr;
            var parsed = IpNetwork.TryToCidr(IPAddress.IPv6Any, out cidr);
            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual((byte)0, cidr, "cidr");
        }


        [TestMethod]
        public void TestTryToCidrAe2()
        {
            byte? cidr;
            var parsed = IpNetwork.TryToCidr(IPAddress.Parse("6.6.6.6"), out cidr);
            Assert.AreEqual(false, parsed, "parsed");
        }

        public void TestTryToCidr32Through0()
        {
            var mask = new List<string> { "255.255.255.255", "255.255.255.0", "255.255.0.0", "255.0.0.0", "0.0.0.0" };
            var expectedresult = new List<byte> { 32, 24, 16, 8, 0 };

            foreach (var parsedmask in mask.Select(IPAddress.Parse))
            {
                byte? cidr;
                var parsed = IpNetwork.TryToCidr(parsedmask, out cidr);
                Assert.AreEqual(true, parsed, "parsed");
                Assert.AreEqual(cidr, expectedresult, "cidr");
            }
        }

        #endregion

        #region ToBigInteger

        [TestMethod]
        public void TestToBigInteger32()
        {
            var mask = IPAddress.Parse("255.255.255.255");
            const uint uintMask = 0xffffffff;
            var result = IpNetwork.ToBigInteger(mask);

            Assert.AreEqual(uintMask, result, "uint");
        }

        [TestMethod]
        public void TestToBigInteger24()
        {
            var mask = IPAddress.Parse("255.255.255.0");
            const uint uintMask = 0xffffff00;
            BigInteger? result = IpNetwork.ToBigInteger(mask);

            Assert.AreEqual(uintMask, result, "uint");
        }

        [TestMethod]
        public void TestToBigInteger16()
        {
            var mask = IPAddress.Parse("255.255.0.0");
            const uint uintMask = 0xffff0000;
            BigInteger? result = IpNetwork.ToBigInteger(mask);

            Assert.AreEqual(uintMask, result, "uint");
        }

        [TestMethod]
        public void TestToBigInteger8()
        {
            var mask = IPAddress.Parse("255.0.0.0");
            const uint uintMask = 0xff000000;
            BigInteger? result = IpNetwork.ToBigInteger(mask);

            Assert.AreEqual(uintMask, result, "uint");
        }

        [TestMethod]
        public void TestToBigInteger0()
        {
            var mask = IPAddress.Parse("0.0.0.0");
            const uint uintMask = 0x00000000;
            BigInteger? result = IpNetwork.ToBigInteger(mask);

            Assert.AreEqual(uintMask, result, "uint");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestToBigIntegerAne()
        {
            BigInteger? result = IpNetwork.ToBigInteger(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestToBigIntegerAne3()
        {
            IPAddress ip = null;
            BigInteger? result = IpNetwork.ToBigInteger(ip);
        }

        [TestMethod]
        public void TestToBigIntegerAne2()
        {
            BigInteger? result = IpNetwork.ToBigInteger(IPAddress.IPv6Any);
            uint expected = 0;
            Assert.AreEqual(expected, result, "result");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestToBigIntegerByte()
        {
            var result = IpNetwork.ToUint(33, AddressFamily.InterNetwork);
        }

        [TestMethod]
        public void TestToBigIntegerByte2()
        {
            var result = IpNetwork.ToUint(32, AddressFamily.InterNetwork);
            const uint expected = 4294967295;
            Assert.AreEqual(expected, result, "result");
        }


        [TestMethod]
        public void TestToBigIntegerByte3()
        {
            var result = IpNetwork.ToUint(0, AddressFamily.InterNetwork);
            const uint expected = 0;
            Assert.AreEqual(expected, result, "result");
        }

        #endregion

        #region TryToBigInteger

        [TestMethod]
        public void TestTryToBigInteger32()
        {
            var mask = IPAddress.Parse("255.255.255.255");
            const uint uintMask = 0xffffffff;
            BigInteger? result;
            var parsed = IpNetwork.TryToBigInteger(mask, out result);

            Assert.AreEqual(uintMask, result, "uint");
            Assert.AreEqual(true, parsed, "parsed");
        }

        [TestMethod]
        public void TestTryToBigInteger24()
        {
            var mask = IPAddress.Parse("255.255.255.0");
            const uint uintMask = 0xffffff00;
            BigInteger? result;
            var parsed = IpNetwork.TryToBigInteger(mask, out result);

            Assert.AreEqual(uintMask, result, "uint");
            Assert.AreEqual(true, parsed, "parsed");
        }

        [TestMethod]
        public void TestTryToBigInteger16()
        {
            var mask = IPAddress.Parse("255.255.0.0");
            const uint uintMask = 0xffff0000;
            BigInteger? result;
            var parsed = IpNetwork.TryToBigInteger(mask, out result);

            Assert.AreEqual(uintMask, result, "uint");
            Assert.AreEqual(true, parsed, "parsed");
        }

        [TestMethod]
        public void TestTryToBigInteger8()
        {
            var mask = IPAddress.Parse("255.0.0.0");
            const uint uintMask = 0xff000000;

            BigInteger? result;
            var parsed = IpNetwork.TryToBigInteger(mask, out result);

            Assert.AreEqual(uintMask, result, "uint");
            Assert.AreEqual(true, parsed, "parsed");
        }

        [TestMethod]
        public void TestTryToBigInteger0()
        {
            var mask = IPAddress.Parse("0.0.0.0");
            const uint uintMask = 0x00000000;
            BigInteger? result;
            var parsed = IpNetwork.TryToBigInteger(mask, out result);

            Assert.AreEqual(uintMask, result, "uint");
            Assert.AreEqual(true, parsed, "parsed");
        }

        [TestMethod]
        public void TestTryToBigIntegerAne()
        {
            BigInteger? result;
            var parsed = IpNetwork.TryToBigInteger(null, out result);

            Assert.AreEqual(null, result, "uint");
            Assert.AreEqual(false, parsed, "parsed");
        }

        [TestMethod]
        public void TestTryToBigIntegerAne3()
        {
            IPAddress ip = null;
            BigInteger? result;
            var parsed = IpNetwork.TryToBigInteger(ip, out result);

            Assert.AreEqual(null, result, "uint");
            Assert.AreEqual(false, parsed, "parsed");
        }

        [TestMethod]
        public void TestTryToBigIntegerAne2()
        {
            BigInteger? result;
            var parsed = IpNetwork.TryToBigInteger(IPAddress.IPv6Any, out result);

            Assert.AreEqual(0, result, "result");
            Assert.AreEqual(true, parsed, "parsed");
        }

        #endregion

        #region TryToNetmask

        [TestMethod]
        public void TryToNetmask1()
        {
            IPAddress result;
            var parsed = IpNetwork.TryToNetmask(0, AddressFamily.InterNetwork, out result);
            var expected = IPAddress.Parse("0.0.0.0");

            Assert.AreEqual(expected, result, "Netmask");
            Assert.AreEqual(true, parsed, "parsed");
        }

        [TestMethod]
        public void TryToNetmask2()
        {
            IPAddress result;
            var parsed = IpNetwork.TryToNetmask(33, AddressFamily.InterNetwork, out result);
            IPAddress expected = null;

            Assert.AreEqual(expected, result, "Netmask");
            Assert.AreEqual(false, parsed, "parsed");
        }

        #endregion

        #region ToNetmask

        [TestMethod]
        public void ToNetmask32()
        {
            const byte cidr = 32;
            const string netmask = "255.255.255.255";
            var result = IpNetwork.ToNetmask(cidr, AddressFamily.InterNetwork).ToString();

            Assert.AreEqual(netmask, result, "netmask");
        }

        [TestMethod]
        public void ToNetmask31()
        {
            const byte cidr = 31;
            const string netmask = "255.255.255.254";
            var result = IpNetwork.ToNetmask(cidr, AddressFamily.InterNetwork).ToString();

            Assert.AreEqual(netmask, result, "netmask");
        }

        [TestMethod]
        public void ToNetmask30()
        {
            const byte cidr = 30;
            const string netmask = "255.255.255.252";
            var result = IpNetwork.ToNetmask(cidr, AddressFamily.InterNetwork).ToString();

            Assert.AreEqual(netmask, result, "netmask");
        }

        [TestMethod]
        public void ToNetmask29()
        {
            const byte cidr = 29;
            const string netmask = "255.255.255.248";
            var result = IpNetwork.ToNetmask(cidr, AddressFamily.InterNetwork).ToString();

            Assert.AreEqual(netmask, result, "netmask");
        }

        [TestMethod]
        public void ToNetmask1()
        {
            const byte cidr = 1;
            const string netmask = "128.0.0.0";
            var result = IpNetwork.ToNetmask(cidr, AddressFamily.InterNetwork).ToString();

            Assert.AreEqual(netmask, result, "netmask");
        }


        [TestMethod]
        public void ToNetmask0()
        {
            const byte cidr = 0;
            const string netmask = "0.0.0.0";
            var result = IpNetwork.ToNetmask(cidr, AddressFamily.InterNetwork).ToString();

            Assert.AreEqual(netmask, result, "netmask");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ToNetmaskOore1()
        {
            const byte cidr = 33;
            var result = IpNetwork.ToNetmask(cidr, AddressFamily.InterNetwork).ToString();
        }

        #endregion

        #region ValidNetmask

        [TestMethod]
        public void TestValidNetmask0()
        {
            var mask = IPAddress.Parse("255.255.255.255");
            const bool expected = true;
            var result = IpNetwork.ValidNetmask(mask);

            Assert.AreEqual(expected, result, "ValidNetmask");
        }

        [TestMethod]
        public void TestValidNetmask1()
        {
            var mask = IPAddress.Parse("255.255.255.0");
            const bool expected = true;
            var result = IpNetwork.ValidNetmask(mask);

            Assert.AreEqual(expected, result, "ValidNetmask");
        }

        [TestMethod]
        public void TestValidNetmask2()
        {
            var mask = IPAddress.Parse("255.255.0.0");
            const bool expected = true;
            var result = IpNetwork.ValidNetmask(mask);

            Assert.AreEqual(expected, result, "ValidNetmask");
        }


        [TestMethod]
        public void TestValidNetmaskEae1()
        {
            var mask = IPAddress.Parse("0.255.0.0");
            const bool expected = false;
            var result = IpNetwork.ValidNetmask(mask);

            Assert.AreEqual(expected, result, "ValidNetmask");
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestValidNetmaskEae2()
        {
            IPAddress mask = null;
            const bool expected = true;
            var result = IpNetwork.ValidNetmask(mask);

            Assert.AreEqual(expected, result, "ValidNetmask");
        }


        [TestMethod]
        public void TestValidNetmaskEae3()
        {
            var mask = IPAddress.Parse("255.255.0.1");
            const bool expected = false;
            var result = IpNetwork.ValidNetmask(mask);

            Assert.AreEqual(expected, result, "ValidNetmask");
        }

        #endregion

        #region BitsSet

        [TestMethod]
        public void TestBitsSet32()
        {
            var ip = IPAddress.Parse("255.255.255.255");
            const uint bits = 32;
            var result = IpNetwork.BitsSet(ip);

            Assert.AreEqual(bits, result, "BitsSet");
        }

        [TestMethod]
        public void TestBitsSet24()
        {
            var ip = IPAddress.Parse("255.255.255.0");
            const uint bits = 24;
            var result = IpNetwork.BitsSet(ip);

            Assert.AreEqual(bits, result, "BitsSet");
        }

        [TestMethod]
        public void TestBitsSet16()
        {
            var ip = IPAddress.Parse("255.255.0.0");
            const uint bits = 16;
            var result = IpNetwork.BitsSet(ip);

            Assert.AreEqual(bits, result, "BitsSet");
        }

        [TestMethod]
        public void TestBitsSet4()
        {
            var ip = IPAddress.Parse("128.128.128.128");
            const uint bits = 4;
            var result = IpNetwork.BitsSet(ip);

            Assert.AreEqual(bits, result, "BitsSet");
        }

        #endregion

        #region Contains

        [TestMethod]
        public void TestContains1()
        {
            var ipnetwork = IpNetwork.Parse("192.168.0.1/24");
            var ipaddress = IPAddress.Parse("192.168.0.100");

            var result = IpNetwork.Contains(ipnetwork, ipaddress);
            const bool expected = true;

            Assert.AreEqual(expected, result, "contains");
        }

        [TestMethod]
        public void TestContains2()
        {
            var ipnetwork = IpNetwork.Parse("192.168.0.1/24");
            var ipaddress = IPAddress.Parse("10.10.10.10");

            var result = IpNetwork.Contains(ipnetwork, ipaddress);
            const bool expected = false;

            Assert.AreEqual(expected, result, "contains");
        }

        [TestMethod]
        public void TestContains3()
        {
            var ipnetwork = IpNetwork.Parse("192.168.0.1/24");
            var ipnetwork2 = IpNetwork.Parse("192.168.0.1/24");

            var result = IpNetwork.Contains(ipnetwork, ipnetwork2);
            const bool expected = true;

            Assert.AreEqual(expected, result, "contains");
        }

        [TestMethod]
        public void TestContains4()
        {
            var ipnetwork = IpNetwork.Parse("192.168.0.1/16");
            var ipnetwork2 = IpNetwork.Parse("192.168.1.1/24");

            var result = IpNetwork.Contains(ipnetwork, ipnetwork2);
            const bool expected = true;

            Assert.AreEqual(expected, result, "contains");
        }

        [TestMethod]
        public void TestContains5()
        {
            var ipnetwork = IpNetwork.Parse("192.168.0.1/16");
            var ipnetwork2 = IpNetwork.Parse("10.10.10.0/24");

            var result = IpNetwork.Contains(ipnetwork, ipnetwork2);
            const bool expected = false;

            Assert.AreEqual(expected, result, "contains");
        }


        [TestMethod]
        public void TestContains6()
        {
            var ipnetwork = IpNetwork.Parse("192.168.1.1/24");
            var ipnetwork2 = IpNetwork.Parse("192.168.0.0/16");

            var result = IpNetwork.Contains(ipnetwork, ipnetwork2);
            const bool expected = false;

            Assert.AreEqual(expected, result, "contains");
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestContains7()
        {
            IpNetwork ipnetwork = null;
            IpNetwork ipnetwork2 = null;

            var result = IpNetwork.Contains(ipnetwork, ipnetwork2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestContains8()
        {
            var ipnetwork = IpNetwork.Parse("0.0.0.0/0");
            IpNetwork ipnetwork2 = null;

            var result = IpNetwork.Contains(ipnetwork, ipnetwork2);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestContains9()
        {
            IpNetwork ipnetwork = null;
            IPAddress ipaddress = null;

            var result = IpNetwork.Contains(ipnetwork, ipaddress);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestContains10()
        {
            var ipnetwork = IpNetwork.Parse("0.0.0.0/0");
            IPAddress ipaddress = null;

            var result = IpNetwork.Contains(ipnetwork, ipaddress);
        }

        #endregion

        #region Overlap

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestOverlap1()
        {
            IpNetwork network1 = null;
            IpNetwork network2 = null;
            IpNetwork.Overlap(network1, network2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestOverlap2()
        {
            var network1 = IpNetwork.Parse("10.0.0.0/0");
            IpNetwork network2 = null;
            IpNetwork.Overlap(network1, network2);
        }

        [TestMethod]
        public void TestOverlap3()
        {
            var network1 = IpNetwork.Parse("10.0.0.0/0");
            var network2 = IpNetwork.Parse("10.0.0.0/0");
            var result = IpNetwork.Overlap(network1, network2);
            const bool expected = true;

            Assert.AreEqual(expected, result, "overlap");
        }

        [TestMethod]
        public void TestOverlap4()
        {
            var network1 = IpNetwork.Parse("10.10.0.0/16");
            var network2 = IpNetwork.Parse("10.10.1.0/24");
            var result = IpNetwork.Overlap(network1, network2);
            const bool expected = true;

            Assert.AreEqual(expected, result, "overlap");
        }

        [TestMethod]
        public void TestOverlap5()
        {
            var network1 = IpNetwork.Parse("10.10.0.0/24");
            var network2 = IpNetwork.Parse("10.10.1.0/24");
            var result = IpNetwork.Overlap(network1, network2);
            const bool expected = false;

            Assert.AreEqual(expected, result, "overlap");
        }

        [TestMethod]
        public void TestOverlap6()
        {
            var network1 = IpNetwork.Parse("10.10.1.0/24");
            var network2 = IpNetwork.Parse("10.10.0.0/16");
            var result = IpNetwork.Overlap(network1, network2);
            const bool expected = true;

            Assert.AreEqual(expected, result, "overlap");
        }

        #endregion

        #region Examples

        public void Example1()
        {
            var ipnetwork = IpNetwork.Parse("192.168.168.100/24");

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
            var ipnetwork = IpNetwork.Parse("192.168.0.0/24");
            var ipaddress = IPAddress.Parse("192.168.0.100");
            var ipaddress2 = IPAddress.Parse("192.168.1.100");

            var ipnetwork2 = IpNetwork.Parse("192.168.0.128/25");
            var ipnetwork3 = IpNetwork.Parse("192.168.1.1/24");

            var contains1 = IpNetwork.Contains(ipnetwork, ipaddress);
            var contains2 = IpNetwork.Contains(ipnetwork, ipaddress2);
            var contains3 = IpNetwork.Contains(ipnetwork, ipnetwork2);
            var contains4 = IpNetwork.Contains(ipnetwork, ipnetwork3);

            var overlap1 = IpNetwork.Overlap(ipnetwork, ipnetwork2);
            var overlap2 = IpNetwork.Overlap(ipnetwork, ipnetwork3);

            Console.WriteLine($"{ipnetwork} contains {ipaddress} : {contains1}");
            Console.WriteLine($"{ipnetwork} contains {ipaddress2} : {contains2}");
            Console.WriteLine($"{ipnetwork} contains {ipnetwork2} : {contains3}");
            Console.WriteLine($"{ipnetwork} contains {ipnetwork3} : {contains4}");


            Console.WriteLine($"{ipnetwork} overlap {ipnetwork2} : {overlap1}");
            Console.WriteLine($"{ipnetwork} overlap {ipnetwork3} : {overlap2}");
        }

        public void Example2B()
        {
            var ipnetwork1 = IpNetwork.Parse("10.1.0.0/16");
            var ipnetwork2 = IpNetwork.Parse("192.168.1.0/24");

            var ipaddress1 = IPAddress.Parse("192.168.1.1");
            var ipaddress2 = IPAddress.Parse("192.168.2.100");
            var ipaddress3 = IPAddress.Parse("10.1.2.3");
            var ipaddress4 = IPAddress.Parse("10.4.5.6");


            var contains1 = IpNetwork.Contains(ipnetwork2, ipaddress1);
            var contains2 = IpNetwork.Contains(ipnetwork2, ipaddress2);
            var contains3 = IpNetwork.Contains(ipnetwork1, ipaddress3);
            var contains4 = IpNetwork.Contains(ipnetwork1, ipaddress4);


            Console.WriteLine("{0} contains {1} : {2}", ipnetwork1, ipaddress1, contains1);
            Console.WriteLine("{0} contains {1} : {2}", ipnetwork1, ipaddress2, contains2);
            Console.WriteLine("{0} contains {1} : {2}", ipnetwork2, ipaddress3, contains3);
            Console.WriteLine("{0} contains {1} : {2}", ipnetwork2, ipaddress4, contains4);
        }

        public void Example3()
        {
            var ianaABlock = IpNetwork.IanaAblkReserved1;
            var ianaBBlock = IpNetwork.IanaBblkReserved1;
            var ianaCBlock = IpNetwork.IanaCblkReserved1;

            Console.WriteLine("IANA_ABLK_RESERVED1 is {0}", ianaABlock);
            Console.WriteLine("IANA_BBLK_RESERVED1 is {0}", ianaBBlock);
            Console.WriteLine("IANA_CBLK_RESERVED1 is {0}", ianaCBlock);
        }


        public void Example4()
        {
            var wholeInternet = IpNetwork.Parse("0.0.0.0/0");
            const byte newCidr = 2;
            var subneted = IpNetwork.Subnet(wholeInternet, newCidr);

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
            var ipnetwork1 = IpNetwork.Parse("192.168.0.0/24");
            var ipnetwork2 = IpNetwork.Parse("192.168.1.0/24");
            var ipnetwork3 = IpNetwork.Supernet(new[] { ipnetwork1, ipnetwork2 });

            Console.WriteLine($"{ipnetwork1} + {ipnetwork2} = {ipnetwork3[0]}");
        }


        public void Example7()
        {
            var ipnetwork = IpNetwork.Parse("192.168.168.100/24");

            var ipaddress = IPAddress.Parse("192.168.168.200");
            var ipaddress2 = IPAddress.Parse("192.168.0.200");

            var contains1 = IpNetwork.Contains(ipnetwork, ipaddress);
            var contains2 = IpNetwork.Contains(ipnetwork, ipaddress2);

            Console.WriteLine("{0} contains {1} : {2}", ipnetwork, ipaddress, contains1);
            Console.WriteLine("{0} contains {1} : {2}", ipnetwork, ipaddress2, contains2);
        }

        public void Example9()
        {
            var network = IpNetwork.Parse("192.168.0.1");
            var network2 = IpNetwork.Parse("192.168.0.254");

            var ipnetwork = IpNetwork.Supernet(network, network2);

            Console.WriteLine("Network : {0}", ipnetwork.Network);
            Console.WriteLine("Netmask : {0}", ipnetwork.Netmask);
            Console.WriteLine("Broadcast : {0}", ipnetwork.Broadcast);
            Console.WriteLine("FirstUsable : {0}", ipnetwork.FirstUsable);
            Console.WriteLine("LastUsable : {0}", ipnetwork.LastUsable);
            Console.WriteLine("Usable : {0}", ipnetwork.Usable);
            Console.WriteLine("Cidr : {0}", ipnetwork.Cidr);
        }


        public void Example10()
        {
            var ipnetwork = IpNetwork.Parse("192.168.0.1/25");

            Console.WriteLine("Network : {0}", ipnetwork.Network);
            Console.WriteLine("Netmask : {0}", ipnetwork.Netmask);
            Console.WriteLine("Broadcast : {0}", ipnetwork.Broadcast);
            Console.WriteLine("FirstUsable : {0}", ipnetwork.FirstUsable);
            Console.WriteLine("LastUsable : {0}", ipnetwork.LastUsable);
            Console.WriteLine("Usable : {0}", ipnetwork.Usable);
            Console.WriteLine("Cidr : {0}", ipnetwork.Cidr);
        }

        #endregion

        #region IANA blocks

        [TestMethod]
        public void TestIana1()
        {
            var ipaddress = IPAddress.Parse("192.168.66.66");
            var expected = true;
            var result = IpNetwork.IsIANAReserved(ipaddress);

            Assert.AreEqual(expected, result, "IANA");
        }

        [TestMethod]
        public void TestIana2()
        {
            var ipaddress = IPAddress.Parse("10.0.0.0");
            var expected = true;
            var result = IpNetwork.IsIANAReserved(ipaddress);

            Assert.AreEqual(expected, result, "IANA");
        }

        [TestMethod]
        public void TestIana3()
        {
            var ipaddress = IPAddress.Parse("172.17.10.10");
            var expected = true;
            var result = IpNetwork.IsIANAReserved(ipaddress);

            Assert.AreEqual(expected, result, "IANA");
        }

        [TestMethod]
        public void TestIana4()
        {
            var ipnetwork = IpNetwork.Parse("192.168.66.66/24");
            var expected = true;
            var result = IpNetwork.IsIANAReserved(ipnetwork);

            Assert.AreEqual(expected, result, "IANA");
        }

        [TestMethod]
        public void TestIana5()
        {
            var ipnetwork = IpNetwork.Parse("10.10.10/18");
            var expected = true;
            var result = IpNetwork.IsIANAReserved(ipnetwork);

            Assert.AreEqual(expected, result, "IANA");
        }

        [TestMethod]
        public void TestIana6()
        {
            var ipnetwork = IpNetwork.Parse("172.31.10.10/24");
            var expected = true;
            var result = IpNetwork.IsIANAReserved(ipnetwork);

            Assert.AreEqual(expected, result, "IANA");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestIana7()
        {
            IPAddress ipaddress = null;
            IpNetwork.IsIANAReserved(ipaddress);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestIana8()
        {
            IpNetwork ipnetwork = null;
            IpNetwork.IsIANAReserved(ipnetwork);
        }


        [TestMethod]
        public void TestIana9()
        {
            var ipaddress = IPAddress.Parse("1.2.3.4");
            var expected = false;
            var result = IpNetwork.IsIANAReserved(ipaddress);

            Assert.AreEqual(expected, result, "IANA");
        }

        [TestMethod]
        public void TestIana10()
        {
            var ipnetwork = IpNetwork.Parse("172.16.0.0/8");
            var expected = false;
            var result = IpNetwork.IsIANAReserved(ipnetwork);

            Assert.AreEqual(expected, result, "IANA");
        }


        [TestMethod]
        public void TestIana11()
        {
            var ipnetwork = IpNetwork.Parse("192.168.15.1/8");
            var expected = false;
            var result = IpNetwork.IsIANAReserved(ipnetwork);

            Assert.AreEqual(expected, result, "IANA");
        }

        #endregion

        #region ToString

        [TestMethod]
        public void TestToString()
        {
            var ipnetwork = IpNetwork.Parse("192.168.15.1/8");
            var expected = "192.0.0.0/8";
            var result = ipnetwork.ToString();

            Assert.AreEqual(expected, result, "ToString");
        }

        [TestMethod]
        public void TestToString1()
        {
            var ipnetwork = IpNetwork.Parse("192.168.15.1/9");
            var expected = "192.128.0.0/9";
            var result = ipnetwork.ToString();

            Assert.AreEqual(expected, result, "ToString");
        }

        [TestMethod]
        public void TestToString2()
        {
            var ipnetwork = IpNetwork.Parse("192.168.15.1/10");
            var expected = "192.128.0.0/10";
            var result = ipnetwork.ToString();

            Assert.AreEqual(expected, result, "ToString");
        }

        [TestMethod]
        public void TestToString3()
        {
            var ipnetwork = IpNetwork.Parse("192.168.15.1/11");
            var expected = "192.160.0.0/11";
            var result = ipnetwork.ToString();

            Assert.AreEqual(expected, result, "ToString");
        }

        [TestMethod]
        public void TestToString4()
        {
            var ipnetwork = IpNetwork.Parse("192.168.15.1/12");
            var expected = "192.160.0.0/12";
            var result = ipnetwork.ToString();

            Assert.AreEqual(expected, result, "ToString");
        }

        [TestMethod]
        public void TestToString5()
        {
            var ipnetwork = IpNetwork.Parse("192.168.15.1/13");
            var expected = "192.168.0.0/13";
            var result = ipnetwork.ToString();

            Assert.AreEqual(expected, result, "ToString");
        }

        [TestMethod]
        public void TestToString6()
        {
            var ipnetwork = IpNetwork.Parse("192.168.15.1/14");
            var expected = "192.168.0.0/14";
            var result = ipnetwork.ToString();

            Assert.AreEqual(expected, result, "ToString");
        }

        [TestMethod]
        public void TestToString7()
        {
            var ipnetwork = IpNetwork.Parse("192.168.15.1/15");
            var expected = "192.168.0.0/15";
            var result = ipnetwork.ToString();

            Assert.AreEqual(expected, result, "ToString");
        }

        [TestMethod]
        public void TestToString8()
        {
            var ipnetwork = IpNetwork.Parse("192.168.15.1/16");
            var expected = "192.168.0.0/16";
            var result = ipnetwork.ToString();

            Assert.AreEqual(expected, result, "ToString");
        }

        #endregion

        #region Subnet

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSubnet1()
        {
            IpNetwork ipnetwork = null;
            byte cidr = 9;

            var subnets = IpNetwork.Subnet(ipnetwork, cidr);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestSubnet3()
        {
            var ipnetwork = IpNetwork.IanaAblkReserved1;
            byte cidr = 55;

            var subnets = IpNetwork.Subnet(ipnetwork, cidr);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSubnet4()
        {
            var ipnetwork = IpNetwork.IanaAblkReserved1;
            byte cidr = 1;

            var subnets = IpNetwork.Subnet(ipnetwork, cidr);
        }


        [TestMethod]
        public void TestSubnet5()
        {
            var ipnetwork = IpNetwork.IanaAblkReserved1;
            byte cidr = 9;

            var subnets = IpNetwork.Subnet(ipnetwork, cidr);
            Assert.AreEqual(2, subnets.Count, "count");
            Assert.AreEqual("10.0.0.0/9", subnets[0].ToString(), "subnet1");
            Assert.AreEqual("10.128.0.0/9", subnets[1].ToString(), "subnet2");
        }


        [TestMethod]
        public void TestSubnet6()
        {
            var ipnetwork = IpNetwork.IanaCblkReserved1;
            byte cidr = 20;

            var subnets = IpNetwork.Subnet(ipnetwork, cidr);
            Assert.AreEqual(16, subnets.Count, "count");
            Assert.AreEqual("192.168.0.0/20", subnets[0].ToString(), "subnet1");
            Assert.AreEqual("192.168.16.0/20", subnets[1].ToString(), "subnet2");
            Assert.AreEqual("192.168.32.0/20", subnets[2].ToString(), "subnet3");
            Assert.AreEqual("192.168.48.0/20", subnets[3].ToString(), "subnet4");
            Assert.AreEqual("192.168.64.0/20", subnets[4].ToString(), "subnet5");
            Assert.AreEqual("192.168.80.0/20", subnets[5].ToString(), "subnet6");
            Assert.AreEqual("192.168.96.0/20", subnets[6].ToString(), "subnet7");
            Assert.AreEqual("192.168.112.0/20", subnets[7].ToString(), "subnet8");
            Assert.AreEqual("192.168.128.0/20", subnets[8].ToString(), "subnet9");
            Assert.AreEqual("192.168.144.0/20", subnets[9].ToString(), "subnet10");
            Assert.AreEqual("192.168.160.0/20", subnets[10].ToString(), "subnet11");
            Assert.AreEqual("192.168.176.0/20", subnets[11].ToString(), "subnet12");
            Assert.AreEqual("192.168.192.0/20", subnets[12].ToString(), "subnet13");
            Assert.AreEqual("192.168.208.0/20", subnets[13].ToString(), "subnet14");
            Assert.AreEqual("192.168.224.0/20", subnets[14].ToString(), "subnet15");
            Assert.AreEqual("192.168.240.0/20", subnets[15].ToString(), "subnet16");
        }


        [TestMethod]
        public void TestSubnet7()
        {
            var ipnetwork = IpNetwork.IanaCblkReserved1;
            byte cidr = 24;

            var subnets = IpNetwork.Subnet(ipnetwork, cidr);
            Assert.AreEqual(256, subnets.Count, "count");
            Assert.AreEqual("192.168.0.0/24", subnets[0].ToString(), "subnet1");
            Assert.AreEqual("192.168.255.0/24", subnets[255].ToString(), "subnet16");
        }


        [TestMethod]
        public void TestSubnet8()
        {
            var ipnetwork = IpNetwork.IanaCblkReserved1;
            byte cidr = 24;

            var subnets = IpNetwork.Subnet(ipnetwork, cidr);
            Assert.AreEqual(256, subnets.Count, "count");
            Assert.AreEqual("192.168.0.0/24", subnets[0].ToString(), "subnet1");
            Assert.AreEqual("192.168.255.0/24", subnets[255].ToString(), "subnet256");
        }

        [TestMethod]
        public void TestSubnet9()
        {
            var ipnetwork = IpNetwork.Parse("192.168.0.0/24");
            const byte cidr = 32;

            var subnets = IpNetwork.Subnet(ipnetwork, cidr);
            var index = 0;
            foreach (var subnet in subnets)
            {
                Console.WriteLine($"IPNetwork.Subnet #{index}={subnet}");
                index++;
            }
            Assert.AreEqual(256, subnets.Count, "count");
            Assert.AreEqual("192.168.0.0/32", subnets[0].ToString(), "subnet1");
            Assert.AreEqual("192.168.0.255/32", subnets[255].ToString(), "subnet256");
        }


        [TestMethod]
        public void TestSubnet10()
        {
            var ipnetwork = IpNetwork.Parse("0.0.0.0/0");
            byte cidr = 32;

            // Here I spawm a OOM dragon ! beware of the beast !
            var subnets = IpNetwork.Subnet(ipnetwork, cidr);
            Assert.AreEqual(4294967296, subnets.Count, "count");
            Assert.AreEqual("0.0.0.0/32", subnets[0].ToString(), "subnet1");
            Assert.AreEqual("255.255.255.255/32", subnets[4294967295].ToString(), "subnet256");
        }


        [TestMethod]
        public void TestSubnet12()
        {
            var ipnetwork = IpNetwork.IanaCblkReserved1;
            byte cidr = 20;
            var i = -1;
            var subnets = IpNetwork.Subnet(ipnetwork, cidr);
            foreach (var ipn in subnets)
            {
                i++;
                Assert.AreEqual(subnets[i], ipn, "subnet");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestSubnet13()
        {
            var ipnetwork = IpNetwork.IanaCblkReserved1;
            byte cidr = 20;
            var subnets = IpNetwork.Subnet(ipnetwork, cidr);
            var error = subnets[1000];
        }

        #endregion

        #region TrySubnet

        [TestMethod]
        public void TestTrySubnet1()
        {
            IpNetwork ipnetwork = null;
            byte cidr = 9;

            IpNetworkCollection subnets = null;
            var subnetted = IpNetwork.TrySubnet(ipnetwork, cidr, out subnets);

            Assert.AreEqual(false, subnetted, "subnetted");
        }

        [TestMethod]
        public void TestTrySubnet3()
        {
            var ipnetwork = IpNetwork.IanaAblkReserved1;
            byte cidr = 55;

            IpNetworkCollection subnets = null;
            var subnetted = IpNetwork.TrySubnet(ipnetwork, cidr, out subnets);

            Assert.AreEqual(false, subnetted, "subnetted");
        }


        [TestMethod]
        public void TestTrySubnet4()
        {
            var ipnetwork = IpNetwork.IanaAblkReserved1;
            byte cidr = 1;

            IpNetworkCollection subnets = null;
            var subnetted = IpNetwork.TrySubnet(ipnetwork, cidr, out subnets);

            Assert.AreEqual(false, subnetted, "subnetted");
        }


        [TestMethod]
        public void TestTrySubnet5()
        {
            var ipnetwork = IpNetwork.IanaAblkReserved1;
            byte cidr = 9;


            IpNetworkCollection subnets = null;
            var subnetted = IpNetwork.TrySubnet(ipnetwork, cidr, out subnets);

            Assert.AreEqual(true, subnetted, "subnetted");
            Assert.AreEqual(2, subnets.Count, "count");
            Assert.AreEqual("10.0.0.0/9", subnets[0].ToString(), "subnet1");
            Assert.AreEqual("10.128.0.0/9", subnets[1].ToString(), "subnet2");
        }


        [TestMethod]
        public void TestTrySubnet6()
        {
            var ipnetwork = IpNetwork.IanaCblkReserved1;
            byte cidr = 20;

            IpNetworkCollection subnets = null;
            var subnetted = IpNetwork.TrySubnet(ipnetwork, cidr, out subnets);

            Assert.AreEqual(true, subnetted, "subnetted");
            Assert.AreEqual(16, subnets.Count, "count");
            Assert.AreEqual("192.168.0.0/20", subnets[0].ToString(), "subnet1");
            Assert.AreEqual("192.168.16.0/20", subnets[1].ToString(), "subnet2");
            Assert.AreEqual("192.168.32.0/20", subnets[2].ToString(), "subnet3");
            Assert.AreEqual("192.168.48.0/20", subnets[3].ToString(), "subnet4");
            Assert.AreEqual("192.168.64.0/20", subnets[4].ToString(), "subnet5");
            Assert.AreEqual("192.168.80.0/20", subnets[5].ToString(), "subnet6");
            Assert.AreEqual("192.168.96.0/20", subnets[6].ToString(), "subnet7");
            Assert.AreEqual("192.168.112.0/20", subnets[7].ToString(), "subnet8");
            Assert.AreEqual("192.168.128.0/20", subnets[8].ToString(), "subnet9");
            Assert.AreEqual("192.168.144.0/20", subnets[9].ToString(), "subnet10");
            Assert.AreEqual("192.168.160.0/20", subnets[10].ToString(), "subnet11");
            Assert.AreEqual("192.168.176.0/20", subnets[11].ToString(), "subnet12");
            Assert.AreEqual("192.168.192.0/20", subnets[12].ToString(), "subnet13");
            Assert.AreEqual("192.168.208.0/20", subnets[13].ToString(), "subnet14");
            Assert.AreEqual("192.168.224.0/20", subnets[14].ToString(), "subnet15");
            Assert.AreEqual("192.168.240.0/20", subnets[15].ToString(), "subnet16");
        }

        #endregion

        #region Supernet

        [TestMethod]
        public void TestSupernet1()
        {
            var network1 = IpNetwork.Parse("192.168.0.1/24");
            var network2 = IpNetwork.Parse("192.168.1.1/24");
            var expected = IpNetwork.Parse("192.168.0.0/23");
            var supernet = IpNetwork.Supernet(network1, network2);

            Assert.AreEqual(expected, supernet, "supernet");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSupernet2()
        {
            IpNetwork network1 = null;
            var network2 = IpNetwork.Parse("192.168.1.1/24");
            var supernet = IpNetwork.Supernet(network1, network2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSupernet3()
        {
            var network1 = IpNetwork.Parse("192.168.1.1/24");
            IpNetwork network2 = null;
            var supernet = IpNetwork.Supernet(network1, network2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSupernet4()
        {
            var network1 = IpNetwork.Parse("192.168.0.1/24");
            var network2 = IpNetwork.Parse("192.168.1.1/25");
            var supernet = IpNetwork.Supernet(network1, network2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestSupernet5()
        {
            var network1 = IpNetwork.Parse("192.168.0.1/24");
            var network2 = IpNetwork.Parse("192.168.5.1/24");
            var supernet = IpNetwork.Supernet(network1, network2);
        }

        [TestMethod]
        public void TestSupernet6()
        {
            var network1 = IpNetwork.Parse("192.168.0.1/24");
            var network2 = IpNetwork.Parse("192.168.0.1/25");
            var expected = IpNetwork.Parse("192.168.0.0/24");
            var supernet = IpNetwork.Supernet(network1, network2);

            Assert.AreEqual(expected, supernet, "supernet");
        }

        [TestMethod]
        public void TestSupernet7()
        {
            var network1 = IpNetwork.Parse("192.168.0.1/25");
            var network2 = IpNetwork.Parse("192.168.0.1/24");
            var expected = IpNetwork.Parse("192.168.0.0/24");
            var supernet = IpNetwork.Supernet(network1, network2);

            Assert.AreEqual(expected, supernet, "supernet");
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSupernet8()
        {
            var network1 = IpNetwork.Parse("192.168.1.1/24");
            var network2 = IpNetwork.Parse("192.168.2.1/24");
            var supernet = IpNetwork.Supernet(network1, network2);
        }

        #endregion

        #region TrySupernet

        [TestMethod]
        public void TestTrySupernet1()
        {
            var network1 = IpNetwork.Parse("192.168.0.1/24");
            var network2 = IpNetwork.Parse("192.168.1.1/24");
            var supernetExpected = IpNetwork.Parse("192.168.0.0/23");
            IpNetwork supernet;
            var parsed = true;
            var result = IpNetwork.TrySupernet(network1, network2, out supernet);

            Assert.AreEqual(supernetExpected, supernet, "supernet");
            Assert.AreEqual(parsed, result, "parsed");
        }

        [TestMethod]
        public void TestTrySupernet2()
        {
            IpNetwork network1 = null;
            var network2 = IpNetwork.Parse("192.168.1.1/24");
            IpNetwork supernetExpected = null;
            IpNetwork supernet;
            var parsed = false;
            var result = IpNetwork.TrySupernet(network1, network2, out supernet);

            Assert.AreEqual(supernetExpected, supernet, "supernet");
            Assert.AreEqual(parsed, result, "parsed");
        }

        [TestMethod]
        public void TestTrySupernet3()
        {
            var network1 = IpNetwork.Parse("192.168.1.1/24");
            IpNetwork network2 = null;
            IpNetwork supernetExpected = null;
            IpNetwork supernet;
            var parsed = false;
            var result = IpNetwork.TrySupernet(network1, network2, out supernet);

            Assert.AreEqual(supernetExpected, supernet, "supernet");
            Assert.AreEqual(parsed, result, "parsed");
        }

        [TestMethod]
        public void TestTrySupernet4()
        {
            var network1 = IpNetwork.Parse("192.168.0.1/24");
            var network2 = IpNetwork.Parse("192.168.1.1/25");
            IpNetwork supernetExpected = null;
            IpNetwork supernet;
            var parsed = false;
            var result = IpNetwork.TrySupernet(network1, network2, out supernet);

            Assert.AreEqual(supernetExpected, supernet, "supernet");
            Assert.AreEqual(parsed, result, "parsed");
        }

        [TestMethod]
        public void TestTrySupernet5()
        {
            var network1 = IpNetwork.Parse("192.168.0.1/24");
            var network2 = IpNetwork.Parse("192.168.5.1/24");
            IpNetwork supernetExpected = null;
            IpNetwork supernet;
            var parsed = false;
            var result = IpNetwork.TrySupernet(network1, network2, out supernet);

            Assert.AreEqual(supernetExpected, supernet, "supernet");
            Assert.AreEqual(parsed, result, "parsed");
        }

        [TestMethod]
        public void TestTrySupernet6()
        {
            var network1 = IpNetwork.Parse("192.168.0.1/24");
            var network2 = IpNetwork.Parse("192.168.0.1/25");
            var supernetExpected = IpNetwork.Parse("192.168.0.0/24");
            IpNetwork supernet;
            var parsed = true;
            var result = IpNetwork.TrySupernet(network1, network2, out supernet);

            Assert.AreEqual(supernetExpected, supernet, "supernet");
            Assert.AreEqual(parsed, result, "parsed");
        }

        [TestMethod]
        public void TestTrySupernet7()
        {
            var network1 = IpNetwork.Parse("192.168.0.1/25");
            var network2 = IpNetwork.Parse("192.168.0.1/24");
            var supernetExpected = IpNetwork.Parse("192.168.0.0/24");
            IpNetwork supernet;
            var parsed = true;
            var result = IpNetwork.TrySupernet(network1, network2, out supernet);

            Assert.AreEqual(supernetExpected, supernet, "supernet");
            Assert.AreEqual(parsed, result, "parsed");
        }

        [TestMethod]
        public void TestTrySupernet8()
        {
            var network1 = IpNetwork.Parse("192.168.1.1/24");
            var network2 = IpNetwork.Parse("192.168.2.1/24");
            IpNetwork supernetExpected = null;
            IpNetwork supernet;
            var parsed = false;
            var result = IpNetwork.TrySupernet(network1, network2, out supernet);

            Assert.AreEqual(supernetExpected, supernet, "supernet");
            Assert.AreEqual(parsed, result, "parsed");
        }

        [TestMethod]
        public void TestTrySupernet9()
        {
            var network1 = IpNetwork.Parse("192.168.1.1/24");
            var network2 = IpNetwork.Parse("192.168.2.1/24");
            IpNetwork[] network3 = { network1, network2 };
            IpNetwork[] supernetExpected = { network1, network2 };
            IpNetwork[] supernet;
            var parsed = true;
            var result = IpNetwork.TrySupernet(network3, out supernet);

            Assert.AreEqual(supernetExpected[0], supernet[0], "supernet");
            Assert.AreEqual(supernetExpected[1], supernet[1], "supernet");
            Assert.AreEqual(parsed, result, "parsed");
        }


        [TestMethod]
        public void TestTrySupernet10()
        {
            var network1 = IpNetwork.Parse("192.168.0.1/24");
            var network2 = IpNetwork.Parse("192.168.1.1/24");
            IpNetwork[] network3 = { network1, network2 };
            IpNetwork[] supernetExpected = { IpNetwork.Parse("192.168.0.0/23") };
            IpNetwork[] supernet;
            var parsed = true;
            var result = IpNetwork.TrySupernet(network3, out supernet);

            Assert.AreEqual(supernetExpected[0], supernet[0], "supernet");
            Assert.AreEqual(parsed, result, "parsed");
        }


        [TestMethod]
        public void TestTrySupernet11()
        {
            IpNetwork[] network3 = null;
            IpNetwork[] supernetExpected = { IpNetwork.Parse("192.168.0.0/23") };
            IpNetwork[] supernet;
            var parsed = false;
            var result = IpNetwork.TrySupernet(network3, out supernet);

            Assert.AreEqual(null, supernet, "supernet");
            Assert.AreEqual(parsed, result, "parsed");
        }

        #endregion

        #region SupernetArray

        [TestMethod]
        public void TestTrySupernetArray()
        {
            var ipnetwork1 = IpNetwork.Parse("192.168.0.0/24");
            var ipnetwork2 = IpNetwork.Parse("192.168.1.0/24");
            var ipnetwork3 = IpNetwork.Parse("192.168.2.0/24");
            var ipnetwork4 = IpNetwork.Parse("192.168.3.0/24");

            IpNetwork[] ipnetworks = { ipnetwork1, ipnetwork2, ipnetwork3, ipnetwork4 };
            IpNetwork[] expected = { IpNetwork.Parse("192.168.0.0/22") };

            var result = IpNetwork.Supernet(ipnetworks);

            Assert.AreEqual(expected.Length, result.Length, "supernetarray");
            Assert.AreEqual(expected[0], expected[0], "suppernet");
        }

        [TestMethod]
        public void TestTrySupernetArray1()
        {
            IpNetwork[] ipnetworks = { };
            IpNetwork[] expected = { };

            var result = IpNetwork.Supernet(ipnetworks);

            Assert.AreEqual(expected.Length, result.Length, "supernetarray");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestTrySupernetArray2()
        {
            IpNetwork[] ipnetworks = null;
            var result = IpNetwork.Supernet(ipnetworks);
        }


        [TestMethod]
        public void TestTrySupernetArray3()
        {
            IpNetwork ipnetwork1 = null;
            IpNetwork ipnetwork2 = null;
            IpNetwork ipnetwork3 = null;
            IpNetwork ipnetwork4 = null;

            IpNetwork[] ipnetworks = { ipnetwork1, ipnetwork2, ipnetwork3, ipnetwork4 };
            IpNetwork[] expected = { };

            var result = IpNetwork.Supernet(ipnetworks);

            Assert.AreEqual(expected.Length, result.Length, "supernetarray");
        }

        [TestMethod]
        public void TestTrySupernetArray4()
        {
            var ipnetwork1 = IpNetwork.Parse("192.168.0.0/24");
            var subnetted = IpNetwork.Subnet(ipnetwork1, 32);
            var ipnetworks = subnetted.ToArray();
            Assert.AreEqual(256, ipnetworks.Length, "subnet");

            IpNetwork[] expected = { IpNetwork.Parse("192.168.0.0/24") };

            var result = IpNetwork.Supernet(ipnetworks);

            Assert.AreEqual(expected.Length, result.Length, "supernetarray");
            Assert.AreEqual(expected[0], ipnetwork1, "suppernet");
        }


        [TestMethod]
        public void TestTrySupernetArray5()
        {
            var ipnetwork1 = IpNetwork.Parse("192.168.0.0/16");
            var subnetted = IpNetwork.Subnet(ipnetwork1, 32);
            var ipnetworks = subnetted.ToArray();
            Assert.AreEqual(65536, ipnetworks.Length, "subnet");

            IpNetwork[] expected = { IpNetwork.Parse("192.168.0.0/16") };

            var result = IpNetwork.Supernet(ipnetworks);

            Assert.AreEqual(expected.Length, result.Length, "supernetarray");
            Assert.AreEqual(expected[0], ipnetwork1, "suppernet");
        }

        public void TestTrySupernetArray6()
        {
            var ipnetwork1 = IpNetwork.Parse("192.168.0.0/8");
            var subnetted = IpNetwork.Subnet(ipnetwork1, 32);
            var ipnetworks = subnetted.ToArray();
            Assert.AreEqual(16777216, ipnetworks.Length, "subnet");

            IpNetwork[] expected = { IpNetwork.Parse("192.0.0.0/8") };

            var result = IpNetwork.Supernet(ipnetworks);

            Assert.AreEqual(expected.Length, result.Length, "supernetarray");
            Assert.AreEqual(expected[0], ipnetwork1, "suppernet");
        }

        [TestMethod]
        public void TestTrySupernetArray7()
        {
            IpNetwork[] ipnetworks =
            {
                IpNetwork.Parse("10.0.2.2/24"),
                IpNetwork.Parse("192.168.0.0/24"),
                IpNetwork.Parse("192.168.1.0/24"),
                IpNetwork.Parse("192.168.2.0/24"),
                IpNetwork.Parse("10.0.1.1/24"),
                IpNetwork.Parse("192.168.3.0/24")
            };

            IpNetwork[] expected =
            {
                IpNetwork.Parse("10.0.1.0/24"),
                IpNetwork.Parse("10.0.2.0/24"),
                IpNetwork.Parse("192.168.0/22")
            };

            var result = IpNetwork.Supernet(ipnetworks);

            Assert.AreEqual(expected.Length, result.Length, "supernetarray");
            Assert.AreEqual(expected[0], result[0], "suppernet");
            Assert.AreEqual(expected[1], result[1], "suppernet1");
            Assert.AreEqual(expected[2], result[2], "suppernet2");
        }

        [TestMethod]
        public void TestTrySupernetArray8()
        {
            IpNetwork[] ipnetworks =
            {
                IpNetwork.Parse("10.0.2.2/24"),
                IpNetwork.Parse("192.168.0.0/24"),
                IpNetwork.Parse("192.168.1.0/24"),
                IpNetwork.Parse("192.168.2.0/24"),
                IpNetwork.Parse("10.0.1.1/24"),
                IpNetwork.Parse("192.168.3.0/24"),
                IpNetwork.Parse("10.6.6.6/8")
            };

            IpNetwork[] expected =
            {
                IpNetwork.Parse("10.0.0.0/8"),
                IpNetwork.Parse("192.168.0/22")
            };

            var result = IpNetwork.Supernet(ipnetworks);

            Assert.AreEqual(expected.Length, result.Length, "supernetarray");
            Assert.AreEqual(expected[0], result[0], "suppernet");
            Assert.AreEqual(expected[1], result[1], "suppernet1");
        }


        [TestMethod]
        public void TestTrySupernetArray9()
        {
            IpNetwork[] ipnetworks =
            {
                IpNetwork.Parse("10.0.2.2/24"),
                IpNetwork.Parse("192.168.0.0/24"),
                IpNetwork.Parse("192.168.1.0/24"),
                IpNetwork.Parse("192.168.2.0/24"),
                IpNetwork.Parse("10.0.1.1/24"),
                IpNetwork.Parse("192.168.3.0/24"),
                IpNetwork.Parse("10.6.6.6/8"),
                IpNetwork.Parse("11.6.6.6/8"),
                IpNetwork.Parse("12.6.6.6/8")
            };

            IpNetwork[] expected =
            {
                IpNetwork.Parse("10.0.0.0/7"),
                IpNetwork.Parse("12.0.0.0/8"),
                IpNetwork.Parse("192.168.0/22")
            };

            var result = IpNetwork.Supernet(ipnetworks);

            Assert.AreEqual(expected.Length, result.Length, "supernetarray");
            Assert.AreEqual(expected[0], result[0], "suppernet");
            Assert.AreEqual(expected[1], result[1], "suppernet1");
            Assert.AreEqual(expected[2], result[2], "suppernet2");
        }

        #endregion

        #region Equals

        [TestMethod]
        public void TestEquals1()
        {
            var network1 = IpNetwork.Parse("192.168.0.1/24");
            var network2 = IpNetwork.Parse("192.168.0.1/24");
            var result = network1.Equals(network2);
            var expected = true;

            Assert.AreEqual(expected, result, "equals");
        }

        [TestMethod]
        public void TestEquals2()
        {
            var network1 = IpNetwork.Parse("192.168.0.1/24");
            IpNetwork network2 = null;
            var result = network1.Equals(network2);
            var expected = false;

            Assert.AreEqual(expected, result, "equals");
        }

        [TestMethod]
        public void TestEquals3()
        {
            var network1 = IpNetwork.Parse("192.168.0.1/24");
            object network2 = "";
            var result = network1.Equals(network2);
            var expected = false;

            Assert.AreEqual(expected, result, "equals");
        }

        [TestMethod]
        public void TestEquals4()
        {
            var network1 = IpNetwork.Parse("192.168.0.1/24");
            var network2 = IpNetwork.Parse("192.168.0.1/25");
            var result = network1.Equals(network2);
            var expected = false;

            Assert.AreEqual(expected, result, "equals");
        }


        [TestMethod]
        public void TestEquals5()
        {
            var network1 = IpNetwork.Parse("192.168.0.1/24");
            var network2 = IpNetwork.Parse("192.168.1.1/24");
            var result = network1.Equals(network2);
            var expected = false;

            Assert.AreEqual(expected, result, "equals");
        }

        #endregion

        #region WideSubnet

        [TestMethod]
        public void TestWideSubnet1()
        {
            var start = "192.168.168.0";
            var end = "192.168.168.255";
            var expected = IpNetwork.Parse("192.168.168.0/24");

            var wideSubnet = IpNetwork.WideSubnet(start, end);
            Assert.AreEqual(expected, wideSubnet, "wideSubnet");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestWideSubnet2()
        {
            string start = null;
            var end = "192.168.168.255";

            var wideSubnet = IpNetwork.WideSubnet(start, end);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestWideSubnet3()
        {
            var start = "192.168.168.255";
            string end = null;

            var wideSubnet = IpNetwork.WideSubnet(start, end);
        }

        [TestMethod]
        public void TestWideSubnet4()
        {
            var start = "192.168.168.255";
            var end = "192.168.168.0";


            var expected = IpNetwork.Parse("192.168.168.0/24");

            var wideSubnet = IpNetwork.WideSubnet(start, end);
            Assert.AreEqual(expected, wideSubnet, "wideSubnet");
        }


        [TestMethod]
        public void TestWideSubnet7()
        {
            var start = "0.0.0.0";
            var end = "0.255.255.255";

            var expected = IpNetwork.Parse("0.0.0.0/8");

            var wideSubnet = IpNetwork.WideSubnet(start, end);
            Assert.AreEqual(expected, wideSubnet, "wideSubnet");
        }


        [TestMethod]
        public void TestWideSubnet8()
        {
            var start = "1.2.3.4";
            var end = "5.6.7.8";

            var expected = IpNetwork.Parse("0.0.0.0/5");

            var wideSubnet = IpNetwork.WideSubnet(start, end);
            Assert.AreEqual(expected, wideSubnet, "wideSubnet");
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestWideSubnetInvalid1()
        {
            var start = "invalid";
            var end = "5.6.7.8";

            var wideSubnet = IpNetwork.WideSubnet(start, end);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestWideSubnetInvalid2()
        {
            var start = "1.2.3.4";
            var end = "invalid";

            var wideSubnet = IpNetwork.WideSubnet(start, end);
        }

        #endregion

        #region TryGuessCidr

        [TestMethod]
        public void TestTryGuessCidrNull()
        {
            byte cidr;
            var parsed = IpNetwork.TryGuessCidr(null, out cidr);

            Assert.AreEqual(false, parsed, "parsed");
            Assert.AreEqual(0, cidr, "cidr");
        }

        [TestMethod]
        public void TestTryGuessCidrA()
        {
            byte cidr;
            var parsed = IpNetwork.TryGuessCidr("10.0.0.0", out cidr);

            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(8, cidr, "cidr");
        }

        [TestMethod]
        public void TestTryGuessCidrB()
        {
            byte cidr;
            var parsed = IpNetwork.TryGuessCidr("172.0.0.0", out cidr);

            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(16, cidr, "cidr");
        }

        [TestMethod]
        public void TestTryGuessCidrC()
        {
            byte cidr;
            var parsed = IpNetwork.TryGuessCidr("192.0.0.0", out cidr);

            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(24, cidr, "cidr");
        }

        [TestMethod]
        public void TestTryGuessCidrD()
        {
            byte cidr;
            var parsed = IpNetwork.TryGuessCidr("224.0.0.0", out cidr);

            Assert.AreEqual(false, parsed, "parsed");
        }

        [TestMethod]
        public void TestTryGuessCidrE()
        {
            byte cidr;
            var parsed = IpNetwork.TryGuessCidr("240.0.0.0", out cidr);

            Assert.AreEqual(false, parsed, "parsed");
        }

        #endregion

        #region GetHashCode

        [TestMethod]
        public void TestGetHashCode1()
        {
            var ipnetwork = IpNetwork.Parse("0.0.1.1/0");
            var hashCode = ipnetwork.GetHashCode();
            Assert.AreEqual(-1611127575, hashCode, "hashcode");
        }

        [TestMethod]
        public void TestGetHashCode2()
        {
            var ipnetwork = IpNetwork.Parse("0.0.0.0/1");
            var hashCode = ipnetwork.GetHashCode();
            Assert.AreEqual(-1014887953, hashCode, "hashcode");
        }

        [TestMethod]
        public void TestGetHashCode3()
        {
            var ipnetwork = IpNetwork.Parse("0.0.0.0/32");
            var hashCode = ipnetwork.GetHashCode();
            Assert.AreEqual(-1013970447, hashCode, "hashcode");
        }

        #endregion

        #region Print

        [TestMethod]
        public void Print()
        {
            var ipn = IpNetwork.Parse("0.0.0.0/0");
            var print = IpNetwork.Print(ipn);
            Assert.AreEqual(@"IPNetwork   : 0.0.0.0/0
Network     : 0.0.0.0
Netmask     : 0.0.0.0
Cidr        : 0
Broadcast   : 255.255.255.255
FirstUsable : 0.0.0.1
LastUsable  : 255.255.255.254
Usable      : 4294967294
", print, "Print");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PrintNull()
        {
            IpNetwork ipn = null;
            var print = IpNetwork.Print(ipn);
        }

        #endregion

        #region Sort

        [TestMethod]
        public void TestSort1()
        {
            string[] ips = { "1.1.1.1", "255.255.255.255", "2.2.2.2", "0.0.0.0" };
            var ipns = new List<IpNetwork>();
            Array.ForEach(ips, delegate (string ip)
            {
                IpNetwork ipn;
                if (IpNetwork.TryParse(ip, 32, out ipn))
                {
                    ipns.Add(ipn);
                }
            });

            ipns.Sort();
            Assert.AreEqual("0.0.0.0/32", ipns[0].ToString(), "0");
            Assert.AreEqual("1.1.1.1/32", ipns[1].ToString(), "1");
            Assert.AreEqual("2.2.2.2/32", ipns[2].ToString(), "2");
            Assert.AreEqual("255.255.255.255/32", ipns[3].ToString(), "3");
        }

        [TestMethod]
        public void TestSort2()
        {
            string[] ips = { "0.0.0.100/32", "0.0.0.0/24" };
            var ipns = new List<IpNetwork>();
            Array.ForEach(ips, delegate (string ip)
            {
                IpNetwork ipn;
                if (IpNetwork.TryParse(ip, out ipn))
                {
                    ipns.Add(ipn);
                }
            });

            ipns.Sort();
            Assert.AreEqual("0.0.0.0/24", ipns[0].ToString(), "0");
            Assert.AreEqual("0.0.0.100/32", ipns[1].ToString(), "1");
        }

        #endregion

        #region TryWideSubnet

        [TestMethod]
        public void TryWideSubnet1()
        {
            string[] ips = { "1.1.1.1", "255.255.255.255", "2.2.2.2", "0.0.0.0" };
            var ipns = new List<IpNetwork>();
            Array.ForEach(ips, delegate (string ip)
            {
                IpNetwork ipn;
                if (IpNetwork.TryParse(ip, 32, out ipn))
                {
                    ipns.Add(ipn);
                }
            });

            IpNetwork ipnetwork = null;
            var wide = IpNetwork.TryWideSubnet(ipns.ToArray(), out ipnetwork);
            Assert.AreEqual(true, wide, "wide");
            Assert.AreEqual("0.0.0.0/0", ipnetwork.ToString(), "ipnetwork");
        }

        [TestMethod]
        public void TryWideSubnet2()
        {
            string[] ips = { "1.1.1.1", "10.0.0.0", "2.2.2.2", "0.0.0.0" };
            var ipns = new List<IpNetwork>();
            Array.ForEach(ips, delegate (string ip)
            {
                IpNetwork ipn;
                if (IpNetwork.TryParse(ip, 32, out ipn))
                {
                    ipns.Add(ipn);
                }
            });

            IpNetwork ipnetwork = null;
            var wide = IpNetwork.TryWideSubnet(ipns.ToArray(), out ipnetwork);
            Assert.AreEqual(true, wide, "wide");
            Assert.AreEqual("0.0.0.0/4", ipnetwork.ToString(), "ipnetwork");
        }


        [TestMethod]
        public void TryWideSubnet3()
        {
            string[] ips = { "a", "b", "c", "d" };
            var ipns = new List<IpNetwork>();
            Array.ForEach(ips, delegate (string ip)
            {
                IpNetwork ipn;
                if (IpNetwork.TryParse(ip, 32, out ipn))
                {
                    ipns.Add(ipn);
                }
            });

            IpNetwork ipnetwork = null;
            var wide = IpNetwork.TryWideSubnet(ipns.ToArray(), out ipnetwork);
            Assert.AreEqual(false, wide, "wide");
        }

        [TestMethod]
        public void TryWideSubnet4()
        {
            string[] ips = { "a", "b", "1.1.1.1", "d" };
            var ipns = new List<IpNetwork>();
            Array.ForEach(ips, delegate (string ip)
            {
                IpNetwork ipn;
                if (IpNetwork.TryParse(ip, 32, out ipn))
                {
                    ipns.Add(ipn);
                }
            });

            IpNetwork ipnetwork = null;
            var wide = IpNetwork.TryWideSubnet(ipns.ToArray(), out ipnetwork);
            Assert.AreEqual(true, wide, "wide");
            Assert.AreEqual("1.1.1.1/32", ipnetwork.ToString(), "ipnetwork");
        }

        [TestMethod]
        public void TryWideSubnetNull()
        {
            IpNetwork ipnetwork = null;
            var wide = IpNetwork.TryWideSubnet(null, out ipnetwork);
            Assert.AreEqual(false, wide, "wide");
        }

        #endregion

        #region WideSubnet

        [TestMethod]
        public void WideSubnet1()
        {
            string[] ips = { "1.1.1.1", "255.255.255.255", "2.2.2.2", "0.0.0.0" };
            var ipns = new List<IpNetwork>();
            Array.ForEach(ips, delegate (string ip)
            {
                IpNetwork ipn;
                if (IpNetwork.TryParse(ip, 32, out ipn))
                {
                    ipns.Add(ipn);
                }
            });

            var ipnetwork = IpNetwork.WideSubnet(ipns.ToArray());
            Assert.AreEqual("0.0.0.0/0", ipnetwork.ToString(), "ipnetwork");
        }

        [TestMethod]
        public void WideSubnet2()
        {
            string[] ips = { "1.1.1.1", "10.0.0.0", "2.2.2.2", "0.0.0.0" };
            var ipns = new List<IpNetwork>();
            Array.ForEach(ips, delegate (string ip)
            {
                IpNetwork ipn;
                if (IpNetwork.TryParse(ip, 32, out ipn))
                {
                    ipns.Add(ipn);
                }
            });

            var ipnetwork = IpNetwork.WideSubnet(ipns.ToArray());
            Assert.AreEqual("0.0.0.0/4", ipnetwork.ToString(), "ipnetwork");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WideSubnetNull()
        {
            var ipnetwork = IpNetwork.WideSubnet(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WideSubnetNull2()
        {
            string[] ips = { "a", "b", "e", "d" };
            var ipns = new List<IpNetwork>();
            Array.ForEach(ips, delegate (string ip)
            {
                IpNetwork ipn;
                if (IpNetwork.TryParse(ip, 32, out ipn))
                {
                    ipns.Add(ipn);
                }
            });

            var ipnetwork = IpNetwork.WideSubnet(ipns.ToArray());
        }

        #endregion

        /**
         * 
        #region TrySubstractNetwork


        [TestMethod]
        public void TrySubstractNetwork1() {
            string[] ips = new[] { "178.82.0.0/16" };
            string substract = "178.82.131.209/32";

            List<IPNetwork> ipns = new List<IPNetwork>();
            Array.ForEach<string>(ips, new Action<string>(
                delegate(string ip)
                {
                    IPNetwork ipn;
                    if (IPNetwork.TryParse(ip, out ipn)) {
                        ipns.Add(ipn);
                    }
                }
            ));

            var nsubstract = IPNetwork.Parse(substract);

            IEnumerable<IPNetwork> result;
            bool substracted = IPNetwork.TrySubstractNetwork(ipns.ToArray(), nsubstract, out result);
            Assert.AreEqual(true, substracted, "substracted");

        }

        [TestMethod]
        public void TrySubstractNetwork2() {
            string[] ips = new[] { "0.0.0.0/0" };
            string substract = "1.1.1.1/32";

            List<IPNetwork> ipns = new List<IPNetwork>();
            Array.ForEach<string>(ips, new Action<string>(
                delegate(string ip)
                {
                    IPNetwork ipn;
                    if (IPNetwork.TryParse(ip, out ipn)) {
                        ipns.Add(ipn);
                    }
                }
            ));

            var nsubstract = IPNetwork.Parse(substract);

            IEnumerable<IPNetwork> result;
            bool substracted = IPNetwork.TrySubstractNetwork(ipns.ToArray(), nsubstract, out result);
            Assert.AreEqual(true, substracted, "substracted");

        }



        #endregion
        **/

        #region Count

        [TestMethod]
        public void Total32()
        {
            var network = IpNetwork.Parse("0.0.0.0/32");
            var total = 1;
            Assert.AreEqual(total, network.Total, "Total");
        }

        [TestMethod]
        public void Total31()
        {
            var network = IpNetwork.Parse("0.0.0.0/31");
            var total = 2;
            Assert.AreEqual(total, network.Total, "Total");
        }

        [TestMethod]
        public void Total30()
        {
            var network = IpNetwork.Parse("0.0.0.0/30");
            var total = 4;
            Assert.AreEqual(total, network.Total, "Total");
        }

        [TestMethod]
        public void Total24()
        {
            var network = IpNetwork.Parse("0.0.0.0/24");
            var total = 256;
            Assert.AreEqual(total, network.Total, "Total");
        }

        [TestMethod]
        public void Total16()
        {
            var network = IpNetwork.Parse("0.0.0.0/16");
            var total = 65536;
            Assert.AreEqual(total, network.Total, "Total");
        }

        [TestMethod]
        public void Total8()
        {
            var network = IpNetwork.Parse("0.0.0.0/8");
            var total = 16777216;
            Assert.AreEqual(total, network.Total, "Total");
        }

        [TestMethod]
        public void Total0()
        {
            var network = IpNetwork.Parse("0.0.0.0/0");
            var total = 4294967296;
            Assert.AreEqual(total, network.Total, "Total");
        }

        #endregion

        #region Usable

        [TestMethod]
        public void Usable32()
        {
            var network = IpNetwork.Parse("0.0.0.0/32");
            uint usable = 0;
            Assert.AreEqual(usable, network.Usable, "Usable");
        }

        [TestMethod]
        public void Usable31()
        {
            var network = IpNetwork.Parse("0.0.0.0/31");
            uint usable = 0;
            Assert.AreEqual(usable, network.Usable, "Usable");
        }

        [TestMethod]
        public void Usable30()
        {
            var network = IpNetwork.Parse("0.0.0.0/30");
            uint usable = 2;
            Assert.AreEqual(usable, network.Usable, "Usable");
        }

        [TestMethod]
        public void Usable24()
        {
            var network = IpNetwork.Parse("0.0.0.0/24");
            uint usable = 254;
            Assert.AreEqual(usable, network.Usable, "Usable");
        }

        [TestMethod]
        public void Usable16()
        {
            var network = IpNetwork.Parse("0.0.0.0/16");
            uint usable = 65534;
            Assert.AreEqual(usable, network.Usable, "Usable");
        }

        [TestMethod]
        public void Usable8()
        {
            var network = IpNetwork.Parse("0.0.0.0/8");
            uint usable = 16777214;
            Assert.AreEqual(usable, network.Usable, "Usable");
        }

        [TestMethod]
        public void Usable0()
        {
            var network = IpNetwork.Parse("0.0.0.0/0");
            var usable = 4294967294;
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
            var parsed = IpNetwork.TryParseCidr(sidr, AddressFamily.InterNetwork, out cidr);

            Assert.AreEqual(true, parsed, "parsed");
            Assert.AreEqual(result, cidr, "cidr");
        }

        [TestMethod]
        public void TryParseCidr2()
        {
            var sidr = "sadsd";
            byte? cidr;
            byte? result = null;

            var parsed = IpNetwork.TryParseCidr(sidr, AddressFamily.InterNetwork, out cidr);

            Assert.AreEqual(false, parsed, "parsed");
            Assert.AreEqual(result, cidr, "cidr");
        }

        [TestMethod]
        public void TryParseCidr3()
        {
            var sidr = "33";
            byte? cidr;
            byte? result = null;

            var parsed = IpNetwork.TryParseCidr(sidr, AddressFamily.InterNetwork, out cidr);

            Assert.AreEqual(false, parsed, "parsed");
            Assert.AreEqual(result, cidr, "cidr");
        }

        #endregion
    }
}