﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CreatureScriptsParser.Packets;

namespace CreatureScriptsParser
{
    public static class Utils
    {
        public static string GetValueWithoutComma(this float value)
        {
            return value.ToString().Replace(",", ".");
        }

        public static string ConverNameToCoreFormat(string name)
        {
            return name.Replace("\'", "").Replace(" ", "").Replace("(", "").Replace(")", "").Replace("-", "");
        }

        public static uint GetCreatureEntryUsingGuid(List<object> packets, string guid)
        {
            var updaeObjectPacket = packets.FirstOrDefault(x => ((Packet)x).type == Packet.PacketTypes.SMSG_UPDATE_OBJECT && ((Packet)x).guid == guid && ((UpdateObjectPacket)x).updateType == UpdateObjectPacket.UpdateType.CreateObject);
            if (updaeObjectPacket != null)
            {
                return ((UpdateObjectPacket)packets.FirstOrDefault(x => ((Packet)x).type == Packet.PacketTypes.SMSG_UPDATE_OBJECT && ((Packet)x).guid == guid && ((UpdateObjectPacket)x).updateType == UpdateObjectPacket.UpdateType.CreateObject)).creatureEntry;
            }
            else
                return 0;
        }

        public static string GetCreatureNameFromDb(uint entry)
        {
            string sqlQuery = "SELECT `Name1` FROM `creature_template_wdb` WHERE `entry` = " + entry;

            if (Sql.WorldDatabaseSelectQuery(sqlQuery).Tables["table"].Rows.Count != 0)
            {
                return Sql.WorldDatabaseSelectQuery(sqlQuery).Tables["table"].Select().First().ItemArray.First().ToString();
            }
            else
                return "Unknown";
        }

        public static string GetSpellName(uint spellId)
        {
            if (Dbc.Dbc.SpellName.ContainsKey((int)spellId))
            {
                return Dbc.Dbc.SpellName[(int)spellId].Name;
            }

            return "Unknown";
        }

        public static string AddSpacesCount(uint count)
        {
            string spaces = "";

            for (uint i = 0; i < count; i++)
            {
                spaces += ' ';
            }

            return spaces;
        }

        public static string ToFormattedString(this TimeSpan span)
        {
            return $"{span.Hours:00}:{span.Minutes:00}:{span.Seconds:00}.{span.Milliseconds:000}";
        }

        public static bool IsEmoteRelatedToText(string text, uint emoteId)
        {
            var query = Sql.WorldDatabaseSelectQuery("SELECT `emote` FROM `creature_text` WHERE `text` LIKE " + "'%" + text.Replace("'", "''") + "%'");

            if (query.Tables["table"].Rows.Count != 0)
            {
                foreach (DataRow row in query.Tables["table"].Rows)
                {
                    foreach (var item in row.ItemArray)
                    {
                        if (item.ToString() == emoteId.ToString())
                            return true;
                    }
                }

                return false;
            }

            return false;
        }
    }
}
