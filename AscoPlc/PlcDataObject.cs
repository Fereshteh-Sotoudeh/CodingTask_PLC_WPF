using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using PlcWrapper.ASCOPlc;
using PlcWrapper.Interfaces;

namespace ASCOPlc
{

    #region Plc interface implementation
    public class PlcDataObject<T_PLC> : IPlcDataObject where T_PLC : Enum
    {
        internal IAscoPlcDataObject<T_PLC> ascoPlcObj;

        public object DataObj => ascoPlcObj;
        public T_PLC Device => ascoPlcObj.Device;

        public Type VarType => ascoPlcObj.Attr.VarType;

        public string Name => ascoPlcObj.Device.GetPlcAdress();
        public string Description => ascoPlcObj.Attr.Description;

        public object OldValue => ascoPlcObj.OldValue;

        public object ValueToWrite { get => ascoPlcObj.ValueToWrite; set => ascoPlcObj.ValueToWrite = value; }

        public object Value => ascoPlcObj.Value;

        public bool IsCylicReading { get => ascoPlcObj.Attr.ReadMode == AscoPlcReadMode.Monitor; set => throw new NotImplementedException(); }

        public event EventHandler ValueChanged { add => ascoPlcObj.ValueChanged += value; remove => ascoPlcObj.ValueChanged -= value; }

        public void ReadValue()
        {
            MockAscoPlc<T_PLC>.Instance.ReadValue(this);
        }

        public virtual void WriteValue()
        {
            MockAscoPlc<T_PLC>.Instance.WriteDelayed(this);
        }

        public void UpdateErrorOrWarn() => ascoPlcObj.TriggerErrorOrWarn();
    }

    #endregion

    #region AscoPlc interface
    internal interface IAscoPlcDataObject<T> where T : Enum
    {
        T Device { get; }
        AscoPlcAttribute Attr { get; }

        void OnWriteSuccess();
        Type GetDataType();
        void TriggerErrorOrWarn();
        void MockReadValue();
        object OldValue { get; }
        object Value { get; }
        object ValueToWrite { get; set; }

        event EventHandler ValueChanged;
        bool TriggerValueChangedOnWrites { get; set; }

        void OnValueChanged();
    }
    #endregion

    #region AscoPlc interface implementation
    internal class AscoPlcDataObject<T, T_PLC> : IAscoPlcDataObject<T_PLC> where T_PLC : Enum
    {
        public T_PLC Device { get; private set; }
        public AscoPlcAttribute Attr { get; private set; }

        private T value;
        public T valueToWrite;
        private T oldValue;

        object IAscoPlcDataObject<T_PLC>.OldValue => oldValue;

        object IAscoPlcDataObject<T_PLC>.Value => value;
        object IAscoPlcDataObject<T_PLC>.ValueToWrite { get => valueToWrite; set => valueToWrite = (T)Convert.ChangeType(value, typeof(T)); }

        public event EventHandler ValueChanged;
        public bool TriggerValueChangedOnWrites { get; set; }


        public AscoPlcDataObject(T_PLC dev)
        {
            Device = dev;
            Attr = Device.Get();

            if (Attr.ReadMode == AscoPlcReadMode.Monitor)
                Task.Run(async () =>
                {
                    while (true)
                    {
                        await Task.Delay(2000);
                        MockReadValue();
                    }
                });
        }

        private void AscoPlcDataObject_ValueChanged(object sender, EventArgs e)
        {
            UpdateErrorOrWarnFromValueChanged();
        }

        public void TriggerErrorOrWarn()
        {
            // not relevant for coding task
        }

        private void UpdateErrorOrWarnFromValueChanged()
        {
            // not relevant for coding task
        }

        public void OnValueChanged()
        {
            ValueChanged?.Invoke(this, null);
        }

        public void OnWriteSuccess()
        {
            oldValue = value;
            value = valueToWrite;
            if (TriggerValueChangedOnWrites)
                ValueChanged?.Invoke(this, null);
        }

        public Type GetDataType()
        {
            return typeof(T);
        }

        // simulates a aread operation on the plc object
        public void MockReadValue()
        {
            switch (Device.Get().VarType.Name)
            {
                case "Boolean":
                    bool newBool = (DateTime.Now.Second % 2) == 1 ? true : false;
                    value = (T)Convert.ChangeType(newBool, typeof(T));
                    break;
                case "Int32":
                    int newInt = DateTime.Now.Second % 32000;
                    value = (T)Convert.ChangeType(newInt, typeof(T));
                    break;
                default:
                    throw new Exception("Type not supported by PLC.");
            }
            Console.WriteLine($"Device: {Attr.Description} value: {value}");
            OnValueChanged();
        }
    }
    #endregion
}
