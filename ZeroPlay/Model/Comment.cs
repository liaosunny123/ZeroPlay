using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;

namespace ZeroPlay.Model
{
    public class Comment
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string Content { get; set; }
        public string CreateDate { get; set; }
    }
}
