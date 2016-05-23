using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TS3QueryLib.Core;
using TS3QueryLib.Core.Common;
using TS3QueryLib.Core.Common.Responses;
using TS3QueryLib.Core.Server;
using TS3QueryLib.Core.Server.Responses;
using TS3QueryLib.Core.Server.Entities;

namespace Lyralei.Core.ChannelManager.Props
{
    public class Channel
    {
        public Channel()
        {

        }

        public Channel(string dumpString)
        {
            Parse(dumpString);
        }

        public bool? ForcedSilence { get; protected set; }
        public bool? IsDefaultChannel;
        public bool? IsMaxClientsUnlimited;
        public bool? IsMaxFamilyClientsInherited;
        public bool? IsMaxFamilyClientsUnlimited;
        public bool? IsPasswordProtected;
        public bool? IsPermanent;
        public bool? IsSemiPermanent;
        public bool? IsSpacer;
        public bool? IsUnencrypted;
        public double? CodecQuality;
        public int? MaxClients;
        public int? MaxFamilyClients;
        public int? SecondsEmpty;
        public int? TotalClients;
        public int? TotalClientsFamily;
        public string Description;
        public string FilePath;
        public string Name;
        public string PasswordHash;
        public string PhoneticName;
        public string SecuritySalt;
        public string Topic;
        public uint? ChannelId;
        public uint? DeleteDelay;
        public uint? IconId;
        public uint? LatencyFactor;
        public uint? NeededSubscribePower;
        public uint? NeededTalkPower;
        public uint? Order;
        public uint? ParentChannelId;
        public uint? ChannelIconId;
        public ushort? Codec;

        public void Parse(ChannelInfoResponse info, ChannelListEntry entry, bool overWriteExisting = false)
        {
            if (overWriteExisting || ForcedSilence == null)
                ForcedSilence = info.ForcedSilence;

            if (overWriteExisting || IsDefaultChannel == null)
                IsDefaultChannel = info.IsDefaultChannel;

            if (overWriteExisting || IsMaxClientsUnlimited == null)
                IsMaxClientsUnlimited = info.IsMaxClientsUnlimited;

            if (overWriteExisting || IsMaxFamilyClientsInherited == null)
                IsMaxFamilyClientsInherited = info.IsMaxFamilyClientsInherited;

            if (overWriteExisting || IsMaxFamilyClientsUnlimited == null)
                IsMaxFamilyClientsUnlimited = info.IsMaxFamilyClientsUnlimited;

            if (overWriteExisting || IsPasswordProtected == null)
                IsPasswordProtected = info.IsPasswordProtected;

            if (overWriteExisting || IsPermanent == null)
                IsPermanent = info.IsPermanent;

            if (overWriteExisting || IsSemiPermanent == null)
                IsSemiPermanent = info.IsSemiPermanent;

            if (overWriteExisting || IsSpacer == null)
                IsSpacer = entry.IsSpacer;

            if (overWriteExisting || IsUnencrypted == null)
                IsUnencrypted = info.IsUnencrypted;

            if (overWriteExisting || CodecQuality == null)
                CodecQuality = info.CodecQuality;

            if (overWriteExisting || MaxClients == null)
                MaxClients = info.MaxClients;

            if (overWriteExisting || MaxFamilyClients == null)
                MaxFamilyClients = info.MaxFamilyClients;

            if (overWriteExisting || SecondsEmpty == null)
                SecondsEmpty = info.SecondsEmpty;

            if (overWriteExisting || TotalClients == null)
                TotalClients = entry.TotalClients;

            if (overWriteExisting || TotalClientsFamily == null)
                TotalClientsFamily = entry.TotalClientsFamily;

            if (overWriteExisting || Description == null)
                Description = info.Description;

            if (overWriteExisting || FilePath == null)
                FilePath = info.FilePath;

            if (overWriteExisting || Name == null)
                Name = info.Name;

            if (overWriteExisting || PasswordHash == null)
                PasswordHash = info.PasswordHash;

            if (overWriteExisting || PhoneticName == null)
                PhoneticName = info.PhoneticName;

            if (overWriteExisting || SecuritySalt == null)
                SecuritySalt = info.SecuritySalt;

            if (overWriteExisting || Topic == null)
                Topic = info.Topic;

            if (overWriteExisting || ChannelId == null)
                ChannelId = entry.ChannelId;

            if (overWriteExisting || DeleteDelay == null)
                DeleteDelay = info.DeleteDelay;

            if (overWriteExisting || IconId == null)
                IconId = info.IconId;

            if (overWriteExisting || LatencyFactor == null)
                LatencyFactor = info.LatencyFactor;

            if (overWriteExisting || NeededSubscribePower == null)
                NeededSubscribePower = entry.NeededSubscribePower;

            if (overWriteExisting || NeededTalkPower == null)
                NeededTalkPower = info.NeededTalkPower;

            if (overWriteExisting || Order == null)
                Order = info.Order;

            if (overWriteExisting || ParentChannelId == null)
                ParentChannelId = info.ParentChannelId;

            if (overWriteExisting || ChannelIconId == null)
                ChannelIconId = entry.ChannelIconId;

            if (overWriteExisting || Codec == null)
                Codec = info.Codec;
        }

