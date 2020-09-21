using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Game.Scripts.ChessBoard;
using Game.Scripts.ChessBoard.Models;
using Game.Scripts.Utils;
using UnityEngine;

/// <summary>
/// Keeps a reference to the interactive objects in the world.
/// It is also a Singleton for ease of access.
/// </summary>
public class InteractionWorld : Singleton<InteractionWorld>
{
    public List<Tile> Tiles = new List<Tile>();
    public List<Grabbable> Grabbables = new List<Grabbable>();

    public void AddGrabbable(Grabbable grabbable)
    {
        Grabbables.Add(grabbable);
    }

    public void AddTile(Tile tile)
    {
        Tiles.Add(tile);
    }

    public void ResetTileValidMoves()
    {
        foreach (var t in Tiles) t.SetValid(false);
    }

    public void SetTileValidMove(byte x, byte y)
    {
        foreach (Tile tile in Tiles)
        {
            bool isValid =
                ChessBoardConstants.Instance.Engine.IsValidMove(
                    x, y,
                    tile.Position.x, tile.Position.y);

            if (isValid)
            {
                tile.SetValid(true);
            }


            if (tile.Position.x == x && tile.Position.y == y)
            {
                tile.SetValid(true);
            }
        }
    }

    public List<Tile> GetValidTiles()
    {
        List<Tile> t = new List<Tile>();

        foreach (Tile tile in Tiles)
        {
            if (tile.IsValid)
            {
                t.Add(tile);
            }
        }

        return t;
    }

    public void RemoveGrabbable(Grabbable grabbable)
    {
        Grabbables.Remove(grabbable);
    }
}