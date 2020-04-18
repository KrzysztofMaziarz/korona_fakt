using System;
using System.Collections.Generic;
using System.Text;

namespace FakeNewsCovid.Domain.Settings
{
    public class ElasticsearchSettings
    {
        public string ConnectionString { get; set; }

        public string DefaultIndex { get; set; }
    }
}
