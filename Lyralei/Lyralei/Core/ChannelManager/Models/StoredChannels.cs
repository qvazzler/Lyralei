using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using TS3QueryLib.Core.Server.Responses;
using TS3QueryLib.Core.Server.Entities;

namespace Lyralei.Core.ChannelManager.Models
{
    public class StoredChannels
    {
        public StoredChannels()
        {
            SetStoredChannelUniqueId();
        }

        public StoredChannels(int SubscriberId, string SubscriberUniqueId)
        {
            this.SubscriberId = SubscriberId;
            this.SubscriberUniqueId = SubscriberUniqueId;

            SetStoredChannelUniqueId();
        }

        public void SetStoredChannelUniqueId()
        {
            StoredChannelUniqueId = Guid.NewGuid().ToString("N");
        }

        [Key]
        public int StoredChannelId { get; private set; }

        public int SubscriberId { get; set; }
        public string SubscriberUniqueId { get; set; }

        // Channel custom data
        public string StoredChannelUniqueId { get; set; }

        // Channel properties
        public bool? ForcedSilence { get; protected set; }
        public bool? IsDefaultChannel { get; set; }
        public bool? IsMaxClientsUnlimited { get; set; }
        public bool? IsMaxFamilyClientsInherited { get; set; }
        public bool? IsMaxFamilyClientsUnlimited { get; set; }
        public bool? IsPasswordProtected { get; set; }
        public bool? IsPermanent { get; set; }
        public bool? IsSemiPermanent { get; set; }
        public bool? IsSpacer { get; set; }
        public bool? IsUnencrypted { get; set; }
        public double? CodecQuality { get; set; }
        public int? MaxClients { get; set; }
        public int? MaxFamilyClients { get; set; }
        public int? SecondsEmpty { get; set; }
        public int? TotalClients { get; set; }
        public int? TotalClientsFamily { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneticName { get; set; }
        public string SecuritySalt { get; set; }
        public string Topic { get; set; }
        public int? ChannelId { get; set; }
        public int? DeleteDelay { get; set; }
        public int? IconId { get; set; }
        public int? LatencyFactor { get; set; }
        public int? NeededSubscribePower { get; set; }
        public int? NeededTalkPower { get; set; }
        public int? Order { get; set; }
        public int? ParentChannelId { get; set; }
        public int? ChannelIconId { get; set; }
        public short? Codec { get; set; }

        //public string ServerQueryUsername { get; set; }
        //public string ServerQueryPassword { get; set; }

        //public string UserTeamSpeakClientUniqueId { get; set; }
        //public int UserId { get; set; } // foreign
        //public Core.UserManager.Models.Users Users { get; set; } // dependant

        // Convert to ChannelModification
        public ChannelModification ToChannelModification()
        {
            ChannelModification channelMod = new ChannelModification();

            //ForcedSilence
            channelMod.IsDefault = IsDefaultChannel;
            channelMod.HasUnlimitedMaxClients = IsMaxClientsUnlimited;
            //HasIsMaxFamilyClientsInherited
            channelMod.HasUnlimitedMaxFamilyClients = IsMaxFamilyClientsUnlimited;
            channelMod.IsPermanent = IsPermanent;
            channelMod.IsSemiPermanent = IsSemiPermanent;
            //IsUnencrypted
            channelMod.CodecQuality = CodecQuality;
            channelMod.MaxClients = MaxClients;
            channelMod.MaxFamilyClients = MaxFamilyClients;
            channelMod.Description = Description;
            //FilePath
            channelMod.Name = Name;
            //PasswordHash
            channelMod.NamePhonetic = PhoneticName;
            //SecuritySalt
            channelMod.Topic = Topic;
            //ChannelId
            //DeleteDelay
            channelMod.IconId = (uint?)IconId;
            //LatencyFactor
            //NeededSubscribePower
            channelMod.NeededTalkPower = (uint?)NeededTalkPower;
            channelMod.ChannelOrder = (uint?)Order;
            channelMod.ParentChannelId = (uint?)ParentChannelId;
            //IconId = ChannelIconId
            //Codec = TS3QueryLib.Core.Server.Entities.Codec;

            return channelMod;
        }

        // Parsing TS3 Channels
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
                ChannelId = (int?)entry.ChannelId;

            if (overWriteExisting || DeleteDelay == null)
                DeleteDelay = (int?)info.DeleteDelay;

            if ((overWriteExisting || IconId == null) && info.IconId != 0)
                IconId = (int?)info.IconId;

            if (overWriteExisting || LatencyFactor == null)
                LatencyFactor = (int?)info.LatencyFactor;

            if (overWriteExisting || NeededSubscribePower == null)
                NeededSubscribePower = (int?)entry.NeededSubscribePower;

            if (overWriteExisting || NeededTalkPower == null)
                NeededTalkPower = (int?)info.NeededTalkPower;

            if (overWriteExisting || Order == null)
                Order = (int?)info.Order;

            if (overWriteExisting || ParentChannelId == null)
                ParentChannelId = (int?)info.ParentChannelId;

            if (overWriteExisting || ChannelIconId == null)
                ChannelIconId = (int?)entry.ChannelIconId;

            if (overWriteExisting || Codec == null)
                Codec = (short?)info.Codec;
        }

