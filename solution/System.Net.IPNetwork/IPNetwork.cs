using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Numerics;
using System.Text.RegularExpressions;

namespace System.Net.IPNetwork
{
    /// <summary>
    ///     IP Network utility class.
    ///     Use IPNetwork.Parse to create instances.
    /// </summary>
    public class IpNetwork : IComparable<IpNetwork>
    {
        #region constructor

        internal IpNetwork(BigInteger ipaddress, AddressFamily family, byte cidr)
        {
            var maxCidr = family == AddressFamily.InterNetwork ? 32 : 128;
            if (cidr > maxCidr)
            {
                throw new ArgumentOutOfRangeException(nameof(cidr));
            }

            _ipaddress = ipaddress;
            AddressFamily = family;
            Cidr = cidr;
        }

        #endregion

        #region overlap

        /// <summary>
        ///     return true is network2 overlap network
        /// </summary>
        /// <param name="network"></param>
        /// <param name="network2"></param>
        /// <returns></returns>
        public static bool Overlap(IpNetwork network, IpNetwork network2)
        {
            if (network == null)
            {
                throw new ArgumentNullException(nameof(network));
            }

            if (network2 == null)
            {
                throw new ArgumentNullException(nameof(network2));
            }


            var uintNetwork = network._network;
            var uintBroadcast = network._broadcast;

            var uintFirst = network2._network;
            var uintLast = network2._broadcast;

            var overlap =
                (uintFirst >= uintNetwork && uintFirst <= uintBroadcast)
                || (uintLast >= uintNetwork && uintLast <= uintBroadcast)
                || (uintFirst <= uintNetwork && uintLast >= uintBroadcast)
                || (uintFirst >= uintNetwork && uintLast <= uintBroadcast);

            return overlap;
        }

        #endregion

        #region ToString

        public override string ToString()
        {
            return $"{Network}/{Cidr}";
        }

        #endregion

        #region GetHashCode

        public override int GetHashCode()
        {
            return $"{_ipaddress.GetHashCode()}|{_network.GetHashCode()}|{Cidr.GetHashCode()}".GetHashCode();
        }

        #endregion

        #region Print

        /// <summary>
        ///     Print an ipnetwork in a clear representation string
        /// </summary>
        /// <param name="ipnetwork"></param>
        /// <returns></returns>
        public static string Print(IpNetwork ipnetwork)
        {
            if (ipnetwork == null)
            {
                throw new ArgumentNullException(nameof(ipnetwork));
            }
            var sw = new StringWriter();

            sw.WriteLine($"IPNetwork   : {ipnetwork}");
            sw.WriteLine($"Network     : {ipnetwork.Network}");
            sw.WriteLine($"Netmask     : {ipnetwork.Netmask}");
            sw.WriteLine($"Cidr        : {ipnetwork.Cidr}");
            sw.WriteLine($"Broadcast   : {ipnetwork.Broadcast}");
            sw.WriteLine($"FirstUsable : {ipnetwork.FirstUsable}");
            sw.WriteLine($"LastUsable  : {ipnetwork.LastUsable}");
            sw.WriteLine($"Usable      : {ipnetwork.Usable}");

            return sw.ToString();
        }

        #endregion

        #region ListIPAddress

        public static IpAddressCollection ListIpAddress(IpNetwork ipnetwork)
        {
            return new IpAddressCollection(ipnetwork);
        }

        #endregion

        #region properties

        //private uint _network;
        private readonly BigInteger _ipaddress;
        //private uint _netmask;
        //private uint _broadcast;
        //private uint _firstUsable;
        //private uint _lastUsable;
        //private uint _usable;

        #endregion

        #region accessors

        private BigInteger _network
        {
            get
            {
                var uintNetwork = _ipaddress & _netmask;
                return uintNetwork;
            }
        }

        /// <summary>
        ///     Network address
        /// </summary>
        public IPAddress Network => ToIpAddress(_network, AddressFamily);

        /// <summary>
        ///     Address Family
        /// </summary>
        public AddressFamily AddressFamily { get; }

        private BigInteger _netmask => ToUint(Cidr, AddressFamily);

        /// <summary>
        ///     Netmask
        /// </summary>
        public IPAddress Netmask => ToIpAddress(_netmask, AddressFamily);

        private BigInteger _broadcast
        {
            get
            {
                var width = AddressFamily == AddressFamily.InterNetwork ? 4 : 16;
                var uintBroadcast = _network + _netmask.PositiveReverse(width);
                return uintBroadcast;
            }
        }

        /// <summary>
        ///     Broadcast address
        /// </summary>
        public IPAddress Broadcast => AddressFamily == AddressFamily.InterNetworkV6 ? null : ToIpAddress(_broadcast, AddressFamily);

        /// <summary>
        ///     First usable IP adress in Network
        /// </summary>
        public IPAddress FirstUsable
        {
            get
            {
                var fisrt = AddressFamily == AddressFamily.InterNetworkV6
                    ? _network
                    : Usable <= 0 ? _network : _network + 1;
                return ToIpAddress(fisrt, AddressFamily);
            }
        }

        /// <summary>
        ///     Last usable IP adress in Network
        /// </summary>
        public IPAddress LastUsable
        {
            get
            {
                var last = AddressFamily == AddressFamily.InterNetworkV6
                    ? _broadcast
                    : Usable <= 0 ? _network : _broadcast - 1;
                return ToIpAddress(last, AddressFamily);
            }
        }

        /// <summary>
        ///     Number of usable IP adress in Network
        /// </summary>
        public BigInteger Usable
        {
            get
            {
                if (AddressFamily == AddressFamily.InterNetworkV6)
                {
                    return Total;
                }
                byte[] mask = {0xff, 0xff, 0xff, 0xff, 0x00};
                var bmask = new BigInteger(mask);
                var usableIps = Cidr > 30 ? 0 : (bmask >> Cidr) - 1;
                return usableIps;
            }
        }

        /// <summary>
        ///     Number of IP adress in Network
        /// </summary>
        public BigInteger Total
        {
            get
            {
                var max = AddressFamily == AddressFamily.InterNetwork ? 32 : 128;
                var count = BigInteger.Pow(2, max - Cidr);
                return count;
            }
        }


        /// <summary>
        ///     The CIDR netmask notation
        /// </summary>
        public byte Cidr { get; }

        #endregion

        #region parsers

