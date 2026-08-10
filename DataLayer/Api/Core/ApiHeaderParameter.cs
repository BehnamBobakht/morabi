using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Api
{
    public class ApiHeaderParameter
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public ApiHeaderParameter() { }

        public ApiHeaderParameter(string Name, string Value)
        {
            this.Name = Name;
            this.Value = Value;
        }
    }
}
