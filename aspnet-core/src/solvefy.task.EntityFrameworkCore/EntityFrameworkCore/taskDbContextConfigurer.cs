using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace solvefy.task.EntityFrameworkCore
{
    public static class taskDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<taskDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<taskDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