        /// <summary>
        ///     192.168.168.100 - 255.255.255.0
        ///     Network   : 192.168.168.0
        ///     Netmask   : 255.255.255.0
        ///     Cidr      : 24
        ///     Start     : 192.168.168.1
        ///     End       : 192.168.168.254
        ///     Broadcast : 192.168.168.255
        /// </summary>
        /// <param name="ipaddress"></param>
        /// <param name="netmask"></param>
        /// <returns></returns>
        public static IpNetwork Parse(string ipaddress, string netmask)
        {
            IpNetwork ipnetwork;
            InternalParse(false, ipaddress, netmask, out ipnetwork);
            return ipnetwork;
        }

        /// <summary>
        ///     192.168.168.100/24
        ///     Network   : 192.168.168.0
        ///     Netmask   : 255.255.255.0
        ///     Cidr      : 24
        ///     Start     : 192.168.168.1
        ///     End       : 192.168.168.254
        ///     Broadcast : 192.168.168.255
        /// </summary>
        /// <param name="ipaddress"></param>
        /// <param name="cidr"></param>
        /// <returns></returns>
        public static IpNetwork Parse(string ipaddress, byte cidr)
        {
            IpNetwork ipnetwork;
            InternalParse(false, ipaddress, cidr, out ipnetwork);
            return ipnetwork;
        }

        /// <summary>
        ///     192.168.168.100 255.255.255.0
        ///     Network   : 192.168.168.0
        ///     Netmask   : 255.255.255.0
        ///     Cidr      : 24
        ///     Start     : 192.168.168.1
        ///     End       : 192.168.168.254
        ///     Broadcast : 192.168.168.255
        /// </summary>
        /// <param name="ipaddress"></param>
        /// <param name="netmask"></param>
        /// <returns></returns>
        public static IpNetwork Parse(IPAddress ipaddress, IPAddress netmask)
        {
            IpNetwork ipnetwork;
            InternalParse(false, ipaddress, netmask, out ipnetwork);
            return ipnetwork;
        }

        /// <summary>
        ///     192.168.0.1/24
        ///     192.168.0.1 255.255.255.0
        ///     Network   : 192.168.0.0
        ///     Netmask   : 255.255.255.0
        ///     Cidr      : 24
        ///     Start     : 192.168.0.1
        ///     End       : 192.168.0.254
        ///     Broadcast : 192.168.0.255
        /// </summary>
        /// <param name="network"></param>
        /// <returns></returns>
        public static IpNetwork Parse(string network)
        {
            IpNetwork ipnetwork;
            InternalParse(false, network, out ipnetwork);
            return ipnetwork;
        }

        #endregion

        #region TryParse

        /// <summary>
        ///     192.168.168.100 - 255.255.255.0
        ///     Network   : 192.168.168.0
        ///     Netmask   : 255.255.255.0
        ///     Cidr      : 24
        ///     Start     : 192.168.168.1
        ///     End       : 192.168.168.254
        ///     Broadcast : 192.168.168.255
        /// </summary>
        /// <param name="ipaddress"></param>
        /// <param name="netmask"></param>
        /// <returns></returns>
        public static bool TryParse(string ipaddress, string netmask, out IpNetwork ipnetwork)
        {
            IpNetwork ipnetwork2;
            InternalParse(true, ipaddress, netmask, out ipnetwork2);
            var parsed = ipnetwork2 != null;
            ipnetwork = ipnetwork2;
            return parsed;
        }


        /// <summary>
        ///     192.168.168.100/24
        ///     Network   : 192.168.168.0
        ///     Netmask   : 255.255.255.0
        ///     Cidr      : 24
        ///     Start     : 192.168.168.1
        ///     End       : 192.168.168.254
        ///     Broadcast : 192.168.168.255
        /// </summary>
        /// <param name="ipaddress"></param>
        /// <param name="cidr"></param>
        /// <returns></returns>
        public static bool TryParse(string ipaddress, byte cidr, out IpNetwork ipnetwork)
        {
            IpNetwork ipnetwork2;
            InternalParse(true, ipaddress, cidr, out ipnetwork2);
            var parsed = ipnetwork2 != null;
            ipnetwork = ipnetwork2;
            return parsed;
        }

        /// <summary>
        ///     192.168.0.1/24
        ///     192.168.0.1 255.255.255.0
        ///     Network   : 192.168.0.0
        ///     Netmask   : 255.255.255.0
        ///     Cidr      : 24
        ///     Start     : 192.168.0.1
        ///     End       : 192.168.0.254
        ///     Broadcast : 192.168.0.255
        /// </summary>
        /// <param name="network"></param>
        /// <param name="ipnetwork"></param>
        /// <returns></returns>
        public static bool TryParse(string network, out IpNetwork ipnetwork)
        {
            IpNetwork ipnetwork2;
            InternalParse(true, network, out ipnetwork2);
            var parsed = ipnetwork2 != null;
            ipnetwork = ipnetwork2;
            return parsed;
        }

        /// <summary>
        ///     192.168.0.1/24
        ///     192.168.0.1 255.255.255.0
        ///     Network   : 192.168.0.0
        ///     Netmask   : 255.255.255.0
        ///     Cidr      : 24
        ///     Start     : 192.168.0.1
        ///     End       : 192.168.0.254
        ///     Broadcast : 192.168.0.255
        /// </summary>
        /// <param name="ipaddress"></param>
        /// <param name="netmask"></param>
        /// <param name="ipnetwork"></param>
        /// <returns></returns>
        public static bool TryParse(IPAddress ipaddress, IPAddress netmask, out IpNetwork ipnetwork)
        {
            IpNetwork ipnetwork2;
            InternalParse(true, ipaddress, netmask, out ipnetwork2);
            var parsed = ipnetwork2 != null;
            ipnetwork = ipnetwork2;
            return parsed;
        }

        #endregion

        #region InternalParse

        /// <summary>
        ///     192.168.168.100 - 255.255.255.0
        ///     Network   : 192.168.168.0
        ///     Netmask   : 255.255.255.0
        ///     Cidr      : 24
        ///     Start     : 192.168.168.1
        ///     End       : 192.168.168.254
        ///     Broadcast : 192.168.168.255
        /// </summary>
        /// <param name="ipaddress"></param>
        /// <param name="netmask"></param>
        /// <returns></returns>
        private static void InternalParse(bool tryParse, string ipaddress, string netmask, out IpNetwork ipnetwork)
        {
            if (string.IsNullOrEmpty(ipaddress))
            {
                if (tryParse == false)
                {
                    throw new ArgumentNullException(nameof(ipaddress));
                }
                ipnetwork = null;
                return;
            }

            if (string.IsNullOrEmpty(netmask))
            {
                if (tryParse == false)
                {
                    throw new ArgumentNullException(nameof(netmask));
                }
                ipnetwork = null;
                return;
            }

            IPAddress ip;
            var ipaddressParsed = IPAddress.TryParse(ipaddress, out ip);
            if (ipaddressParsed == false)
            {
                if (tryParse == false)
                {
                    throw new ArgumentException("ipaddress");
                }
                ipnetwork = null;
                return;
            }

            IPAddress mask;
            var netmaskParsed = IPAddress.TryParse(netmask, out mask);
            if (netmaskParsed == false)
            {
                if (tryParse == false)
                {
                    throw new ArgumentException("netmask");
                }
                ipnetwork = null;
                return;
            }

            InternalParse(tryParse, ip, mask, out ipnetwork);
        }

