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

        /// <summary>
        /// Initializes the history, loading any saved history from <paramref name="simpleStorage"/>.
        /// </summary>
        /// <param name="simpleStorage">The underlying storage.</param>
        public History(ISimpleStorage simpleStorage)
        {
            _simpleStorage = simpleStorage;

            var currentIndexString = _simpleStorage.Load("history-currentIndex");
            if (currentIndexString == null)
            {
                _currentIndex = 0;
                _history = new List<int> { Bible.John_1_1 };
            }
            else
            {
                _currentIndex = int.Parse(currentIndexString, CultureInfo.InvariantCulture);
                _history = new List<int>(_simpleStorage.Load("history-history").Split(',').Select(x => int.Parse(x, CultureInfo.InvariantCulture)));
            }
        }

        /// <summary>
        /// Gets the verse number at the current position in the history stack.
        /// </summary>
        public int CurrentAbsoluteVerseNumber => _history[_currentIndex];

        /// <summary>
        /// Whether the history can be moved back.
        /// </summary>
        public bool CanMoveBack => _currentIndex != 0;

        /// <summary>
        /// Whether the history can be moved forward.
        /// </summary>
        public bool CanMoveForward => _currentIndex != _history.Count - 1;

        /// <summary>
        /// Notification that <see cref="CanMoveBack"/> and/or <see cref="CanMoveForward"/> may have changed.
        /// </summary>
        public event Action CanMoveChanged;

        /// <summary>
        /// Sets the verse number for the current position in the history stack, and saves the history to storage.
        /// </summary>
        /// <param name="currentAbsoluteVerseNumber">The absolute verse number.</param>
        public void Save(int currentAbsoluteVerseNumber)
        {
            if (currentAbsoluteVerseNumber == Bible.InvalidAbsoluteVerseNumber)
                throw new InvalidOperationException("Invalid verse number");
            _history[_currentIndex] = currentAbsoluteVerseNumber;
            _simpleStorage.Save("history-currentIndex", _currentIndex.ToString(CultureInfo.InvariantCulture));
            _simpleStorage.Save("history-history", string.Join(",", _history.Select(x => x.ToString(CultureInfo.InvariantCulture))));
        }

        /// <summary>
        /// Saves a new verse number for the current position, moves back one position, and returns the verse number for the new position.
        /// This method may only be called when <see cref="CanMoveBack"/> is <c>true</c>.
        /// </summary>
        /// <param name="currentAbsoluteVerseNumber">The new verse number for the current position.</param>
        public int MoveBack(int currentAbsoluteVerseNumber)
        {
            if (currentAbsoluteVerseNumber == Bible.InvalidAbsoluteVerseNumber)
                throw new InvalidOperationException("Invalid verse number");
            if (!CanMoveBack)
                throw new InvalidOperationException("Invalid state");
            _history[_currentIndex] = currentAbsoluteVerseNumber;
            var result = _history[--_currentIndex];
            CanMoveChanged?.Invoke();
            return result;
        }

        /// <summary>
        /// Saves a new verse number for the current position, moves forward one position, and returns the verse number for the new position.
        /// This method may only be called when <see cref="CanMoveForward"/> is <c>true</c>.
        /// </summary>
        /// <param name="currentAbsoluteVerseNumber">The new verse number for the current position.</param>
        public int MoveForward(int currentAbsoluteVerseNumber)
        {
            if (currentAbsoluteVerseNumber == Bible.InvalidAbsoluteVerseNumber)
                throw new InvalidOperationException("Invalid verse number");
            if (!CanMoveForward)
                throw new InvalidOperationException("Invalid state");
            _history[_currentIndex] = currentAbsoluteVerseNumber;
            var result = _history[++_currentIndex];
            CanMoveChanged?.Invoke();
            return result;
        }

        /// <summary>
        /// Notifies the history stack of a jump.
        /// Saves a new verse number for the current position, inserts a new position in the stack, and moves forward to that position.
        /// After this method is called, <see cref="CurrentAbsoluteVerseNumber"/> is equal to <paramref name="jumpAbsoluteVerseNumber"/>.
        /// </summary>
        /// <param name="currentAbsoluteVerseNumber">The new verse number for the current position.</param>
        /// <param name="jumpAbsoluteVerseNumber">The verse number for the new position.</param>
        public void AddJump(int currentAbsoluteVerseNumber, int jumpAbsoluteVerseNumber)
        {
            if (currentAbsoluteVerseNumber == Bible.InvalidAbsoluteVerseNumber || jumpAbsoluteVerseNumber == Bible.InvalidAbsoluteVerseNumber)
                throw new InvalidOperationException("Invalid verse number");
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
