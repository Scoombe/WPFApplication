using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Door_Entry_System
{
    public class Lock
    {
        //all of the valid passwords for this lock
        private Passcode[] passwords = new Passcode[4]
        {
             new Passcode(new Int32[9] {0,0,0,0,0,0,0,0,0}),
             new Passcode(new Int32[9] {1,2,3,4,5,6,7,8,9}),
             new Passcode(new Int32[9] {9,9,9,9,9,9,9,9,9}),
             new Passcode(new Int32[6] {0,0,0,0,0,0 })
        };
        // variable for if the lock is locked or unlocked
        public Boolean status { get; private set; } = false;
        //function for checking the entered codes to the lock codes. 
        public bool updateLock(List<Int32> enteredCode)
        {
            //if the entered code is less then 6
            if (enteredCode.Count >= 6 || enteredCode.Count <= 9)
            {
                // for all of the codes checking that one of them is equal to the entered code. 
                foreach (Passcode code in this.passwords)
                {
                    //reversing the locked status
                    if (enteredCode.SequenceEqual(code.passcode)) {
                        this.status = !this.status;
                        return true;
                    }
                }
             }
             return false;
        }
    }
}
