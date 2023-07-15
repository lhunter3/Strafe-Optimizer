using Gma.System.MouseKeyHook;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;

namespace Strafe_Optimizer
{
    public partial class Form1 : Form
    {

        private InputSimulator inputSimulator = new InputSimulator();
        private IKeyboardMouseEvents events;
        private ILogger logger = new LoggerConfiguration()
                        .WriteTo.Console()
                        .CreateLogger();

        private double prev = 0;
        private bool isPressed = false;
        private char hotkey = 'v';


        public Form1()
        {
            InitializeComponent();
            Unsubscribe();
            Subscribe(Hook.GlobalEvents());
            this.Opacity = 0;
        }

        private void Subscribe(IKeyboardMouseEvents e)
        {
            events = e;
            events.KeyPress += HookManager_KeyPress;
            events.MouseMove += HookManager_MouseMove;
        }


        private void Unsubscribe()
        {
            if (events == null) return;
            events.KeyPress -= HookManager_KeyPress;
            events.MouseMove -= HookManager_MouseMove;
            events = null;
        }

        private void OnFormClosing(object sender, CancelEventArgs e)
        {
            Unsubscribe();
        }

        private void HookManager_KeyPress(object sender, KeyPressEventArgs e)
        {
            //logger.Information(string.Format("KeyPress \t\t\t {0}\n", e.KeyChar));

            if (e.KeyChar.Equals(hotkey))
                isPressed = true;
            else
                isPressed = false;

        }



        private void HookManager_MouseMove(object sender, MouseEventArgs e)
        {

            //logger.Information(string.Format("{0}", e.X));

            if (isPressed)
            {
                if (prev < e.X)
                {
                    prev = e.X;
                    logger.Information("right");
                    inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_D);
                }
                else
                {
                    prev = e.X;
                    logger.Information("left");
                    inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_A);

                }

            }
            
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            Form form = (Form)sender;
            form.ShowInTaskbar = false;
            form.Opacity = 0.01;
        }
    }
}
