using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Threading;
using Rug.Osc;

namespace OSC_Terminal
{
    public partial class FormTerminal : Form
    {
        #region Variables and objects

        /// <summary>
        /// Timer to update terminal textbox at fixed interval.
        /// </summary>
        private System.Windows.Forms.Timer formUpdateTimer = new System.Windows.Forms.Timer();

        /// <summary>
        /// Sample counter to calculate performance statics.
        /// </summary>
        private RateLogger packetCounter = new RateLogger();

        /// <summary>
        /// TextBoxBuffer containing text printed to terminal.
        /// </summary>
        private TextBoxBuffer textBoxBuffer = new TextBoxBuffer(4096);

        /// <summary>
        /// Receive port history
        /// </summary>
        private List<ushort> receivePorts = new List<ushort>();

        private OscListenerManager m_Listener;
        private OscReceiver m_Receiver;
        private Thread m_Thread;

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public FormTerminal()
        {
            InitializeComponent();
        }

        #region Form load and close

        /// <summary>
        /// From load event.
        /// </summary>
        private void FormTerminal_Load(object sender, EventArgs e)
        {
            // Set form caption
            this.Text = Assembly.GetExecutingAssembly().GetName().Name;

            // Set default port
            OpenReceiver(8000);

            // Setup form update timer
            formUpdateTimer.Interval = 50;
            formUpdateTimer.Tick += new EventHandler(formUpdateTimer_Tick);
            formUpdateTimer.Start();

            //set up log menu options
            pauseLogToolStripMenuItem.Enabled = false;
            stopLoggingToolStripMenuItem.Enabled = false;

            //init log info
            toolStripStatusLogInfo.Text = "Log Stopped";
        }

        private void FormTerminal_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_Receiver.Close();
            m_Thread.Join();
        }

        #endregion

        #region Terminal textbox

        /// <summary>
        /// formUpdateTimer Tick event to update terminal textbox.
        /// </summary>
        void formUpdateTimer_Tick(object sender, EventArgs e)
        {
            // Print textBoxBuffer to terminal
            if (textBox.Enabled && !textBoxBuffer.IsEmpty())
            {
                textBox.AppendText(textBoxBuffer.Get());
                if (textBox.Text.Length > textBox.MaxLength)    // discard first half of textBox when number of characters exceeds length
                {
                    textBox.Text = textBox.Text.Substring(textBox.Text.Length / 2, textBox.Text.Length - textBox.Text.Length / 2);
                }
            }
            else
            {
                textBoxBuffer.Clear();
            }

            // Update sample counter values
            toolStripStatusLabelPacketsReceived.Text = "Packets Recieved: " + packetCounter.PacketsReceived.ToString();
            toolStripStatusLabelPacketRate.Text = "Packet Rate: " + packetCounter.PacketRate.ToString();
            toolStripStatusDataRate.Text = "Data Rate: " + packetCounter.DataRate().ToString();
        }

        #endregion

        #region Menu strip

