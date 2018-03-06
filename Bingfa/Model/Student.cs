using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bingfa.Model
{
    public class Student
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Pwd { get; set; }
        public int Age { get; set; }
        public DateTime LastChanged { get; set; }
    }
}
