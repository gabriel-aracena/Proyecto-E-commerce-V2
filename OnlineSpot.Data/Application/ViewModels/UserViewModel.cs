using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSpot.Data.Application.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string password { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Username { get; set; }
        public bool IsActive { get; set; }
    
    }
}
