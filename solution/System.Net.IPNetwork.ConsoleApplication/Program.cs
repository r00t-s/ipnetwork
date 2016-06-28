using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Numerics;
using System.Reflection;
using Gnu.Getopt;

namespace System.Net.ConsoleApplication
{
    public class Program
    {
        private static readonly Dictionary<int, ArgParsed> Args = new Dictionary<int, ArgParsed>();

        private static readonly ArgParsed[] ArgsList =
        {
            new ArgParsed('i', delegate(ProgramContext ac, string arg) { ac.IPNetwork = true; }),
            new ArgParsed('n', delegate(ProgramContext ac, string arg) { ac.Network = true; }),
            new ArgParsed('m', delegate(ProgramContext ac, string arg) { ac.Netmask = true; }),
            new ArgParsed('c', delegate(ProgramContext ac, string arg) { ac.Cidr = true; }),
            new ArgParsed('b', delegate(ProgramContext ac, string arg) { ac.Broadcast = true; }),
            new ArgParsed('f', delegate(ProgramContext ac, string arg) { ac.FirstUsable = true; }),
            new ArgParsed('l', delegate(ProgramContext ac, string arg) { ac.LastUsable = true; }),
            new ArgParsed('u', delegate(ProgramContext ac, string arg) { ac.Usable = true; }),
            new ArgParsed('t', delegate(ProgramContext ac, string arg) { ac.Total = true; }),
            new ArgParsed('w', delegate(ProgramContext ac, string arg) { ac.Action = ActionEnum.Supernet; }),
            new ArgParsed('W', delegate(ProgramContext ac, string arg) { ac.Action = ActionEnum.WideSupernet; }),
            new ArgParsed('h', delegate(ProgramContext ac, string arg) { ac.Action = ActionEnum.Usage; }),
            new ArgParsed('x', delegate(ProgramContext ac, string arg) { ac.Action = ActionEnum.ListIPAddress; }),
            new ArgParsed('?', delegate { }),
            new ArgParsed('D', delegate(ProgramContext ac, string arg) { ac.CidrParse = CidrParseEnum.Default; }),
            new ArgParsed('d', delegate(ProgramContext ac, string arg)
            {
                byte? cidr;
                if (!IPNetwork.TryParseCidr(arg, AddressFamily.InterNetwork, out cidr))
                {
                    Console.WriteLine("Invalid cidr {0}", cidr);
                    ac.Action = ActionEnum.Usage;
                    return;
                }
                ac.CidrParse = CidrParseEnum.Value;
                ac.CidrParsed = (byte) cidr;
            }),
            new ArgParsed('s', delegate(ProgramContext ac, string arg)
            {
                byte? cidr;
                if (!IPNetwork.TryParseCidr(arg, AddressFamily.InterNetwork, out cidr))
                {
                    Console.WriteLine("Invalid cidr {0}", cidr);
                    ac.Action = ActionEnum.Usage;
                    return;
                }

                ac.Action = ActionEnum.Subnet;
                ac.SubnetCidr = (byte) cidr;
            }),
            new ArgParsed('C', delegate(ProgramContext ac, string arg)
            {
                IPNetwork ipnetwork;
                if (!TryParseIPNetwork(arg, ac.CidrParse, ac.CidrParsed, out ipnetwork))
                {
                    Console.WriteLine("Unable to parse ipnetwork {0}", arg);
                    ac.Action = ActionEnum.Usage;
                    return;
                }

                ac.Action = ActionEnum.ContainNetwork;
                ac.ContainNetwork = ipnetwork;
            }),
            new ArgParsed('o', delegate(ProgramContext ac, string arg)
            {
                IPNetwork ipnetwork;
                if (!TryParseIPNetwork(arg, ac.CidrParse, ac.CidrParsed, out ipnetwork))
                {
                    Console.WriteLine("Unable to parse ipnetwork {0}", arg);
                    ac.Action = ActionEnum.Usage;
                    return;
                }

                ac.Action = ActionEnum.OverlapNetwork;
                ac.OverlapNetwork = ipnetwork;
            }),
            new ArgParsed('S', delegate(ProgramContext ac, string arg)
            {
                IPNetwork ipnetwork;
                if (!TryParseIPNetwork(arg, ac.CidrParse, ac.CidrParsed, out ipnetwork))
                {
                    Console.WriteLine("Unable to parse ipnetwork {0}", arg);
                    ac.Action = ActionEnum.Usage;
                    return;
                }

                ac.Action = ActionEnum.SubstractNetwork;
                ac.SubstractNetwork = ipnetwork;
            })
        };


