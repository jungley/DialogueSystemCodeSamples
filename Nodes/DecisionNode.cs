using RydenCam.BranchCamEditor.Nodes.Connections;
using RydenCam.Common;
using RydenCam.SequenceData;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.RydenCam.Scripts.BranchCamCC
{
    /// <summary>
    /// Decision Node for branching dialogue
    /// Holds multiple choices via DecisionOptions
    /// </summary>
    [System.Serializable]
    public class DecisionNode : Node, ITalkable
    {

        public ConversationData NodeConvodataField;

        public ConversationData NodeConvodata 
        { 
            get { return NodeConvodataField; } 
            set {  NodeConvodataField = value; }
        }

        public List<string> DecisionOptions;

        public bool ShowPreviousDialog;

        public Node MakeDecision(int choiceIndex)
        {
            return PointOut[choiceIndex].ConnectedTo?.Node;
        }

        public DecisionNode(Vector2 position) : base(position)
        {
            TypeOfNode = NodeType.DecisionNode;
            DecisionOptions = new List<string>() { "" };
            PointIn = new ConnectionPoint(this, ConnectionPointType.In);
            PointOut = new List<ConnectionPoint>() { new ConnectionPoint(this, ConnectionPointType.Out) };
            NodeConvodata = new ConversationData();
        }
    }
}
