using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TS3QueryLib.Core.Common;

namespace Lyralei.TS3_Objects.EventArguments
{
    /*
     * namespace TS3QueryLib.Core.Server.Entities
{
    public enum Codec
    {
        SpeexNarrowband = 0,
        SpeexWideband = 1,
        SpeexUltrawideband = 2,
        CeltMono = 3,
    }
}*/

    public struct dbProperty
    {
        public int ServerId;
        public string Name;
        public string Value;
    }

    public struct dbPermission
    {
        public int ServerId;
        public string Name;
        public string ServerGroupId;
        public bool Allow;
        public string CustomData;
    }

    public class ChannelCreatedEventArgs : EventArgs, IDump
    {
        public int? Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Topic { get; set; }
        public string NamePhonetic { get; set; }
        public int? Codec { get; set; }
        public int? CodecQuality { get; set; }
        public int? Order { get; set; }
        public int? FlagPassword { get; set; }
        public int? FlagSemiPermanent { get; set; }
        public int? DeleteDelay { get; set; }
        public int? FlagPermanent { get; set; }
        public int? FlagDefault { get; set; }
        public int? CodecIsUnencrypted { get; set; }
        public int? NeededTalkPower { get; set; }
        public int? MaxClients { get; set; }
        public int? MaxFamilyClients { get; set; }
        public int? FlagMaxClients_Unlimited { get; set; }
        public int? FlagMaxFamilyClients_Unlimited { get; set; }
        public int? FlagMaxFamilyClients_Inherited { get; set; }
        public int? InvokerClientId { get; set; }
        public string InvokerNickname { get; set; }
        public string InvokerUniqueId { get; set; }

        public ChannelCreatedEventArgs()
        {
        }

        public ChannelCreatedEventArgs(string[] query)
        {
            Parse(query);
        }

        public void Parse(string[] query)
        {
            #region lots of parsing
            try
            {
                //cid
                Id = Int32.Parse(Array.Find(query, p => p.StartsWith("cid=")).Replace("cid=", ""));
            }
            catch (Exception) { }

            try
            {
                //cpid
                ParentId = Int32.Parse(Array.Find(query, p => p.StartsWith("cpid=")).Replace("cpid=", ""));
            }

            catch (Exception) { }

            try
            {
                //channel_name
                Name = Array.Find(query, p => p.StartsWith("channel_name=")).Replace("channel_name=", "");
            }
            catch (Exception) { }

            try
            {
                //channel_topic
                Topic = Array.Find(query, p => p.StartsWith("channel_topic=")).Replace("channel_topic=", "");
            }
            catch (Exception) { }

            try
            {
                //channel_name_phonetic
                NamePhonetic = Array.Find(query, p => p.StartsWith("channel_name_phonetic=")).Replace("channel_name_phonetic=", "");
            }
            catch (Exception) { }

            try
            {
                //channel_codec
                Codec = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_codec=")).Replace("channel_codec=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_codec_quality
                CodecQuality = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_codec_quality=")).Replace("channel_codec_quality=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_flag_permanent
                FlagPermanent = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_flag_permanent=")).Replace("channel_flag_permanent=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_flag_default
                FlagDefault = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_flag_default=")).Replace("channel_flag_default=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_needed_talk_power
                NeededTalkPower = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_needed_talk_power=")).Replace("channel_needed_talk_power=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_order
                Order = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_order=")).Replace("channel_order=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_codec_is_unencrypted
                CodecIsUnencrypted = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_codec_is_unencrypted=")).Replace("channel_codec_is_unencrypted=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_flag_maxfamilyclients_unlimited
                FlagMaxFamilyClients_Unlimited = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_flag_maxfamilyclients_unlimited=")).Replace("channel_flag_maxfamilyclients_unlimited=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_flag_maxfamilyclients_inherited
                FlagMaxFamilyClients_Inherited = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_flag_maxfamilyclients_inherited=")).Replace("channel_flag_maxfamilyclients_inherited=", ""));
            }
            catch (Exception) { }

            try
            {
                //invokerid
                InvokerClientId = Int32.Parse(Array.Find(query, p => p.StartsWith("invokerid=")).Replace("invokerid=", ""));
            }
            catch (Exception) { }

            try
            {
                //invokername
                InvokerNickname = Array.Find(query, p => p.StartsWith("invokername=")).Replace("invokername=", "");
            }
            catch (Exception) { }

            try
            {
                //invokeruid
                InvokerUniqueId = Array.Find(query, p => p.StartsWith("invokeruid=")).Replace("invokeruid=", "");
            }
            catch (Exception) { }

            try
            {
                //channel_delete_delay
                DeleteDelay = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_delete_delay=")).Replace("channel_delete_delay=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_flag_password
                FlagPassword = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_flag_password=")).Replace("channel_flag_password=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_clag_permanent
                FlagPermanent = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_flag_permanent=")).Replace("channel_flag_permanent=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_flag_semi_permanent
                FlagSemiPermanent = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_flag_semi_permanent=")).Replace("channel_flag_semi_permanent=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_maxclients
                MaxClients = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_maxclients=")).Replace("channel_maxclients=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_maxfamilyclients
                MaxFamilyClients = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_maxfamilyclients=")).Replace("channel_maxfamilyclients=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_flag_maxclients_unlimited
                FlagMaxClients_Unlimited = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_flag_maxclients_unlimited=")).Replace("channel_flag_maxclients_unlimited=", ""));
            }
            catch (Exception) { }

            #endregion
        }
    }

