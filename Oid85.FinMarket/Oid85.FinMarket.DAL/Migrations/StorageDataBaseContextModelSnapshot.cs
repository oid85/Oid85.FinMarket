﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Oid85.FinMarket.DAL;

#nullable disable

namespace Oid85.FinMarket.DAL.Migrations
{
    [DbContext(typeof(StorageDataBaseContext))]
    partial class StorageDataBaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Oid85.FinMarket.DAL.Entities.AssetEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Figi")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("figi");

                    b.Property<string>("Ticker")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("ticker");

                    b.HasKey("Id");

                    b.ToTable("assets", "public");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Figi = "BBG004730N97",
                            Ticker = "SBER"
                        });
                });

            modelBuilder.Entity("Oid85.FinMarket.DAL.Entities.CandleOneDayEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<decimal>("Close")
                        .HasColumnType("numeric")
                        .HasColumnName("close");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("datetime");

                    b.Property<decimal>("High")
                        .HasColumnType("numeric")
                        .HasColumnName("high");

                    b.Property<decimal>("Low")
                        .HasColumnType("numeric")
                        .HasColumnName("low");

                    b.Property<decimal>("Open")
                        .HasColumnType("numeric")
                        .HasColumnName("open");

                    b.Property<string>("Ticker")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("ticker");

                    b.Property<decimal>("Volume")
                        .HasColumnType("numeric")
                        .HasColumnName("volume");

                    b.HasKey("Id");

                    b.ToTable("candles_one_day", "public");
                });

            modelBuilder.Entity("Oid85.FinMarket.DAL.Entities.CandleOneMinuteEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<decimal>("Close")
                        .HasColumnType("numeric")
                        .HasColumnName("close");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("datetime");

                    b.Property<decimal>("High")
                        .HasColumnType("numeric")
                        .HasColumnName("high");

                    b.Property<decimal>("Low")
                        .HasColumnType("numeric")
                        .HasColumnName("low");

                    b.Property<decimal>("Open")
                        .HasColumnType("numeric")
                        .HasColumnName("open");

                    b.Property<string>("Ticker")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("ticker");

                    b.Property<decimal>("Volume")
                        .HasColumnType("numeric")
                        .HasColumnName("volume");

                    b.HasKey("Id");

                    b.ToTable("candles_one_minute", "public");
                });

            modelBuilder.Entity("Oid85.FinMarket.DAL.Entities.MarketEventEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("MarketEventTypeId")
                        .HasColumnType("bigint")
                        .HasColumnName("market_event_type_id");

                    b.Property<string>("Ticker")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("ticker");

                    b.HasKey("Id");

                    b.HasIndex("MarketEventTypeId");

                    b.ToTable("market_events", "public");
                });

            modelBuilder.Entity("Oid85.FinMarket.DAL.Entities.MarketEventTypeEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("market_event_types", "public");
                });

            modelBuilder.Entity("Oid85.FinMarket.DAL.Entities.MarketEventEntity", b =>
                {
                    b.HasOne("Oid85.FinMarket.DAL.Entities.MarketEventTypeEntity", "MarketEventTypeEntity")
                        .WithMany()
                        .HasForeignKey("MarketEventTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MarketEventTypeEntity");
                });
#pragma warning restore 612, 618
        }
    }
}
