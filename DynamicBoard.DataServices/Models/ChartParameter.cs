using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicBoard.DataServices.Models
{
    public class ChartParameter
    {
        public long Id { get; set; }

        public long ChartId { get; set; }

        public string Tag { get; set; }

        public string SQLPlaceHolder { get; set; }
        

        public bool IsRequired { get; set; }

        public string DefaultValue { get; set; }

        public string DefaultValueV2 { get; set; }

        public string ChangValues { get; set; }

    }

}
