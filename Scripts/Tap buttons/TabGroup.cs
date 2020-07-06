using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    public TabButton selectedTab;
    public List<TabButton> tabs;

    public List<GameObject> pages = new List<GameObject>(); 


    public Sprite offImage;
    public Sprite hoverImage;
    public Sprite activeImage;
    public void Addbutton(TabButton button)
    {
        if (tabs.Count <= 0)
        {
            tabs = new List<TabButton>();
            selectedTab = button;
            button.image.sprite = activeImage;
        }
        tabs.Add(button);
    }

    public void onClickButton(TabButton button)
    {
        selectedTab = button;

        for (int i = 0; i < tabs.Count; i++)
        {
            if (tabs[i] != button)
            {
                tabs[i].image.sprite = offImage;
                pages[i].SetActive(false);
            }
            else
            {
                pages[i].SetActive(true);
            }
        }

        selectedTab.image.sprite = activeImage;
    }

    public void onEnterButton(TabButton button)
    {
        if (button != selectedTab)
        {
            button.image.sprite = hoverImage;
        }
    }

    public void onExitButton(TabButton button)
    {
        if (button != selectedTab)
        {
            button.image.sprite = offImage;
        }
    }
}
