﻿using System;
using System.Collections.Generic;
using System.Linq;
using BlueMilk.Codegen;
using BlueMilk.IoC.Frames;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.IoC.Planning
{
    public class EnumerableStep : BuildStep
    {
        private static readonly List<Type> _enumerableTypes = new List<Type>
        {
            typeof (IEnumerable<>),
            typeof (IList<>),
            typeof (IReadOnlyList<>),
            typeof (List<>)
        };

        private readonly BuildStep[] _childSteps;

        public static bool IsEnumerable(Type type)
        {
            if (type.IsArray) return true;

            return type.IsGenericType && _enumerableTypes.Contains(type.GetGenericTypeDefinition());
        }

        public static Type DetermineElementType(Type serviceType)
        {
            if (serviceType.IsArray)
            {
                return serviceType.GetElementType();
            }

            return serviceType.GetGenericArguments().First();
        }

        public EnumerableStep(Type serviceType, BuildStep[] childSteps) : base(serviceType, ServiceLifetime.Transient)
        {
            _childSteps = childSteps;
        }

        public override IEnumerable<BuildStep> ReadDependencies(BuildStepPlanner planner)
        {
            return _childSteps.SelectMany(x => x.ReadDependencies(planner));
        }

        protected override Variable buildVariable(BuildMode mode)
        {
            var elements = _childSteps.Select(x => x.CreateVariable(mode)).ToArray();
            return ServiceType.IsArray
                ? new ArrayAssignmentFrame(DetermineElementType(ServiceType), elements).Variable
                : new ListAssignmentFrame(ServiceType, elements).Variable;
        }
    }
}