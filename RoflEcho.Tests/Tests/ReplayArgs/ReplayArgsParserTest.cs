using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RoflEcho.Tests
{
    [TestClass]
    public class ReplayArgsParserTest
    {
        [TestMethod]
        public void CanParseCommandLine()
        {
            const string executablePath = @"A:\Test Directory\A.exe";
            const string replayFilePath = @"B:\Rofl Directory\B.rofl";
            const string extraArgument1 = "-argument1";
            const string extraArgument2 = "-argument2=abc";

            var (args, error) = ReplayArgsParser.Parse(@$"""{executablePath}"" ""{replayFilePath}"" ""{extraArgument1}"" ""{extraArgument2}""");

            Assert.AreEqual(ReplayArgsParserError.None, error);
            Assert.IsNotNull(args);
            Assert.AreEqual(executablePath, args.Executable.FullName);
            Assert.AreEqual(replayFilePath, args.ReplayFile.FullName);

            Assert.AreEqual(2, args.ExtraArgs.Length);
            Assert.AreEqual(extraArgument1, args.ExtraArgs[0]);
            Assert.AreEqual(extraArgument2, args.ExtraArgs[1]);
        }

        [TestMethod]
        public void ShouldReturnErrorWhenFirstArgumentIsNotExecutable()
        {
            const string executablePath = @"A:\Test Directory\A.dummy";
            const string replayFilePath = @"B:\Rofl Directory\B.rofl";

            var (_, error) = ReplayArgsParser.Parse(@$"""{executablePath}"" ""{replayFilePath}""");

            Assert.AreEqual(ReplayArgsParserError.FirstArgumentIsNotExe, error);
        }

        [TestMethod]
        public void ShouldReturnErrorWhenSecondArgumentIsNotReplayFile()
        {
            const string executablePath = @"A:\Test Directory\A.exe";
            const string replayFilePath = @"B:\Rofl Directory\B.dummy";

            var (_, error) = ReplayArgsParser.Parse(@$"""{executablePath}"" ""{replayFilePath}""");

            Assert.AreEqual(ReplayArgsParserError.SecondArgumentIsNotRofl, error);
        }

        [TestMethod]
        public void ShouldReturnErrorWhenArgumentIsNotEnough()
        {
            const string executablePath = @"A:\Test Directory\A.exe";

            var (_, error) = ReplayArgsParser.Parse(@$"""{executablePath}""");

            Assert.AreEqual(ReplayArgsParserError.LackOfArguments, error);
        }

        [TestMethod]
        public void ShouldIgnoreExtensionCase()
        {
            const string executablePath = @"A:\Test Directory\A.ExE";
            const string replayFilePath = @"B:\Rofl Directory\B.RoFl";

            var (_, error) = ReplayArgsParser.Parse(@$"""{executablePath}"" ""{replayFilePath}""");

            Assert.AreEqual(ReplayArgsParserError.None, error);
        }
    }
}
