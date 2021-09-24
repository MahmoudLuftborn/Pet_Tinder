namespace Pet_Tinder.Models.DTO.Pagination
{
	public class PagedSearchParameters : IPagedSearchParameters
	{
		public int Page { get; set; } = 1;
		public int Size { get; set; } = 10;
	}
}