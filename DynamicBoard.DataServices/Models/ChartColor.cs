using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicBoard.DataServices.Models
{
    public class ChartColor
    {
        public long ID { get; set; }

        public long ThemeID { get; set; }

        public string FillBackgroundRGBA { get; set; }

        public string FillBorderRGB { get; set; }

        public bool IsDeleted { get; set; }

    }

}
