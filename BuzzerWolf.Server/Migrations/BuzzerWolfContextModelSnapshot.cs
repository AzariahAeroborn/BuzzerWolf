﻿// <auto-generated />
using System;
using BuzzerWolf.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BuzzerWolf.Server.Migrations
{
    [DbContext(typeof(BuzzerWolfContext))]
    partial class BuzzerWolfContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0");

            modelBuilder.Entity("BuzzerWolf.Server.Models.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Divisions")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("FirstSeason")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("BuzzerWolf.Server.Models.PromotionStanding", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConferenceName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ConferenceRank")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Country")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Division")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsAutoPromotion")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsBotPromotion")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsChampionPromotion")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsTotalPromotion")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LeagueName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Losses")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PointDifference")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PromotionRank")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Season")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TeamId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TeamName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Wins")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("PromotionStandings");
                });

            modelBuilder.Entity("BuzzerWolf.Server.Models.Season", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("Finish")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("Start")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Seasons");
                });

            modelBuilder.Entity("BuzzerWolf.Server.Models.Sync", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("LastSync")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("NextAutoSync")
                        .HasColumnType("TEXT");

                    b.Property<string>("Params")
                        .HasColumnType("TEXT");

                    b.Property<string>("Table")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("TeamId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Sync");
                });
#pragma warning restore 612, 618
        }
    }
}