    public class ChannelEditedEventArgs : EventArgs, IDump
    {
        public int? Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Topic { get; set; }
        public string NamePhonetic { get; set; }
        public int? Codec { get; set; }
        public int? CodecQuality { get; set; }
        public int? Order { get; set; }
        public int? FlagPassword { get; set; }
        public int? FlagSemiPermanent { get; set; }
        public int? DeleteDelay { get; set; }
        public int? FlagPermanent { get; set; }
        public int? FlagDefault { get; set; }
        public int? CodecIsUnencrypted { get; set; }
        public int? NeededTalkPower { get; set; }
        public int? MaxClients { get; set; }
        public int? MaxFamilyClients { get; set; }
        public int? FlagMaxClients_Unlimited { get; set; }
        public int? FlagMaxFamilyClients_Unlimited { get; set; }
        public int? FlagMaxFamilyClients_Inherited { get; set; }
        public int? InvokerClientId { get; set; }
        public string InvokerNickname { get; set; }
        public string InvokerUniqueId { get; set; }

        public ChannelEditedEventArgs()
        {

        }

        public ChannelEditedEventArgs(string[] query)
        {
            Parse(query);
        }

        public void Parse(string[] query)
        {
            #region lots of parsing
            try
            {
                //cid
                Id = Int32.Parse(Array.Find(query, p => p.StartsWith("cid=")).Replace("cid=", ""));
            }
            catch (Exception) { }

            try
            {
                //cpid
                ParentId = Int32.Parse(Array.Find(query, p => p.StartsWith("cpid=")).Replace("cpid=", ""));
            }

            catch (Exception) { }

            try
            {
                //channel_name
                Name = Array.Find(query, p => p.StartsWith("channel_name=")).Replace("channel_name=", "");
            }
            catch (Exception) { }

            try
            {
                //channel_topic
                Topic = Array.Find(query, p => p.StartsWith("channel_topic=")).Replace("channel_topic=", "");
            }
            catch (Exception) { }

            try
            {
                //channel_name_phonetic
                NamePhonetic = Array.Find(query, p => p.StartsWith("channel_name_phonetic=")).Replace("channel_name_phonetic=", "");
            }
            catch (Exception) { }

            try
            {
                //channel_codec
                Codec = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_codec=")).Replace("channel_codec=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_codec_quality
                CodecQuality = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_codec_quality=")).Replace("channel_codec_quality=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_flag_permanent
                FlagPermanent = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_flag_permanent=")).Replace("channel_flag_permanent=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_flag_default
                FlagDefault = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_flag_default=")).Replace("channel_flag_default=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_needed_talk_power
                NeededTalkPower = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_needed_talk_power=")).Replace("channel_needed_talk_power=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_order
                Order = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_order=")).Replace("channel_order=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_codec_is_unencrypted
                CodecIsUnencrypted = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_codec_is_unencrypted=")).Replace("channel_codec_is_unencrypted=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_flag_maxfamilyclients_unlimited
                FlagMaxFamilyClients_Unlimited = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_flag_maxfamilyclients_unlimited=")).Replace("channel_flag_maxfamilyclients_unlimited=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_flag_maxfamilyclients_inherited
                FlagMaxFamilyClients_Inherited = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_flag_maxfamilyclients_inherited=")).Replace("channel_flag_maxfamilyclients_inherited=", ""));
            }
            catch (Exception) { }

            try
            {
                //invokerid
                InvokerClientId = Int32.Parse(Array.Find(query, p => p.StartsWith("invokerid=")).Replace("invokerid=", ""));
            }
            catch (Exception) { }

            try
            {
                //invokername
                InvokerNickname = Array.Find(query, p => p.StartsWith("invokername=")).Replace("invokername=", "");
            }
            catch (Exception) { }

            try
            {
                //invokeruid
                InvokerUniqueId = Array.Find(query, p => p.StartsWith("invokeruid=")).Replace("invokeruid=", "");
            }
            catch (Exception) { }

            try
            {
                //channel_delete_delay
                DeleteDelay = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_delete_delay=")).Replace("channel_delete_delay=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_flag_password
                FlagPassword = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_flag_password=")).Replace("channel_flag_password=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_clag_permanent
                FlagPermanent = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_flag_permanent=")).Replace("channel_flag_permanent=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_flag_semi_permanent
                FlagSemiPermanent = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_flag_semi_permanent=")).Replace("channel_flag_semi_permanent=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_maxclients
                MaxClients = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_maxclients=")).Replace("channel_maxclients=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_maxfamilyclients
                MaxFamilyClients = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_maxfamilyclients=")).Replace("channel_maxfamilyclients=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_flag_maxclients_unlimited
                FlagMaxClients_Unlimited = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_flag_maxclients_unlimited=")).Replace("channel_flag_maxclients_unlimited=", ""));
            }
            catch (Exception) { }

            #endregion
        }
    }

