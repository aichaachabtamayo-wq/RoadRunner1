// Screen.cs
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
        StreamReader streamReader = null; // create an empty reader, no file opened yet
        try
        {
            streamReader = new StreamReader(filepath); // open the file at the given path
            text = streamReader.ReadToEnd(); // read the entire file content
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            if (streamReader != null)
                streamReader.Close(); // always close the file
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
            if (streamReader != null)
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