        public void Parse(ChannelInfoResponse info, bool overWriteExisting = false)
        {
            if (overWriteExisting || ForcedSilence == null)
                ForcedSilence = info.ForcedSilence;

            if (overWriteExisting || IsDefaultChannel == null)
                IsDefaultChannel = info.IsDefaultChannel;

            if (overWriteExisting || IsMaxClientsUnlimited == null)
                IsMaxClientsUnlimited = info.IsMaxClientsUnlimited;

            if (overWriteExisting || IsMaxFamilyClientsInherited == null)
                IsMaxFamilyClientsInherited = info.IsMaxFamilyClientsInherited;

            if (overWriteExisting || IsMaxFamilyClientsUnlimited == null)
                IsMaxFamilyClientsUnlimited = info.IsMaxFamilyClientsUnlimited;

            if (overWriteExisting || IsPasswordProtected == null)
                IsPasswordProtected = info.IsPasswordProtected;

            if (overWriteExisting || IsPermanent == null)
                IsPermanent = info.IsPermanent;

            if (overWriteExisting || IsSemiPermanent == null)
                IsSemiPermanent = info.IsSemiPermanent;

            if (overWriteExisting || IsUnencrypted == null)
                IsUnencrypted = info.IsUnencrypted;

            if (overWriteExisting || CodecQuality == null)
                CodecQuality = info.CodecQuality;

            if (overWriteExisting || MaxClients == null)
                MaxClients = info.MaxClients;

            if (overWriteExisting || MaxFamilyClients == null)
                MaxFamilyClients = info.MaxFamilyClients;

            if (overWriteExisting || SecondsEmpty == null)
                SecondsEmpty = info.SecondsEmpty;

            if (overWriteExisting || Description == null)
                Description = info.Description;

            if (overWriteExisting || FilePath == null)
                FilePath = info.FilePath;

            if (overWriteExisting || Name == null)
                Name = info.Name;

            if (overWriteExisting || PasswordHash == null)
                PasswordHash = info.PasswordHash;

            if (overWriteExisting || PhoneticName == null)
                PhoneticName = info.PhoneticName;

            if (overWriteExisting || SecuritySalt == null)
                SecuritySalt = info.SecuritySalt;

            if (overWriteExisting || Topic == null)
                Topic = info.Topic;

            if (overWriteExisting || DeleteDelay == null)
                DeleteDelay = info.DeleteDelay;

            if (overWriteExisting || IconId == null)
                IconId = info.IconId;

            if (overWriteExisting || LatencyFactor == null)
                LatencyFactor = info.LatencyFactor;

            if (overWriteExisting || NeededTalkPower == null)
                NeededTalkPower = info.NeededTalkPower;

            if (overWriteExisting || Order == null)
                Order = info.Order;

            if (overWriteExisting || ParentChannelId == null)
                ParentChannelId = info.ParentChannelId;

            if (overWriteExisting || Codec == null)
                Codec = info.Codec;
        }

