using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class TestExample : UnitTestBase
    {
        public TestExample() : base()
        {

        }

        [Fact]
        public void Example()
        {
            ////Arrange
            //_someMock.Setup(x => x.Method1(It.IsAny<YourParamType>())).Returns(YourResultCase);
            //var msg = SomeData();

            ////Act
            //var result = _actualTestService.SomeTestMethod(msg);

            ////Assert
            //Assert.True(result, "Info");
        }
    }
}
