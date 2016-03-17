using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TS3QueryLib.Core.CommandHandling;
using TS3QueryLib.Core.Common;

namespace Lyralei.TS3_Objects.EventArguments
{
    public static class PermissionMessages
    {
        public static string PermissionDenied = "You lack permission to execute this command, or it does not exist.";
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