        static Program()
        {
            foreach (var ap in ArgsList)
            {
                Args.Add(ap.Arg, ap);
            }
        }

        public static void testme()
        {

            var ipnetwork = IPNetwork.Parse("192.168.0.0/24");
            const byte cidr = 32;

            var subnets = IPNetwork.Subnet(ipnetwork, cidr);
            var index = 0;
            foreach (var subnet in subnets)
            {
                Console.WriteLine($"IPNetwork.Subnet #{index}={subnet}");
                index++;
            }
        }

        public static void Main(string[] args)
        {
            var ac = ParseArgs(args);

            switch (ac.Action)
            {
                case ActionEnum.Subnet:
                    SubnetNetworks(ac);
                    break;
                case ActionEnum.Supernet:
                    SupernetNetworks(ac);
                    break;
                case ActionEnum.WideSupernet:
                    WideSupernetNetworks(ac);
                    break;
                case ActionEnum.PrintNetworks:
                    PrintNetworks(ac);
                    break;
                case ActionEnum.ContainNetwork:
                    ContainNetwork(ac);
                    break;
                case ActionEnum.OverlapNetwork:
                    OverlapNetwork(ac);
                    break;
                case ActionEnum.ListIPAddress:
                    ListIPAddress(ac);
                    break;
                /**
             * Need a better way to do it
             * 
            } else if (ac.Action == ActionEnum.SubstractNetwork) {
                Program.SubstractNetwork(ac);
                 * 
            */
                default:
                    testme();
                    Usage();
                    break;
            }
        }

        private static void ListIPAddress(ProgramContext ac)
        {
            foreach (var ipaddress in ac.Networks.SelectMany(IPNetwork.ListIPAddress))
            {
                Console.WriteLine(ipaddress);
            }
        }

        private static void ContainNetwork(ProgramContext ac)
        {
            foreach (var ipnetwork in ac.Networks)
            {
                var contain = IPNetwork.Contains(ac.ContainNetwork, ipnetwork);
                Console.WriteLine($"{ac.ContainNetwork} contains {ipnetwork} : {contain}");
            }
        }

        private static void OverlapNetwork(ProgramContext ac)
        {
            foreach (var ipnetwork in ac.Networks)
            {
                var overlap = IPNetwork.Overlap(ac.OverlapNetwork, ipnetwork);
                Console.WriteLine($"{ac.OverlapNetwork} overlaps {ipnetwork} : {overlap}");
            }
        }

        /**
         * Need a better way to do it
         * 
        private static void SubstractNetwork(ProgramContext ac) {
            
            IEnumerable<IPNetwork> result = null;
            if (!IPNetwork.TrySubstractNetwork(ac.Networks, ac.SubstractNetwork, out result)) {
                Console.WriteLine("Unable to substract subnet from these networks");
            }
            
            foreach (IPNetwork ipnetwork in result.OrderBy( s => s.ToString() )) {
                Console.WriteLine("{0}", ipnetwork);
                //Program.PrintNetwork(ac, ipnetwork);
            }
        }
        **/

        private static void WideSupernetNetworks(ProgramContext ac)
        {
            IPNetwork widesubnet;
            if (!IPNetwork.TryWideSubnet(ac.Networks, out widesubnet))
            {
                Console.WriteLine("Unable to wide subnet these networks");
            }
            PrintNetwork(ac, widesubnet);
        }

        private static void SupernetNetworks(ProgramContext ac)
        {
            IPNetwork[] supernet;
            if (!IPNetwork.TrySupernet(ac.Networks, out supernet))
            {
                Console.WriteLine("Unable to supernet these networks");
            }
            PrintNetworks(ac, supernet, supernet.Length);
        }

