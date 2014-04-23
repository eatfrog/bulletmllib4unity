using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;


namespace BulletMLLib
{
	/// <summary>
	/// This is a single node from a BulletML document.
	/// Used as the base node for all the other node types.
	/// </summary>
	public class BulletMLNode
	{

        internal string PatternName { get; set; }
        public string GetPatternName()
        {
            return !String.IsNullOrEmpty(PatternName) ? PatternName : GetPatternName(this);
        }

	    private static string GetPatternName(BulletMLNode n)
	    {
	        while (true)
	        {
	            if (String.IsNullOrEmpty(n.PatternName) && n.Parent != null)
	            {
	                n = n.Parent;
	                continue;
	            }

	            return n.PatternName;
	            break;
	        }
	    }

	    /// <summary>
		/// The XML node name of this item
		/// </summary>
		public NodeName Name { get; private set; }

		/// <summary>
		/// The type modifier of this node... like is it a sequence, or whatever
		/// </summary>
		private NodeType _nodeType = NodeType.None;


		public virtual NodeType NodeType 
		{ 
			get
			{
				return _nodeType;
			}
			protected set
			{
				_nodeType = value;
			}
		}

		/// <summary>
		/// The label of this node
		/// This can be used by other nodes to reference this node
		/// </summary>
		public string Label { get; protected set; }

		/// <summary>
		/// An equation used to get a value of this node.
		/// </summary>
		/// <value>The node value.</value>
		private readonly BulletMLEquation _nodeEquation = new BulletMLEquation();

		/// <summary>
		/// A list of all the child nodes for this dude
		/// </summary>
		public List<BulletMLNode> ChildNodes { get; private set; }

		/// <summary>
		/// pointer to the parent node of this dude
		/// </summary>
		public BulletMLNode Parent { get; set; }


	    /// <summary>
		/// Initializes a new instance of the <see cref="BulletMLLib.BulletMLNode"/> class.
		/// </summary>
		public BulletMLNode(NodeName nodeType)
		{
			ChildNodes = new List<BulletMLNode>();
			Name = nodeType;
			NodeType = NodeType.None;
		}

		/// <summary>
		/// Convert a string to it's NodeType enum equivalent
		/// </summary>
		/// <returns>NodeType: the nuem value of that string</returns>
		/// <param name="str">The string to convert to an enum</param>
		public static NodeType StringToType(string str)
		{			
			if (String.IsNullOrEmpty(str))		
				return NodeType.None;			

			return (NodeType)Enum.Parse(typeof(NodeType), str, true);

		}
		
		/// <summary>
		/// Convert a string to it's NodeName enum equivalent
		/// </summary>
		/// <returns>NodeName: the nuem value of that string</returns>
		/// <param name="str">The string to convert to an enum</param>
		public static NodeName StringToName(string str)
		{
			return (NodeName)Enum.Parse(typeof(NodeName), str, true);
		}

		/// <summary>
		/// Gets the root node.
		/// </summary>
		/// <returns>The root node.</returns>
		public BulletMLNode GetRootNode()
		{
		    //recurse up until we get to the root node
		    return Parent != null ? Parent.GetRootNode() : this;

		}

	    /// <summary>
		/// Find a node of a specific type and label
		/// Recurse into the xml tree until we find it!
		/// </summary>
		/// <returns>The label node.</returns>
		/// <param name="label">Label of the node we are looking for</param>
		/// <param name="name">name of the node we are looking for</param>
		public BulletMLNode FindLabelNode(string strLabel, NodeName name)
		{
			//this uses breadth first search, since labelled nodes are usually top level

			//Check if any of our child nodes match the request
			for (int i = 0; i < ChildNodes.Count; i++)
			{
				if ((name == ChildNodes[i].Name) && (strLabel == ChildNodes[i].Label))
				{
					return ChildNodes[i];
				}
			}

			//recurse into the child nodes and see if we find any matches
			for (int i = 0; i < ChildNodes.Count; i++)
			{
				BulletMLNode foundNode = ChildNodes[i].FindLabelNode(strLabel, name);
				if (null != foundNode)
				{
					return foundNode;
				}
			}

			//didnt find a BulletMLNode with that name :(
			return null;
		}

