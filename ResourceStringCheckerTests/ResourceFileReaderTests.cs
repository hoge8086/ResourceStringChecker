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
    public class ResourceFileReaderTests
    {
        [TestMethod()]
        public void FindStringTest()
        {
            var reader = new ResourceFileReader(new ResourceFile() { FilePath = "resource.rc", Language = Language.Japanease, Encoding = "shift-jis" });
            var str = reader.FindString("IDC_BUTTON3");
        }
        [TestMethod()]
        public void FindStringTest2()
        {
            var reader = new ResourceFileReader(new ResourceFile() { FilePath = "resource.rc", Language = Language.Japanease, Encoding = "shift-jis" });
            var str = reader.FindString("IDD_ABOUTBOX.IDC_STATIC");
        }
    }
}