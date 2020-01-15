using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Sorting777 : MonoBehaviour
{
    static public Sorting777 instance { get; set; }

    private void Awake()
    {
        instance = this;
    }

    public void DoSorting()
    {
        foreach (RectTransform s in TileCreator.TC.Slots)
        {
            s.GetComponent<SlotScript>().isOccupied = false;
        }

        TileCreator.TC.CurrentOpenSlotIndex = 0;

        List<RectTransform> tileList = TileCreator.TC.tilesSelected;

        List<TileSpecs> tileSpecsList = new List<TileSpecs>();

        List<TileSpecs> removedTiles = new List<TileSpecs>();

        for (int i = 0; i < tileList.Count; i++)
        {
            tileList[i].GetComponent<TileSpecs>().isUsed = false;
            tileSpecsList.Add(tileList[i].GetComponent<TileSpecs>());
        }

        Do777Sorting(tileList, tileSpecsList, removedTiles);


        for (int i = 0; i < removedTiles.Count; i++)
        {
            LeanTween.move(removedTiles[i].gameObject,
                                                    TileCreator.TC.Slots[TileCreator.TC.CurrentOpenSlotIndex].transform.position,
                                                    0.35f);
           // removedTiles[i].TargetPos = TileCreator.TC.Slots[TileCreator.TC.CurrentOpenSlotIndex].transform.position;
            TileCreator.TC.Slots[TileCreator.TC.CurrentOpenSlotIndex].GetComponent<SlotScript>().isOccupied = true;
            TileCreator.TC.CurrentOpenSlotIndex++;
        }

    }

    public void Do777Sorting(List<RectTransform> tileList, List<TileSpecs> tileSpecsList, List<TileSpecs> removedTiles)
    {
        TileSpecs UniqueTile = null;
        List<List<TileSpecs>> MatchedTiles = new List<List<TileSpecs>>();
        for (int i = 0; i < tileList.Count; i++)
        {
           
            if (tileList[i].GetComponent<TileSpecs>().isUnique)
            {
                UniqueTile = tileList[i].GetComponent<TileSpecs>();
            }
            
        }

        

        for (int i = 1; i < tileSpecsList.Count; i++)
        {
            int count = tileSpecsList.FindAll(s => s.number.Equals(i) && !s.isDuplicated && !s.isUnique && !s.isUsed).Count;

            if (count >= 3)
            {
                MatchedTiles.Add(tileSpecsList.FindAll(s => s.number.Equals(i) && !s.isDuplicated && !s.isUnique && !s.isUsed));
            }
            else if (count == 2  && !UniqueTile.isUsed)
            {
                UniqueTile.isUsed = true;
                List<TileSpecs> UniqeTileAdder;
                UniqeTileAdder = tileSpecsList.FindAll(s => s.number.Equals(i) && !s.isDuplicated && !s.isUnique && !s.isUsed);
                UniqeTileAdder.Add(UniqueTile);

                MatchedTiles.Add(UniqeTileAdder);
            }

        }

   

        for (int i = 0; i < MatchedTiles.Count; i++)
        {
            for (int j = 0; j < MatchedTiles[i].Count; j++)
            {
                if (MatchedTiles[i][j] != null)
                {
                    LeanTween.move(MatchedTiles[i][j].gameObject,
                                                    TileCreator.TC.Slots[TileCreator.TC.CurrentOpenSlotIndex].transform.position,
                                                    0.35f);
                  //  MatchedTiles[i][j].TargetPos = TileCreator.TC.Slots[TileCreator.TC.CurrentOpenSlotIndex].transform.position;
                    MatchedTiles[i][j].isUsed = true;
                    TileCreator.TC.Slots[TileCreator.TC.CurrentOpenSlotIndex].GetComponent<SlotScript>().isOccupied = true;
                    //   Debug.Log("Number: " + MatchedTiles[i][j].number + " Color: " + MatchedTiles[i][j].color + " Duplicated: "
                    //      + MatchedTiles[i][j].isDuplicated);
                    TileCreator.TC.CurrentOpenSlotIndex++;
                }
            }
            TileCreator.TC.CurrentOpenSlotIndex++;

        }

        for (int i = 0; i < tileSpecsList.Count; i++)
        {
            if (!tileSpecsList[i].isUsed)
            {
                if(!removedTiles.Contains(tileSpecsList[i]))
                {
                    removedTiles.Add(tileSpecsList[i]);
                    Debug.Log("ALOOOOOOOOOOOOO");
                }
               
               
             //   tileSpecsList[i].transform.position = TileCreator.TC.Slots[TileCreator.TC.CurrentOpenSlotIndex].transform.position;
               // TileCreator.TC.CurrentOpenSlotIndex++;
            }
        }
    }


}
