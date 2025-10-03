using System;
using System.Threading;
using System.Threading.Tasks;

namespace MatrixConsoleApp
{
    class Program
    {
        
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        
        private static readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private static volatile bool _pauseForMessage = false;

        private static readonly ManualResetEventSlim _pauseEvent = new ManualResetEventSlim(true);

        static async Task Main(string[] args)
        {
            Console.CancelKeyPress += (sender, e) =>
            {
                e.Cancel = true;
                _cancellationTokenSource.Cancel();
                Console.WriteLine("\n\nShutting down...");
            };

            Console.WriteLine("Starting Matrix Console... Press Ctrl+C to exit.\n");
            await Task.Delay(1000);

           
            var binaryTask = Task.Run(() => OutputBinaryAsync(_cancellationTokenSource.Token), _cancellationTokenSource.Token);
            var messageTask = Task.Run(() => DisplayMessageAsync(_cancellationTokenSource.Token), _cancellationTokenSource.Token);

            try
            {
              
                await Task.WhenAll(binaryTask, messageTask);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Application terminated gracefully.");
            }
            finally
            {
                
                _semaphore?.Dispose();
                _pauseEvent?.Dispose();
                _cancellationTokenSource?.Dispose();
            }
        }

       
        private static async Task OutputBinaryAsync(CancellationToken cancellationToken)
        {
            var random = new Random();

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    
                    _pauseEvent.Wait(cancellationToken);

                   
                    await _semaphore.WaitAsync(cancellationToken);

                    try
                    {
                        if (!_pauseForMessage)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;

                           
                            for (int i = 0; i < 50; i++)
                            {
                                Console.Write(random.Next(2));

                                if (_pauseForMessage || cancellationToken.IsCancellationRequested)
                                    break;
                            }

                            Console.ResetColor();
                        }
                    }
                    finally
                    {
                        _semaphore.Release();
                    }

                   
                    await Task.Delay(10, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

       
        private static async Task DisplayMessageAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                   
                    await Task.Delay(5000, cancellationToken);

                    _pauseForMessage = true;
                    _pauseEvent.Reset();

                   
                    await Task.Delay(100, cancellationToken);

                  
                    await _semaphore.WaitAsync(cancellationToken);

                    try
                    {
                       
                        Console.WriteLine(); 
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n╔════════════════════════════════════╗");
                        Console.WriteLine("║  Neo, you are the chosen one       ║");
                        Console.WriteLine("╚════════════════════════════════════╝\n");
                        Console.ResetColor();
                    }
                    finally
                    {
                        _semaphore.Release();
                    }

                  
                    await Task.Delay(5000, cancellationToken);

                  
                    _pauseForMessage = false;
                    _pauseEvent.Set();
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }
    }
}