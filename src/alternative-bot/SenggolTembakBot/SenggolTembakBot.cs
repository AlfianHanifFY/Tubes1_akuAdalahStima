using System;
using System.Drawing;
using System.Collections.Generic;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class SenggolTembakBot : Bot
{
    private Dictionary<int, ScannedBotInfo> scannedBots = new Dictionary<int, ScannedBotInfo>();
    private bool isScanning = false;
    private const double MOVE_DISTANCE = 50;
    
    private Random random = new Random();
    
    static void Main()
    {
        new SenggolTembakBot().Start();
    }
    
    SenggolTembakBot() : base(BotInfo.FromFile("SenggolTembakBot.json")) { }
    
public override void Run()
{
    BodyColor = Color.Red;
    TurretColor = Color.Black;
    RadarColor = Color.Orange;
    BulletColor = Color.Yellow;
    ScanColor = Color.Yellow;

    while (IsRunning)
    {
        StartNewScanCycle();
        
        TurnRadarRight(360);
        
        if (RadarTurnRemaining == 0)
        {
            FireAtClosestTarget();
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
        double moveDistance = random.NextDouble() * MOVE_DISTANCE + 20; // Jarak antara 20-70
        SetForward(moveDistance);
        WaitFor(new TurnCompleteCondition(this));

    }
    
    private void FireAtClosestTarget()
    {
        int closestBotId = -1;
        double closestDistance = double.MaxValue;
        
        foreach (var entry in scannedBots)
        {
            if (entry.Value.Distance < closestDistance)
            {
                closestDistance = entry.Value.Distance;
                closestBotId = entry.Key;
            }
        }
        
        if (closestBotId != -1)
        {
            var target = scannedBots[closestBotId];
            
            TurnLeft(target.Bearing);
            
            while (GunTurnRemaining != 0) { }
            
            SmartFire(target.Distance);
            
            Move();

        }
    }


    private void SmartFire(double distance)
    {
        double power;
        if (distance > 300){
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
            double distance = DistanceTo(e.X,e.Y);
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