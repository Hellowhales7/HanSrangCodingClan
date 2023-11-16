<script lang="ts">
	import { WebRTC } from '$lib/webrtc.js';
	import { onMount } from 'svelte';

	export let data;
	let localVideo: HTMLVideoElement;
	let webRTC: WebRTC;

	onMount(() => {
		const urlParams = new URLSearchParams(window.location.search);
		const roomId = urlParams.get('roomId');

		webRTC = new WebRTC(localVideo, roomId!, 'wordguess');
	});
</script>

<div class="container">
	<div class="videoArea">
		<button
			on:click={() => {
				webRTC.start();
			}}>시작</button
		>
		<video id="localVideo" autoplay muted width="600" height="400" bind:this={localVideo} />
	</div>
	<div class="game">
		{@html data.page}
	</div>
</div>

<style>
	.container {
		display: flex;
		flex-direction: column;
		align-items: center;
		justify-content: center;
		height: 100%;
	}

	.videoArea {
		position: absolute;
		width: 700px;
		height: 100vh;
		border-left: 1px solid white;
		left: 0;
		margin-top: auto;
		margin-bottom: auto;
		z-index: 1000;
	}
</style>
