using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PacketDotNet;
using SharpPcap;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Data.SQLite;
using System.Threading;
using FindAvailableMachines;

/*
 * This is a project done for the Computer Security course at Georgia Soutern University.
 * Dr. Jim Harris provided the code for sending and capturing packets. He is to be credited with that section of the code.
 * The purpose of this application is to find available machines on the subnet of which the machine running the application is on.
 * Ex. -> An IT machine will be able to scan IT lab machines an only IT lab machines. The same follows for other buildings.
 * The end goal of this application is for it to be used by IT Services at Georgia Southern University.
 */

namespace MyPacketCapturer
{
    public partial class frmCapture : Form
    {
        CaptureDeviceList devices; //List of devices for this computer
        public static ICaptureDevice device; //The device we will be using
        public static string stringPackets = ""; //Data that is captured
        public static HashSet<string> ipAddresses;
        public static StreamWriter file;
        string[] ITrooms = { "1201", "1202", "1203", "1204", "2204", "2212", "3208", "3210", "3212", "3214", "3302", "3314" };
        string[] COBArooms = { "1117", "1119" };
        string[] buildings = { "IT", "COBA", "COE", "ENG", "Carruth" };
        public static string hostname;
        static string[] hosts = new string[2040]; // if all hosts are used, there will be 2040 hostnames
        static SQLiteConnection dbConnect;
        static SQLiteCommand command;
        static string path = "master.sqlite"; // Path to database
        static List<string> machineTableContents;
        static List<string> hashSetContents;
        int scanLength = 6; // seconds
        ITScanner itScan;
        COBAScanner cobaScan;
        static string currentMachineIP;
        static string scannableSubnet;
        static int ipResponseCount = 0;

        public frmCapture()
        {
            InitializeComponent();
            devices = CaptureDeviceList.Instance;

            //Make sure that there is at least one device
            if (devices.Count < 1)
            {
                MessageBox.Show("No Capture Devices Found!!!");
                Application.Exit();
            }

            /*
            //Add the devices to the combo box
            foreach (ICaptureDevice dev in devices)
            {
                cmbDevices.Items.Add(dev.Description);
            }
            */

            //Get the second device and display in combo box
            ICaptureDevice[] cap = devices.ToArray();
            for (int i = 0; i < cap.Length; i++)
            {
                string str = Convert.ToString(cap[i]);
                if (str.Contains("Intel")) // Using "Intel" to find ethernet on campus machine
                {
                    device = cap[i];
                    //cmbDevices.Text = device.Description;
                }
            }

            setUpUI(this);

            // Set up database
            if (!File.Exists(path))
            {
                Debug.Write("Database file does not exist");
                SQLiteConnection.CreateFile(path);
                // Connect to database and create table
                dbConnect = new SQLiteConnection("Data Source=" + path + ";Version=3;");
                dbConnect.Open();
                string table = "create table machines (ip varchar(30), host varchar(30))";
                command = new SQLiteCommand(table, dbConnect);
                command.ExecuteNonQuery();
            } else
            {
                Debug.Write("Database exists");
            }

            dbConnect = new SQLiteConnection("Data Source=" + path + ";Version=3;");
            dbConnect.Open();

            //Register our handler function to the "packet arrival" event
            device.OnPacketArrival += new SharpPcap.PacketArrivalEventHandler(device_OnPacketArrival);

            //Open the device for capturing
            int readTimeoutMilliseconds = 1000;
            device.Open(DeviceMode.Promiscuous, readTimeoutMilliseconds);

            //Register our handler function to the "column click" event
            this.ipCaptureList.ColumnClick += new ColumnClickEventHandler(ColumnClick);

            progressLabel.Visible = false;

            string machineHostName = Dns.GetHostName();
            currentMachineIP = Dns.GetHostByName(machineHostName).AddressList[2].ToString();
            this.currentIPLbl.Text = currentMachineIP;

            string [] ipSeg = currentMachineIP.Split('.');
            scannableSubnet = getScannableSubnet(ipSeg[2]);
            this.bldgLbl.Text = scannableSubnet;

            this.scanBtn.Text = "Scan " + scannableSubnet + " building";

            Debug.Write(System.IO.Directory.GetCurrentDirectory());
        }

