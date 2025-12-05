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

        // 💡 التعديل المقترح: Constructor يحسب TotalPages تلقائياً
        public PagedResult(IEnumerable<T> items, int count, int pageIndex, int pageSize)
        {
            CurrentPage = pageIndex;
            PageSize = pageSize;
            TotalCount = count;
            Items = items;
            // حساب إجمالي الصفحات (باستخدام Math.Ceiling لتأمين الحساب لأي باقي)
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }
    }
}
