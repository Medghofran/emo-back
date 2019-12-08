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
using emo_back.Services;

namespace emo_back.Controllers
{
    [Controller]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]/[action]")]
    public class MessagingController : ControllerBase
    {

        private readonly ILogger<ChannelsController> _logger;
        private readonly EmoDbContext _dbContext;
        private readonly MessagingService _messagingService;

        public MessagingController(ILogger<ChannelsController> logger, EmoDbContext dbContext, MessagingService messagingService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _messagingService = messagingService;
        }

        [HttpPost]
        [Route("{channelId}")]
        public async Task<IActionResult> SendMessage(MessageViewModel message)
        {
            var msg = await _messagingService.SendMessage(message);

            return new JsonResult(msg);
        }

    }
}
