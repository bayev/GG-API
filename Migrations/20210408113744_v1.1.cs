using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Migrations
{
    public partial class v11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "root-0c0-aa65-4af8-bd17-00bd9344e575",
                column: "ConcurrencyStamp",
                value: "ca7c152b-4581-4686-8799-1ee2cc4baac4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "user-2c0-aa65-4af8-bd17-00bd9344e575",
                column: "ConcurrencyStamp",
                value: "0e217f13-1d4b-4468-92b7-881cc6d878c7");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4d6ac5f4-d540-4bd1-a003-0bbdc9f31f4e", "AQAAAAEAACcQAAAAEBYa6Lu0d/nScDqm/9dG80HNl7gewc1/+dnz+PnrAKbK8p4DWN0z/1W2JceP235k2A==", "f092a791-3004-4b24-a8f9-183ffb88e2aa" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "root-0c0-aa65-4af8-bd17-00bd9344e575",
                column: "ConcurrencyStamp",
                value: "7f922c05-6202-4910-9293-06110488f3c4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "user-2c0-aa65-4af8-bd17-00bd9344e575",
                column: "ConcurrencyStamp",
                value: "a7e50490-6c12-4523-a816-c266d36dfebc");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ddd8b146-2d68-457a-94e9-59c1996a6396", "AQAAAAEAACcQAAAAEOJV1P0CydVvbHs3swfou5PTVojqqCYzsRdDoELLR/FBPFOLNuM4UmyRjoHWfHstSg==", "f99f0696-4ad5-428e-8abd-09307ed8973b" });
        }
    }
}
