using System;
using Android.Content;
using Android.Views;

namespace PewBibleKjv.Util
{
    public sealed class SwipeTouchListener: Java.Lang.Object, View.IOnTouchListener
    {
        private readonly GestureDetector _gestureDetector;

        public SwipeTouchListener(Context context)
        {
            _gestureDetector = new GestureDetector(context, new SwipeGestureListener(this));
        }

        public event Action OnSwipeLeft;
        public event Action OnSwipeRight;

        public bool OnTouch(View v, MotionEvent e) => _gestureDetector.OnTouchEvent(e);

        public sealed class SwipeGestureListener : GestureDetector.SimpleOnGestureListener
        {
            private readonly SwipeTouchListener _swipeTouchListener;

            public SwipeGestureListener(SwipeTouchListener swipeTouchListener)
            {
                _swipeTouchListener = swipeTouchListener;
            }

            private const int SwipeDistanceThreshold = 120;
            private const int SwipeVelocityThreshold = 200;

            public override bool OnDown(MotionEvent e) => true;

            public override bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
            {
                var distanceX = e2.GetX() - e1.GetX();
                var distanceY = e2.GetY() - e1.GetY();
                if (Math.Abs(distanceX) < Math.Abs(distanceY) || Math.Abs(distanceX) < SwipeDistanceThreshold || Math.Abs(velocityX) < SwipeVelocityThreshold)
                    return false;
                if (distanceX > 0)
                    _swipeTouchListener.OnSwipeRight?.Invoke();
                else
                    _swipeTouchListener.OnSwipeLeft?.Invoke();
                return true;
            }
        }
    }
}