using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicBoard.DataServices.Models
{
    public class ChartThemeExtends : Charts
    {
        public ChartColorTheme ChartColorTheme { get; set; }
        public ChartColor ChartColor { get; set; }
    }
}
