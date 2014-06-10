using AutoMapper.QueryableExtensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Template.Data.Core;
using Template.Objects;
using Template.Tests.Data;
using Template.Tests.Helpers;

namespace Template.Tests.Unit.Data.Core
{
    [TestFixture]
    public class RepositoryTests
    {
        private Repository<Account> repository;
        private AContext context;

        [SetUp]
        public void SetUp()
        {
            context = new TestingContext();
            repository = new Repository<Account>(context);

            TearDownData();
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
            Account account = ObjectFactory.CreateAccount();
            context.Set<Account>().Add(account);
            context.SaveChanges();

            Account expected = context.Set<Account>().SingleOrDefault(acc => acc.Id == account.Id);
            Account actual = repository.GetById(account.Id);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetById_OnModelNotFoundReturnsNull()
        {
            Assert.IsNull(repository.GetById(String.Empty));
        }

        #endregion

        #region Method: Query()

        [Test]
        public void Query_ReturnsContextsSet()
        {
            Assert.AreEqual(context.Set<Account>(), repository.Query());
        }

        #endregion

        #region Method: Query<TView>()

        [Test]
        public void Query_ProjectsContextsSet()
        {
            Account model = ObjectFactory.CreateAccount();
            context.Set<Account>().Add(model);
            context.SaveChanges();

            IEnumerable<String> expected = context.Set<Account>().Project().To<AccountView>().Select(account => account.Id).ToList();
            IEnumerable<String> actual = repository.Query<AccountView>().Select(account => account.Id).ToList();

            CollectionAssert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Query(Expression<Func<TModel, Boolean>> predicate)

        [Test]
        public void Query_FiltersByPredicate()
        {
            Account model1 = ObjectFactory.CreateAccount(1);
            Account model2 = ObjectFactory.CreateAccount(2);
            context.Set<Account>().Add(model1);
            context.Set<Account>().Add(model2);
            context.SaveChanges();

            IEnumerable<Account> expected = context.Set<Account>().Where(account => account.Id == model1.Id).ToList();
            IEnumerable<Account> actual = repository.Query().Where(account => account.Id == model1.Id).ToList();

            TestHelper.EnumPropertyWiseEquals(expected, actual);
        }

        #endregion

        #region Method: Query<TView>(Expression<Func<TModel, Boolean>> predicate)

        [Test]
        public void Query_FiltersProjectedViewsByPredicate()
        {
            Account account1 = ObjectFactory.CreateAccount(1);
            Account account2 = ObjectFactory.CreateAccount(2);
            context.Set<Account>().Add(account1);
            context.Set<Account>().Add(account2);
            context.SaveChanges();

            IEnumerable<AccountView> expected = context.Set<Account>().Where(account => account.Id == account1.Id).Project().To<AccountView>().ToList();
            IEnumerable<AccountView> actual = repository.Query<AccountView>(account => account.Id == account1.Id).ToList();

            TestHelper.EnumPropertyWiseEquals(expected, actual);
        }

        #endregion

        #region Method: Insert(TModel model)

        [Test]
        public void Insert_InsertsModel()
        {
            Account expected = ObjectFactory.CreateAccount();
            repository.Insert(expected);
            context.SaveChanges();

            Account actual = context.Set<Account>().SingleOrDefault(account => account.Id == expected.Id);

            TestHelper.PropertyWiseEquals(expected, actual);
        }

        #endregion

        #region Method: Update(TModel model)

        [Test]
        public void Update_UpdatesAttachedModel()
        {
            Account expected = ObjectFactory.CreateAccount();
            context.Set<Account>().Add(expected);
            context.SaveChanges();

            expected.Username = "Test";
            repository.Update(expected);
            context.SaveChanges();

            Account actual = context.Set<Account>().SingleOrDefault(account => account.Id == expected.Id);

            TestHelper.PropertyWiseEquals(expected, actual);
        }

        [Test]
        public void Update_UpdatesNotAttachedModel()
        {
            Account expected = ObjectFactory.CreateAccount();
            context.Set<Account>().Add(expected);
            context.SaveChanges();

            expected.Username = "Test";
            context = new TestingContext();
            repository = new Repository<Account>(context);
            repository.Update(expected);
            context.SaveChanges();

            Account actual = context.Set<Account>().SingleOrDefault(account => account.Id == expected.Id);

            TestHelper.PropertyWiseEquals(expected, actual);
        }

        [Test]
        public void Update_DoesNotModifyEntityDate()
        {
            Account account = ObjectFactory.CreateAccount();
            repository.Update(account);

            Assert.IsFalse(context.Entry(account).Property(prop => prop.EntityDate).IsModified);
        }

        #endregion

        #region Method: Delete(String id)

        [Test]
        public void Delete_DeletesModelById()
        {
            Account expected = ObjectFactory.CreateAccount();
            repository.Insert(expected);
            context.SaveChanges();

            repository.Delete(expected.Id);
            context.SaveChanges();

            Account actual = context.Set<Account>().SingleOrDefault(account => account.Id == expected.Id);

            Assert.IsNull(actual);
        }

        #endregion

        #region Test helpers

        private void TearDownData()
        {
            context.Set<Account>().RemoveRange(context.Set<Account>().Where(account => account.Id.StartsWith(ObjectFactory.TestId)));
            context.SaveChanges();
        }

        #endregion
    }
}
