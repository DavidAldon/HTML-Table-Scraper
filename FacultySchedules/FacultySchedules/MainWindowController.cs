﻿using System;
using Foundation;
using AppKit;
using System.Linq;
using System.Collections.Generic;

namespace FacultySchedules
{
	public partial class MainWindowController : NSWindowController
	{
		string facName, firstName, lastName;
		Run runInit = new Run();
		allFaculty allFacultyInit = new allFaculty();
		List<string> names = new List<string>();

		public void ViewDidLoad()
		{
			base.AwakeFromNib();
			//Set initial values here
		}

		partial void clickedAddFacultyButton(NSObject sender)
		{
			runInit.giveDB.createTable(firstNameInput.StringValue + lastNameInput.StringValue);

		}

		partial void clickedRemoveFacultyButton(NSObject sender)
		{
			runInit.giveDB.dropTable(firstNameInput.StringValue + lastNameInput.StringValue);
		}

		partial void clickedScrapeButton(Foundation.NSObject sender)
		{
			foreach (string eachName in allFacultyInit.getEveryonesName())
			{
				facultyListCombo.AddItem(eachName);
				names.Add(eachName);
			}
			if (allFacultyCheckBox.State == NSCellStateValue.On)
			{
				foreach (string name in names)
				{
					facName = name;
					grabSplitAndRun();
				}
			}
			else
			{
				facName = facultyListCombo.TitleOfSelectedItem;
				grabSplitAndRun();
			}
		}

		partial void clickedScheduleButton(Foundation.NSObject sender)
		{
			facName = facultyListCombo.TitleOfSelectedItem;
			firstName = splitNameGetFirst(facName);
			lastName = splitNameGetLast(facName);
			webViewSchedule.MainFrameUrl = "http://www.cis.gvsu.edu/public/staffListing/index.php?page=staff&fname=" + firstName + "&lname=" + lastName;
		}

		public MainWindowController(IntPtr handle) : base(handle) { }

		[Export("initWithCoder:")]
		public MainWindowController(NSCoder coder) : base(coder) { }

		public MainWindowController() : base("MainWindow") { }

		public override void AwakeFromNib()
		{
			base.AwakeFromNib();
		}

		public new MainWindow Window
		{
			get { return (MainWindow)base.Window; }
		}

		public void grabSplitAndRun()
		{
			firstName = splitNameGetFirst(facName);
			lastName = splitNameGetLast(facName);
			facName = RemoveWhitespace(facName);
			runInit.start(facName, firstName, lastName);
		}

		public static string splitNameGetFirst(string input)
		{
			string[] nameToSplit = input.Split();
			string first = nameToSplit[0];
			return first;
		}

		public string splitNameGetLast(string input)
		{
			string[] nameToSplit = input.Split();
			string last = nameToSplit[1];
			return last;
		}

		public static string RemoveWhitespace(string input)
		{
			return new string(input.ToCharArray()
				.Where(c => !Char.IsWhiteSpace(c))
				.ToArray());
		}
	}
}
