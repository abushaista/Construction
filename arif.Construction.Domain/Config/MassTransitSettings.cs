using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arif.Construction.Domain.Config
{
    public class MassTransitSettings
    {
        public LogLevel? LogLevel
        {
            get
            {
                if (Enum.TryParse(Level, true, out LogLevel level))
                {
                    return level;
                }

                return null;
            }
        }

        public string Level { get; set; }
        public int Frequency { get; set; }
        public List<MessageType> MessageTypes { get; set; }
    }

    public class MessageType
    {
        public LogLevel? LogLevel
        {
            get
            {
                if (Enum.TryParse(Level, true, out LogLevel level))
                {
                    return level;
                }

                return null;
            }
        }

        public string Level { get; set; }
        public int? Frequency { get; set; }
        public string Type { get; set; }
    }
}
