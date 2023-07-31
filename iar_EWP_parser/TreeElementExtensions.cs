using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iar_EWP_parser
{
    public static class TreeElementExtensions
    {
        //public static TreeElement GetChildByName(this TreeElement element, string name)
        //{
        //    while (element != null)
        //    {
        //        if(element.Name == name)
        //            return element;
        //        if(element.GetType() == typeof(GroupConfig))

        //    }
        //}
        public static List<string> ElementExcludedFrom(this TreeElement element)
        {
            var result = new List<string>();

            while (element != null)
            {
                result.Concat(element.Excluded).Distinct();
                element = element.Parent;
            }

            return result;
        }
        public static List<string> GetProjectsList(this GroupConfig sfGroup)
        {
            if (!sfGroup.Name.Contains("!!!SF_Projects"))
            {
                Console.WriteLine("This method only operates on SF_Projects group.");
                return null;
            }

            return sfGroup.Groups.Select(_ => _.Name).ToList();
        }
    }
}
