using Microsoft.WindowsAPICodePack.Taskbar;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using YonatanMankovich.ComputerTasksTimer.Properties;

namespace YonatanMankovich.ComputerTasksTimer
{
    public partial class MainForm : Form
    {
        private uint TotalSecondsAtStart { get; set; } = 0;
        private string OriginalFormTitle { get; }
        private bool IsFormFocused { get; set; } = true;

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        public MainForm(string[] args)
        {
            InitializeComponent();
            OriginalFormTitle = Text;
            TaskSelector.SelectedIndex = Settings.Default.TaskIndex;
            MyDateTimePicker.CustomFormat = "MM/dd/yyyy H:mm:ss";

            if (args.Length > 0) // Arg 0 is seconds; Arg 1 is the task name.
            {
                // Start the timer with the seconds set by an outside program.
                SetCounts(uint.Parse(args[0]));

                if (args.Length > 1)
                    TaskSelector.SelectedIndex = (int)Enum.Parse(typeof(ComputerTask), args[1], ignoreCase: true);

                StartBTN_Click(null, EventArgs.Empty);
            }
        }

        private uint GetTotalSeconds()
        {
            return (uint)(SecondsCount.Value + MinutesCount.Value * 60 + HoursCount.Value * 3600);
        }

        private void SetCounts(uint totalSeconds)
        {
            SecondsCount.Value = MinutesCount.Value = HoursCount.Value = 0;
            HoursCount.Value = totalSeconds / 3600;
            totalSeconds -= (uint)HoursCount.Value * 3600;
            MinutesCount.Value = totalSeconds / 60;
            totalSeconds -= (uint)MinutesCount.Value * 60;
            SecondsCount.Value = totalSeconds;
        }

        private void SecondsCount_ValueChanged(object sender, EventArgs e)
        {
            if (SecondsCount.Value == SecondsCount.Maximum)
            {
                if (MinutesCount.Value < MinutesCount.Maximum && HoursCount.Value == HoursCount.Maximum)
                    SecondsCount.Value = SecondsCount.Maximum - 1;
                else
                {
                    MinutesCount.Value++;
                    SecondsCount.Value = 0;
                }
            }

            if (SecondsCount.Value == SecondsCount.Minimum)
            {
                if (HoursCount.Value > HoursCount.Minimum || MinutesCount.Value > MinutesCount.Minimum + 1)
                {
                    MinutesCount.Value--;
                    SecondsCount.Value = SecondsCount.Maximum - 1;
                }
                else
                    SecondsCount.Value = 0;
            }
        }

        private void MinutesCount_ValueChanged(object sender, EventArgs e)
        {
            if (MinutesCount.Value == MinutesCount.Maximum)
            {
                if (HoursCount.Value < HoursCount.Maximum)
                {
                    HoursCount.Value++;
                    MinutesCount.Value = 0;
                }
                else
                    MinutesCount.Value = MinutesCount.Maximum - 1;
            }

            if (MinutesCount.Value == MinutesCount.Minimum)
            {
                if (HoursCount.Value > HoursCount.Minimum)
                {
                    MinutesCount.Value = MinutesCount.Maximum - 1;
                    HoursCount.Value--;
                }
                else
                    MinutesCount.Value = 0;
            }
        }

        private void TaskTimer_Tick(object sender, EventArgs e)
        {
            uint totalSeconds = GetTotalSeconds();
            MyDateTimePicker.Value = DateTime.Now.AddSeconds(totalSeconds);
            MyDateTimePicker.Checked = false;

            if (totalSeconds > 0)
            {
                SetCounts(totalSeconds - 1);

                Text = IsFormFocused ? OriginalFormTitle
                    : new TimeSpan((int)HoursCount.Value, (int)MinutesCount.Value, (int)SecondsCount.Value).ToString();

                // If the user changes the seconds after starting the timer (prevents a negative value later).
                if (TotalSecondsAtStart < totalSeconds)
                    TotalSecondsAtStart = totalSeconds;

                MyProgressBar.Value = (int)(100 * (TotalSecondsAtStart - totalSeconds) / TotalSecondsAtStart);

                TaskbarManager taskbar = TaskbarManager.Instance;
                if (MyProgressBar.Value > 75)
                    taskbar.SetProgressState(TaskbarProgressBarState.Error);
                else if (MyProgressBar.Value > 50)
                    taskbar.SetProgressState(TaskbarProgressBarState.Paused);
                else
                    taskbar.SetProgressState(TaskbarProgressBarState.Normal);

                taskbar.SetProgressValue(MyProgressBar.Value, 100);
            }
            else
            {
                StartBTN.Text = "Start";
                TaskTimer.Enabled = false;

                ComputerTaskExecuter.Execute((ComputerTask)TaskSelector.SelectedIndex);
                Application.Exit();
            }
            KeepAwake();
        }

        private void StartBTN_Click(object sender, EventArgs e)
        {
            uint totalSeconds = GetTotalSeconds();
            TotalSecondsAtStart = totalSeconds;
            Text = OriginalFormTitle;
            MyProgressBar.Value = 0;

            if (MyDateTimePicker.Checked)
            {
                if (MyDateTimePicker.Value < DateTime.Now)
                {
                    MyDateTimePicker.Value = DateTime.Now;
                    MessageBox.Show("The time cannot be past time...");
                }
                else
                    SetCounts((uint)(MyDateTimePicker.Value - DateTime.Now).TotalSeconds);
            }

            if (!TaskTimer.Enabled)
                HoursCount.Enabled = MinutesCount.Enabled = SecondsCount.Enabled = true;

            TaskTimer.Enabled = !TaskTimer.Enabled;

            if (totalSeconds < 10 && TaskTimer.Enabled)
            {
                TaskTimer.Enabled = false;

                if (MessageBox.Show("Your timer is set to less than 10 seconds. Do you want to stop the timer?",
                    "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    TaskTimer.Enabled = true;
            }

            StartBTN.Text = !TaskTimer.Enabled ? "Start" : "Pause";
            MyDateTimePicker.Enabled = !TaskTimer.Enabled;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.Default.TaskIndex = TaskSelector.SelectedIndex;
            Settings.Default.Save();
        }

        private void MyDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if (!TaskTimer.Enabled)
                HoursCount.Enabled = MinutesCount.Enabled = SecondsCount.Enabled = !MyDateTimePicker.Checked;
        }

        private void MainForm_Deactivate(object sender, EventArgs e)
        {
            IsFormFocused = false;
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            IsFormFocused = true;
        }

        private void KeepAwake()
        {
            // Disable monitor sleep and keep system awake and prevent idle to sleep
            SetThreadExecutionState(EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_SYSTEM_REQUIRED);
        }
    }
}