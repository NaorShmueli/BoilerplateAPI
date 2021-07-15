using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class UnitTestBase
    {
        private readonly MockRepository _mockRepository;
        private readonly IConfiguration _configuration;
        public UnitTestBase()
        {
            _mockRepository = new MockRepository(MockBehavior.Default);
            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.test.json").Build();
            //Create your interfaces
        }

    }
}
