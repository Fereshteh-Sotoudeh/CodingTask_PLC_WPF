using System;
using System.Linq;
using System.Reflection;

namespace PlcWrapper.ASCOPlc
{
    public enum AscoPlcReadMode { OnDemand, Monitor };
    public enum AscoPlcWriteMode { Async, Sync };
    public enum AscoPlcWordSize { Single, Double };

    public static class EnumExtensions
    {
        public static string GetPlcAdress(this Enum enumValue)
        {
            return enumValue.ToString();
        }

        public static AscoPlcAttribute Get(this Enum enumValue)
        {
            Type enumType = enumValue.GetType();
            MemberInfo info = enumType.GetMember(enumValue.ToString()).First();

            if (info != null && info.CustomAttributes.Any())
            {
                AscoPlcAttribute nameAttr = info.GetCustomAttribute<AscoPlcAttribute>();
                return nameAttr;
            }
            return null;
        }
    }

    public class AscoPlcAttribute : Attribute
    {
        public Type VarType { get; }
        public AscoPlcReadMode ReadMode { get; } = AscoPlcReadMode.OnDemand;
        public AscoPlcWriteMode WriteMode { get; } = AscoPlcWriteMode.Async;
        public AscoPlcWordSize WordSize { get; set; } = AscoPlcWordSize.Single;
        public string Description { get; }
        public object plcObject;

        public AscoPlcAttribute(Type varType, AscoPlcReadMode readMode, string descr) 
            : this(varType, readMode, AscoPlcWriteMode.Async, descr) { }
        public AscoPlcAttribute(Type varType, AscoPlcReadMode readMode, AscoPlcWriteMode writeMode, string descr)
        {
            VarType = varType;
            ReadMode = readMode;
            WriteMode = writeMode;
            Description = descr;
            switch (VarType.Name)
            {
                case "Int32":
                case "UInt32":
                    WordSize = AscoPlcWordSize.Double;
                    break;
             }
        }

    }

}
