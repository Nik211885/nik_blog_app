using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class changeTableNameReactionEntites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentReactions_Comments_EntityId",
                table: "CommentReactions");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentReactions_Posts_EntityId",
                table: "CommentReactions");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentReactions_Users_UserId",
                table: "CommentReactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentReactions",
                table: "CommentReactions");

            migrationBuilder.RenameTable(
                name: "CommentReactions",
                newName: "ReactionEntities");

            migrationBuilder.RenameIndex(
                name: "IX_CommentReactions_EntityId",
                table: "ReactionEntities",
                newName: "IX_ReactionEntities_EntityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReactionEntities",
                table: "ReactionEntities",
                columns: new[] { "UserId", "EntityType", "EntityId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ReactionEntities_Comments_EntityId",
                table: "ReactionEntities",
                column: "EntityId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReactionEntities_Posts_EntityId",
                table: "ReactionEntities",
                column: "EntityId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReactionEntities_Users_UserId",
                table: "ReactionEntities",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReactionEntities_Comments_EntityId",
                table: "ReactionEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_ReactionEntities_Posts_EntityId",
                table: "ReactionEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_ReactionEntities_Users_UserId",
                table: "ReactionEntities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReactionEntities",
                table: "ReactionEntities");

            migrationBuilder.RenameTable(
                name: "ReactionEntities",
                newName: "CommentReactions");

            migrationBuilder.RenameIndex(
                name: "IX_ReactionEntities_EntityId",
                table: "CommentReactions",
                newName: "IX_CommentReactions_EntityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentReactions",
                table: "CommentReactions",
                columns: new[] { "UserId", "EntityType", "EntityId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CommentReactions_Comments_EntityId",
                table: "CommentReactions",
                column: "EntityId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentReactions_Posts_EntityId",
                table: "CommentReactions",
                column: "EntityId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentReactions_Users_UserId",
                table: "CommentReactions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