        private static void InternalParse(bool tryParse, string network, out IpNetwork ipnetwork)
        {
            if (string.IsNullOrEmpty(network))
            {
                if (tryParse == false)
                {
                    throw new ArgumentNullException(nameof(network));
                }
                ipnetwork = null;
                return;
            }

            network = Regex.Replace(network, @"[^0-9a-f\.\/\s\:]+", "");
            network = Regex.Replace(network, @"\s{2,}", " ");
            network = network.Trim();
            var args = network.Split(' ', '/');
            byte cidr;
            if (args.Length == 1)
            {
                if (TryGuessCidr(args[0], out cidr))
                {
                    InternalParse(tryParse, args[0], cidr, out ipnetwork);
                    return;
                }

                if (tryParse == false)
                {
                    throw new ArgumentException("network");
                }
                ipnetwork = null;
                return;
            }

            if (byte.TryParse(args[1], out cidr))
            {
                InternalParse(tryParse, args[0], cidr, out ipnetwork);
                return;
            }

            InternalParse(tryParse, args[0], args[1], out ipnetwork);
        }


        /// <summary>
        ///     192.168.168.100 255.255.255.0
        ///     Network   : 192.168.168.0
        ///     Netmask   : 255.255.255.0
        ///     Cidr      : 24
        ///     Start     : 192.168.168.1
        ///     End       : 192.168.168.254
        ///     Broadcast : 192.168.168.255
        /// </summary>
        /// <param name="ipaddress"></param>
        /// <param name="netmask"></param>
        /// <returns></returns>
        private static void InternalParse(bool tryParse, IPAddress ipaddress, IPAddress netmask, out IpNetwork ipnetwork)
        {
            if (ipaddress == null)
            {
                if (tryParse == false)
                {
                    throw new ArgumentNullException(nameof(ipaddress));
                }
                ipnetwork = null;
                return;
            }

            if (netmask == null)
            {
                if (tryParse == false)
                {
                    throw new ArgumentNullException(nameof(netmask));
                }
                ipnetwork = null;
                return;
            }

            var uintIpAddress = ToBigInteger(ipaddress);
            byte? cidr2;
            var parsed = TryToCidr(netmask, out cidr2);
            if (parsed == false)
            {
                if (tryParse == false)
                {
                    throw new ArgumentException("netmask");
                }
                ipnetwork = null;
                return;
            }
            var cidr = (byte) cidr2;

            var ipnet = new IpNetwork(uintIpAddress, ipaddress.AddressFamily, cidr);
            ipnetwork = ipnet;
        }


        /// <summary>
        ///     192.168.168.100/24
        ///     Network   : 192.168.168.0
        ///     Netmask   : 255.255.255.0
        ///     Cidr      : 24
        ///     Start     : 192.168.168.1
        ///     End       : 192.168.168.254
        ///     Broadcast : 192.168.168.255
        /// </summary>
        /// <param name="ipaddress"></param>
        /// <param name="cidr"></param>
        /// <returns></returns>
        private static void InternalParse(bool tryParse, string ipaddress, byte cidr, out IpNetwork ipnetwork)
        {
            if (string.IsNullOrEmpty(ipaddress))
            {
                if (tryParse == false)
                {
                    throw new ArgumentNullException(nameof(ipaddress));
                }
                ipnetwork = null;
                return;
            }


            IPAddress ip = null;
            var ipaddressParsed = IPAddress.TryParse(ipaddress, out ip);
            if (ipaddressParsed == false)
            {
                if (tryParse == false)
                {
                    throw new ArgumentException("ipaddress");
                }
                ipnetwork = null;
                return;
            }

            IPAddress mask = null;
            var parsedNetmask = TryToNetmask(cidr, ip.AddressFamily, out mask);
            if (parsedNetmask == false)
            {
                if (tryParse == false)
                {
                    throw new ArgumentException("cidr");
                }
                ipnetwork = null;
                return;
            }


            InternalParse(tryParse, ip, mask, out ipnetwork);
        }

        #endregion

        #region converters

        #region ToUint

        /// <summary>
        ///     Convert an ipadress to decimal
        ///     0.0.0.0 -> 0
        ///     0.0.1.0 -> 256
        /// </summary>
        /// <param name="ipaddress"></param>
        /// <returns></returns>
        public static BigInteger ToBigInteger(IPAddress ipaddress)
        {
            BigInteger? uintIpAddress;
            InternalToBigInteger(false, ipaddress, out uintIpAddress);
            return (BigInteger) uintIpAddress;
        }

        /// <summary>
        ///     Convert an ipadress to decimal
        ///     0.0.0.0 -> 0
        ///     0.0.1.0 -> 256
        /// </summary>
        /// <param name="ipaddress"></param>
        /// <returns></returns>
        public static bool TryToBigInteger(IPAddress ipaddress, out BigInteger? uintIpAddress)
        {
            BigInteger? uintIpAddress2 = null;
            InternalToBigInteger(true, ipaddress, out uintIpAddress2);
            var parsed = uintIpAddress2 != null;
            uintIpAddress = uintIpAddress2;
            return parsed;
        }

        private static void InternalToBigInteger(bool tryParse, IPAddress ipaddress, out BigInteger? uintIpAddress)
        {
            if (ipaddress == null)
            {
                if (tryParse == false)
                {
                    throw new ArgumentNullException(nameof(ipaddress));
                }
                uintIpAddress = null;
                return;
            }

            var bytes = ipaddress.GetAddressBytes();
            if (bytes.Length != 4 && bytes.Length != 16)
            {
                if (tryParse == false)
                {
                    throw new ArgumentException("bytes");
                }
                uintIpAddress = null;
                return;
            }


            Array.Reverse(bytes);
            var unsigned = new List<byte>(bytes) {0};
            uintIpAddress = new BigInteger(unsigned.ToArray());
        }


