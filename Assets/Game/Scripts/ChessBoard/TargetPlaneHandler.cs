﻿using Game.Scripts.ChessBoard.Models;
using UnityEngine;

namespace Game.Scripts.ChessBoard
{
    public class TargetPlaneHandler : MonoBehaviour
    {
        public GameObject TargetPlane;
        
        public void Awake()
        {
            TargetPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            TargetPlane.transform.position = new Vector3(0, 0.51f, 0);
            TargetPlane.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            TargetPlane.transform.SetParent(transform, false);
            TargetPlane.GetComponent<MeshRenderer>().material = ChessBoardAssets.Instance.TargetPlaneMaterial;

            SetTargetPlaneVisible(false);
        }

        public void SetTargetPlaneVisible(bool isVisible)
        {
            TargetPlane.GetComponent<Renderer>().enabled = isVisible;
        }
    }
}