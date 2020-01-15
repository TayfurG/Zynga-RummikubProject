using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TileCreator : MonoBehaviour
{
    static public TileCreator TC { get; set; }
    // Start is called before the first frame update

    public GameObject tile = null;
    public Canvas canvas = null;
    public RectTransform Board = null;

    [System.NonSerialized]
    public List<TileStruct> tiles = new List<TileStruct>();

   
    public List<RectTransform> Slots = new List<RectTransform>();

    [System.NonSerialized]
    public int CurrentOpenSlotIndex = 0;

    public List<RectTransform> tilesSelected = new List<RectTransform>();

    public List<RectTransform> allTiles = new List<RectTransform>();

    [SerializeField]
    private RectTransform NewTilesButton = null;

    private Coroutine CreateTileCR = null;

    [SerializeField]
    private RectTransform Okey = null;
    private void Awake()
    {
        TC = this;
    }

    void Start()
    {
        CreateNewTiles();     
    }

   public void CreateNewTiles()
    {

        Color[] colors = new Color[4];
        colors[0] = Color.red;
        colors[1] = Color.blue;
        colors[2] = Color.black;
        colors[3] = Color.yellow;

        Color currColor = Color.red;

        int nextColorIndex = 0;
        int colorCount = colors.Length;

        int numbAssign = 1;

        for (int i = 0; i < 104; i++)
        {


            if (i % 13 == 0)
            {
                nextColorIndex++;
                if (nextColorIndex >= colorCount)
                    nextColorIndex = 0;
                numbAssign = 1;
            }

            TileStruct T = new TileStruct();
            T.color = colors[nextColorIndex];
            T.number = numbAssign++;
            T.isUsed = false;
            tiles.Add(T);



        }

        if(CreateTileCR != null)
        StopCoroutine(CreateTileCR);
        CreateTileCR = StartCoroutine(CreateTiles());

       
    }

    IEnumerator CreateTiles()
    {
       

        for (int i = 0; i < 14; i++)
        {
            yield return new WaitForSeconds(0.01f);
            int index = Random.Range(0, tiles.Count - 1);

            GameObject g = Instantiate(tile, NewTilesButton.transform.position, Quaternion.identity, canvas.transform);
            LeanTween.move(g, Slots[i].position, 0.07f);
            if (allTiles.Exists(s => s.GetComponent<TileSpecs>().color == tiles[index].color &&
                                    s.GetComponent<TileSpecs>().number == tiles[index].number))
            {
                g.GetComponent<TileSpecs>().isDuplicated = true;
            }
            else
            {
                g.GetComponent<TileSpecs>().isDuplicated = false;
            }

            allTiles.Add(g.GetComponent<RectTransform>());
            Slots[i].GetComponent<SlotScript>().isOccupied = true;



            g.GetComponent<TileSpecs>().color = tiles[index].color;
            g.GetComponent<TileSpecs>().number = tiles[index].number;
            g.GetComponent<TileSpecs>().isUsed = tiles[index].isUsed;

            if (i == 0)
            {
                g.GetComponent<TileSpecs>().isUnique = true;
                Okey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = g.GetComponent<TileSpecs>().color;
                Okey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = g.GetComponent<TileSpecs>().number.ToString();
            }
            else
            {
                g.GetComponent<TileSpecs>().isUnique = false;
            }

            g.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = tiles[index].color;
            g.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = tiles[index].number.ToString();

            tilesSelected.Add(g.GetComponent<RectTransform>());
            tiles.RemoveAt(index);


        }
    }



}
