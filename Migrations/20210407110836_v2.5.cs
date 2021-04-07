using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Migrations
{
    public partial class v25 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "root-0c0-aa65-4af8-bd17-00bd9344e575",
                column: "ConcurrencyStamp",
                value: "8ca2fbf9-7953-4672-8f61-e6d70b257c45");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "user-2c0-aa65-4af8-bd17-00bd9344e575",
                column: "ConcurrencyStamp",
                value: "913c1319-1371-4317-a9bf-f3011f324b5d");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d16ff640-0fe7-41fc-89bf-c4bb6186d484", "AQAAAAEAACcQAAAAEKH9zAyVtcsA8eTXLiCcdG3ZdBEEshGtfs+24VhLXx5hwc+0o42RfgfOAIIZOipuWg==", "48cc8ae5-211f-4db2-8746-b55d5e7ee178" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "root-0c0-aa65-4af8-bd17-00bd9344e575",
                column: "ConcurrencyStamp",
                value: "644351f1-64af-46e1-bbc4-070ee76d6158");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "user-2c0-aa65-4af8-bd17-00bd9344e575",
                column: "ConcurrencyStamp",
                value: "4ab824a4-4599-42d6-832d-2371a71f8192");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1f41c482-896f-40cf-956c-cce6f724163d", "AQAAAAEAACcQAAAAEPK1WhbSOuT0z7PAscm2dLzXCmpQhYLsBSWlvC8fN72lwQRRA7auODD6XS05QSsZkw==", "fe9b8de0-d98d-4f8c-aa5e-cc4b2561beb5" });
        }
    }
}
