using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keepi.Shared
{
    [Table("PostsTbl")]

    public class Post
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; }
        public string Content { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }

    }
}
