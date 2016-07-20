using System;
using PW.Mobile.API.DTO;

namespace PW.Mobile.Core.Model
{
	public class Transfer
	{
		public Transfer(TransferVDTO data, bool isInconming)
		{
			Data = data;
			IsIncoming = isInconming;
		}

		public TransferVDTO Data { get; set; }
		public bool IsIncoming { get; set; }
	}
}