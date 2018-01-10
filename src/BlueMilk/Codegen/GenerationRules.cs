﻿using System.Collections.Generic;
using System.Reflection;
using BlueMilk.Codegen.Variables;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.Codegen
{
    public class GenerationRules
    {
        public GenerationRules(string applicationNamespace)
        {
            ApplicationNamespace = applicationNamespace;
        }

        public string ApplicationNamespace { get; }

        public readonly IList<IVariableSource> Sources = new List<IVariableSource>();

        public readonly IList<Assembly> Assemblies = new List<Assembly>();

        public void ReadServices(IServiceCollection services)
        {
            Services = new OldServiceGraph(services);
        }

        public OldServiceGraph Services { get; private set; } = new OldServiceGraph(new ServiceRegistry());
    }


}
