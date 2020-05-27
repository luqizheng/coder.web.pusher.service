using Coder.WebPusher;
using Xunit;

namespace XUnitTestProject1
{
    public class FileUtilityTest
    {
        [Fact]
        public void Test()
        {
            var file = "kjk/kjk\\kjk2?";
            var actual = FileUtility.GetStandardFileName(file);
            Assert.Equal("kjk_kjk_kjk2_", actual);
        }
    }
}