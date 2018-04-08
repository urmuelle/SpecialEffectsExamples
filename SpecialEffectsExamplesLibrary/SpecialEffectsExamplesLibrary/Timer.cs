// <copyright file="Timer.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>

namespace SpecialEffectsExamplesLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Timer
    {
        public static List<Timer> Timers = new List<Timer>();

        private bool isRunning;
        private float time;

        public Timer()
        {
            Timers.Add(this);
            Stop();
        }

        public float Time
        {
            get { return time; }
            set { time = value; }
        }

        public static void UpdateAll(float elapsedTime)
        {
            foreach (var timer in Timers)
            {
                timer.Update(elapsedTime);
            }
        }

        public void Start()
        {
            isRunning = true;
        }

        public void Pause()
        {
            isRunning = false;
        }

        public void Stop()
        {
            Pause();
            time = 0;
        }

        public void Begin()
        {
            Stop();
            Start();
        }

        public void BeginWithDelay(float fDelay)
        {
            time = -fDelay;
            Start();
        }

        public bool IsRunning()
        {
            return isRunning;
        }

        public void Update(float fElapsedTime)
        {
            if (isRunning)
            {
                time += fElapsedTime;
            }
        }
    }
}
