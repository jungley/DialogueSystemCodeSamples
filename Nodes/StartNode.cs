using RydenCam.BranchCamEditor.Nodes.Connections;
using RydenCam.Common;
using RydenCam.SequenceData;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.RydenCam.Scripts.BranchCamCC
{
    /// <summary>
    /// Holds the initial starter data of the conversation
    /// </summary>
    [System.Serializable]
    public class StartNode : Node
    {
        public List<ActorInfo> ActorsInScene;

        public bool StartPositionsEnabled;
        public bool OverrideRotation;
        public bool ReturnToOriginalPositions;
        public List<Pose> OriginalPositions;
        public List<Pose> SetStartPositions;

        public string UnitySceneName;
        public string SequenceName;
        public Side CameraSide;

        public override float NodeHeight => 70;

        public StartNode(Vector2 position) : base(position)
        {
            TypeOfNode = NodeType.StartNode;
            ActorsInScene = new List<ActorInfo>();
            PointOut = new List<ConnectionPoint>() { new ConnectionPoint(this, ConnectionPointType.Out) }; 
            
        }
    }
}
