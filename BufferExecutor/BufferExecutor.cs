namespace BufferExecutor;
public class BufferExecutor
{
    private int limit;
    private Action? action;
    private readonly object sync = new();

    public void Execute(Action action)
    {
        lock (sync)
        {
            this.action = action;

            if (limit == 0)
            {
                limit++;
                ExecuteCore();
            }
        }

    }

    private void ExecuteCore()
    {
        ThreadPool.QueueUserWorkItem(_ =>
        {
            Action? action = null;

            while (true)
            {
                lock (sync)
                {
                    if (this.action == null)
                    {
                        limit--;
                        break;
                    }

                    action = this.action;
                    this.action = null;
                }

                action();
            }
        });
    }
}
