﻿namespace Nummy.HttpLogger.Data.Services;

internal interface INummyHttpLoggerService
{
    Task LogRequestAsync(string requestBody, string requestPath, string remoteIpAddress, string httpLogGuid);
    Task LogResponseAsync(string responseBody, string httpLogGuid);
}