        /// <summary>
        /// toolStripMenuItemReceivePort DropDownItemClicked event to set the receive port
        /// </summary>
        private void toolStripMenuItemReceivePort_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ushort port;
            if (((ToolStripMenuItem)e.ClickedItem).Text == "...")
            {
                FormGetValue formGetValue = new FormGetValue();
                formGetValue.ShowDialog();
                try
                {
                    port = ushort.Parse(formGetValue.value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
            }
            else
            {
                port = ushort.Parse(((ToolStripMenuItem)e.ClickedItem).Text);
            }
            OpenReceiver(port);
        }

        /// <summary>
        /// toolStripMenuItemEnabled CheckStateChanged event to toggle enabled state of the terminal text box.
        /// </summary>
        private void toolStripMenuItemEnabled_CheckStateChanged(object sender, EventArgs e)
        {
            if (toolStripMenuItemEnabled.Checked)
            {
                textBox.Enabled = true;
            }
            else
            {
                textBox.Enabled = false;
            }
        }

        /// <summary>
        /// toolStripMenuItemClear Click event to clear terminal text box.
        /// </summary>
        private void toolStripMenuItemClear_Click(object sender, EventArgs e)
        {
            textBox.Text = "";
        }

        /// <summary>
        /// toolStripMenuItemAbout Click event to display version details.
        /// </summary>
        private void toolStripMenuItemAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Assembly.GetExecutingAssembly().GetName().Name + " " + Assembly.GetExecutingAssembly().GetName().Version.Major.ToString() + "." + Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString(), "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// toolStripMenuItemSourceCode Click event to open web browser.
        /// </summary>
        private void toolStripMenuItemSourceCode_Click(object sender, EventArgs e)
        {
            try
            {
                //System.Diagnostics.Process.Start("https://github.com/xioTechnologies/OSC-Terminal");
            }
            catch { }
        }

        #endregion

        #region OSC receiver

        private void OpenReceiver(ushort port)
        {
            // Update port list
            if (!receivePorts.Contains(port))
            {
                receivePorts.Add(port);
                receivePorts.Sort();
            }
            toolStripMenuItemReceivePort.DropDownItems.Clear();
            foreach (ushort p in receivePorts)
            {
                toolStripMenuItemReceivePort.DropDownItems.Add(p.ToString());
            }
            toolStripMenuItemReceivePort.DropDownItems.Add("...");

            // Check selected port
            foreach (ToolStripMenuItem toolStripMenuItem in toolStripMenuItemReceivePort.DropDownItems)
            {
                if (toolStripMenuItem.Text == port.ToString())
                {
                    toolStripMenuItem.Checked = true;
                }
            }

            // Open reciever
            if (m_Receiver != null)
            {
                m_Receiver.Close();
            }
            if (m_Thread != null)
            {
                m_Thread.Join();
            }
            m_Listener = new OscListenerManager();
            m_Receiver = new OscReceiver(port);
            m_Thread = new Thread(new ThreadStart(ListenLoop));
            m_Receiver.Connect();
            m_Thread.Start();
        }

        private void ListenLoop()
        {
            try
            {
                while (m_Receiver.State != OscSocketState.Closed)
                {
                    // if we are in a state to recieve
                    if (m_Receiver.State == OscSocketState.Connected)
                    {
                        // get the next message 
                        // this will block until one arrives or the socket is closed
                        OscPacket packet = m_Receiver.Receive();

                        switch (m_Listener.ShouldInvoke(packet))
                        {
                            case OscPacketInvokeAction.Invoke:
                                packetCounter.Increment();
                                //textBoxBuffer.WriteLine("Received packet");
                                textBoxBuffer.WriteLine(packet.ToString());
                                m_Listener.Invoke(packet);
                                break;
                            case OscPacketInvokeAction.DontInvoke:
                                textBoxBuffer.WriteLine("Cannot invoke");
                                textBoxBuffer.WriteLine(packet.ToString());
                                break;
                            case OscPacketInvokeAction.HasError:
                                textBoxBuffer.WriteLine("Error reading osc packet, " + packet.Error);
                                textBoxBuffer.WriteLine(packet.ErrorMessage);
                                break;
                            case OscPacketInvokeAction.Pospone:
                                textBoxBuffer.WriteLine("Posponed bundle");
                                textBoxBuffer.WriteLine(packet.ToString());
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // if the socket was connected when this happens
                // then tell the user
                if (m_Receiver.State == OscSocketState.Connected)
                {
                    textBoxBuffer.WriteLine("Exception in listen loop");
                    textBoxBuffer.WriteLine(ex.Message);
                }
            }
        }

        #endregion

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //check to see if log is running still
            if (packetCounter.logState != RateLogger.logStatus.log_stopped)
            {
                try
                {
                    packetCounter.stopLog();
                }
                catch (RateLogger.logNotRunningException a)
                {
                    MessageBox.Show("Cannot stop log as it is not currently running", "Log not running", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            this.Close();
        }

        /// <summary>
        /// Handles a button press on the start log button
        /// Opens a fileOpen dialog to allow user to select log filepath
        /// and then starts logging operation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Set up save file dialog
            SaveFileDialog saveLog = new SaveFileDialog();
            saveLog.CheckPathExists = true;
            saveLog.DefaultExt = "txt";
            saveLog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveLog.ValidateNames = true;
            saveLog.Title = "Save Log File";
            if (saveLog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    packetCounter.startLog(1, saveLog.FileName);
                }
                catch (RateLogger.logAlreadyRunningException a)
                {
                    MessageBox.Show("Cannot start log - log already running. Please stop the logging operation and try again.", "Log Already Running", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //cheange GUI controls to reflect the log is running
                startLogToolStripMenuItem.Enabled = false;
                pauseLogToolStripMenuItem.Enabled = true;
                stopLoggingToolStripMenuItem.Enabled = true;
                toolStripStatusLogInfo.Text = "Log Running";
            }
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles an event triggered by the pause log button being pressed.
        /// Pauses and resumes logging and toggles button text
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">send parameters</param>
        private void pauseLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (packetCounter.logState == RateLogger.logStatus.log_running)//check to see if log is running
            {
                //attempt to pause log
                try
                {
                    packetCounter.pauseLog();
                }
                catch (RateLogger.logNotRunningException a)
                {
                    MessageBox.Show("Cannot pause log - log is not running", "Log not running", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Update GUI Controls
                pauseLogToolStripMenuItem.Text = "Resume log";
                toolStripStatusLogInfo.Text = "Log Paused";
            }
            else
            {
                //attempt to resume log
                try
                {
                    packetCounter.resumeLog();
                }
                catch (RateLogger.logAlreadyRunningException a)
                {
                    MessageBox.Show("Cannot resume log - log is already", "Log already running", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Update GUI Controls
                pauseLogToolStripMenuItem.Text = "Pause log";
                toolStripStatusLogInfo.Text = "Log Running";
            }
        }

        /// <summary>
        /// Handles a button press on the stop log button
        /// Halts logging operation
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Object parameters</param>
        private void stopLoggingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //try to stop log
            try
            {
                packetCounter.stopLog();
            }
            catch(RateLogger.logNotRunningException a)
            {
                MessageBox.Show("Cannot stop log as it is not currently running", "Log not running", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            pauseLogToolStripMenuItem.Enabled = false;
            pauseLogToolStripMenuItem.Text = "Pause log";
            stopLoggingToolStripMenuItem.Enabled = false;
            startLogToolStripMenuItem.Enabled = true;
            toolStripStatusLogInfo.Text = "Log Stopped";
            MessageBox.Show("Logging completed sucessfully at: " + DateTime.Now, "Logging complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
