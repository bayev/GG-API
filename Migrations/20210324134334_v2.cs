using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CartToProducts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Timestamp = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartToProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartToProducts_Carts_Id",
                        column: x => x.Id,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartToProducts_Products_Id",
                        column: x => x.Id,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "root-0c0-aa65-4af8-bd17-00bd9344e575",
                column: "ConcurrencyStamp",
                value: "9ba703ef-60e1-45e0-92bb-aff0e07b89b0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "user-2c0-aa65-4af8-bd17-00bd9344e575",
                column: "ConcurrencyStamp",
                value: "c01b8cff-972a-4382-ad6b-5a79098c9c05");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "681b96e2-e21f-48e3-9650-54911ec2dabc", "AQAAAAEAACcQAAAAEID1EJucXlKvToRhbwOt1H2FCpn5+/rsWQb0wd9/ym/lCZvwvlKsCb7AHR1uW1wn8A==", "5f6f00e8-226b-4512-ae87-caa72da0191b" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartToProducts");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "root-0c0-aa65-4af8-bd17-00bd9344e575",
                column: "ConcurrencyStamp",
                value: "30f4ceab-ba68-4db3-ba39-6cc0f3f28b25");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "user-2c0-aa65-4af8-bd17-00bd9344e575",
                column: "ConcurrencyStamp",
                value: "91e450de-188b-4957-8cd2-7c25f99f3ccf");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d19fe725-2983-4ad4-afb9-3ba8c2377e10", "AQAAAAEAACcQAAAAEF1j0Qzeq8P9FnhNHcr8kHB8IPlKtrthA3Y6pDKv7/8I5vpu8B5TYhhwPbz89sitwA==", "8e9ab40d-f0d1-471d-b7b3-802bc2b48407" });
        }
    }
}