        private void device_OnPacketArrival(object sender, CaptureEventArgs packet)
        {
            //Array to store our data
            byte[] data = packet.Packet.Data;

            if (data[34] == 0)
            {
                ipResponseCount++;
                string srcIP = data[26] + "." + data[27] + "." + data[28] + "." + data[29];
                ipAddresses.Add(srcIP); // adds source IP to HashSet
                ipCaptureList.Invoke(new Action(() => ipCaptureList.Items.Add(srcIP)));
            }
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            ipResponseCount = 0;
            progressLabel.Visible = true;
            scanBtn.Enabled = false;
            tbPacketSend.Text = System.IO.File.ReadAllText("master_icmp.txt");
            progressLabel.Text = "Scanning subnets";
            ipCountLbl.Text = "0";
            try
            {
                string filter = "icmp"; // Create capture filter
                device.Filter = filter;
                ipAddresses = new HashSet<string>();
                device.StartCapture();
                scanSubnet(this);
                dbProgress.Maximum = scanLength;
                dbProgress.Value = 0;
                dbTimer.Start();
                //dbProgress.Maximum = ipAddresses.Count;
            }
            catch (Exception exp)
            {

            }
        }

        public static string getHostName(string ipAddress)
        {
            try
            {
                IPHostEntry entry = Dns.GetHostEntry(ipAddress);
                if (entry != null)
                {
                    return entry.HostName;
                }
                else
                {
                    return null;
                }
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                Debug.Write(ipAddress);
                return null;
            }
        }

