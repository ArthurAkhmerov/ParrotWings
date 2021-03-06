//     This code was generated by a Reinforced.Typings tool. 
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.


module PW {
	export interface IUserVDTO
	{
		id: string;
		username: string;
		email: string;
		createdAt: string;
		state: string;
		role: string;
		balance: number;
	}
	export interface IPwClient
	{
		clientId: string;
		userId: string;
	}
	export interface ITransferVDTO
	{
		id: string;
		userFromId: string;
		userFromName: string;
		userToId: string;
		userToName: string;
		amount: number;
		createdAt: string;
	}
	export interface ITransferRequestVDTO
	{
		userFromId: string;
		recipientsIds: string[];
		amount: number;
	}
	export interface ISendTransferResultVDTO
	{
		success: boolean;
		message: string;
	}
	export interface IAuthRequestVDTO
	{
		email: string;
		password: string;
	}
	export interface ISignupRequestVDTO
	{
		name: string;
		email: string;
		password: string;
	}
	export interface IAuthData
	{
		sessionId: string;
		userId: string;
	}
	export interface IAuthResultVDTO
	{
		success: boolean;
		data: PW.IAuthData;
		message: string;
	}
}
