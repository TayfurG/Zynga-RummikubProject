using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SorthingSmart : MonoBehaviour
{
  public void DoSorthing()
    {
        TileCreator.TC.CurrentOpenSlotIndex = 0;

        List<RectTransform> tileList = TileCreator.TC.tilesSelected;

        List<TileSpecs> tileSpecsList = new List<TileSpecs>();

        List<TileSpecs> UselessTiles = new List<TileSpecs>();

        Sorting123.instance.Do123Sorting(tileList, tileSpecsList, UselessTiles, false);
        Sorting777.instance.Do777Sorting(tileList, tileSpecsList, UselessTiles);

        for (int i = 0; i < UselessTiles.Count; i++)
        {
            LeanTween.move(UselessTiles[i].gameObject,
                                                 TileCreator.TC.Slots[TileCreator.TC.CurrentOpenSlotIndex].transform.position,
                                                 0.35f);
           // UselessTiles[i].TargetPos = TileCreator.TC.Slots[TileCreator.TC.CurrentOpenSlotIndex].transform.position;
            TileCreator.TC.Slots[TileCreator.TC.CurrentOpenSlotIndex].GetComponent<SlotScript>().isOccupied = true;
            TileCreator.TC.CurrentOpenSlotIndex++;
        }
        Debug.Log(UselessTiles.Count);
    }
}
