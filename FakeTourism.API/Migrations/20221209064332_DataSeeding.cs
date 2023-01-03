using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FakeTourism.API.Migrations
{
    public partial class DataSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "touristRoutes",
                columns: new[] { "Id", "CreateTime", "DepartureTime", "Description", "DiscountPresent", "Features", "Fees", "Notes", "OriginalPrice", "Title", "UpdateTime" },
                values: new object[] { new Guid("dbc912a9-bda9-487b-97f4-00ff45203c0a"), new DateTime(2022, 12, 9, 6, 43, 31, 569, DateTimeKind.Utc).AddTicks(3417), null, "Testing Description", null, null, null, null, 0m, "Testing Title", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "touristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("dbc912a9-bda9-487b-97f4-00ff45203c0a"));
        }
    }
}
