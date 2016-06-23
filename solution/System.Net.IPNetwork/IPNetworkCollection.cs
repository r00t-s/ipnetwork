using System.Collections.Generic;
using System.Collections;
using System.Numerics;

namespace System.Net {
    public class IPNetworkCollection : IEnumerable<IPNetwork>, IEnumerator<IPNetwork> {

        private BigInteger _enumerator;
        private readonly byte _cidrSubnet;
        private readonly IPNetwork _ipnetwork;

        private byte _cidr => _ipnetwork.Cidr;

        private BigInteger _broadcast => IPNetwork.ToBigInteger(_ipnetwork.Broadcast);

        private BigInteger _lastUsable => IPNetwork.ToBigInteger(_ipnetwork.LastUsable);

        private BigInteger _network => IPNetwork.ToBigInteger(_ipnetwork.Network);

        internal IPNetworkCollection(IPNetwork ipnetwork, byte cidrSubnet) {

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
                var count = BigInteger.Pow(2, _cidrSubnet - _cidr);
                return count; 
            }
        }

        public IPNetwork this[BigInteger i] {
            get
            {
                if (i >= Count)
                {
                    throw new ArgumentOutOfRangeException("i");
                }

                var last = _ipnetwork.AddressFamily == Sockets.AddressFamily.InterNetworkV6
                    ? _lastUsable : _broadcast;
                var increment = (last - _network) / Count;
                var uintNetwork = _network + ((increment + 1) * i);
                var ipn = new IPNetwork(uintNetwork, _ipnetwork.AddressFamily, _cidrSubnet);
                return ipn;
            }
        }

        #endregion

        #region IEnumerable Members

        IEnumerator<IPNetwork> IEnumerable<IPNetwork>.GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        #region IEnumerator<IPNetwork> Members

        public IPNetwork Current => this[_enumerator];

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
