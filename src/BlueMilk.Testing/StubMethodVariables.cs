﻿using System;
using System.Collections.Generic;
using System.Linq;
using BlueMilk.Codegen;
using BlueMilk.Codegen.Variables;

namespace BlueMilk.Testing
{
    public class StubMethodVariables : IMethodVariables
    {
        public readonly Dictionary<Type, Variable> Variables = new Dictionary<Type, Variable>();
        public readonly IList<Variable> Extras = new List<Variable>();
        

        public Variable FindVariable(Type type)
        {
            return Variables[type];
        }

        public Variable FindVariableByName(Type dependency, string name)
        {
            var found = TryFindVariableByName(dependency, name, out Variable variable);
            if (found) return variable;
            
            throw new Exception($"No known variable for {dependency} named {name}");
        }

        public bool TryFindVariableByName(Type dependency, string name, out Variable variable)
        {
            variable = Variables.Values.Concat(Extras).FirstOrDefault(x => x.Usage == name && x.VariableType == dependency);
            return variable != null;
        }

        public Variable TryFindVariable(Type type, VariableSource source)
        {
            return Variables.ContainsKey(type) ? Variables[type] : null;
        }

        public void Store(Variable variable)
        {
            Variables[variable.VariableType] = variable;
            Extras.Add(variable);
        }

        public void Store<T>(string variableName = null)
        {
            var variable = Variable.For<T>(variableName);
            Store(variable);
        }
    }
}