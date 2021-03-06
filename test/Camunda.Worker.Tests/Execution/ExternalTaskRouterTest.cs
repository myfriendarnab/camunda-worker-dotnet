using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace Camunda.Worker.Execution
{
    public class ExternalTaskRouterTest
    {
        private readonly Mock<IServiceProvider> _providerMock = new Mock<IServiceProvider>();
        private readonly Mock<IExternalTaskContext> _contextMock = new Mock<IExternalTaskContext>();
        private readonly Mock<IEndpointProvider> _endpointProviderMock = new Mock<IEndpointProvider>();
        private readonly ExternalTaskRouter _router;

        public ExternalTaskRouterTest()
        {
            _contextMock.SetupGet(context => context.ServiceProvider).Returns(_providerMock.Object);
            _contextMock.SetupGet(context => context.Task).Returns(new ExternalTask("1", "testWorker", "testTopic"));
            _router = new ExternalTaskRouter(
                _endpointProviderMock.Object
            );
        }

        [Fact]
        public async Task TestRouteAsync()
        {
            var calls = new List<IExternalTaskContext>();

            Task ExternalTaskDelegate(IExternalTaskContext context)
            {
                calls.Add(context);
                return Task.CompletedTask;
            }

            _endpointProviderMock.Setup(factory => factory.GetEndpointDelegate(It.IsAny<ExternalTask>()))
                .Returns(ExternalTaskDelegate);

            await _router.RouteAsync(_contextMock.Object);

            Assert.Single(calls);
        }
    }
}
