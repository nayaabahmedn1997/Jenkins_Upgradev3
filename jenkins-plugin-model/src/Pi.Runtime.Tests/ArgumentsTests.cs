using NUnit.Framework;
using PowerArgs;

namespace Pi.Runtime.Tests
{
    [TestFixture]
    public class ArgumentsTests
    {
        [Test]
        public void Defaults()
        {
            var arguments = Args.Parse<Arguments>(new string[0]);
            Assert.That(arguments.DecimalPlaces, Is.EqualTo(20));
            Assert.That(arguments.Mode, Is.EqualTo(RunMode.Console));
            Assert.That(arguments.OutputPath, Is.EqualTo("/pi.txt"));
        }

        [Test]
        public void DecimalPlaces()
        {
            var arguments = Args.Parse<Arguments>(new string[] { "-dp", "10" });
            Assert.That(arguments.DecimalPlaces, Is.EqualTo(10));
        }

        [Test]
        public void Unknown()
        {
            var arguments = Args.Parse<Arguments>(new string[] { "-unk", "own"});
            Assert.That(arguments, Is.Not.Null);
        }
    }
}