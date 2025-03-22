using System;
using System.Drawing;
using System.Collections.Generic;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class SenggolTembakBot : Bot
{
    private Dictionary<int, ScannedBotInfo> scannedBots = new Dictionary<int, ScannedBotInfo>();
    
    private bool isScanning = false;
    private int remainingBots = 3;
    private const double FIRE_POWER = 3;
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
            SetTurnRadarRight(45);
            WaitFor(new TurnCompleteCondition(this));
            
            
            if (scannedBots.Count == remainingBots){
                Rescan();
            }
            FireAtClosestTarget();
            FinishScanCycle();
            
            MoveRandomly();
                        
        }
    }
    
    public override void OnBotDeath(BotDeathEvent e){
        remainingBots--;
    }

    private void StartNewScanCycle()
    {
        isScanning = true;
        scannedBots.Clear();
    }
    
    private void FinishScanCycle()
    {
        isScanning = false;
    }
    
    private void MoveRandomly()
    {

        
        if (random.NextDouble() < 0.4)
        {
            double turnAngle = random.NextDouble() * 180 - 90;
            TurnRight(turnAngle);

        }
        
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
            

            
            double gunTurn = NormalizeAngle(target.Bearing - GunDirection);
            while (GunTurnRemaining > 0){}
            SetTurnGunRight(gunTurn);
            // while (GunTurnRemaining > 0){
            //     SetForward(0);
            //     SetBack(0);  
            //     WaitFor(new TurnCompleteCondition(this));
            // }


            Fire(CalculateFirePower(target.Distance));

        }
    }


    private double CalculateFirePower(double distance)
    {
        if (distance < 100)
            return FIRE_POWER;
        else if (distance < 200)
            return FIRE_POWER * 0.8;
        else
            return FIRE_POWER * 0.5;
    }
    
    private double NormalizeAngle(double angle)
    {
        while (angle > 180)
            angle -= 360;
        while (angle < -180)
            angle += 360;
        return angle;
    }
    
    public override void OnScannedBot(ScannedBotEvent e)
    {

        if (isScanning)
        {
            double distance = DistanceTo(e.X,e.Y);
            double absoluteBearing = Math.Atan2(e.Y - Y, e.X - X) * (180 / Math.PI);

            scannedBots[e.ScannedBotId] = new ScannedBotInfo
            {
                Distance = distance,
                Bearing = NormalizeAngle(absoluteBearing),
                Energy = e.Energy,
                LastSeen = 1
            };
        }
    }
    
    // Ketika bot kita menabrak tembok
    public override void OnHitWall(HitWallEvent e)
    {
        SetBack(30);
        WaitFor(new TurnCompleteCondition(this));
        SetTurnRight(180);
        WaitFor(new TurnCompleteCondition(this));

    }
    
    
    private class ScannedBotInfo
    {
        public double Distance { get; set; }
        public double Bearing { get; set; }
        public double Energy { get; set; }
        public double LastSeen { get; set; }
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
        // turn is complete when the remainder of the turn is zero
        return bot.TurnRemaining == 0;
    }
}