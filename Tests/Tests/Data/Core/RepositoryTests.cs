using AutoMapper.QueryableExtensions;
using NUnit.Framework;
using System;
using System.Linq;
using Template.Data.Core;
using Template.Objects;
using Template.Tests.Data;
using Template.Tests.Helpers;

namespace Template.Tests.Tests.Data.Core
{
    [TestFixture]
    public class RepositoryTests
    {
        private Repository<Person> repository;
        private AContext context;

        [SetUp]
        public void SetUp()
        {
            context = new TestingContext();
            repository = new Repository<Person>(context);
            context.Set<Person>().RemoveRange(context.Set<Person>().Where(person => person.Id.StartsWith(ObjectFactory.TestId)));
            context.SaveChanges();
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
            var person = ObjectFactory.CreatePerson();
            context.Set<Person>().Add(person);
            context.SaveChanges();

            var expected = context.Set<Person>().Find(person.Id);
            var actual = repository.GetById(person.Id);
            context.Set<Person>().Remove(person);
            context.SaveChanges();

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
            Assert.AreEqual(context.Set<Person>(), repository.Query());
        }

        #endregion

        #region Method: Query<TView>()

        [Test]
        public void Query_ProjectsContextsSet()
        {
            var model = ObjectFactory.CreatePerson();
            context.Set<Person>().Add(model);
            context.SaveChanges();

            var expected = context.Set<Person>().Project().To<PersonView>().Select(person => person.Id).ToList();
            var actual = repository.Query<PersonView>().Select(person => person.Id).ToList();

            context.Set<Person>().Remove(model);
            context.SaveChanges();

            CollectionAssert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Query(Expression<Func<TModel, Boolean>> predicate)

        [Test]
        public void Query_FiltersByPredicate()
        {
            var model1 = ObjectFactory.CreatePerson(1);
            var model2 = ObjectFactory.CreatePerson(2);
            context.Set<Person>().Add(model1);
            context.Set<Person>().Add(model2);
            context.SaveChanges();

            var expected = context.Set<Person>().Where(person => person.Id == model1.Id).ToList();
            var actual = repository.Query().Where(person => person.Id == model1.Id).ToList();

            context.Set<Person>().Remove(model1);
            context.Set<Person>().Remove(model2);
            context.SaveChanges();

            TestHelper.EnumPropertyWiseEquals(expected, actual);
        }
        
        #endregion

        #region Method: Query<TView>(Expression<Func<TModel, Boolean>> predicate)

        [Test]
        public void Query_FiltersProjectedViewsByPredicate()
        {
            var person1 = ObjectFactory.CreatePerson(1);
            var person2 = ObjectFactory.CreatePerson(2);
            context.Set<Person>().Add(person1);
            context.Set<Person>().Add(person2);
            context.SaveChanges();
            
            var expected = context.Set<Person>().Where(person => person.Id == person1.Id).Project().To<PersonView>().ToList();
            var actual = repository.Query<PersonView>(person => person.Id == person1.Id).ToList();
            
            context.Set<Person>().Remove(person1);
            context.Set<Person>().Remove(person2);
            context.SaveChanges();

            TestHelper.EnumPropertyWiseEquals(expected, actual);
        }

        #endregion

        #region Method: Insert(TModel model)

        [Test]
        public void Insert_InsertsModel()
        {
            var expected = ObjectFactory.CreatePerson();
            repository.Insert(expected);
            context.SaveChanges();

            var actual = context.Set<Person>().Find(expected.Id);
            context.Set<Person>().Remove(expected);
            context.SaveChanges();

            TestHelper.PropertyWiseEquals(expected, actual);
        }

        #endregion

        #region Method: Update(TModel model)

        [Test]
        public void Update_UpdatesAttachedModel()
        {
            var expected = ObjectFactory.CreatePerson();
            context.Set<Person>().Add(expected);
            context.SaveChanges();

            expected.FirstName = "Test";
            repository.Update(expected);
            context.SaveChanges();

            var actual = context.Set<Person>().Find(expected.Id);
            context.Set<Person>().Remove(expected);
            context.SaveChanges();

            TestHelper.PropertyWiseEquals(expected, actual);
        }

        [Test]
        public void Update_UpdatesNotAttachedModel()
        {
            var expected = ObjectFactory.CreatePerson();
            context.Set<Person>().Add(expected);
            context.SaveChanges();

            expected.FirstName = "Test";
            context = new TestingContext();
            repository = new Repository<Person>(context);
            repository.Update(expected);
            context.SaveChanges();

            var actual = context.Set<Person>().Find(expected.Id);
            context.Set<Person>().Remove(expected);
            context.SaveChanges();

            TestHelper.PropertyWiseEquals(expected, actual);
        }

        [Test]
        public void Update_DoesNotModifyEntityDate()
        {
            var person = ObjectFactory.CreatePerson();
            repository.Update(person);

            Assert.IsFalse(context.Entry(person).Property(prop => prop.EntityDate).IsModified);
        }

        #endregion

        #region Method: Delete(String id)

        [Test]
        public void Delete_DeletesModelById()
        {
            var expected = ObjectFactory.CreatePerson();
            repository.Insert(expected);
            context.SaveChanges();

            repository.Delete(expected.Id);
            context.SaveChanges();

            var actual = context.Set<Person>().Find(expected.Id);
            if (actual != null)
            {
                context.Set<Person>().Remove(expected);
                context.SaveChanges();
            }

            Assert.IsNull(actual);
        }

        #endregion
    }
}
