using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using ChessEngine.Engine;
using DefaultNamespace;
using Game.Scripts.ChessBoard;
using Game.Scripts.ChessBoard.Models;
using Game.Scripts.Utils;
using ResourcesLoader;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;


public class ChessBoardConstants : Singleton<ChessBoardConstants>
{
    public Engine Engine;

    
    public const int Size = 8;


    public void Awake()
    {
        Engine = new Engine();
    }
}

public class ChessBoardAssets : Singleton<ChessBoardAssets>
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

    public Material TargetPlaneMaterial;

    // Start is called before the first frame update
    private new void Awake()
    {
        base.Awake();

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

        TargetPlaneMaterial = ResourceLoader.Load<Material>("Chess/Interaction/TargetPlane/Green");
    }
}


public class ChessBoardGenerator : MonoBehaviour
{

    public void Start()
    {

        Generate();
    }

    /// <summary>
    /// Generate the chess board (tiles and pieces).
    /// </summary>
    private void Generate()
    {
        GameObject board = new GameObject("Board");
        board.transform.parent = transform;

        GameObject tilesParent = new GameObject("Tiles");
        GameObject piecesParent = new GameObject("Pieces");

        tilesParent.transform.parent = board.transform;
        piecesParent.transform.parent = board.transform;

        for (byte x = 0; x < ChessBoardConstants.Size; x++)
        {
            for (byte y = 0; y < ChessBoardConstants.Size; y++)
            {
                Tile tile = CreateTileAt(x, y, tilesParent);

                GameObject piece = CreatePieceAt(x, y, tile);

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
    private Tile CreateTileAt(byte x, byte y, GameObject parent)
    {
        GameObject tilePrefab = ChessBoardAssets.Instance.TilePrefab;

        GameObject tile = Instantiate(tilePrefab, new Vector3(x * tileOffset, 0, y * tileOffset), Quaternion.identity);
        tile.name = "Tile (" + x + "," + y + ")";
        tile.transform.parent = parent.transform;

        if ((x + y) % 2 == 1)
            tile.GetComponent<MeshRenderer>().material = ChessBoardAssets.Instance.TileBlackMaterial;
        else
            tile.GetComponent<MeshRenderer>().material = ChessBoardAssets.Instance.TileWhiteMaterial;

        Tile t = tile.AddComponent<Tile>();
        t.SetPosition(new Position(x, y));

        return t;
    }

    /// <summary>
    /// Creates a chess piece at a given coordinate.
    /// </summary>
    private GameObject CreatePieceAt(int x, int y, Tile tile)
    {
        var type = ChessBoardConstants.Instance.Engine.GetPieceTypeAt((byte) x, (byte) y);

        // Init prefab
        GameObject piecePrefab = null;

        switch (type)
        {
            case ChessPieceType.Bishop:
                piecePrefab = ChessBoardAssets.Instance.BishopPrefab;
                break;
            case ChessPieceType.King:
                piecePrefab = ChessBoardAssets.Instance.KingPrefab;
                break;
            case ChessPieceType.Knight:
                piecePrefab = ChessBoardAssets.Instance.KnightPrefab;
                break;
            case ChessPieceType.Pawn:
                piecePrefab = ChessBoardAssets.Instance.PawnData;
                break;
            case ChessPieceType.Queen:
                piecePrefab = ChessBoardAssets.Instance.QueenPrefab;
                break;
            case ChessPieceType.Rook:
                piecePrefab = ChessBoardAssets.Instance.RookPrefab;
                break;
            case ChessPieceType.None:
                break;
        }

        // Happens when there is no piece at given board position.
        if (piecePrefab == null)
            return null;

        // Create parent
        GameObject parent = new GameObject();
        parent.transform.position = Vector3.zero;

        GameObject piece = Instantiate(piecePrefab, parent.transform, true);
        piece.name = "Piece";
        piece.transform.position = Vector3.zero;

        // Set parent name and piece color
        var color = ChessBoardConstants.Instance.Engine.GetPieceColorAt((byte) x, (byte) y);

        if (color == ChessPieceColor.White)
        {
            parent.name = "White";
            piece.GetComponentInChildren<MeshRenderer>().material = ChessBoardAssets.Instance.PieceWhiteMaterial;
        }
        else
        {
            parent.name = "Black";
            piece.GetComponentInChildren<MeshRenderer>().material = ChessBoardAssets.Instance.PieceBlackMaterial;
        }

        BoxCollider b = piece.AddComponent<BoxCollider>();

        parent.name += " " + type + " (" + x + ", " + y + ")";
        piece.transform.localScale =
            new Vector3(piece.transform.localScale.x / 2,
                piece.transform.localScale.y / 2,
                piece.transform.localScale.z / 2);


        Grabbable g = parent.AddComponent<Grabbable>();
        g.SetCurrentTile(tile);

        return parent;
    }
}