using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MolineMart.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedMoreDataIntoNewlyCreatedTabled : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ProductFeatures",
                columns: new[] { "Id", "FeatureDescription", "FeatureTitle", "ProductId" },
                values: new object[,]
                {
                    { 3, "48MP main camera with cinematic mode", "Camera", 1 },
                    { 4, "Up to 24 hours video playback", "Battery", 1 }
                });

            migrationBuilder.InsertData(
                table: "ProductImages",
                columns: new[] { "Id", "ImageUrl", "ProductId" },
                values: new object[,]
                {
                    { 2, "/images/iphone14pro_back.jpg", 1 },
                    { 3, "/images/iphone14pro_side.jpg", 1 },
                    { 4, "/images/galaxys23ultra_front.jpg", 2 },
                    { 5, "/images/galaxys23ultra_back.jpg", 2 },
                    { 6, "/images/galaxys23ultra_side.jpg", 2 },
                    { 7, "/images/oneplus11r_front.jpg", 3 },
                    { 8, "/images/oneplus11r_back.jpg", 3 },
                    { 9, "/images/pixel8_front.jpg", 4 },
                    { 10, "/images/pixel8_back.jpg", 4 },
                    { 11, "/images/pixel8_side.jpg", 4 }
                });

            migrationBuilder.UpdateData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "ReviewDate",
                value: new DateTime(2025, 11, 11, 10, 41, 43, 42, DateTimeKind.Utc).AddTicks(2328));

            migrationBuilder.InsertData(
                table: "ProductReviews",
                columns: new[] { "Id", "ProductId", "Rating", "ReviewDate", "ReviewText", "UserName" },
                values: new object[,]
                {
                    { 2, 1, 4, new DateTime(2025, 11, 11, 10, 41, 43, 42, DateTimeKind.Utc).AddTicks(2335), "Excellent but expensive.", "Neha Gupta" },
                    { 3, 2, 5, new DateTime(2025, 11, 11, 10, 41, 43, 42, DateTimeKind.Utc).AddTicks(2336), "S Pen is absolutely amazing.", "Rahul Verma" },
                    { 4, 2, 4, new DateTime(2025, 11, 11, 10, 41, 43, 42, DateTimeKind.Utc).AddTicks(2339), "Battery life could be better.", "Priya Singh" },
                    { 5, 3, 5, new DateTime(2025, 11, 11, 10, 41, 43, 42, DateTimeKind.Utc).AddTicks(2340), "Fast performance and super smooth UI.", "Karan Malik" },
                    { 6, 4, 3, new DateTime(2025, 11, 11, 10, 41, 43, 42, DateTimeKind.Utc).AddTicks(2341), "Great software experience – clean and intuitive.", "Anil Kumar" }
                });

            migrationBuilder.InsertData(
                table: "RelatedProducts",
                columns: new[] { "Id", "ProductId", "RelatedProductId" },
                values: new object[,]
                {
                    { 1, 1, 3 },
                    { 2, 1, 4 },
                    { 3, 2, 3 },
                    { 4, 2, 5 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductFeatures",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ProductFeatures",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "RelatedProducts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RelatedProducts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RelatedProducts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "RelatedProducts",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "ReviewDate",
                value: new DateTime(2025, 11, 10, 16, 24, 42, 327, DateTimeKind.Utc).AddTicks(7950));
        }
    }
}
