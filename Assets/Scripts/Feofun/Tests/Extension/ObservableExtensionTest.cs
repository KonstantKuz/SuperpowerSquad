using Feofun.Extension;
using NUnit.Framework;
using UniRx;

namespace Feofun.Tests.Extension
{
    public class ObservableExtensionTest
    {
        [Test]
        public void Diff()
        {
            float value = 0;
            var source = new FloatReactiveProperty(0);
            source.Diff().Subscribe(it => value = it);
            Assert.That(value, Is.EqualTo(0));

            source.Value = 1;
            Assert.That(value, Is.EqualTo(1));
            
            source.Value = 3;
            Assert.That(value, Is.EqualTo(2));

            source.Value = 2;
            Assert.That(value, Is.EqualTo(-1));
        }
    }
}