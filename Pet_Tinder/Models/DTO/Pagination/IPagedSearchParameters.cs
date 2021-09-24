namespace Pet_Tinder.Models.DTO.Pagination
{
	public interface IPagedSearchParameters
	{
		int Page { get; set; }
		int Size { get; set; }
	}
}