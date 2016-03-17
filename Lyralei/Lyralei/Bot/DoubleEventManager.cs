﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lyralei.Bot
{
    class DoubleEventManager
    {
        private List<string> eventDumpStrings = new List<string>();

        public bool IsDouble(string eventValue)
        {
            //if (eventDumpStrings.Count(item => String.Join(" ", item) == String.Join(" ", eventDumpString)) == 2)
            if (eventDumpStrings.Count(item => item == eventValue) == 2)
            {
                eventDumpStrings.RemoveAll(item => item == eventValue);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}