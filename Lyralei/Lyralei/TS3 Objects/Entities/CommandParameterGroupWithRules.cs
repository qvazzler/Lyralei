using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS3QueryLib.Core.CommandHandling;

namespace Lyralei.TS3_Objects.Entities
{
    public class CommandParameterGroupWithRules : CommandParameterGroup
    {
        public CommandParameterGroupWithRules(CommandParameterGroupWithRules cmdPGE) : base()
        {
            this.AddRange(cmdPGE);
        }

        public CommandParameterGroupWithRules() : base()
        {

        }

        public CommandParameterWithRules this[string index]
        {
            get
            {
                return (CommandParameterWithRules)this.SingleOrDefault(x => x.Name == index);
            }
            protected set
            {
                // NOPE
            }
        }

        public void ValidateAddData(CommandParameterGroup cmdPG)
        {
            int recognizedParamsCount = 0;

            try
            {
                // First try to patch up the params with value missing, implying name to be used as value
                for (int i = 0; i < this.Count && i < cmdPG.Count(); i++)
                {
                    var frame = (CommandParameterWithRules)this[i];
                    var content = cmdPG[i];

                    if (cmdPG[i].Value == null)
                    {
                        if (frame.IsBaseCommand != true)
                        {
                            if (frame.NameValueSetting != NameValueSetting.NameOnly)
                                content.Value = content.Name;

                            // See if value fits the parameter, else throw exception
                            frame.ValidateSetData(content);
                            recognizedParamsCount++;

                            if (frame.NameValueSetting != NameValueSetting.NameOnly)
                                content.Value = null;
                        }
                    }
                }

                // Then iterate the known parameter name and value pairs
                foreach (var content in cmdPG.Where(cmdP => cmdP.Value != null))
                {
                    var matchingCmd = (CommandParameterWithRules)this.SingleOrDefault(frame => frame.Name == content.Name);
                    matchingCmd.ValidateSetData(content);
                    recognizedParamsCount++;
                }

                foreach (CommandParameterWithRules cmdP in this)
                    if(cmdP.Required && cmdP.Value == null)
                        throw new Exception(String.Format("Required parameter '{0}' not defined", cmdP.Name));

                if (recognizedParamsCount != cmdPG.Count - 1)
                    throw new Exception("Unrecognized params");

                if (this.Distinct().Count() != this.Count)
                    throw new Exception("Duplicate parameter names");
            }
            catch (Exception ex)
            {
                //Reset data
                this.ForEach(x => x.Value = null);
                throw new Exception("Value validation failed. See inner exception for details.", ex);
            }
        }
    }
}
