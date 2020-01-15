using System.Collections;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class TileSpecs : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,IPointerDownHandler,IPointerUpHandler
{

        public Color color;
        public int number;
        public bool isUnique;
        public bool isUsed;
        public bool isDuplicated;

    private float requiredHoldTime = 0.65f;

    [SerializeField] private Sprite FrontFace = null;
    [SerializeField] private Sprite BackFace = null;

    private Coroutine FlipFaceCR = null;

    private Vector3 startDragPos = Vector3.zero;
    private RectTransform startingSpot = null;


 

    IEnumerator FlipFace()
    {
        yield return new WaitForSeconds(requiredHoldTime);
        
        if(transform.GetComponent<Image>().sprite == FrontFace)
        {
            transform.GetComponent<Image>().sprite = BackFace;
            transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            transform.GetComponent<Image>().sprite = FrontFace;
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(isUnique)
        {
            FlipFaceCR = StartCoroutine(FlipFace());
        }
           
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(isUnique)
        ResetPointer();
    }


    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        float distance = float.PositiveInfinity;

        foreach (RectTransform rectT in TileCreator.TC.Slots)
        {
            if ((rectT.transform.position - transform.position).magnitude < distance)
            {
                startingSpot = rectT;
                distance = (rectT.transform.position - transform.position).magnitude;
            }
        }

        startDragPos = transform.position;
        transform.position = Input.mousePosition;      
        if (isUnique)
        ResetPointer();
    }

    public void OnDrag(PointerEventData eventData)
    {

        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        RectTransform closestSpotPoint = null;
        float distance = float.PositiveInfinity;

        foreach (RectTransform rectT in TileCreator.TC.Slots)
        {     
            if((rectT.transform.position - transform.position).magnitude < distance)
            {
                closestSpotPoint = rectT;
                distance = (rectT.transform.position - transform.position).magnitude;
            }      
        }
        
        if(closestSpotPoint.GetComponent<SlotScript>().isOccupied)
        {
            transform.position = startDragPos;
        }
        else
        {
            transform.position = closestSpotPoint.transform.position;
            startingSpot.GetComponent<SlotScript>().isOccupied = false;
            closestSpotPoint.GetComponent<SlotScript>().isOccupied = true;
        }
  
        
    }

    void ResetPointer()
    {
        StopCoroutine(FlipFaceCR);

    }

}
