namespace Clinic_System.Core.Exceptions
{
    public class ApiException : Exception
    {
        public int StatusCode { get; }
        public Dictionary<string, List<string>> Errors { get; }

        public ApiException(string message, int statusCode = 400, IEnumerable<string>? errors = null)
            : base(message)
        {
            StatusCode = statusCode;
            Errors = new Dictionary<string, List<string>>();

            if (errors != null)
            {
                foreach (var err in errors)
                {
                    // نفصل PropertyName عن الرسالة لو مكتوبة بالشكل "Property: Message"
                    var parts = err.Split(":");
                    var key = parts[0].Trim();
                    var value = parts.Length > 1 ? parts[1].Trim() : "Invalid value";

                    if (!Errors.ContainsKey(key))
                        Errors[key] = new List<string>();

                    Errors[key].Add(value);
                }
            }
        }
    }
}
