
using System.Threading;
using System.Windows.Forms;

namespace PauseThread
{
    public partial class Form1 : Form
    {
        SynchronizationContext synchronizationContext;
        CancellationTokenSource cancellationTokenSource;
        CancellationToken cancellationToken;
        Thread th;
        bool IsPouse =false;
        public Form1()
        {
            InitializeComponent();
            synchronizationContext = SynchronizationContext.Current;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;
            th = new Thread(() =>
            {

                Random random = new Random();
                for (int i = 0; i < 100; i++)
                {
                    try
                    {
                       
                        if (cancellationToken.IsCancellationRequested )
                        {
                            throw new OperationCanceledException();
                            
                        }
                        synchronizationContext.Send(state =>listBox1.Items.Add(random.Next()),null);
                        Thread.Sleep(1000);
                    }
                    catch(OperationCanceledException)
                    {
                        IsPouse = !IsPouse;
                        cancellationTokenSource = new CancellationTokenSource();
                        cancellationToken = cancellationTokenSource.Token;
                        
                    }
                    try 
                    {
                        if (IsPouse)
                            Thread.Sleep(Timeout.Infinite);

                    }
                    catch (ThreadInterruptedException)
                    {
                        IsPouse = false;

                    }
                }


            });
            th.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!IsPouse) 
            { 
                cancellationTokenSource.Cancel();
            }
            else
            {
                th.Interrupt();
            }
             

        }
    }
}