        public void Parse(ChannelListEntry entry, bool overWriteExisting = false)
        {
            if (overWriteExisting || IsDefaultChannel == null)
                IsDefaultChannel = entry.IsDefaultChannel;

            if (overWriteExisting || IsPasswordProtected == null)
                IsPasswordProtected = entry.IsPasswordProtected;

            if (overWriteExisting || IsPermanent == null)
                IsPermanent = entry.IsPermanent;

            if (overWriteExisting || IsSemiPermanent == null)
                IsSemiPermanent = entry.IsSemiPermanent;

            if (overWriteExisting || IsSpacer == null)
                IsSpacer = entry.IsSpacer;

            if (overWriteExisting || CodecQuality == null)
                CodecQuality = entry.CodecQuality;

            if (overWriteExisting || MaxClients == null)
                MaxClients = entry.MaxClients;

            if (overWriteExisting || MaxFamilyClients == null)
                MaxFamilyClients = entry.MaxFamilyClients;

            if (overWriteExisting || TotalClients == null)
                TotalClients = entry.TotalClients;

            if (overWriteExisting || TotalClientsFamily == null)
                TotalClientsFamily = entry.TotalClientsFamily;

            if (overWriteExisting || Name == null)
                Name = entry.Name;

            if (overWriteExisting || Topic == null)
                Topic = entry.Topic;

            if (overWriteExisting || ChannelId == null)
                ChannelId = entry.ChannelId;

            if (overWriteExisting || NeededSubscribePower == null)
                NeededSubscribePower = entry.NeededSubscribePower;

            if (overWriteExisting || NeededTalkPower == null)
                NeededTalkPower = entry.NeededTalkPower;

            if (overWriteExisting || Order == null)
                Order = entry.Order;

            if (overWriteExisting || ParentChannelId == null)
                ParentChannelId = entry.ParentChannelId;

            if (overWriteExisting || ChannelIconId == null)
                ChannelIconId = entry.ChannelIconId;

            if (overWriteExisting || Codec == null)
                Codec = entry.Codec;
        }

