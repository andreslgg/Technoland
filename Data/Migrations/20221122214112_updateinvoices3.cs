using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Technoland.Data.Migrations
{
    public partial class updateinvoices3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cart",
                table: "Invoices");

            migrationBuilder.AddColumn<Guid>(
                name: "InvoicesId",
                table: "Smartphones",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Smartphones_InvoicesId",
                table: "Smartphones",
                column: "InvoicesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Smartphones_Invoices_InvoicesId",
                table: "Smartphones",
                column: "InvoicesId",
                principalTable: "Invoices",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Smartphones_Invoices_InvoicesId",
                table: "Smartphones");

            migrationBuilder.DropIndex(
                name: "IX_Smartphones_InvoicesId",
                table: "Smartphones");

            migrationBuilder.DropColumn(
                name: "InvoicesId",
                table: "Smartphones");

            migrationBuilder.AddColumn<string>(
                name: "Cart",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
