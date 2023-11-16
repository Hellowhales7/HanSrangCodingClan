<script lang="ts">
	import { checkLogin } from '$lib/checkLogin';
	import { config } from '$lib/config';
	import type { Score } from '$lib/types';
	import Paper, { Title, Content } from '@smui/paper';
	import { onMount } from 'svelte';

	let scores: Score[] = [];
	const getRanks = async () => {
		const res = await fetch(config.apiHost + '/api/game/ranks', {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json',
				Authorization: 'Bearer ' + localStorage.getItem('token') ?? '',
			},
			body: JSON.stringify({
				page: 1,
			}),
		})
			.then((res) => res.json())
			.catch((err) => alert('랭킹불러오기실패 ㅠㅠ'));

		if (res.ok === false) {
			alert('랭킹불러오기실패 ㅠㅠ');
		} else {
			scores = res.data.scores;
		}
	};

	onMount(() => {
		checkLogin().then((logged_in) => {
			if (logged_in === false) {
				alert('로그인이 필요합니다.');
				location.href = '/login';
			}
			getRanks();
		});
	});
</script>

<div class="container">
	{#each scores as score, i}
		<div class="paper-container">
			<Paper color="primary" variant="outlined" style="width: 350px;">
				<Title>{i + 1}위 : {score.Score}점</Title>
				<Content>
					{score.User1.Username}<br />
					{score.User2.Username}
				</Content>
			</Paper>
		</div>
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
