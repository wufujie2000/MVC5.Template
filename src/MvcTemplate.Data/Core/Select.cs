using AutoMapper.QueryableExtensions;
using MvcTemplate.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MvcTemplate.Data.Core
{
    public class Select<TModel> : ISelect<TModel> where TModel : BaseModel
    {
        private IQueryable<TModel> set;

        public Type ElementType
        {
            get
            {
                return set.ElementType;
            }
        }
        public Expression Expression
        {
            get
            {
                return set.Expression;
            }
        }
        public IQueryProvider Provider
        {
            get
            {
                return set.Provider;
            }
        }

        public Select(IQueryable<TModel> set)
        {
            this.set = set;
        }

        public ISelect<TModel> Where(Expression<Func<TModel, Boolean>> predicate)
        {
            set = set.Where(predicate);

            return this;
        }

        public IQueryable<TView> To<TView>() where TView : BaseView
        {
            return set.Project().To<TView>();
        }

        public IEnumerator<TModel> GetEnumerator()
        {
            return set.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
