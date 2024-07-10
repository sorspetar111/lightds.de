using System;
using System.Collections.Generic;
using System.Text;

namespace TT.Lib.Entities
{
    public class Property : BaseName
    {
        public int ParentId { get; set; }
        public string Value { get; set; }
    }
}
