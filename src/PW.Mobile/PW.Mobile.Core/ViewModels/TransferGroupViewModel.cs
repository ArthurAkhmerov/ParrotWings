using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PW.Mobile.Core.Model;

namespace PW.Mobile.Core.ViewModels
{
	public class TransferGroupViewModel: ObservableCollection<Transfer>
	{
		public DateTime Date { get; set; }

		public TransferGroupViewModel(DateTime date, IEnumerable<Transfer> transfers): base(transfers)
		{
			Date = date;
		}
	}
}