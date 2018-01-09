using Shouldly;
using Xunit;

namespace StructureMap.Testing
{
    public class PrimitiveArrayTester
    {
        [Fact]
        public void specify_a_string_array()
        {
            var container = new Container(x =>
            {
                x.ForConcreteType<ClassWithStringAndIntArray>().Configure
                    .Ctor<string[]>().Is(new[] { "a", "b", "c" })
                    .Ctor<int[]>().Is(new[] { 1, 2, 3 });
            });

            var objectWithArrays = container.GetInstance<ClassWithStringAndIntArray>();
            objectWithArrays.Numbers.ShouldBe(new[] { 1, 2, 3 });
            objectWithArrays.Strings.ShouldBe(new[] { "a", "b", "c" });
        }

        public class ClassWithStringAndIntArray
        {
            private readonly int[] _numbers;
            private readonly string[] _strings;

            public ClassWithStringAndIntArray(int[] numbers, string[] strings)
            {
                _numbers = numbers;
                _strings = strings;
            }

            public int[] Numbers
            {
                get { return _numbers; }
            }

            public string[] Strings
            {
                get { return _strings; }
            }
        }
    }
}