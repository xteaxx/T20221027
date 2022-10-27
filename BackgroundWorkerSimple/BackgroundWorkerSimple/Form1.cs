using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BackgroundWorkerSimple
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            backgroundWorker1.WorkerReportsProgress = true;  // 取得或設定值，指出 BackgroundWorker 是否可以報告進度更新
            backgroundWorker1.WorkerSupportsCancellation = true;  // 取得或設定值，指出 BackgroundWorker 是否支援非同步取消
        }

        private void startAsyncButton_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy != true) // 取得值，指出 BackgroundWorker 是否正在執行非同步作業
            {
                backgroundWorker1.RunWorkerAsync();  // 開始執行背景作業 Start the asynchronous operation.
            }
        }

        private void cancelAsyncButton_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.WorkerSupportsCancellation == true)
            {
                backgroundWorker1.CancelAsync();  // 要求取消暫止的背景作業 Cancel the asynchronous operation.
            }
        }

        // This event handler is where the time-consuming work is done.
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            for (int i = 1; i <= 10; i++)
            {
                if (worker.CancellationPending == true)  // 取得值，指出應用程式是否已經要求取消背景作業
                {
                    e.Cancel = true;   // 取得或設定值，這個值表示是否應該取消事件。
                    break;
                }
                else
                {
                    // Perform a time consuming operation and report progress.
                    System.Threading.Thread.Sleep(500);
                    worker.ReportProgress(i * 10);  // 引發 ProgressChanged 事件
                }
            }
        }

        // This event handler updates the progress.
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            resultLabel.Text = (e.ProgressPercentage.ToString() + "%");
        }

        // This event handler deals with the results of the background operation.
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)   // 取得值，指出非同步作業是否已取消。
            {
                resultLabel.Text = "Canceled!";
            }
            else if (e.Error != null)   // 取得值，指出非同步作業期間是否發生錯誤
            {
                resultLabel.Text = "Error: " + e.Error.Message;
            }
            else
            {
                resultLabel.Text = "Done!";
            }
        }
    }
}
