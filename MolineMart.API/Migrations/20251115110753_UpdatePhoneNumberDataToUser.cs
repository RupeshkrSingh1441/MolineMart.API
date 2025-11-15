using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MolineMart.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePhoneNumberDataToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Update Address fields for all existing users
            migrationBuilder.Sql(@"
        UPDATE AspNetUsers
        SET 
            PhoneNumber = COALESCE(PhoneNumber, '+919999999999'),
            PhoneNumberConfirmed = COALESCE(PhoneNumberConfirmed, 1)
        WHERE PhoneNumber IS NULL
         OR PhoneNumberConfirmed = 0;
    ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        UPDATE AspNetUsers
        SET 
            PhoneNumber = NULL,
            PhoneNumberConfirmed = 0;
    ");
        }

    }
}
