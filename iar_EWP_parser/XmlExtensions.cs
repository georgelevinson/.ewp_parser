using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

#pragma warning disable CS8604 // Possible null reference argument.

namespace iar_EWP_parser
{
    public static class XmlExtensions
    {
        public static List<XmlElement> ChildNodesFilterAndReturn(this XmlElement root, string name)
        {
            List<XmlElement> match = new List<XmlElement>();

            if (root == null) return match;

            foreach (XmlElement node in root.ChildNodes)
            {
                if (node.Name == name)
                {
                    match.Add(node);
                }
            }

            return match;
        }

        public static List<FileConfig> DeserializeXml_FileConfigs(this XmlElement root, GroupConfig parentGroup)
        {
            List<XmlElement> fileConfigsXml = root.ChildNodesFilterAndReturn("file");

            return fileConfigsXml.Select(_ => new FileConfig
            {
                Name = _.FirstChild?.Name == "name" ? _.FirstChild.InnerText : "none",
                Parent = parentGroup,
                Excluded = _.ChildNodesFilterAndReturn("excluded")
                            .SingleOrDefault()
                            .ChildNodesFilterAndReturn("configuration")
                            .Select(_ => _.InnerText)
                            .ToList()
            }).ToList();
        }

        public static GroupConfig DeserializeXml_GroupConfig(this XmlElement root, GroupConfig parentGroup)
        {
            parentGroup.Name = root?.FirstChild?.Name == "name" ? root.FirstChild.InnerText : "none";
            parentGroup.Files = root.DeserializeXml_FileConfigs(parentGroup);
            parentGroup.Groups = root.ChildNodesFilterAndReturn("group")
                                     .Select(_ => _.DeserializeXml_GroupConfig(new GroupConfig() { Parent = parentGroup }))
                                     .ToList();
            parentGroup.Excluded = root.ChildNodesFilterAndReturn("excluded")
                                       .SingleOrDefault()
                                       .ChildNodesFilterAndReturn("configuration")
                                       .Select(_ => _.InnerText)
                                       .ToList();

            return parentGroup;
        }
    }
}
#pragma warning restore CS8604 // Possible null reference argument.