using Lavalink4NET.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duck_Bot_.Net_Core.Core.Commands
{
    public class Addons
    {
        public string _out(params String[] input)
        {
            string output = null;
            for (int i = 0; i < input.Length; i++)
            {
                output += input.ElementAt(i);
                if (i != input.Length - 1)
                {
                    output += " ";
                }
            }
            return output;
        }

        public string _outRoles(params String[] input)
        {
            string output = ",";
            for (int i = 0; i < input.Length; i++)
            {
                output += ",";
                output += input.ElementAt(i);
                if (i != input.Length - 1)
                {
                    output += ",";
                }
            }
            return output;
        }
    }
}
