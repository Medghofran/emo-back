using System.Threading.Tasks;
//project dependencies
using emo_back.ViewModels;

namespace emo_back
{
    public interface IMessagingHubClient
    {
        Task BroadcastMessage(MessageViewModel message);
        Task BroadcastVideoStream(VideoCaptureViewModel videoCapture);
    }
}