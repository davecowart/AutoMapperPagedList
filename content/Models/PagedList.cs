using System.Collections.Generic;
using System.Linq;

namespace System.Web.Mvc {
	public interface IPagedList {
		int TotalCount { get; set; }
		int PageIndex { get; set; }
		int PageSize { get; set; }
		bool IsPreviousPage { get; }
		bool IsNextPage { get; }
	}

	/// <summary>
	/// An implementation of PagedList that uses AutoMapper to emit view models from an IQueryable. 
	/// AutoMapper is not explicitly required, but is the easiest way to create the mapper parameter.
	/// </summary>
	/// <typeparam name="TSource">Source type (your Model)</typeparam>
	/// <typeparam name="TOutput">Output type (your View Model)</typeparam>
	public class MappedPagedList<TSource, TOutput> : PagedList<TOutput> {
		/// <param name="source">Source of items to page</param>
		/// <param name="index">Zero-based page index</param>
		/// <param name="pageSize">Number of items to display per page</param>
		/// <param name="mapper">A mapping function, most easily created using AutoMapper</param>
		public MappedPagedList(IQueryable<TSource> source, int index, int pageSize, Func<IEnumerable<TSource>, IEnumerable<TOutput>> map)
			: base(index, pageSize, source.Count()) {
			this.AddRange(map(source.Skip(index * pageSize).Take(pageSize).ToList()));
		}

		/// <param name="source">Source of items to page</param>
		/// <param name="index">Zero-based page index</param>
		/// <param name="pageSize">Number of items to display per page</param>
		/// <param name="m">A mapping function, most easily created using AutoMapper</param>
		public MappedPagedList(List<TSource> source, int index, int pageSize, Func<IEnumerable<TSource>, IEnumerable<TOutput>> map)
			: base(index, pageSize, source.Count()) {
			this.AddRange(map(source.Skip(index * pageSize).Take(pageSize).ToList()));
		}
	}

	public class PagedList<T> : List<T>, IPagedList {
		public PagedList(IQueryable<T> source, int index, int pageSize)
			: this(index, pageSize, source.Count()) {
			this.AddRange(source.Skip(index * pageSize).Take(pageSize).ToList());
		}

		public PagedList(List<T> source, int index, int pageSize)
			: this(index, pageSize, source.Count()) {
			this.AddRange(source.Skip(index * pageSize).Take(pageSize).ToList());
		}

		protected PagedList(int index, int pageSize, int totalCount) {
			this.PageSize = pageSize;
			this.PageIndex = index;
			this.TotalCount = totalCount;
		}

		public int TotalCount { get; set; }
		public int PageIndex { get; set; }
		public int PageSize { get; set; }

		public bool IsPreviousPage { get { return (PageIndex > 0); } }
		public bool IsNextPage { get { return (PageIndex * PageSize) < TotalCount - PageSize; } }
	}

	public static class Pagination {
		public static PagedList<T> ToPagedList<T>(this IQueryable<T> source, int index, int pageSize = 10) {
			return new PagedList<T>(source, index, pageSize);
		}

		public static MappedPagedList<TSource, TOutput> ToPagedList<TSource, TOutput>(this IQueryable<TSource> source, int index, Func<IEnumerable<TSource>, IEnumerable<TOutput>> mapper, int pageSize = 10) {
			return new MappedPagedList<TSource, TOutput>(source, index, pageSize, mapper);
		}
	}
}