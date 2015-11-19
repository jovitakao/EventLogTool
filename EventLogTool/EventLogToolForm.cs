using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;
using NCCUCS.AspectW;

namespace EventLogTool
{
    public partial class EventLogToolForm : Form
    {
        public EventLogToolForm()
        {
            InitializeComponent();
        }

        private void EventLogToolForm_Load(object sender, EventArgs e)
        {
            // init. cboEntryType
            ArrayList cboEntryTypeItems = new ArrayList();
            cboEntryTypeItems.Add(new KeyValuePair<EventLogEntryType, string>(EventLogEntryType.Information, EventLogEntryType.Information.ToString()));
            cboEntryTypeItems.Add(new KeyValuePair<EventLogEntryType, string>(EventLogEntryType.SuccessAudit, EventLogEntryType.SuccessAudit.ToString()));
            cboEntryTypeItems.Add(new KeyValuePair<EventLogEntryType, string>(EventLogEntryType.Warning, EventLogEntryType.Warning.ToString()));
            cboEntryTypeItems.Add(new KeyValuePair<EventLogEntryType, string>(EventLogEntryType.Error, EventLogEntryType.Error.ToString()));
            cboEntryTypeItems.Add(new KeyValuePair<EventLogEntryType, string>(EventLogEntryType.FailureAudit, EventLogEntryType.FailureAudit.ToString()));
            //
            cboEntryType.DataSource = cboEntryTypeItems;
            cboEntryType.ValueMember = "Key";
            cboEntryType.DisplayMember = "Value";
            cboEntryType.SelectedIndex = 0;
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            AspectW.Define
                .WaitCursor(this, btnCheck)
                .Validate(txtSource.Text.Trim() != string.Empty, "Source為必填。")
                .Validate(txtMachineName.Text.Trim() != string.Empty, "Machine Name為必填。")
                .Ignore()
                .TraceException((ex) => txtOutput.AppendText(ex.Message + "\r\n"))
                .Do(() =>
                {
                    string source = txtSource.Text.Trim();
                    string machineName = txtMachineName.Text.Trim();
                    if (EventLog.SourceExists(source, machineName))
                    {
                        txtOutput.AppendText("已存在。\r\n");
                    }
                    else
                    {
                        txtOutput.AppendText("不存在。\r\n");
                    }
                });
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            AspectW.Define
                .WaitCursor(this, btnCheck)
                .Validate(txtSource.Text.Trim() != string.Empty, "Source為必填。")
                .Validate(txtMachineName.Text.Trim() != string.Empty, "Machine Name為必填。")
                .Ignore()
                .TraceException((ex) => txtOutput.AppendText(ex.Message + "\r\n"))
                .Do(() =>
                {
                    string source = txtSource.Text.Trim();
                    string machineName = txtMachineName.Text.Trim();
                    EventLog.CreateEventSource(source, source);
                });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AspectW.Define
            .WaitCursor(this, btnCheck)
            .Validate(txtSource.Text.Trim() != string.Empty, "Source為必填。")
            .Validate(txtMachineName.Text.Trim() != string.Empty, "Machine Name為必填。")
            .Ignore()
            .TraceException((ex) => txtOutput.AppendText(ex.Message + "\r\n"))
            .Do(() =>
            {
                string source = txtSource.Text.Trim();
                string machineName = txtMachineName.Text.Trim();
                string message = txtMessage.Text;
                EventLogEntryType eventType = (EventLogEntryType)cboEntryType.SelectedValue;

                using(EventLog loger = new EventLog(source, machineName, source))
                {
                    loger.WriteEntry(message, eventType);
                    txtOutput.AppendText("已寫入一筆Log到系統。\r\n");
                }
            });


        }

    }
}
