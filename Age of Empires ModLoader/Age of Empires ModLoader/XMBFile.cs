using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace Age_of_Empires_ModLoader
{

    public struct XmlElement
    {
        public int Index;
        public int Line;
        public string Content;
        public int NumParentElements;
        public List<XmlParameter> Parameters;
        public List<XmlElement> CildElements;
    }

    public struct XmlParameter
    {
        public int Index;
        public string Content;
    }

    public class XmbFile
    {
        byte[] fileData;
        public XmbFile(byte[] data)
        {
            fileData = data;
        }

        public string Read()
        {
            //Lists of elementnames and parameternames
            List<string> elements = new List<string>();
            List<string> parameters = new List<string>();

            MemoryStream mStream = new MemoryStream(fileData);//Read stream
            mStream.Skip(2);//X1
            int documentLength = BitConverter.ToInt32(mStream.ReadBytes(4), 0);
            mStream.Skip(10);//Encoding

            int numberOfElementNames = BitConverter.ToInt32(mStream.ReadBytes(4), 0);//number of different elements
            for (int i = 0; i < numberOfElementNames; i++)
            {
                int elementNameLenght = BitConverter.ToInt32(mStream.ReadBytes(4), 0);
                elements.Add(ReadString(mStream.ReadBytes(elementNameLenght * 2)));
            }


            int numberOfParameterNames = BitConverter.ToInt32(mStream.ReadBytes(4), 0);//number Of different parameters
            for (int i = 0; i < numberOfParameterNames; i++)
            {
                int parameterNameLenght = BitConverter.ToInt32(mStream.ReadBytes(4), 0);//parameterNameLenght byte
                parameters.Add(ReadString(mStream.ReadBytes(parameterNameLenght * 2)));
            }

            XmlElement element = GetElement(ref mStream, 0);

            //prepare file text
            string returnString = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n";
            int currentLine = 2;

            //read element data
            returnString += ReadElement(element, ref elements, ref parameters, ref currentLine);

            return returnString;
        }

        string ReadString(byte[] stringData)
        {
            string returnString = "";
            for (int chr = 0; chr < stringData.Length; chr++)
            {
                Char c = (Char)stringData[chr];
                if (c != '\0')
                    returnString += c;
            }
            return returnString;
        }

        XmlElement GetElement(ref MemoryStream mStream, int numParentElements)
        {
            //prepare element to return
            XmlElement returnElement = new XmlElement();
            returnElement.CildElements = new List<XmlElement>();
            returnElement.Parameters = new List<XmlParameter>();
            returnElement.NumParentElements = numParentElements;

            //
            mStream.Skip(6);
            int elementContentNameLenght = BitConverter.ToInt32(mStream.ReadBytes(4), 0);//condent text Lenght
            returnElement.Content = ReadString(mStream.ReadBytes(elementContentNameLenght * 2));//Content text
            returnElement.Index = BitConverter.ToInt32(mStream.ReadBytes(4), 0);//ElementIndex
            returnElement.Line = BitConverter.ToInt32(mStream.ReadBytes(4), 0);//line in xml file
            int numParameters = BitConverter.ToInt32(mStream.ReadBytes(4), 0);//Number of parameters
            //Add parameters
            for (int i = 0; i < numParameters; i++)
            {
                XmlParameter parameter = new XmlParameter();
                parameter.Index = BitConverter.ToInt32(mStream.ReadBytes(4), 0);
                int parameterContentNameLenght = BitConverter.ToInt32(mStream.ReadBytes(4), 0);
                parameter.Content = ReadString(mStream.ReadBytes(parameterContentNameLenght * 2));
                returnElement.Parameters.Add(parameter);
            }
            //Get the child elements
            int numberOfCildElements = BitConverter.ToInt32(mStream.ReadBytes(4), 0);
            for (int i = 0; i < numberOfCildElements; i++)
            {
                returnElement.CildElements.Add(GetElement(ref mStream, returnElement.NumParentElements + 1));
            }
            return returnElement;
        }

        string ReadElement(XmlElement element, ref List<string> elements, ref List<string> parameters, ref int currentLine)
        {
            //prepare return string
            string returnString = "";
            //add number of line as in original file
            for (int i = 0; i < element.Line - currentLine; i++)
            {
                returnString += "\n";

            }
            currentLine += element.Line - currentLine;//keep track of ref currentline

            //add xml tabs for readability
            for (int i = 0; i < element.NumParentElements; i++)
            {
                returnString += "   ";
            }
            //Open element
            returnString += "<" + elements[element.Index];

            //ad parameters to element
            foreach (XmlParameter parameter in element.Parameters)
            {
                returnString += " " + parameters[parameter.Index] + "=\"" + parameter.Content + "\"";
            }

            //read all cild elements and close parent element
            if (element.CildElements.Count > 0)
            {
                returnString += ">\n";
                currentLine++;

                if (element.Content.Length > 0)//Add content if exist
                {
                    //add xml tabs for readability
                    for (int i = 0; i < (element.NumParentElements + 1); i++)
                    {
                        returnString += "   ";
                    }

                    returnString += element.Content + "\n";
                    currentLine++;
                }

                foreach (XmlElement el in element.CildElements)
                {
                    returnString += ReadElement(el, ref elements, ref parameters, ref currentLine);
                }
                for (int i = 0; i < element.NumParentElements; i++)
                {
                    returnString += "   ";
                }
                returnString += "</" + elements[element.Index] + ">\n";
                currentLine++;
            }
            else
            {
                if (element.Content.Length > 0)//Add content if exist
                {
                    returnString += ">" + element.Content + "</" + elements[element.Index] + ">\n";
                    currentLine++;
                }
                else
                {
                    returnString += "/>\n";
                    currentLine++;
                }
            }

            return returnString;
        }
    }
}
