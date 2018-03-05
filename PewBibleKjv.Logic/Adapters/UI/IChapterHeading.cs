namespace PewBibleKjv.Logic.Adapters.UI
{
    /// <summary>
    /// UI that displays the chapter heading text for the current verse.
    /// </summary>
    public interface IChapterHeading
    {
        /// <summary>
        /// Sets the chapter heading text.
        /// </summary>
        string Text { set; }
    }
}
