using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Riktam.GroupChat.SqlDbProvider.Migrations;

/// <inheritdoc />
public partial class InitialEntities : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "tblGroups",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_tblGroups", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "tblUsers",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Password = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_tblUsers", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "tblGroupMessages",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                GroupId = table.Column<int>(type: "int", nullable: false),
                UserId = table.Column<int>(type: "int", nullable: false),
                Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_tblGroupMessages", x => x.Id);
                table.ForeignKey(
                    name: "FK_tblGroupMessages_tblGroups_GroupId",
                    column: x => x.GroupId,
                    principalTable: "tblGroups",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_tblGroupMessages_tblUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "tblUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_tblGroupMessages_GroupId",
            table: "tblGroupMessages",
            column: "GroupId");

        migrationBuilder.CreateIndex(
            name: "IX_tblGroupMessages_UserId",
            table: "tblGroupMessages",
            column: "UserId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "tblGroupMessages");

        migrationBuilder.DropTable(
            name: "tblGroups");

        migrationBuilder.DropTable(
            name: "tblUsers");
    }
}
