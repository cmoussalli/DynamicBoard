using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicBoard.DataServices
{
    public class DBProvider
    {
        public long ID { get; set; }

        public string Title { get; set; }

        public string ConnectionStringTemplate { get; set; }
    }
}
