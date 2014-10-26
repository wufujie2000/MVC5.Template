using AutoMapper;
using MvcTemplate.Tests.Objects;

namespace MvcTemplate.Tests.Data.Mapping
{
    public class TestObjectMapper
    {
        public static void MapObjects()
        {
            Mapper.CreateMap<TestModel, TestView>();
            Mapper.CreateMap<TestView, TestModel>();
        }
    }
}
