using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicBoard.DataServices
{
    public class ExtendChart:Charts
    {
        public DBConnections DBConnections { get; set; }
        public ChartTypes ChartTypes { get; set; }
    }
}
