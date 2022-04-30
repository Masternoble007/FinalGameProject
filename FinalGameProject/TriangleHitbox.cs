using System;
using System.Collections.Generic;
using System.Text;

namespace FinalGameProject
{
    public class TriangleHitbox
    {
        public double x1;
        public double x2;
        public double x3;
        public double y1;
        public double y2;
        public double y3;

        public TriangleHitbox(double x1, double x2, double x3, double y1, double y2, double y3)
        {
            this.x1 = x1;
            this.x2 = x2;
            this.x3 = x3;
            this.y1 = y1;
            this.y2 = y2;
            this.y3 = y3;
        }
    }
}
