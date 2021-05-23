﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DragonSouvenirs.Data.Migrations
{
    public partial class EditFavouriteProductsEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "FavouriteProducts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "FavouriteProducts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "FavouriteProducts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_FavouriteProducts_IsDeleted",
                table: "FavouriteProducts",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FavouriteProducts_IsDeleted",
                table: "FavouriteProducts");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "FavouriteProducts");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "FavouriteProducts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "FavouriteProducts");
        }
    }
}
