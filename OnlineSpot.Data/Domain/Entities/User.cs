using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSpot.Data.Domain.Entities
{
    public class User
    {
        public  required Guid id { get; set; }
        public required string name { get; set; }
        public required  int role { get; set; }
        public required string lastName { get; set; }
        public string userName { get; set; } = null!;
        public string email { get; set; } = null!;
        public string Tel { get; set; }
        public string passwordHash { get; set; } = null!;
        public DateTime createdAt { get; set; }
        public bool IsActive { get; set; } = false;
      

    }
}
