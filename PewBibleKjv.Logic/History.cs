using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using PewBibleKjv.Logic.Adapters.Services;
using PewBibleKjv.Text;

namespace PewBibleKjv.Logic
{
    public sealed class History
    {
        private const int MaxHistory = 20;
        private readonly ISimpleStorage _simpleStorage;

        /// <summary>
        /// The current location in history.
        /// This may be 0, indicating we're at the beginning of history.
        /// This always refers to a valid index in the range <c>[0, <see cref="_history"/>.length)</c>.
        /// </summary>
        private int _currentIndex;

        /// <summary>
        /// Collection of absolute verse numbers. This always has at least one element.
        /// </summary>
        private readonly List<int> _history;

        public History(ISimpleStorage simpleStorage)
        {
            _simpleStorage = simpleStorage;

            var currentIndexString = _simpleStorage.Load("history-currentIndex");
            if (currentIndexString == null)
            {
                _currentIndex = 0;
                _history = new List<int> { 26045 };
            }
            else
            {
                _currentIndex = int.Parse(currentIndexString, CultureInfo.InvariantCulture);
                _history = new List<int>(_simpleStorage.Load("history-history").Split(',').Select(x => int.Parse(x, CultureInfo.InvariantCulture)));
            }
        }

        public int CurrentAbsoluteVerseNumber => _history[_currentIndex];
        public bool CanMovePrevious => _currentIndex != 0;
        public bool CanMoveNext => _currentIndex != _history.Count - 1;
        public event Action CanMoveChanged;

        public void Save(int currentAbsoluteVerseNumber)
        {
            _history[_currentIndex] = currentAbsoluteVerseNumber;
            _simpleStorage.Save("history-currentIndex", _currentIndex.ToString(CultureInfo.InvariantCulture));
            _simpleStorage.Save("history-history", string.Join(",", _history.Select(x => x.ToString(CultureInfo.InvariantCulture))));
        }

        public int MovePrevious(int currentAbsoluteVerseNumber)
        {
            _history[_currentIndex] = currentAbsoluteVerseNumber;
            var result = _history[--_currentIndex];
            CanMoveChanged?.Invoke();
            return result;
        }

        public int MoveNext(int currentAbsoluteVerseNumber)
        {
            _history[_currentIndex] = currentAbsoluteVerseNumber;
            var result = _history[++_currentIndex];
            CanMoveChanged?.Invoke();
            return result;
        }

        public void SaveJump(int currentAbsoluteVerseNumber, int jumpAbsoluteVerseNumber)
        {
            _history[_currentIndex] = currentAbsoluteVerseNumber;

            // If the user is jumping from the same location, don't insert a duplicate.
            if (currentAbsoluteVerseNumber == jumpAbsoluteVerseNumber)
                return;

            // If the user is jumping to the same location, don't insert a duplicate.
            var next = (_currentIndex == _history.Count - 1) ? Bible.InvalidAbsoluteVerseNumber : _history[_currentIndex + 1];
            if (next == jumpAbsoluteVerseNumber)
            {
                ++_currentIndex;
                CanMoveChanged?.Invoke();
                return;
            }

            _history.Insert(++_currentIndex, jumpAbsoluteVerseNumber);

            if (_history.Count > MaxHistory)
            {
                if (_currentIndex <= MaxHistory / 2)
                {
                    _history.RemoveAt(_history.Count - 1);
                }
                else
                {
                    _history.RemoveAt(0);
                    --_currentIndex;
                }
            }
            CanMoveChanged?.Invoke();
        }
    }
}
