﻿using System;
using System.Collections.Generic;
using Engine;
using Engine.TemplateCode;
using OpenTK;

namespace RaytraceEngine
{
    public class Raytracer
    {
        private int winWidth;
        private int winHeight;

        public Raytracer(int winWidth, int winHeight)
        {
            this.winWidth = winWidth;
            this.winHeight = winHeight;
        }

        public static List<Tuple<Ray, RayHit>> Rays = new List<Tuple<Ray, RayHit>>();
        
        public void Render(Surface surface, RayScene scene)
        {
            Rays.Clear();
            var projectionPlane = scene.CurrentCamera.GetNearClippingPlane();

            int ri = 0;
            for (int x = 0; x < winWidth; ++x)
            for (int y = 0; y < winHeight; y++) {
                Ray ray = RayFromPixel(projectionPlane, scene.CurrentCamera, x, y);
                RayHit hit;
                foreach (var primitive in scene.Primitives) {
                    bool isHit = primitive.CheckHit(ray, out hit);
                    if (isHit) {
                        surface.Plot(x, y, 255);
                        
                        if (y == winHeight >> 1) {
                            if(ri % 8 == 0)Rays.Add(new Tuple<Ray, RayHit>(ray, hit));
                            ++ri;
                        }
                    }
                }
            }
        }

        Ray RayFromPixel(FinitePlane projectionPlane, Camera camera, int x, int y)
        {
            Vector3 onPlane = ((float)x / winWidth) * projectionPlane.NHor + ((float) y / winHeight) * projectionPlane.NVert + projectionPlane.Origin;
            onPlane.Normalize();
            
            Ray ret = new Ray();
            ret.Direction = onPlane;
            ret.Origin = camera.Position;
            return ret;
        }
    }
}