        private void roomSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bldgLbl.Text == "CEIT")
            {
                this.machineCountLbl.Text = "Calculating...";
                int machineCount = itScan.resolveUsers();
                this.machineCountLbl.Text = Convert.ToString(machineCount);
            } else if (bldgLbl.Text == "COBA")
            {
                cobaScan.resolveUsers();
            }
        }

        private static void doFunDatabaseThings(frmCapture form)
        {
            form.dbProgress.Value = 0;
            form.progressLabel.Text = "Removing and Adding IPs/Host to Database";
            Debug.Write("Starting database stuff");

            // Get number of rows from 'machines' table
            string sql = "select count(*) from machines";
            command = new SQLiteCommand(sql, dbConnect);
            int count = Convert.ToInt32(command.ExecuteScalar());

            if (count > 0)
            {
                Debug.Write("Machines table has data");
                string readData = "select * from machines";
                machineTableContents = new List<string>();
                hashSetContents = new List<string>();
                command = new SQLiteCommand(readData, dbConnect);
                SQLiteDataReader dr = command.ExecuteReader();

                // Load table contents into list
                while (dr.Read())
                {
                    machineTableContents.Add(dr["ip"].ToString());
                }
                // Load hashSet contents into list
                foreach (string ip in ipAddresses)
                {
                    hashSetContents.Add(ip);
                }

                var ipsToAdd = hashSetContents.Except(machineTableContents); // to add
                var ipsToRemove = machineTableContents.Except(hashSetContents); // to remove

                form.dbProgress.Maximum = ipsToAdd.Count() + ipsToRemove.Count();

                foreach (string ip in ipsToRemove)
                {
                    string delete = "delete from machines where ip = '" + ip + "'";
                    Debug.Write(ip + " was removed from the table");
                    command = new SQLiteCommand(delete, dbConnect);
                    command.ExecuteNonQuery();
                    form.dbProgress.Value++;
                }

                foreach (string ip in ipsToAdd)
                {
                    // These IPs are subject to change, this are currently static IPs that do not belong to lab machines
                    if (ip == "141.165.208.112" || ip == "141.165.210.15" || ip == "141.165.210.83" || ip == "141.165.211.73" || ip == "141.165.209.173")
                    {
                        Debug.Write(ip + " skipped");
                        continue;
                    }
                    string fullHostName = getHostName(ip);
                    if (fullHostName == null || fullHostName == "")
                    {
                        continue;
                    }
                    string[] host = fullHostName.Split('.');
                    Console.WriteLine("Adding " + host[0] + " to the database");
                    string insert = "insert into machines (ip, host) values ('" + ip + "', '" + host[0] + "')";
                    Debug.Write(host[0] + " with an IP Address of " + ip + " was added");
                    command = new SQLiteCommand(insert, dbConnect);
                    command.ExecuteNonQuery();
                    form.dbProgress.Value++;
                }

            }
            else
            { // table is empty, process takes ~20 minutes
                Debug.Write("Machines table is empty");
                form.dbProgress.Maximum = ipAddresses.Count;
                foreach (string ip in ipAddresses)
                {
                    string fullHostName = getHostName(ip);
                    string[] host = fullHostName.Split('.');
                    string insert = "insert into machines (ip, host) values ('" + ip + "', '" + host[0] + "')";
                    Debug.Write(host[0] + " with an IP Address of " + ip + " was added");
                    command = new SQLiteCommand(insert, dbConnect);
                    command.ExecuteNonQuery();
                    form.dbProgress.Value++;
                }
            }

            Debug.Write("Ending database stuff");
            form.dbProgress.Value = form.dbProgress.Maximum;
            form.progressLabel.Text = "Ready to view available machines";
        }

        int timerCount = 0;
        private void dbTimer_Tick(object sender, EventArgs e)
        {
            timerCount++;
            if (timerCount == 1) {
                progressLabel.Text = "Finishing up scan";
            }
            dbProgress.Value++;
            if (timerCount == scanLength)
            {
                ipCountLbl.Text = Convert.ToString(ipResponseCount);
                device.StopCapture();
                progressLabel.Text = "Updating database";
                doFunDatabaseThings(this);
                timerCount = 0;
                dbTimer.Stop();
                scanBtn.Enabled = true;
            } 
        }

        private void scanSubnet(frmCapture form)
        {
            if (form.bldgLbl.Text == "CEIT")
            {
                itScan.scan();
            } else if (form.bldgLbl.Text == "COBA")
            {
                cobaScan.scan();
            }
        }

        public SQLiteConnection getDatabaseConnection()
        {
            return dbConnect;
        }

        private void setUpUI(frmCapture form)
        {
            form.ipCaptureList.Columns.Add("IP Addresses");
            form.ipCaptureList.Columns[0].Width = -2;

            form.machineList.Columns.Add("Machine Name");
            form.machineList.Columns.Add("User");
            form.machineList.Columns[0].Width = 200;
            form.machineList.Columns[1].Width = 160;

            form.machineCountLbl.Text = "0";
            form.ipCountLbl.Text = "0";
        }

        // Not functional yet
        private void ColumnClick(object o, ColumnClickEventArgs e)
        {
            Debug.Write(this.ipCaptureList.Columns[0]);
            int ipCount = this.ipCaptureList.Items.Count;
            Debug.Write(ipAddresses);

            // More than one IP Address in the listview
            if (ipCount > 1)
            {
                this.ipCaptureList.Sort();
            }
        }

        private void devLabel1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Email: kruseshane057@gmail.com\nGitHub: https://github.com/kruseshane", "Developer Information");
        }

        private void devLabel2_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Version: 1.0\nLast Updated: 11/12/2018", "Product Information");
        }

        private string getScannableSubnet(string subnet)
        {
            int subnetNum = Convert.ToInt32(subnet);
            if (subnetNum == 18)
            {
                return "Carruth";
            } else if (subnetNum == 37 || subnetNum == 38)
            {
                return "COE";
            } else if (subnetNum >= 208 && subnetNum <= 215)
            {
                itScan = new ITScanner(this);
                for (int i = 0; i < ITrooms.Length; i++)
                {
                    roomSelection.Items.Add(ITrooms[i]);
                }
                return "CEIT";
            } else if (subnetNum == 230 || subnetNum == 231)
            {
                cobaScan = new COBAScanner(this);
                for (int i = 0; i < COBArooms.Length; i++)
                {
                    roomSelection.Items.Add(COBArooms[i]);
                }
                return "COBA";
            } else if (subnetNum >= 233 && subnetNum <= 235)
            {
                return "EENG";
            } else
            {
                return "N/A";
            }
        }

        private void frmCapture_Load(object sender, EventArgs e)
        {

        }
    }

    
}
