<script lang="ts">
	import Textfield from '@smui/textfield';
	import HelperText from '@smui/textfield/helper-text';
	import Button, { Label } from '@smui/button';
	import { goto } from '$app/navigation';
	import { config } from '$lib/config';

	let userId = '';
	let userName = '';
	let password = '';
	let passwordConfirm = '';

	const Register = async () => {
		if (userId === '') {
			alert('아이디를 입력해주세요.');
			return;
		} else if (userName === '') {
			alert('닉네임을 입력해주세요.');
			return;
		} else if (password === '') {
			alert('비밀번호를 입력해주세요.');
			return;
		} else if (password !== passwordConfirm) {
			alert('비밀번호가 일치하지 않습니다.');
			return;
		}

		const res = await fetch(config.apiHost + '/api/auth/register', {
			method: 'POST',
			headers: {
				'content-type': 'application/json',
			},
			body: JSON.stringify({
				userId,
				userName,
				password,
			}),
		})
			.then((res) => res.json())
			.catch((err) => alert('회원가입실패 ㅠㅠ'));

		if (res.ok === false) {
			alert('회원가입실패 ㅠㅠ');
		} else {
			goto('/login');
		}
	};
</script>

<div class="container">
	<Textfield variant="outlined" bind:value={userId} label="아이디">
		<HelperText slot="helper">아이디</HelperText>
	</Textfield>

	<Textfield variant="outlined" bind:value={userName} label="닉네임">
		<HelperText slot="helper">닉네임</HelperText>
	</Textfield>
	<Textfield type="password" variant="outlined" bind:value={password} label="비밀번호">
		<HelperText slot="helper">비밀번호</HelperText>
	</Textfield>
	<Textfield type="password" variant="outlined" bind:value={passwordConfirm} label="비밀번호 확인">
		<HelperText slot="helper">비밀번호 확인</HelperText>
	</Textfield>
	<div>
		<Button
			on:click={() => {
				Register();
			}}
			variant="raised"
		>
			<Label>회원가입</Label>
		</Button>
		<Button
			on:click={() => {
				goto('/login');
			}}
			color="secondary"
			variant="raised"
		>
			<Label>로그인</Label>
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
