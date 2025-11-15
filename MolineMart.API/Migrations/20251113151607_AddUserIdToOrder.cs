using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MolineMart.API.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "ReviewDate",
                value: new DateTime(2025, 11, 13, 15, 16, 6, 391, DateTimeKind.Utc).AddTicks(5926));

            migrationBuilder.UpdateData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "ReviewDate",
                value: new DateTime(2025, 11, 13, 15, 16, 6, 391, DateTimeKind.Utc).AddTicks(5937));

            migrationBuilder.UpdateData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 3,
                column: "ReviewDate",
                value: new DateTime(2025, 11, 13, 15, 16, 6, 391, DateTimeKind.Utc).AddTicks(5939));

            migrationBuilder.UpdateData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 4,
                column: "ReviewDate",
                value: new DateTime(2025, 11, 13, 15, 16, 6, 391, DateTimeKind.Utc).AddTicks(5941));

            migrationBuilder.UpdateData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 5,
                column: "ReviewDate",
                value: new DateTime(2025, 11, 13, 15, 16, 6, 391, DateTimeKind.Utc).AddTicks(5943));

            migrationBuilder.UpdateData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 6,
                column: "ReviewDate",
                value: new DateTime(2025, 11, 13, 15, 16, 6, 391, DateTimeKind.Utc).AddTicks(5944));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "ReviewDate",
                value: new DateTime(2025, 11, 11, 11, 28, 10, 752, DateTimeKind.Utc).AddTicks(4528));

            migrationBuilder.UpdateData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "ReviewDate",
                value: new DateTime(2025, 11, 11, 11, 28, 10, 752, DateTimeKind.Utc).AddTicks(4532));

            migrationBuilder.UpdateData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 3,
                column: "ReviewDate",
                value: new DateTime(2025, 11, 11, 11, 28, 10, 752, DateTimeKind.Utc).AddTicks(4534));

            migrationBuilder.UpdateData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 4,
                column: "ReviewDate",
                value: new DateTime(2025, 11, 11, 11, 28, 10, 752, DateTimeKind.Utc).AddTicks(4535));

            migrationBuilder.UpdateData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 5,
                column: "ReviewDate",
                value: new DateTime(2025, 11, 11, 11, 28, 10, 752, DateTimeKind.Utc).AddTicks(4535));

            migrationBuilder.UpdateData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 6,
                column: "ReviewDate",
                value: new DateTime(2025, 11, 11, 11, 28, 10, 752, DateTimeKind.Utc).AddTicks(4536));
        }
    }
}
