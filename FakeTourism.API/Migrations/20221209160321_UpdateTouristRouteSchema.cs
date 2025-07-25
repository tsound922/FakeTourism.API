﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FakeTourism.API.Migrations
{
    public partial class UpdateTouristRouteSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Rating",
                table: "touristRoutes",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "departureCity",
                table: "touristRoutes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "travelDays",
                table: "touristRoutes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "tripType",
                table: "touristRoutes",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "touristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("2430bf64-fd56-460c-8b75-da0a1d1cd74c"),
                columns: new[] { "Rating", "departureCity", "travelDays", "tripType" },
                values: new object[] { 5.0, 3, 4, 3 });

            migrationBuilder.UpdateData(
                table: "touristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("39996f34-013c-4fc6-b1b3-0c1036c47110"),
                columns: new[] { "Rating", "departureCity", "travelDays", "tripType" },
                values: new object[] { 3.2000000000000002, 3, 2, 3 });

            migrationBuilder.UpdateData(
                table: "touristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("39996f34-013c-4fc6-b1b3-0c1036c47111"),
                columns: new[] { "Rating", "departureCity", "travelDays", "tripType" },
                values: new object[] { 4.5, 3, 4, 3 });

            migrationBuilder.UpdateData(
                table: "touristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("39996f34-013c-4fc6-b1b3-0c1036c47112"),
                columns: new[] { "Rating", "departureCity", "travelDays", "tripType" },
                values: new object[] { 5.0, 3, 4, 3 });

            migrationBuilder.UpdateData(
                table: "touristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("39996f34-013c-4fc6-b1b3-0c1036c47113"),
                columns: new[] { "Rating", "departureCity", "travelDays", "tripType" },
                values: new object[] { 4.5, 3, 4, 3 });

            migrationBuilder.UpdateData(
                table: "touristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("39996f34-013c-4fc6-b1b3-0c1036c47114"),
                columns: new[] { "Rating", "departureCity", "travelDays", "tripType" },
                values: new object[] { 4.5, 3, 4, 3 });

            migrationBuilder.UpdateData(
                table: "touristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("39996f34-013c-4fc6-b1b3-0c1036c47115"),
                columns: new[] { "Rating", "departureCity", "travelDays", "tripType" },
                values: new object[] { 4.5, 3, 4, 3 });

            migrationBuilder.UpdateData(
                table: "touristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("39996f34-013c-4fc6-b1b3-0c1036c47116"),
                columns: new[] { "Rating", "departureCity", "travelDays", "tripType" },
                values: new object[] { 4.5, 3, 4, 3 });

            migrationBuilder.UpdateData(
                table: "touristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("39996f34-013c-4fc6-b1b3-0c1036c47117"),
                columns: new[] { "Rating", "departureCity", "travelDays", "tripType" },
                values: new object[] { 4.5, 3, 4, 3 });

            migrationBuilder.UpdateData(
                table: "touristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("39996f34-013c-4fc6-b1b3-0c1036c47118"),
                columns: new[] { "Rating", "departureCity", "travelDays", "tripType" },
                values: new object[] { 4.5, 3, 4, 3 });

            migrationBuilder.UpdateData(
                table: "touristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("39996f34-013c-4fc6-b1b3-0c1036c47119"),
                columns: new[] { "Rating", "departureCity", "travelDays", "tripType" },
                values: new object[] { 4.5, 3, 4, 3 });

            migrationBuilder.UpdateData(
                table: "touristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("39996f34-013c-4fc6-b1b3-0c1036c47169"),
                columns: new[] { "Rating", "departureCity", "travelDays", "tripType" },
                values: new object[] { 2.1000000000000001, 3, 6, 3 });

            migrationBuilder.UpdateData(
                table: "touristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("3ecbcd92-a9e0-45f7-9b29-e03272cb0862"),
                columns: new[] { "Rating", "departureCity", "travelDays", "tripType" },
                values: new object[] { 5.0, 1, 4, 3 });

            migrationBuilder.UpdateData(
                table: "touristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("88cf89b9-e4b5-4b42-a5bf-622bd3039601"),
                columns: new[] { "Rating", "departureCity", "travelDays", "tripType" },
                values: new object[] { 3.0, 2, 2, 4 });

            migrationBuilder.UpdateData(
                table: "touristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("fb6d4f10-79ed-4aff-a915-4ce29dc9c7e1"),
                columns: new[] { "Rating", "departureCity", "travelDays", "tripType" },
                values: new object[] { 3.5, 0, 8, 0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "touristRoutes");

            migrationBuilder.DropColumn(
                name: "departureCity",
                table: "touristRoutes");

            migrationBuilder.DropColumn(
                name: "travelDays",
                table: "touristRoutes");

            migrationBuilder.DropColumn(
                name: "tripType",
                table: "touristRoutes");
        }
    }
}
