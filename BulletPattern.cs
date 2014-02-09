using System;
using System.Diagnostics;
using System.Xml;
using System.Xml.Schema;
using System.IO;
using UnityEngine;
using System.Collections;

namespace BulletMLLib
{
	/// <summary>
	/// This is a complete document that describes a bullet pattern.
	/// </summary>
	public class BulletPattern
	{
		#region Members

		/// <summary>
		/// The root node of a tree structure that describes the bullet pattern
		/// </summary>
		public BulletMLNode RootNode { get; private set; }

		//TODO: move filename class to github and use it here

		/// <summary>
		/// Gets the filename.
		/// This property is only set by calling the parse method
		/// </summary>
		/// <value>The filename.</value>
		public string Filename { get; private set; }

		/// <summary>
		/// the orientation of this bullet pattern: horizontal or veritcal
		/// this is read in from the xml
		/// </summary>
		/// <value>The orientation.</value>
		public EPatternType Orientation { get; private set; }

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="BulletMLLib.BulletPattern"/> class.
		/// </summary>
		public BulletPattern()
		{
			RootNode = null;
		}

		/// <summary>
		/// convert a string to a pattern type enum
		/// </summary>
		/// <returns>The type to name.</returns>
		/// <param name="str">String.</param>
		private static EPatternType StringToPatternType(string str)
		{
			return (EPatternType)Enum.Parse(typeof(EPatternType), str);
		}

		/// <summary>
		/// Parses a bulletml document into this bullet pattern
		/// </summary>
		/// <param name="xmlFilePath">Xml file name.</param>
        public void ParseXML(XmlReader reader, Action callback)
		{




			try
			{
				//Open the file.
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.Load(reader);
				XmlNode rootXmlNode = xmlDoc.DocumentElement;
				
				//make sure it is actually an xml node
				if (rootXmlNode.NodeType == XmlNodeType.Element)
				{
					//eat up the name of that xml node
					string strElementName = rootXmlNode.Name;
					if ("bulletml" != strElementName)
					{
						//The first node HAS to be bulletml
                        UnityEngine.Debug.Log("Error reading \"" + "\": XML root node needs to be \"bulletml\", found \"" + strElementName + "\" instead");
						throw new Exception("Error reading \"" + "\": XML root node needs to be \"bulletml\", found \"" + strElementName + "\" instead"); 
					}

					//Create the root node of the bulletml tree
					RootNode = new BulletMLNode(ENodeName.bulletml);

					//Read in the whole bulletml tree
					RootNode.Parse(rootXmlNode, null);

					//Find what kind of pattern this is: horizontal or vertical
					XmlNamedNodeMap mapAttributes = rootXmlNode.Attributes;
					for (int i = 0; i < mapAttributes.Count; i++)
					{
						//will only have the name attribute
						string strName = mapAttributes.Item(i).Name;
						string strValue = mapAttributes.Item(i).Value;
						if ("type" == strName)
						{
							//if  this is a top level node, "type" will be veritcal or horizontal
							Orientation = StringToPatternType(strValue);
						}
					}
				}
			}
			catch (Exception ex)
			{
				//an error ocurred reading in the tree
                UnityEngine.Debug.Log("Error reading." + ex.Message);
				throw new Exception("Error reading \"" + "\"", ex);
			}

            reader.Close();

			//grab that filename 
			//Filename = xmlFilePath;

			//validate that the bullet nodes are all valid
			try
			{
				RootNode.ValidateNode();
			}
			catch (Exception ex)
			{
				//an error ocurred reading in the tree
                UnityEngine.Debug.Log("Error reading \"" +  "\"" + ex.Message);
				throw new Exception("Error reading \"" + "\"", ex);
			}
            UnityEngine.Debug.Log("Loading done.");
            callback();
		}

		/// <summary>
		/// delegate method that gets called when a validation error occurs
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="args">Arguments.</param>
		public static void MyValidationEventHandler(object sender, ValidationEventArgs args)
		{
			throw new XmlSchemaException("Error validating bulletml document: " + args.Message, 
			                             args.Exception, 
			                             args.Exception.LineNumber,
			                             args.Exception.LinePosition);
		}
		#endregion //Methods
	}
}
