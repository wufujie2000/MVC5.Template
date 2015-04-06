using MvcTemplate.Components.Logging;
using MvcTemplate.Objects;
using MvcTemplate.Tests.Data;
using NSubstitute;
using System;
using System.Data.Entity;
using System.Linq;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Logging
{
    public class LoggerTests
    {
        #region Method: Log(String message)

        [Fact]
        public void Log_LogsMessage()
        {
            using (TestingContext context = new TestingContext())
            {
                context.Set<Log>().RemoveRange(context.Set<Log>());
                context.SaveChanges();

                new Logger(context).Log(new String('L', 10000));

                Log expected = new Log { Message = new String('L', 10000) };
                Log actual = context.Set<Log>().Single();

                Assert.Equal(expected.AccountId, actual.AccountId);
                Assert.Equal(expected.Message, actual.Message);
            }
        }

        #endregion

        #region Method: Log(String accountId, String message)

        [Fact]
        public void Log_LogsAccountIdAndMessage()
        {
            using (TestingContext context = new TestingContext())
            {
                context.Set<Log>().RemoveRange(context.Set<Log>());
                context.SaveChanges();

                new Logger(context).Log("Test", new String('L', 10000));

                Log expected = new Log { AccountId = "Test", Message = new String('L', 10000) };
                Log actual = context.Set<Log>().Single();

                Assert.Equal(expected.AccountId, actual.AccountId);
                Assert.Equal(expected.Message, actual.Message);
            }
        }

        #endregion

        #region Method: Dispose()

        [Fact]
        public void Dispose_DisposesContext()
        {
            DbContext context = Substitute.For<DbContext>();
            Logger logger = new Logger(context);

            logger.Dispose();

            context.Received().Dispose();
        }

        [Fact]
        public void Dispose_CanBeCalledMultipleTimes()
        {
            DbContext context = Substitute.For<DbContext>();
            Logger logger = new Logger(context);

            logger.Dispose();
            logger.Dispose();
        }

        #endregion
    }
}
