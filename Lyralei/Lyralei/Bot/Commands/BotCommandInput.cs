using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lyralei.Bot.Commands
{
    public class BotCommandInput
    {
        string cPrefix = "!";
        string cDelimiter = " ";
        public string BaseCommand = "";

        public List<string> Parameters = new List<string>();

        public BotCommandInput(string rawCmd, string prefix, string delimiter)
        {
            Parameters = Parse(rawCmd);
            BaseCommand = Parameters[0];

            Parameters.RemoveAt(0); //Remove the base command from Parameters
        }

        public List<string> Parse(string rawCmd)
        {
            //Remove any start prefix if still present
            if(rawCmd.StartsWith(cPrefix))
            rawCmd = rawCmd.Substring(cPrefix.Length);

            bool quoted = false;
            int lastSplitPos = 0;
            List<string> result = new List<string>();

            //Iterate through each character in order to do a smart split with use of quotes
            for (int i = 0; i < rawCmd.Length; i++)
            {
                //Console.WriteLine(rawCmd[i]);
                if (rawCmd[i] == cDelimiter[0] && quoted == false)
                {
                    //A delimiter was found, and it is not within quotes. Add new parameter!
                    result.Add(rawCmd.Substring(lastSplitPos, i));
                    lastSplitPos = i + 1;
                    //i++; //To skip the delimiter
                }
                else if (rawCmd[i] == '\"' && quoted == false && lastSplitPos == i)
                {
                    //A quotation mark was found, ignore any delimiters until we find the other one..
                    quoted = true;
                }
                else if (rawCmd[i] == '\"' && quoted == true && lastSplitPos != i && i + 1 > rawCmd.Length) //This one is for if we reach end of string
                {
                    //The second quotation mark was found. Add new parameter! (exclude the quotation marks though)
                    result.Add(rawCmd.Substring(lastSplitPos + 1, i - 1));
                    lastSplitPos = i + 2;
                    //i++;
                    quoted = false;
                }
                else if (rawCmd[i] == '\"' && quoted == true && lastSplitPos != i && (i+1 == rawCmd.Length || rawCmd[i + 1] == cDelimiter[0])) //This one is for if we're still in the middle of the string
                {
                    //The second quotation mark was found. Add new parameter! (exclude the quotation marks though)
                    result.Add(rawCmd.Substring(lastSplitPos + 1, i - lastSplitPos-1));
                    lastSplitPos = i + 2;
                    i++;
                    quoted = false;
                }
                else if (i == rawCmd.Length-1 && lastSplitPos != i)
                {
                    //If we've approached the final iterations, none of the above has matched, and the results array is empty, just add what we have.
                    result.Add(rawCmd.Substring(lastSplitPos, rawCmd.Length - lastSplitPos));
                }
                else if (rawCmd[i] != cDelimiter[0] || quoted == true)
                {
                    //Finally if the current iteration is just a character, or delimiter enclosed in quotes, just keep going.
                }
                else
                {
                    //If none of the above fits with the next step, something must have gone wrong. Abort!
                    throw new FormatException("Could not parse command");
                }
            }
            return result;
        }
    }
}
