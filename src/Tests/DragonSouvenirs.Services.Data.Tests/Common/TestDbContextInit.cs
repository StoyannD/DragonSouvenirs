namespace DragonSouvenirs.Services.Data.Tests.Common
{
    using System;
    using DragonSouvenirs.Data;
    using Microsoft.EntityFrameworkCore;

    public class TestDbContextInit
    {
        public static ApplicationDbContext Init()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }
    }
}
