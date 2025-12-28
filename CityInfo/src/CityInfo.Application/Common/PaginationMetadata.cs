namespace CityInfo.Application.Common
{
    public class PaginationMetadata
    {
        #region [ Fields ]
        public int TotalItemCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; }
        public string? PreviousPageLink { get; set; }
        public string? NextPageLink { get; set; }
        #endregion

        #region [ Constructure ]
        public PaginationMetadata(int totalItemCount, int pageSize, int currentPage, int totalPages,
            string? previousPageLink = "", string? nextPageLink = "")
        {
            TotalItemCount = totalItemCount;
            PageSize = pageSize;
            CurrentPage = currentPage;
            TotalPages = totalPages;
            PreviousPageLink = previousPageLink;
            NextPageLink = nextPageLink;
        }
        #endregion
    }
}
