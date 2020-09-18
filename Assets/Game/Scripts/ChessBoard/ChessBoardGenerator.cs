using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using ChessEngine.Engine;
using DefaultNamespace;
using Game.Scripts.ChessBoard;
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
        PieceWhiteMaterial = ResourceLoader.Load<Material>("Chess/Pieces/Materials/White");
        PieceBlackMaterial = ResourceLoader.Load<Material>("Chess/Pieces/Materials/Black");

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
        board.transform.parent = transform;

        GameObject tilesParent = new GameObject("Tiles");
        GameObject piecesParent = new GameObject("Pieces");

        tilesParent.transform.parent = board.transform;
        piecesParent.transform.parent = board.transform;

        for (int x = 0; x < ChessBoardConstants.Size; x++)
        {
            for (int y = 0; y < ChessBoardConstants.Size; y++)
            {
                Tile tile = CreateTileAt(x, y, tilesParent);
                
                GameObject piece = CreatePieceAt(x, y);

                if (piece != null)
                {
                    tile.SetPiece(piece);
                }
            }
        }

        // Rescale
        board.transform.transform.position = new Vector3(-0.5f, 5.5f, 0.3f);
        board.transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);
    }


    const float tileOffset = 2.5f;

    /// <summary>
    /// Create a board tile at a given coordinate.
    /// </summary>
    private Tile CreateTileAt(int x, int y, GameObject parent)
    {
        GameObject tilePrefab = _chessBoardAssets.TilePrefab;

        GameObject tile = Instantiate(tilePrefab, new Vector3(x * tileOffset, 0, y * tileOffset), Quaternion.identity);
        tile.name = "Tile (" + x + "," + y + ")";
        tile.transform.parent = parent.transform;

        if ((x + y) % 2 == 1)
            tile.GetComponent<MeshRenderer>().material = _chessBoardAssets.TileBlackMaterial;
        else
            tile.GetComponent<MeshRenderer>().material = _chessBoardAssets.TileWhiteMaterial;

        Tile t = tile.AddComponent<Tile>();
        return t;
    }

    /// <summary>
    /// Creates a chess piece at a given coordinate.
    /// </summary>
    private GameObject CreatePieceAt(int x, int y)
    {
        var type = engine.GetPieceTypeAt((byte) x, (byte) y);

        // Init prefab
        GameObject piecePrefab = null;

        switch (type)
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

        // Happens when there is no piece at given board position.
        if (piecePrefab == null)
            return null;

        // Create parent
        GameObject parentPiece = new GameObject();

        GameObject piece = Instantiate(piecePrefab);
        piece.name = "Piece";

        // Set name and color
        var color = engine.GetPieceColorAt((byte) x, (byte) y);

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

        parentPiece.name += " " + type + " (" + x + ", " + y + ")";

        // Make it grabbable
        Grabbable g = parentPiece.AddComponent<Grabbable>();
        g.gameObject.AddComponent<BoxCollider>();
        piece.transform.parent = g.GrabbingSphere.transform;
        g.SetGrabbableObject(piece);

        return parentPiece;
    }
}