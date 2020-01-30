using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RingClock
{
    public sealed class Arc : Shape
    {
        public Point Center
        {
            get { return (Point)GetValue(CenterProperty); }
            set { SetValue(CenterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Center.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CenterProperty =
            DependencyProperty.Register("Center", typeof(Point), typeof(Arc)
            , new FrameworkPropertyMetadata(new Point(0, 0), FrameworkPropertyMetadataOptions.AffectsRender));


        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartAngle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register("StartAngle", typeof(double), typeof(Arc)
            , new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public double EndAngle
        {
            get { return (double)GetValue(EndAngleProperty); }
            set { SetValue(EndAngleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EndAngle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EndAngleProperty =
            DependencyProperty.Register("EndAngle", typeof(double), typeof(Arc)
            , new FrameworkPropertyMetadata(Math.PI / 2.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Radius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(Arc)
            , new FrameworkPropertyMetadata(10.0, FrameworkPropertyMetadataOptions.AffectsRender));



        public bool SmallAngle
        {
            get { return (bool)GetValue(SmallAngleProperty); }
            set { SetValue(SmallAngleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SmallAngle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SmallAngleProperty =
            DependencyProperty.Register("SmallAngle", typeof(bool), typeof(Arc)
            , new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));


        static Arc()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Arc), new FrameworkPropertyMetadata(typeof(Arc)));
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                var a0 = StartAngle;
                var a1 = EndAngle;

                if (a1 < a0)
                    a1 += Math.PI * 2;

                bool large = false;
                if (Math.Abs(a1 - a0) > Math.PI)
                    large = true;

                var p0 = new Point(0, 0) + new Vector(Math.Cos(a0), Math.Sin(a0)) * Radius;
                var p1 = new Point(0, 0) + new Vector(Math.Cos(a1), Math.Sin(a1)) * Radius;

                List<PathSegment> segments = new List<PathSegment>(1);
                segments.Add(new ArcSegment(p1, new Size(Radius, Radius), 0.0, large, SweepDirection.Clockwise, true));

                List<PathFigure> figures = new List<PathFigure>(1);
                PathFigure pf = new PathFigure(p0, segments, true);
                pf.IsClosed = false;
                figures.Add(pf);

                Geometry g = new PathGeometry(figures, FillRule.EvenOdd, null);
                return g;
            }
        }
    }
}