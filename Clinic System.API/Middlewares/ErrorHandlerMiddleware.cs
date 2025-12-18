namespace Clinic_System.API.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            logger.LogInformation("ErrorHandlerMiddleware Invoked.");
            try
            {
                logger.LogInformation("Processing Request.");
                // حاول تشغل الطلب عادي
                await _next(context);
            }
            catch (Exception error)
            {
                logger.LogError(error, "An error occurred while processing the request.");
                // لو حصل أي خطأ، تعال هنا
                var response = context.Response;
                response.ContentType = "application/json";

                // تجهيز الرد الموحد (Response<T>)
                var responseModel = new Response<string>()
                {
                    Succeeded = false, 
                    Message = error.Message
                };

                switch (error)
                {
                    case ApiException e:
                        response.StatusCode = e.StatusCode;
                        responseModel.StatusCode = (HttpStatusCode)e.StatusCode;
                        responseModel.Message = e.Message;

                        if (e.Errors != null && e.Errors.Any())
                        {
                            logger.LogInformation("Adding API exception errors to response.");
                            responseModel.Errors = e.Errors;
                        }
                        break;

                    // معالجة باقي الأخطاء
                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        logger.LogInformation("KeyNotFoundException encountered: {Message}", e.Message);
                        break;

                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        logger.LogInformation("Unhandled exception encountered: {Message}", error.Message);
                        break;
                }

                // كتابة الرد كـ JSON
                var result = JsonSerializer.Serialize(responseModel, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                }); 

                await response.WriteAsync(result);
            }
        }
    }
}
