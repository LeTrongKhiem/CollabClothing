using System;
using System.Collections.Generic;

#nullable disable

namespace CollabClothing.WebApp.Models
{
    public partial class Topic
    {
        public Topic()
        {
            Contacts = new HashSet<Contact>();
        }

        public string Id { get; set; }
        public string NameTopic { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; }
    }
}
