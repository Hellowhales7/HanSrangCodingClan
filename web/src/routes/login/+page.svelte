<script lang="ts">
	import Textfield from '@smui/textfield';
	import HelperText from '@smui/textfield/helper-text';
	import Button, { Label } from '@smui/button';
	import { goto } from '$app/navigation';
	import { config } from '$lib/config';

	let userId = '';
	let password = '';

	const Login = async () => {
		const res = await fetch(config.apiHost + '/api/auth/login', {
			method: 'POST',
			headers: {
				'content-type': 'application/json',
			},
			body: JSON.stringify({
				userId,
				password,
			}),
		})
			.then((res) => res.json())
			.catch((err) => alert('로그인실패 ㅠㅠ'));

		if (res.ok === false) {
			alert('로그인실패 ㅠㅠ');
		} else {
			alert('로그인성공!');
			localStorage.setItem('token', res.data.token);
			goto('/');
		}
	};
</script>

<div class="container">
	<Textfield variant="outlined" bind:value={userId} label="아이디">
		<HelperText slot="helper">아이디</HelperText>
	</Textfield>
	<Textfield type="password" variant="outlined" bind:value={password} label="비밀번호">
		<HelperText slot="helper">비밀번호</HelperText>
	</Textfield>
	<div>
		<Button
			on:click={() => {
				Login();
			}}
			variant="raised"
		>
			<Label>로그인</Label>
		</Button>
		<Button
			on:click={() => {
				goto('/register');
			}}
			color="secondary"
			variant="raised"
		>
			<Label>회원가입</Label>
		</Button>
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
</style>
