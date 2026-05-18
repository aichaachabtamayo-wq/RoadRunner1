using System;

namespace TemplateVSCode;

public class UI
{
    protected List<UI_Element> uiElements;

    public UI()
    {
       uiElements = new List<UI_Element>();
    }

    public void Add(UI_Element element)
    {
        uiElements.Add(element);
    }

    public void Draw()
    {
        foreach (UI_Element element in uiElements)
        {
            element.Draw();
        }
    }

    public void UpdateUIElementValue(string elementName, int newValue)
    {
        bool updated = false;
        for(int i = 0; i < uiElements.Count && !updated; i++)
        {
            if(uiElements[i].Name == elementName) 
            {
                uiElements[i].ElementValue = newValue;
                updated = true; //om niet verder in de lijst te zoeken eens gevonden
            }
        }
    }
    }

   
