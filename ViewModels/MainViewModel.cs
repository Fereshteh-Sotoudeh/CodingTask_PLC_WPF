using ASCOPlc;
using CodingTask.ViewModels;
using PlcWrapper.ASCOPlc;
using PlcWrapper.MitsuiQR;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CodingTask
{
    public class MainViewModel
    {
        private ObservableCollection<PlcVariableViewModel> _plcVariables;     
        public ObservableCollection<PlcVariableViewModel> PlcVariables
        {
            get { return _plcVariables; }
            set { _plcVariables = value;   }
        }
      
        public MainViewModel()
        {
            PlcVariables = new ObservableCollection<PlcVariableViewModel>();
     
            foreach (var v in ASCOPlc.MockAscoPlc.Instance.Variables)
            {              
                var dataModel = new PlcVariableDataModel
                {
                    Variable = v.Key,
                    Description = v.Value.Description,
                    Value= v.Value.Value,
                };

                PlcVariables.Add(new PlcVariableViewModel(dataModel));
            }

        }
    }
}
