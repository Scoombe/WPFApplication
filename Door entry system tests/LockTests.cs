using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Door_Entry_System;
using Door_Entry_System.ViewModels;
using System.Collections.Generic;
using Door_Entry_System.Utilities;
using System.IO;

namespace Door_entry_system_tests
{
    [TestClass]
    public class LockTests
    {
        [TestMethod]
        //test for the right code added
        //using the lock code
        public void rightCode()
        {
            // creating a new lock
            Lock mylock = new Lock();
            // updating the lock;
            List<int> code = new List<int>();
            code.AddRange(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            mylock.updateLock(code);
            //checking if the status is true
            Assert.IsTrue(mylock.status);
        }
        [TestMethod]
        //test for the wrong code
        //using the lock class
        public void wrongCode()
        {
            //creating new lock
            Lock myLock = new Lock();
            //updating the lock
            List<int> code = new List<int>();
            code.AddRange(new Int32[] { 1, 2, 3, 4, 5, 6, 7 });
            var lockReturn = myLock.updateLock(code);
            Assert.IsFalse(lockReturn);
        }

        [TestMethod]
        //test for adding a new digit
        public void addDigitTest()
        {
            LockViewModel viewModal = new LockViewModel();
            viewModal.AddDigitCommand.Execute(1);
            Assert.AreEqual(viewModal.CurrentMessage, "*"); 
         }

        [TestMethod]
        //test for deleting a digit
        public void deleteDigitTest()
        {
            LockViewModel viewModal = new LockViewModel();
            viewModal.AddDigitCommand.Execute(1);
            viewModal.AddDigitCommand.Execute(2);
            viewModal.AddDigitCommand.Execute(3);
            viewModal.DeleteDigitCommand.Execute(1);
            Assert.AreEqual(viewModal.CurrentMessage, "**");

        }
        [TestMethod]
        //test method for checking the entered code
        public void codeCheck()
        {
            LockViewModel viewModal = new LockViewModel();
            //adding the digits
            for (int i = 1; i <= 3; i++)
            {
                viewModal.AddDigitCommand.Execute(i);
            }
            //expected strings
            string expected = "123";
            string actual = "";
            //converting the curent code to an array
            int[] viewCodes = viewModal.CurrentCode.ToArray();
            //foreach of the codes, add the int to the actual string
            foreach (int code in viewCodes) { actual += code.ToString(); }
            //checking that the strings are equal.
            Assert.AreEqual(expected,actual);
        }
        [TestMethod]
        //test for a correct code submit
        //checking that the locked boolean changed
        public void correctSubmit()
        {
            LockViewModel viewModal = new LockViewModel();
            for (int i = 1; i <= 9; i++)
            {
                viewModal.AddDigitCommand.Execute(i);
            }
            //executing the submit comand after the digits have been added in the code above.
            viewModal.SubmitCommand.Execute(1);
            //checking if the locked = false
            Assert.IsFalse(viewModal.Locked);
        }
        [TestMethod]
        //test for 2 correct code submits
        //checking that the locked boolean changed back to locked
        public void LockedTest()
        {
            LockViewModel viewModal = new LockViewModel();
            for (int i = 0; i < 2; i++)
            {
                for (int j = 1; j <= 9; j++)
                {
                    viewModal.AddDigitCommand.Execute(j);
                }
                //executing the submit comand after the digits have been added in the code above.
                viewModal.SubmitCommand.Execute(1);
            }
            //checking if the locked = True
            //meaning that it has locked again
            Assert.IsTrue(viewModal.Locked);
        }
        [TestMethod]
        //test for the disabling of the lock after 3 tries
        public void DisabledLock()
        {
            LockViewModel viewModal = new LockViewModel();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    viewModal.AddDigitCommand.Execute(j);
                }
                viewModal.SubmitCommand.Execute(1);
            }
            Assert.IsTrue(viewModal.Disabled);
        }
        [TestMethod]
        //test for the disabling of the lock after 3 tries
        public void DisabledLockCommand()
        {
            LockViewModel viewModal = new LockViewModel();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    viewModal.AddDigitCommand.Execute(j);
                }
                viewModal.SubmitCommand.Execute(1);
            }
            Assert.IsFalse(viewModal.IsAddAvailable);
            Assert.IsFalse(viewModal.IsDeleteAvailable);
            Assert.IsFalse(viewModal.IsSubmitAvailable);
            Assert.AreEqual(false,viewModal.IsCommandAvailable);   
        }
        [TestMethod]
        //test for all of the logger functions
        public void loggerTest()
        {
            string testfile = "testLog.txt";
            SysLog logger = new SysLog(testfile);

            List<int> codes = new List<int>(new int[] { 1, 2, 3, 4, 5, 6, 7 });
            logger.attempt(codes,false);
            logger.attempt(codes, false);
            logger.attempt(codes, false);
            logger.disabled();
            Assert.IsTrue(File.Exists(testfile));
        }

    }
}
