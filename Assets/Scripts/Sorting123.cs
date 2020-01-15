using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorting123 : MonoBehaviour
{
    static public Sorting123 instance { get; set; }

    private void Awake()
    {
        instance = this;
    }


    public void DoSorting()
    {
        TileCreator.TC.CurrentOpenSlotIndex = 0;

        List<RectTransform> tileList = TileCreator.TC.tilesSelected;

        List<TileSpecs> tileSpecsList = new List<TileSpecs>();

        List<TileSpecs> removedTiles = new List<TileSpecs>();

        Do123Sorting(tileList, tileSpecsList, removedTiles, true);


        for (int i = 0; i < removedTiles.Count; i++)
        {
            LeanTween.move(removedTiles[i].gameObject,
                                                    TileCreator.TC.Slots[TileCreator.TC.CurrentOpenSlotIndex].transform.position,
                                                    0.35f);
            //removedTiles[i].TargetPos = TileCreator.TC.Slots[TileCreator.TC.CurrentOpenSlotIndex].transform.position;
            TileCreator.TC.Slots[TileCreator.TC.CurrentOpenSlotIndex].GetComponent<SlotScript>().isOccupied = true;
            TileCreator.TC.CurrentOpenSlotIndex++;
        }

    }

    public void Do123Sorting(List<RectTransform> tileList, List<TileSpecs> tileSpecsList, List<TileSpecs> removedTiles, bool solo)
    {
        InsertionSort(tileList);

        for (int i = 0; i < tileList.Count; i++)
        {
            tileSpecsList.Add(tileList[i].GetComponent<TileSpecs>());
        }
        for (int i = 0; i < tileSpecsList.Count; i++)
        {
            tileSpecsList[i].isUsed = false;
        }

        List<TileSpecs> redTiles = new List<TileSpecs>();
        List<TileSpecs> blackTiles = new List<TileSpecs>();
        List<TileSpecs> blueTiles = new List<TileSpecs>();
        List<TileSpecs> yellowTiles = new List<TileSpecs>();

        CreateListByColor(redTiles, tileSpecsList, Color.red);
        CreateListByColor(blackTiles, tileSpecsList, Color.black);
        CreateListByColor(blueTiles, tileSpecsList, Color.blue);
        CreateListByColor(yellowTiles, tileSpecsList, Color.yellow);

        List<List<TileSpecs>> allMatchedTiles = new List<List<TileSpecs>>();
     

        SetTiles(redTiles, allMatchedTiles, removedTiles, solo);
        SetTiles(blackTiles, allMatchedTiles, removedTiles,solo);
        SetTiles(blueTiles, allMatchedTiles, removedTiles, solo);
        SetTiles(yellowTiles, allMatchedTiles, removedTiles, solo);

        MoveTiles(allMatchedTiles, removedTiles);
    }

    void MoveTiles(List<List<TileSpecs>> allMatchedTiles, List<TileSpecs> removedTiles)
    {

        foreach (RectTransform s in TileCreator.TC.Slots)
        {
            s.GetComponent<SlotScript>().isOccupied = false;
        }

        for (int i = 0; i < allMatchedTiles.Count; i++)
        {
            for (int j = 0; j < allMatchedTiles[i].Count; j++)
            {
                LeanTween.move(allMatchedTiles[i][j].gameObject,
                                                     TileCreator.TC.Slots[TileCreator.TC.CurrentOpenSlotIndex].transform.position,
                                                     0.35f);
                //allMatchedTiles[i][j].transform.position = TileCreator.TC.Slots[TileCreator.TC.CurrentOpenSlotIndex].transform.position;
                TileCreator.TC.Slots[TileCreator.TC.CurrentOpenSlotIndex].GetComponent<SlotScript>().isOccupied = true;
                allMatchedTiles[i][j].isUsed = true;
                TileCreator.TC.CurrentOpenSlotIndex++;

            }
            TileCreator.TC.CurrentOpenSlotIndex++;
        }



    }

    void SetTiles(List<TileSpecs> tiles, List<List<TileSpecs>> allMatchedTiles, List<TileSpecs> removedTiles, bool solo)
    {
        List<int> selectedNumbers = new List<int>();

        while(selectedNumbers != null)
        {

    

        selectedNumbers = FindCons(tiles);
        List<TileSpecs> ChosenTiles = new List<TileSpecs>();



        if (selectedNumbers != null)
        {
            for (int i = 0; i < selectedNumbers.Count; i++)
            {
                for (int j = 0; j < tiles.Count; j++)
                {
                    if (selectedNumbers[i] == tiles[j].number)
                    {
                        ChosenTiles.Add(tiles[j]);
                        break;
                    }
                }
            }

                allMatchedTiles.Add(ChosenTiles);
        }

        for (int i = 0; i < ChosenTiles.Count; i++)
        {
            for (int j = 0; j < tiles.Count; j++)
            {
                if (ChosenTiles[i] == tiles[j])
                {
                     
                        tiles.RemoveAt(j);
                      
                    break;
                }
            }
        }

        if(solo)
            {
                for (int i = 0; i < tiles.Count; i++)
                {
                    if (!removedTiles.Contains(tiles[i]))
                    {
                        removedTiles.Add(tiles[i]);
                    }
                }
            }
           

        }
    }

    private void CreateListByColor(List<TileSpecs> listToBeCreated, List<TileSpecs> tilesCache, Color color)
    {
        for (int i = 0; i < tilesCache.Count; i++)
        {
            if (tilesCache[i].color == color)
            {
                listToBeCreated.Add(tilesCache[i].GetComponent<TileSpecs>());
            }

        }
    }

    private List<int> FindCons(List<TileSpecs> tileSpecsList)
    {
        if (tileSpecsList.Count < 1)
            return null;
        Dictionary<int, int> hash = new Dictionary<int, int>();
        hash.Add(tileSpecsList[0].number, 1);
        int LIS_size = 1;
        int LIS_index = 0;

        for (int i = 1; i < tileSpecsList.Count; i++)
        {
            if (hash.ContainsKey(tileSpecsList[i].number - 1))
            {
                var val = hash[tileSpecsList[i].number - 1];
                hash.Remove(tileSpecsList[i].number);
                hash.Add(tileSpecsList[i].number, val + 1);

            }
            else
            {
                if(!hash.ContainsKey(tileSpecsList[i].number))
                {
                    hash.Add(tileSpecsList[i].number, 1);
                }
                
             
            }
            if (LIS_size < hash[tileSpecsList[i].number])
            {
                LIS_size = hash[tileSpecsList[i].number];
                LIS_index = tileSpecsList[i].number;
            }
        }

        if(LIS_size >= 3)
        {
            List<int> SelectedNumbers = new List<int>();

            int start = LIS_index - LIS_size + 1;
            while (start <= LIS_index)
            {
                SelectedNumbers.Add(start);

                start++;
            }
            return SelectedNumbers;
        }
        else
        {
            return null;
        }
          
    }

    private void InsertionSort(List<RectTransform> tileList)
    {
        //1. For each value in the array...
        for (int i = 1; i < tileList.Count; ++i)
        {
            //2. Store the current value in a variable.
            int currentValue = tileList[i].GetComponent<TileSpecs>().number;
            RectTransform currents = tileList[i];
            int pointer = i - 1;

            //3. While we are pointing to a valid value...
            //4. If the current value is less than the value we are pointing at...
            while (pointer >= 0 && tileList[pointer].GetComponent<TileSpecs>().number > currentValue)
            {
                //5. Then move the pointed-at value up one space, and store the
                //   current value at the pointed-at position.
                tileList[pointer + 1] = tileList[pointer];
                pointer = pointer - 1;
            }
            tileList[pointer + 1] = currents;
        }
    }
}
