using PewBibleKjv.Logic;
using PewBibleKjv.Logic.Adapters.UI;

namespace UnitTests.Util;

public sealed class StubThemeModeApplier : IThemeModeApplier
{
    public ThemeMode? LastAppliedMode { get; private set; }
    public int ApplyCount { get; private set; }

    public void Apply(ThemeMode mode)
    {
        LastAppliedMode = mode;
        ++ApplyCount;
    }
}
