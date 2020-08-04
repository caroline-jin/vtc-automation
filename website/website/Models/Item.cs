using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace website.Models
{
    public class Item
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Extra { get; set; }
        public int count { get; set; }
        public string unique_id { get; set; }
        public string history { get; set; }
    }
}
