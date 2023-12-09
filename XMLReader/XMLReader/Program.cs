using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace XMLReader
{
    internal class Program
    {
        static void Main(string[] args)
        {

            string currTempurature = "mts-1-1-Temperature";
            string currWindSpeed = "mts-1-1-windspeedms";
            DateTime now = new DateTime();
            XElement el = new XElement("root");
            float temper = 0;
            float wind = 0;
            try
            {
                using (var reader1 = XmlReader.Create("response.xml"))
                {
                    while (reader1.Read())
                    {


                        //if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "wml2:MeasurementTimeseries")
                        //{
                        if (reader1.HasAttributes)
                        {
                            if (reader1.GetAttribute("gml:id") != null)
                            {
                                string atrr = reader1.GetAttribute("gml:id").ToLower().Trim();
                                if (atrr == currWindSpeed.ToLower().Trim())
                                {
                                    el = (XElement)XNode.ReadFrom(reader1);
                                    IEnumerable<XElement> names =
                                                (from e in el.Elements()
                                                 select e).ToList();
                                    foreach (XElement n in names)
                                    {
                                        string a = n.Value;
                                        a = a.Replace("\n", "").Replace("\t", "").Trim();

                                        now = DateTime.Parse(a.Split(' ')[0]).AddHours(-4);
                                        int ssnow = now.Hour;
                                        if (ssnow == DateTime.Now.Hour && now.Date == DateTime.Now.Date)
                                        {
                                            wind = float.Parse(a.Split('Z')[1].Trim());
                                          Console.WriteLine("the now value is " + now);
                                            var b = a.Split('Z')[1].Trim();
                                            Console.WriteLine("the b value is " + b);
                                        }
                                    }

                                }

                                if (atrr == currTempurature.ToLower().Trim())
                                {
                                    el = (XElement)XNode.ReadFrom(reader1);
                                    IEnumerable<XElement> names =
                                                (from e in el.Elements()
                                                 select e).ToList();
                                    foreach (XElement n in names)
                                    {
                                        string a = n.Value;
                                        a = a.Replace("\n", "").Replace("\t", "").Trim();

                                        now = DateTime.Parse(a.Split(' ')[0]).AddHours(-4);
                                        int ssnow = now.Hour;
                                        if (ssnow == DateTime.Now.Hour && now.Date == DateTime.Now.Date)
                                        {
                                            temper = float.Parse(a.Split('Z')[1].Trim());
                                            Console.WriteLine("the now value is " + now);
                                          Console.WriteLine("the b value is" + a.Split('Z')[1].Trim());
                                        }
                                    }
                                }
                            }
                        }
                        //}



                        //if (reader1.NodeType == XmlNodeType.Element && reader1.Name == "omso:PointTimeSeriesObservation") // Check if the node is an observation element
                        //{
                        //    string id = reader1.GetAttribute("gml:id"); // Get the id attribute
                        //    Console.WriteLine($"Observation id: {id}");
                        //}
                        //if (reader1.NodeType == XmlNodeType.Element && reader1.Name == "gml:TimePeriod") // Check if the node is a time period element
                        //{
                        //    reader1.ReadToDescendant("gml:beginPosition"); // Move to the begin position element
                        //    string begin = reader1.ReadElementContentAsString(); // Read the element content as string
                        //    reader1.ReadToNextSibling("gml:endPosition"); // Move to the end position element
                        //    string end = reader1.ReadElementContentAsString(); // Read the element content as string
                        //    Console.WriteLine($"Phenomenon time: {begin} - {end}");
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            //var filePath = "../response.xml";
            var reader = XmlReader.Create("response.xml"); // Load the XML response from a file or stream
            while (reader.Read()) // Read each node
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "omso:PointTimeSeriesObservation") // Check if the node is an observation element
                {
                    string id = reader.GetAttribute("gml:id"); // Get the id attribute
                    Console.WriteLine($"Observation id: {id}");
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "gml:TimePeriod") // Check if the node is a time period element
                {
                    reader.ReadToDescendant("gml:beginPosition"); // Move to the begin position element
                    string begin = reader.ReadElementContentAsString(); // Read the element content as string
                    reader.ReadToNextSibling("gml:endPosition"); // Move to the end position element
                    string end = reader.ReadElementContentAsString(); // Read the element content as string
                    Console.WriteLine($"Phenomenon time: {begin} - {end}");
                }
            }

            Console.ReadLine();

        }
    }
}
