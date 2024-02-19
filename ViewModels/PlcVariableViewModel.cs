using PlcWrapper.ASCOPlc;
using PlcWrapper.Interfaces;
using System;
using System.Windows.Media;

namespace CodingTask.ViewModels
{
    public class PlcVariableViewModel
    {
        private PlcVariableDataModel _dataModel;
    

        public PlcVariableViewModel(PlcVariableDataModel dataModel)
        {
            _dataModel = dataModel;          
        }
    
        public SolidColorBrush BackgroundColor => new SolidColorBrush(GetBackgroundColor());
        public string Address => _dataModel.Variable.GetPlcAdress();
        public string Description => _dataModel.Description;
        public object Value => _dataModel.Value;

        //determines the background color based on the Value property
        private Color GetBackgroundColor()
        {
            if (Value == null)
            {
                return Colors.Transparent;
            }

            if (Value is bool boolValue)
            {
                return boolValue ? Colors.Green : Colors.Red;
            }
            return Colors.Transparent;
            
        }
    }
}
