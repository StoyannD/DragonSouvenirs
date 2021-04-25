namespace DragonSouvenirs.Web.ViewModels.Administration
{
    using System.Collections.Generic;

    public class AllUsersViewModel
    {
        public IEnumerable<ApplicationUserViewModel> Users { get; set; }
    }
}
