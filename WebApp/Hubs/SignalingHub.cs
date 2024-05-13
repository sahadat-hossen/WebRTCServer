using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WebRTCServer.Hubs
{
    public class SignalingHub : Hub
    {
     
        public const string HubUrl = "/signalingHub";
        private IWebHostEnvironment _webHost;



        public SignalingHub(IWebHostEnvironment webHost)
        {

            _webHost = webHost;
          

        }
        public override Task OnConnectedAsync()
        {
            string connectionId = Context.ConnectionId;
            
            return base.OnConnectedAsync();
        }
        public async Task SendOffer(string connectionId, object offer)
        {
            await Clients.Client(connectionId).SendAsync("ReceiveOffer", offer);
        }

        // Method to send an answer to a specific client
        public async Task SendAnswer(string connectionId, object answer)
        {
            await Clients.Client(connectionId).SendAsync("ReceiveAnswer", answer);
        }

        // Method to notify a client about an incoming call
        public async Task SendCall(string connectionId)
        {
            await Clients.Client(connectionId).SendAsync("ReceiveCall");
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
  
            return base.OnDisconnectedAsync(exception);
        }

    }
    



}
