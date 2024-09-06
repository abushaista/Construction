using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arif.Construction.Domain.Config;

public class DbOptions
{
    public const string SectionName = "DbSettings";
    public string ConnectionString { get; set; } = string.Empty;
}
