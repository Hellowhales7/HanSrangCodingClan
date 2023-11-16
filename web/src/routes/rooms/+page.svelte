<script lang="ts">
	import Card, { Content, Actions } from '@smui/card';
	import Button, { Label } from '@smui/button';
	import type { Room } from '$lib/types';
	import { config } from '$lib/config';
	import { checkLogin } from '$lib/checkLogin';
	import { onMount } from 'svelte';

	let rooms: Room[] = [];
	const getRooms = async () => {
		const res = await fetch(config.apiHost + '/api/room', {
			method: 'GET',
			headers: {
				'Content-Type': 'application/json',
				Authorization: 'Bearer ' + localStorage.getItem('token') ?? '',
			},
		})
			.then((res) => res.json())
			.catch((err) => alert('방목록불러오기실패 ㅠㅠ'));

		if (res.ok === false) {
			alert('방목록불러오기실패 ㅠㅠ');
		} else {
			rooms = res.data.rooms;
		}
	};

	onMount(() => {
		checkLogin().then((logged_in) => {
			if (logged_in === false) {
				alert('로그인이 필요합니다.');
				location.href = '/login';
			}
			// getRooms();
		});

		rooms = [
			{
				Id: '1',
				RoomName: '멋쟁이클럽',
				User1: {
					Id: '1',
					Username: '멋쟁이',
				},
				User2: {
					Id: '2',
					Username: '한글대마왕',
				},
			},
			{
				Id: '2',
				RoomName: '슈퍼겁쟁이클럽',
				User1: {
					Id: '3',
					Username: '게살버거레시피훔치다걸린사람',
				},
			},
		];
	});
</script>

<div class="container">
	{#each rooms as room, i}
		{@const numUsers = room.User2 ? 2 : 1}
		<Card style="width: 300px;">
			<Content>
				{room.RoomName} ({numUsers}/2)<br />
			</Content>
			<Actions style="display: flex; flex-direction: row-reverse">
				<Button
					on:click={() => {
						if (i === 0) location.href = '/tetrisgame/?roomId=asd';
						else location.href = '/wordguessgame/?roomId=asd';
					}}
				>
					<Label>참가</Label>
				</Button>
			</Actions>
		</Card>
	{/each}
</div>

<style>
	.container {
		display: flex;
		flex-direction: column;
		align-items: center;
		justify-content: center;
		gap: 5px;
		height: 100%;
	}
</style>
