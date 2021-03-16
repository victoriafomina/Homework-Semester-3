using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GUIForFTP
{
    public class MyCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action<object> act;
        private Func<object, bool> canExecute;

        public MyCommand(Action<object> act, Func<object, bool> canExecute)
        {
            this.act = act;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            act(parameter);
        }
    }
}
