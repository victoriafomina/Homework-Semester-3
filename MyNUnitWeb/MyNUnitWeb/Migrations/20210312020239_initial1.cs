using Microsoft.EntityFrameworkCore.Migrations;

namespace MyNUnitWeb.Migrations
{
    public partial class initial1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tests_Assemblies_AssemblyId",
                table: "Tests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Assemblies",
                table: "Assemblies");

            migrationBuilder.RenameTable(
                name: "Assemblies",
                newName: "AssemblyViewModel");

            migrationBuilder.AlterColumn<bool>(
                name: "Passed",
                table: "Tests",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssemblyViewModel",
                table: "AssemblyViewModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_AssemblyViewModel_AssemblyId",
                table: "Tests",
                column: "AssemblyId",
                principalTable: "AssemblyViewModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tests_AssemblyViewModel_AssemblyId",
                table: "Tests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AssemblyViewModel",
                table: "AssemblyViewModel");

            migrationBuilder.RenameTable(
                name: "AssemblyViewModel",
                newName: "Assemblies");

            migrationBuilder.AlterColumn<bool>(
                name: "Passed",
                table: "Tests",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Assemblies",
                table: "Assemblies",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_Assemblies_AssemblyId",
                table: "Tests",
                column: "AssemblyId",
                principalTable: "Assemblies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
