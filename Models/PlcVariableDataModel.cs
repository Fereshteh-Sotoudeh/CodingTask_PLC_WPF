using PlcWrapper.MitsuiQR;
using System;

namespace CodingTask
{
    public class PlcVariableDataModel
    {
        public MitsuRVariables Variable { get; set; }
        public object Value { get; set; }
        public string Description { get; set; }
        public Type VarType { get; set; }
    }

}

