﻿using System;
using OpenTK;

namespace RaytraceEngine.Objects.Lights
{
    public abstract class MultiSampleLight : PointLight
    {
        public override Vector3 Position
        {
            get { return position; }
            set
            {
                position = value;
                InitSamples();
            }
        }

        protected int uniqSamples = 128;
        protected Vector3[] allPoints;
        protected Vector3 position;

        protected MultiSampleLight(int uniqSamples)
        {
            this.uniqSamples = uniqSamples;
            allPoints = new Vector3[uniqSamples];
        }
        
        protected abstract void InitSamples();
        
        public override Vector3[] GetPoints(uint maxSamples, bool rng)
        {
            int size = 1 + (int)Math.Abs(maxSamples - 1);
            Vector3[] res = new Vector3[size];
            res[size - 1] = position;
            if (rng)
                for (int i = 0; i < size - 1; i++)
                    res[i] = allPoints[RMath.ThreadLocalRandom.Instance.Next(0, uniqSamples - 1)];
            else
                for (int i = 0; i < size - 1; i++)
                    res[i] = allPoints[i % allPoints.Length];
            return res;
        }
    }
    
    public class SphereAreaLight : MultiSampleLight
    {
        protected float radius;
        public float Radius
        {
            get { return radius; }
            set
            {
                radius = value;
                InitSamples();
            }
        }

        public SphereAreaLight(int uniqSamples) : base(uniqSamples) { }
        
        protected override void InitSamples()
        {
            int a = 0;
            int steps = (int)Math.Sqrt(uniqSamples);
            for (int i = 0; i < steps; i++)
            for (int j = 0; j < steps; j++)
                allPoints[a++] = position + RMath.RndUnitStratified(steps, i, j) * radius;
            
        }
    }
}