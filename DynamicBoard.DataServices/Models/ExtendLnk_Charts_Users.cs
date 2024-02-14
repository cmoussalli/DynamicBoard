using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicBoard.DataServices.Models
{
    public class ExtendLnk_Charts_Users: Lnk_Charts_Users
    {
        public Charts Charts { get; set; }
        public Users User{ get; set; }
        public ChartTypes ChartTypes { get; set; }
    }
}
