using System.Collections.Generic;

namespace DragonSouvenirs.Web.ViewModels.Administration
{
    public class AllUsersViewModel
    {
        public IEnumerable<ApplicationUserViewModel> users { get; set; }
    }
}
