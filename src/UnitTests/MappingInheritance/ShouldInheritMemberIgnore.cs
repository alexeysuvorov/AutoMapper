using AutoMapper.Mappers;

using NUnit.Framework;

namespace AutoMapper.UnitTests.MappingInheritance {
    [TestFixture]
    public class ShouldInheritMemberIgnore {
        public class BaseClass {
            public string PropToIgnore { get; set; }
        }
        public class Class : BaseClass { }

        public class BaseDto {
            public string PropToIgnore { get; set; }
        }
        public class Dto : BaseDto { }

        [Test]
        public void should_inherit_ignore() {
            // arrange
            var source = new Class { PropToIgnore = "test" };
            var configurationProvider = new ConfigurationStore(new TypeMapFactory(), MapperRegistry.AllMappers());
            
            configurationProvider
                .CreateMap<BaseClass, BaseDto>()
                .ForMember(p => p.PropToIgnore, x=>x.Ignore())
                .Include<Class, Dto>();

            configurationProvider.CreateMap<Class, Dto>();
            var mappingEngine = new MappingEngine(configurationProvider);

            // act
            var dest = mappingEngine.Map<Class, Dto>(source);

            // assert
            Assert.AreNotEqual("test", dest.PropToIgnore);
        }
    }
}