using System;
using System.Collections.Generic;
using System.Text;

namespace Space_CLib.Models
{
    class Message
    {
        public int MessageID { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; }

        public User UserMessaged { get; set; }

        public User MessagedUser { get; set; }


    }
}
