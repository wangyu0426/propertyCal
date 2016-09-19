using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using WebApp.Models;
namespace WebApp.Data.Plumping {
        public static class EfExtensions {
            public static void Update<TEntity>(this DbContext dbContext, TEntity entity) where TEntity : class {

                var properties = GetUpdateFields(entity);
                var entry = dbContext.Entry(entity);
                if (entry.State == EntityState.Detached) {
                    dbContext.Set<TEntity>().Attach(entity);
                }
                foreach (var p in properties) {
                    if ((p.GetValue(entity) is string)&&((string)p.GetValue(entity)).Equals("DBNULL", StringComparison.OrdinalIgnoreCase))
                    {
                        p.SetValue(entity,null);
                    }
                    entry.Property(p.Name).IsModified = true;
                }
            }
            //this function reflects fields for specific  purposes, more general version of this function is written into SSData->Plumping->Graphdiff->DbContextExtensions.cs 
            private static IList<PropertyInfo> GetUpdateFields<TEntity>(TEntity entity) {
                var propertyInfo = entity.GetType().GetProperties();
                return propertyInfo
                    .Where(info =>
                        info.PropertyType.IsPrimitive || info.PropertyType == typeof(string)
                        || info.PropertyType == typeof(DateTime?) || info.PropertyType == typeof(DateTime) || info.PropertyType == typeof(Decimal?) || info.PropertyType == typeof(Decimal))
                    .Select(p =>
                        (p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase)
                        || p.GetValue(entity) == null)
                        ? null : p).Where(x => x != null)
                    .ToList();
            }
            public static void UpdateEntityValue<TEntity>(this DbContext dbContext, TEntity target, TEntity source) where TEntity : class {
                var properties = GetUpdateFields(source);
                var entry = dbContext.Entry(target);
                if (entry.State == EntityState.Detached) {
                    dbContext.Set<TEntity>().Attach(target);
                }
                foreach (var p in properties) {
                    entry.Property(p.Name).CurrentValue = p.GetValue(source);
                }
            }
        }
}