        /// <summary>
        ///     Convert a cidr to BigInteger netmask
        /// </summary>
        /// <param name="cidr"></param>
        /// <returns></returns>
        public static BigInteger ToUint(byte cidr, AddressFamily family)
        {
            BigInteger? uintNetmask;
            InternalToBigInteger(false, cidr, family, out uintNetmask);
            return (BigInteger) uintNetmask;
        }


        /// <summary>
        ///     Convert a cidr to uint netmask
        /// </summary>
        /// <param name="cidr"></param>
        /// <returns></returns>
        public static bool TryToUint(byte cidr, AddressFamily family, out BigInteger? uintNetmask)
        {
            BigInteger? uintNetmask2;
            InternalToBigInteger(true, cidr, family, out uintNetmask2);
            var parsed = uintNetmask2 != null;
            uintNetmask = uintNetmask2;
            return parsed;
        }

        /// <summary>
        ///     Convert a cidr to uint netmask
        /// </summary>
        /// <param name="cidr"></param>
        /// <returns></returns>
        private static void InternalToBigInteger(bool tryParse, byte cidr, AddressFamily family,
            out BigInteger? uintNetmask)
        {
            if (family == AddressFamily.InterNetwork && cidr > 32)
            {
                if (tryParse == false)
                {
                    throw new ArgumentOutOfRangeException(nameof(cidr));
                }
                uintNetmask = null;
                return;
            }

            if (family == AddressFamily.InterNetworkV6 && cidr > 128)
            {
                if (tryParse == false)
                {
                    throw new ArgumentOutOfRangeException(nameof(cidr));
                }
                uintNetmask = null;
                return;
            }

            if (family != AddressFamily.InterNetwork
                && family != AddressFamily.InterNetworkV6)
            {
                throw new NotSupportedException(family.ToString());
            }

            if (family == AddressFamily.InterNetwork)
            {
                uintNetmask = cidr == 0 ? 0 : 0xffffffff << (32 - cidr);
                return;
            }

            var mask = new BigInteger(new byte[]
            {
                0xff, 0xff, 0xff, 0xff,
                0xff, 0xff, 0xff, 0xff,
                0xff, 0xff, 0xff, 0xff,
                0xff, 0xff, 0xff, 0xff,
                0x00
            });

            var masked = cidr == 0 ? 0 : mask << (128 - cidr);
            var m = masked.ToByteArray();
            var bmask = new byte[17];
            var copy = m.Length > 16 ? 16 : m.Length;
            Array.Copy(m, 0, bmask, 0, copy);
            uintNetmask = new BigInteger(bmask);
        }

        #endregion

        #region ToCidr

        /// <summary>
        ///     Convert netmask to CIDR
        ///     255.255.255.0 -> 24
        ///     255.255.0.0   -> 16
        ///     255.0.0.0     -> 8
        /// </summary>
        /// <param name="netmask"></param>
        /// <returns></returns>
        private static byte ToCidr(BigInteger netmask, AddressFamily family)
        {
            byte? cidr = null;
            InternalToCidr(false, netmask, family, out cidr);
            return (byte) cidr;
        }

        /// <summary>
        ///     Convert netmask to CIDR
        ///     255.255.255.0 -> 24
        ///     255.255.0.0   -> 16
        ///     255.0.0.0     -> 8
        /// </summary>
        /// <param name="netmask"></param>
        /// <returns></returns>
        private static void InternalToCidr(bool tryParse, BigInteger netmask, AddressFamily family, out byte? cidr)
        {
            if (!ValidNetmask(netmask, family))
            {
                if (tryParse == false)
                {
                    throw new ArgumentException("netmask");
                }
                cidr = null;
                return;
            }

            var cidr2 = BitsSet(netmask, family);
            cidr = cidr2;
        }

        /// <summary>
        ///     Convert netmask to CIDR
        ///     255.255.255.0 -> 24
        ///     255.255.0.0   -> 16
        ///     255.0.0.0     -> 8
        /// </summary>
        /// <param name="netmask"></param>
        /// <returns></returns>
        public static byte ToCidr(IPAddress netmask)
        {
            byte? cidr = null;
            InternalToCidr(false, netmask, out cidr);
            return (byte) cidr;
        }

        /// <summary>
        ///     Convert netmask to CIDR
        ///     255.255.255.0 -> 24
        ///     255.255.0.0   -> 16
        ///     255.0.0.0     -> 8
        /// </summary>
        /// <param name="netmask"></param>
        /// <returns></returns>
        public static bool TryToCidr(IPAddress netmask, out byte? cidr)
        {
            byte? cidr2 = null;
            InternalToCidr(true, netmask, out cidr2);
            var parsed = cidr2 != null;
            cidr = cidr2;
            return parsed;
        }

        private static void InternalToCidr(bool tryParse, IPAddress netmask, out byte? cidr)
        {
            if (netmask == null)
            {
                if (tryParse == false)
                {
                    throw new ArgumentNullException(nameof(netmask));
                }
                cidr = null;
                return;
            }
            BigInteger? uintNetmask2 = null;
            var parsed = TryToBigInteger(netmask, out uintNetmask2);
            if (parsed == false)
            {
                if (tryParse == false)
                {
                    throw new ArgumentException("netmask");
                }
                cidr = null;
                return;
            }
            var uintNetmask = (BigInteger) uintNetmask2;

            byte? cidr2 = null;
            InternalToCidr(tryParse, uintNetmask, netmask.AddressFamily, out cidr2);
            cidr = cidr2;
        }

        #endregion

        #region ToNetmask

        /// <summary>
        ///     Convert CIDR to netmask
        ///     24 -> 255.255.255.0
        ///     16 -> 255.255.0.0
        ///     8 -> 255.0.0.0
        /// </summary>
        /// <see cref="http://snipplr.com/view/15557/cidr-class-for-ipv4/" />
        /// <param name="cidr"></param>
        /// <returns></returns>
        public static IPAddress ToNetmask(byte cidr, AddressFamily family)
        {
            IPAddress netmask = null;
            InternalToNetmask(false, cidr, family, out netmask);
            return netmask;
        }

        /// <summary>
        ///     Convert CIDR to netmask
        ///     24 -> 255.255.255.0
        ///     16 -> 255.255.0.0
        ///     8 -> 255.0.0.0
        /// </summary>
        /// <see cref="http://snipplr.com/view/15557/cidr-class-for-ipv4/" />
        /// <param name="cidr"></param>
        /// <returns></returns>
        public static bool TryToNetmask(byte cidr, AddressFamily family, out IPAddress netmask)
        {
            IPAddress netmask2 = null;
            InternalToNetmask(true, cidr, family, out netmask2);
            var parsed = netmask2 != null;
            netmask = netmask2;
            return parsed;
        }


