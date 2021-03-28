using Microsoft.VisualStudio.TestTools.UnitTesting;
using ResourceStringChecker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceStringChecker.Tests
{
    [TestClass()]
    public class CheckResultTests
    {
        [TestMethod()]
        public void StringCompareTest()
        {
            int ret;
            Assert.AreEqual(true, CheckResult.StringCompare("abc", "abc", out ret));
        }
        [TestMethod()]
        public void StringCompareTest2()
        {
            int ret;
            Assert.AreEqual(false, CheckResult.StringCompare("abc", "Abc", out ret));
            Assert.AreEqual(1, ret);
        }
        [TestMethod()]
        public void StringCompareTest3()
        {
            int ret;
            Assert.AreEqual(false, CheckResult.StringCompare("abc", "aBc", out ret));
            Assert.AreEqual(2, ret);
        }
        [TestMethod()]
        public void StringCompareTest4()
        {
            int ret;
            Assert.AreEqual(false, CheckResult.StringCompare("abcd", "abc", out ret));
            Assert.AreEqual(4, ret);
        }
        [TestMethod()]
        public void StringCompareTest5()
        {
            int ret;
            Assert.AreEqual(false, CheckResult.StringCompare("abc", "abcd", out ret));
            Assert.AreEqual(4, ret);
        }
        [TestMethod()]
        public void StringCompareTest6()
        {
            int ret;
            Assert.AreEqual(false, CheckResult.StringCompare("", "abcd", out ret));
            Assert.AreEqual(1, ret);
        }
        [TestMethod()]
        public void StringCompareTest7()
        {
            int ret;
            Assert.AreEqual(true, CheckResult.StringCompare("", "", out ret));
        }
    }
}