using System;
using System.Collections.Generic;

namespace emo_back.Models
{
    public class Channel : BaseEntity
    {
        public string ConversationName { get; set; }
        public List<Message> Messages { get; set; }
    }
}