﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicBoard.DataServices.Models
{
    public class Lnk_Charts_Users
    {
        public long ID { get; set; }

        public long ChartId { get; set; }

        public string UserId { get; set; }

        public bool? IsDeleted { get; set; }

    }
}
