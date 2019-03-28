#region LICENSE

// Copyright (c) Alexey Malinin. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

#endregion


using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Camunda.Worker.Execution
{
    public class DefaultExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<DefaultExceptionHandler> _logger;

        public DefaultExceptionHandler(ILogger<DefaultExceptionHandler> logger = default)
        {
            _logger = logger ?? new NullLogger<DefaultExceptionHandler>();
        }

        public bool TryTransformToResult(Exception exception, out IExecutionResult executionResult)
        {
            executionResult = new FailureResult(exception);
            _logger.LogInformation("Exception of type {Type} transformed to \"FailureResult\"", exception.GetType());
            return true;
        }
    }
}