        private static void InternalToNetmask(bool tryParse, byte cidr, AddressFamily family, out IPAddress netmask)
        {
            if (family != AddressFamily.InterNetwork
                && family != AddressFamily.InterNetworkV6)
            {
                if (tryParse == false)
                {
                    throw new ArgumentException("family");
                }
                netmask = null;
                return;
            }

            if (cidr < 0)
            {
                if (tryParse == false)
                {
                    throw new ArgumentOutOfRangeException(nameof(cidr));
                }
                netmask = null;
                return;
            }

            var maxCidr = family == AddressFamily.InterNetwork ? 32 : 128;
            if (cidr > maxCidr)
            {
                if (tryParse == false)
                {
                    throw new ArgumentOutOfRangeException(nameof(cidr));
                }
                netmask = null;
                return;
            }

            var mask = ToUint(cidr, family);
            var netmask2 = ToIpAddress(mask, family);
            netmask = netmask2;
        }

        #endregion

        #endregion

        #region utils

        #region BitsSet

        /// <summary>
        ///     Count bits set to 1 in netmask
        /// </summary>
        /// <see
        ///     cref="http://stackoverflow.com/questions/109023/best-algorithm-to-count-the-number-of-set-bits-in-a-32-bit-integer" />
        /// <param name="netmask"></param>
        /// <returns></returns>
        private static byte BitsSet(BigInteger netmask, AddressFamily family)
        {
            var s = netmask.ToBinaryString();
            return (byte) s.Replace("0", "")
                .ToCharArray()
                .Length;
        }

        private static BigInteger BigMask(byte b, int length)
        {
            var bytes = new List<byte>(length + 1);
            for (var i = 0; i < length; i++)
            {
                bytes.Add(b);
            }
            bytes.Add(0);

            return new BigInteger(bytes.ToArray());
        }

        /// <summary>
        ///     Count bits set to 1 in netmask
        /// </summary>
        /// <param name="netmask"></param>
        /// <returns></returns>
        public static uint BitsSet(IPAddress netmask)
        {
            var uintNetmask = ToBigInteger(netmask);
            uint bits = BitsSet(uintNetmask, netmask.AddressFamily);
            return bits;
        }

        #endregion

        #region ValidNetmask

        /// <summary>
        ///     return true if netmask is a valid netmask
        ///     255.255.255.0, 255.0.0.0, 255.255.240.0, ...
        /// </summary>
        /// <see cref="http://www.actionsnip.com/snippets/tomo_atlacatl/calculate-if-a-netmask-is-valid--as2-" />
        /// <param name="netmask"></param>
        /// <returns></returns>
        public static bool ValidNetmask(IPAddress netmask)
        {
            if (netmask == null)
            {
                throw new ArgumentNullException(nameof(netmask));
            }
            var uintNetmask = ToBigInteger(netmask);
            var valid = ValidNetmask(uintNetmask, netmask.AddressFamily);
            return valid;
        }

        private static bool ValidNetmask(BigInteger netmask, AddressFamily family)
        {
            if (family != AddressFamily.InterNetwork
                && family != AddressFamily.InterNetworkV6)
            {
                throw new ArgumentException("family");
            }

            var mask = family == AddressFamily.InterNetwork
                ? new BigInteger(0x0ffffffff)
                : new BigInteger(new byte[]
                {
                    0xff, 0xff, 0xff, 0xff,
                    0xff, 0xff, 0xff, 0xff,
                    0xff, 0xff, 0xff, 0xff,
                    0xff, 0xff, 0xff, 0xff,
                    0x00
                });

            var neg = ~netmask & mask;
            var isNetmask = ((neg + 1) & neg) == 0;
            return isNetmask;
        }

        #endregion

        #region ToIPAddress

        /// <summary>
        ///     Transform a uint ipaddress into IPAddress object
        /// </summary>
        /// <param name="ipaddress"></param>
        /// <returns></returns>
        public static IPAddress ToIpAddress(BigInteger ipaddress, AddressFamily family)
        {
            var width = family == AddressFamily.InterNetwork ? 4 : 16;
            var bytes = ipaddress.ToByteArray();
            var bytes2 = new byte[width];
            var copy = bytes.Length > width ? width : bytes.Length;
            Array.Copy(bytes, 0, bytes2, 0, copy);
            Array.Reverse(bytes2);

            var sized = Resize(bytes2, family);
            var ip = new IPAddress(sized);
            return ip;
        }

        private static byte[] Resize(byte[] bytes, AddressFamily family)
        {
            if (family != AddressFamily.InterNetwork
                && family != AddressFamily.InterNetworkV6)
            {
                throw new ArgumentException("family");
            }

            var width = family == AddressFamily.InterNetwork ? 4 : 16;

            if (bytes.Length > width)
            {
                throw new ArgumentException("bytes");
            }

            var result = new byte[width];
            Array.Copy(bytes, 0, result, 0, bytes.Length);
            return result;
        }

        #endregion

        #endregion

        #region contains

        /// <summary>
        ///     return true if ipaddress is contained in network
        /// </summary>
        /// <param name="network"></param>
        /// <param name="ipaddress"></param>
        /// <returns></returns>
        public static bool Contains(IpNetwork network, IPAddress ipaddress)
        {
            if (network == null)
            {
                throw new ArgumentNullException(nameof(network));
            }

            if (ipaddress == null)
            {
                throw new ArgumentNullException(nameof(ipaddress));
            }

            var uintNetwork = network._network;
            var uintBroadcast = network._broadcast;
            var uintAddress = ToBigInteger(ipaddress);

            var contains = uintAddress >= uintNetwork
                           && uintAddress <= uintBroadcast;

            return contains;
        }

        /// <summary>
        ///     return true is network2 is fully contained in network
        /// </summary>
        /// <param name="network"></param>
        /// <param name="network2"></param>
        /// <returns></returns>
        public static bool Contains(IpNetwork network, IpNetwork network2)
        {
            if (network == null)
            {
                throw new ArgumentNullException(nameof(network));
            }

            if (network2 == null)
            {
                throw new ArgumentNullException(nameof(network2));
            }

            var uintNetwork = network._network;
            var uintBroadcast = network._broadcast;

            var uintFirst = network2._network;
            var uintLast = network2._broadcast;

            var contains = uintFirst >= uintNetwork
                           && uintLast <= uintBroadcast;

            return contains;
        }

        #endregion

        #region IANA block

        private static IpNetwork _ianaAblockReserved;
        private static IpNetwork _ianaBblockReserved;
        private static IpNetwork _ianaCblockReserved;

