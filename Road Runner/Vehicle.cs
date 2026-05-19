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

    public Vehicle(VehicleType newType, int newX, int newY, ConsoleColor newColor, string newSymbol, double newXSpeed, double newYSpeed) : base(newX, newY, newSymbol, newColor)
    {
        type = newType;
        xSpeed = newXSpeed;
        ySpeed = newYSpeed;
    }

    public void Move(double dx, double dy, int screenWidth, int screenHeight)
    {
        xPos += dx; // new position = old position + step
        yPos += dy;

        // moving left to right: disappears at the right border and reappears at the left border
        if (xPos >= screenWidth - 1 - symbol.Length)
        {
            xPos = 1;
        }

        // moving right to left: disappears at the left border and reappears at the right border
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
