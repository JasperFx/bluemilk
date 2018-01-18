﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Baseline;
using BlueMilk.Codegen;
using BlueMilk.IoC.Instances;

namespace BlueMilk.IoC.Diagnostics
{
    public class ModelQuery
    {
        public string Namespace;
        public Type ServiceType;
        public Assembly Assembly;
        public string TypeName;

        public IEnumerable<IServiceFamilyConfiguration> Query(IModel model)
        {
            var enumerable = model.ServiceTypes;

            if (Namespace.IsNotEmpty())
            {
                enumerable = enumerable.Where(x => x.ServiceType.IsInNamespace(Namespace));
            }

            if (ServiceType != null)
            {
                enumerable = enumerable.Where(x => x.ServiceType == ServiceType);
            }

            if (Assembly != null)
            {
                enumerable = enumerable.Where(x => x.ServiceType.GetTypeInfo().Assembly == Assembly);
            }

            if (TypeName.IsNotEmpty())
            {
                enumerable = enumerable.Where(x => x.ServiceType.Name.ToLower().Contains(TypeName.ToLower()));
            }

            return enumerable;
        }
    }

    public class WhatDoIHaveWriter
    {
        private readonly IModel _graph;
        private List<Instance> _instances;
        private TextReportWriter _writer;
        private readonly StringWriter _stringWriter = new StringWriter();

        public WhatDoIHaveWriter(IModel graph)
        {
            _graph = graph;
        }

        public string GetText(ModelQuery query, string title = null)
        {
            if (title.IsNotEmpty())
            {
                _stringWriter.WriteLine(title);
            }

            _stringWriter.WriteLine("");

            var model = _graph;

            var serviceTypes = query.Query(model);

            writeContentsOfServiceTypes(serviceTypes);

            return _stringWriter.ToString();
        }

        private void writeContentsOfServiceTypes(IEnumerable<IServiceFamilyConfiguration> serviceTypes)
        {
            _writer = new TextReportWriter(5);
            _instances = new List<Instance>();

            _writer.AddDivider('=');
            _writer.AddText("ServiceType", "Namespace", "Lifecycle", "Description", "Name");

            serviceTypes.Where(x => x.Instances.Any()).OrderBy(x => x.ServiceType.Name).Each(writeServiceType);

            _writer.AddDivider('=');

            _writer.Write(_stringWriter);
        }

        private void writeServiceType(IServiceFamilyConfiguration serviceType)
        {
            _writer.AddDivider('-');
            var contents = new[]
            {
                "{0}".ToFormat(serviceType.ServiceType.NameInCode()),
                serviceType.ServiceType.Namespace,
                string.Empty,
                string.Empty,
                string.Empty
            };

            var instances = serviceType.Instances.ToArray();
            
            setContents(contents, instances[0], instances[0].Name, instances[0] == serviceType.Default);
            _writer.AddText(contents);


            for (int i = 1; i < serviceType.Instances.Count(); i++)
            {
                writeInstance(instances[i], serviceType);
            }

        }

        private void writeInstance(Instance instance, IServiceFamilyConfiguration serviceType, string name = null)
        {
            if (_instances.Contains(instance) || instance == null)
            {
                return;
            }

            var contents = new[] {string.Empty, string.Empty, string.Empty, string.Empty, string.Empty};

            setContents(contents, instance, name, instance == serviceType.Default);

            _writer.AddText(contents);
        }


        private void setContents(string[] contents, Instance instance, string name, bool isDefault)
        {
            contents[2] = instance.Lifetime.ToString();

            contents[3] = instance.ToString();

            contents[4] = instance.Name;

            if (isDefault)
            {
                if (contents[4].IsEmpty())
                {
                    contents[4] = "(Default)";
                }
                else
                {
                    contents[4] += " (Default)";
                }
            }

            if (contents[4].Length > 30)
            {
                contents[4] = contents[4].Substring(0, 27) + "...";
            }


            _instances.Add(instance);
        }
    }
}