        /// <summary>
        ///     10.0.0.0/8
        /// </summary>
        /// <returns></returns>
        public static IpNetwork IanaAblkReserved1
        {
            get
            {
                if (_ianaAblockReserved == null)
                {
                    _ianaAblockReserved = Parse("10.0.0.0/8");
                }
                return _ianaAblockReserved;
            }
        }

        /// <summary>
        ///     172.12.0.0/12
        /// </summary>
        /// <returns></returns>
        public static IpNetwork IanaBblkReserved1
        {
            get
            {
                if (_ianaBblockReserved == null)
                {
                    _ianaBblockReserved = Parse("172.16.0.0/12");
                }
                return _ianaBblockReserved;
            }
        }

        /// <summary>
        ///     192.168.0.0/16
        /// </summary>
        /// <returns></returns>
        public static IpNetwork IanaCblkReserved1
        {
            get
            {
                if (_ianaCblockReserved == null)
                {
                    _ianaCblockReserved = Parse("192.168.0.0/16");
                }
                return _ianaCblockReserved;
            }
        }

        /// <summary>
        ///     return true if ipaddress is contained in
        ///     IANA_ABLK_RESERVED1, IANA_BBLK_RESERVED1, IANA_CBLK_RESERVED1
        /// </summary>
        /// <param name="ipaddress"></param>
        /// <returns></returns>
        public static bool IsIANAReserved(IPAddress ipaddress)
        {
            if (ipaddress == null)
            {
                throw new ArgumentNullException(nameof(ipaddress));
            }

            return Contains(IanaAblkReserved1, ipaddress)
                   || Contains(IanaBblkReserved1, ipaddress)
                   || Contains(IanaCblkReserved1, ipaddress);
        }

        /// <summary>
        ///     return true if ipnetwork is contained in
        ///     IANA_ABLK_RESERVED1, IANA_BBLK_RESERVED1, IANA_CBLK_RESERVED1
        /// </summary>
        /// <param name="ipnetwork"></param>
        /// <returns></returns>
        public static bool IsIANAReserved(IpNetwork ipnetwork)
        {
            if (ipnetwork == null)
            {
                throw new ArgumentNullException(nameof(ipnetwork));
            }

            return Contains(IanaAblkReserved1, ipnetwork)
                   || Contains(IanaBblkReserved1, ipnetwork)
                   || Contains(IanaCblkReserved1, ipnetwork);
        }

        #endregion

        #region Subnet

        /// <summary>
        ///     Subnet a network into multiple nets of cidr mask
        ///     Subnet 192.168.0.0/24 into cidr 25 gives 192.168.0.0/25, 192.168.0.128/25
        ///     Subnet 10.0.0.0/8 into cidr 9 gives 10.0.0.0/9, 10.128.0.0/9
        /// </summary>
        /// <param name="ipnetwork"></param>
        /// <param name="cidr"></param>
        /// <returns></returns>
        public static IpNetworkCollection Subnet(IpNetwork network, byte cidr)
        {
            IpNetworkCollection ipnetworkCollection;
            InternalSubnet(false, network, cidr, out ipnetworkCollection);
            return ipnetworkCollection;
        }

        /// <summary>
        ///     Subnet a network into multiple nets of cidr mask
        ///     Subnet 192.168.0.0/24 into cidr 25 gives 192.168.0.0/25, 192.168.0.128/25
        ///     Subnet 10.0.0.0/8 into cidr 9 gives 10.0.0.0/9, 10.128.0.0/9
        /// </summary>
        /// <param name="ipnetwork"></param>
        /// <param name="cidr"></param>
        /// <returns></returns>
        public static bool TrySubnet(IpNetwork network, byte cidr, out IpNetworkCollection ipnetworkCollection)
        {
            IpNetworkCollection inc = null;
            InternalSubnet(true, network, cidr, out inc);
            if (inc == null)
            {
                ipnetworkCollection = null;
                return false;
            }

            ipnetworkCollection = inc;
            return true;
        }

        private static void InternalSubnet(bool trySubnet, IpNetwork network, byte cidr,
            out IpNetworkCollection ipnetworkCollection)
        {
            if (network == null)
            {
                if (trySubnet == false)
                {
                    throw new ArgumentNullException(nameof(network));
                }
                ipnetworkCollection = null;
                return;
            }

            var maxCidr = network.AddressFamily == AddressFamily.InterNetwork ? 32 : 128;
            if (cidr > maxCidr)
            {
                if (trySubnet == false)
                {
                    throw new ArgumentOutOfRangeException(nameof(cidr));
                }
                ipnetworkCollection = null;
                return;
            }

            if (cidr < network.Cidr)
            {
                if (trySubnet == false)
                {
                    throw new ArgumentException("cidr");
                }
                ipnetworkCollection = null;
                return;
            }

            ipnetworkCollection = new IpNetworkCollection(network, cidr);
        }

        #endregion

        #region Supernet

        /// <summary>
        ///     Supernet two consecutive cidr equal subnet into a single one
        ///     192.168.0.0/24 + 192.168.1.0/24 = 192.168.0.0/23
        ///     10.1.0.0/16 + 10.0.0.0/16 = 10.0.0.0/15
        ///     192.168.0.0/24 + 192.168.0.0/25 = 192.168.0.0/24
        /// </summary>
        /// <param name="network1"></param>
        /// <param name="network2"></param>
        /// <returns></returns>
        public static IpNetwork Supernet(IpNetwork network1, IpNetwork network2)
        {
            IpNetwork supernet;
            InternalSupernet(false, network1, network2, out supernet);
            return supernet;
        }

        /// <summary>
        ///     Try to supernet two consecutive cidr equal subnet into a single one
        ///     192.168.0.0/24 + 192.168.1.0/24 = 192.168.0.0/23
        ///     10.1.0.0/16 + 10.0.0.0/16 = 10.0.0.0/15
        ///     192.168.0.0/24 + 192.168.0.0/25 = 192.168.0.0/24
        /// </summary>
        /// <param name="network1"></param>
        /// <param name="network2"></param>
        /// <returns></returns>
        public static bool TrySupernet(IpNetwork network1, IpNetwork network2, out IpNetwork supernet)
        {
            IpNetwork outSupernet;
            InternalSupernet(true, network1, network2, out outSupernet);
            var parsed = outSupernet != null;
            supernet = outSupernet;
            return parsed;
        }

