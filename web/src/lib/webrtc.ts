import { config } from './config';

const peerConnectionConfig = {
	iceServers: [
		{
			urls: ['stun:stun.l.google.com:19302'],
		},
	],
};

const constraints: MediaStreamConstraints = {
	preferCurrentTab: true,
	video: true,
	audio: true,
};

const mediaConstraints = {
	offerToReceiveAudio: true,
	offerToReceiveVideo: true,
};

export class WebRTC {
	serverConnection: WebSocket | null = null;
	localStream: MediaStream | null = null;
	localVideo: HTMLVideoElement;
	peerConnection: RTCPeerConnection | null = null;
	userType: 'tetris' | 'wordguess' | '' = '';
	mediaStream: MediaStream = new MediaStream();

	constructor(localVideo: HTMLVideoElement, session: string, userType: 'tetris' | 'wordguess' | '') {
		this.localVideo = localVideo;
		this.localVideo.srcObject = this.mediaStream;
		this.userType = userType;
		const address = config.apiWsHost + '/api/game/signaling/' + session;
		console.log('tries establish websocket connection on ' + address);
		this.peerConnection = new RTCPeerConnection(peerConnectionConfig);
		this.peerConnection.onicecandidate = (e) => {
			this.gotIceCandidate(e);
		};
		this.peerConnection.ontrack = (e) => {
			this.gotRemoteStream(e);
		};
		this.serverConnection = new WebSocket(address);
		this.serverConnection.onmessage = this.gotMessageFromServer;
		this.serverConnection.onopen = async () => {
			await navigator.mediaDevices
				.getDisplayMedia(constraints)
				.then((stream) => {
					this.localStream = stream;
					this.localStream?.getTracks().forEach((track) => {
						this.peerConnection?.addTrack(track, this.localStream!);
					});
				})
				.catch((e) => {
					this.errorHandler(e);
				});
			this.serverConnection?.send(JSON.stringify({ userType: this.userType }));
		};
	}

	start = async () => {
		console.log('establish connection with peer');
		if (this.userType == 'tetris') {
			this.peerConnection
				?.createOffer(mediaConstraints)
				.then((description) => {
					return this.createdDescription(description);
				})
				.catch((e) => this.errorHandler(e));
		}
	};

	gotIceCandidate = (event: RTCPeerConnectionIceEvent) => {
		console.log('got ice candidate');
		if (event.candidate != null) {
			this.serverConnection?.send(JSON.stringify({ type: 'ice', data: { ice: event.candidate } }));
		}
	};

	gotRemoteStream = async (event: RTCTrackEvent) => {
		console.log('got remote stream');
		this.mediaStream.addTrack(event.track);
		this.localVideo.srcObject = this.mediaStream;
	};

	gotMessageFromServer = async (message: MessageEvent) => {
		const data = JSON.parse(message.data);

		console.log(data);

		if (data.type == 'join') {
			this.start();
		} else if (data.type == 'sdp') {
			await this.peerConnection?.setRemoteDescription(new RTCSessionDescription(data.data.sdp)).catch((e) => {
				this.errorHandler(e);
			});

			if (data.data.sdp.type == 'offer') {
				console.log('offer', this.peerConnection);
				const description = await this.peerConnection?.createAnswer(mediaConstraints).catch((e) => {
					this.errorHandler(e);
				});
				console.log(description);
				this.createdDescription(description!);
			} else if (data.data.sdp.type == 'answer') {
				console.log('answer');
			}
		} else if (data.type == 'ice') {
			this.peerConnection?.addIceCandidate(new RTCIceCandidate(data.data.ice)).catch((e) => {
				this.errorHandler(e);
			});
		}
	};

	createdDescription = (description: RTCSessionDescriptionInit) => {
		this.peerConnection
			?.setLocalDescription(description)
			.then(() => {
				console.log('set local description');
				this.serverConnection?.send(
					JSON.stringify({
						type: 'sdp',
						data: { sdp: this.peerConnection?.localDescription },
					}),
				);
			})
			.catch(this.errorHandler);
	};

	errorHandler = (error: Error) => {
		console.log(error);
	};

	currentState = () => {
		console.log(this.peerConnection?.signalingState, this.peerConnection?.connectionState);
		return this.peerConnection?.signalingState;
	};
}
