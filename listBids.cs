using System;

namespace WorkEX
{
	public class ListBids
	{
		public ListBids ()
		{
		}
		public int ID { get; set; }
		public int CateID { get; set; }
		public string Name { get; set; }
		public string Date { get; set; }
		public string DateUpd { get; set; }
		public string Title { get; set; }
		public string Text { get; set; }
		public string Type { get; set; }
		public string Adress { get; set; }
		public string Telefone { get; set; }
		public string Status { get; set; }
		public string TimeFinish { get; set; }
		public string Rating { get; set; }
		public bool Done { get; set; }	// TODO: add this field to the user-interface

	}
}