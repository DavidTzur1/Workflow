using DevExpress.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using WFBuilder.Models;
using WFEngine.SDK;

namespace UnitTestWFBuilder
{
    [TestClass]
    public class DocumentParser
    {
        enum Colors
        {
            Red,
            Orange,
            Green,
            Blue,
            Black
        }

        Colors color = Colors.Black;
        string Document = @"C:\Users\dtzur\source\repos\Workflow\WFBuilder\Documents\Document.xml";
        [TestMethod]
        public void TestCreateWorkflow()
        {
            object date = DateTime.Now;
            Debug.WriteLine(date.ToString());
            string str = Enum.GetName(typeof(Colors), color);
            string path = @"D:\WFData\Documents\Test\LogAdapter.xml";
            //string path = @"D:\WFData\Documents\Test\CreateSetEscData.xml";
            var xml = Workflow.Create(path);
        }
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


            ////////////Adapters and Connections//////////////////////////////////////////////
            outXml.Add(new XElement("Activities"));
            outXml.Add(new XElement("Connections"));
            var children = xml.Element("Items").Element("Item1").Element("Children");
            foreach (var item in children.Elements())
            {
                if(item.Attribute("Tag")?.Value == "Adapter")
                {
                    XElement adapter = new XElement("Adapter",new XAttribute("ID",item.Attribute("AdapterID").Value),new XAttribute("Name",item.Attribute("Header").Value),new XAttribute("Type",item.Attribute("ItemKind").Value));
                    adapter.Add(new XElement("Properties"));
                    adapter.Element("Properties").Add(GetProperties(item));
                    adapter.Element("Properties").Add(GetPins(item.Element("Children")));

                    outXml.Element("Activities").Add(adapter);
                }
                else if (item.Attribute("ItemKind")?.Value == "DiagramConnector")
                {
                    outXml.Element("Connections").Add(GetConnection(item,children));
                }

            }


        }

        public List<XAttribute> GetProperties(XElement root)
        {
            List<XAttribute>  properties = new List<XAttribute>();
            foreach (var item in root.Attributes())
            {
                if (item.Name.ToString().StartsWith("_"))
                {
                    
                    properties.Add(new XAttribute(item.Name.ToString().TrimStart('_'), item.Value));
                }
            }
           
            return properties;
        }

        public XElement GetPins(XElement panels)
        {
            XElement pins = new XElement("Pins");
            foreach (var panel in panels.Elements())
            {
                if (panel.Attribute("Tag")?.Value == "InputPanel")
                {
                    XElement input = new XElement("In");
                    foreach (var item in panel.Element("Children").Elements())
                    {
                        XElement pin = new XElement("Pin");
                        foreach(var container in item.Elements())
                        {
                            foreach(var tag in container.Elements())
                            {
                                if(tag.Attribute("Tag").Value == "label")
                                {
                                    pin.Add(new XAttribute("Name", tag.Attribute("Content").Value));
                                }
                                else if (tag.Attribute("Tag").Value == "line")
                                {
                                    pin.Add(new XAttribute("ID", tag.Attribute("Content").Value));
                                }
                            }
                            input.Add(pin);
                        }

                    }
                    pins.Add(input);


                }
                else if (panel.Attribute("Tag")?.Value == "OutputPanel")
                {
                    XElement output = new XElement("Out");
                    foreach (var item in panel.Element("Children").Elements())
                    {
                        XElement pin = new XElement("Pin");
                        foreach (var container in item.Elements())
                        {
                            foreach (var tag in container.Elements())
                            {
                                if (tag.Attribute("Tag").Value == "label")
                                {
                                    pin.Add(new XAttribute("Name", tag.Attribute("Content").Value));
                                }
                                else if (tag.Attribute("Tag").Value == "line")
                                {
                                    pin.Add(new XAttribute("ID", tag.Attribute("Content").Value));
                                }
                            }
                            output.Add(pin);
                        }

                    }
                    pins.Add(output);

                }

            }
            return pins;
        }

        public XElement GetConnection(XElement item , XElement children)
        {
            XElement conn = new XElement("Connection");
            var numbers = item.Attribute("BeginItem").Value.Split(',').Select(Int32.Parse).ToList().Select(x=> x+1).ToList();
            string sourceAdapterID = children.Element($"Item{numbers[0]}").Attribute("AdapterID").Value;
            
            string sourcePinID = (children.Element($"Item{numbers[0]}").Element("Children").Element($"Item{numbers[1]}")
                .Element("Children").Element($"Item{numbers[2]}").Element("Children").Elements().Where(x => x.Attribute("Tag").Value == "line").FirstOrDefault() as XElement).Attribute("Content").Value; 

            numbers = item.Attribute("EndItem").Value.Split(',').Select(Int32.Parse).ToList().Select(x => x + 1).ToList();
            string destAdapterID = children.Element($"Item{numbers[0]}").Attribute("AdapterID").Value;
            
            string destPinID = (children.Element($"Item{numbers[0]}").Element("Children").Element($"Item{numbers[1]}")
                .Element("Children").Element($"Item{numbers[2]}").Element("Children").Elements().Where(x => x.Attribute("Tag").Value == "line").FirstOrDefault() as XElement).Attribute("Content").Value;

            conn.Add(new XAttribute("Source", $"{sourceAdapterID}:{sourcePinID}"), new XAttribute("Destination", $"{destAdapterID}:{destPinID}"));

            return conn;
        }



        public object DeserializeString(string base64String)
        {
            var str = base64String.Replace("~Xtra#Base64", string.Empty);
            return SafeBinaryFormatter.Deserialize(str);
        }

        
    }
}
