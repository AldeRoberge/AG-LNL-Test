using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using ChessEngine.Engine;
using DefaultNamespace;
using ResourcesLoader;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;


public static class ChessBoardConstants
{
    public const int Size = 8;
}

public class ChessBoardAssets : MonoBehaviour
{
    public GameObject TilePrefab;

    public Material TileWhiteMaterial;
    public Material TileBlackMaterial;

    public GameObject QueenPrefab;
    public GameObject KingPrefab;
    public GameObject RookPrefab;
    public GameObject BishopPrefab;
    public GameObject KnightPrefab;
    public GameObject PawnData;

    public Material PieceWhiteMaterial;
    public Material PieceBlackMaterial;

    public UnityEvent OnLoadComplete = new UnityEvent();

    // Start is called before the first frame update
    void Awake()
    {
        // Tiles
        TileWhiteMaterial = ResourceLoader.Load<Material>("Chess/Tiles/Materials/TileMatBlack");
        TileBlackMaterial = ResourceLoader.Load<Material>("Chess/Tiles/Materials/TileMatWhite");

        TilePrefab = ResourceLoader.Load<GameObject>("Chess/Tiles/Prefabs/Tile");

        // Pieces
        PieceWhiteMaterial = ResourceLoader.Load<Material>("Chess/Pieces/Materials/Black");
        PieceBlackMaterial = ResourceLoader.Load<Material>("Chess/Pieces/Materials/White");

        QueenPrefab = ResourceLoader.Load<GameObject>("Chess/Pieces/Prefabs/Queen");
        KingPrefab = ResourceLoader.Load<GameObject>("Chess/Pieces/Prefabs/King");
        RookPrefab = ResourceLoader.Load<GameObject>("Chess/Pieces/Prefabs/Rook");
        BishopPrefab = ResourceLoader.Load<GameObject>("Chess/Pieces/Prefabs/Bishop");
        KnightPrefab = ResourceLoader.Load<GameObject>("Chess/Pieces/Prefabs/Knight");
        PawnData = ResourceLoader.Load<GameObject>("Chess/Pieces/Prefabs/Pawn");


        OnLoadComplete.Invoke();
    }
}


public class ChessBoardGenerator : MonoBehaviour
{
    private ChessBoardAssets _chessBoardAssets;

    private Engine engine;

    public void Start()
    {
        engine = new Engine("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");

        _chessBoardAssets = gameObject.AddComponent<ChessBoardAssets>();
        GenerateGrid();
    }


    private void GenerateGrid()
    {
        GameObject board = new GameObject("Board");

        GameObject tilesParent = new GameObject("Tiles");
        GameObject piecesParent = new GameObject("Pieces");

        tilesParent.transform.parent = board.transform;
        piecesParent.transform.parent = board.transform;

        for (int x = 0; x < ChessBoardConstants.Size; x++)
        {
            for (int y = 0; y < ChessBoardConstants.Size; y++)
            {
                CreateTileAt(x, y, tilesParent);
                CreatePieceAt(x, y, piecesParent);
            }
        }

        // Rescale

        board.transform.transform.position = new Vector3(-0.5f, 5.5f, 0.3f);
        board.transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);
    }

    const float tileOffset = 2.5f;

    private void CreatePieceAt(int x, int y, GameObject parent)
    {
        var chessPieceType = engine.GetPieceTypeAt((byte) x, (byte) y);
        var color = engine.GetPieceColorAt((byte) x, (byte) y);

        GameObject piecePrefab = null;

        switch (chessPieceType)
        {
            case ChessPieceType.Bishop:
                piecePrefab = _chessBoardAssets.BishopPrefab;
                break;
            case ChessPieceType.King:
                piecePrefab = _chessBoardAssets.KingPrefab;
                break;
            case ChessPieceType.Knight:
                piecePrefab = _chessBoardAssets.KnightPrefab;
                break;
            case ChessPieceType.Pawn:
                piecePrefab = _chessBoardAssets.PawnData;
                break;
            case ChessPieceType.Queen:
                piecePrefab = _chessBoardAssets.QueenPrefab;
                break;
            case ChessPieceType.Rook:
                piecePrefab = _chessBoardAssets.RookPrefab;
                break;
            case ChessPieceType.None:
                break;
        }

        if (piecePrefab != null)
        {
            GameObject parentPiece = new GameObject();
            parentPiece.transform.position = new Vector3(x * tileOffset, 0, y * tileOffset);
            parentPiece.transform.parent = parent.transform;

            GameObject piece = Instantiate(piecePrefab);
            piece.name = "Piece";
            piece.transform.parent = parentPiece.transform;

            if (color == ChessPieceColor.White)
            {
                parentPiece.name = "White";
                piece.GetComponent<MeshRenderer>().material = _chessBoardAssets.PieceWhiteMaterial;
            }
            else
            {
                parentPiece.name = "Black";
                piece.GetComponent<MeshRenderer>().material = _chessBoardAssets.PieceBlackMaterial;
            }

            parentPiece.name += " " + chessPieceType + " (" + x + ", " + y + ")";

            /* parentPiece.transform.Rotate(-90, 0, 0);
 
             piece.transform.rotation();*/

            piece.transform.position = new Vector3(
                parentPiece.transform.position.x,
                parentPiece.transform.position.y + 3,
                parentPiece.transform.position.z);

            piece.AddComponent<Rigidbody>();
            piece.AddComponent<BoxCollider>();

            Grabbable g = parentPiece.AddComponent<Grabbable>();
            g.SetGrabbableObject(piece);
        }
    }

    private void CreateTileAt(int x, int y, GameObject parent)
    {
        GameObject tile = _chessBoardAssets.TilePrefab;

        GameObject o = Instantiate(tile, new Vector3(x * tileOffset, 0, y * tileOffset), Quaternion.identity);
        o.name = "Tile (" + x + "," + y + ")";
        o.transform.parent = parent.transform;

        if ((x + y) % 2 == 1)
            o.GetComponent<MeshRenderer>().material = _chessBoardAssets.TileBlackMaterial;
        else
            o.GetComponent<MeshRenderer>().material = _chessBoardAssets.TileWhiteMaterial;
    }
}