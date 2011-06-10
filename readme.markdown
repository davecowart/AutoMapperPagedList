# Usage

	//Controller
	public ViewResult Index(int page = 0) {
		int pageSize = 10;
		IQueryable<Post> posts = context.Posts.OrderByDescending(p => p.Timestamp).AsQueryable();
		PagedList<PostViewModel> pagedList = posts.ToPagedList<Post, PostViewModel>(page, Mapper.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>, pageSize);
		return View(pagedList);
	}

	//View
	@if (Model.IsPreviousPage) {
		@Html.ActionLink("Previous", "Index", new { page = Model.PageIndex - 1 })
	}
	@(Model.PageIndex + 1) of @(Model.TotalCount / Model.PageSize)
	@if (Model.IsNextPage) {
		@Html.ActionLink("Next", "Index", new { page = Model.PageIndex + 1 })
	}

# More Info

For more info, see http://davecowart.wordpress.com/2011/06/09/automapperpagedlist/