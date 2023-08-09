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

            // find all the projects folders
            var groups = sfGroup.Groups.Select(_ => _.Name).ToList();

            // from those find such that contain different compilation groups and select those nested groups (e.g. StreetSiren/StreetSiren_RFM301, StreetSiren/StreetSiren_RFM66, etc.)
            var nested = sfGroup.Groups.SelectMany(_ => _.Groups.Select(_ => _.Name)).Where(_ => groups.Any(g => _.Contains(g))).ToList();

            // from proj folders find and remove those that contain multiple subproj (e.g. remove StreetSiren, only leave StreetSiren_RFM66, StreetSiren_RFM301 etc.)
            var parents = groups.Where(g => nested.Any(n => n.Contains(g))).ToList();
            parents.ForEach(p => groups.Remove(p));

            // merge the two to obtain same list of compilations as the drop-down in IAR
            groups.AddRange(nested);

            return groups;
        }
        public static List<string> IsIncludedInProjects(this TreeElement element, IEnumerable<string> allProjects)
        {
            return allProjects.Except(element.Excluded).ToList();
        }

        // this method operates on a given set of FileConfigs assumed to be different implementations of a single module
        // it outputs inclusion analysis to console output
        public static void AnalyzeModulesInclusion(this IEnumerable<FileConfig> fileConfigs, GroupConfig root)
        {
            List<string> allProjects = root.Groups
                .Where(_ => _.Name == "!!!SF_Projects")
                .Single()
                .GetProjectsList();

            List<string> noImplementations = new List<string>(allProjects);
            List<List<string>> fileIncludedIn = new List<List<string>>();

            foreach (var fileConfig in fileConfigs)
            {
                var included = fileConfig.IsIncludedInProjects(allProjects);
                fileIncludedIn.Add(included);
                noImplementations = noImplementations.Except(included).ToList();
                Console.WriteLine("\n\r\n\rModule name: " + fileConfig.Name + "\n\r" + string.Join("\n\r", included));
            }

            var projectsWithMultipleImplementations = fileIncludedIn
                .SelectMany(_ => _)
                .GroupBy(_ => _)
                .Where(_ => _.Count() > 1)
                .Select(_ => _.Key)
                .ToList();

            Console.WriteLine("\n\r\n\rThe following projects include multiple implementations: " + "\n\r" + string.Join("\n\r", projectsWithMultipleImplementations));
            Console.WriteLine("\n\r\n\rThe following projects include no implementations: " + "\n\r" + string.Join("\n\r", noImplementations));
        }
    }
}
