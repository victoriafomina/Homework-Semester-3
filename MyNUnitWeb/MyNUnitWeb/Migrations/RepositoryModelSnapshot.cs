﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyNUnitWeb.Models;

namespace MyNUnitWeb.Migrations
{
    [DbContext(typeof(Repository))]
    partial class RepositoryModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MyNUnitWeb.Models.AssemblyViewModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Assemblies");
                });

            modelBuilder.Entity("MyNUnitWeb.Models.TestViewModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AssemblyId")
                        .HasColumnType("int");

                    b.Property<string>("ClassName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("ElapsedTime")
                        .HasColumnType("time");

                    b.Property<string>("IgnoreMessage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Ignored")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Passed")
                        .HasColumnType("bit");

                    b.Property<DateTime>("TestLaunchTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AssemblyId");

                    b.ToTable("Tests");
                });

            modelBuilder.Entity("MyNUnitWeb.Models.TestViewModel", b =>
                {
                    b.HasOne("MyNUnitWeb.Models.AssemblyViewModel", "Assembly")
                        .WithMany("Tests")
                        .HasForeignKey("AssemblyId");

                    b.Navigation("Assembly");
                });

            modelBuilder.Entity("MyNUnitWeb.Models.AssemblyViewModel", b =>
                {
                    b.Navigation("Tests");
                });
#pragma warning restore 612, 618
        }
    }
}