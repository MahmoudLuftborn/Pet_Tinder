using Newtonsoft.Json;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Pet_Tinder.Models.DTO.Pagination
{
	[JsonObject]
	public class PageModel<T> : IEnumerable<T>
	{
		public long Total { get; set; }
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
		public IEnumerable<T> List { get; set; }

		public IEnumerator<T> GetEnumerator()
		{
			return List.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return List.GetEnumerator();
		}

		public PageModel<TResult> Convert<TResult>(Func<T, TResult> converter)
		{
			var copy = new PageModel<TResult>
			{
				Total = Total,
				PageNumber = PageNumber,
				PageSize = PageSize,
				List = List.Select(converter)
			};
			return copy;
		}
	}
}