    public class ChannelMovedEventArgs : EventArgs, IDump
    {
        public int? Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Topic { get; set; }
        public string NamePhonetic { get; set; }
        public int? Codec { get; set; }
        public int? CodecQuality { get; set; }
        public int? Order { get; set; }
        public int? FlagPassword { get; set; }
        public int? FlagSemiPermanent { get; set; }
        public int? DeleteDelay { get; set; }
        public int? FlagPermanent { get; set; }
        public int? FlagDefault { get; set; }
        public int? CodecIsUnencrypted { get; set; }
        public int? NeededTalkPower { get; set; }
        public int? MaxClients { get; set; }
        public int? MaxFamilyClients { get; set; }
        public int? FlagMaxClients_Unlimited { get; set; }
        public int? FlagMaxFamilyClients_Unlimited { get; set; }
        public int? FlagMaxFamilyClients_Inherited { get; set; }
        public int? InvokerClientId { get; set; }
        public string InvokerNickname { get; set; }
        public string InvokerUniqueId { get; set; }

        public ChannelMovedEventArgs()
        {

        }

        public ChannelMovedEventArgs(string[] query)
        {
            Parse(query);
        }

        public void Parse(string[] query)
        {
            #region lots of parsing
            try
            {
                //cid
                Id = Int32.Parse(Array.Find(query, p => p.StartsWith("cid=")).Replace("cid=", ""));
            }
            catch (Exception) { }

            try
            {
                //cpid
                ParentId = Int32.Parse(Array.Find(query, p => p.StartsWith("cpid=")).Replace("cpid=", ""));
            }

            catch (Exception) { }

            try
            {
                //channel_name
                Name = Array.Find(query, p => p.StartsWith("channel_name=")).Replace("channel_name=", "");
            }
            catch (Exception) { }

            try
            {
                //channel_topic
                Topic = Array.Find(query, p => p.StartsWith("channel_topic=")).Replace("channel_topic=", "");
            }
            catch (Exception) { }

            try
            {
                //channel_name_phonetic
                NamePhonetic = Array.Find(query, p => p.StartsWith("channel_name_phonetic=")).Replace("channel_name_phonetic=", "");
            }
            catch (Exception) { }

            try
            {
                //channel_codec
                Codec = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_codec=")).Replace("channel_codec=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_codec_quality
                CodecQuality = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_codec_quality=")).Replace("channel_codec_quality=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_flag_permanent
                FlagPermanent = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_flag_permanent=")).Replace("channel_flag_permanent=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_flag_default
                FlagDefault = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_flag_default=")).Replace("channel_flag_default=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_needed_talk_power
                NeededTalkPower = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_needed_talk_power=")).Replace("channel_needed_talk_power=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_order
                Order = Int32.Parse(Array.Find(query, p => p.StartsWith("order=")).Replace("order=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_codec_is_unencrypted
                CodecIsUnencrypted = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_codec_is_unencrypted=")).Replace("channel_codec_is_unencrypted=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_flag_maxfamilyclients_unlimited
                FlagMaxFamilyClients_Unlimited = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_flag_maxfamilyclients_unlimited=")).Replace("channel_flag_maxfamilyclients_unlimited=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_flag_maxfamilyclients_inherited
                FlagMaxFamilyClients_Inherited = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_flag_maxfamilyclients_inherited=")).Replace("channel_flag_maxfamilyclients_inherited=", ""));
            }
            catch (Exception) { }

            try
            {
                //invokerid
                InvokerClientId = Int32.Parse(Array.Find(query, p => p.StartsWith("invokerid=")).Replace("invokerid=", ""));
            }
            catch (Exception) { }

            try
            {
                //invokername
                InvokerNickname = Array.Find(query, p => p.StartsWith("invokername=")).Replace("invokername=", "");
            }
            catch (Exception) { }

            try
            {
                //invokeruid
                InvokerUniqueId = Array.Find(query, p => p.StartsWith("invokeruid=")).Replace("invokeruid=", "");
            }
            catch (Exception) { }

            try
            {
                //channel_delete_delay
                DeleteDelay = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_delete_delay=")).Replace("channel_delete_delay=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_flag_password
                FlagPassword = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_flag_password=")).Replace("channel_flag_password=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_clag_permanent
                FlagPermanent = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_flag_permanent=")).Replace("channel_flag_permanent=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_flag_semi_permanent
                FlagSemiPermanent = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_flag_semi_permanent=")).Replace("channel_flag_semi_permanent=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_maxclients
                MaxClients = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_maxclients=")).Replace("channel_maxclients=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_maxfamilyclients
                MaxFamilyClients = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_maxfamilyclients=")).Replace("channel_maxfamilyclients=", ""));
            }
            catch (Exception) { }

            try
            {
                //channel_flag_maxclients_unlimited
                FlagMaxClients_Unlimited = Int32.Parse(Array.Find(query, p => p.StartsWith("channel_flag_maxclients_unlimited=")).Replace("channel_flag_maxclients_unlimited=", ""));
            }
            catch (Exception) { }

            #endregion
        }
    }

    public static class DebugMessages
    {
        //Key-related
        public static string KEY_WAIT = "Waiting for key";
        public static string KEY_GOT = "Got key";
        public static string KEY_RETURN = "Returned key";

        //Double-event related
        public static string DOUBLE_EVENT = "Double event";

        //Event-related
        public static string INVOKING_EVENTS = "Invoking events..";
        public static string INVOKED_EVENTS = "Events invoked";
    }

    public static class PermissionMessages
    {
        public static string PermissionDenied = "You lack permission to execute this command, or it does not exist.";
    }

    public enum MsgTypes
    {
        Key = 0,
        Event = 1,
        Info = 2,
        WarnException = 3,
        CriticalException = 4,
    }

    public enum Codec
    {
        SpeexNarrowband = 0,
        SpeexWideband = 1,
        SpeexUltrawideband = 2,
        CeltMono = 3,
        OpusVoice = 4,
        OpusMusic = 5,
    }

    public struct MySQLServerInfo
    {
        //string cs = @"server=localhost;userid=root;password=Password;database=squall";
        public int id;
        public string server;
        public string userid;
        public string password;
        public string database;
    }

    public struct ServerInfo
    {
        public int id;
        public string server;
        public ushort port;
        public string user;
        public string pass;
        public uint virtualserver_id;
    }

    public enum AutoChannelNameType
    {
        Numeric = 0,
        Alphabetic = 1,
        LFG = 2,
    }

    public static class Naming_Entities
    {
        public static string[] LFG = {
                                         "Beginner",
                                         "Low",
                                         "Mid",
                                         "High",
                                         "Very High",
                                     };

        public static string Number_Prefix = " #";
        public static string Number_Suffix = "";
        public static string LFG_Prefix = "[ ";
        public static string LFG_Suffix = " ]";

        public static string[] Alphabet = {
                                       "Alpha",
                                       "Bravo",
                                       "Charlie",
                                       "Delta",
                                       "Echo",
                                       "Foxtrot",
                                       "Golf",
                                       "Hotel",
                                       "India",
                                       "Juliett",
                                       "Kilo",
                                       "Lima",
                                       "Mike",
                                       "November",
                                       "Oscar",
                                       "Papa",
                                       "Quebec",
                                       "Romeo",
                                       "Sierra",
                                       "Tango",
                                       "Uniform",
                                       "Victor",
                                       "Whiskey",
                                       "Xray",
                                       "Yankee"
                                   };

        public static string Alphabet_Delimiter = "Zulu";
    }
}
