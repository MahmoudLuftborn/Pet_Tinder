using Pet_Tinder.Models.DTO.Pagination;

namespace Pet_Tinder.Models.DTO.Search
{
	public class BaseSearchParamters : PagedSearchParameters
	{
		public string SearchValue { get; set; }
		public string SortValue { get; set; }
		public bool SortAscending { get; set; }
	}
}