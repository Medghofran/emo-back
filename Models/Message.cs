using System;

namespace emo_back.Models
{
    public class Message : BaseEntity
    {
        public string MessageBody { get; set; }
        public string Emotion { get; set; }

        public int ChannelId { get; set; }
        public Channel Channel { get; set; }

        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}