        public void Parse(string dumpString)
        {
            string[] query = dumpString.Replace("\t", "").Replace("\r", "").Split('\n');

            #region Parsing
            try
            {
                //Id -> ChannelId
                ChannelId = (uint?)Int32.Parse(Array.Find(query, p => p.StartsWith("Id: ")).Replace("Id: ", ""));
            }
            catch (Exception) { }

            try
            {
                //ParentId -> ParentChannelId
                ParentChannelId = (uint?)Int32.Parse(Array.Find(query, p => p.StartsWith("ParentChannelId: ")).Replace("ParentChannelId: ", ""));
            }
            catch (Exception) { }

            try
            {
                //Name -> Name
                Name = Array.Find(query, p => p.StartsWith("Name: ")).Replace("Name: ", "");
            }
            catch (Exception) { }

            try
            {
                //Topic -> Topic
                Topic = Array.Find(query, p => p.StartsWith("Topic: ")).Replace("Topic: ", "");
            }
            catch (Exception) { }

            try
            {
                //NamePhonetic -> PhoneticName
                PhoneticName = Array.Find(query, p => p.StartsWith("NamePhonetic: ")).Replace("NamePhonetic: ", "");
            }
            catch (Exception) { }

            try
            {
                //Codec -> Codec
                Codec = (ushort?)Int32.Parse(Array.Find(query, p => p.StartsWith("Codec: ")).Replace("Codec: ", ""));
            }
            catch (Exception) { }

            try
            {
                //CodecQuality -> CodecQuality
                CodecQuality = (double?)Double.Parse(Array.Find(query, p => p.StartsWith("CodecQuality: ")).Replace("CodecQuality: ", ""));
            }
            catch (Exception) { }

            try
            {
                //Order -> Order
                Order = (uint?)Int32.Parse(Array.Find(query, p => p.StartsWith("Order: ")).Replace("Order: ", ""));
            }
            catch (Exception) { }

            try
            {
                //FlagPassword -> IsPasswordProtected
                IsPasswordProtected = (bool?)Boolean.Parse(Array.Find(query, p => p.StartsWith("FlagPassword: ")).Replace("FlagPassword: ", ""));
            }
            catch (Exception) { }

            try
            {
                //FlagSemiPermanent -> IsSemiPermanent
                IsSemiPermanent = (bool?)Boolean.Parse(Array.Find(query, p => p.StartsWith("FlagSemiPermanent: ")).Replace("FlagSemiPermanent: ", ""));
            }
            catch (Exception) { }

            try
            {
                //FlagSemiPermanent -> IsSemiPermanent
                IsSemiPermanent = (bool?)Boolean.Parse(Array.Find(query, p => p.StartsWith("FlagSemiPermanent: ")).Replace("FlagSemiPermanent: ", ""));
            }
            catch (Exception) { }

            try
            {
                //DeleteDelay -> DeleteDelay
                DeleteDelay = (uint?)Int32.Parse(Array.Find(query, p => p.StartsWith("DeleteDelay: ")).Replace("DeleteDelay: ", ""));
            }
            catch (Exception) { }

            try
            {
                //FlagPermanent -> IsPermanent
                IsPermanent = (bool?)Boolean.Parse(Array.Find(query, p => p.StartsWith("FlagPermanent: ")).Replace("FlagPermanent: ", ""));
            }
            catch (Exception) { }

            try
            {
                //FlagDefault -> IsDefaultChannel
                IsDefaultChannel = (bool?)Boolean.Parse(Array.Find(query, p => p.StartsWith("FlagDefault: ")).Replace("FlagDefault: ", ""));
            }
            catch (Exception) { }

            try
            {
                //CodecIsUnencrypted -> IsUnencrypted
                IsUnencrypted = (bool?)Boolean.Parse(Array.Find(query, p => p.StartsWith("CodecIsUnencrypted: ")).Replace("CodecIsUnencrypted: ", ""));
            }
            catch (Exception) { }

            try
            {
                //NeededTalkPower -> NeededTalkPower
                NeededTalkPower = (uint?)Int32.Parse(Array.Find(query, p => p.StartsWith("NeededTalkPower: ")).Replace("NeededTalkPower: ", ""));
            }
            catch (Exception) { }

            try
            {
                //MaxClients -> MaxClients
                MaxClients = (int?)Int32.Parse(Array.Find(query, p => p.StartsWith("MaxClients: ")).Replace("MaxClients: ", ""));
            }
            catch (Exception) { }

            try
            {
                //MaxFamilyClients -> MaxFamilyClients
                MaxFamilyClients = (int?)Int32.Parse(Array.Find(query, p => p.StartsWith("MaxFamilyClients: ")).Replace("MaxFamilyClients: ", ""));
            }
            catch (Exception) { }

            try
            {
                //FlagMaxClients_Unlimited -> MaxFamilyClients
                IsMaxClientsUnlimited = (bool?)Boolean.Parse(Array.Find(query, p => p.StartsWith("FlagMaxClients_Unlimited: ")).Replace("FlagMaxClients_Unlimited: ", ""));
            }
            catch (Exception) { }

            try
            {
                //FlagMaxFamilyClients_Unlimited -> IsMaxFamilyClientsUnlimited
                IsMaxFamilyClientsUnlimited = (bool?)Boolean.Parse(Array.Find(query, p => p.StartsWith("FlagMaxFamilyClients_Unlimited: ")).Replace("FlagMaxFamilyClients_Unlimited: ", ""));
            }
            catch (Exception) { }

            try
            {
                //FlagMaxFamilyClients_Inherited -> IsMaxFamilyClientsInherited
                IsMaxFamilyClientsInherited = (bool?)Boolean.Parse(Array.Find(query, p => p.StartsWith("FlagMaxFamilyClients_Inherited: ")).Replace("FlagMaxFamilyClients_Inherited: ", ""));
            }
            catch (Exception) { }

            #endregion
        }
    }
}
