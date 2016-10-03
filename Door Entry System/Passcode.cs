using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Door_Entry_System
{
    class Passcode
    {
        private int maxLen = 9;
        private int minLen = 0;
        public int[] passcode;
        public Passcode(Int32[] code)
        {
            //calling the validate password function
            //if it returns true set the passcode
            if (validatePasscode(code))
            {
                this.passcode = code;
            }
        }
        private Boolean validatePasscode(Int32[] passcode)
        {
            return (passcode.Length >= minLen && passcode.Length <= maxLen) && passcode.All(z => z >= 0 && z <= 9);
        }
    }
}