        private static void PrintNetworks(ProgramContext ac, IEnumerable<IPNetwork> ipnetworks, BigInteger networkLength)
        {
            var i = 0;
            foreach (var ipn in ipnetworks)
            {
                i++;
                PrintNetwork(ac, ipn);
                PrintSeparator(networkLength, i);
            }
        }

        private static void SubnetNetworks(ProgramContext ac)
        {
            BigInteger i = 0;
            foreach (var ipnetwork in ac.Networks)
            {
                i++;
                var networkLength = ac.Networks.Length;
                IPNetworkCollection ipnetworks;
                if (!IPNetwork.TrySubnet(ipnetwork, ac.SubnetCidr, out ipnetworks))
                {
                    Console.WriteLine("Unable to subnet ipnetwork {0} into cidr {1}", ipnetwork, ac.SubnetCidr);
                    PrintSeparator(networkLength, i);
                    continue;
                }

                PrintNetworks(ac, ipnetworks, ipnetworks.Count);
                PrintSeparator(networkLength, i);
            }
        }

        //private static void PrintSeparator(Array network, int index) {
        //    if (network.Length > 1 && index != network.Length) {
        //        Console.WriteLine("--");
        //    }
        //}
        private static void PrintSeparator(BigInteger max, BigInteger index)
        {
            if (max > 1 && index != max)
            {
                Console.WriteLine("--");
            }
        }

        private static void PrintNetworks(ProgramContext ac)
        {
            var i = 0;
            foreach (var ipnetwork in ac.Networks)
            {
                i++;
                PrintNetwork(ac, ipnetwork);
                PrintSeparator(ac.Networks.Length, i);
            }
        }

        private static void PrintNetwork(ProgramContext ac, IPNetwork ipn)
        {
            var sw = new StringWriter();
            if (ac.IPNetwork) sw.WriteLine($"IPNetwork   : {ipn}");
            if (ac.Network) sw.WriteLine($"Network     : {ipn.Network}");
            if (ac.Netmask) sw.WriteLine($"Netmask     : {ipn.Netmask}");
            if (ac.Cidr) sw.WriteLine($"Cidr        : {ipn.Cidr}");
            if (ac.Broadcast) sw.WriteLine($"Broadcast   : {ipn.Broadcast}");
            if (ac.FirstUsable) sw.WriteLine($"FirstUsable : {ipn.FirstUsable}");
            if (ac.LastUsable) sw.WriteLine($"LastUsable  : {ipn.LastUsable}");
            if (ac.Usable) sw.WriteLine($"Usable      : {ipn.Usable}");
            if (ac.Total) sw.WriteLine($"Total       : {ipn.Total}");
            Console.Write(sw.ToString());
        }


        private static ProgramContext ParseArgs(string[] args)
        {
            int c;
            var g = new Getopt("ipnetwork", args, "inmcbfltud:Dhs:wWxC:o:S:");
            var ac = new ProgramContext();

            while ((c = g.getopt()) != -1)
            {
                var optArg = g.Optarg;
                Args[c].Run(ac, optArg);
            }

            var ipnetworks = new List<string>();
            for (var i = g.Optind; i < args.Length; i++)
            {
                if (!string.IsNullOrEmpty(args[i]))
                {
                    ipnetworks.Add(args[i]);
                }
            }
            ac.NetworksString = ipnetworks.ToArray();
            ParseIPNetworks(ac);

            if (ac.Networks.Length == 0)
            {
                Console.WriteLine("Provide at least one ipnetwork");
                ac.Action = ActionEnum.Usage;
            }

            if (ac.Action == ActionEnum.Supernet
                && ipnetworks.Count < 2)
            {
                Console.WriteLine("Supernet action required at least two ipnetworks");
                ac.Action = ActionEnum.Usage;
            }

            if (ac.Action == ActionEnum.WideSupernet
                && ipnetworks.Count < 2)
            {
                Console.WriteLine("WideSupernet action required at least two ipnetworks");
                ac.Action = ActionEnum.Usage;
            }

            if (PrintNoValue(ac))
            {
                PrintAll(ac);
            }

            if (g.Optind == 0)
            {
                PrintAll(ac);
            }

            return ac;
        }

