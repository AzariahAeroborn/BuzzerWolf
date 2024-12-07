namespace BuzzerWolf.Server.Extensions
{
    public static class SemaphoreSlimExtensions
    {
        public static async Task<IDisposable> UseWaitAsync(
            this SemaphoreSlim semaphore,
            CancellationToken cancelToken = default)
        {
            await semaphore.WaitAsync(cancelToken).ConfigureAwait(false);
            return new ReleaseWrapper(semaphore);
        }

        private class ReleaseWrapper(SemaphoreSlim semaphore) : IDisposable
        {
            private readonly SemaphoreSlim _semaphore = semaphore;

            private bool _isDisposed;

            public void Dispose()
            {
                if (_isDisposed)
                    return;

                _semaphore.Release();
                _isDisposed = true;
            }
        }
    }
}
