import page from './index.html?raw';
import type { PageLoad } from './$types';

export const load: PageLoad = async () => {
	return { page };
};
