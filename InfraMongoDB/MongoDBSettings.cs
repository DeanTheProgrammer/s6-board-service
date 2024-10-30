using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraMongoDB
{
    public class MongoDBSettings
    {
        public const string Settings = "MongoDB";
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
