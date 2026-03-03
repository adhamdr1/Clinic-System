namespace Clinic_System.Application.Common
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        [JsonConstructor]
        public PagedResult(IEnumerable<T> items, int totalCount, int currentPage, int pageSize)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalCount = totalCount;
            Items = items ?? new List<T>();

            // حساب إجمالي الصفحات
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }
    }
}
