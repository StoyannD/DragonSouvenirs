namespace DragonSouvenirs.Web.ViewModels.Categories
{
    using DragonSouvenirs.Common.Enums;

    public class CategoryPaginationInfo
    {
        public int? MinPrice { get; set; }

        public int? MaxPrice { get; set; }

        public string Route { get; set; }

        public string CategoryName { get; set; }

        public int? CurrentPage { get; set; }

        public int PagesCount { get; set; }

        public int AllProductsCount { get; set; }

        public int? ProductsPerPage { get; set; }

        public SortBy SortBy { get; set; }

        public string SearchString { get; set; }
    }
}
