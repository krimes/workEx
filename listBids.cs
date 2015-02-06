using System;

namespace WorkEX
{
	public class ListBids
	{
		public ListBids ()
		{
		}
		public int ID { get; set; }
		public string Date { get; set; }
		public string Text { get; set; }
		public bool Done { get; set; }	// TODO: add this field to the user-interface

	}
}