using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelection : MonoBehaviour
{
    public RectTransform selectionBox;
    public LayerMask unitLayerMask;

    private List<Unit> selectedUnits = new List<Unit>();
    private Vector2 startPos;

    private Camera cam;
    private Player player;

    private void Awake()
    {
        //get the components
        cam = Camera.main;
        player = GetComponent<Player>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //mouse down
        if (Input.GetMouseButtonDown(0))
        {
            ToggleSelectionVisual(false);
            selectedUnits = new List<Unit>();

            //TrySelect(Input.mousePosition);
            startPos = Input.mousePosition;
        }

        //mouse up
        if (Input.GetMouseButtonUp(0))
        {
            releaseSelectionBox();
        }

        //mouse hold down
        if (Input.GetMouseButton(0))
        {
            updateSelectionBox(Input.mousePosition);
        }
        //player action for movement after confirmation check
        if(Input.GetMouseButton(1)&&checkIfAnyCanMove())
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                List<Vector3> targetPositionList = GetPositionListAround(hit.point, new float[] { 10f, 20f, 30f }, new int[] { 5, 10, 20 });
                int targetPositionListIndex = 0;
                foreach (Unit unit in selectedUnits)
                {
                    unit.GetComponent<Movement>().movementDestination(targetPositionList[targetPositionListIndex]);
                    targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;
                }
            }
        }
    }


    private List<Vector3> GetPositionListAround(Vector3 startPosition, float[] ringDistanceArray, int[] ringPositionCountArray)
    {
        List<Vector3> positionList = new List<Vector3>();
        positionList.Add(startPosition);
        for (int i = 0; i < ringDistanceArray.Length; i++)
        {
            positionList.AddRange(GetPositionListAround(startPosition, ringDistanceArray[i], ringPositionCountArray[i]));
        }
        return positionList;
    }

    private List<Vector3> GetPositionListAround(Vector3 startPosition, float distance, int positionCount)
    {
        List<Vector3> positionList = new List<Vector3>();
        for (int i = 0; i < positionCount; i++)
        {
            float angle = i * (360f / positionCount);
            Vector3 dir = ApplyRotationToVector(new Vector3(1, 0), angle);
            Vector3 position = startPosition + dir * distance;
            positionList.Add(position);
        }
        return positionList;
    }

    private Vector3 ApplyRotationToVector(Vector3 vec, float angle)
    {
        return Quaternion.Euler(0, 0, angle) * vec;
    }

    void releaseSelectionBox()
    {
        selectionBox.gameObject.SetActive(false);

        Vector2 min = selectionBox.anchoredPosition - (selectionBox.sizeDelta / 2);
        Vector2 max = selectionBox.anchoredPosition + (selectionBox.sizeDelta / 2);

        //Make Player
        foreach(Unit unit in player.units)
        {
            Vector3 screenPos = cam.WorldToScreenPoint(unit.transform.position);
            if(screenPos.x > min.x && screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y)
            {
                selectedUnits.Add(unit);
                unit.GetComponent<Movement>().selectUnit(true);
                unit.toggleSelectionVisual(true);
            }
        }
    }

    //called whoçe selecting
    void updateSelectionBox(Vector2 curMousePos)
    {
        if (!selectionBox.gameObject.activeInHierarchy)
        {
            selectionBox.gameObject.SetActive(true);
        }
        print(curMousePos);
        print(startPos);
        float width = curMousePos.x - startPos.x;
        float heigth = curMousePos.y - startPos.y;

        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(heigth));
        selectionBox.anchoredPosition = startPos + new Vector2(width / 2, heigth / 2);
    }

    void ToggleSelectionVisual (bool selected)
    {
        foreach(Unit unit in selectedUnits)
        {
            unit.toggleSelectionVisual(selected);
            unit.GetComponent<Movement>().selectUnit(selected);
        }
    }


    private bool checkIfAnyCanMove()
    {
        foreach (Unit units in selectedUnits)
        {
            return units.gameObject.GetComponent<Movement>();
        }
        return false;
    }
}
