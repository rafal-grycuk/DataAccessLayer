using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Core.EntityFramework.Utilities
{
    public static class Extensions
    {
        public static void AddOrUpdate<TEntity>(this DbContext context, Expression<Func<TEntity, bool>> predicate, TEntity entity) where TEntity : class
        {
            var filteredEntity = context.Set<TEntity>().FirstOrDefaultAsync(predicate).Result;
            if (filteredEntity == null)
                context.Entry(entity).State = EntityState.Added;
            else
            {
                var primaryKeyNames = context.Model.FindEntityType(typeof(TEntity)).FindPrimaryKey().Properties.Select(x => x.Name).ToList();
                context.Entry(filteredEntity).State = EntityState.Detached;
                primaryKeyNames.ForEach(pk =>
                {
                    var primaryKeyValue = filteredEntity.GetType().GetProperty(pk).GetValue(filteredEntity, null);

                    entity.GetType().GetProperty(pk).SetValue(entity, primaryKeyValue);
                });
                context.Entry(entity).State = EntityState.Modified;
            }
        }
    }
}
