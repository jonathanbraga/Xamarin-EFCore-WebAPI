﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using VanEscolar.Data;

namespace VanEscolar.Data.Migrations
{
    [DbContext(typeof(VanEscolarContext))]
    [Migration("20171221200410_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("VanEscolar.Domain.Parent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("City");

                    b.Property<string>("Complement");

                    b.Property<string>("Email");

                    b.Property<string>("Name");

                    b.Property<string>("Neighborhood");

                    b.Property<int>("Number");

                    b.Property<string>("Phone");

                    b.Property<string>("Street");

                    b.HasKey("Id");

                    b.ToTable("Parents");
                });

            modelBuilder.Entity("VanEscolar.Domain.Scholl", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("City");

                    b.Property<string>("Complement");

                    b.Property<string>("Name");

                    b.Property<string>("Neighborhood");

                    b.Property<int>("Number");

                    b.Property<string>("Phone");

                    b.Property<string>("Street");

                    b.HasKey("Id");

                    b.ToTable("Scholls");
                });

            modelBuilder.Entity("VanEscolar.Domain.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("InScholl");

                    b.Property<string>("Name");

                    b.Property<DateTime>("OutScholl");

                    b.Property<int?>("ParentId");

                    b.Property<int?>("SchollId");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.HasIndex("SchollId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("VanEscolar.Domain.Student", b =>
                {
                    b.HasOne("VanEscolar.Domain.Parent", "Parent")
                        .WithMany("Students")
                        .HasForeignKey("ParentId");

                    b.HasOne("VanEscolar.Domain.Scholl", "Scholl")
                        .WithMany("Students")
                        .HasForeignKey("SchollId");
                });
#pragma warning restore 612, 618
        }
    }
}