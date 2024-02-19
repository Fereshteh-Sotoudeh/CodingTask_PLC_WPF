using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlcWrapper.Interfaces
{
    public interface IPlcDataObject
    {
        string Name { get; }
        object OldValue { get; }
        object ValueToWrite { get; set; }
        object Value { get; }
        bool IsCylicReading { get; set; }
        object DataObj { get; }
        string Description { get; }

        event EventHandler ValueChanged;

        void ReadValue();
        string ToString();
        void WriteValue();
        void UpdateErrorOrWarn();
    }
}
