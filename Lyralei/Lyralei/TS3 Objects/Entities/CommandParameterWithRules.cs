using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TS3QueryLib.Core.CommandHandling;

namespace Lyralei.TS3_Objects.Entities
{
    public enum NameValueSetting
    {
        NameOnly,
        NameOrNameAndValue,
        ValueOnly,
        ValueOrValueAndName,
        NameAndValue,
    }

    public enum ValueType
    {
        String,
        Integer,
        Float,
        Boolean,
        Regex,
    }

    public class CommandParameterWithRules : CommandParameter
    {
        public NameValueSetting NameValueSetting = NameValueSetting.NameOrNameAndValue;
        public ValueType ValueType = ValueType.String;
        public bool Nullable = false;
        public bool Required = false;
        public bool IsBaseCommand = false;
        public string DefaultValue;
        public string Help;
        public string RegexPattern = ".*";

        public CommandParameterWithRules(string name, bool IsBaseCommand) : base(name)
        {
            this.IsBaseCommand = IsBaseCommand;
        }

        public CommandParameterWithRules(string name) : base(name)
        {

        }

        public CommandParameterWithRules(string name, string value) : base(name, value)
        {

        }

        public void ValidateSetData(CommandParameter cmdP)
        {
            try
            {
                ValidateData(cmdP);
            }
            catch (Exception)
            {

            }

            //Made it this far? Set the value
            if (this.ValueType == ValueType.Regex)
                this.Value = Regex.Match(cmdP.Value, this.RegexPattern).Value;
            else
                this.Value = cmdP.Value;
        }

        public void ValidateData(CommandParameter cmdP)
        {
            if (this.IsBaseCommand)
                throw new Exception("Cannot set value on base command!");

            if (this.NameValueSetting == NameValueSetting.NameOnly)
                throw new Exception("Cannot set value on this parameter!");

            //Value type checking
            if (this.ValueType == ValueType.String)
                Convert.ToString(cmdP.Value);
            else if (this.ValueType == ValueType.Boolean)
                Convert.ToBoolean(cmdP.Value);
            else if (this.ValueType == ValueType.Float)
                Convert.ToDouble(cmdP.Value);
            else if (this.ValueType == ValueType.Integer)
                Convert.ToInt64(cmdP.Value);
            else if (this.ValueType == ValueType.Regex && Regex.IsMatch(Value, RegexPattern) == false)
                throw new Exception("Value did not match type");
        }
    }
}
