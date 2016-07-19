module PW {
	declare var md5: any;

	export interface ISecurityProvider {
		calculateMD5(...content: any[]): string;
	}

	export class SecurityProvider implements ISecurityProvider {

		calculateMD5(...content: any[]): string {
			var dataAsString: string = JSON.stringify(content);
			var hash = md5(dataAsString);

			return hash;
		}
	}

	angular.module("PW")
		.factory("PW.SecurityProvider", [() => new SecurityProvider()]);
}