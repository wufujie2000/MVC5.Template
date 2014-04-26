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
            Person person = ObjectFactory.CreatePerson();
            context.Set<Person>().Add(person);
            context.SaveChanges();

            Person expected = context.Set<Person>().Find(person.Id);
            Person actual = repository.GetById(person.Id);
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
            Person model = ObjectFactory.CreatePerson();
            context.Set<Person>().Add(model);
            context.SaveChanges();

            IEnumerable<String> expected = context.Set<Person>().Project().To<PersonView>().Select(person => person.Id).ToList();
            IEnumerable<String> actual = repository.Query<PersonView>().Select(person => person.Id).ToList();

            context.Set<Person>().Remove(model);
            context.SaveChanges();

            CollectionAssert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Query(Expression<Func<TModel, Boolean>> predicate)

        [Test]
        public void Query_FiltersByPredicate()
        {
            Person model1 = ObjectFactory.CreatePerson(1);
            Person model2 = ObjectFactory.CreatePerson(2);
            context.Set<Person>().Add(model1);
            context.Set<Person>().Add(model2);
            context.SaveChanges();

            IEnumerable<Person> expected = context.Set<Person>().Where(person => person.Id == model1.Id).ToList();
            IEnumerable<Person> actual = repository.Query().Where(person => person.Id == model1.Id).ToList();

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
            Person person1 = ObjectFactory.CreatePerson(1);
            Person person2 = ObjectFactory.CreatePerson(2);
            context.Set<Person>().Add(person1);
            context.Set<Person>().Add(person2);
            context.SaveChanges();

            IEnumerable<PersonView> expected = context.Set<Person>().Where(person => person.Id == person1.Id).Project().To<PersonView>().ToList();
            IEnumerable<PersonView> actual = repository.Query<PersonView>(person => person.Id == person1.Id).ToList();
            
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
            Person expected = ObjectFactory.CreatePerson();
            repository.Insert(expected);
            context.SaveChanges();

            Person actual = context.Set<Person>().Find(expected.Id);
            context.Set<Person>().Remove(expected);
            context.SaveChanges();

            TestHelper.PropertyWiseEquals(expected, actual);
        }

        #endregion

        #region Method: Update(TModel model)

        [Test]
        public void Update_UpdatesAttachedModel()
        {
            Person expected = ObjectFactory.CreatePerson();
            context.Set<Person>().Add(expected);
            context.SaveChanges();

            expected.FirstName = "Test";
            repository.Update(expected);
            context.SaveChanges();

            Person actual = context.Set<Person>().Find(expected.Id);
            context.Set<Person>().Remove(expected);
            context.SaveChanges();

            TestHelper.PropertyWiseEquals(expected, actual);
        }

        [Test]
        public void Update_UpdatesNotAttachedModel()
        {
            Person expected = ObjectFactory.CreatePerson();
            context.Set<Person>().Add(expected);
            context.SaveChanges();

            expected.FirstName = "Test";
            context = new TestingContext();
            repository = new Repository<Person>(context);
            repository.Update(expected);
            context.SaveChanges();

            Person actual = context.Set<Person>().Find(expected.Id);
            context.Set<Person>().Remove(expected);
            context.SaveChanges();

            TestHelper.PropertyWiseEquals(expected, actual);
        }

        [Test]
        public void Update_DoesNotModifyEntityDate()
        {
            Person person = ObjectFactory.CreatePerson();
            repository.Update(person);

            Assert.IsFalse(context.Entry(person).Property(prop => prop.EntityDate).IsModified);
        }

        #endregion

        #region Method: Delete(String id)

        [Test]
        public void Delete_DeletesModelById()
        {
            Person expected = ObjectFactory.CreatePerson();
            repository.Insert(expected);
            context.SaveChanges();

            repository.Delete(expected.Id);
            context.SaveChanges();

            Person actual = context.Set<Person>().Find(expected.Id);
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
