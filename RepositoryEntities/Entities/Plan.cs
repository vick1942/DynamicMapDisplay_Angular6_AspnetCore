using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Entities
{
    public class Plan
    {
        public string Number { get; set; }
        public string Name { get; set; }
        public List<Network> Networks { get; set; }
    }
}