﻿namespace System.Net.IPNetwork.ConsoleApplication {
    public class ProgramContext {

        public bool IpNetwork;
        public bool Network;
        public bool Netmask;
        public bool Cidr;
        public bool Broadcast;
        public bool FirstUsable;
        public bool LastUsable;
        public bool Usable;
        public bool Total;

        public CidrParseEnum CidrParse = CidrParseEnum.Value;
        public byte CidrParsed = 32;

        public IpNetwork ContainNetwork;
        public IpNetwork OverlapNetwork;
        public IpNetwork SubstractNetwork;

        public ActionEnum Action = ActionEnum.PrintNetworks;
        public byte SubnetCidr;

        public string[] NetworksString;
        public IpNetwork[] Networks;



    }
}
