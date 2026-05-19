using System;
using System.Drawing;

namespace TemplateVSCode;

public class Road
{
    protected int width;
    protected List<RoadElement> rows; // list of road elements representing each row of the map
    protected List<RoadElement> buffer; // buffer list used for the endless scrolling mechanic
    protected List<Vehicle> vehicles; // list that holds all vehicle objects on the road
    protected Random rndGen = new Random();

    public int Width
    {
        get { return width; }
        set { width = value; }
    }
    public List<RoadElement> Rows
    {
        get { return rows; }
        set { rows = value; }
    }
    public List<RoadElement> Buffer
    {
        get { return buffer; }
        set { buffer = value; }
    }
    public List<Vehicle> Vehicles
    {
        get { return vehicles; }
        set { vehicles = value; }
    }
    public Random RndGen
    {
        get { return rndGen; }
        set { rndGen = value; }
    }

    public Road(int newWidth)
    {
        width = newWidth;
        rows = new List<RoadElement>();
        {
            rows.Add(new RoadElement(RoadElementType.Border, "#", ConsoleColor.White, ConsoleColor.Black));   // top border
            rows.Add(new RoadElement(RoadElementType.Road, "-", ConsoleColor.Gray, ConsoleColor.Black));      // lane 10
            rows.Add(new RoadElement(RoadElementType.Road, "-", ConsoleColor.Gray, ConsoleColor.Black));      // lane 9
            rows.Add(new RoadElement(RoadElementType.Road, "-", ConsoleColor.Gray, ConsoleColor.Black));      // lane 8
            rows.Add(new RoadElement(RoadElementType.Grass, ":", ConsoleColor.Green, ConsoleColor.Black));    // safe zone
            rows.Add(new RoadElement(RoadElementType.Road, "-", ConsoleColor.Gray, ConsoleColor.Black));      // lane 7
            rows.Add(new RoadElement(RoadElementType.Road, "-", ConsoleColor.Gray, ConsoleColor.Black));      // lane 6
            rows.Add(new RoadElement(RoadElementType.Road, "-", ConsoleColor.Gray, ConsoleColor.Black));      // lane 5
            rows.Add(new RoadElement(RoadElementType.Grass, ":", ConsoleColor.Green, ConsoleColor.Black));    // safe zone
            rows.Add(new RoadElement(RoadElementType.Road, "-", ConsoleColor.Gray, ConsoleColor.Black));      // lane 4
            rows.Add(new RoadElement(RoadElementType.Road, "-", ConsoleColor.Gray, ConsoleColor.Black));      // lane 3
            rows.Add(new RoadElement(RoadElementType.Grass, ":", ConsoleColor.Green, ConsoleColor.Black));    // safe zone
            rows.Add(new RoadElement(RoadElementType.Road, "-", ConsoleColor.Gray, ConsoleColor.Black));      // lane 2
            rows.Add(new RoadElement(RoadElementType.Grass, ":", ConsoleColor.Green, ConsoleColor.Black));    // safe zone
            rows.Add(new RoadElement(RoadElementType.Road, "-", ConsoleColor.Gray, ConsoleColor.Black));      // lane 1
            rows.Add(new RoadElement(RoadElementType.Grass, ":", ConsoleColor.Green, ConsoleColor.Black));    // player start position
            rows.Add(new RoadElement(RoadElementType.Grass, ":", ConsoleColor.Green, ConsoleColor.Black));    // player start position
            rows.Add(new RoadElement(RoadElementType.Grass, ":", ConsoleColor.Green, ConsoleColor.Black));    // player start position
            rows.Add(new RoadElement(RoadElementType.Grass, ":", ConsoleColor.Green, ConsoleColor.Black));    // player start position
            rows.Add(new RoadElement(RoadElementType.Border, "#", ConsoleColor.White, ConsoleColor.Black));   // bottom border
        }

        buffer = new List<RoadElement>();
        for (int i = 1; i < rows.Count - 1; i++) // add all rows except the first and last border to the buffer
        {
            buffer.Add(rows[i]);
        }

        vehicles = new List<Vehicle>();
        SpawnVehicles();
    }

    public void Draw(int xOffset, int yOffset)
    {
        for (int y = 0; y < rows.Count; y++) // outer loop goes through all rows
        {
            for (int x = 0; x < width; x++) // inner loop goes through all cells in that row
            {
                if (x == 0 || x == width - 1) // draw a border # on the left and right side
                {
                    Console.SetCursorPosition(x + xOffset, y + yOffset);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("#");
                }
                else
                {
                    rows[y].Draw(x + xOffset, y + yOffset); // otherwise draw the road element at position x, y
                }
            }
            Console.WriteLine();
        }
    }

    public void SpawnVehicles()
    {
        for (int rowIndex = 1; rowIndex < rows.Count - 1; rowIndex++) // loop through all rows except the borders
        {
            if (rows[rowIndex].Type == RoadElementType.Road) // check if the row is of enum type Road
            {
                SpawnVehiclesOnRow(rowIndex);
            }
        }
    }

    public void Scroll()
    {
        // remove the row just above the bottom border
        RoadElement lastRow = rows[rows.Count - 2];
        buffer.Add(lastRow);
        rows.RemoveAt(rows.Count - 2);

        int index = rndGen.Next(0, buffer.Count); // pick a random index between 0 and the number of rows in the buffer
        rows.Insert(1, buffer[index]); // insert the randomly chosen row at position 1 (just after the top border)

        foreach (Vehicle v in vehicles)
        {
            v.YPos++; // shift all vehicles down to match the scrolling map
        }

        // loop backwards so that when an element is removed, no elements are skipped
        for (int i = vehicles.Count - 1; i >= 0; i--)
        {
            if (vehicles[i].YPos >= rows.Count - 1) // check if the vehicle has gone below the bottom border
            {
                vehicles.RemoveAt(i); // remove it from the list
            }
        }

        if (rows[1].Type == RoadElementType.Road) // if the new top row is a road, spawn vehicles on it
        {
            SpawnVehiclesOnRow(1);
        }
    }

    public void SpawnVehiclesOnRow(int rowIndex)
    {
        double speed;
        if (rndGen.Next(0, 2) == 0) // randomly pick 0 or 1
        {
            speed = 1; // 0 = move to the right
        }
        else
        {
            speed = -1; // 1 = move to the left
        }

        int type = rndGen.Next(0, 3); // choose type OUTSIDE the loop so all vehicles on this lane have the same type

        int vehiclesAmount = rndGen.Next(3, 4); // randomly determine the number of vehicles per row

        for (int y = 0; y < vehiclesAmount; y++)
        {
            int xPosition;
            if (speed > 0)
            {
                xPosition = y * 10; // start on the left side, each vehicle starts 10 positions further
            }
            else
            {
                xPosition = width - 1 - 5 - (y * 10); // start on the right side, same but mirrored
            }

            if (type == 0)
            {
                Vehicles.Add(new Vehicle(VehicleType.SlowCar, xPosition, rowIndex, ConsoleColor.Red, "[=]", speed * 6, 0));
            }
            else if (type == 1)
            {
                Vehicles.Add(new Vehicle(VehicleType.FastCar, xPosition, rowIndex, ConsoleColor.Cyan, "{>}", speed * 8, 0));
            }
            else
            {
                Vehicles.Add(new Vehicle(VehicleType.SlowTruck, xPosition, rowIndex, ConsoleColor.Magenta, "[===]", speed * 4, 0));
            }
        }
    }
}
