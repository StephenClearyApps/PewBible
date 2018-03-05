using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PewBibleKjv.Logic;
using PewBibleKjv.Text;
using UnitTests.Util;
using Xunit;

namespace UnitTests
{
    public class AppUnitTests
    {
        [Fact]
        public void InitialStartup_StartsAtJohn_1_1()
        {
            var app = new StubbedApp();
            Assert.Equal(Bible.John_1_1, app.StubVerseView.CurrentAbsoluteVerseNumber);
            Assert.Equal(Location.Create(Bible.John_1_1).ChapterHeadingText, app.StubChapterHeading.Text);
        }

        [Fact]
        public void Scrolling_UpdatesHistory()
        {
            var app = new StubbedApp();
            var verse1 = Location.Create(Bible.John_1_1 + 13).AbsoluteVerseNumber;
            var verse2 = VerseHelper.Find("Psalms", 23).AbsoluteVerseNumber;
            app.StubVerseView.RaiseOnScroll(verse1);
            app.Recreate(verse2);
            app.StubHistoryControls.RaiseBackClick();
            Assert.Equal(verse1, app.StubVerseView.CurrentAbsoluteVerseNumber);

            app.StubHistoryControls.RaiseForwardClick();
            Assert.Equal(verse2, app.StubVerseView.CurrentAbsoluteVerseNumber);
        }

        [Fact]
        public void ScrollToSameChapter_DoesNotChangeChapterHeading()
        {
            var app = new StubbedApp();
            var john1ChapterHeading = app.StubChapterHeading.Text;
            app.StubVerseView.RaiseOnScroll(VerseHelper.Find("John", 1).Chapter.EndVerse - 1);
            Assert.Equal(john1ChapterHeading, app.StubChapterHeading.Text);
        }

        [Fact]
        public void ScrollToPreviousChapter_ChangesChapterHeading()
        {
            var app = new StubbedApp();
            var john1ChapterHeading = app.StubChapterHeading.Text;
            app.StubVerseView.RaiseOnScroll(app.StubVerseView.CurrentAbsoluteVerseNumber - 1);
            Assert.Equal(VerseHelper.Find("Luke", 24).ChapterHeadingText, app.StubChapterHeading.Text);
            Assert.NotEqual(john1ChapterHeading, app.StubChapterHeading.Text);
        }

        [Fact]
        public void ScrollToNextChapter_ChangesChapterHeading()
        {
            var app = new StubbedApp();
            var john1ChapterHeading = app.StubChapterHeading.Text;
            app.StubVerseView.RaiseOnScroll(VerseHelper.Find("John", 1).Chapter.EndVerse - 1);
            app.StubVerseView.RaiseOnScroll(VerseHelper.Find("John", 2).Chapter.BeginVerse);
            Assert.Equal(VerseHelper.Find("John", 2).ChapterHeadingText, app.StubChapterHeading.Text);
            Assert.NotEqual(john1ChapterHeading, app.StubChapterHeading.Text);
        }

        [Fact]
        public void JumpToSameChapter_DoesNotChangeChapterHeading()
        {
            var app = new StubbedApp();
            app.StubVerseView.RaiseOnScroll(VerseHelper.Find("John", 2).Chapter.EndVerse - 1);
            var john2ChapterHeading = app.StubChapterHeading.Text;
            app.Recreate(VerseHelper.Find("John", 2).AbsoluteVerseNumber);
            Assert.Equal(john2ChapterHeading, app.StubChapterHeading.Text);
        }

        [Fact]
        public void JumpToDifferentChapter_ChangesChapterHeading()
        {
            var app = new StubbedApp();
            var john1ChapterHeading = app.StubChapterHeading.Text;
            app.Recreate(VerseHelper.Find("John", 2).AbsoluteVerseNumber);
            Assert.Equal(VerseHelper.Find("John", 2).ChapterHeadingText, app.StubChapterHeading.Text);
            Assert.NotEqual(john1ChapterHeading, app.StubChapterHeading.Text);
        }
        
        [Fact]
        public void SwipeLeft_MovesToNextChapter()
        {
            var app = new StubbedApp();
            var john1ChapterHeading = app.StubChapterHeading.Text;
            app.StubVerseView.RaiseOnSwipeLeft();
            var expectedVerse = VerseHelper.Find("John", 2);
            Assert.Equal(expectedVerse.AbsoluteVerseNumber, app.StubVerseView.CurrentAbsoluteVerseNumber);
            Assert.Equal(expectedVerse.ChapterHeadingText, app.StubChapterHeading.Text);
            Assert.NotEqual(john1ChapterHeading, app.StubChapterHeading.Text);
        }

        [Fact]
        public void SwipeRight_MovesToPreviousChapter()
        {
            var app = new StubbedApp();
            var john1ChapterHeading = app.StubChapterHeading.Text;
            app.StubVerseView.RaiseOnSwipeRight();
            var expectedVerse = VerseHelper.Find("John", 1).PreviousChapter();
            Assert.Equal(expectedVerse.AbsoluteVerseNumber, app.StubVerseView.CurrentAbsoluteVerseNumber);
            Assert.Equal(expectedVerse.ChapterHeadingText, app.StubChapterHeading.Text);
            Assert.NotEqual(john1ChapterHeading, app.StubChapterHeading.Text);
        }
    }
}
