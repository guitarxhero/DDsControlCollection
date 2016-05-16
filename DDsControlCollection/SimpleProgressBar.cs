﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DDsControlCollection
{
    public enum ProgressBarTextStyle
    {
        None,
        ValueOnMaximum,
        Pourcentage,
        UserDefined
    }

    public enum MarqueeAnimation
    {
        Slide, Bouncy
    }

    /// <summary>
    /// Represent a simple ProgressBar.
    /// </summary>
    public class SimpleProgressBar : Control
    {
        // Marquee

        enum MarqueeDirection { Right, Left, Up, Down }
        MarqueeDirection _marqueeDirection;
        int _marqueeWidth; // public
        int _marqueeSpeed; // public
        int _marqueePosition; // public?

        public System.Timers.Timer MarqueeTimer { get; private set; }
        public MarqueeAnimation MarqueeAnimation { get; set; }

        // Construct

        public SimpleProgressBar()
        {
            _maximum = 100;
            _textBrush = new SolidBrush(Color.Black);
            _borderPen = new Pen(Color.DarkGray, 1);
            _style = ProgressBarStyle.Continuous;

            Step = 10;
            Font = new Font("Segoi UI", 10);
            Padding = new Padding(2);
            Size = new Size(100, 23);
            _foreColorBrush = new SolidBrush(Color.LightGreen);
            _backColorBrush = new SolidBrush(Color.WhiteSmoke);
            _textBrush = new SolidBrush(Color.Black);

            DoubleBuffered = true;

            // Marquee stuff
            _marqueeWidth = 50;
            _marqueeSpeed = 5;
            MarqueeTimer = new System.Timers.Timer(100);
            MarqueeTimer.Elapsed += (s, e) =>
            {
                switch (MarqueeAnimation)
                {
                    case MarqueeAnimation.Bouncy:
                        switch (_marqueeDirection)
                        {
                            case MarqueeDirection.Right:
                                if (_marqueePosition + _marqueeSpeed + _marqueeWidth
                                    > Width)
                                {
                                    _marqueeDirection = MarqueeDirection.Left;
                                    _marqueePosition -= _marqueeSpeed;
                                }
                                else
                                    _marqueePosition += _marqueeSpeed;
                                break;

                            case MarqueeDirection.Left:
                                if (_marqueePosition - _marqueeSpeed < Padding.Left)
                                {
                                    _marqueeDirection = MarqueeDirection.Right;
                                    _marqueePosition += _marqueeSpeed;
                                }
                                else
                                    _marqueePosition -= _marqueeSpeed;
                                break;
                        }
                        break;

                    case MarqueeAnimation.Slide:
                        if (_marqueePosition > Width)
                            _marqueePosition = -_marqueeWidth;
                        else
                            _marqueePosition += _marqueeSpeed;
                        break;
                }
                
                Invalidate();
            };
        }

        // Methods
        
        public void PerformStep()
        {
            if (_style != ProgressBarStyle.Marquee)
            {
                if (_value + Step <= _maximum)
                    Value += Step;
                else
                    Value += _maximum - _value;
            }
            else
            {
                if (_value + Step <= _maximum)
                    _value += Step;
                else
                    _value += _maximum - _value;
            }
        }

        // Properties without private variables

        [DefaultValue(10)]
        public int Step { get; set; }

        // Properties
        
        Pen _borderPen;
        [ReadOnly(true)]
        public Color BorderColor
        {
            get { return _borderPen.Color; }
            set
            {
                _borderPen.Color = value;

                Invalidate();
            }
        }
        [DefaultValue(1)]
        public float BorderThickness
        {
            get { return _borderPen.Width; }
            set
            {
                _borderPen.Width = value;

                Invalidate();
            }
        }

        SolidBrush _foreColorBrush;
        public new Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                _foreColorBrush.Color =
                    base.ForeColor =
                        value;

                Invalidate();
            }
        }

        SolidBrush _backColorBrush;
        public new Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                _backColorBrush.Color =
                    base.BackColor =
                        value;

                Invalidate();
            }
        }

        int _maximum;
        [DefaultValue(100)]
        public int Maximum
        {
            get { return _maximum; }
            set
            {
                if (value < _value)
                    throw new ArgumentOutOfRangeException("Maximum is lower than Value.");

                _maximum = value;

                Invalidate();
            }
        }

        int _minimum;
        [DefaultValue(0)]
        public int Minimum
        {
            get { return _minimum; }
            set
            {
                if (value > _value)
                    throw new ArgumentOutOfRangeException("Minimum is higher than Value.");

                _minimum = value;

                Invalidate();
            }
        }

        int _value;
        [DefaultValue(0)]
        public int Value
        {
            get { return _value; }
            set
            {
                if (value <= _maximum)
                    _value = value;
                else if (value > _maximum)
                    throw new ArgumentOutOfRangeException("Value is higher than Maximum.");
                else if (value < _minimum)
                    throw new ArgumentOutOfRangeException("Value is lower than Minimum.");

                Invalidate();
            }
        }

        ProgressBarTextStyle _textStyle;
        [DefaultValue(ProgressBarTextStyle.None)]
        public ProgressBarTextStyle TextStyle
        {
            get { return _textStyle; }
            set
            {
                _textStyle = value;

                Invalidate();
            }
        }

        string _text;
        public new string Text
        {
            get { return _text; }
            set
            {
                _text = value;

                if (string.IsNullOrWhiteSpace(_text))
                    _textStyle = ProgressBarTextStyle.None;
                else if (_textStyle != ProgressBarTextStyle.UserDefined)
                    _textStyle = ProgressBarTextStyle.UserDefined;

                Invalidate();
            }
        }

        SolidBrush _textBrush;
        public Color TextColor
        {
            get { return _textBrush.Color; }
            set
            {
                _textBrush.Color = value;

                Invalidate();
            }
        }

        Orientation _orientation;
        public Orientation BarOrientation
        {
            get { return _orientation; }
            set
            {
                _orientation = value;

                Invalidate();
            }
        }

        bool _invertOrientation;
        public bool InvertOrientation
        {
            get { return _invertOrientation; }
            set
            {
                _invertOrientation = value;

                Invalidate();
            }
        }
        public override RightToLeft RightToLeft
        {
            get
            {
                return _invertOrientation ? RightToLeft.Yes : RightToLeft.No;
            }
            set
            {
                _invertOrientation = value == RightToLeft.Yes;

                Invalidate();
            }
        }

        ProgressBarStyle _style;
        public ProgressBarStyle Style
        {
            get { return _style; }
            set
            {
                _style = value;

                switch (value)
                {
                    case ProgressBarStyle.Blocks:
                    case ProgressBarStyle.Continuous:
                        {
                            MarqueeTimer.Stop();
                        }
                        break;
                    case ProgressBarStyle.Marquee:
                        {
                            switch (MarqueeAnimation)
                            {
                                case MarqueeAnimation.Slide:
                                    _marqueePosition = -_marqueeWidth;
                                    break;
                                case MarqueeAnimation.Bouncy:
                                    _marqueePosition = Padding.Left;
                                    break;
                            }

                            MarqueeTimer.Start();
                        }
                        break;
                }

                Invalidate();
            }
        }

        // Events

        protected override void OnPaint(PaintEventArgs e)
        {
            switch (_style)
            {
                case ProgressBarStyle.Blocks:
                    {

                    }
                    break;
                case ProgressBarStyle.Continuous:
                    {
                        switch (_orientation)
                        {
                            case Orientation.Horizontal:
                                {
                                    if (_invertOrientation)
                                        e.Graphics.FillRectangle(_foreColorBrush,
                                            (Width - ((_value * Width) / _maximum)) + Padding.Left, Padding.Top,
                                            ((_value * Width) / _maximum) - Padding.Horizontal, Height - Padding.Vertical);
                                    else
                                        e.Graphics.FillRectangle(_foreColorBrush,
                                            Padding.Left, Padding.Top,
                                            ((_value * Width) / _maximum) - Padding.Horizontal, Height - Padding.Vertical);
                                }
                                break;

                            case Orientation.Vertical:
                                {
                                    if (_invertOrientation)
                                        e.Graphics.FillRectangle(_foreColorBrush,
                                            Padding.Left, Padding.Top,
                                            Width - Padding.Horizontal, ((_value * Height) / _maximum) - Padding.Vertical);
                                    else
                                        e.Graphics.FillRectangle(_foreColorBrush,
                                            Padding.Left, (Height - ((_value * Height) / _maximum)) + Padding.Top,
                                            Width - Padding.Horizontal, ((_value * Height) / _maximum) - Padding.Vertical);
                                }
                                break;
                        }
                    }
                    break;
                case ProgressBarStyle.Marquee:
                    {
                        e.Graphics.FillRectangle(_foreColorBrush,
                            _marqueePosition, Padding.Top,
                            _marqueeWidth, Height - Padding.Vertical);
                    }
                    break;
            }
            
            if (_textStyle != ProgressBarTextStyle.None)
            {
                switch (_textStyle)
                {
                    case ProgressBarTextStyle.ValueOnMaximum:
                        _text = $"{_value} / {_maximum}";
                        break;

                    case ProgressBarTextStyle.Pourcentage:
                        _text = (float)_value / _maximum * 100 + "%";
                        break;
                }

                SizeF ts = e.Graphics.MeasureString(_text, Font);

                e.Graphics.DrawString(_text, Font,
                    _textBrush,
                    (Width / 2) - (ts.Width / 2),
                    (Height / 2) - (ts.Height / 2));
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(_backColorBrush, e.ClipRectangle);

            if (_borderPen.Width > 0)
            {
                // Top
                e.Graphics.DrawLine(_borderPen,
                    0, 0, Width, 0);

                // Right
                e.Graphics.DrawLine(_borderPen,
                    Width - 1, 0, Width - 1, Height - 1);

                // Bottom
                e.Graphics.DrawLine(_borderPen,
                    0, Height - 1, Width - 1, Height - 1);

                // Left
                e.Graphics.DrawLine(_borderPen,
                    0, 0, 0, Height);
            }
        }
    }
}
