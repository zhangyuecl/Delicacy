﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace EF
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CommentRecord> CommentRecord { get; set; }
        public virtual DbSet<CookBook> CookBook { get; set; }
        public virtual DbSet<CookMaterial> CookMaterial { get; set; }
        public virtual DbSet<CookProcess> CookProcess { get; set; }
        public virtual DbSet<FoodMaterial> FoodMaterial { get; set; }
        public virtual DbSet<FoodMaterial_CookBook> FoodMaterial_CookBook { get; set; }
        public virtual DbSet<FoodSort> FoodSort { get; set; }
        public virtual DbSet<LikeCookBook> LikeCookBook { get; set; }
        public virtual DbSet<SubjectArticle> SubjectArticle { get; set; }
        public virtual DbSet<SubjectSort> SubjectSort { get; set; }
        public virtual DbSet<SupportScanRecord> SupportScanRecord { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Taste> Taste { get; set; }
        public virtual DbSet<UserInfo> UserInfo { get; set; }
        public virtual DbSet<VerifyRegister> VerifyRegister { get; set; }
        public virtual DbSet<AdminUser> AdminUser { get; set; }
    }
}
