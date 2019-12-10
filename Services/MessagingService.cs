using System;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
// project dependencies
using emo_back.Models;
using emo_back.ViewModels;
using emo_back.Data;
using IBM.Cloud.SDK.Core.Authentication.Iam;
using IBM.Watson.ToneAnalyzer.v3;
using IBM.Watson.ToneAnalyzer.v3.Model;
using Microsoft.Extensions.Logging;

namespace emo_back.Services
{
    public class MessagingService
    {
        private readonly ToneAnalyzerService _toneAnalyser;
        private readonly EmoDbContext _dbContext;
        private readonly ILogger<MessagingService> _logger;

        private readonly string _apiKey = "6lUqRLQlZNopx1dSOoA8_It9FBDQ-9QAKMicMQPkJf8q";
        private readonly string _serviceUrl = "https://gateway-lon.watsonplatform.net/tone-analyzer/api";
        private readonly string _versionDate = "2017-09-21";

        public MessagingService(EmoDbContext context, ILogger<MessagingService> logger)
        {
            _dbContext = context;
            _logger = logger;
            IamAuthenticator authenticator = new IamAuthenticator(apikey: _apiKey);

            _toneAnalyser = new ToneAnalyzerService(_versionDate, authenticator);
            _toneAnalyser.SetServiceUrl(_serviceUrl);
        }

        private String Tone(string body)
        {
            ToneInput toneInput = new ToneInput()
            {
                Text = body
            };

            var result = _toneAnalyser.Tone(toneInput: toneInput);

            // get the tone with the maximum tone score from the list of tones  
            var maxToneScore = result.Result.DocumentTone.Tones.Max(t => t.Score).GetValueOrDefault();
            var tone = result.Result.DocumentTone.Tones.FirstOrDefault(t => t.Score == maxToneScore)?.ToneId;

            result.Result.DocumentTone.Tones.ForEach((t) =>
            {
                _logger.LogInformation($"{t.ToneId} (score):{t.Score}");
            });

            return tone;
        }

        public async Task<MessageViewModel> SendMessage(MessageViewModel messageModel)
        {

            var tone = Tone(messageModel.MessageBody);
            Message message = new Message
            {
                ChannelId = messageModel.ChannelId,
                CreatedAt = DateTime.Now,
                Emotion = tone,
                MessageBody = messageModel.MessageBody,
            };

            await _dbContext.Messages.AddAsync(message);
            await _dbContext.SaveChangesAsync();
            messageModel.Emotion = message.Emotion;
            messageModel.SentAt = message.CreatedAt.ToString("yyyy-MM-dd");

            _logger.LogInformation(tone);
            return messageModel;
        }

    }
}