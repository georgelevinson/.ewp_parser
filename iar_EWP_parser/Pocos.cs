using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iar_EWP_parser
{
    public class FileConfig
    {
        public string Name { get; set; }

        public List<string> Excluded { get; set; }
    }

    public class GroupConfig
    {
        public string Name { get; set; }

        public List<FileConfig> Files { get; set; }

        public List<GroupConfig> Groups { get; set; }

        public List<string> Excluded { get; set; }
    }
}
