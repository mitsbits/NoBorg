using System;

namespace Borg.Infra.Tests
{
    internal class ObjectToSerialize
    {
        public double Numeric { get; set; }
        public Guid Id { get; set; }
        public string Textual { get; set; }
    }
}