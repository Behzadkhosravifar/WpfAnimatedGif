using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WaitSplash;

namespace TestWaitSplash
{
    public partial class BaseForm : Form
    {
        public static Splash WaitSplash = new Splash();

        public Action OnStartupAction;

        public BaseForm()
        {
            InitializeComponent();

            WaitSplash.OwnerControl = this;

            Application.Idle += Application_Idle;
        }

        async void Application_Idle(object sender, EventArgs e)
        {
            Application.Idle -= Application_Idle;

            await InvokeAsync(OnStartupAction);
        }

        public async Task InvokeAsync(Action doSomething)
        {
            WaitSplash.Start();

            if (doSomething != null)
            {
                await Task.Run(() =>
                {
                    var invokeThread = new Thread(new ThreadStart(doSomething));
                    invokeThread.SetApartmentState(ApartmentState.STA);
                    invokeThread.Start();
                    invokeThread.Join();
                });
            }

            WaitSplash.Stop();

            this.Focus();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            WaitSplash.CenterToParent(this);
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);

            WaitSplash.CenterToParent(this);
        }


        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            WaitSplash.Focus();
        }
    }
}
