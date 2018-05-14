using Common.Constants;
using IBusiness;
using System;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;
using NLog;

namespace P.Service
{
    public partial class AutomateImportService : ServiceBase
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        Logger loggers = LogManager.GetLogger(Constants.PServiceLogger);
        
        public readonly IImportService _importService;
        private ManualResetEvent StoppedEvent { get; }
        public AutomateImportService(IImportService importService)
        {
            _importService = importService;
        }

        private bool Stopping { get; set; }

        public static Timer KeepAlive = new Timer();

        public AutomateImportService()
        {
            InitializeComponent();
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            Stopping = false;
            StoppedEvent = new ManualResetEvent(false);
            KeepAlive.Elapsed += (sender, e) => KeepAliveEvent(sender, e);

        }

        public void KeepAliveEvent(object source, ElapsedEventArgs e)
        {
           _importService.UpdateSystemStatus();
        }

        protected override void OnStart(string[] args)
        {
            loggers.Debug("AutomateImportService has been started");
            KeepAlive.Interval = double.Parse("60000");
            KeepAlive.Enabled = true;
            KeepAlive.AutoReset = true;
            KeepAlive.Start();

            ThreadPool.QueueUserWorkItem(async state =>
            {
                loggers.Debug("Initializing PWatcher");
                var watcher = new PWatcher(_importService);
                while (!Stopping)
                {
                    try
                    {
                        await watcher.RunWatcherAsync();
                        loggers.Debug("Checking for more files in 5 seconds");
                        Thread.Sleep(5000);
                    }
                    catch (Exception ex)
                    {
                        loggers.Error(ex, $"An exception occurred trying to process inbound files.  We will attempt again in 20 seconds. {ex.Message}");
                        Thread.Sleep(20000);
                    }
                }
                KeepAlive.Stop();
                watcher = null;
                KeepAlive = null;
                StoppedEvent.Set();
            });
        }

        protected override void OnStop()
        {
            loggers.Debug("AutomateImportService is stopping.");
            Stopping = true;
            StoppedEvent.WaitOne();
            loggers.Debug("AutomateImportService has been stopped.");
        }

        public void TestStartupAndStop(string[] args)
        {
            try
            {
                OnStart(args);
                Console.Read();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
