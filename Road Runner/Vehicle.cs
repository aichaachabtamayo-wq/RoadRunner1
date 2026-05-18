using System;

namespace TemplateVSCode;

public enum VehicleType
{
    SlowCar,
    FastCar,
    SlowTruck
}
public class Vehicle : Sprite
{
    protected VehicleType type;
    protected double xSpeed;
    protected double ySpeed;
    public VehicleType Type
    {
        get { return type; }
        set { type = value; }
    }
    public double XSpeed
    {
        get { return xSpeed; }
        set { xSpeed = value; }
    }
    public double YSpeed
    {
        get { return ySpeed; }
        set { ySpeed = value; }
    }


    public Vehicle(VehicleType newType,int newX, int newY, ConsoleColor newColor, string newSymbol, double newXSpeed, double newYSpeed) : base(newX, newY, newSymbol, newColor)
    {
        type = newType;
        xSpeed = newXSpeed;
        ySpeed = newYSpeed;
    }

    public void Move(double dx, double dy, int screenWidth, int screenHeight)
    {
        xPos += dx; // nieuwe positie = oude positie + stapje
        yPos += dy;

        // van links naar rechts → verdwijnt aan de rechter broder en komt terug aan de linker border
        if (xPos >= screenWidth - 1 - symbol.Length)
        {
            xPos = 1;
        }

        // van rechts naar links → verdwijnt aan de linker broder en komt terug aan de rechter border
        else if (xPos <= 1)
        {
            xPos = screenWidth - 1 - symbol.Length;
        }
    }
    public override void Update(double dt, int screenWidth, int screenHeight)
    {
        Move(xSpeed * dt, ySpeed * dt, screenWidth, screenHeight);
    }

}
