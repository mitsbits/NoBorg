using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Borg.Bookstore.Data.Migrations
{
    [DbContext(typeof(BookstoreDbContext))]
    [Migration("CustomMigration_CacheTable")]
    public class CacheTableMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE SCHEMA [cache];
                                    GO
                                    CREATE TABLE[cache].[Store](
                                        [Id][nvarchar](449) NOT NULL,
                                        [Value][varbinary](max) NOT NULL,
                                        [ExpiresAtTime][datetimeoffset](7) NOT NULL,
                                        [SlidingExpirationInSeconds][bigint] NULL,
                                        [AbsoluteExpiration][datetimeoffset](7) NULL,
                                        CONSTRAINT[pk_Id] PRIMARY KEY CLUSTERED([Id] ASC) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
                                    ON[PRIMARY]) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY];
                                    GO   ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP TABLE[cache].[Store];
                                   GO
                                   DROP SCHEMA [cache];
                                   GO");
        }
    }
}