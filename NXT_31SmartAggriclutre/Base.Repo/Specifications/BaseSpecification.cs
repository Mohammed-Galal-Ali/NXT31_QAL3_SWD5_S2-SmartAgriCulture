using Base.DAL.Models.BaseModels;
using Base.Repo.Specifications;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryProject.Specifications
{
    public class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDescending { get; set; }

        public List<Func<IQueryable<T>, IIncludableQueryable<T, object>>> AllIncludes { get; set; } = new List<Func<IQueryable<T>, IIncludableQueryable<T, object>>>();

        public int Skip { get; private set; }
        public int Take { get; private set; }
        public bool IsPagingEnabled { get; private set; }

        public BaseSpecification()
        {
            //Includes = new List<Expression<Func<T, object>>>();
        }
        public BaseSpecification(Expression<Func<T, bool>> CriteriaExpression)
        {
            Criteria = CriteriaExpression;
            //Includes = new List<Expression<Func<T, object>>>();
        }

        // Fluent methods
        public BaseSpecification<T> Where(Expression<Func<T, bool>> criteria)
        {
            Criteria = Criteria == null ? criteria : Criteria.And(criteria);
            return this;
        }

        public BaseSpecification<T> AddInclude(Expression<Func<T, object>> include)
        {
            Includes.Add(include);
            return this;
        }

        public BaseSpecification<T> AddInclude(Func<IQueryable<T>, IIncludableQueryable<T, object>> includeFunc)
        {
            AllIncludes.Add(includeFunc);
            return this;
        }

        public void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        public void AddOrderByDesc(Expression<Func<T, object>> orderByDescExpression)
        {
            OrderByDescending = orderByDescExpression;
        }
        public void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }
    }
    // Helper extension for combining expressions
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            if (first == null) return second;
            if (second == null) return first;

            var parameter = first.Parameters[0];

            var visitor = new ReplaceParameterVisitor(second.Parameters[0], parameter);
            var body = Expression.AndAlso(first.Body, visitor.Visit(second.Body)!);

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        private class ReplaceParameterVisitor : ExpressionVisitor
        {
            private readonly ParameterExpression _oldParameter;
            private readonly ParameterExpression _newParameter;

            public ReplaceParameterVisitor(ParameterExpression oldParam, ParameterExpression newParam)
            {
                _oldParameter = oldParam;
                _newParameter = newParam;
            }

            protected override Expression VisitParameter(ParameterExpression node)
                => node == _oldParameter ? _newParameter : base.VisitParameter(node);
        }
    }
}
