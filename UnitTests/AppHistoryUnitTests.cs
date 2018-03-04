using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using PewBibleKjv.Logic;
using PewBibleKjv.Logic.Adapters.Services;
using PewBibleKjv.Logic.Adapters.UI;
using PewBibleKjv.Text;
using UnitTests.Util;
using Xunit;

namespace UnitTests
{
    public class AppHistoryUnitTests
    {
        [Fact]
        public void InitialStartup_HistoryButtonsDisabled()
        {
            var app = new StubbedApp();
            Assert.False(app.StubHistoryControls.BackEnabled);
            Assert.False(app.StubHistoryControls.ForwardEnabled);
        }
    }
}
