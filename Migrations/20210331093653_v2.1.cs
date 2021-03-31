using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Migrations
{
    public partial class v21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "root-0c0-aa65-4af8-bd17-00bd9344e575",
                column: "ConcurrencyStamp",
                value: "dd7ef9be-3e8a-4803-a57a-46eeebda2c6a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "user-2c0-aa65-4af8-bd17-00bd9344e575",
                column: "ConcurrencyStamp",
                value: "cbe44a43-8dad-45b9-a8e9-de0b4901e000");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e05e1227-ba34-41f5-8ad3-80a2666c2afb", "AQAAAAEAACcQAAAAECbd5oyASAJt0LDz+yEmX78RcM7jvaaeWGB/ZRJP76/x4a+ECQc6pIWJjrdVTvgZyg==", "c4c8a809-c7e6-44f3-afd4-7d054539b321" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "root-0c0-aa65-4af8-bd17-00bd9344e575",
                column: "ConcurrencyStamp",
                value: "46e00039-b667-4547-a10d-60e6473cb392");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "user-2c0-aa65-4af8-bd17-00bd9344e575",
                column: "ConcurrencyStamp",
                value: "4a16af01-d961-40b5-8f96-facd507f836e");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c2edfbd0-3a73-4647-a1f4-140eb6f66cf6", "AQAAAAEAACcQAAAAEBlGbSLqmrFOPKHm2wHg7HOI8qfF15z7balKNi88Z7AWmvDxlE92n2obe4ed4SsSgw==", "bbe55a66-e56b-488a-bef9-f1688ff6d14d" });
        }
    }
}
