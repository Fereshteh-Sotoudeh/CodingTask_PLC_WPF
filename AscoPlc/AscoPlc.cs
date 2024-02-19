using PlcWrapper.ASCOPlc;
using PlcWrapper.MitsuiQR;
using System;
using System.Collections.Generic;

namespace ASCOPlc
{
    public class MockAscoPlc<T_PLC> where T_PLC : Enum
    {
        public static MockAscoPlc<T_PLC> Instance { get; internal set; } = new MockAscoPlc<T_PLC>();
        public Dictionary<T_PLC, PlcDataObject<T_PLC>> Variables = new Dictionary<T_PLC, PlcDataObject<T_PLC>>();

        private MockAscoPlc()
        {
            foreach (T_PLC e in Enum.GetValues(typeof(T_PLC)))
            {
                var plcObj = CreatePlcObject(e);
                Variables.Add(e, plcObj);
            }
        }

        static PlcDataObject<T_PLC> CreatePlcObject(T_PLC v)
        {
            var plcObj = new PlcDataObject<T_PLC>();
            switch (v.Get().VarType.Name)
            {
                case "Boolean":
                    plcObj.ascoPlcObj = new AscoPlcDataObject<bool, T_PLC>(v);
                    break;
                case "Int32":
                    plcObj.ascoPlcObj = new AscoPlcDataObject<Int32, T_PLC>(v);
                    break;
                case "Int16":
                    plcObj.ascoPlcObj = new AscoPlcDataObject<Int16, T_PLC>(v);
                    break;
                case "UInt16":
                    plcObj.ascoPlcObj = new AscoPlcDataObject<UInt16, T_PLC>(v);
                    break;
                case "UInt32":
                    plcObj.ascoPlcObj = new AscoPlcDataObject<UInt32, T_PLC>(v);
                    break;
                case "Single":
                    plcObj.ascoPlcObj = new AscoPlcDataObject<float, T_PLC>(v);
                    break;
                default:
                    throw new Exception("Type not supported by PLC.");
            }

            return plcObj;
        }


        internal void ReadValue(PlcDataObject<T_PLC> plcDataObject)
        {
            plcDataObject.ascoPlcObj.MockReadValue();
        }

        internal void WriteValue(PlcDataObject<T_PLC> plcDataObject)
        {
            throw new NotImplementedException();
        }

        internal void WriteDelayed(PlcDataObject<T_PLC> plcDataObject)
        {
            throw new NotImplementedException();
        }
    }


    public class MockAscoPlc
    {
        public static MockAscoPlc<MitsuRVariables> Instance => MockAscoPlc<MitsuRVariables>.Instance;
    }

    public class PlcDataObject : PlcDataObject<MitsuRVariables> { }

}