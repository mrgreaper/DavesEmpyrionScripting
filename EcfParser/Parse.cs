﻿using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace EcfParser
{

    public static class Parse
    {
        public static EcfFile Deserialize(params string[] lines)
        {
            var result = new EcfFile();
            var i = -1;

            do
            {
                string currentLine = ReadNextLine();

                if (!string.IsNullOrEmpty(currentLine))
                {
                    if (currentLine.StartsWith("{"))
                    {
                        var block = ReadBlock(false, currentLine, ReadNextLine);

                        if (result.Blocks == null) result.Blocks = new List<EcfBlock>();
                        result.Blocks.Add(block);
                    }
                }
            } while (i < lines.Length - 1);

            return result;

            string ReadNextLine()
            {
                var line = lines[++i].Trim();
                var commentPos = line.IndexOf('#');
                line = (commentPos >= 0 ? line.Substring(0, commentPos) : line).Trim();
                commentPos = line.IndexOf("/*");
                if(commentPos >= 0)
                {
                    var commentEnd = line.IndexOf("*/");
                    if (commentEnd >= 0) return 
                            (line.Substring(0, commentPos) + 
                            (line.Length == commentEnd + 2 ? string.Empty : line.Substring(commentEnd + 2))).Trim();

                    while (line.IndexOf("*/") == -1) line = lines[++i].Trim();
                    return string.Empty;
                }
                return line;
            }
        }

        public static IReadOnlyDictionary<string, int> ReadBlockMapping(string filename)
        {
            if (!File.Exists(filename)) return null;

            var result = new ConcurrentDictionary<string, int>();

            var fileContent = File.ReadAllBytes(filename);
            for (var currentOffset = 9; currentOffset < fileContent.Length;)
            {
                var len = fileContent[currentOffset++];
                var name = System.Text.Encoding.ASCII.GetString(fileContent, currentOffset, len);
                currentOffset += len;

                var id = fileContent[currentOffset++] | fileContent[currentOffset++] << 8;

                result.AddOrUpdate(name, id, (s, i) => id);
            }

            return result;
        }

        public static void ReplaceWithMappedIds(EcfFile result, IReadOnlyDictionary<string, int> blockIdMapping)
        {
            result.Blocks.ForEach(B => {
                if (B.Name != "Block" && B.Name != "Item") return;

                var blockIdAttr     = B.Attr?.FirstOrDefault(a => a.Name == "Id");
                var blockId         = blockIdAttr?.Value;
                var blockNameAttr   = B.Attr?.FirstOrDefault(a => a.Name == "Name");
                var blockName       = blockNameAttr?.Value;

                if (string.IsNullOrEmpty(blockName?.ToString()) && blockIdAttr != null) blockIdAttr.AddOns?.TryGetValue("Name", out blockName);

                if (!string.IsNullOrEmpty(blockName?.ToString()) && blockIdMapping.TryGetValue(blockName.ToString(), out var id))
                {
                    if (blockIdAttr != null) blockIdAttr.Value = id;
                    else if (blockName != null && blockId == null)
                    {
                        var idAttr = new EcfAttribute { Name = "Id", Value = id, AddOns = blockNameAttr.AddOns ?? new Dictionary<string, object>() };
                        idAttr.AddOns.Add("Name", blockName.ToString());
                        B.Attr.Remove(blockNameAttr);
                        B.Attr.Insert(0, idAttr);

                        B.EcfValues.Add("Id", idAttr);
                    }

                    if (B.Values.ContainsKey("Id")) B.Values["Id"] = id;
                    else                            B.Values.Add("Id", id);
                }
            });
        }

        private static EcfBlock ReadBlock(bool isChild, string line, Func<string> nextLine)
        {
            var currentLine = line.Substring(1).Trim();
            var nameDelimiterPos = isChild ? currentLine.Length : currentLine.IndexOf(' ');
            var block = new EcfBlock()
            {
                Name = nameDelimiterPos > 0 ? currentLine.Substring(0, nameDelimiterPos).Trim() : null
            };
            if(block.Name != null && block.Name.StartsWith("+")) block.Name = block.Name.Substring(1);

            if (nameDelimiterPos == currentLine.Length) currentLine = string.Empty;
            else if (nameDelimiterPos > 0) currentLine = currentLine.Substring(nameDelimiterPos).Trim();

            int unnamedChild = 0;
            bool firstLine = true;

            do{
                if (currentLine.StartsWith("{"))
                {
                    var childBlock = ReadBlock(true, currentLine, nextLine);
                    if (block.Childs == null) block.Childs = new Dictionary<string, EcfBlock>();
                    block.Childs.Add(childBlock.Name ?? unnamedChild++.ToString(), childBlock);

                    childBlock.EcfValues?.Values
                        .Where(A => A.Name != null && !block.EcfValues.ContainsKey(A.Name))
                        .ToList()
                        .ForEach(A => {
                            block.Values   .Add(A.Name, A.Value);
                            block.EcfValues.Add(A.Name, A); 
                        });
                }
                else
                {
                    var attr = ReadAttribute(currentLine);
                    if(attr != null)
                    {
                        if (block.Attr      == null) block.Attr      = new List<EcfAttribute>();
                        if (block.Values    == null) block.Values    = new Dictionary<string, object>();
                        if (block.EcfValues == null) block.EcfValues = new Dictionary<string, EcfAttribute>();
                        block.Attr.Add(attr);
                        if (attr.Name != null && !block.EcfValues.ContainsKey(attr.Name))
                        {
                            block.Values   .Add(attr.Name, attr.Value);
                            block.EcfValues.Add(attr.Name, attr);
                        }

                        if (firstLine)
                        {
                            attr.AddOns?.ToList().ForEach(A => {
                                block.Values.Add(A.Key, A.Value);
                                block.EcfValues.Add(A.Key, new EcfAttribute() { Name = A.Key, Value = A.Value });
                            });
                        }

                        firstLine = false;
                    }
                }

                currentLine = nextLine().Trim();
            } while (!currentLine.StartsWith("}"));

            return block;
        }

        public static EcfAttribute ReadAttribute(string lineData)
        {
            var line = lineData.Trim();
            var delimiterPos = line.IndexOfAny(new[] { ':', ' ' });
            if (delimiterPos == -1) return null;

            EcfAttribute result = null;

            do{
                delimiterPos = line.IndexOfAny(new[] { ':', ' ' });
                var name = delimiterPos == -1 ? line : line.Substring(0, delimiterPos);
                line = delimiterPos == -1 ? string.Empty : line.Substring(delimiterPos + 1).Trim();

                var nextPayload = line.IndexOfAny(new[] { ',', '"' });

                if (nextPayload == -1)
                {
                    if (result == null) result = new EcfAttribute()
                    {
                        Name = name,
                        Value = ParseValue(line.Trim()),
                    };
                    else
                    {
                        if (result.AddOns == null) result.AddOns = new Dictionary<string, object>();
                        result.AddOns.Add(name, ParseValue(line.Trim()));
                    }
                    line = null;
                }
                else if (line[nextPayload] == '"')
                {
                    var payloadEnd = line.IndexOf('"', nextPayload + 1);
                    if (result == null) result = new EcfAttribute()
                    {
                        Name = name,
                        Value = ParseValue(line.Substring(nextPayload + 1, payloadEnd - nextPayload - 1).Trim()),
                    };
                    else
                    {
                        if (result.AddOns == null) result.AddOns = new Dictionary<string, object>();
                        result.AddOns.Add(name, ParseValue(line.Substring(nextPayload + 1, payloadEnd - nextPayload - 1).Trim()));
                    }

                    if (payloadEnd + 1 >= line.Length) line = null;
                    else
                    {
                        payloadEnd = line.IndexOf(',', payloadEnd + 1);
                        line = line.Substring(payloadEnd + 1).Trim();
                    }
                }
                else if(nextPayload > 0) {
                    if (result == null) result = new EcfAttribute()
                    {
                        Name = name,
                        Value = ParseValue(line.Substring(0, nextPayload).Trim()),
                    };
                    else
                    {
                        if (result.AddOns == null) result.AddOns = new Dictionary<string, object>();
                        result.AddOns.Add(name, ParseValue(line.Substring(0, nextPayload).Trim()));
                    }
                    line = line.Substring(nextPayload + 1).Trim();
                }
                        
            } while (!string.IsNullOrEmpty(line));

            return result;
        }

        private static object ParseValue(string v)
        {
            if (bool  .TryParse(v, out var b)) return b;
            if (int   .TryParse(v, NumberStyles.Integer, null, out var i)) return i;
            if (double.TryParse(v, NumberStyles.Float, CultureInfo.InvariantCulture, out var f)) return f;

            return v;
        }
    }
}
