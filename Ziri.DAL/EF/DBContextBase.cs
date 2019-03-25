using System;
using System.Data.Entity;
using System.Data.Common;
using Ziri.MDL;

namespace Ziri.DAL
{
    public abstract class DBContextBase : DbContext, IDisposable
    {
        protected DBContextBase(DbConnection DBCN, bool contextOwnsConnection = true) : base(DBCN, contextOwnsConnection)
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //商品数量
            modelBuilder.Entity<GoodsCounter>().Property(i => i.Quantity).HasPrecision(18, 0);
            //经纬值
            modelBuilder.Entity<ContactInfo>().Property(i => i.Latitude).HasPrecision(18, 15);
            modelBuilder.Entity<ContactInfo>().Property(i => i.Longitude).HasPrecision(18, 15);
            //订单金额
            modelBuilder.Entity<SalesOrderItem>().Property(i => i.Price).HasPrecision(18, 2);
            modelBuilder.Entity<SalesOrderItem>().Property(i => i.Amount).HasPrecision(18, 2);
        }
    }
}