        private static void InternalSupernet(bool trySupernet, IpNetwork network1, IpNetwork network2,
            out IpNetwork supernet)
        {
            if (network1 == null)
            {
                if (trySupernet == false)
                {
                    throw new ArgumentNullException(nameof(network1));
                }
                supernet = null;
                return;
            }

            if (network2 == null)
            {
                if (trySupernet == false)
                {
                    throw new ArgumentNullException(nameof(network2));
                }
                supernet = null;
                return;
            }


            if (Contains(network1, network2))
            {
                supernet = new IpNetwork(network1._network, network1.AddressFamily, network1.Cidr);
                return;
            }

            if (Contains(network2, network1))
            {
                supernet = new IpNetwork(network2._network, network2.AddressFamily, network2.Cidr);
                return;
            }

            if (network1.Cidr != network2.Cidr)
            {
                if (trySupernet == false)
                {
                    throw new ArgumentException("cidr");
                }
                supernet = null;
                return;
            }

            var first = network1._network < network2._network ? network1 : network2;
            var last = network1._network > network2._network ? network1 : network2;

            /// Starting from here :
            /// network1 and network2 have the same cidr,
            /// network1 does not contain network2,
            /// network2 does not contain network1,
            /// first is the lower subnet
            /// last is the higher subnet


            if (first._broadcast + 1 != last._network)
            {
                if (trySupernet == false)
                {
                    throw new ArgumentOutOfRangeException(nameof(trySupernet));
                }
                supernet = null;
                return;
            }

            var uintSupernet = first._network;
            var cidrSupernet = (byte) (first.Cidr - 1);

            var networkSupernet = new IpNetwork(uintSupernet, first.AddressFamily, cidrSupernet);
            if (networkSupernet._network != first._network)
            {
                if (trySupernet == false)
                {
                    throw new ArgumentException("network");
                }
                supernet = null;
                return;
            }
            supernet = networkSupernet;
        }

        #endregion

        #region SupernetArray

        /// <summary>
        ///     Supernet a list of subnet
        ///     192.168.0.0/24 + 192.168.1.0/24 = 192.168.0.0/23
        ///     192.168.0.0/24 + 192.168.1.0/24 + 192.168.2.0/24 + 192.168.3.0/24 = 192.168.0.0/22
        /// </summary>
        /// <param name="ipnetworks"></param>
        /// <param name="supernet"></param>
        /// <returns></returns>
        public static IpNetwork[] Supernet(IpNetwork[] ipnetworks)
        {
            IpNetwork[] supernet;
            InternalSupernet(false, ipnetworks, out supernet);
            return supernet;
        }

        /// <summary>
        ///     Supernet a list of subnet
        ///     192.168.0.0/24 + 192.168.1.0/24 = 192.168.0.0/23
        ///     192.168.0.0/24 + 192.168.1.0/24 + 192.168.2.0/24 + 192.168.3.0/24 = 192.168.0.0/22
        /// </summary>
        /// <param name="ipnetworks"></param>
        /// <param name="supernet"></param>
        /// <returns></returns>
        public static bool TrySupernet(IpNetwork[] ipnetworks, out IpNetwork[] supernet)
        {
            var supernetted = InternalSupernet(true, ipnetworks, out supernet);
            return supernetted;
        }

        public static bool InternalSupernet(bool trySupernet, IpNetwork[] ipnetworks, out IpNetwork[] supernet)

        {
            if (ipnetworks == null)
            {
                if (trySupernet == false)
                {
                    throw new ArgumentNullException(nameof(ipnetworks));
                }
                supernet = null;
                return false;
            }

            if (ipnetworks.Length <= 0)
            {
                supernet = new IpNetwork[0];
                return true;
            }

            var supernetted = new List<IpNetwork>();
            var ipns = Array2List(ipnetworks);
            var current = List2Stack(ipns);
            var previousCount = 0;
            var currentCount = current.Count;

            while (previousCount != currentCount)
            {
                supernetted.Clear();
                while (current.Count > 1)
                {
                    var ipn1 = current.Pop();
                    var ipn2 = current.Peek();

                    IpNetwork outNetwork;
                    var success = TrySupernet(ipn1, ipn2, out outNetwork);
                    if (success)
                    {
                        current.Pop();
                        current.Push(outNetwork);
                    }
                    else
                    {
                        supernetted.Add(ipn1);
                    }
                }
                if (current.Count == 1)
                {
                    supernetted.Add(current.Pop());
                }

                previousCount = currentCount;
                currentCount = supernetted.Count;
                current = List2Stack(supernetted);
            }
            supernet = supernetted.ToArray();
            return true;
        }

        private static Stack<IpNetwork> List2Stack(List<IpNetwork> list)
        {
            var stack = new Stack<IpNetwork>();
            list.ForEach(delegate(IpNetwork ipn) { stack.Push(ipn); });
            return stack;
        }

        private static List<IpNetwork> Array2List(IpNetwork[] array)
        {
            var ipns = new List<IpNetwork>();
            ipns.AddRange(array);
            RemoveNull(ipns);
            ipns.Sort(delegate(IpNetwork ipn1, IpNetwork ipn2)
            {
                var networkCompare = ipn1._network.CompareTo(ipn2._network);
                if (networkCompare != 0) return networkCompare;
                var cidrCompare = ipn1.Cidr.CompareTo(ipn2.Cidr);
                return cidrCompare;
            });
            ipns.Reverse();

            return ipns;
        }

        private static void RemoveNull(List<IpNetwork> ipns)
        {
            ipns.RemoveAll(ipn => ipn == null);
        }

        #endregion

        #region WideSubnet

        public static IpNetwork WideSubnet(string start, string end)
        {
            if (string.IsNullOrEmpty(start))
            {
                throw new ArgumentNullException(nameof(start));
            }

            if (string.IsNullOrEmpty(end))
            {
                throw new ArgumentNullException(nameof(end));
            }

            IPAddress startIp;
            if (!IPAddress.TryParse(start, out startIp))
            {
                throw new ArgumentException("start");
            }

            IPAddress endIp;
            if (!IPAddress.TryParse(end, out endIp))
            {
                throw new ArgumentException("end");
            }

            if (startIp.AddressFamily != endIp.AddressFamily)
            {
                throw new NotSupportedException("MixedAddressFamily");
            }

            var ipnetwork = new IpNetwork(0, startIp.AddressFamily, 0);
            for (byte cidr = 32; cidr >= 0; cidr--)
            {
                var wideSubnet = Parse(start, cidr);
                if (!Contains(wideSubnet, endIp)) continue;
                ipnetwork = wideSubnet;
                break;
            }
            return ipnetwork;
        }

        public static bool TryWideSubnet(IpNetwork[] ipnetworks, out IpNetwork ipnetwork)
        {
            IpNetwork ipn;
            InternalWideSubnet(true, ipnetworks, out ipn);
            if (ipn == null)
            {
                ipnetwork = null;
                return false;
            }
            ipnetwork = ipn;
            return true;
        }

