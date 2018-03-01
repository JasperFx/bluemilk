﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BlueMilk.Codegen.Frames;
using BlueMilk.Codegen.ServiceLocation;
using BlueMilk.Codegen.Variables;
using BlueMilk.Util;

namespace BlueMilk.Codegen
{


    public class MethodFrameArranger : IMethodVariables
    {
        private readonly GeneratedMethod _method;
        private readonly GeneratedType _type;
        private readonly Dictionary<Type, Variable> _variables = new Dictionary<Type, Variable>();
        private readonly ServiceVariableSource _services;

        public MethodFrameArranger(GeneratedMethod method, GeneratedType type, ServiceGraph services) : this(method, type)
        {
            if (services != null) _services = new ServiceVariableSource(services);
        }

        public MethodFrameArranger(GeneratedMethod method, GeneratedType type)
        {
            _method = method;
            _type = type;

        }


        public void Arrange(out AsyncMode asyncMode, out Frame topFrame)
        {
            var compiled = compileFrames(_method.Frames);



            asyncMode = AsyncMode.AsyncTask;

            if (compiled.All(x => !x.IsAsync))
            {
                asyncMode = AsyncMode.None;
            }
            else if (compiled.Count(x => x.IsAsync) == 1 && compiled.Last().IsAsync && compiled.Last().CanReturnTask())
            {
                asyncMode = compiled.Any(x => x.Wraps) ? AsyncMode.AsyncTask : AsyncMode.ReturnFromLastNode;
            }

            topFrame = chainFrames(compiled);
        }


        protected Frame chainFrames(Frame[] frames)
        {
            // Step 5, put into a chain.
            for (int i = 1; i < frames.Length; i++)
            {
                frames[i - 1].Next = frames[i];
            }

            return frames[0];
        }

        protected Frame[] compileFrames(IList<Frame> frames)
        {
            // Step 1, resolve all the necessary variables
            foreach (var frame in frames)
            {
                frame.ResolveVariables(this);
            }

            // Step 1a;) -- figure out if you can switch to inline service
            // creation instead of the container.
            _services?.ReplaceVariables();

            // Step 2, calculate dependencies
            var dependencies = new DependencyGatherer(this, frames);
            findInjectedFields(dependencies);
            findSetters(dependencies);

            // Step 3, gather any missing frames and
            // add to the beginning of the list
            dependencies.Dependencies.GetAll().SelectMany(x => x).Distinct()
                .Where(x => !frames.Contains(x))
                .Each(x => frames.Insert(0, x));

            // Step 4, topological sort in dependency order
            return frames.TopologicalSort(x => dependencies.Dependencies[x], true).ToArray();
        }

        internal void findInjectedFields(DependencyGatherer dependencies)
        {
            // Stupid. Can't believe I haven't fixed this in Baseline
            var list = new List<InjectedField>();
            dependencies.Variables.Each((key, _) =>
            {
                if (key is InjectedField)
                {
                    list.Add(key.As<InjectedField>());
                }
            });

            _method.Fields = list.ToArray();
        }

        internal void findSetters(DependencyGatherer dependencies)
        {
            var list = new List<Setter>();
            dependencies.Variables.Each((key, _) =>
            {
                if (key is Setter)
                {
                    list.Add(key.As<Setter>());
                }
            });

            _method.Setters = list.ToArray();
        }

        private IEnumerable<IVariableSource> allVariableSources(VariableSource variableSource)
        {
            foreach (var source in _method.Sources)
            {
                yield return source;
            }

            foreach (var source in _type.Rules.Sources)
            {
                yield return source;
            }

            // To get injected fields
            yield return _type;

            if (variableSource == VariableSource.All && _services != null)
            {
                yield return _services;
            }
        }


        private Variable findVariable(Type type, VariableSource variableSource)
        {
            var argument = _method.Arguments.Concat(_method.DerivedVariables).FirstOrDefault(x => x.VariableType == type);
            if (argument != null)
            {
                return argument;
            }

            var created = _method.Frames.SelectMany(x => x.Creates).FirstOrDefault(x => x.VariableType == type);
            if (created != null)
            {
                return created;
            }

            var source = allVariableSources(variableSource).FirstOrDefault(x => x.Matches(type));
            return source?.Create(type);
        }

        public Variable FindVariableByName(Type dependency, string name)
        {
            if (TryFindVariableByName(dependency, name, out var variable)) return variable;

            throw new ArgumentOutOfRangeException(nameof(dependency), $"Cannot find a matching variable {dependency.FullName} {name}");
        }

        public Variable FindVariable(Type type)
        {
            if (_variables.ContainsKey(type))
            {
                return _variables[type];
            }

            var variable = findVariable(type, VariableSource.All);
            if (variable == null)
            {
                throw new ArgumentOutOfRangeException(nameof(type),
                    $"Jasper doesn't know how to build a variable of type '{type.FullName}'");
            }

            _variables.Add(type, variable);

            return variable;
        }


        public bool TryFindVariableByName(Type dependency, string name, out Variable variable)
        {
            variable = null;

            // It's fine here for now that we aren't looking through the services for
            // variables that could potentially be built by the IoC container
            var sourced = _method.Sources.Where(x => x.Matches(dependency)).Select(x => x.Create(dependency));
            var created = _method.Frames.SelectMany(x => x.Creates);

            var candidate = _variables.Values
                .Concat(_method.Arguments)
                .Concat(_method.DerivedVariables)
                .Concat(created)
                .Concat(sourced)
                .Where(x => x != null)
                .FirstOrDefault(x => x.VariableType == dependency && x.Usage == name);


            if (candidate != null)
            {
                variable = candidate;
                return true;
            }

            return false;
        }

        public Variable TryFindVariable(Type type, VariableSource source)
        {
            if (_variables.ContainsKey(type))
            {
                return _variables[type];
            }

            var variable = findVariable(type, source);
            if (variable != null)
            {
                _variables.Add(type, variable);
            }

            return variable;
        }
    }
    }
