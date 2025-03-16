using Core.Entities;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public static class SpecificationEvalutor<T> where T : BaseEntity
    {
        
        public static IQueryable<T> GetQuery(IQueryable<T> InputQuery, ISpecifications<T> spec)
        {
            var Query = InputQuery;

            if(spec.Criteria is not null)
            {
                Query = Query.Where(spec.Criteria);
            }

            if(spec.OrderBy is not null)
            {
                Query = Query.OrderBy(spec.OrderBy);
            }
            if(spec.OrderByDescending is not null)
            {
                Query = Query.OrderByDescending(spec.OrderByDescending);
            }
            if (spec.IsPaginationEnabled)
            {
                Query = Query.Skip(spec.Skip).Take(spec.Take);
            }

            Query = spec.Includes.Aggregate(Query, (CurrentQuery, IncludeExpression) => CurrentQuery.Include(IncludeExpression));

            return Query;
        }
    }
}
