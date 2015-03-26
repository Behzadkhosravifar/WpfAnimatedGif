using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WaitSplash
{
    /// <summary>
    /// Interaction logic for WaitWPFControl.xaml
    /// </summary>
    public partial class WaitWPFControl : UserControl
    {
        #region Constructors

        public WaitWPFControl()
        {
            InitializeComponent();

            TickCounter = new DispatcherTimer();
            TickCounter.Tick += TickCounter_Tick;
        }

        #endregion

        #region Properties

        protected object SyncLocker = new object();
        protected int ShowTimes = 0;
        protected long TickCount;
        protected DispatcherTimer TickCounter;

        #endregion

        #region Methods

        void TickCounter_Tick(object sender, System.EventArgs e)
        {
            lblTimeNo.Text = ((Environment.TickCount - TickCount) / 1000).ToString(CultureInfo.InvariantCulture);
        }

        public void Start()
        {
            lock (SyncLocker)
            {
                if (++ShowTimes == 1)
                {
                    TickCount = Environment.TickCount;
                    TickCounter.Start();
                    this.Visibility = Visibility.Visible;
                }
            }
        }

        public void Stop()
        {
            lock (SyncLocker)
            {
                if (--ShowTimes <= 0)
                {
                    this.Visibility = Visibility.Hidden; ;
                    TickCounter.Stop();
                }
            }
        }

        #endregion
    }
}
