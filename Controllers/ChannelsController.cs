using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
// project dependencies
using emo_back.Data;
using emo_back.Models;
using emo_back.ViewModels;
// framework dependencies
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace emo_back.Controllers
{
    [Controller]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]/[action]")]
    public class ChannelsController : ControllerBase
    {

        private readonly ILogger<ChannelsController> _logger;
        private readonly EmoDbContext _dbContext;


        public ChannelsController(ILogger<ChannelsController> logger, EmoDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetChannels()
        {
            var result = await _dbContext.Channels.Select(c => new ChannelViewModel
            {
                ChannelId = c.ID,
                ChannelName = c.ConversationName
            }).ToListAsync();

            return new JsonResult(result);
        }

        [HttpGet]
        public async Task<IActionResult> AddChannel(ChannelViewModel channel)
        {
            if (!ModelState.IsValid)
                return new JsonResult("failed");

            Channel channelModel = new Channel
            {
                ConversationName = channel.ChannelName,
                CreatedAt = DateTime.Now
            };
            _dbContext.Entry(channelModel).State = EntityState.Added;
            await _dbContext.SaveChangesAsync();

            return new JsonResult(channelModel);
        }

        [Route("{channelId}")]
        public async Task<IActionResult> GetMessages(int channelId)
        {
            var messages = await _dbContext.Messages
            .Where(m => m.ChannelId == channelId)
            .Include(m => m.User)
            .Select(m => new MessageViewModel
            {
                ChannelId = channelId,
                MessageBody = m.MessageBody,
                Emotion = m.Emotion,
                SentAt = m.CreatedAt.ToString("dd/MM/yyyy"),
                Username = m.User.UserName
            }).ToListAsync();

            return new JsonResult(messages);
        }

    }
}
