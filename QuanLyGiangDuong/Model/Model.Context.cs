﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuanLyGiangDuong.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class QLGDEntities : DbContext
    {
        public QLGDEntities()
            : base("name=QLGDEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CLASS> CLASSes { get; set; }
        public virtual DbSet<EVENT_> EVENT_ { get; set; }
        public virtual DbSet<EXAM> EXAMs { get; set; }
        public virtual DbSet<FACAULTY> FACAULTies { get; set; }
        public virtual DbSet<LECTURER> LECTURERs { get; set; }
        public virtual DbSet<PERIOD_TIMERANGE> PERIOD_TIMERANGE { get; set; }
        public virtual DbSet<ROOM> ROOMs { get; set; }
        public virtual DbSet<ROOMIGNORE> ROOMIGNOREs { get; set; }
        public virtual DbSet<SUBJECT_> SUBJECT_ { get; set; }
        public virtual DbSet<TRAINING_PROGRAM> TRAINING_PROGRAM { get; set; }
        public virtual DbSet<USINGCLASS> USINGCLASSes { get; set; }
        public virtual DbSet<USINGEVENT> USINGEVENTs { get; set; }
        public virtual DbSet<USINGEXAM> USINGEXAMs { get; set; }
    }
}
