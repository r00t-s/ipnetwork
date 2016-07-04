using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace System.Net.IPNetwork {
    public class IpAddressCollection : IEnumerable<IPAddress>, IEnumerator<IPAddress> {

        private readonly IpNetwork _ipnetwork;
        private BigInteger _enumerator;

        internal IpAddressCollection(IpNetwork ipnetwork) {
            _ipnetwork = ipnetwork;
            _enumerator = -1;
        }


        #region Count, Array, Enumerator

        public BigInteger Count => _ipnetwork.Total;

        public IPAddress this[BigInteger i] {
            get {
                if (i >= Count) {
                    throw new ArgumentOutOfRangeException(nameof(i));
                }
                var width = _ipnetwork.AddressFamily == Sockets.AddressFamily.InterNetwork ? (byte)32 : (byte)128;
                var ipn = IpNetwork.Subnet(_ipnetwork, width);
                return ipn[i].Network;
            }
        }

        #endregion

        #region IEnumerable Members

        IEnumerator<IPAddress> IEnumerable<IPAddress>.GetEnumerator() {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this;
        }

        #region IEnumerator<IPNetwork> Members

        public IPAddress Current => this[_enumerator];

        #endregion

        #region IDisposable Members

        public void Dispose() {
            // nothing to dispose
        }

        #endregion

        #region IEnumerator Members

        object IEnumerator.Current => Current;

        public bool MoveNext() {
            _enumerator++;
            return _enumerator < Count;
        }

        public void Reset() {
            _enumerator = -1;
        }

        #endregion

        #endregion
    }
}
