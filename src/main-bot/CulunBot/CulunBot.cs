using System;
using System.Drawing;
using System.Collections.Generic;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class CulunBot : Bot
{
    /* A bot that drives forward and backward, and fires a bullet */
    static void Main(string[] args)
    {
        new CulunBot().Start();
    }

    CulunBot() : base(BotInfo.FromFile("CulunBot.json")) { }


    double enemyX, enemyY, targetX, targetY;
    int savedEnemies;
    bool shouldMove;
    HashSet<int> detectedBots = new();

    public override void Run()
    {
        /* Customize bot colors, read the documentation for more information */
        BodyColor = Color.Pink;
        TurretColor = Color.Pink;
        RadarColor = Color.Yellow;
        BulletColor = Color.Black;
        ScanColor = Color.Pink;

        AdjustGunForBodyTurn = true;
        shouldMove = true;
        while (IsRunning)
        {
            SetTurnGunRight(Double.PositiveInfinity);
            if (shouldMove)
            {
                ResetTracking();
                EscapeFromEnemies();
                shouldMove = false;
            }
            else
            {
                AttackEnemies();
            }
        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        if (!detectedBots.Contains(e.ScannedBotId))
        {
            detectedBots.Add(e.ScannedBotId);
            enemyX += e.X;
            enemyY += e.Y;
            savedEnemies++;
        }
        var distance = DistanceTo(e.X, e.Y);

        if (distance < 100)
        {
            shouldMove = true;
        }
        SmartFire(distance);
        Rescan();

    }

    public override void OnBotDeath(BotDeathEvent e)
    {
        detectedBots.Remove(e.VictimId);
    }

    private void EscapeFromEnemies()
    {
        MaxSpeed = 100;
        if (savedEnemies > 0)
        {
            targetX = 800 - (enemyX / savedEnemies);
            targetY = 600 - (enemyY / savedEnemies);
            TurnToFaceTarget(targetX, targetY);
            Forward(DistanceTo(targetX, targetY) + 5);
        }
    }

    private void AttackEnemies()
    {
        SetTurnLeft(5_000);
        MaxSpeed = 5;
        Forward(5_000);
    }

    private void ResetTracking()
    {
        enemyX = 0;
        enemyY = 0;
        savedEnemies = 0;
        detectedBots = new HashSet<int>();
    }

    private void SmartFire(double distance)
    {
        double power;

        if (Energy > 50)
            power = 3;
        else if (Energy > 15)
            power = 2;
        else
            power = 1;

        Fire(power);
    }

    private void TurnToFaceTarget(double x, double y)
    {
        TurnLeft(BearingTo(x, y));
    }

    public override void OnHitByBullet(HitByBulletEvent e)
    {
        EscapeFromEnemies();
    }

    public override void OnHitBot(HitBotEvent e)
    {
        shouldMove = true;
    }

    public override void OnHitWall(HitWallEvent e)
    {
        SetTurnRight(20_000);
        MaxSpeed = 5;
        Back(50);
    }
}
