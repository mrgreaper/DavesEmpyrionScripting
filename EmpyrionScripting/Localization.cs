﻿using EmpyrionNetAPIDefinitions;
using EmpyrionScripting.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EmpyrionScripting
{
    public class Localization
    {
        public static Action<string, LogLevel> Log { get; set; } = (s, l) => Console.WriteLine(s);

        public Dictionary<string, List<string>> LocalisationData { get; }
        public Localization(string contentPath, string activeScenario)
        {
            var scenarioPath = string.IsNullOrEmpty(activeScenario) ? null : Path.Combine(contentPath, "Scenarios", activeScenario);

            LocalisationData = ReadLocalisation(contentPath).ToDictionary(item => item.Key, item => RemoveFormats(item.Value));

            if (!string.IsNullOrEmpty(scenarioPath))
            {
                ReadLocalisation(scenarioPath).ForEach(item =>
                {
                    if (LocalisationData.ContainsKey(item.Key)) LocalisationData[item.Key] = RemoveFormats(item.Value);
                    else                                        LocalisationData.Add(item.Key, RemoveFormats(item.Value));
                });
            }
        }

        private List<string> RemoveFormats(List<string> values)
            => values.Select(v => RemoveFormats(v)).ToList();

        public static string RemoveFormats(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;

            var startPos = value.IndexOf('[');
            while(startPos >= 0)
            {
                var endPos = value.IndexOf(']', startPos);
                if (endPos >= 0) value = value.Substring(0, startPos) + value.Substring(endPos + 1);
                else             startPos++;

                if (startPos >= value.Length) return value;

                startPos = value.IndexOf('[', startPos);
            }
            return value;
        }

        private Dictionary<string, List<string>> ReadLocalisation(string contentPath)
            => ReadLocalisationFile(contentPath)
                .Where(L => !string.IsNullOrEmpty(L) && char.IsLetter(L[0]))
                .Select(L =>
                {
                    var line = L.Split(',');
                    return new { ID = line.First(), Names = line.Skip(1) };
                })
                .SafeToDictionary(L => L.ID, L => L.Names.ToList(), StringComparer.CurrentCultureIgnoreCase);

        private static string[] ReadLocalisationFile(string contentPath)
        {
            var filename = Path.Combine(contentPath, @"Extras\Localization.csv");
            Log($"LocalisationData from '{filename}'", LogLevel.Message);
            return File.Exists(filename) ? File.ReadAllLines(filename) : new string[] { };
        }

        public string GetName(string name, string language)
        {
            if (string.IsNullOrEmpty(name)) return string.Empty;
            if (!LocalisationData.TryGetValue(name, out List<string> i18nData)) return RemoveFormats(name);

            var languagePos = LocalisationData["KEY"].IndexOf(language);
            return languagePos == -1 || languagePos >= i18nData.Count
                ? i18nData.Count >= 1
                    ? i18nData[0] // Fallback Engisch
                    : RemoveFormats(name)
                : string.IsNullOrEmpty(i18nData[languagePos]) ? i18nData[0] : i18nData[languagePos];
        }
    }
}