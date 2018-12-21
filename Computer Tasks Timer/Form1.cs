﻿using System;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace Computer_Tasks_Timer
{
    public partial class MainForm : Form
    {
        int totalSecondsAtStart = 0;
        string formTitle;
        bool isFormFocused = true;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SendMessage(int hWnd, int hMsg, int wParam, int lParam);
        public MainForm(string[] args)
        {
            InitializeComponent();
            if (args.Length > 0)
            {   //Starts the timer with the seconds set by an outside program.
                SetCounts(int.Parse(args[0]));
                StartBTN_Click(null, EventArgs.Empty);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            formTitle = this.Text;
            TaskSelector.SelectedIndex = Properties.Settings.Default.TaskIndex;
            MyDateTimePicker.CustomFormat = "MM/dd/yyyy H:mm:ss";
        }

        private int GetTotalSeconds()
        {
            return (int)(SecondsCount.Value + MinutesCount.Value * 60 + HoursCount.Value * 3600);
        }

        private void SetCounts(int totalSeconds)
        {
            SecondsCount.Value = MinutesCount.Value = HoursCount.Value = 0;
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

        enum Tasks
        {
            Shutdown,
            Restart,
            Lock,
            Sleep,
            Hibernate,
            ScreenOff,
            ScreenOffAndLock,
            SignOut
        }

        private void TaskTimer_Tick(object sender, EventArgs e)
        {
            MyDateTimePicker.Value = DateTime.Now.AddSeconds(GetTotalSeconds());
            MyDateTimePicker.Checked = false;
            if (GetTotalSeconds() > 0)
            {
                SetCounts(GetTotalSeconds() - 1);
                if (!isFormFocused)
                {
                    this.Text = "";
                    if (HoursCount.Value != 0)
                        this.Text = $"{(HoursCount.Value < 10 ? "0" : "") + HoursCount.Value}:";
                    if (MinutesCount.Value != 0)
                        this.Text += $"{(MinutesCount.Value < 10 ? "0" : "") + MinutesCount.Value}:";
                    this.Text += $"{(SecondsCount.Value < 10 ? "0" : "") + SecondsCount.Value}";
                }
                else
                    this.Text = formTitle;
                if (totalSecondsAtStart < GetTotalSeconds()) // If the user changes the seconds after starting the timer (Prevent a negative value later)
                    totalSecondsAtStart = GetTotalSeconds();
                MyProgressBar.Value = 100 * (totalSecondsAtStart - GetTotalSeconds()) / totalSecondsAtStart;
                TaskbarManager.Instance.SetProgressValue(MyProgressBar.Value, 100);
                if (MyProgressBar.Value > 75)
                    TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Error);
                else if (MyProgressBar.Value > 50)
                    TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Paused);
                else
                    TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
            }
            else
            {
                StartBTN.Text = "Start";
                TaskTimer.Enabled = false;
                switch ((Tasks)TaskSelector.SelectedIndex)
                {
                    case Tasks.Shutdown:
                        System.Diagnostics.Process.Start("shutdown.exe", "/s /t 0");
                        break;
                    case Tasks.Restart:
                        System.Diagnostics.Process.Start("shutdown.exe", "/r /t 0");
                        break;
                    case Tasks.Sleep:
                        System.Diagnostics.Process.Start("rundll32.exe", "powrprof.dll,SetSuspendState 0,1,0");
                        break;
                    case Tasks.ScreenOff:
                        SendMessage(0xFFFF, 0x112, 0xF170, 2);
                        break;
                    case Tasks.ScreenOffAndLock:
                        System.Diagnostics.Process.Start("Rundll32.exe", "User32.dll,LockWorkStation");
                        SendMessage(0xFFFF, 0x112, 0xF170, 2);
                        break;
                    case Tasks.Lock:
                        System.Diagnostics.Process.Start("Rundll32.exe", "User32.dll,LockWorkStation");
                        break;
                    case Tasks.Hibernate:
                        System.Diagnostics.Process.Start("shutdown.exe", "/h");
                        break;
                    case Tasks.SignOut:
                        System.Diagnostics.Process.Start("shutdown.exe", "/l");
                        break;
                    default:
                        MessageBox.Show("Unknown task");
                        break;
                }
                Application.Exit();
            }
        }

        private void StartBTN_Click(object sender, EventArgs e)
        {
            this.Text = formTitle;
            totalSecondsAtStart = GetTotalSeconds();
            MyProgressBar.Value = 0;
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
                HoursCount.Enabled = MinutesCount.Enabled = SecondsCount.Enabled = true;
            TaskTimer.Enabled = !TaskTimer.Enabled;
            if (GetTotalSeconds() < 10 && TaskTimer.Enabled)
            {
                TaskTimer.Enabled = false;
                DialogResult dialogResult = MessageBox.Show("Your timer is set to less than 10 seconds. Do you want to stop the timer?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.No)
                    TaskTimer.Enabled = true;
            }
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
                HoursCount.Enabled = MinutesCount.Enabled = SecondsCount.Enabled = !MyDateTimePicker.Checked;
        }

        private void MainForm_Deactivate(object sender, EventArgs e)
        {
            isFormFocused = false;
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            isFormFocused = true;
        }
    }
}