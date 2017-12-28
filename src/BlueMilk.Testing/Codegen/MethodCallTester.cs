using System.Linq;
using System.Threading.Tasks;
using BlueMilk.Codegen;
using NSubstitute;
using Shouldly;
using Xunit;
using Xunit.Sdk;

namespace BlueMilk.Testing.Codegen
{
    public class MethodCallTester
    {
        [Fact]
        public void determine_return_value_of_simple_type()
        {
            var @call = MethodCall.For<MethodCallTarget>(x => x.GetValue());
            @call.ReturnVariable.ShouldNotBeNull();

            @call.ReturnVariable.VariableType.ShouldBe(typeof(string));
            @call.ReturnVariable.Usage.ShouldBe("result_of_GetValue");
            @call.ReturnVariable.Creator.ShouldBeSameAs(@call);
        }

        [Fact]
        public void determine_return_value_of_not_simple_type()
        {
            var @call = MethodCall.For<MethodCallTarget>(x => x.GetError());
            @call.ReturnVariable.ShouldNotBeNull();

            @call.ReturnVariable.VariableType.ShouldBe(typeof(ErrorMessage));
            @call.ReturnVariable.Usage.ShouldBe(Variable.DefaultArgName(typeof(ErrorMessage)));
            @call.ReturnVariable.Creator.ShouldBeSameAs(@call);
        }

        [Fact]
        public void no_return_variable_on_void_method()
        {
            var @call = MethodCall.For<MethodCallTarget>(x => x.Go(null));
            @call.ReturnVariable.ShouldBeNull();
        }

        [Fact]
        public void determine_return_value_of_Task_of_T_simple()
        {
            var @call = MethodCall.For<MethodCallTarget>(x => x.GetString());
            @call.ReturnVariable.ShouldNotBeNull();

            @call.ReturnVariable.VariableType.ShouldBe(typeof(string));
            @call.ReturnVariable.Usage.ShouldBe("result_of_GetString");
            @call.ReturnVariable.Creator.ShouldBeSameAs(@call);
        }


        [Fact]
        public void determine_return_value_of_not_simple_type_in_a_task()
        {
            var @call = MethodCall.For<MethodCallTarget>(x => x.GetAsyncError());
            @call.ReturnVariable.ShouldNotBeNull();

            @call.ReturnVariable.VariableType.ShouldBe(typeof(ErrorMessage));
            @call.ReturnVariable.Usage.ShouldBe(Variable.DefaultArgName(typeof(ErrorMessage)));
            @call.ReturnVariable.Creator.ShouldBeSameAs(@call);
        }

        [Fact]
        public void explicitly_set_parameter_by_variable_type()
        {
            var @call = MethodCall.For<MethodCallTarget>(x => x.DoSomething(0, 0, null));

            var stringVariable = Variable.For<string>();
            var generalInt = Variable.For<int>();

            // Only one of that type, so it works
            @call.TrySetParameter(stringVariable)
                .ShouldBeTrue();

            @call.Variables[2].ShouldBeSameAs(stringVariable);

            // Multiple parameters of the same type, nothing
            @call.TrySetParameter(generalInt).ShouldBeFalse();
            @call.Variables[0].ShouldBeNull();
            @call.Variables[1].ShouldBeNull();
        }

        [Fact]
        public void explicitly_set_parameter_by_variable_type_and_name()
        {
            var @call = MethodCall.For<MethodCallTarget>(x => x.DoSomething(0, 0, null));

            var generalInt = Variable.For<int>();

            @call.TrySetParameter("count", generalInt)
                .ShouldBeTrue();

            @call.Variables[0].ShouldBeNull();
            @call.Variables[1].ShouldBeSameAs(generalInt);
            @call.Variables[2].ShouldBeNull();
        }

        [Fact]
        public void find_handler_if_not_local()
        {
            var @call = MethodCall.For<MethodCallTarget>(x => x.GetValue());
            var chain = Substitute.For<IMethodVariables>();

            var handler = Variable.For<MethodCallTarget>();
            chain.FindVariable(typeof(MethodCallTarget)).Returns(handler);
            
            @call.FindVariables(chain).Single()
                .ShouldBeSameAs(handler);
        }
        
        [Fact]
        public void find_no_handler_if_local()
        {
            var @call = MethodCall.For<MethodCallTarget>(x => x.GetValue());
            @call.IsLocal = true;
            
            var chain = Substitute.For<IMethodVariables>();

            var handler = Variable.For<MethodCallTarget>();
            chain.FindVariable(typeof(MethodCallTarget)).Returns(handler);
            
            @call.FindVariables(chain).Any().ShouldBeFalse();
        }

        [Fact]
        public void find_no_handler_variable_if_it_is_static()
        {
            var @call = new MethodCall(typeof(MethodCallTarget), nameof(MethodCallTarget.GoStatic));
            @call.IsLocal = true;
            
            var chain = Substitute.For<IMethodVariables>();

            var handler = Variable.For<MethodCallTarget>();
            chain.FindVariable(typeof(MethodCallTarget)).Returns(handler);
            
            @call.FindVariables(chain).Any().ShouldBeFalse();
        }
        
        /* TESTS
         1. Fill simple values by name and type
         2. Fill simple values by falling down to type only

         5. FindVariables returns set parameters
         
         
         */

        [Fact]
        public void use_a_type_alias()
        {
            var variables = new StubMethodVariables();
            var basketball = Variable.For<Basketball>();
            variables.Store(basketball);

            var @call = MethodCall.For<MethodCallTarget>(x => x.Throw(null));
            @call.IsLocal = true;
            
            @call.Aliases[typeof(Ball)] = typeof(Basketball);
            
            @call.FindVariables(variables)
                .Single()
                .ShouldBeOfType<CastVariable>()
                .Inner
                .ShouldBeSameAs(basketball);
        }
    }

    public class Ball
    {
        
    }

    public class Basketball : Ball
    {
        
    }

    public class MethodCallTarget
    {
        public void Throw(Ball ball)
        {
            
        }
        
        public string GetValue()
        {
            return "foo";
        }

        public static void GoStatic()
        {
            
        }

        public ErrorMessage GetError()
        {
            return null;
        }

        public Task<ErrorMessage> GetAsyncError()
        {
            return null;
        }

        public void Go(string text)
        {

        }

        public void DoSomething(int age, int count, string name)
        {

        }

        public Task<string> GetString()
        {
            return Task.FromResult("foo");
        }
    }
}
