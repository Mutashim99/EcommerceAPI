using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommerceAPI.Migrations
{
    /// <inheritdoc />
    public partial class addedSuperAdminseed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "EmailVerificationToken", "IsEmailVerified", "PasswordHash", "PhoneNumber", "ResetPasswordToken", "ResetPasswordTokenExpiry", "Role", "Username" },
                values: new object[] { 1, "superadmin@ecommerce.com", null, true, "$2a$11$5i0fmOjS1mglQE4XUp7BPO.3S5puIyLqFIef9CBqODZjdSLopY3Uu", "03202048421", null, null, "SuperAdmin", "Super Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
