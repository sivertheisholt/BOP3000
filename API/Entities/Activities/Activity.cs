using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities.Activities
{
    public class Activity
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Identifier { get; set; }
        public string Text { get; set; }

    }
}