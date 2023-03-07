using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM.DbModel
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public int CreatedByUserId { get; set; }

        public int LastUpdatedByUserId { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public DateTime LastUpdatedOnUtc { get; set; }

    }
}
