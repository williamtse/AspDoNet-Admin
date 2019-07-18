﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MvcMovie.Models;

namespace MvcMovie.Migrations
{
    [DbContext(typeof(MvcMovieContext))]
    partial class MvcMovieContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099");

            modelBuilder.Entity("MvcMovie.Models.Menu", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Icon");

                    b.Property<int>("Order");

                    b.Property<int>("ParentID");

                    b.Property<string>("Permission");

                    b.Property<string>("Title");

                    b.Property<string>("Uri");

                    b.HasKey("ID");

                    b.ToTable("Menu");
                });

            modelBuilder.Entity("MvcMovie.Models.Permission", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("HttpMethods");

                    b.Property<string>("HttpPath");

                    b.Property<string>("Name");

                    b.Property<string>("Slug");

                    b.HasKey("ID");

                    b.ToTable("Permission");
                });

            modelBuilder.Entity("MvcMovie.Models.Role", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("Slug");

                    b.HasKey("ID");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("MvcMovie.Models.RoleMenu", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("MenuID");

                    b.Property<int>("RoleID");

                    b.HasKey("ID");

                    b.ToTable("RoleMenu");
                });

            modelBuilder.Entity("MvcMovie.Models.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Avatar");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<string>("RememberToken");

                    b.Property<string>("Salt");

                    b.Property<string>("Username");

                    b.HasKey("ID");

                    b.ToTable("User");
                });
#pragma warning restore 612, 618
        }
    }
}
