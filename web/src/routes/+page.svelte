<script>
	import { goto } from '$app/navigation';
	import { checkLogin } from '$lib/checkLogin';
	import Button, { Label } from '@smui/button';
	import { onMount } from 'svelte';

	onMount(() => {
		checkLogin().then((res) => {
			if (!res) {
				goto('/login');
			}
		});
	});
</script>

<div class="container">
	<img src="/logo.jpeg" alt="" width="600" height="600" />
	<div>
		<Button
			on:click={() => {
				goto('/rank');
			}}
			variant="raised"
			style="width: 200px; height: 100px; font-size: 25px; margin-right: 2px;"
		>
			<Label>순위</Label>
		</Button>
		<Button
			on:click={() => {
				goto('/rooms');
			}}
			variant="raised"
			style="width: 200px; height: 100px; font-size: 25px; margin-left: 2px;"
		>
			<Label>대기실</Label>
		</Button>
	</div>
	Sejong's Cipher by 한사랑코딩회
</div>

<style>
	.container {
		display: flex;
		flex-direction: column;
		align-items: center;
		justify-content: center;
		height: 100%;
		gap: 10px;
	}
</style>
