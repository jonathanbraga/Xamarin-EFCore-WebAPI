using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VanEscolar.Data.Migrations
{
    public partial class AuthMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Parents");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                schema: "dbo",
                table: "Users",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "dbo",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TravelStudentId",
                table: "Travels",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TravelStudentId",
                table: "Students",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateAt = table.Column<DateTime>(nullable: false),
                    MessageType = table.Column<int>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Parents_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Parents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TravelsStudent",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FinishAt = table.Column<DateTime>(nullable: false),
                    StartAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TravelsStudent", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Travels_TravelStudentId",
                table: "Travels",
                column: "TravelStudentId",
                unique: true,
                filter: "[TravelStudentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Students_TravelStudentId",
                table: "Students",
                column: "TravelStudentId",
                unique: true,
                filter: "[TravelStudentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ParentId",
                table: "Messages",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_TravelsStudent_TravelStudentId",
                table: "Students",
                column: "TravelStudentId",
                principalTable: "TravelsStudent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Travels_TravelsStudent_TravelStudentId",
                table: "Travels",
                column: "TravelStudentId",
                principalTable: "TravelsStudent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_TravelsStudent_TravelStudentId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Travels_TravelsStudent_TravelStudentId",
                table: "Travels");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "TravelsStudent");

            migrationBuilder.DropIndex(
                name: "IX_Travels_TravelStudentId",
                table: "Travels");

            migrationBuilder.DropIndex(
                name: "IX_Students_TravelStudentId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "CreateAt",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TravelStudentId",
                table: "Travels");

            migrationBuilder.DropColumn(
                name: "TravelStudentId",
                table: "Students");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Parents",
                nullable: true);
        }
    }
}
