using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;

namespace Netwroker_Scanner
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(500);
            Ping ping;
            IPAddress addr;
            PingReply pingReply;
            IPHostEntry host;
            string name;

             
            Parallel.For(0, 254, (i, loopstate) =>
            {
                ping = new Ping();
                pingReply = ping.Send(textBox1.Text + i.ToString());
                this.BeginInvoke((Action)delegate()
                {
                    if (pingReply.Status == IPStatus.Success)
                    {
                        try
                        {
                            addr = IPAddress.Parse(textBox1.Text + i.ToString());
                            host = Dns.GetHostEntry(addr);
                            name = host.HostName;

                            dataGridView1.Rows.Add();
                            int nRowIndex = dataGridView1.Rows.Count - 1;
                            dataGridView1.Rows[nRowIndex].Cells[0].Value = textBox1.Text + i.ToString();
                            dataGridView1.Rows[nRowIndex].Cells[1].Value = name;
                            System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
                            string macAddress;
                            pProcess.StartInfo.FileName = "arp";
                            pProcess.StartInfo.Arguments = "-a " + dataGridView1.Rows[nRowIndex].Cells[0].Value.ToString();
                            pProcess.StartInfo.UseShellExecute = false;
                            pProcess.StartInfo.RedirectStandardOutput = true;
                            pProcess.StartInfo.CreateNoWindow = true;
                            pProcess.Start();
                            string strOutput = pProcess.StandardOutput.ReadToEnd();
                            string[] substrings = strOutput.Split('-');
                            if (substrings.Length >= 8)
                            {
                                macAddress = substrings[3].Substring(Math.Max(0, substrings[3].Length - 2))
                                         + "-" + substrings[4] + "-" + substrings[5] + "-" + substrings[6]
                                         + "-" + substrings[7] + "-"
                                         + substrings[8].Substring(0, 2);
                                dataGridView1.Rows[nRowIndex].Cells[2].Value = macAddress;
                                // return macAddress;

                                label1.Text = macAddress;
                                string IPP;
                                IPP = label1.Text;
                                IPP = IPP.Remove(IPP.Length - 9);
                                switch (IPP)
                                {
                                    case "98-E7-43":
                                        dataGridView1.Rows[nRowIndex].Cells[3].Value = "Dell INC";
                                        break;
                                    case "C8-F7-50":
                                        dataGridView1.Rows[nRowIndex].Cells[3].Value = "Dell Inc";
                                        break;
                                    case "6C-2B-59":
                                        dataGridView1.Rows[nRowIndex].Cells[3].Value = "Dell Inc";
                                        break;
                                    case "c0-b8-83":
                                        dataGridView1.Rows[nRowIndex].Cells[3].Value = "HP";
                                        break;
                                    case "88-3A-30":
                                        dataGridView1.Rows[nRowIndex].Cells[3].Value = "Hewlett Packard Enterprise Company";
                                        break;
                                    case "D4-6B-A6":
                                        dataGridView1.Rows[nRowIndex].Cells[3].Value = "HUAWEI TECHNOLOGIES CO.,LTD";
                                        break;
                                    case "D0-43-1E":
                                        dataGridView1.Rows[nRowIndex].Cells[3].Value = "Dell Inc";
                                        break;
                                    case "E4-54-E8":
                                        dataGridView1.Rows[nRowIndex].Cells[3].Value = "Dell INC";
                                        break;
                                    case "8C-04-BA":
                                        dataGridView1.Rows[nRowIndex].Cells[3].Value = "Dell Inc";
                                        break;
                                    case "F0-D4-E2":
                                        dataGridView1.Rows[nRowIndex].Cells[3].Value = "Dell Inc";
                                        break;
                                    case "E4-43-4B":
                                        dataGridView1.Rows[nRowIndex].Cells[3].Value = "Dell Inc";
                                        break;
                                    case "D0-67-E5":
                                        dataGridView1.Rows[nRowIndex].Cells[3].Value = "Dell Inc";
                                        break;
                                    case "28-F1-0E":
                                        dataGridView1.Rows[nRowIndex].Cells[3].Value = "Dell inc";
                                        break;
                                    case "6c-d9-4c":
                                        dataGridView1.Rows[nRowIndex].Cells[3].Value = "Vivo";
                                        break;
                                    default:
                                        dataGridView1.Rows[nRowIndex].Cells[3].Value = "Vender not found";
                                        break;
                                }
                            }

                            else
                            {
                                //  return "not found";
                            }
                            // dataGridView1.Rows[nRowIndex].Cells[2].Value = "Active";
                        }
                        catch (SocketException ex)
                        {
                            name = "?";

                        }
                    }
                });

            });
            MessageBox.Show("Scan complete");
            var macAddr =
        (
            from nic in NetworkInterface.GetAllNetworkInterfaces()
            where nic.OperationalStatus == OperationalStatus.Up
            select nic.GetPhysicalAddress().ToString()
        ).FirstOrDefault();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IPHostEntry host;
            string localIP = "?";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    string IPP;
                    IPP = localIP = ip.ToString();
                    string[] alloctates = IPP.Split('.');

                    foreach (string str in alloctates)
                    {
                        IPADD.Text = IPADD.Text + str + "\n";
                    }

                    int value1 = Int32.Parse(alloctates[0]);
                    int value2 = Int32.Parse(alloctates[1]);
                    int value3 = Int32.Parse(alloctates[2]);
                    IPADD.Text = value1 + "." + value2 + "." + value3 + ".";
                }

            if (IPADD.Text == "127.0.0.")
            {
                MessageBox.Show("No internet connection Found");
            }
            else
            {
                textBox1.Text = IPADD.Text;
                backgroundWorker1.RunWorkerAsync();
            }
            string txt;
            txt = IPADD.Text;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            option f2 = new option();
            f2.Get_ip = this.dataGridView1.CurrentRow.Cells[0].Value.ToString();
            f2.Get_Host = this.dataGridView1.CurrentRow.Cells[1].Value.ToString();
            f2.ShowDialog();
        }
    }
}
 
