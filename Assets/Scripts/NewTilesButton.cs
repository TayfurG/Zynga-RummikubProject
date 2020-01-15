using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTilesButton : MonoBehaviour
{
    public void NewTiles()
    {
        for (int i = 0; i < TileCreator.TC.Slots.Count; i++)
        {
            TileCreator.TC.Slots[i].GetComponent<SlotScript>().isOccupied = false;
        }

        TileCreator.TC.tiles = new List<TileStruct>();

        TileCreator.TC.tilesSelected = new List<RectTransform>();

        foreach (RectTransform r in TileCreator.TC.allTiles)
        {
            Destroy(r.gameObject);
        }
        TileCreator.TC.allTiles = new List<RectTransform>();

        TileCreator.TC.CreateNewTiles();
    }
}
