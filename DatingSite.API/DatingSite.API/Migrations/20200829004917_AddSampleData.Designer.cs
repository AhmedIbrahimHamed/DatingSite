﻿// <auto-generated />
using DatingSite.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DatingSite.API.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20200829004917_AddSampleData")]
    partial class AddSampleData
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DatingSite.API.Models.Value", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Values");

                    b.HasData(
                        new { Id = 1, Name = "Value 101" },
                        new { Id = 2, Name = "Value 102" },
                        new { Id = 3, Name = "Value 103" }
                    );
                });
#pragma warning restore 612, 618
        }
    }
}
