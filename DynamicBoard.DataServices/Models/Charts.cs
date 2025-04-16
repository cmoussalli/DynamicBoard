namespace DynamicBoard.DataServices
{
    public class Charts
    {
        public long ID { get; set; }

        public long ChartTypeID { get; set; }
        public long ChartTheme { get; set; }

        public long DBConnectionID { get; set; }

        public string DataScript { get; set; }

        public string TitleEn { get; set; }

        public string TitleAr { get; set; }

        public string SubtitleEn { get; set; }

        public string SubtitleAr { get; set; }

        public bool Display { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; } = false;
        public int RefershTime { get; set; } = 0;
        public string CreatedBy { get; set; } = "test user";


    }

}
