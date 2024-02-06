﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TuwaiqInternal.Data;

#nullable disable

namespace TuwaiqRecruitment.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240205084259_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("TuwaiqInternal.Data.ChecksHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("FileUrl")
                        .HasColumnType("longtext");

                    b.Property<string>("FirstName")
                        .HasColumnType("longtext");

                    b.Property<string>("FourthName")
                        .HasColumnType("longtext");

                    b.Property<string>("IdentitiesList")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("LastName")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("LastUpdate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("SecondName")
                        .HasColumnType("longtext");

                    b.Property<string>("Status")
                        .HasColumnType("longtext");

                    b.Property<string>("ThirdName")
                        .HasColumnType("longtext");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("ChecksHistory", (string)null);
                });

            modelBuilder.Entity("TuwaiqInternal.Data.ToBeChecked", b =>
                {
                    b.Property<string>("NationalId")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime?>("CheckedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("FirstName")
                        .HasColumnType("longtext");

                    b.Property<string>("FourthName")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsChecked")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("IsRegistered")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .HasColumnType("longtext");

                    b.Property<string>("Response")
                        .HasColumnType("longtext");

                    b.Property<string>("SecondName")
                        .HasColumnType("longtext");

                    b.Property<string>("ThirdName")
                        .HasColumnType("longtext");

                    b.HasKey("NationalId");

                    b.ToTable("ToBeChecked", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
