using Door_Entry_System.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace Door_Entry_System.ViewModels
{
    public class LockViewModel : ILockViewModel, INotifyPropertyChanged
    {
        #region classVariables
        private Lock viewLock;
        private int _failedAttempts;
        private readonly SynchronizationContext _context = System.Threading.SynchronizationContext.Current;
        private List<Int32> _CurrentCode;
        private string _CurrentMessage;
        private ICommand _DeleteDigitCommand;
        private ICommand _AddDigitCommand;
        private ICommand _SubmitCommand;
        private ICommand _HelpMessageCommand;
        public bool _IsDeleteAvailable;
        public bool _IsAddAvailable;
        public bool _IsSubmitAvailable;
        private bool _CommandAvailable;
        private bool _Disabled;
        private bool _Locked;
        private SysLog logger;
        #endregion

        #region constructor
        public LockViewModel()
        {
            CurrentCode = new List<Int32>();
            viewLock = new Lock();
            Locked = true;
            failedAttempts = 0;
            ChangeButtonStatus(true);
            AddDigitCommand = new Command(Param => AddDigit(Param), Param => IsAddAvailable);
            DeleteDigitCommand = new Command(Param => DeleteDigit(), Param => IsDeleteAvailable);
            SubmitCommand = new Command(Param => Submit(), Param => IsSubmitAvailable);
            HelpMessageCommnad = new Command(Param => ShowMessage());
            logger = new SysLog();
            
        }
        #endregion

        #region properties
        //property for how many failed attempts the lock has recieved.
        private int failedAttempts
        {
            get { return _failedAttempts; }
            set { _failedAttempts = value;
                OnPropertyChanged("FailedAttempts");
            }
        }
        //property for wether or not the system is locked
        public bool Locked
        {
            get { return _Locked; }
            set {
                _Locked = value;
                OnPropertyChanged("Locked");
            }
        }
        //property for when the code has been entered incorrectly entered 3 times
        public bool Disabled
        {
            get { return _Disabled; }
            set { _Disabled = value;
                OnPropertyChanged("Disabled");
            }
        }
        //property for the current entered code
        public List<Int32> CurrentCode
        {
            get { return _CurrentCode; }
            set {
                _CurrentCode = value;
                OnPropertyChanged("CurrentCode");
            }
        }
        //property for the messgae to be displayed in the textbox
        public string CurrentMessage
        {
            get { return _CurrentMessage; }
            private set
            {
                _CurrentMessage = value;
                OnPropertyChanged("CurrentMessage");
            }
        }
        //property for returning stars of the length of the curentCode
        public string CodeLen
        {
            get
            {
                //returning stars for the length of the _currentCode
                string stars = string.Empty;
                for (int i = 0; i < CurrentCode.Count; i++)
                { stars += "*"; }
                return stars;
            }
        }

        #endregion

        #region command properties

        //property for checking if the add digit command is avaiable
        public Boolean IsAddAvailable
        {
            get
            {
                return _IsAddAvailable && CurrentCode.Count < 9;
            }
            set
            {
                _IsAddAvailable = value;
                OnPropertyChanged("AddDigit");
            }
        }
        public ICommand AddDigitCommand
        {
            get
            {
                if (_AddDigitCommand == null)
                    _AddDigitCommand = new Command(param => AddDigit(param), param => IsCommandAvailable);
                return _AddDigitCommand;
            }
            set
            {
                _AddDigitCommand = value;
            }
        }

        //public command for the help message that is brought up when f1 is pressed
        public ICommand HelpMessageCommnad
        {
            get
            {
                if (_HelpMessageCommand == null)
                    _HelpMessageCommand = new Command(param => ShowMessage());
                return _HelpMessageCommand;
            }
            set
            {
                _HelpMessageCommand = value;
            }
        }

        //public boolean for if the avaibility of the delete command
        public Boolean IsDeleteAvailable
        {
            get { return CurrentCode.Count > 0 && _IsDeleteAvailable; }
            set {
                OnPropertyChanged("IsDeleteAvailable");
                _IsDeleteAvailable = value; }
        }

        public ICommand DeleteDigitCommand
        {
            get
            {
                if (_DeleteDigitCommand == null)
                    _DeleteDigitCommand = new Command(param => DeleteDigit(), param => IsCommandAvailable);
                return _DeleteDigitCommand; }
            set
            {
                _DeleteDigitCommand = value;
            }
        }

        public bool IsCommandAvailable
        {
            get { return _CommandAvailable; }
            set { _CommandAvailable = value;
                OnPropertyChanged("CommandAvailable");
            }
        }
        //public boolean for the availability of the cubmit command 
        public Boolean IsSubmitAvailable
        {
            get { return _IsSubmitAvailable && CurrentCode.Count > 5 && CurrentCode.Count <= 9; }
            set {
                OnPropertyChanged("IsSubmitAvailable");
                _IsSubmitAvailable = value; }
        }

        public ICommand SubmitCommand
        {
            get
            {
                if (_SubmitCommand == null)
                {
                    _SubmitCommand = new Command(param => Submit(), param => IsCommandAvailable);
                }
                return _SubmitCommand;
            }
            set
            {
                _SubmitCommand = value;
            }
        }
        #endregion

        #region functions
        //function for deleting a digit called by the delete digit command
        private void DeleteDigit()
        {
            //removing from the code
            CurrentCode.RemoveAt(CurrentCode.Count - 1);
            ChangeCode();
        }
        //function for adding a digit to the current code and outputting it into the text box
        //passed an object from the add digit command
        private void AddDigit(object digit) {
            Int32 num;
            //creating a string 
            string digitStr = digit.ToString();
            Int32.TryParse(digitStr, out num);
            CurrentCode.Add(num);
            ChangeCode();
        }
        //function that is called after the execute on the Submit command is called
        private void Submit()
        {
            bool codeSubmit = viewLock.updateLock(CurrentCode);
            logger.attempt(CurrentCode, codeSubmit);
            if (codeSubmit)
            {
                //switching the bool value of the locked property
                Locked = !Locked;
                failedAttempts = 0;
            }
            else
            {
                CurrentMessage = "INVALID";
                failedAttempts++;
            }
            CurrentCode = new List<int>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        //rasing a new  generic property changed event
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                if (propertyName != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            if (propertyName == "Locked")
            {
                showState();
            }
            //when you create a new currentcode list, for a submit then this code
            //will run and make the current message to be blank overwriting the invalid or Unlocked
            if (propertyName == "CurrentCode" && CodeLen != "")
            {
                CurrentMessage = CodeLen;
            }
            if (propertyName == "FailedAttempts")
            {
                if (failedAttempts == 3)
                {
                    Disabled = true;
                }
            }
            if (propertyName == "Disabled")
            {
                if (Disabled == true)
                {
                    DisableLock();
                }
            }
        }
        public void showState()
        {
            if (Locked)
            {
                CurrentMessage = "LOCKED";
            }
            else
            {
                CurrentMessage = "UNLOCKED";
            }
        }
        private void ChangeCode()
        {
            CurrentMessage = CodeLen;
        }
        //method for changing the is command available to disabled
        private void DisableLock()
        {
            CurrentMessage = "DISABLED";
            logger.disabled();
            IsCommandAvailable = false;
            ChangeButtonStatus(false);
            System.Timers.Timer disableLockTimer = new System.Timers.Timer();
            disableLockTimer.Interval = 10000;
            disableLockTimer.Start();
            disableLockTimer.Elapsed += (sender, e) => {
                disableLockTimer.Stop();

                Application.Current.Dispatcher.Invoke(new Action(() => 
                {
                    //calling the change state method
                    ChangeButtonStatus(true);
                    //setting the failed attempts to default
                    failedAttempts = 0;
                    CurrentCode = new List<int>();
                    showState();
                    //not working the can execute requery isn't working
                    CommandManager.InvalidateRequerySuggested();
                }));
            };
        }
        //the help message funcion for showing the help mesage when f1 is pressed
        public void ShowMessage()
        {
            MessageBox.Show("Valid code => 'INVALID' shown in the display.\n\n3 possible codes - Min 6 digits, max 9 digits\n\nValid code reverses status flag and 'LOCKED' OR 'UNLOCKED' shown.\n\nThree invalid codes => buttons ignored for 10 seconds  and 'DISABLED' shown.", "Help Message");
        }
        public void ChangeButtonStatus(bool status)
        {
            IsSubmitAvailable = status;
            IsDeleteAvailable = status;
            IsAddAvailable = status;
        }
        #endregion
    }
}
