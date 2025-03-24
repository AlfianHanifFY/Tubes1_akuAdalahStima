using System;
using System.Drawing;
using System.Collections.Generic;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class NyampahBot : Bot
{
    private Dictionary<int, ScannedBotInfo> scannedBots = new Dictionary<int, ScannedBotInfo>();
    private bool isScanning = false;
    private const double MOVE_DISTANCE = 50;

    private Random random = new Random();

    static void Main()
    {
        new NyampahBot().Start();
    }

    NyampahBot() : base(BotInfo.FromFile("NyampahBot.json")) { }

    public override void Run()
    {
        BodyColor = Color.Pink;
        TurretColor = Color.Pink;
        RadarColor = Color.Yellow;
        BulletColor = Color.Black;
        ScanColor = Color.Pink;

        while (IsRunning)
        {
            StartNewScanCycle();

            TurnRadarRight(360);

            if (RadarTurnRemaining == 0)
            {
                FireAtWeakestTarget();
            }

            FinishScanCycle();
        }
    }

    public override void OnRoundStarted(RoundStartedEvent e)
    {
        Forward(10);
        scannedBots.Clear();
    }

    private void StartNewScanCycle()
    {
        isScanning = true;
        scannedBots.Clear();
    }

    private void FinishScanCycle()
    {
        isScanning = false;
        scannedBots.Clear();
    }

    private void Move()
    {
        double moveDistance = random.NextDouble() * MOVE_DISTANCE + 20;
        SetForward(moveDistance);
        WaitFor(new TurnCompleteCondition(this));

    }

    private void FireAtWeakestTarget()
    {
        int weakestBotId = -1;
        double lowestEnergy = double.MaxValue;

        foreach (var entry in scannedBots)
        {
            if (entry.Value.Energy < lowestEnergy)
            {
                lowestEnergy = entry.Value.Energy;
                weakestBotId = entry.Key;
            }
        }


        if (weakestBotId != -1)
        {
            var target = scannedBots[weakestBotId];

            TurnLeft(target.Bearing);

            while (GunTurnRemaining != 0) { }

            SmartFire(target.Distance);

            Move();

        }
    }


    private void SmartFire(double distance)
    {
        double power;
        if (distance > 300)
        {
            return;
        }
        if (distance < 100)
            power = 3;
        else if (distance < 200)
            power = 2;
        else
            power = 1;

        Fire(power);
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {

        if (isScanning)
        {
            double distance = DistanceTo(e.X, e.Y);
            double bearing = BearingTo(e.X, e.Y);

            scannedBots[e.ScannedBotId] = new ScannedBotInfo
            {
                Distance = distance,
                Bearing = bearing,
                Energy = e.Energy,
            };
        }
    }

    public override void OnHitWall(HitWallEvent e)
    {
        Back(30);
        TurnRight(180);
        SetForward(30);

    }

    public override void OnHitBot(HitBotEvent e)
    {
        Fire(3);
    }

    private class ScannedBotInfo
    {
        public double Distance { get; set; }
        public double Bearing { get; set; }
        public double Energy { get; set; }
    }
}

public class TurnCompleteCondition : Condition
{
    private readonly Bot bot;

    public TurnCompleteCondition(Bot bot)
    {
        this.bot = bot;
    }

    public override bool Test()
    {
        return bot.TurnRemaining == 0;
    }
}