﻿// <auto-generated />
using System;
using CameraSystem.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CameraSystem.Server.Migrations
{
    [DbContext(typeof(VideoDataDb))]
    [Migration("20211209001527_PrimaryKeyFix1")]
    partial class PrimaryKeyFix1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.0");

            modelBuilder.Entity("CameraSystem.Shared.LoggedCameraTelemetry", b =>
                {
                    b.Property<string>("CardId")
                        .HasColumnType("character varying")
                        .HasColumnName("card_id");

                    b.Property<string>("Log")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("video_log");

                    b.Property<int>("LogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("log_id");

                    b.Property<DateTime>("PassDateTime")
                        .HasColumnType("datetime")
                        .HasColumnName("pass_datetime");

                    b.HasKey("CardId")
                        .HasName("log_ids_pkey");

                    b.ToTable("camera_telemetry_data", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}