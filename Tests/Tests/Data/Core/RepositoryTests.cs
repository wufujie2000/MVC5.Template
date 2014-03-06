using AutoMapper.QueryableExtensions;
using NUnit.Framework;
using System;
using System.Linq;
using Template.Data.Core;
using Template.Objects;
using Template.Tests.Data;

namespace Template.Tests.Tests.Data.Core
{
    [TestFixture]
    public class RepositoryTests
    {
        private Repository<User> repository;
        private AContext context;

        [SetUp]
        public void SetUp()
        {
            context = new TestingContext();
            repository = new Repository<User>(context);
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }

        #region Method: GetById(Object id)

        [Test]
        public void GetById_GetsModelById()
        {
            var user = new User();
            context.Set<User>().Add(user);
            context.SaveChanges();

            var actual = repository.GetById(user.Id);
            context.Set<User>().Remove(user);
            context.SaveChanges();

            Assert.AreEqual(user.Id, actual.Id);
        }

        [Test]
        public void GetById_OnNotFoundReturnsNull()
        {
            Assert.IsNull(repository.GetById(String.Empty));
        }

        #endregion

        #region Method: Query()

        [Test]
        public void Query_ReturnsDbSet()
        {
            Assert.AreEqual(context.Set<User>(), repository.Query());
        }

        #endregion

        #region Method: Query<TView>()

        [Test]
        public void Query_ProjectsDbSet()
        {
            var model = new User();
            context.Set<User>().Add(model);
            context.SaveChanges();

            var expected = context.Set<User>().Project().To<UserView>().Select(user => user.Id);
            var actual = repository.Query<UserView>().Select(user => user.Id);

            context.Set<User>().Remove(model);
            context.SaveChanges();

            CollectionAssert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Query(Expression<Func<TModel, Boolean>> predicate)

        [Test]
        public void Query_FiltersByPredicate()
        {
            var model = new User();
            context.Set<User>().Add(model);
            context.SaveChanges();
            
            var expected = context.Set<User>().Where(user => user.Id == "Test").Select(user => user.Id);
            var actual = repository.Query().Where(user => user.Id == "Test").Select(user => user.Id);

            context.Set<User>().Remove(model);
            context.SaveChanges();

            CollectionAssert.AreEqual(expected, actual);
        }
        
        #endregion

        #region Method: Query<TView>(Expression<Func<TModel, Boolean>> predicate)

        [Test]
        public void Query_FiltersProjectedByPredicate()
        {
            var model = new User();
            context.Set<User>().Add(model);
            context.SaveChanges();

            var expected = context.Set<User>().Project().To<UserView>().Where(user => user.Id == "Test").Select(user => user.Id);
            var actual = repository.Query<UserView>(user => user.Id == "Test").Select(user => user.Id);

            context.Set<User>().Remove(model);
            context.SaveChanges();

            CollectionAssert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Insert(TModel model)

        [Test]
        public void Insert_InsertsModel()
        {
            var expected = new User();
            repository.Insert(expected);
            context.SaveChanges();

            var actual = context.Set<User>().Find(expected.Id);
            context.Set<User>().Remove(expected);
            context.SaveChanges();

            Assert.AreEqual(expected.Id, actual.Id);
        }

        #endregion

        #region Method: Update(TModel model)

        [Test]
        public void Update_UpdatesAttachedModel()
        {
            var expected = new User();
            context.Set<User>().Add(expected);
            context.SaveChanges();

            expected.FirstName = "Test";
            repository.Update(expected);
            context.SaveChanges();

            var actual = context.Set<User>().Find(expected.Id);
            context.Set<User>().Remove(expected);
            context.SaveChanges();

            Assert.AreEqual(expected.FirstName, actual.FirstName);
        }

        [Test]
        public void Update_UpdatesNotAttachedModel()
        {
            var expected = new User();
            context.Set<User>().Add(expected);
            context.SaveChanges();

            expected.FirstName = "Test";
            context = new TestingContext();
            repository = new Repository<User>(context);
            repository.Update(expected);
            context.SaveChanges();

            var actual = context.Set<User>().Find(expected.Id);
            context.Set<User>().Remove(expected);
            context.SaveChanges();

            Assert.AreEqual(expected.FirstName, actual.FirstName);
        }

        #endregion

        #region Method: Delete(TModel model)

        [Test]
        public void Delete_DeletesModel()
        {
            var expected = new User();
            repository.Insert(expected);
            context.SaveChanges();

            repository.Delete(expected);
            context.SaveChanges();

            var actual = context.Set<User>().Find(expected.Id);
            if (actual != null)
            {
                context.Set<User>().Remove(expected);
                context.SaveChanges();
            }

            Assert.IsNull(actual);
        }

        #endregion

        #region Method: Delete(String id)

        [Test]
        public void Delete_DeletesModelById()
        {
            var expected = new User();
            repository.Insert(expected);
            context.SaveChanges();

            repository.Delete(expected.Id);
            context.SaveChanges();

            var actual = context.Set<User>().Find(expected.Id);
            if (actual != null)
            {
                context.Set<User>().Remove(expected);
                context.SaveChanges();
            }

            Assert.IsNull(actual);
        }

        #endregion
    }
}
