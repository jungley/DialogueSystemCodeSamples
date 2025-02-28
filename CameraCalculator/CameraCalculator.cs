﻿using Assets.RydenCam.Scripts.BranchCamEditor.Camera;
using Cinemachine.Utility;
using RydenCam.BranchCamEditor.Controllers;
using RydenCam.BranchCamEditor.Extensions;
using RydenCam.BranchCamEditor.Managers;
using RydenCam.Common;
using RydenCam.SequenceData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RydenCam.BranchCamEditor.BranchCam
{
    /// <summary>
    /// Calculate the camera placement for a given shot.
    /// </summary>
    public class CameraCalculator
    {

        public CameraSettings CamSettings { get; set; }

        //TODO: Remove GameObject dependency
        List<Transform> ActorsInScene
        {
            get
            {
                return NodeManager.Instance.ActorsInScene
                   .Where(item => item.ActorGO != null)
                   .Select(item => item.ActorGO.transform)
                   .ToList();
            }
        }

        public CameraCalculator()
        {
            CamSettings = new CameraSettings();
        }

        /// <summary>
        /// Calculate the camera placement for a given shot.
        /// </summary>
        /// <param name="shot"></param>
        /// <param name="actorPositionData"></param>
        /// <returns></returns>
        public Pose CalculatePlacement(CamShotConfig shot, PreviewActorPositionData actorPositionData)
        {
            switch (shot.GoalType)
            {
                case CameraGoal.Portrait:
                    return CalculatePortrait(shot, actorPositionData);

                case CameraGoal.OverShoulder:
                    return CalculateOverShoulder(shot, actorPositionData); 

                case CameraGoal.FrameShare:
                    return CalculateFrameShare(shot, actorPositionData);

                case CameraGoal.Custom:
                    return CalculateCustom(shot);

                default:
                    return new Pose();
             }
        }

        /// <summary>
        /// Calculate the camera position based on the custom camera settings.
        /// TODO: Fix global and local
        /// </summary>
        /// <param name="shot"></param>
        /// <returns></returns>
        private Pose CalculateCustom(CamShotConfig shot)
        {
            if (!shot.IsCustomSet) return new Pose();

            if (shot.GoalCustomType == CustomCameraType.Local)
            {
                GameObject target = GameObject.Find(shot.actor);
                Vector3 pos_result = target.transform.position - shot.LocalRelativeActorPos;
                Vector3 localCamPos = (shot.GlobalCustomCamPos + pos_result);
                return new Pose(localCamPos, shot.GlobalCustomCamRot);
            }

            if(shot.GoalCustomType == CustomCameraType.Global)
            {
                return new Pose(shot.GlobalCustomCamPos, shot.GlobalCustomCamRot);
            }

            return new Pose();
        }

        /// <summary>
        /// Calculate the orbit position around a target.
        /// </summary>
        /// <param name="targetPos"></param>
        /// <param name="camPos"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        private Vector3 CalculateOrbitPosition(Vector3 targetPos, Vector3 camPos, float angle)
        {
            Vector3 direction = camPos - targetPos;
            float distance = direction.magnitude;
            direction.Normalize();

            // Rotation matrix for Y-axis rotation
            float cosAngle = Mathf.Cos(angle * Mathf.Deg2Rad);
            float sinAngle = Mathf.Sin(angle * Mathf.Deg2Rad);

            // Rotate the direction vector
            Vector3 rotatedDirection = new Vector3(
                cosAngle * direction.x - sinAngle * direction.z,
                direction.y,
                sinAngle * direction.x + cosAngle * direction.z
            );

            return targetPos + rotatedDirection * distance;
        }

        /// <summary>
        /// Calculate the camera position for a portrait shot.
        /// </summary>
        /// <param name="shot"></param>
        /// <param name="posData"></param>
        /// <returns></returns>
        private Pose CalculatePortrait(CamShotConfig shot, PreviewActorPositionData posData)
        {
            // Retrieve shot parameters
            Vector3 targPos = posData.ActorPosition;
            Vector3 forwardN = posData.ForwardN;
            float distance = CamSettings.GetDistance(shot);
            float angleHeight = CamSettings.GetAngle(shot);
            float biasX = CamSettings.DefaultBiasX;
            float orbitAngle = CamSettings.DefaultOrbitAngle;

            // Calculate initial camera position
            Vector3 camPos = targPos + forwardN * distance;
            camPos.y += angleHeight;

            // Compute two possible camera positions based on orbit angle
            Vector3 option1 = CalculateOrbitPosition(targPos, camPos, orbitAngle);
            Vector3 option2 = CalculateOrbitPosition(targPos, camPos, -orbitAngle);

            Vector3 ChosenSideMarker = SetSide(ActorsInScene.Select(x => x.position).ToList());
            camPos = ChosenSideMarker.GetClosest(option1, option2);

            // Calculate camera rotation
            Quaternion camRot = Quaternion.LookRotation(targPos - camPos);

            // Apply bias
            camPos += camRot * Vector3.right * biasX;

            return new Pose(camPos, camRot);
        }

        /// <summary>
        /// Calculate the camera position for an over-shoulder shot.
        /// </summary>
        /// <param name="shot"></param>
        /// <param name="posData"></param>
        /// <returns></returns>
        private Pose CalculateOverShoulder(CamShotConfig shot, PreviewActorPositionData posData)
        {

            Vector3 targPos = posData.OppPosition;
            Vector3 camPos = posData.ActorPosition;
            float angleHeight = CamSettings.GetAngle(shot);
            camPos -= posData.ForwardN * CamSettings.GetDistance(shot);

            Vector3 rightN = Vector3.Cross(posData.OppForwardN, Vector3.up).normalized; 
            Vector3 option1 = camPos + rightN * CamSettings.GetDistance(shot);
            Vector3 option2 = camPos - rightN * CamSettings.GetDistance(shot);
            
            
            Vector3 ChosenSideMarker = SetSide(ActorsInScene.Select(x => x.position).ToList());
            camPos = ChosenSideMarker.GetClosest(option1, option2);

            camPos = new Vector3(camPos.x, camPos.y + angleHeight, camPos.z);

            Vector3 midpoint = (posData.ActorPosition + posData.OppPosition) / 2;
            Quaternion camRot = Quaternion.LookRotation(midpoint - camPos);

            return new Pose(camPos, camRot);
        }

        /// <summary>
        /// Calculate the camera position for a frame share shot.
        /// </summary>
        /// <param name="shot"></param>
        /// <param name="posData"></param>
        /// <returns></returns>
        private Pose CalculateFrameShare(CamShotConfig shot, PreviewActorPositionData posData)
        {
            float actorDistance = Vector3.Distance(posData.ActorPosition, posData.OppPosition);
            Vector3 actorADirN = (posData.ActorPosition - posData.OppPosition).normalized;
            Vector3 MidPoint = posData.OppPosition + actorADirN * (actorDistance / 2);

            Vector3 PDir1 = Quaternion.AngleAxis(90, Vector3.up) * actorADirN;
            Vector3 PDir2 = Quaternion.AngleAxis(-90, Vector3.up) * actorADirN;
            Vector3 option1 = MidPoint + PDir1 * (actorDistance + CamSettings.GetDistance(shot));
            Vector3 option2 = MidPoint + PDir2 * (actorDistance + CamSettings.GetDistance(shot));

            Vector3 ChosenSideMarker = SetSide(ActorsInScene.Select(x => x.position).ToList());
            Vector3 camPos = ChosenSideMarker.GetClosest(option1, option2);
            float angleHeight = CamSettings.GetAngle(shot);
            camPos = new Vector3(camPos.x, camPos.y + angleHeight, camPos.z);
            Quaternion camRot = Quaternion.LookRotation(MidPoint - camPos);

            return new Pose(camPos, camRot);
        }

        /// <summary>
        /// Calculate the midpoint of a list of focus targets.
        /// </summary>
        /// <param name="focusTargets"></param>
        /// <returns></returns>
        public Vector3 CalculateMidPoint(List<Vector3> focusTargets)
        {
            Vector3 vecCounter = Vector3.zero;
            foreach (var focusTarget in focusTargets)
            {
                vecCounter.x += focusTarget.x;
                vecCounter.y += focusTarget.y;
                vecCounter.z += focusTarget.z;
            }

            return vecCounter / focusTargets.Count;
        }

        public Vector3 SetSide(List<Vector3> actorPositions)
        {

            Side camSide = NodeManager.Instance.StartNode.CameraSide;

            if (actorPositions.Count == 1)
            {
                return actorPositions[0];
            }
            else if (actorPositions.Count == 2)
            {
                Vector3 posA = actorPositions[0];
                Vector3 posB = actorPositions[1];

                Vector3 midpoint = (posA + posB) / 2;

                Vector3 direction = (posB - posA).normalized;

                Vector3 rightDir = Quaternion.AngleAxis(90, Vector3.up) * direction;
                Vector3 leftDir = Quaternion.AngleAxis(-90, Vector3.up) * direction;

                Vector3 markerRight = midpoint + (rightDir * 10);
                Vector3 markerLeft = midpoint + (leftDir * 10);

                markerRight.y = posA.y;
                markerLeft.y = posA.y;

                // Select the appropriate marker based on the camera side
                return (camSide == Side.Right) ? markerRight : markerLeft;
            }

            return Vector2.zero;
        }
    }
}

