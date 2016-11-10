﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using HtmlAgilityPack;

namespace FacultySchedules
{
	public class Scrape
	{
		public List<HtmlNode> BeginScrape(string name)
		{
			SpecialNameFormatting specialFormatInit = new SpecialNameFormatting();
			WebClient webClient = new WebClient();

			string page = webClient.DownloadString(specialFormatInit.splitNameGetURL(name));

			HtmlDocument doc = new HtmlDocument();
			doc.LoadHtml(page);

			try
			{
				List<HtmlNode> x = doc.GetElementbyId("ctl0_Main_tblSchedule").Elements("tr").ToList();
				Globals.isMissing = false;
				return x;
			}

			catch (NullReferenceException error)
			{
				Globals.isMissing = true;
				return null;
			}
		}

		public List<HtmlNode> ScrapeFaculty()
		{
			WebClient webClient = new WebClient();

			string page = webClient.DownloadString("http://www.cis.gvsu.edu/public/staffListing/");

			HtmlDocument doc = new HtmlDocument();
			doc.LoadHtml(page);

			List<HtmlNode> x = doc.GetElementbyId("ctl0_Main_grdListing").Elements("tbody").ToList();
			return x;
		}

		public List<HtmlNode> ScrapeFacultyEven()
		{
			WebClient webClient = new WebClient();

			string page = webClient.DownloadString("http://www.cis.gvsu.edu/public/staffListing/");

			HtmlDocument doc = new HtmlDocument();
			doc.LoadHtml(page);

			List<HtmlNode> x = doc.GetElementbyId("ctl0_Main_grdListingEven").Elements("tbody").ToList();
			return x;
		}
	}
}