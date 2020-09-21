using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Game.Scripts.ChessBoard;
using UnityEngine;
using Utils;

public class ChessPieceGrabManager : MonoBehaviour
{
    private PinchDetectorInitializer p;

    public Grabbable currentlyHovering;
    public Grabbable currentlyGrabbing;


    public Tile StartTile;

    // Start is called before the first frame update
    void Start()
    {
        p = gameObject.AddComponent<PinchDetectorInitializer>();

        p.RightHandPinchDetector.OnPinchStart.AddListener(() => { Grab(p.RightHandPinchDetector.FingerTipObj); });
        p.RightHandPinchDetector.OnPinchEnd.AddListener(() => { Ungrab(); });
    }

    private void Grab(GameObject fingerTipObj)
    {
        Grabbable b = NearestObjUtils.GetNearestGameObject(
            fingerTipObj,
            InteractionWorld.Instance.Grabbables);

        // TODO add limit

        currentlyGrabbing = b;
        b.GrabbingStart(fingerTipObj);
        Debug.Log("Started grabbing : " + b.name);

        UpdateValidTiles(b.Tile);
    }

    private void UpdateValidTiles(Tile bTile)
    {
        InteractionWorld.Instance.ResetTileValidMoves();
        InteractionWorld.Instance.SetTileValidMove(bTile.Position.x, bTile.Position.y);
    }

    private void Ungrab()
    {
        if (currentlyGrabbing == null)
        {
            return;
        }

        List<Tile> validMoves = InteractionWorld.Instance.GetValidTiles();

        Debug.Log("There is " + validMoves.Count + " valid moves...");

        Tile nearestValidTile = NearestObjUtils.GetNearestGameObject(
            p.RightHandPinchDetector.FingerTipObj,
            validMoves);

        Debug.Log("The nearest valid tile is " + nearestValidTile + ".");

        byte fromX = (byte) (nearestValidTile.Position.x);
        byte fromY = (byte) (nearestValidTile.Position.y);
        byte toX = (byte) (currentlyGrabbing.Tile.Position.x);
        byte toY = (byte) (currentlyGrabbing.Tile.Position.y);

        Debug.Log(fromX + "," + fromY);
        Debug.Log(toX + "," + toY);

        bool isValidMove =
            ChessBoardConstants.Instance.Engine.IsValidMove(fromX, fromY, toX, toY);

        for (byte x = 0; x < 9; x++)
        {
            for (byte y = 0; y < 9; y++)
            {
                bool isValid = ChessBoardConstants.Instance.Engine.IsValidMove(fromY, fromX, x, y);
                Debug.Log("Is valid move : " + isValid + ", " + x + "," + y);
            }
        }

        if (!isValidMove)
        {
            Debug.Log("Is not a valid move!");
        }

        if (isValidMove)
        {
            ChessBoardConstants.Instance.Engine.MovePiece(fromX, fromY, toX, toY);

            currentlyGrabbing.GrabbingStopped();
            nearestValidTile.SetPiece(currentlyGrabbing.gameObject);

            InteractionWorld.Instance.ResetTileValidMoves();
        }
    }

    public void Update()
    {
        Grabbable b = NearestObjUtils.GetNearestGameObject(
            p.RightHandPinchDetector.FingerTipObj,
            InteractionWorld.Instance.Grabbables);

        if (currentlyHovering != b)
        {
            if (currentlyHovering == null) currentlyHovering = b;

            currentlyHovering.SetIsHovering(false);
            b.SetIsHovering(true);

            currentlyHovering = b;
        }
    }
}