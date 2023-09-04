﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data.SQLite;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using log4net.Repository.Hierarchy;
using log4net;
using log4net.Core;
using System.Reflection;

namespace G910_Logitech_Utilities.libs
{
    internal class LogitechDBQuery
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(type: MethodBase.GetCurrentMethod().DeclaringType);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        public List<KeyBindingsInfo> GetKeyBindingsInfosFromGHubDatabase()
        {
            Logger.Info("Retrieving KeyBindings Infos", null);
            List<KeyBindingsInfo> KeyBindingsInfos = new List<KeyBindingsInfo>();
            List<KeyBindingsInfo> unorderedKeyBindingsInfoList = new List<KeyBindingsInfo>();

            string settingsDbPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LGHUB", "settings.db");
            Logger.Info($"Check for settings db path {settingsDbPath}", null);
            if (!System.IO.File.Exists(settingsDbPath))
            {
                Logger.Info($"Cannot find the Logitech GHub Settings DB: {settingsDbPath}", null);
                return KeyBindingsInfos;
            }
            Logger.Info($"Open Connection to the DB", null);
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={settingsDbPath};Version=3;"))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand("SELECT file FROM data", connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Logger.Info($"Start querying the DB", null);
#pragma warning disable CS8604, CS8602, CS8600 // Possible null reference argument.
                            string jsonData = reader.GetString(0);

                            JObject rootObject = JObject.Parse(jsonData);
                            JObject cardsObject = (JObject)rootObject["cards"];
                            JArray cardsArray = (JArray)cardsObject["cards"];
                            var profileId = cardsArray.FirstOrDefault(card => (string)card["name"] == "DEFAULT_CARD_NAME_FIRMWARE_LIGHTING_SETTINGS" && card["profileId"] != null)["profileId"].ToString();

                            JObject profilesObject = (JObject)rootObject["profiles"];
                            JArray profilesArray = (JArray)profilesObject["profiles"];
                            JObject? currentProfile = profilesArray.FirstOrDefault(profile => (string)profile["id"] == profileId) as JObject;
                            if (currentProfile != null)
                            {
                                JArray assignmentsArray = (JArray)currentProfile["assignments"];

                                foreach (var cardId in assignmentsArray)
                                {
                                    JObject card = (JObject)cardsArray.FirstOrDefault(c => (string)c["id"] == (string)cardId["cardId"]);
                                    if (card != null)
                                    {
                                        var cardSlot = cardId["slotId"].ToString();
                                        var keyboardModel = "g910";
                                        if (cardSlot.Contains(keyboardModel))
                                        {
                                            var KeyBindingsName = (string)card["name"];
                                            Logger.Info($"Start adding KeyBindings information: {cardSlot} - {KeyBindingsName}", null);
#pragma warning disable CS8601 // Possible null reference assignment.
                                            var keyValueWithMacroNumber = cardSlot.Replace(keyboardModel, "").Replace("_", " ");
                                            var keyValue = keyValueWithMacroNumber.Split(" ")[1];
                                            if(keyValue.TrimStart().StartsWith("g"))
                                            {
                                                var macroName = keyValueWithMacroNumber.Split(" ")[2].ToUpper();
                                                Enum.TryParse(macroName, out KeyBindingsInfo.MacroName keyMacroName);
                                                unorderedKeyBindingsInfoList.Add(new KeyBindingsInfo { Key = keyValue, KeyMacroName = keyMacroName, KeyBindingsName = KeyBindingsName });
                                            }
#pragma warning restore CS8601 // Possible null reference assignment.
                                        }
                                    }
                                }
                            }
#pragma warning restore CS8604 ,CS8602, CS8600 // Possible null reference argument.
                        }
                    }
                }
            }
            Logger.Info("Aggregate KeyBindings infos", null);
            KeyBindingsInfos.AddRange(unorderedKeyBindingsInfoList);
            Logger.Info("Return KeyBindings infos", null);
            return KeyBindingsInfos;
        }

    }
}
