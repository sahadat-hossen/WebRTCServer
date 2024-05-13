WebRTC APIs

WebRTC mainly works on three APIs:

<div><ul><li>MediaStream</li><li style="font-size: 14pt;">RTCPeerConnection</li><li>RTCDataChannel&nbsp;</li></ul></div>


<p><strong>MediaStream</strong></p>

<code>  <script src="https://cdnjs.cloudflare.com/ajax/libs/microsoft-signalr/3.1.7/signalr.js"></script>
  <script>
      // Connect to SignalR hub
      const connection = new signalR.HubConnectionBuilder().withUrl("/signalingHub").build();

      let localStream;
      let remoteStream;
      let peerConnection;

      const localVideo = document.getElementById('localVideo');
      const remoteVideo = document.getElementById('remoteVideo');

      // Handle receiving a call from the server
      connection.on("ReceiveCall", async () => {
          await start();
          await call();
      });

      // Start capturing video
      async function start() {
          localStream = await navigator.mediaDevices.getUserMedia({ video: true, audio: true });
          localVideo.srcObject = localStream;
      }

      // Make a call
      async function call() {
          peerConnection = new RTCPeerConnection();
          localStream.getTracks().forEach(track => peerConnection.addTrack(track, localStream));

          peerConnection.ontrack = (event) => {
              remoteStream = event.streams[0];
              remoteVideo.srcObject = remoteStream;
          };
           const configuration = {};
          const offer = await peerConnection.createOffer(configuration);
          await peerConnection.setLocalDescription(offer);

          // Send offer to server for signaling
          connection.invoke("SendOffer", offer);
      }

      // Answer a call
      async function answer(offer) {
          await start();

          peerConnection = new RTCPeerConnection();
          localStream.getTracks().forEach(track => peerConnection.addTrack(track, localStream));

          peerConnection.ontrack = (event) => {
              remoteStream = event.streams[0];
              remoteVideo.srcObject = remoteStream;
          };

          await peerConnection.setRemoteDescription(new RTCSessionDescription(offer));
          const answer = await peerConnection.createAnswer();
          await peerConnection.setLocalDescription(answer);

          // Send answer to server for signaling
          connection.invoke("SendAnswer", answer);
      }

      // Handle receiving an offer from the server
      connection.on("ReceiveOffer", async (offer) => {
          await answer(offer);
      });

      // Handle receiving an answer from the server
      connection.on("ReceiveAnswer", async (answer) => {
          await peerConnection.setRemoteDescription(new RTCSessionDescription(answer));
      });

      // Hang up the call
      function hangup() {
          peerConnection.close();
          remoteVideo.srcObject = null;
      }

      // Set up event listeners for buttons
      document.getElementById('startButton').addEventListener('click', start);
      document.getElementById('callButton').addEventListener('click', call);
      document.getElementById('hangupButton').addEventListener('click', hangup);

      // Start the SignalR connection
      connection.start().then(() => {
          console.log("SignalR connected.");
      }).catch((err) => {
          console.error(err.toString());
      });
  </script></code>
