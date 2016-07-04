using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace System.Net.IPNetwork {
    public class IpNetworkCollection : IEnumerable<IpNetwork>, IEnumerator<IpNetwork> {

        private BigInteger _enumerator;
        private readonly byte _cidrSubnet;
        private readonly IpNetwork _ipnetwork;

        private byte Cidr => _ipnetwork.Cidr;

        private BigInteger Broadcast => IpNetwork.ToBigInteger(_ipnetwork.Broadcast);

        private BigInteger LastUsable => IpNetwork.ToBigInteger(_ipnetwork.LastUsable);

        private BigInteger Network => IpNetwork.ToBigInteger(_ipnetwork.Network);

        internal IpNetworkCollection(IpNetwork ipnetwork, byte cidrSubnet) {

            var maxCidr = ipnetwork.AddressFamily == Sockets.AddressFamily.InterNetwork ? 32 : 128;
            if (cidrSubnet > maxCidr) {
                throw new ArgumentOutOfRangeException(nameof(cidrSubnet));
            }

            if (cidrSubnet < ipnetwork.Cidr) {
                throw new ArgumentException("cidr");
            }

            _cidrSubnet = cidrSubnet;
            _ipnetwork = ipnetwork;
            _enumerator = -1;
        }

        #region Count, Array, Enumerator

        public BigInteger Count
        {
            get
            {
                var count = BigInteger.Pow(2, _cidrSubnet - Cidr);
                return count; 
            }
        }

        public IpNetwork this[BigInteger i] {
            get
            {
                if (i >= Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(i));
                }

                var last = _ipnetwork.AddressFamily == Sockets.AddressFamily.InterNetworkV6
                    ? LastUsable : Broadcast;
                var increment = (last - Network) / Count;
                var uintNetwork = Network + ((increment + 1) * i);
                var ipn = new IpNetwork(uintNetwork, _ipnetwork.AddressFamily, _cidrSubnet);
                return ipn;
            }
        }

        #endregion

        #region IEnumerable Members

        IEnumerator<IpNetwork> IEnumerable<IpNetwork>.GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        #region IEnumerator<IPNetwork> Members

        public IpNetwork Current => this[_enumerator];

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            // nothing to dispose
        }

        #endregion

        #region IEnumerator Members

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            _enumerator++;
            return _enumerator < Count;
        }

        public void Reset()
        {
            _enumerator = -1;
        }

        #endregion

        #endregion

    }
}
