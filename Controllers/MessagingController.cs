using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
// project dependencies
using emo_back.Data;
using emo_back.Models;
using emo_back.Services;
using emo_back.ViewModels;
// framework dependencies
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
namespace emo_back.Controllers
{
    [Controller]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]/[action]")]
    public class MessagingController : ControllerBase
    {

        private readonly ILogger<ChannelsController> _logger;
        private IHubContext<MessagingHub, IMessagingHubClient> _hubContext;
        private readonly EmoDbContext _dbContext;
        private readonly MessagingService _messagingService;

        public MessagingController(ILogger<ChannelsController> logger, EmoDbContext dbContext, MessagingService messagingService, IHubContext<MessagingHub, IMessagingHubClient> hubContext)
        {
            _logger = logger;
            _dbContext = dbContext;
            _messagingService = messagingService;
            _hubContext = hubContext;
        }

        [HttpPost]
        [Route("{channelId}")]
        public async Task<IActionResult> SendMessage(int channelId, [FromBody]MessageViewModel message)
        {
            string retMessage = string.Empty;
            var msg = await _messagingService.SendMessage(message);
            try
            {
                await _hubContext.Clients.All.BroadcastMessage(message);
                retMessage = "Success";
            }
            catch (Exception e)
            {
                retMessage = e.ToString();
            }
            return new JsonResult(msg);
        }

        [HttpPost]
        public async Task<IActionResult> UploadStream([FromBody]VideoCaptureViewModel videoCapture)
        {
            string retMessage = string.Empty;

            try
            {
                await _hubContext.Clients.All.BroadcastVideoStream(videoCapture);
            }
            catch (Exception e)
            {
                retMessage = e.Message;
            }

            return new JsonResult("peepee");
        }

    }
}
