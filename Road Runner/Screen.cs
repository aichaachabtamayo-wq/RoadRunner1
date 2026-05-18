using System;

namespace TemplateVSCode;

public class Screen
{
    protected string text;
    protected ConsoleColor foregroundColor;
    protected ConsoleColor backgroundColor;
    public string Text
    {
        get { return text; }
        set { text = value; }
    }
    public Screen(string filepath)
    {
        foregroundColor = ConsoleColor.White;
        backgroundColor = ConsoleColor.Black;   
        StreamReader streamReader = null; 
        try
        {
           streamReader = new StreamReader(filepath);

           text = streamReader.ReadToEnd();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            streamReader.Close();
        }
    }

     public Screen(string filepath, ConsoleColor newForegroundColor, ConsoleColor newBackgroundColor)
    {
        foregroundColor = newForegroundColor;
        backgroundColor = newBackgroundColor;  
        StreamReader streamReader = null; 
        try
        {
           streamReader = new StreamReader(filepath);

           text = streamReader.ReadToEnd();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            streamReader.Close();
        }
    }

    public virtual void Draw()
    {
       Console.SetCursorPosition(0, 0);
       Console.ForegroundColor = foregroundColor;
       Console.BackgroundColor = backgroundColor;
       Console.WriteLine(text); 
    }
}
