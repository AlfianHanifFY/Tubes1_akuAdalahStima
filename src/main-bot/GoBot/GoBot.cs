using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class GoBot : Bot
{
    static void Main(string[] args)
    {
        new GoBot().Start();
    }

    GoBot() : base(BotInfo.FromFile("GoBot.json")) { }

    int targetId = -1;
    int lastId = -1;
    double targetX;
    double targetY;
    int x = 1;

    public override void Run()
    {
        /* Customize bot colors, read the documentation for more information */
        BodyColor = Color.Gray;
        AdjustRadarForGunTurn = true;
        AdjustGunForBodyTurn = false;
        MaxRadarTurnRate = 100;

        while (IsRunning)
        {
            if (targetId == -1)
            {
                // Jika belum ada target, terus putar radar untuk mencari musuh
                SetTurnRadarRight(45);
            }
            else
            {
                // Kunci radar pada target
                SetTurnRadarRight((BearingTo(targetX, targetY) - RadarDirection) * x);

                // Kejar target
                TurnToFaceTarget(targetX, targetY);
                SetForward(DistanceTo(targetX, targetY) - 50);
            }
            Go();
        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        if (targetId == -1 || targetId == lastId)
        {
            targetId = e.ScannedBotId;
            Console.WriteLine("Target ditemukan: " + targetId);
        }

        if (e.ScannedBotId == targetId)
        {
            targetX = e.X;
            targetY = e.Y;

            // Menembak terus-menerus
            SmartFire(DistanceTo(targetX, targetY));
            x *= 1;
        }
        Rescan();
    }

    public override void OnBotDeath(BotDeathEvent e)
    {
        lastId = targetId;
        if (e.VictimId == targetId)
        {
            targetId = -1;
            Console.WriteLine("Target hancur, mencari target baru...");
        }
    }

    private void TurnToFaceTarget(double x, double y)
    {
        SetTurnLeft(BearingTo(x, y));
    }

    private void SmartFire(double distance)
    {
        double power;
        if (Energy > 40)
        {
            power = 3;
        }
        else if (Energy > 10)
        {
            power = 2;
        }
        else if (Energy > 5)
        {
            power = 1;
        }
        else
        {
            power = 0.1;
        }

        if (distance < 200)
        {
            Fire(power);
        }
    }
}