        public void Parse(int ChannelId, ChannelInfoResponse info, bool overWriteExisting = false)
        {
            this.ChannelId = ChannelId;

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
                DeleteDelay = (int?)info.DeleteDelay;

            if ((overWriteExisting || IconId == null) && info.IconId != 0)
                IconId = (int?)info.IconId;

            if (overWriteExisting || LatencyFactor == null)
                LatencyFactor = (int?)info.LatencyFactor;

            if (overWriteExisting || NeededTalkPower == null)
                NeededTalkPower = (int?)info.NeededTalkPower;

            if (overWriteExisting || Order == null)
                Order = (int?)info.Order;

            if (overWriteExisting || ParentChannelId == null)
                ParentChannelId = (int?)info.ParentChannelId;

            if (overWriteExisting || Codec == null)
                Codec = (short?)info.Codec;
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
                ChannelId = (int?)entry.ChannelId;

            if (overWriteExisting || NeededSubscribePower == null)
                NeededSubscribePower = (int?)entry.NeededSubscribePower;

            if (overWriteExisting || NeededTalkPower == null)
                NeededTalkPower = (int?)entry.NeededTalkPower;

            if (overWriteExisting || Order == null)
                Order = (int?)entry.Order;

            if (overWriteExisting || ParentChannelId == null)
                ParentChannelId = (int?)entry.ParentChannelId;

            if (overWriteExisting || ChannelIconId == null)
                ChannelIconId = (int?)entry.ChannelIconId;

            if (overWriteExisting || Codec == null)
                Codec = (short?)entry.Codec;
        }

        public void Parse(string dumpString)
        {
            string[] query = dumpString.Replace("\t", "").Replace("\r", "").Split('\n');

            #region Parsing
            try
            {
                //Id -> ChannelId
                ChannelId = (int?)Int32.Parse(Array.Find(query, p => p.StartsWith("Id: ")).Replace("Id: ", ""));
            }
            catch (Exception) { }

            try
            {
                //ParentId -> ParentChannelId
                ParentChannelId = (int?)Int32.Parse(Array.Find(query, p => p.StartsWith("ParentChannelId: ")).Replace("ParentChannelId: ", ""));
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
                Codec = (short?)Int32.Parse(Array.Find(query, p => p.StartsWith("Codec: ")).Replace("Codec: ", ""));
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
                Order = (int?)Int32.Parse(Array.Find(query, p => p.StartsWith("Order: ")).Replace("Order: ", ""));
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
                DeleteDelay = (int?)Int32.Parse(Array.Find(query, p => p.StartsWith("DeleteDelay: ")).Replace("DeleteDelay: ", ""));
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
                NeededTalkPower = (int?)Int32.Parse(Array.Find(query, p => p.StartsWith("NeededTalkPower: ")).Replace("NeededTalkPower: ", ""));
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
