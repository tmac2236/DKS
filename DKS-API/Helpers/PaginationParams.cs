namespace DKS_API.Helpers
{
    public class PaginationParams
    {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1; //d4 第一頁
        private int pageSize = 10;               //d4 一頁10個obj
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }

        public string OrderBy { get; set; }
        public bool IsPaging { get; set; }
        public string loginUser { get; set; }
    }
}