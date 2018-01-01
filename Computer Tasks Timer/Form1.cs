using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Computer_Tasks_Timer
{
    public partial class MainForm : Form
    {
        string formTitle;
        [DllImport("user32.dll")]
        private static extern int SendMessage(int hWnd, int hMsg, int wParam, int lParam);
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            formTitle = this.Text;
            TaskSelector.SelectedIndex = Properties.Settings.Default.TaskIndex;
            MyDateTimePicker.CustomFormat = "MM/dd/yyyy hh:mm:ss";
        }

        private int TotalSeconds()
        {
            return (int)(SecondsCount.Value + MinutesCount.Value * 60 + HoursCount.Value * 3600);
        }

        private void SetCounts(int totalSeconds)
        {
            SecondsCount.Value = 0;
            MinutesCount.Value = 0;
            HoursCount.Value = 0;
            while (totalSeconds - 3600 > 0)
            {
                HoursCount.Value++;
                totalSeconds -= 3600;
            }
            while (totalSeconds - 60 > 0)
            {
                MinutesCount.Value++;
                totalSeconds -= 60;
            }
            SecondsCount.Value = totalSeconds;
        }

        private void SecondsCount_ValueChanged(object sender, EventArgs e)
        {
            if (SecondsCount.Value == SecondsCount.Maximum)
                if (MinutesCount.Value < MinutesCount.Maximum && HoursCount.Value == HoursCount.Maximum)
                    SecondsCount.Value = SecondsCount.Maximum - 1;
                else
                {
                    MinutesCount.Value++;
                    SecondsCount.Value = 0;
                }

            if (SecondsCount.Value == SecondsCount.Minimum)
                if (HoursCount.Value > HoursCount.Minimum || MinutesCount.Value > MinutesCount.Minimum + 1)
                {
                    MinutesCount.Value--;
                    SecondsCount.Value = SecondsCount.Maximum - 1;
                }
                else
                    SecondsCount.Value = 0;
        }

        private void MinutesCount_ValueChanged(object sender, EventArgs e)
        {
            if (MinutesCount.Value == MinutesCount.Maximum)
                if (HoursCount.Value < HoursCount.Maximum)
                {
                    HoursCount.Value++;
                    MinutesCount.Value = 0;
                }
                else
                    MinutesCount.Value = MinutesCount.Maximum - 1;

            if (MinutesCount.Value == MinutesCount.Minimum)
                if (HoursCount.Value > HoursCount.Minimum)
                {
                    MinutesCount.Value = MinutesCount.Maximum - 1;
                    HoursCount.Value--;
                }
                else
                    MinutesCount.Value = 0;
        }

        private void TaskTimer_Tick(object sender, EventArgs e)
        {
            MyDateTimePicker.Value = DateTime.Now.AddSeconds(TotalSeconds());
            MyDateTimePicker.Checked = false;
            if (TotalSeconds() > 0)
            {
                SetCounts(TotalSeconds() - 1);
                if (WindowState == FormWindowState.Minimized)
                    this.Text = $"{(HoursCount.Value < 10 ? "0" : "") + HoursCount.Value}:{(MinutesCount.Value < 10 ? "0" : "") + MinutesCount.Value}:{(SecondsCount.Value < 10 ? "0" : "") + SecondsCount.Value}";
                else
                    this.Text = formTitle;
            }
            else
            {
                StartBTN.Text = "Start";
                TaskTimer.Enabled = false;
                switch (TaskSelector.SelectedItem)
                {
                    case "Shutdown": System.Diagnostics.Process.Start("shutdown.exe", "/s /t 0"); break;
                    case "Restart": System.Diagnostics.Process.Start("shutdown.exe", "/r /t 0"); break;
                    case "Sleep": System.Diagnostics.Process.Start("rundll32.exe", "powrprof.dll,SetSuspendState 0,1,0"); break;
                    case "Screen Off": System.Diagnostics.Process.Start("Rundll32.exe", "User32.dll,LockWorkStation"); SendMessage(0xFFFF, 0x112, 0xF170, 2); break;
                    case "Lock": System.Diagnostics.Process.Start("Rundll32.exe", "User32.dll,LockWorkStation"); break;
                    case "Hibernate": System.Diagnostics.Process.Start("shutdown.exe", "/h"); break;
                    case "Sign Out": System.Diagnostics.Process.Start("shutdown.exe", "/l"); break;
                    default: MessageBox.Show("Unknown item selected..."); break;
                }
                Application.Exit();
            }
        }

        private void StartBTN_Click(object sender, EventArgs e)
        {
            if (MyDateTimePicker.Checked)
            {
                if (MyDateTimePicker.Value < DateTime.Now)
                {
                    MyDateTimePicker.Value = DateTime.Now;
                    MessageBox.Show("The time cannot be past time...");
                }
                else
                    SetCounts((int)(MyDateTimePicker.Value - DateTime.Now).TotalSeconds);
            }
            if (!TaskTimer.Enabled)
            {
                HoursCount.Enabled = true;
                MinutesCount.Enabled = true;
                SecondsCount.Enabled = true;
            }
            TaskTimer.Enabled = !TaskTimer.Enabled;
            StartBTN.Text = !TaskTimer.Enabled ? "Start" : "Pause";
            MyDateTimePicker.Enabled = !TaskTimer.Enabled;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.TaskIndex = TaskSelector.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        private void MyDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if (!TaskTimer.Enabled)
            {
                HoursCount.Enabled = !MyDateTimePicker.Checked;
                MinutesCount.Enabled = !MyDateTimePicker.Checked;
                SecondsCount.Enabled = !MyDateTimePicker.Checked;
            }
        }
    }
}