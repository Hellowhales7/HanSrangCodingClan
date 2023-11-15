import { config } from './config';

export const checkLogin = async () => {
	const loggedIn = await fetch(config.apiHost + '/api/auth/check', {
		method: 'GET',
		headers: {
			'Content-Type': 'application/json',
			Authorization: 'Bearer ' + localStorage.getItem('token') ?? '',
		},
	})
		.then((response) => {
			if (response.status === 200) {
				return true;
			} else {
				return false;
			}
		})
		.catch((error) => {
			console.log(error);
			return false;
		});

	return loggedIn;
};
