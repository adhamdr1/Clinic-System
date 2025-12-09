namespace Clinic_System.API.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                // حاول تشغل الطلب عادي
                await _next(context);
            }
            catch (Exception error)
            {
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
                            responseModel.Errors = e.Errors;
                        break;

                    // معالجة باقي الأخطاء
                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;

                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
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
