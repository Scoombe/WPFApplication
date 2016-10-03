using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Door_Entry_System.ViewModels
{
    interface ILockViewModel
    {
        bool IsSubmitAvailable { get; }
        bool IsAddAvailable { get; }
        bool IsDeleteAvailable { get; }
        ICommand DeleteDigitCommand { get; }
        ICommand AddDigitCommand { get; }
        ICommand SubmitCommand { get; }
    }
}
