using System;
using System.Drawing;

namespace TemplateVSCode;

public class Road
{
    protected int width;
    protected List<RoadElement> rows; //array van de RoadElementType(Grass, Road, Border)
    protected List<RoadElement> buffer;
    protected List<Vehicle> vehicles; //lijst die Vehicle objecten kan bijhouden
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
            rows.Add(new RoadElement(RoadElementType.Border, "#", ConsoleColor.White, ConsoleColor.Black));   // border
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
            rows.Add(new RoadElement(RoadElementType.Grass, ":", ConsoleColor.Green, ConsoleColor.Black));    // startpositie speler
            rows.Add(new RoadElement(RoadElementType.Grass, ":", ConsoleColor.Green, ConsoleColor.Black));    // startpositie speler
            rows.Add(new RoadElement(RoadElementType.Grass, ":", ConsoleColor.Green, ConsoleColor.Black));    // startpositie speler
            rows.Add(new RoadElement(RoadElementType.Grass, ":", ConsoleColor.Green, ConsoleColor.Black));    // startpositie speler
            rows.Add(new RoadElement(RoadElementType.Border, "#", ConsoleColor.White, ConsoleColor.Black));   // border
        }
        ;

        buffer = new List<RoadElement>();
        for (int i = 1; i < rows.Count - 1; i++) //voeg mijn list toe behalve de eerste en laatste border
        {
            buffer.Add(rows[i]);
        }

        vehicles = new List<Vehicle>();
        SpawnVehicles();
    }
    public void Draw(int xOffset, int yOffset)
    {
        for (int y = 0; y < rows.Count; y++) //buitenste lus gaat door alle rijen (dus 0 tot 19)
        {
            for (int x = 0; x < width; x++) //binnenste lus gaat door alle vakjes IN die rij (van 0 tot 59)
            {
                if (x == 0 || x == width - 1) //teken een rand # aan de linker en rechterkant
                {
                    Console.SetCursorPosition(x + xOffset, y + yOffset);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("#");
                }
                else
                {
                    rows[y].Draw(x + xOffset, y + yOffset); //anders, teken het RoadElement van die rij op positie x, y
                }
            }
            Console.WriteLine();
        }
    }

    public void SpawnVehicles()
    {
        for(int rowIndex = 1; rowIndex < rows.Count - 1; rowIndex++) //door alle rijen loopen behalve de borders, daarom vanaf 1 en -1
        {
            if(rows[rowIndex].Type == RoadElementType.Road)// check of de rij van enum type Road is
            {
                SpawnVehiclesOnRow(rowIndex);
            }
        }
        
        /*vehicles.Add(new Vehicle(VehicleType.SlowCar, 1, 3, ConsoleColor.Yellow, "{>}", 10, 0));
        //voegt een nieuw voertuig toe aan de lijst: vehicles.Add()
        //elk voertuig krijgt een type, startpositie, kleur, symbool en snelheid
        vehicles.Add(new Vehicle(VehicleType.SlowTruck, width - 5, 4, ConsoleColor.Magenta, "[===]", -8, 0));
        vehicles.Add(new Vehicle(VehicleType.SlowCar, 1, 5, ConsoleColor.Yellow, "{>}", 12, 0));
        vehicles.Add(new Vehicle(VehicleType.SlowCar, 1, 7, ConsoleColor.Blue, "[=]", 9, 0));
        vehicles.Add(new Vehicle(VehicleType.SlowCar, width - 3, 8, ConsoleColor.Blue, "[=]", -7, 0));
        vehicles.Add(new Vehicle(VehicleType.SlowCar, 1, 9, ConsoleColor.Yellow, "{>}", 11, 0));
        vehicles.Add(new Vehicle(VehicleType.SlowCar, 1, 11, ConsoleColor.Yellow, "{>}", 10, 0));
        vehicles.Add(new Vehicle(VehicleType.SlowTruck, width - 5, 12, ConsoleColor.Magenta, "[===]", -6, 0));
        vehicles.Add(new Vehicle(VehicleType.SlowCar, 1, 14, ConsoleColor.Blue, "[=]", 13, 0));
        vehicles.Add(new Vehicle(VehicleType.SlowCar, 1, 16, ConsoleColor.Blue, "[=]", 8, 0));*/
    }

    public void Scroll()
    {
        //om de rij net voor de onderste border te verwijderen
        RoadElement lastRow = rows[rows.Count - 2];
        buffer.Add(lastRow);
        rows.RemoveAt(rows.Count - 2);

        int index = rndGen.Next(0, buffer.Count); //willekeurig getal tussen index 0 en aantal rijen in mijn buffer
        rows.Insert(1, buffer[index]); //voeg random gekozen rij op positie 1 (index voor de border)

        foreach(Vehicle v in vehicles)
        {
            v.YPos++; //verschuif alle vehicles mee naar beneden
        }

        for(int i = vehicles.Count - 1; i >= 0; i--)
        /*omgekeerd loopen zodat wnnr je een element verwijdert uit de list, 
        je geen elementen overslaat omdat je van achter naar voor werkt*/
        {
            if(vehicles[i].YPos >= rows.Count - 1) //checken of vehicle onder de onderste border gaat
            {
                vehicles.RemoveAt(i);
            }
        }

        if(rows[1].Type == RoadElementType.Road)
        {
            SpawnVehiclesOnRow(1);
        }
    }

    public void SpawnVehiclesOnRow(int rowIndex)
    {
        double speed;
        if(rndGen.Next(0, 2) == 0) //kies een random getal: 0 of 1
        {
            speed = 1; // 0 = naar rechts
        }
        else
        {
            speed = -1; // 1 = naar links
        }
        
        int type = rndGen.Next(0, 3); //kies type BUITEN de lus, zodat alle autos op die lane hetzelfde type hebben

        int vehiclesAmount = rndGen.Next(3, 4); //random gen aanmaken om aantal vehicles per rij te bepalen

        for(int y = 0; y < vehiclesAmount; y++)
        {
            int xPosition;
            if(speed > 0)
            {
                xPosition = y * 10; // start aan de linkerkant, elke auto start 10 posities verder
            }
            else
            {
                xPosition = width - 1 - 5 - (y * 10); // start aan de rechterkant, zelfde maar rechts
            }

            if(type == 0)
            {
                Vehicles.Add(new Vehicle(VehicleType.SlowCar, xPosition, rowIndex, ConsoleColor.Red, "[=]", speed * 6, 0));
            }
            else if(type == 1)
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
