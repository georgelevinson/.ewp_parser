using System.Xml;
using System.Linq;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace iar_EWP_parser
{
    internal class Program
    {
        private static readonly string ewp_path = "C:\\Users\\user\\Source\\device1\\Devices1 PRJ_3.11.ewp";
        static void Main(string[] args)
        {
            var proj_obj = ReadXmlAndDeserializeGroups(ewp_path);

            List<FileConfig> tempModules = proj_obj.Groups
                .Where(_ => _.Name == "Devices_Modules")
                .Single().Groups
                .Where(_ => _.Name == "Tempearture")
                .Single().Files.ToList();

            tempModules.AnalyzeModulesInclusion(proj_obj);


            Console.WriteLine("\n\r\n\rEnd of execution");
        }
        private static GroupConfig ReadXmlAndDeserializeGroups(string diskAddr)
        {
            try
            {
                XmlReader reader = XmlReader.Create(diskAddr);
                XmlDocument eww = new XmlDocument();

                eww.Load(reader);
                reader.Close();

                var root = eww.DocumentElement;

                return root.DeserializeXml_GroupConfig(new GroupConfig());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("\n\rException caught, exiting DeserializeXmlBranch(), null returned to the caller.\n\r");

                return null;
            }
        }
    }
}