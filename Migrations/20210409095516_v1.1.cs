using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Migrations
{
    public partial class v11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ShippingFee",
                table: "Orders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "root-0c0-aa65-4af8-bd17-00bd9344e575",
                column: "ConcurrencyStamp",
                value: "8b060d67-1c7c-499f-a23f-769908dc066f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "user-2c0-aa65-4af8-bd17-00bd9344e575",
                column: "ConcurrencyStamp",
                value: "8fe1579a-bbf0-47b1-9fd2-39b23f22ab86");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2643f7b6-09d1-4d9c-b2bf-757c5cf30c93", "AQAAAAEAACcQAAAAEFWhBzTF/2Uh36UOAusjE/oRkrMX60rFwmb4Iw/J9NLAkRDKBmWcRXFWPCoE9MVcpQ==", "189afbe7-a767-4e2c-aaea-53b44ad5c092" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingFee",
                table: "Orders");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "root-0c0-aa65-4af8-bd17-00bd9344e575",
                column: "ConcurrencyStamp",
                value: "8d787dd3-e3ee-4653-9846-0d4c303b21c9");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "user-2c0-aa65-4af8-bd17-00bd9344e575",
                column: "ConcurrencyStamp",
                value: "a3fdeb8a-cf7b-480d-b387-9c3f2b89ca64");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "50a675ff-075d-4f2a-bb21-8bb26164dc27", "AQAAAAEAACcQAAAAEIcBNffh9GY9n6mCA00ea7dXDlcLvAAUnbhc10eux3kgIPasNnD2RHm3wZalJGzjew==", "6cbe54ed-eed2-4644-a95b-54608c7618fc" });
        }
    }
}
