using System;
using System.Collections.Generic;

namespace HatchwaysBlog.Models
{
    public class Post
    {
        public long id { get; set; }
        public string author { get; set; }
        public long authorId { get; set; }
        public long likes { get; set; }
        public long popularity { get; set; }
        public long reads { get; set; }
        public string[] tags { get; set;}
    }
}


