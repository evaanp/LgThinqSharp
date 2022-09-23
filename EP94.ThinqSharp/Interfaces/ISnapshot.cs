using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EP94.ThinqSharp.Interfaces
{
    public interface ISnapshot
    {
        public event EventHandler? ValueChanged;
        // TODO: common properties of different devices

        bool ValuesUpToDate { get; }
        internal bool ClientConnected { get; set; }

        void Merge(Dictionary<string, object> snapshot);
    }
}
