using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace JBKClubs1.Models
{
    public class KeyValue
    {
        public KeyValue(Int32 _key, string _value)
        {
            Key = _key;
            Value = _value;
        }
        public int Key{ get; set; }
        public string Value { get; set; }
    }
}
