using System;

namespace TemplateVSCode;

public class Menu : Screen
{
    protected List<MenuItem> menuItems;
    protected int activeMenuItemID;
    protected ConsoleColor activeItemForegroundColor;
    protected ConsoleColor activeItemBackgroundColor;
    protected int previousActiveMenuItemID;
    public Menu(string filepath) 
    : base(filepath)
    {
        menuItems = new List<MenuItem>();
        activeMenuItemID = 0;
        activeItemForegroundColor = ConsoleColor.Black;
        activeItemBackgroundColor = ConsoleColor.White;
    }

    public Menu(string filepath, 
                ConsoleColor foregroundColor, 
                ConsoleColor backgroundColor, 
                ConsoleColor newActiveItemForegroundColor, 
                ConsoleColor newActiveItemBackgroundColor) 
    : base(filepath, foregroundColor, backgroundColor)
    {
        menuItems = new List<MenuItem>();
        activeMenuItemID = 0;
        activeItemForegroundColor = newActiveItemForegroundColor;
        activeItemBackgroundColor = newActiveItemBackgroundColor;
    }

    public void AddMenuItem(MenuItem newMenuItem)
    {
        menuItems.Add(newMenuItem);
    }

    public void SelectNextItem()
    {
        if (activeMenuItemID == menuItems.Count - 1)
        {
            activeMenuItemID = 0;
        }
        else
        {
            activeMenuItemID++;
        }
    }

    public void SelectPreviousItem()
    {
        if(activeMenuItemID == 0)
        {
            activeMenuItemID = menuItems.Count - 1;
        }
        else
        {
            activeMenuItemID--;
        }
    }

    public bool DidActiveMenuItemChange()
    {
        return activeMenuItemID != previousActiveMenuItemID;
    }

    public void ActivateMenuItem(Game game)
    {
        menuItems[activeMenuItemID].Activate(game);
    }

    public override void Draw()
    {
        base.Draw();

        for(int i = 0; i < menuItems.Count; i++)
        {
            if(i == activeMenuItemID)
            {
                Console.ForegroundColor = activeItemForegroundColor;
                Console.BackgroundColor = activeItemBackgroundColor;
            }
            else
            {
                Console.ForegroundColor = foregroundColor; //als het niet op het actievemenuitem is zetten we het op de default kleuren
                Console.BackgroundColor = backgroundColor;
            }
            menuItems[i].Draw();
        }

        previousActiveMenuItemID = activeMenuItemID;
    }
}
