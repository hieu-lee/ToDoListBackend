using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDoListBackend.Migrations
{
    public partial class _ : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    username = table.Column<string>(type: "TEXT", nullable: false),
                    password = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.username);
                });

            migrationBuilder.CreateTable(
                name: "Lists",
                columns: table => new
                {
                    listId = table.Column<string>(type: "TEXT", nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: true),
                    timeCreate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ownerUsername = table.Column<string>(type: "TEXT", nullable: true),
                    deleteHeight = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lists", x => x.listId);
                    table.ForeignKey(
                        name: "FK_Lists_Accounts_ownerUsername",
                        column: x => x.ownerUsername,
                        principalTable: "Accounts",
                        principalColumn: "username",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    itemId = table.Column<string>(type: "TEXT", nullable: false),
                    parentListId = table.Column<string>(type: "TEXT", nullable: true),
                    owner = table.Column<string>(type: "TEXT", nullable: true),
                    timeCreate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    timeRemind = table.Column<DateTime>(type: "TEXT", nullable: true),
                    important = table.Column<bool>(type: "INTEGER", nullable: false),
                    completed = table.Column<bool>(type: "INTEGER", nullable: false),
                    content = table.Column<string>(type: "TEXT", nullable: true),
                    title = table.Column<string>(type: "TEXT", nullable: true),
                    contentHeight = table.Column<string>(type: "TEXT", nullable: true),
                    deleteHeight = table.Column<string>(type: "TEXT", nullable: true),
                    lastNotified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ToDoListlistId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.itemId);
                    table.ForeignKey(
                        name: "FK_Items_Lists_ToDoListlistId",
                        column: x => x.ToDoListlistId,
                        principalTable: "Lists",
                        principalColumn: "listId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_ToDoListlistId",
                table: "Items",
                column: "ToDoListlistId");

            migrationBuilder.CreateIndex(
                name: "IX_Lists_ownerUsername",
                table: "Lists",
                column: "ownerUsername");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Lists");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
