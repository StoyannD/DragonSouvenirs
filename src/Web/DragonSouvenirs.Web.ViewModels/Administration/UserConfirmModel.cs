namespace DragonSouvenirs.Web.ViewModels.Administration
{
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;

    public class UserConfirmModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public bool IsDeleted { get; set; }
    }
}
