namespace Clinic_System.Infrastructure.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _db;
        private readonly ILogger<RedisCacheService> _logger;

        private static readonly JsonSerializerOptions _options =
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };

        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer, ILogger<RedisCacheService> logger)
        {
            _db = connectionMultiplexer.GetDatabase();
            _logger = logger;
        }

        public async Task<T?> GetDataAsync<T>(string key)
        {
            try
            {
                var value = await _db.StringGetAsync(key);

                if (!value.HasValue)
                    return default;

                // الداتا بترجع كـ String (JSON)، بنحولها للـ Entity بتاعنا
                return JsonSerializer.Deserialize<T>(value.ToString(), _options);
            }
            catch (RedisConnectionException ex)
            {
                // 🔥 الـ Graceful Degradation: لو الـ Redis وقع، هنسجل الخطأ ونرجع null
                // الـ Handler لما يلاقيها null هيروح يجيبها من הـ SQL كأن مفيش كاش
                _logger.LogWarning(ex, "Redis is down! Failed to GET data for key: {Key}", key);
                return default;
            }
            catch (RedisTimeoutException ex)
            {
                _logger.LogWarning(ex, "Redis timeout! Failed to GET data for key: {Key}", key);
                return default;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex,
                    "JSON deserialization failed for key: {Key}", key);
                return default;
            }
        }

        public async Task<bool> SetDataAsync<T>(string key, T value, TimeSpan expirationTime)
        {
            try
            {
                var jsonValue = JsonSerializer.Serialize(value , _options);

                // بنستخدم StringSetAsync مع الـ Time To Live (TTL)
                return await _db.StringSetAsync(key, jsonValue, expirationTime);
            }
            catch (Exception ex) when (ex is RedisConnectionException || ex is RedisTimeoutException)
            {
                _logger.LogWarning(ex, "Redis is down! Failed to SET data for key: {Key}", key);
                return false; // فشل في الكاش، بس السيستم هيكمل عادي
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex,
                    "JSON serialization failed for key: {Key}", key);
                return false;
            }
        }

        public async Task<bool> RemoveDataAsync(string key)
        {
            try
            {
                // لو الداتا اتعدلت (مثلاً دكتور اتضاف)، لازم نمسح الكاش القديم
                bool isDeleted = await _db.KeyDeleteAsync(key);
                return isDeleted;
            }
            catch (Exception ex) when (ex is RedisConnectionException || ex is RedisTimeoutException)
            {
                _logger.LogWarning(ex, "Redis is down! Failed to REMOVE key: {Key}", key);
                return false;
            }
        }
    }
}