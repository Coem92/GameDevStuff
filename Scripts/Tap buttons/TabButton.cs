using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour ,IPointerClickHandler,IPointerEnterHandler , IPointerExitHandler
{
    public TabGroup tabGroup;
    public Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        tabGroup.Addbutton(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.onClickButton(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tabGroup.onEnterButton(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabGroup.onExitButton(this);
    }
}
