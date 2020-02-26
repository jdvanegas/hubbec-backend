using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "account");

            migrationBuilder.CreateTable(
                name: "User",
                schema: "account",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    LastName = table.Column<string>(maxLength: 255, nullable: true),
                    DocumentType = table.Column<int>(nullable: false),
                    DocumentNumber = table.Column<string>(maxLength: 125, nullable: true),
                    Phone = table.Column<string>(maxLength: 255, nullable: true),
                    Email = table.Column<string>(maxLength: 255, nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Token = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contact",
                schema: "account",
                columns: table => new
                {
                    FirstUserId = table.Column<Guid>(nullable: false),
                    SecondUserId = table.Column<Guid>(nullable: false),
                    RelationType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => new { x.FirstUserId, x.SecondUserId });
                    table.ForeignKey(
                        name: "FK_Contact_User_FirstUserId",
                        column: x => x.FirstUserId,
                        principalSchema: "account",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contact_User_SecondUserId",
                        column: x => x.SecondUserId,
                        principalSchema: "account",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contact_SecondUserId",
                schema: "account",
                table: "Contact",
                column: "SecondUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contact",
                schema: "account");

            migrationBuilder.DropTable(
                name: "User",
                schema: "account");
        }
    }
}
