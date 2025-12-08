namespace PewBibleKjv.VerseView;

public interface ISimpleCacheItem<out T>
{
    T Instance { get; }
    void Free();
}