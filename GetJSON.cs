//
//  GetJSON.cs
//
//  Author:
//       Krimes <neolus2@ya.ru>
//
//  Copyright (c) 2015 Krimes
//
//  This program is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; either version 2 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

using Org.Json;


namespace WorkEX
{
	public static class GetJSON
	{
		const int ActionGetUserUid = 0;
		const int ActionGetCatalog = 1;
		const int ActionAddBid = 2;
		const int ActionListBid = 3;
		const int ActionDeleteBid = 4;

		public static string[] ResultFinal;

		class GetProp
		{
			public string Prop1;
			public string Prop2;
			public string Prop3;
			public string Prop4;
			public string Prop5;
			public string Prop6;
		}

		static GetProp prop1;

		public static JSONTokener Get (int i)
		{

			switch (i) {
			case ActionGetUserUid: //Get User_Uid
				{
					return GetRequest (String.Format ("http://funtea.ru/api.php"));
				}
			case ActionGetCatalog: //Get Catalog
				{
					return GetRequest (String.Format ("http://funtea.ru/api.php?uid={0}&action=catalog", WorkEX.MainActivity.UserId));
				}
			case ActionAddBid: //Add Bid
				{
					return GetRequest (String.Format ("http://funtea.ru/api.php?uid={0}&action=add-bids&title={1}&text={2}&cate_id={3}&adress={4}&telephone={5}&name={6}", WorkEX.MainActivity.UserId, prop1.Prop1, prop1.Prop2, prop1.Prop3, prop1.Prop4,prop1.Prop5, prop1.Prop6));
				}
			case ActionListBid: //List Bid
				{
					return GetRequest (String.Format ("http://funtea.ru/api.php?uid={0}&action=all-my-bids", WorkEX.MainActivity.UserId));
				}
			case ActionDeleteBid: //Delete Bid
				{
					return GetRequest (String.Format ("http://funtea.ru/api.php?uid={0}&action=delete-bids&bids_id={1}", WorkEX.MainActivity.UserId, prop1.Prop1));
				}
			default:
				{
					return null;
				}
			}

		}

		static JSONTokener GetRequest (string searchUrl)
		{
			//New Method
			var httpReq = (HttpWebRequest)WebRequest.Create (new Uri (searchUrl));
			Stream objStream;
			objStream = httpReq.GetResponse ().GetResponseStream ();
			JSONTokener j;
			var objReader = new StreamReader (objStream);
			var s1 = objReader.ReadToEnd ();
			j = new JSONTokener (s1);
			return j;
		}

		public static string GetUserId ()
		{
			return new JSONObject (Get (ActionGetUserUid)).GetString ("user_uid");
		}

		public static List<string> GetCatalog ()
		{
			var JSON = new JSONArray (Get (ActionGetCatalog));
			var l_name_try = new List<string> ();
			try {
				for (int i = 0; i < JSON.Length (); i++) {
					JSONObject Cate = JSON.GetJSONObject (i);
					String name = Cate.GetString ("cate_name");
					var parent_id = Cate.GetInt ("parent_cate");
					if (parent_id == 0) {
						l_name_try.Add (name);
					}
				}
			} catch {
				return l_name_try;
			}
			return l_name_try;
		}

		public static int GetCatalogId (string catname)
		{
			var JSON = new JSONArray (Get (ActionGetCatalog));
			try {
				for (int i = 0; i < JSON.Length (); i++) {
					JSONObject Cate = JSON.GetJSONObject (i);
					String name = Cate.GetString ("cate_name");
					if (name == catname) {
						return Cate.GetInt ("cate_id");
					}
				}
			} finally {

			}
			return 0;
		}

		public static bool AddBidsByUserId (int idcat, string title, string text, string adress, string tel, string name_user)
		{
			prop1 = new GetProp ();
			prop1.Prop1 = title;
			prop1.Prop2 = text;
			prop1.Prop3 = idcat.ToString ();
			prop1.Prop4 = adress;
			prop1.Prop5 = tel;
			prop1.Prop6 = name_user;
			var JSON = Get (ActionAddBid);
			return Equals(JSON,"done");
		}

		public static bool DeleteBidsByUserId (int id)
		{
			prop1 = new GetProp ();
			prop1.Prop1 = id.ToString();
			var JSON = Get (ActionDeleteBid);
			return Equals(JSON,"done");
		}

		public static IList<ListBids> ShowBidsByUserId ()
		{
			IList<ListBids> listBids1 = new List<ListBids> ();
			try 
			{
			var JSON = new JSONArray (Get (ActionListBid));
			try {
					for (int i = JSON.Length ()-1; i >= 0; i--) {
						JSONObject Cate = JSON.GetJSONObject (i);
						String date = Cate.GetString ("date_on");
						var tempList = new ListBids ();
						tempList.ID = Cate.GetInt ("bids_id");
						tempList.CateID = Cate.GetInt ("cate_id");
						tempList.Date = Cate.GetString ("date_on");
						tempList.Title = Cate.GetString ("title");
						tempList.Text = Cate.GetString ("text");
						tempList.DateUpd = Cate.GetString ("date_upd");
						tempList.Type = Cate.GetString ("type");
						tempList.Status = Cate.GetString ("status");
						tempList.TimeFinish = Cate.GetString ("time_finish");
						tempList.Rating = Cate.GetString ("rating");
						tempList.Telefone = Cate.GetString ("telephone");
						tempList.Adress = Cate.GetString("adress");
						tempList.Name = Cate.GetString("name");
						listBids1.Add (tempList);
					}
				} catch {
					var temp1 = new ListBids ();
					temp1.Date = DateTime.Now.ToLongDateString();
					temp1.Title = "Ошибка параметров";
					listBids1.Add(temp1);
				}
			} catch {
				var temp1 = new ListBids ();
				temp1.Date = DateTime.Now.ToLongDateString();
				temp1.Title = "Список пуст";
				listBids1.Add(temp1);
			}

			return listBids1;
		}
	}
}