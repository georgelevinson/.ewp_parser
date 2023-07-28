using System.Xml;

namespace iar_EWP_parser
{
    internal class Program
    {
        private static readonly string ewp_path = "C:\\Users\\user\\Source\\device1\\Devices1 PRJ_3.11.ewp";
        static void Main(string[] args)
        {
            var proj_obj = new IAR_Project(ewp_path);
            Console.WriteLine("End of execution");
        }
    }

    public class IAR_Project
    {
        public XmlNode Workspace { get; set; }

        public IAR_Project(string ewp_path)
        {
            XmlReader reader = XmlReader.Create(ewp_path);
            XmlDocument eww = new XmlDocument();
            eww.Load(reader);
            reader.Close();

            Workspace = eww.DocumentElement;

            // Find the configuration in args[1] in the eww file
            //XmlNodeList projects = workspace.SelectNodes("project");
        }
    }
}