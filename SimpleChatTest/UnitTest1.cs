using Chat;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleChatTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void FindClientModeSuccess()
        {
            string[] cmd = new string[] { "/m", "client", "/p", "8989", "/a", "10.10.10.10" };
            CmdParser parser = new CmdParser(cmd);
            parser.Parse();
            Assert.AreEqual(CmdParser.MODE.CLIENT, parser.mode);
        }

        [TestMethod]
        public void FindClientModeFailed()
        {
            string[] cmd = new string[] { "client", "/p", "8989", "/a", "10.10.10.10" };
            CmdParser parser = new CmdParser(cmd);
            parser.Parse();
            Assert.AreNotEqual(CmdParser.MODE.CLIENT, parser.mode);
        }

        [TestMethod]
        public void FindServerModeSuccess()
        {
            string[] cmd = new string[] { "/m", "server", "/p", "8989", "/a", "10.10.10.10" };
            CmdParser parser = new CmdParser(cmd);
            parser.Parse();
            Assert.AreEqual(CmdParser.MODE.SERVER, parser.mode);
        }

        [TestMethod]
        public void FindServerModeFailed()
        {
            string[] cmd = new string[] { "/m", "client", "/p", "8989", "/a", "10.10.10.10" };
            CmdParser parser = new CmdParser(cmd);
            parser.Parse();
            Assert.AreNotEqual(CmdParser.MODE.SERVER, parser.mode);
        }

        [TestMethod]
        public void FindPortSuccess()
        {
            string[] cmd = new string[] { "/m", "client", "/p", "8989", "/a", "10.10.10.10" };
            CmdParser parser = new CmdParser(cmd);
            parser.Parse();
            Assert.IsTrue(parser.port != -1);
        }

        [TestMethod]
        public void FindPortFailed()
        {
            string[] cmd = new string[] { "/m", "client", "8989", "/a", "10.10.10.10" };
            CmdParser parser = new CmdParser(cmd);
            parser.Parse();
            Assert.IsTrue(parser.port == -1);
        }

        [TestMethod]
        public void FindAddressSuccess()
        {
            string[] cmd = new string[] { "/m", "client", "/p", "8989", "/a", "10.10.10.10" };
            CmdParser parser = new CmdParser(cmd);
            parser.Parse();
            Assert.AreEqual("10.10.10.10", parser.address);
        }

        [TestMethod]
        public void FindAddressFailed()
        {
            string[] cmd = new string[] { "/m", "client", "/p", "8989", "10.10.10.10" };
            CmdParser parser = new CmdParser(cmd);
            parser.Parse();
            Assert.AreNotEqual("10.10.10.10", parser.address);
        }

        [TestMethod]
        public void CheckUnnecessaryParamForServer()
        {
            string[] cmd = new string[] { "/m", "client", "/p", "8989" };
            CmdParser parser = new CmdParser(cmd);
            parser.Parse();
            Assert.IsTrue(parser.address == string.Empty);
        }
    }
}
