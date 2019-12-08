using System;

namespace emo_back.ViewModels
{
    public class MessageViewModel
    {
        public string MessageBody { get; set; }
        public string Emotion { get; set; }
        public string SentAt { get; set; }
        public string Username { get; set; }
        public int ChannelId { get; set; }
    }
}