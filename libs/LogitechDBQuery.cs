﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data.SQLite;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;

namespace G910_Macro_Viewer.libs
{
    internal class LogitechDBQuery
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public List<MacroInfo> GetMacroInfosFromGHubDatabase()
        {
            Logger.Info("Retrieving Macro Infos");
            List<MacroInfo> macroInfos = new List<MacroInfo>();
            List<MacroInfo> unorderedMacroInfoList = new List<MacroInfo>();

            string settingsDbPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LGHUB", "settings.db");
            Logger.Info($"Check for settings db path {settingsDbPath}");
            if (!System.IO.File.Exists(settingsDbPath))
            {
                Logger.Info($"Cannot find the Logitech GHub Settings DB: {settingsDbPath}");
                return macroInfos;
            }
            Logger.Info($"Open Connection to the DB");
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={settingsDbPath};Version=3;"))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand("SELECT file FROM data", connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Logger.Info($"Start querying the DB");
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
                                            var macroName = (string)card["name"];
                                            Logger.Info($"Start adding macro information: {cardSlot} - {macroName}");
                                            unorderedMacroInfoList.Add(new MacroInfo { Key = cardSlot.Replace(keyboardModel, "").Replace("_", " "), MacroName = macroName });
                                        }
                                    }
                                }
                            }
#pragma warning restore CS8604 ,CS8602, CS8600 // Possible null reference argument.
                        }
                    }
                }
            }
            Logger.Info("Aggregate macro infos");
            var m1MacroInfos = unorderedMacroInfoList.Where(macro => macro.Key.Contains("m1")).ToList();
            var m2MacroInfos = unorderedMacroInfoList.Where(macro => macro.Key.Contains("m2")).ToList();
            var m3MacroInfos = unorderedMacroInfoList.Where(macro => macro.Key.Contains("m3")).ToList();
            macroInfos.AddRange(m1MacroInfos);
            macroInfos.AddRange(m2MacroInfos);
            macroInfos.AddRange(m3MacroInfos);
            Logger.Info("Return macro infos");
            return macroInfos;
        }

    }
}
