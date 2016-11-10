﻿using System;
using MySql.Data.MySqlClient;
using AppKit;

namespace FacultySchedules
{
	public class GiveData
	{
		string connectionParam = Globals.connectionParam;

		public void DBGather(string[,] weeksWorth, string name)
		{
			int inputDay;
			string inputHour, inputEvent;
			int inputRowSpan;
			createTable(name);

			for (int h = 0; h < 32; h++)
			{
				for (int d = 0; d < 5; d++)
				{
					if ((weeksWorth[h, d] != null) && (weeksWorth[h, d].Contains("nothing") == false))
					{
						string[] subStrings = weeksWorth[h, d].Split(new string[] { "$" }, StringSplitOptions.None); //Seperates the string of data into pieces
						inputDay = int.Parse(subStrings[0]);
						inputHour = subStrings[1];
						inputEvent = subStrings[2];
						inputRowSpan = Convert.ToInt16(subStrings[3]);
						insertIntoTable(Globals.dayList[inputDay], inputHour, inputEvent, inputRowSpan, name);
						Globals.uniqueClassInput.Add(inputEvent);
					}
					else continue;
				}
			}
		}

		public void createTable(string name)
		{
			MySqlConnection connectionCreate = null;
			MySqlDataReader dataReaderCreate = null;
			try
			{
				connectionCreate = new MySqlConnection(connectionParam);
				connectionCreate.Open();
				string stm = "CREATE TABLE `" + name + "` (id int(50) NOT NULL AUTO_INCREMENT, day varchar(50), hour varchar(50), event varchar(50), rowspan int (10), PRIMARY KEY (id))";
				MySqlCommand createCmd = new MySqlCommand(stm, connectionCreate);
				dataReaderCreate = createCmd.ExecuteReader();
			}

			catch (MySqlException error)
			{
				errorHandle(error);
			}

			finally //We need to close all of our connections once everything is retrieved
			{
				if (dataReaderCreate != null)
				{
					dataReaderCreate.Close();
				}

				if (connectionCreate != null)
				{
					connectionCreate.Close();
				}
			}
		}

		public void insertMissingIntoTable(string inputDay, string inputHour, string inputEvent, int inputRowSpan, string name)
		{
			createTable(name);
			MySqlConnection addConnection = null;
			MySqlDataReader addDataReader = null;
			try
			{
				addConnection = new MySqlConnection(connectionParam);
				addConnection.Open();
				string stm = "INSERT INTO `" + name + "` (day, hour, event, rowspan) VALUES(@day, @hour, @event, @rowspan)";
				MySqlCommand cmd = new MySqlCommand(stm, addConnection);
				cmd.Parameters.AddWithValue("@day", inputDay);
				cmd.Parameters.AddWithValue("@hour", inputHour);
				cmd.Parameters.AddWithValue("@event", inputEvent);
				cmd.Parameters.AddWithValue("@rowspan", inputRowSpan);
				addDataReader = cmd.ExecuteReader();
			}

			catch (MySqlException error)
			{
				errorHandle(error);
			}

			finally
			{
				if (addDataReader != null)
				{
					addDataReader.Close();
				}

				if (addConnection != null)
				{
					addConnection.Close();
				}
			}
		}

		public void insertIntoTable(string inputDay, string inputHour, string inputEvent, int inputRowSpan, string name)
		{
			MySqlConnection addConnection = null;
			MySqlDataReader addDataReader = null;
			try
			{
				addConnection = new MySqlConnection(connectionParam);
				addConnection.Open();
				string stm = "INSERT INTO `" + name + "` (day, hour, event, rowspan) VALUES(@day, @hour, @event, @rowspan)";
				MySqlCommand cmd = new MySqlCommand(stm, addConnection);
				cmd.Parameters.AddWithValue("@day", inputDay);
				cmd.Parameters.AddWithValue("@hour", inputHour);
				cmd.Parameters.AddWithValue("@event", inputEvent);
				cmd.Parameters.AddWithValue("@rowspan", inputRowSpan);
				addDataReader = cmd.ExecuteReader();
			}

			catch (MySqlException error)
			{
				errorHandle(error);
			}

			finally
			{
				if (addDataReader != null)
				{
					addDataReader.Close();
				}

				if (addConnection != null)
				{
					addConnection.Close();
				}
			}
		}

		public void dropTable(string name)
		{
			MySqlConnection connection = null;
			MySqlDataReader dataReader = null;
			try
			{
				connection = new MySqlConnection(connectionParam);
				connection.Open();
				string replaceStm = "DROP TABLE IF EXISTS `" + name + "`";
				MySqlCommand replaceCmd = new MySqlCommand(replaceStm, connection);
				dataReader = replaceCmd.ExecuteReader();
			}

			catch (MySqlException error)
			{
				errorHandle(error);
			}

			finally
			{
				if (dataReader != null)
				{
					dataReader.Close();
				}

				if (connection != null)
				{
					connection.Close();
				}
			}
		}

		void errorHandle(MySqlException error)
		{
			NSAlert oAlert = new NSAlert();
			// Set the buttons
			oAlert.InvokeOnMainThread(delegate
			{
				oAlert.AddButton("Ok");
			});
			// Show the message box and capture
			oAlert.MessageText = "There's a problem with the query!";
			oAlert.InformativeText = error.ToString();
			oAlert.AlertStyle = NSAlertStyle.Informational;
		}
	}
}