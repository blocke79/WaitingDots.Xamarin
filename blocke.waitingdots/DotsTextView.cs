/* 
 * Xamarin C# Port of WaitingDots
 * Created by Bradley Locke May 2015 - brad.locke@gmail.com
 * https://github.com/blocke79/WaitingDots.Xamarin
 * 
 * 
 * Originally Java version created by tajchert
 * https://github.com/tajchert/WaitingDots
 * 
 */

using System;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views.Animations;
using Android.Widget;
using Java.Lang;
using Math = Java.Lang.Math;

namespace blocke.waitingdots
{
    public class DotsTextView : LinearLayout
    {
        private const int ShowSpeed = 700;
        private bool _autoPlay;
        private TextView _dotOne;
        private TextView _dotThree;
        private TextView _dotTwo;
        private Handler _handler;

        private bool _isHide;
        private bool _isPlaying;
        private int _jumpHeight;

        private bool _lockDotOne;
        private bool _lockDotThree;
        private bool _lockDotTwo;
        private int _period;
        private long _startTime;
        private int _textColor;
        private int _textSize;
        private int _textWidth;

        protected DotsTextView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public DotsTextView(Context context)
            : base(context)
        {
            Init(context, null);
        }

        public DotsTextView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            Init(context, attrs);
        }

        public DotsTextView(Context context, IAttributeSet attrs, int defStyleAttr)
            : base(context, attrs, defStyleAttr)
        {
            Init(context, attrs);
        }

        public virtual bool IsHide
        {
            get { return _isHide; }
        }

        public virtual bool Playing
        {
            get { return _isPlaying; }
        }

        public virtual int DotsColor
        {
            set
            {
                _textColor = value;
                UpdateStyle();
            }
        }

        public virtual int DotsSize
        {
            set
            {
                _textSize = value;
                UpdateStyle();
            }
        }

        public virtual int JumpHeight
        {
            set { _jumpHeight = value; }
        }

        public virtual int Period
        {
            set { _period = value; }
        }

        private void Init(Context context, IAttributeSet attrs)
        {
            _handler = new Handler(Looper.MainLooper);

            if (attrs != null)
            {
                var typedArray = context.ObtainStyledAttributes(attrs, Resource.Styleable.WaitingDots);
                _textColor = typedArray.GetColor(Resource.Styleable.WaitingDots_android_textColor, Color.Gray);
                _period = typedArray.GetInt(Resource.Styleable.WaitingDots_period, 175);
                _textSize = typedArray.GetDimensionPixelSize(Resource.Styleable.WaitingDots_android_textSize, 14);
                _jumpHeight = typedArray.GetInt(Resource.Styleable.WaitingDots_jumpHeight, _textSize/4);
                _autoPlay = typedArray.GetBoolean(Resource.Styleable.WaitingDots_autoplay, true);
                typedArray.Recycle();
            }
            ResetPosition();
            Inflate(Context, Resource.Layout.dots_text_view, this);
            _dotOne = (TextView) FindViewById(Resource.Id.dot1);
            _dotTwo = (TextView) FindViewById(Resource.Id.dot2);
            _dotThree = (TextView) FindViewById(Resource.Id.dot3);

            if (_autoPlay)
            {
                SetWillNotDraw(false);
            }
            _isPlaying = _autoPlay;

            UpdateStyle();
        }

        private void UpdateStyle()
        {
            _dotOne.SetTextSize(ComplexUnitType.Px, _textSize);
            _dotTwo.SetTextSize(ComplexUnitType.Px, _textSize);
            _dotThree.SetTextSize(ComplexUnitType.Px, _textSize);

            _dotOne.SetTextColor(new Color(_textColor));
            _dotTwo.SetTextColor(new Color(_textColor));
            _dotThree.SetTextColor(new Color(_textColor));


            _dotOne.Measure(0, 0);
            _textWidth = _dotOne.MeasuredWidth;
        }

        public virtual void ResetPosition()
        {
            _startTime = DateTimeHelperClass.CurrentUnixTimeMillis() + _period;
        }

