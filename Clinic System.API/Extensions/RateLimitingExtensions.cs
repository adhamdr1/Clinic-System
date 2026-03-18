namespace Clinic_System.API.Extensions
{
    public static class RateLimitingExtensions
    {
        // بنعمل Extension Method للـ IServiceCollection عشان نناديها من Program.cs
        public static IServiceCollection AddCustomRateLimiting(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.OnRejected = async (context, token) =>
                {
                    var ip = context.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown_IP";
                    var path = context.HttpContext.Request.Path.ToString();
                    var method = context.HttpContext.Request.Method;
                    var userId = context.HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "Guest";

                    var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();

                    logger.LogWarning("Rate Limit Exceeded! IP: {IP}, User: {UserId}, Tried to attack: {Method} {Path}", ip, userId, method, path);

                    string cacheKey = $"BlockedTracker_{ip}_{DateTime.UtcNow.Ticks}";

                    var crimeData = new
                    {
                        AttackerIP = ip,
                        AttackerId = userId,
                        TargetEndpoint = path,
                        AttackTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
                    };

                    await cacheService.SetDataAsync(cacheKey, crimeData, TimeSpan.FromHours(24));

                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    context.HttpContext.Response.ContentType = "application/json";

                    var errorResponse = new
                    {
                        message = "Too many requests. You have been flagged. Please try again later.",
                        statusCode = 429
                    };

                    await context.HttpContext.Response.WriteAsJsonAsync(errorResponse, token);
                };


                // 2. الحماية العامة (Global Limiter) - بيطبق على أي ريكويست
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                {
                    // بنسأل: هل اليوزر ده مسجل دخول (معاه توكن سليم)؟
                    var isAuthenticated = context.User.Identity?.IsAuthenticated ?? false;

                    if (isAuthenticated)
                    {
                        // لو مسجل، بنجيب الـ ID بتاعه من التوكن
                        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                     ?? context.User.Identity?.Name
                                     ?? "unknown_user";

                        // بنعمله جردل خاص بيه باسمه
                        return RateLimitPartition.GetTokenBucketLimiter(
                            partitionKey: $"User_{userId}",
                            factory: _ => new TokenBucketRateLimiterOptions
                            {
                                TokenLimit = 100, // مسموحله بـ 100 ريكويست في الدقيقة
                                ReplenishmentPeriod = TimeSpan.FromMinutes(1), // الجردل بيتملي كل دقيقة
                                TokensPerPeriod = 100, // بنحطله 100 عملة جديدة
                                QueueLimit = 0,
                                AutoReplenishment = true
                            });
                    }
                    else
                    {
                        // لو مش مسجل دخول، بنعامله بالـ IP وبنديله ليميت أقل (60 بس)
                        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown_ip";
                        return RateLimitPartition.GetTokenBucketLimiter(
                            partitionKey: $"IP_{ip}",
                            factory: _ => new TokenBucketRateLimiterOptions
                            {
                                TokenLimit = 60, // الزائر العادي آخره 60 ريكويست
                                ReplenishmentPeriod = TimeSpan.FromMinutes(1),
                                TokensPerPeriod = 60,
                                QueueLimit = 0,
                                AutoReplenishment = true
                            });
                    }
                });

               
                // 3. إنشاء سياسة حماية الـ Auth (باستخدام Sliding Window)
                options.AddPolicy("AuthLimiter", httpContext =>

                    // هنا بنعمل Partition (تقسيم). بنقوله افصل العدادات بناءً على الـ IP Address
                    RateLimitPartition.GetSlidingWindowLimiter(
                        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown_IP",

                        // هنا بنظبط إعدادات الخوارزمية (Sliding Window)
                        factory: _ => new SlidingWindowRateLimiterOptions
                        {
                            PermitLimit = 5,                  // مسموح بـ 5 محاولات فقط
                            Window = TimeSpan.FromMinutes(1), // الإطار الزمني: دقيقة واحدة
                            SegmentsPerWindow = 3,            // (السر هنا) قسم الدقيقة لـ 3 أجزاء (يعني جزء كل 20 ثانية) عشان النافذة تتزحلق بنعومة
                            QueueLimit = 0,                   // لو عدى الـ 5، ارفضه فوراً (مفيش طابور انتظار)
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst, // ملهاش لازمة أوي طالما الـ Queue بـ 0، بس دي بتقول لو في طابور، دخل القديم الأول
                            AutoReplenishment = true          // رجعله الـ 5 محاولات بتوعه بشكل تلقائي لما الوقت يخلص
                        }));
            });

            return services;
        }
    }
}