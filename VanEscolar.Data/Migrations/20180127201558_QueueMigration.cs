using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VanEscolar.Data.Migrations
{
    public partial class QueueMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Travels_TravelId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_TravelsStudent_TravelStudentId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "Travels");

            migrationBuilder.DropIndex(
                name: "IX_Students_TravelId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_TravelStudentId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "TravelId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "TravelStudentId",
                table: "Students");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "TravelsStudent",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "StudentId",
                table: "TravelsStudent",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NeedTravel",
                table: "Students",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Parents",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Queues",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    QueuePosition = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Queues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QueueMembers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    QueuOrder = table.Column<int>(nullable: false),
                    QueueId = table.Column<Guid>(nullable: true),
                    SchoolName = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    StudentID = table.Column<string>(nullable: true),
                    StudentName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueueMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QueueMembers_Queues_QueueId",
                        column: x => x.QueueId,
                        principalTable: "Queues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TravelsStudent_StudentId",
                table: "TravelsStudent",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_QueueMembers_QueueId",
                table: "QueueMembers",
                column: "QueueId");

            migrationBuilder.AddForeignKey(
                name: "FK_TravelsStudent_Students_StudentId",
                table: "TravelsStudent",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TravelsStudent_Students_StudentId",
                table: "TravelsStudent");

            migrationBuilder.DropTable(
                name: "QueueMembers");

            migrationBuilder.DropTable(
                name: "Queues");

            migrationBuilder.DropIndex(
                name: "IX_TravelsStudent_StudentId",
                table: "TravelsStudent");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "TravelsStudent");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "TravelsStudent");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "NeedTravel",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Parents");

            migrationBuilder.AddColumn<Guid>(
                name: "TravelId",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TravelStudentId",
                table: "Students",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Travels",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    NeedTravel = table.Column<bool>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    TravelStudentId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Travels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Travels_TravelsStudent_TravelStudentId",
                        column: x => x.TravelStudentId,
                        principalTable: "TravelsStudent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_TravelId",
                table: "Students",
                column: "TravelId",
                unique: true,
                filter: "[TravelId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Students_TravelStudentId",
                table: "Students",
                column: "TravelStudentId",
                unique: true,
                filter: "[TravelStudentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Travels_TravelStudentId",
                table: "Travels",
                column: "TravelStudentId",
                unique: true,
                filter: "[TravelStudentId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Travels_TravelId",
                table: "Students",
                column: "TravelId",
                principalTable: "Travels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_TravelsStudent_TravelStudentId",
                table: "Students",
                column: "TravelStudentId",
                principalTable: "TravelsStudent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
