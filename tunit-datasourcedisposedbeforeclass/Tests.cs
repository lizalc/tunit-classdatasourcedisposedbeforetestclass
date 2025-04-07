using TUnit.Core.Interfaces;

namespace tunit_datasourcedisposedbeforeclass;

[ClassDataSource<DataClass>]
[ClassDataSource<DataClass>(Shared = SharedType.None)]
[ClassDataSource<DataClass>(Shared = SharedType.Keyed, Key = "")]
[ClassDataSource<DataClass>(Shared = SharedType.PerClass)] // Works as expected
[ClassDataSource<DataClass>(Shared = SharedType.PerAssembly)] // Works as expected
[ClassDataSource<DataClass>(Shared = SharedType.PerTestSession)]
public class Tests(DataClass dataClass) : IAsyncDisposable
{
    [Test]

    public void ClassDataSource()
    { }

    public async ValueTask DisposeAsync()
    {
        await dataClass.CalledOnTestClassDisposal();
    }
}

public class DataClass : IAsyncInitializer, IAsyncDisposable
{
    volatile bool _disposed = false;

    public async Task InitializeAsync()
    { }

    public async ValueTask DisposeAsync()
    {
        _disposed = true;
    }

    public async ValueTask CalledOnTestClassDisposal()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(DataClass));
        }

        Console.WriteLine("This method is called when the test class is disposed");
    }
}
