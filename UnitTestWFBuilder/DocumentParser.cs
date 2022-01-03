using DevExpress.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using WFBuilder.Models;

namespace UnitTestWFBuilder
{
    [TestClass]
    public class DocumentParser
    {

        string Document = @"C:\Users\dtzur\source\repos\Workflow\WFBuilder\Documents\Document.xml";
        [TestMethod]
        public void TestMethod1()
        {
            XElement xml = XElement.Load(Document);
            XElement outXml = new XElement("Workflow");

            /////////////////Variables/////////////////////////////////////
            string variables = xml.Element("Items").Element("Item1").Attribute("Variables").Value.ToString();
            var objVariables = DeserializeString(variables);
            outXml.Add(new XElement("Variables"));
            foreach (var item in objVariables as ObservableCollection<VariableModel>)
            {
                string name = (item.LevelScope == LevelScopeType.Local) ? $"@{item.Name}" : $"::{item.Name}";
                XElement var = new XElement("Variable",new XAttribute("Name", name),new XAttribute("Type",item.ValType),new XAttribute("Value",item.Val),new XAttribute("VariableID", item.VariableID));               
                outXml.Element("Variables").Add(var);              
            }

            ////////////Broadcasts///////////////////////////////////////////////
            string broadcasts = xml.Element("Items").Element("Item1").Attribute("BroadcastMessages").Value.ToString();
            var objBroadcasts = DeserializeString(broadcasts);
            outXml.Add(new XElement("Broadcasts"));
            foreach (var item in objBroadcasts as ObservableCollection<BroadcastMessageModel>)
            {
                XElement var = new XElement("Broadcast", new XAttribute("ID", item.BroadcastMessageID), new XAttribute("Name", item.BroadcastMessageName));
                foreach(var target in item.BrodcastTarges)
                {
                    XElement tar = new XElement("Target", new XText($"{target.AdapterID}:{target.PinID}"));
                    var.Add(tar);
                }
                outXml.Element("Broadcasts").Add(var);
            }

            ////////////EntryPoints///////////////////////////////////////////////
            string entryPoints = xml.Element("Items").Element("Item1").Attribute("EntryPoints").Value.ToString();
            var objEntryPoints = DeserializeString(entryPoints);
            outXml.Add(new XElement("EntryPoints"));
            foreach (var item in objEntryPoints as ObservableCollection<EntryPointModel>)
            {
                XElement var = new XElement("EntryPoint", new XAttribute("Name", item.EntryPointName), new XAttribute("ID", item.EntryPointID), new XAttribute("Target", $"{item.AdapterID}:{item.PinID}"));
                outXml.Element("EntryPoints").Add(var);
            }


            ////////////Adapters////////////////////////////////////////////////////////
            outXml.Add(new XElement("Activities"));
            var children = xml.Element("Items").Element("Item1").Element("Children");
            foreach (var item in children.Elements())
            {
                Debug.WriteLine(item.Attribute("ItemKind")?.Value);

            }
            //foreach (var item in )
            //Debug.WriteLine(item.Name);

        }

        public object DeserializeString(string base64String)
        {
            var str = base64String.Replace("~Xtra#Base64", string.Empty);
            return SafeBinaryFormatter.Deserialize(str);
        }

        
    }
}