        public virtual void Start()
        {
            _isPlaying = true;
            _lockDotOne = false;
            _lockDotTwo = false;
            _lockDotThree = false;
            ResetPosition();
            SetWillNotDraw(false);
        }

        public virtual void Stop()
        {
            _isPlaying = false;
        }

        public virtual void Hide()
        {
            var moveRightToLeft = new TranslateAnimation(0, -(_textWidth*2), 0, 0);
            moveRightToLeft.Duration = ShowSpeed;
            moveRightToLeft.FillAfter = true;

            _dotThree.StartAnimation(moveRightToLeft);

            moveRightToLeft = new TranslateAnimation(0, -_textWidth, 0, 0) {Duration = ShowSpeed, FillAfter = true};

            _dotTwo.StartAnimation(moveRightToLeft);
            _isHide = true;
        }

        public virtual void Show()
        {
            var moveRightToLeft = new TranslateAnimation(-(_textWidth*2), 0, 0, 0)
            {
                Duration = ShowSpeed,
                FillAfter = true
            };

            _dotThree.StartAnimation(moveRightToLeft);

            moveRightToLeft = new TranslateAnimation(-_textWidth, 0, 0, 0) {Duration = ShowSpeed, FillAfter = true};

            _dotTwo.StartAnimation(moveRightToLeft);
            _isHide = false;
        }


        public virtual void ShowAndPlay()
        {
            Show();


            var r = new Runnable(Start);

            _handler.PostDelayed(r, ShowSpeed);
        }

        public virtual void HideAndStop()
        {
            Hide();

            var r = new Runnable(Start);

            _handler.PostDelayed(r, ShowSpeed);
        }


        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            var time = (float) (DateTimeHelperClass.CurrentUnixTimeMillis() - _startTime)/_period;

            if (_isPlaying)
            {
                for (var i = 3; i >= 0; i--)
                {
                    var y = (float) -(_jumpHeight*Math.Max(0, Math.Sin(time + i/1.5f)));
                    switch (i)
                    {
                        case 2:
                            _dotOne.TranslationY = y;
                            break;
                        case 1:
                            _dotTwo.TranslationY = y;
                            break;
                        case 0:
                            _dotThree.TranslationY = y;
                            break;
                    }
                }
            }
            else
            {
                for (var i = 3; i >= 0; i--)
                {
                    var y = (float) -(_jumpHeight*Math.Max(0, Math.Sin(time + i/1.5f)));
                    switch (i)
                    {
                        case 2:
                            if (y == 0 || _lockDotOne)
                            {
                                _lockDotOne = true;
                                _dotOne.TranslationY = 0;
                            }
                            else
                            {
                                _dotOne.TranslationY = y;
                            }
                            break;
                        case 1:
                            if (y == 0 || _lockDotTwo)
                            {
                                _lockDotTwo = true;
                                _dotTwo.TranslationY = 0;
                            }
                            else
                            {
                                _dotTwo.TranslationY = y;
                            }
                            break;
                        case 0:
                            if (y == 0 || _lockDotThree)
                            {
                                _lockDotThree = true;
                                _dotThree.TranslationY = 0;
                            }
                            else
                            {
                                _dotThree.TranslationY = y;
                            }
                            break;
                    }
                }
                if (_lockDotOne && _lockDotTwo && _lockDotThree)
                {
                    //all are in bottom position
                    SetWillNotDraw(true);
                }
            }
            Invalidate();
        }

        //---------------------------------------------------------------------------------------------------------
        //	Copyright © 2007 - 2014 Tangible Software Solutions Inc.
        //	This class can be used by anyone provided that the copyright notice remains intact.
        //
        //	This class is used to replace calls to Java's System.currentTimeMillis with the C# equivalent.
        //	Unix time is defined as the number of seconds that have elapsed since midnight UTC, 1 January 1970.
        //---------------------------------------------------------------------------------------------------------
        internal static class DateTimeHelperClass
        {
            private static readonly DateTime Jan1St1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            internal static long CurrentUnixTimeMillis()
            {
                return (long) (DateTime.UtcNow - Jan1St1970).TotalMilliseconds;
            }
        }
    }
}