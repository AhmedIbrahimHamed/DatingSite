﻿using DatingSite.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingSite.API.Data {
    public class DataContext : DbContext {
        public DataContext(DbContextOptions options) : base(options) {

        }

        public DbSet<Value> Values { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Photo> Photos { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder) {
        //    modelBuilder.Entity<Value>()
        //        .HasData(new Value() {
        //            Id = 1,
        //            Name = "Value 101"
        //        }, new Value() {
        //            Id = 2,
        //            Name = "Value 102"
        //        }, new Value() {
        //            Id = 3,
        //            Name = "Value 103"
        //        });

        //    base.OnModelCreating(modelBuilder);
        //}
    }
}
