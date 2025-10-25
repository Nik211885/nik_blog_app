using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class addMailInfoForSendNotifiationWithChanelMail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MailInfoId",
                table: "NotificationTemplates",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "MailInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Host = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Port = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EmailId = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    UserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "character varying(150)", unicode: false, maxLength: 150, nullable: false),
                    EnableSsl = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailInfos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTemplates_MailInfoId",
                table: "NotificationTemplates",
                column: "MailInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationTemplates_MailInfos_MailInfoId",
                table: "NotificationTemplates",
                column: "MailInfoId",
                principalTable: "MailInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationTemplates_MailInfos_MailInfoId",
                table: "NotificationTemplates");

            migrationBuilder.DropTable(
                name: "MailInfos");

            migrationBuilder.DropIndex(
                name: "IX_NotificationTemplates_MailInfoId",
                table: "NotificationTemplates");

            migrationBuilder.DropColumn(
                name: "MailInfoId",
                table: "NotificationTemplates");
        }
    }
}
