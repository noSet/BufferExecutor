using System;
using System.Threading;
using Xunit;

namespace BufferExecutor.Test;

public class BufferExecutorTest
{
    [Theory]
    [InlineData(1, false)]
    [InlineData(2, true)]
    public void ExecuteTest(int num, bool result)
    {
        var action = () =>
        {
            Thread.Sleep(200);
            num--;
        };

        // 始终执行两次
        BufferExecutor executor = new();
        for (int i = 0; i < 10; i++)
        {
            Thread.Sleep(0); // 放弃当前线程时间片，使executor内部已经开始使用线程池线程
            executor.Execute(action);
        }

        Thread.Sleep(2000);

        Assert.Equal(num == 0, result);
    }
}
