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
        //public string AddonName;

        public CommandParameterGroupWithRules(CommandParameterGroupWithRules cmdPGE) : base()
        {
            this.AddRange(cmdPGE);
        }

        public CommandParameterGroupWithRules() : base()
        {
            //this.AddonName = AddonName;
        }

        public void ValidateAddData(CommandParameterGroup cmdPG)
        {
            var cmdPGFrames = this;

            int recognizedParamsCount = 0;

            try
            {
                var cmdPGWithoutValues = cmdPG.Where(cmdP => cmdP.Value == null);

                // First try to patch up the params with value missing, implying name to be used as value
                for (int i = 0; i < cmdPGWithoutValues.Count(); i++)
                {
                    var frame = (CommandParameterWithRules)cmdPGFrames[i];
                    var content = cmdPG[i];

                    if (frame.IsBaseCommand != true)
                    {
                        // See if value fits the parameter, else throw excetion on the whole operation
                        content.Value = frame.Name;
                        frame.ValidateSetData(content);
                        recognizedParamsCount++;
                    }
                }

                // Then iterate the known parameter name and value pairs
                foreach (var content in cmdPG.Where(cmdP => cmdP.Value != null))
                {
                    var test = (CommandParameterWithRules)cmdPGFrames.SingleOrDefault(frame => frame.Name == content.Name);
                    test.ValidateSetData(content);
                    recognizedParamsCount++;
                }

                if (recognizedParamsCount != cmdPG.Count-1)
                    throw new Exception("Unrecognized params");

                // Parameter data is a correct fit. Let's put them to use.
                //for (int i = 0; i < this.Count; i++)
                //{
                //    //var yes = (CommandParameterExpectations)this[i];
                //    //yes.ValidateSetData(
                //    this[i] = cmdPGFrames[i];
                //}
            }
            catch (Exception)
            {

            }

            //if (frames.SingleOrDefault(frame => frame.Name == ))
            //{

            //}

            //CommandParameterExpectations frame = (CommandParameterExpectations)frames[frameIndex];


            //if (cmdPG[i].Value == null && frame.NameValueSetting == NameValueSetting.NameAndValue)
            //    throw new Exception("Parameter must contain both name and value");

        }
    }
}
