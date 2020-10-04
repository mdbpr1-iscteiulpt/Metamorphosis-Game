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
        }
    }
}
