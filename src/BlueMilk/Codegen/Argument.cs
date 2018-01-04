﻿using System;

namespace BlueMilk.Codegen
{
    public class Argument : Variable
    {
        public Argument(Type variableType, string usage) : base(variableType, usage)
        {
        }

        public string Declaration => $"{VariableType.FullName} {Usage}";

        public new static Argument For<T>(string argName = null)
        {
            return new Argument(typeof(T), argName ?? DefaultArgName(typeof(T)));
        }

        protected bool Equals(Argument other)
        {
            return VariableType == other.VariableType && string.Equals(Usage, other.Usage);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Argument)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((VariableType != null ? VariableType.GetHashCode() : 0) * 397) ^ (Usage != null ? Usage.GetHashCode() : 0);
            }
        }
    }
}