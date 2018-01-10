﻿using System.Diagnostics;
using System.Linq;
using Baseline;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace BlueMilk.Testing.IoC.Acceptance
{
    public class type_scanning
    {
        public interface ISomeInterface<in T>
        {
        }

        public class Base
        {
        }

        public class Derived : Base
        {
        }

        public class Foo : ISomeInterface<Base>
        {
        }

        // Brought over from StructureMap
        public class Bug_101
        {
            [Fact]
            public void open_generic_scanning()
            {
                var container = new Container(i => i.Scan(s =>
                {
                    s.AssemblyContainingType<Bug_101>();
                    //s.WithDefaultConventions();
                    s.AddAllTypesOf(typeof(ISomeInterface<>));
                }));

                container.GetInstance<ISomeInterface<Base>>()
                    .ShouldNotBeNull();

                container.GetInstance<ISomeInterface<Derived>>()
                    .ShouldBeOfType<Foo>()
                    .ShouldNotBeNull();
            }
        }
        

    }
    
    
    public class Bug_247_ConnectOpenTypesToImplementations_doubling_up_registrations
    {
        [Fact]
        public void Scanner_apply_should_only_register_two_instances()
        {
            var container = new Container(_ =>
            {
                _.Scan(x =>
                {
                    x.TheCallingAssembly();
                    x.ConnectImplementationsToTypesClosing(typeof(ISomeServiceOf<>));
                });
            });

            container.GetAllInstances<ISomeServiceOf<string>>().OrderBy(x => x.GetType().Name).Select(x => x.GetType())
                .ShouldHaveTheSameElementsAs(typeof(SomeService1), typeof(SomeService2));
        }

        public interface ISomeServiceOf<T>
        {
        }

        public class SomeService1 : ISomeServiceOf<string>
        {
        }

        public class SomeService2 : ISomeServiceOf<string>
        {
        }
    }
    
    public class Bug_313
    {
        [Fact]
        public void exclude_type_does_indeed_work()
        {
            var container = new Container(_ =>
            {
                _.Scan(x =>
                {
                    x.TheCallingAssembly();
                    x.AddAllTypesOf<IFoo>();
                    x.ExcludeType<Foo2>();
                });
            });

            container.GetAllInstances<IFoo>()
                .Select(x => x.GetType())
                .ShouldHaveTheSameElementsAs(typeof(Foo1), typeof(Foo3));
        }

        public interface IFoo
        {
        }

        public class Foo1 : IFoo
        {
        }

        public class Foo2 : IFoo
        {
        }

        public class Foo3 : IFoo
        {
        }
    }
    
    public class Bug_320_generic_parent_child_relationship
    {
        public class Parent
        {
        };

        public class Child : Parent
        {
        }

        public class ConcreteChild : Child
        {
        }

        public interface IGeneric<in T> where T : class
        {
        }

        public class GenericClass1 : IGeneric<Parent>
        {
        }

        public class GenericClass2 : IGeneric<Child>
        {
        }

        //public class GenericClass3 : IGeneric<ConcreteChild> { }

        [Fact]
        public void StructureMap_Resolves_Generic_Child_Classes()
        {
            typeof(IGeneric<ConcreteChild>).IsAssignableFrom(typeof(GenericClass1)).ShouldBeTrue();
            typeof(IGeneric<ConcreteChild>).IsAssignableFrom(typeof(GenericClass2)).ShouldBeTrue();
            //Assert.ShouldBeTrue(typeof(IGeneric<ConcreteChild>).IsAssignableFrom(typeof(GenericClass3)));

            var container = new Container(cfg =>
            {
                cfg.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.ConnectImplementationsToTypesClosing(typeof(IGeneric<>));
                });
            });

            var instances = container.GetAllInstances<IGeneric<ConcreteChild>>();

            instances.Any(t => t.GetType() == typeof(GenericClass1)).ShouldBeTrue();
            instances.Any(t => t.GetType() == typeof(GenericClass2)).ShouldBeTrue();
        }
    }
    
    public class Bug_338
    {
        public abstract class ParentClass
        {
        }

        public class ChildClass : ParentClass
        {
        }

        public abstract class OtherChildClass : ParentClass { }

        [Fact]
        public void be_smart_and_do_not_add_abstract_types()
        {
            var container = new Container(_ =>
            {
                _.Scan(x =>
                {
                    x.TheCallingAssembly();
                    x.AddAllTypesOf<ParentClass>();
                });
            });

            container.GetAllInstances<ParentClass>()
                .Single().ShouldBeOfType<ChildClass>();
        }
    }
    
    public class CloseOpenGenericsWithSomeSpecifics
    {
        [Fact]
        public void should_handle_default_closed_and_specific_closed()
        {
            var container = new Container(x =>
            {
                x.Scan(y =>
                {
                    y.TheCallingAssembly();
                    y.ConnectImplementationsToTypesClosing(typeof(IAmOpenGeneric<>));
                });

                x.For(typeof(IAmOpenGeneric<>)).Use(typeof(TheClosedGeneric<>));
            });

            container.GetInstance<IAmOpenGeneric<int>>().ShouldBeOfType<TheClosedGeneric<int>>();
            container.GetInstance<IAmOpenGeneric<string>>().ShouldBeOfType<SpecificClosedGeneric>();
        }

        public interface IAmOpenGeneric<T>
        {
        }

        public class TheClosedGeneric<T> : IAmOpenGeneric<T>
        {
        }

        public class SpecificClosedGeneric : TheClosedGeneric<string>
        {
        }
    }
    
    public class ConnectImplementationsToTypesClosing_is_wonky_in_Registry_added_by_Configure
    {
        [Fact]
        public void has_the_correct_number_by_initialize()
        {
            var container = Container.For<BookRegistry>();
            container.GetAllInstances<IBook<SciFi>>().Count().ShouldBe(1);
        }

        [Fact]
        public void has_the_correct_number_by_configure()
        {
            var container = new Container(new BookRegistry());
            container.GetAllInstances<IBook<SciFi>>().Count().ShouldBe(1);
        }

    }

    public class BookRegistry : ServiceRegistry
    {
        public BookRegistry()
        {
            Scan(x =>
            {
                x.Exclude(type => type == typeof(DustCover<>));
                x.TheCallingAssembly();
                x.ConnectImplementationsToTypesClosing(typeof(IBook<>));
            });
        }
    }


    public class DustCover<T> : IBook<T>
    {
        public IBook<T> Book { get; }

        public DustCover(IBook<T> book)
        {
            Book = book;
        }
    }

    public interface IBook<T>
    {
    }

    public class SciFi
    {
    }

    public class SciFiBook : IBook<SciFi>
    {
    }

    public class Fantasy
    {
    }

    public class FantasyBook : IBook<Fantasy>
    {
    }
    
    
    public class GenericVarianceResolution
    {
        public interface INotificationHandler<in TNotification>
        {
            void Handle(TNotification notification);
        }

        public class BaseNotificationHandler : INotificationHandler<object>
        {
            public void Handle(object notification)
            {
            }
        }

        public class OpenNotificationHandler<TNotification> : INotificationHandler<TNotification>
        {
            public void Handle(TNotification notification)
            {
            }
        }

        public class Notification
        {
        }

        public class ConcreteNotificationHandler : INotificationHandler<Notification>
        {
            public void Handle(Notification notification)
            {
            }
        }

        [Fact]
        public void RegisterMultipleHandlersOfSameInterface()
        {
            typeof(OpenNotificationHandler<Notification>).CanBeCastTo<INotificationHandler<Notification>>()
                .ShouldBeTrue();

            typeof(OpenNotificationHandler<>).CanBeCastTo(typeof(INotificationHandler<>))
                .ShouldBeTrue();

            var container = new Container(x =>
            {
                x.Scan(s =>
                {
                    s.TheCallingAssembly();
                    s.ConnectImplementationsToTypesClosing(typeof(INotificationHandler<>));
                });
            });

            var handlers = container.GetAllInstances<INotificationHandler<Notification>>();

            handlers.Select(x => x.GetType()).OrderBy(x => x.Name)
                .Each(x => Debug.WriteLine(x.Name))
                .ShouldHaveTheSameElementsAs(typeof(BaseNotificationHandler), typeof(ConcreteNotificationHandler),
                    typeof(OpenNotificationHandler<Notification>));
        }
    }
    
    public class Jimmys_open_generics_bug_from_early_MediatR
    {
        [Fact]
        public void fix_it()
        {
            var container = new Container(cfg =>
            {
                cfg.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();

                    scan.ConnectImplementationsToTypesClosing(typeof(IBird<>));
                });
            });

            container.GetAllInstances<IBird<Bird>>()
                .Select(x => x.GetType())
                .ShouldHaveTheSameElementsAs(typeof(BirdImpl), typeof(BirdBaseImpl));
        }
    }

    public interface IBird<in T>
    {
    }

    public class BirdBase
    {
    }

    public class Bird : BirdBase
    {
    }

    public class BirdImpl : IBird<Bird>
    {
    }

    public class BirdBaseImpl : IBird<BirdBase>
    {
    }
    
}