        private static void ParseIPNetworks(ProgramContext ac)
        {
            var ipnetworks = new List<IPNetwork>();
            foreach (var ips in ac.NetworksString)
            {
                IPNetwork ipnetwork;
                if (!TryParseIPNetwork(ips, ac.CidrParse, ac.CidrParsed, out ipnetwork))
                {
                    Console.WriteLine("Unable to parse ipnetwork {0}", ips);
                    continue;
                }
                ipnetworks.Add(ipnetwork);
            }
            ac.Networks = ipnetworks.ToArray();
        }

        private static bool TryParseIPNetwork(string ip, CidrParseEnum cidrParseEnum, byte cidr, out IPNetwork ipn)
        {
            IPNetwork ipnetwork = null;
            switch (cidrParseEnum)
            {
                case CidrParseEnum.Default:
                    if (!IPNetwork.TryParse(ip, out ipnetwork))
                    {
                        ipn = null;
                        return false;
                    }
                    break;
                case CidrParseEnum.Value:
                    if (!IPNetwork.TryParse(ip, cidr, out ipnetwork))
                    {
                        if (!IPNetwork.TryParse(ip, out ipnetwork))
                        {
                            ipn = null;
                            return false;
                        }
                    }
                    break;
            }
            ipn = ipnetwork;
            return true;
        }


        private static bool PrintNoValue(ProgramContext ac)
        {
            if (ac == null)
            {
                throw new ArgumentNullException(nameof(ac));
            }

            return ac.IPNetwork == false
                   && ac.Network == false
                   && ac.Netmask == false
                   && ac.Cidr == false
                   && ac.Broadcast == false
                   && ac.FirstUsable == false
                   && ac.LastUsable == false
                   && ac.Total == false
                   && ac.Usable == false;
        }

        private static void PrintAll(ProgramContext ac)
        {
            if (ac == null)
            {
                throw new ArgumentNullException(nameof(ac));
            }

            ac.IPNetwork = true;
            ac.Network = true;
            ac.Netmask = true;
            ac.Cidr = true;
            ac.Broadcast = true;
            ac.FirstUsable = true;
            ac.LastUsable = true;
            ac.Usable = true;
            ac.Total = true;
        }

        private static void Usage()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            var version = fvi.FileVersion;

            Console.WriteLine(
                "Usage: ipnetwork [-inmcbflu] [-d cidr|-D] [-h|-s cidr|-S|-w|-W|-x|-C network|-o network] networks ...");
            Console.WriteLine($"Version: {version}");
            Console.WriteLine();
            Console.WriteLine("Print options");
            Console.WriteLine("\t-i : network");
            Console.WriteLine("\t-n : network address");
            Console.WriteLine("\t-m : netmask");
            Console.WriteLine("\t-c : cidr");
            Console.WriteLine("\t-b : broadcast");
            Console.WriteLine("\t-f : first usable ip address");
            Console.WriteLine("\t-l : last usable ip address");
            Console.WriteLine("\t-u : number of usable ip addresses");
            Console.WriteLine("\t-t : total number of ip addresses");
            Console.WriteLine();
            Console.WriteLine("Parse options");
            Console.WriteLine("\t-d cidr : use cidr if not provided (default /32)");
            Console.WriteLine("\t-D      : IPv4 only - use default cidr (ClassA/8, ClassB/16, ClassC/24)");
            Console.WriteLine();
            Console.WriteLine("Actions");
            Console.WriteLine("\t-h         : help message");
            Console.WriteLine("\t-s cidr    : split network into cidr subnets");
            Console.WriteLine("\t-w         : supernet networks into smallest possible subnets");
            Console.WriteLine("\t-W         : supernet networks into one single subnet");
            Console.WriteLine("\t-x         : list all ipadresses in networks");
            Console.WriteLine("\t-C network : network contain networks");
            Console.WriteLine("\t-o network : network overlap networks");
            Console.WriteLine("\t-S network : substract network from subnet");
            Console.WriteLine("");
            Console.WriteLine("networks  : one or more network addresses ");
            Console.WriteLine(
                "            (1.2.3.4 10.0.0.0/8 10.0.0.0/255.0.0.0 2001:db8::/32 2001:db8:1:2:3:4:5:6/128 )");
        }
    }
}