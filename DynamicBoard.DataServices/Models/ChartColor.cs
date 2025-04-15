namespace DynamicBoard.DataServices.Models
{
    public class ChartColor
    {
        public long ID { get; set; }

        public long ThemeID { get; set; }

        public string FillBackgroundRGBA { get; set; }

        public string FillBorderRGB { get; set; }

        public bool IsDeleted { get; set; }
        public int OrderID { get; set; }
    }

}
