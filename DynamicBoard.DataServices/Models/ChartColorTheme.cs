﻿namespace DynamicBoard.DataServices.Models
{
    public class ChartColorTheme
    {
        public long ID { get; set; }

        public string Title { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string CreatedUser { get; set; }

    }

}
