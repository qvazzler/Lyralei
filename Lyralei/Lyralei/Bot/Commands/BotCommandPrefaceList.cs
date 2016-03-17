using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Lyralei.Bot.Commands
{
    public class BotCommandPrefaceList : List<BotCommandPreface>
    {
        public BotCommandPreface ParseFind(BotCommandInput input)
        {                                                                  
            BotCommandPreface prefacer = null;

            try
            {
                prefacer = this.Single(x => x.baseCmd.Name == input.BaseCommand);

                if (prefacer == null)
                    throw new NotImplementedException();
            }
            catch (InvalidOperationException ex)
            {
                throw new NotImplementedException("Command not contained within addon", ex);
            }
            catch (NotImplementedException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            for (int i = 0; i < prefacer.Parameters.Count; i++)
            {
                try
                {
                    Regex regx = new Regex(prefacer.Parameters[i].Regex);

                    if (regx.IsMatch(input.Parameters[i]))
                    {
                        //Parameter matches formatting requirements
                    }
                    else
                    {
                        throw new FormatException(prefacer.Parameters[i].Name + ": " + prefacer.Parameters[i].RegexName + " - " + prefacer.Parameters[i].RegexDescription);
                    }
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    throw new ArgumentOutOfRangeException(prefacer.Parameters[i].Name + ": " + prefacer.Parameters[i].RegexName + " - " + prefacer.Parameters[i].RegexDescription, ex);
                }
                catch (IndexOutOfRangeException ex)
                {
                    throw new IndexOutOfRangeException(prefacer.Parameters[i].Name + ": " + prefacer.Parameters[i].RegexName + " - " + prefacer.Parameters[i].RegexDescription, ex);
                }
            }

            if (prefacer.Parameters.Count != input.Parameters.Count)
            {
                throw new ArgumentOutOfRangeException("Too many arguments for the request");
            }

            return prefacer;
        }
    }
}