		/// <summary>
		/// Find a parent node of the specified node type
		/// </summary>
		/// <returns>The first parent node of that type, null if none found</returns>
		/// <param name="nodeType">Node type to find.</param>
		public BulletMLNode FindParentNode(NodeName nodeType)
		{
		    //first check if we have a parent node
			if (null == Parent) return null;
		    return nodeType == Parent.Name ? Parent : Parent.FindParentNode(nodeType);
		}

	    /// <summary>
		/// Gets the value of a specific type of child node for a task
		/// </summary>
		/// <returns>The child value. return 0.0 if no node found</returns>
		/// <param name="name">type of child node we want.</param>
		/// <param name="task">Task to get a value for</param>
		public float GetChildValue(NodeName name, BulletMLTask task)
		{
		    return (from tree in ChildNodes where tree.Name == name select tree.GetValue(task)).FirstOrDefault();
		}

	    /// <summary>
		/// Get a direct child node of a specific type.  Does not recurse!
		/// </summary>
		/// <returns>The child.</returns>
		/// <param name="name">type of node we want. null if not found</param>
		public BulletMLNode GetChild(NodeName name)
		{
		    return ChildNodes.FirstOrDefault(node => node.Name == name);
		}

	    /// <summary>
		/// Gets the value of this node for a specific instance of a task.
		/// </summary>
		/// <returns>The value.</returns>
		/// <param name="task">Task.</param>
		public float GetValue(BulletMLTask task)
		{
			//send to the equation for an answer
			return _nodeEquation.Solve(task.GetParamValue);
		}

	    /// <summary>
	    /// Parse the specified bulletNodeElement.
	    /// Read all the data from the xml node into this dude.
	    /// </summary>
	    /// <param name="bulletNodeElement">Bullet node element.</param>
	    /// <param name="parentNode"></param>
	    public void Parse(XmlNode bulletNodeElement, BulletMLNode parentNode)
		{
			// Handle null argument.
			if (null == bulletNodeElement)
			{
				throw new ArgumentNullException("bulletNodeElement");
			}

			//grab the parent node
			Parent = parentNode;

			//Parse all our attributes
			XmlNamedNodeMap mapAttributes = bulletNodeElement.Attributes;

			for (int i = 0; i < mapAttributes.Count; i++)
			{
				string name = mapAttributes.Item(i).Name;
				string value = mapAttributes.Item(i).Value;

                if (name == "type" && Name == NodeName.Bulletml) continue;

                switch (name)
                {
                    case "type":
                        NodeType = StringToType(value);
                        break;
                    case "label":
                        Label = value; //label is just a text value
                        break;
                    case "name":
                        PatternName = value;
                        break;
                }

			}

			//parse all the child nodes
			if (bulletNodeElement.HasChildNodes)
			{
				for (XmlNode childNode = bulletNodeElement.FirstChild;
				     null != childNode;
				     childNode = childNode.NextSibling)
				{
					//if the child node is a text node, parse it into this dude
					if (XmlNodeType.Text == childNode.NodeType)
					{
						//Get the text of the child xml node, but store it in THIS bullet node
						_nodeEquation.Parse(childNode.Value);
						continue;
					}
				    if (XmlNodeType.Comment == childNode.NodeType)
				    {
				        //skip any comments in the bulletml script
				        continue;
				    }

				    //create a new node
					BulletMLNode childBulletNode = NodeFactory.CreateNode(StringToName(childNode.Name));

					//read in the node and store it
					childBulletNode.Parse(childNode, this);
					ChildNodes.Add(childBulletNode);
				}
			}
		}

		/// <summary>
		/// Validates the node.
		/// Overloaded in child classes to validate that each type of node follows the correct business logic.
		/// This checks stuff that isn't validated by the XML validation
		/// </summary>
		public virtual void ValidateNode()
		{
			//validate all the childe nodes
			foreach (BulletMLNode childnode in ChildNodes)
			{
				childnode.ValidateNode();
			}
		}
	}
}
