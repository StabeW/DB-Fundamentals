using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BillsPaymentSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDeleteStatement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentMethods_Users_UserId",
                table: "PaymentMethods");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentMethods_Users_UserId",
                table: "PaymentMethods",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentMethods_Users_UserId",
                table: "PaymentMethods");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentMethods_Users_UserId",
                table: "PaymentMethods",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
