using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Door_Entry_System.Utilities
{
    //class inplementing the Icommand interface
    class Command : ICommand
    {
        #region fields
        readonly Action<object> _action;
        readonly Predicate<object> _CanExecute;
        #endregion

        #region constructor
        //contructor that passes  an action and a predicate
        public Command(Action<object> execute) : this(execute, (Object o) => { return true; })
        {
        }
        //Chained contructor
        public Command(Action<object> action, Predicate<object> CanExecute)
        {
            //if there isn't an action passed in the throw exception because the action is null
            if (action == null)
            {
                throw new ArgumentException("action is null");
            }
            //otherwise the private action is equal to the passed action
            else
            {
                _action = action;
                _CanExecute = CanExecute;
            }
        }
        #endregion

        #region ICommand Memebers
        //determining wether or not the object can execute or not
        public bool CanExecute(object parameter)
        {
            if (parameter != null)
            {
                return _CanExecute(parameter);
            }
            else
            {
                return true;
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        //pasing the parameter to the action<object> running the object
        public void Execute(object parameter)
        {
            _CanExecute(parameter);
            if (parameter != null)
            {
                _action(parameter);
            }
        }
        #endregion
    }
}
