﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using static System.Math;

namespace DDsControlCollection
{
    public enum ClockStyle
    {
        Analog,
        Numeric,
        Binary,
        Word
    }

    public class SimpleClock : Control
    {
        public System.Timers.Timer ClockTimer { get; private set; }
        
        public SimpleClock()
        {
            _framePen = new Pen(Color.Black, 4);
            _secondPen = new Pen(Color.Red, 2);
            _minutePen =
                _hourPen = new Pen(Color.Black, 3);
            _middlePointBrush = new SolidBrush(Color.Black);
            _smoothingMode = SmoothingMode.AntiAlias;
            _showFrame =
                _showMiddlePoint =
                _showSecondNeedle =
                _showMinuteNeedle =
                _showHourNeedle = true;
            Size = new Size(100, 100);

            ClockTimer = new System.Timers.Timer(1000);
            ClockTimer.Elapsed += (s, e) =>
            {
                Invalidate();
            };
            ClockTimer.Start();
        }

        ClockStyle _style;
        public ClockStyle Style
        {
            get { return _style; }
            set
            {
                _style = value;

                Invalidate();
            }
        }

        #region Analog
        // Frame
        Pen _framePen;
        public Color FrameColor
        {
            get { return _framePen.Color; }
            set
            {
                _framePen.Color = value;

                Invalidate();
            }
        }
        public float FrameWidth
        {
            get { return _framePen.Width; }
            set
            {
                _framePen.Width = value;

                Invalidate();
            }
        }

        bool _showFrame;
        public bool ShowFrame
        {
            get { return _showFrame; }
            set
            {
                _showFrame = value;

                Invalidate();
            }
        }

        // Second needle
        Pen _secondPen;
        public Color SecondNeedleColor
        {
            get { return _secondPen.Color; }
            set
            {
                _secondPen.Color = value;

                Invalidate();
            }
        }
        public float SecondNeedleWidth
        {
            get { return _secondPen.Width; }
            set
            {
                _secondPen.Width = value;

                Invalidate();
            }
        }

        bool _showSecondNeedle;
        public bool ShowSecondNeedle
        {
            get { return _showSecondNeedle; }
            set
            {
                _showSecondNeedle = value;

                Invalidate();
            }
        }

        // Minute needle
        Pen _minutePen;
        public Color MinuteNeedleColor
        {
            get { return _minutePen.Color; }
            set
            {
                _minutePen.Color = value;

                Invalidate();
            }
        }
        public float MinuteNeedleWidth
        {
            get { return _minutePen.Width; }
            set
            {
                _minutePen.Width = value;

                Invalidate();
            }
        }

        bool _showMinuteNeedle;
        public bool ShowMinuteNeedle
        {
            get { return _showMinuteNeedle; }
            set
            {
                _showMinuteNeedle = value;

                Invalidate();
            }
        }

        // Hour needle
        Pen _hourPen;
        public Color HourNeedlePen
        {
            get { return _hourPen.Color; }
            set
            {
                _hourPen.Color = value;

                Invalidate();
            }
        }
        public float HourNeedleWidth
        {
            get { return _hourPen.Width; }
            set
            {
                _hourPen.Width = value;

                Invalidate();
            }
        }

        bool _showHourNeedle;
        public bool ShowHourNeedle
        {
            get { return _showHourNeedle; }
            set
            {
                _showHourNeedle = value;

                Invalidate();
            }
        }

        // Middle point
        SolidBrush _middlePointBrush;
        public Color MiddlePointColor
        {
            get { return _middlePointBrush.Color; }
            set
            {
                _middlePointBrush = new SolidBrush(value);

                Invalidate();
            }
        }

        bool _showMiddlePoint;
        public bool ShowMiddlePoint
        {
            get { return _showMiddlePoint; }
            set
            {
                _showMiddlePoint = value;

                Invalidate();
            }
        }
        #endregion

        #region Numeric

        #endregion

        //TODO: TimeZoneOffset

        DateTime _time;
        [Browsable(false)]
        [ReadOnly(true)]
        public DateTime Time
        {
            get { return _time; }
            set
            {
                _time = value;

                ClockTimer.Enabled = false;

                Invalidate();
            }
        }

        SmoothingMode _smoothingMode;
        public SmoothingMode SmoothingMode
        {
            get { return _smoothingMode; }
            set
            {
                _smoothingMode = value;

                Invalidate();
            }
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = _smoothingMode;
            int w = Width;
            int h = Height;

            if (ClockTimer.Enabled)
                _time = DateTime.Now;

            switch (_style)
            {
                case ClockStyle.Analog:
                    {
                        float sn = (_time.Second - 15) * 0.1047f;
                        float mn = (_time.Minute - 15) * 0.1047f;
                        float hn = (_time.Hour - 3) * 0.5235f;
                        // Scaling (Radius)
                        float rw = w * 0.4f;
                        float rh = h * 0.4f;
                        float rwh = w * 0.3f;
                        float rhh = h * 0.3f;
                        // Translation
                        float tw = w / 2;
                        float th = h / 2;

                        // Frame
                        if (_showFrame)
                            e.Graphics.DrawEllipse(_framePen,
                                w / 20, h / 20,
                                w - (w / 10), h - (h / 10));
            
                        // Seconds
                        if (_showSecondNeedle)
                            e.Graphics.DrawLine(_secondPen,
                                tw, th,
                                (float)(rw * Cos(sn) + tw),
                                (float)(rh * Sin(sn) + th));

                        // Minutes
                        if (_showMinuteNeedle)
                            e.Graphics.DrawLine(_minutePen,
                                tw, th,
                                (float)(rw * Cos(mn) + tw),
                                (float)(rh * Sin(mn) + th));

                        // Hours
                        if (_showHourNeedle)
                            e.Graphics.DrawLine(_hourPen,
                                tw, th,
                                (float)(rwh * Cos(hn) + tw),
                                (float)(rhh * Sin(hn) + th));

                        // Middle point
                        if (_showMiddlePoint)
                            e.Graphics.FillEllipse(_middlePointBrush,
                                tw - (w / 30), th - (h / 30),
                                w / 15, h / 15);
                    }
                    break;
                case ClockStyle.Numeric:
                    {

                    }
                    break;
                case ClockStyle.Binary:
                    {

                    }
                    break;
            }
        }
    }
}