        public static IpNetwork WideSubnet(IpNetwork[] ipnetworks)
        {
            IpNetwork ipn = null;
            InternalWideSubnet(false, ipnetworks, out ipn);
            return ipn;
        }

        private static void InternalWideSubnet(bool tryWide, IpNetwork[] ipnetworks, out IpNetwork ipnetwork)
        {
            if (ipnetworks == null)
            {
                if (tryWide == false)
                {
                    throw new ArgumentNullException(nameof(ipnetworks));
                }
                ipnetwork = null;
                return;
            }


            var nnin = Array.FindAll(ipnetworks, ipnet => ipnet != null);

            if (nnin.Length <= 0)
            {
                if (tryWide == false)
                {
                    throw new ArgumentException("ipnetworks");
                }
                ipnetwork = null;
                return;
            }

            if (nnin.Length == 1)
            {
                var ipn0 = nnin[0];
                ipnetwork = ipn0;
                return;
            }

            Array.Sort(nnin);
            var nnin0 = nnin[0];
            var uintNnin0 = nnin0._ipaddress;

            var nninX = nnin[nnin.Length - 1];
            var ipaddressX = nninX.Broadcast;

            var family = ipnetworks[0].AddressFamily;
            if (ipnetworks.Any(ipnx => ipnx.AddressFamily != family))
            {
                throw new ArgumentException("MixedAddressFamily");
            }

            var ipn = new IpNetwork(0, family, 0);
            for (var cidr = nnin0.Cidr; cidr >= 0; cidr--)
            {
                var wideSubnet = new IpNetwork(uintNnin0, family, cidr);
                if (!Contains(wideSubnet, ipaddressX)) continue;
                ipn = wideSubnet;
                break;
            }

            ipnetwork = ipn;
        }

        #endregion

        #region TryGuessCidr

        /// <summary>
        ///     Class              Leading bits    Default netmask
        ///     A (CIDR /8)	       00           255.0.0.0
        ///     A (CIDR /8)	       01           255.0.0.0
        ///     B (CIDR /16)	   10           255.255.0.0
        ///     C (CIDR /24)       11 	        255.255.255.0
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="cidr"></param>
        /// <returns></returns>
        public static bool TryGuessCidr(string ip, out byte cidr)
        {
            IPAddress ipaddress;
            var parsed = IPAddress.TryParse($"{ip}", out ipaddress);
            if (parsed == false)
            {
                cidr = 0;
                return false;
            }

            if (ipaddress.AddressFamily == AddressFamily.InterNetworkV6)
            {
                cidr = 64;
                return true;
            }
            var uintIpAddress = ToBigInteger(ipaddress);
            uintIpAddress = uintIpAddress >> 29;
            if (uintIpAddress <= 3)
            {
                cidr = 8;
                return true;
            }
            if (uintIpAddress <= 5)
            {
                cidr = 16;
                return true;
            }
            if (uintIpAddress <= 6)
            {
                cidr = 24;
                return true;
            }

            cidr = 0;
            return false;
        }

        /// <summary>
        ///     Try to parse cidr. Have to be >= 0 and <= 32 or 128
        /// </summary>
        /// <param name="sidr"></param>
        /// <param name="cidr"></param>
        /// <returns></returns>
        public static bool TryParseCidr(string sidr, AddressFamily family, out byte? cidr)
        {
            byte b = 0;
            if (!byte.TryParse(sidr, out b))
            {
                cidr = null;
                return false;
            }

            IPAddress netmask;
            if (!TryToNetmask(b, family, out netmask))
            {
                cidr = null;
                return false;
            }

            cidr = b;
            return true;
        }

        #endregion

        /**
         * Need a better way to do it
         * 
        #region TrySubstractNetwork

        public static bool TrySubstractNetwork(IPNetwork[] ipnetworks, IPNetwork substract, out IEnumerable<IPNetwork> result) {

            if (ipnetworks == null) {
                result = null;
                return false;
            }
            if (ipnetworks.Length <= 0) {
                result = null;
                return false;
            }
            if (substract == null) {
                result = null;
                return false;
            }
            var results = new List<IPNetwork>();
            foreach (var ipn in ipnetworks) {
                if (!Overlap(ipn, substract)) {
                    results.Add(ipn);
                    continue;
                }

                var collection = IPNetwork.Subnet(ipn, substract.Cidr);
                var rtemp = new List<IPNetwork>();
                foreach(var subnet in collection) {
                    if (subnet != substract) {
                        rtemp.Add(subnet);
                    }
                }
                var supernets = Supernet(rtemp.ToArray());
                results.AddRange(supernets);
            }
            result = results;
            return true;
        }
        #endregion
         * **/

        #region IComparable<IPNetwork> Members

        public static int Compare(IpNetwork left, IpNetwork right)
        {
            //  two null IPNetworks are equal
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null)) return 0;

            //  two same IPNetworks are equal
            if (ReferenceEquals(left, right)) return 0;

            //  null is always sorted first
            if (ReferenceEquals(left, null)) return -1;
            if (ReferenceEquals(right, null)) return 1;

            //  first test the network
            var result = left._network.CompareTo(right._network);
            if (result != 0) return result;

            //  then test the cidr
            result = left.Cidr.CompareTo(right.Cidr);
            return result;
        }

        public int CompareTo(IpNetwork other)
        {
            return Compare(this, other);
        }

        public int CompareTo(object obj)
        {
            //  null is at less
            if (obj == null) return 1;

            //  convert to a proper Cidr object
            var other = obj as IpNetwork;

            //  type problem if null
            if (other == null)
            {
                throw new ArgumentException(
                    "The supplied parameter is an invalid type. Please supply an IPNetwork type.",
                    nameof(obj));
            }

            //  perform the comparision
            return CompareTo(other);
        }

        #endregion

        #region IEquatable<IPNetwork> Members

        public static bool Equals(IpNetwork left, IpNetwork right)
        {
            return Compare(left, right) == 0;
        }

        public bool Equals(IpNetwork other)
        {
            return Equals(this, other);
        }

        public override bool Equals(object obj)
        {
            return Equals(this, obj as IpNetwork);
        }

        #endregion

        #region Operators

        public static bool operator ==(IpNetwork left, IpNetwork right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(IpNetwork left, IpNetwork right)
        {
            return !Equals(left, right);
        }

        public static bool operator <(IpNetwork left, IpNetwork right)
        {
            return Compare(left, right) < 0;
        }

        public static bool operator >(IpNetwork left, IpNetwork right)
        {
            return Compare(left, right) > 0;
        }

        #endregion
    }
}