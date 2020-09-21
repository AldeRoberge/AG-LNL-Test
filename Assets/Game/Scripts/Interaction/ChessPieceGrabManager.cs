using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DefaultNamespace;
using Game.Scripts.ChessBoard;
using UnityEngine;
using Utils;

public class ChessPieceGrabManager : MonoBehaviour
{
    private PinchDetectorInitializer p;

    public Grabbable currentlyHovering;
    public Grabbable currentlyGrabbing;

    // Start is called before the first frame update
    void Start()
    {
        p = gameObject.AddComponent<PinchDetectorInitializer>();

        p.RightHandPinchDetector.OnPinchStart.AddListener(() => { Grab(p.RightHandPinchDetector.FingerTipObj); });
        p.RightHandPinchDetector.OnPinchEnd.AddListener(() => { StartCoroutine(Ungrab()); });
    }

    private void Grab(GameObject fingerTipObj)
    {
        if (ChessBoardConstants.Instance.Engine.WhoseMove != ChessBoardConstants.Instance.Engine.HumanPlayer)
        {
            Debug.Log("Is not currently your turn!");
            return;
        }

        Grabbable b = NearestObjUtils.GetNearestGameObject(
            fingerTipObj,
            InteractionWorld.Instance.Grabbables);

        // TODO add limit

        currentlyGrabbing = b;
        b.GrabbingStart(fingerTipObj);
        Debug.Log("Started grabbing : " + b.name);

        UpdateValidTiles(b.Tile);

        List<Tile> validMoves = InteractionWorld.Instance.GetValidTiles();

        if (validMoves.Count == 0)
        {
            MoveCurrentlyGrabbedPiece(currentlyGrabbing.Tile);
            Debug.Log("Moved piece to original position.");
            return;
        }
    }

    private void UpdateValidTiles(Tile bTile)
    {
        InteractionWorld.Instance.ResetTileValidMoves();

        Debug.Log("Loading valid tiles for " + bTile.Position + ".");

        InteractionWorld.Instance.SetTileValidMove(bTile.Position.x, bTile.Position.y);
    }

    private IEnumerator Ungrab()
    {
        if (currentlyGrabbing == null)
        {
            yield break;
        }

        Debug.Log("It is currently turn of " + ChessBoardConstants.Instance.Engine.WhoseMove);
        Debug.Log("Human is of color " + ChessBoardConstants.Instance.Engine.HumanPlayer);

        List<Tile> validMoves = InteractionWorld.Instance.GetValidTiles();

        Debug.Log("There is " + validMoves.Count + " valid moves from " + currentlyGrabbing.Tile.Position + "...");

        foreach (Tile tile in validMoves)
        {
            Debug.Log("Valid move : " + tile.Position);
        }

        Tile nearestValidTile = NearestObjUtils.GetNearestGameObject(
            p.RightHandPinchDetector.FingerTipObj,
            validMoves);


        Debug.Log("The nearest valid tile is " + nearestValidTile + ".");

        byte fromX = currentlyGrabbing.Tile.Position.x;
        byte fromY = currentlyGrabbing.Tile.Position.y;
        byte toX = nearestValidTile.Position.x;
        byte toY = nearestValidTile.Position.y;

        Debug.Log(fromX + " " + fromY + ", " + toX + "," + toY);


        if (fromX == toX && fromY == toY)
        {
            MoveCurrentlyGrabbedPiece(currentlyGrabbing.Tile);
            yield break;
        }

        bool isValidMove =
            ChessBoardConstants.Instance.Engine.IsValidMove(fromX, fromY, toX, toY);

        for (byte x = 0; x < 9; x++)
        {
            for (byte y = 0; y < 9; y++)
            {
                bool isValid = ChessBoardConstants.Instance.Engine.IsValidMove(fromX, fromY, x, y);
                Debug.Log("Is valid move : " + isValid + ", " + x + "," + y);
            }
        }

        if (!isValidMove)
        {
            Debug.Log("Is not a valid move!");
        }

        ChessBoardConstants.Instance.Engine.MovePiece(fromX, fromY, toX, toY);

        MoveCurrentlyGrabbedPiece(nearestValidTile);

        StartCoroutine(WaitForEngineThinking());
    }

    private void MoveCurrentlyGrabbedPiece(Tile nearestValidTile)
    {
        currentlyGrabbing.GrabbingStopped();
        nearestValidTile.SetPiece(currentlyGrabbing.gameObject);
        InteractionWorld.Instance.ResetTileValidMoves();

        currentlyGrabbing = null;
    }

    private bool EngineIsThinking = false;

    private IEnumerator WaitForEngineThinking()
    {
        Task.Factory.StartNew(() => { ChessBoardConstants.Instance.Engine.AiPonderMove(); });

        // Wait for Engine.Thinking to turn true
        yield return new WaitForSeconds(1);

        while (ChessBoardConstants.Instance.Engine.Thinking)
        {
            if (!EngineIsThinking)
            {
                EngineIsThinking = true;
                Debug.Log("Engine is thinking...");
            }

            yield return new WaitForFixedUpdate();
        }

        EngineIsThinking = false;

        Debug.Log("Engine has stopped thinking.");

        StartCoroutine(ChessBoardGenerator.Instance.RegenPieces());
    }

    public void Update()
    {
        
        // Only update tracking if hand tracking is of high confidence
        if (p.RightHandPinchDetector.OVRHand.IsDataHighConfidence)
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
}