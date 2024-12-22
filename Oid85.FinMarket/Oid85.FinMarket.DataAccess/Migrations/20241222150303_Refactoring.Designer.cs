﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Oid85.FinMarket.DataAccess;

#nullable disable

namespace Oid85.FinMarket.DataAccess.Migrations
{
    [DbContext(typeof(FinMarketContext))]
    [Migration("20241222150303_Refactoring")]
    partial class Refactoring
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("public")
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityAlwaysColumns(modelBuilder);

            modelBuilder.Entity("Oid85.FinMarket.DataAccess.Entities.AnalyseResultEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("AnalyseType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("analyse_type");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date");

                    b.Property<string>("Result")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("result");

                    b.Property<string>("Ticker")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("ticker");

                    b.Property<string>("Timeframe")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("timeframe");

                    b.HasKey("Id")
                        .HasName("pk_analyse_results");

                    b.HasIndex("Ticker")
                        .HasDatabaseName("ix_analyse_results_ticker");

                    b.ToTable("analyse_results", "storage");
                });

            modelBuilder.Entity("Oid85.FinMarket.DataAccess.Entities.BondCouponEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<DateOnly>("CouponDate")
                        .HasColumnType("date")
                        .HasColumnName("coupon_date");

                    b.Property<DateOnly>("CouponEndDate")
                        .HasColumnType("date")
                        .HasColumnName("coupon_end_date");

                    b.Property<long>("CouponNumber")
                        .HasColumnType("bigint")
                        .HasColumnName("coupon_number");

                    b.Property<int>("CouponPeriod")
                        .HasColumnType("integer")
                        .HasColumnName("coupon_period");

                    b.Property<DateOnly>("CouponStartDate")
                        .HasColumnType("date")
                        .HasColumnName("coupon_start_date");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime>("DeletedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_at");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<double>("PayOneBond")
                        .HasColumnType("double precision")
                        .HasColumnName("pay_one_bond");

                    b.Property<double>("Price")
                        .HasColumnType("double precision")
                        .HasColumnName("price");

                    b.Property<string>("Ticker")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("ticker");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_bond_coupons");

                    b.ToTable("bond_coupons", "public");
                });

            modelBuilder.Entity("Oid85.FinMarket.DataAccess.Entities.BondEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime>("DeletedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_at");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Figi")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("figi");

                    b.Property<bool>("FloatingCouponFlag")
                        .HasColumnType("boolean")
                        .HasColumnName("floating_coupon_flag");

                    b.Property<bool>("InWatchList")
                        .HasColumnType("boolean")
                        .HasColumnName("in_watch_list");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<string>("Isin")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("isin");

                    b.Property<DateOnly>("MaturityDate")
                        .HasColumnType("date")
                        .HasColumnName("maturity_date");

                    b.Property<double>("NKD")
                        .HasColumnType("double precision")
                        .HasColumnName("nkd");

                    b.Property<double>("Price")
                        .HasColumnType("double precision")
                        .HasColumnName("price");

                    b.Property<string>("Sector")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("sector");

                    b.Property<string>("Ticker")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("ticker");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_bonds");

                    b.ToTable("bonds", "public");
                });

            modelBuilder.Entity("Oid85.FinMarket.DataAccess.Entities.CandleEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<double>("Close")
                        .HasColumnType("double precision")
                        .HasColumnName("close");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date");

                    b.Property<double>("High")
                        .HasColumnType("double precision")
                        .HasColumnName("high");

                    b.Property<bool>("IsComplete")
                        .HasColumnType("boolean")
                        .HasColumnName("is_complete");

                    b.Property<double>("Low")
                        .HasColumnType("double precision")
                        .HasColumnName("low");

                    b.Property<double>("Open")
                        .HasColumnType("double precision")
                        .HasColumnName("open");

                    b.Property<string>("Ticker")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("ticker");

                    b.Property<string>("Timeframe")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("timeframe");

                    b.Property<long>("Volume")
                        .HasColumnType("bigint")
                        .HasColumnName("volume");

                    b.HasKey("Id")
                        .HasName("pk_candles");

                    b.HasIndex("Ticker")
                        .HasDatabaseName("ix_candles_ticker");

                    b.ToTable("candles", "storage");
                });

            modelBuilder.Entity("Oid85.FinMarket.DataAccess.Entities.CurrencyEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("ClassCode")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("class_code");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime>("DeletedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_at");

                    b.Property<string>("Figi")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("figi");

                    b.Property<bool>("InWatchList")
                        .HasColumnType("boolean")
                        .HasColumnName("in_watch_list");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<string>("Isin")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("isin");

                    b.Property<string>("IsoCurrencyName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("iso_currency_name");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<double>("Price")
                        .HasColumnType("double precision")
                        .HasColumnName("price");

                    b.Property<string>("Ticker")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("ticker");

                    b.Property<string>("Uid")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("uid");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_currencies");

                    b.ToTable("currencies", "public");
                });

            modelBuilder.Entity("Oid85.FinMarket.DataAccess.Entities.DividendInfoEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateOnly>("DeclaredDate")
                        .HasColumnType("date")
                        .HasColumnName("declared_date");

                    b.Property<DateTime>("DeletedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_at");

                    b.Property<double>("Dividend")
                        .HasColumnType("double precision")
                        .HasColumnName("dividend");

                    b.Property<double>("DividendPrc")
                        .HasColumnType("double precision")
                        .HasColumnName("dividend_prc");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<DateOnly>("RecordDate")
                        .HasColumnType("date")
                        .HasColumnName("record_date");

                    b.Property<string>("Ticker")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("ticker");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_dividend_infos");

                    b.ToTable("dividend_infos", "public");
                });

            modelBuilder.Entity("Oid85.FinMarket.DataAccess.Entities.FutureEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime>("DeletedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_at");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<DateOnly>("ExpirationDate")
                        .HasColumnType("date")
                        .HasColumnName("expiration_date");

                    b.Property<string>("Figi")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("figi");

                    b.Property<bool>("InWatchList")
                        .HasColumnType("boolean")
                        .HasColumnName("in_watch_list");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<double>("Price")
                        .HasColumnType("double precision")
                        .HasColumnName("price");

                    b.Property<string>("Ticker")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("ticker");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_futures");

                    b.ToTable("futures", "public");
                });

            modelBuilder.Entity("Oid85.FinMarket.DataAccess.Entities.IndicativeEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("ClassCode")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("class_code");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("currency");

                    b.Property<DateTime>("DeletedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_at");

                    b.Property<string>("Exchange")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("exchange");

                    b.Property<string>("Figi")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("figi");

                    b.Property<bool>("InWatchList")
                        .HasColumnType("boolean")
                        .HasColumnName("in_watch_list");

                    b.Property<string>("InstrumentKind")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("instrument_kind");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<double>("Price")
                        .HasColumnType("double precision")
                        .HasColumnName("price");

                    b.Property<string>("Ticker")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("ticker");

                    b.Property<string>("Uid")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("uid");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_indicatives");

                    b.ToTable("indicatives", "public");
                });

            modelBuilder.Entity("Oid85.FinMarket.DataAccess.Entities.ShareEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime>("DeletedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_at");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Figi")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("figi");

                    b.Property<bool>("InWatchList")
                        .HasColumnType("boolean")
                        .HasColumnName("in_watch_list");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<string>("Isin")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("isin");

                    b.Property<double>("Price")
                        .HasColumnType("double precision")
                        .HasColumnName("price");

                    b.Property<string>("Sector")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("sector");

                    b.Property<string>("Ticker")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("ticker");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_shares");

                    b.ToTable("shares", "public");
                });
#pragma warning restore 612, 618
        }
    }
}
