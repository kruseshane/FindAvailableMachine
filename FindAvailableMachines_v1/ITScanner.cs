using MyPacketCapturer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FindAvailableMachines
{
    class ITScanner : Scanner
    {
        frmCapture form;
        static string[] subnets = { "208", "209", "210", "211", "212", "213", "214", "215" };
        static string[] subnetHexes = { "D0", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8" };
        string[] ITrooms = { "1201", "1202", "1203", "1204", "2204", "2212", "3208", "3210", "3212", "3214", "3302", "3314" }; // 2208 and 2210 ommited obvious reasons
        static SqlConnection dbConnect_IT;

        public ITScanner(frmCapture form)
        {
            this.form = form;
        }

        public void scan()
        {
            form.dbProgress.Value = 0;
            string stringBytes = " ";
            //Get the hex values from the npg file
            foreach (string s in form.tbPacketSend.Lines)
            {
                //Taking out comments
                string[] noComments = s.Split('#');
                string s1 = noComments[0];
                stringBytes += s1 + Environment.NewLine;
            }

            //Extract the values in a byte array
            string[] sBytes = stringBytes.Split(new string[] { "\n", "\r\n", " ", "\t" },
                StringSplitOptions.RemoveEmptyEntries);

            //Send the packet
            string ipStart = "141.165.208.0";
            string[] ipAddrs = ipStart.Split('.');
            int hostCounter = 0;
            int count = 0;
            string hex1 = "";
            string hex2 = "";
            Boolean subnetFinished = false;
            string[] currentIP = getSubnetHostHex();
            string[] ipHeaderSegs = new string[10];

            /*
             * Double for loop implementation not working. Not necessary but
             * but it will make the code more manageable
            */

            /*
            for (int i = 0; i < subnets.Length; i++)
            {
                for (int j = 0; j < 256; j++)
                {
                    hex1 = subnetHexes[i];
                    //string host = Convert.ToString(j);
                    //int x = Convert.ToInt32(host);
                    string bin = Convert.ToString(j, 2);
                    hex2 = Convert.ToInt32(bin, 2).ToString("X");
                    if (hex2.Length == 1)
                    {
                        hex2 = hex2.PadLeft(2, '0');
                    }

                    sBytes[24] = "00";
                    sBytes[25] = "00";
                    sBytes[28] = currentIP[0];
                    sBytes[29] = currentIP[1];
                    sBytes[32] = hex1;
                    sBytes[33] = hex2;

                    for (int c = 14; c < 34; c += 2)
                    {
                        ipHeaderSegs[c - 14 - count] = sBytes[c] + sBytes[c + 1];
                        count++;
                    }

                    count = 0;

                    string ipHeaderFull = string.Join(" ", ipHeaderSegs);
                    string checksum = calculateIPHeaderChecksum(ipHeaderFull);

                    sBytes[24] = checksum.Substring(0, 2);
                    sBytes[25] = checksum.Substring(2);

                    //Change the strings into bytes
                    byte[] packet = new byte[sBytes.Length];
                    int counter = 0;
                    
                    foreach (string s in sBytes)
                    {
                        packet[i] = Convert.ToByte(s, 16);
                        counter++;
                    }

                    try
                    {
                        frmCapture.device.SendPacket(packet); // sends the packet
                    }
                    catch (Exception exp)
                    {
                        
                    }

                }
            } // End double for loop implementation
            */

            while (!subnetFinished)
            {
                if (ipAddrs[2] == "208")
                {
                    hex1 = "D0";
                    ipAddrs[3] = Convert.ToString(hostCounter);
                    int x = Convert.ToInt32(ipAddrs[3]);
                    string bin = Convert.ToString(x, 2);
                    hex2 = Convert.ToInt32(bin, 2).ToString("X");
                    if (hex2.Length == 1)
                    {
                        hex2 = hex2.PadLeft(2, '0');
                    }

                    sBytes[24] = "00";
                    sBytes[25] = "00";
                    sBytes[28] = currentIP[0];
                    sBytes[29] = currentIP[1];
                    sBytes[32] = hex1;
                    sBytes[33] = hex2;

                    for (int c = 14; c < 34; c += 2)
                    {
                        ipHeaderSegs[c - 14 - count] = sBytes[c] + sBytes[c + 1];
                        count++;
                    }

                    count = 0;

                    string ipHeaderFull = string.Join(" ", ipHeaderSegs);
                    string checksum = calculateIPHeaderChecksum(ipHeaderFull);

                    sBytes[24] = checksum.Substring(0, 2);
                    sBytes[25] = checksum.Substring(2);

                    //Change the strings into bytes
                    byte[] packet = new byte[sBytes.Length];
                    int i = 0;
                    foreach (string s in sBytes)
                    {
                        packet[i] = Convert.ToByte(s, 16);
                        i++;
                    }

                    try
                    {
                        frmCapture.device.SendPacket(packet); // sends the packet
                    }
                    catch (Exception exp)
                    {

                    }
                    hostCounter++;
                    if (hostCounter == 255)
                    {
                        hostCounter = 0;
                        ipAddrs[2] = "209";
                    }
                }
                else if (ipAddrs[2] == "209")
                {
                    hex1 = "D1";
                    ipAddrs[3] = Convert.ToString(hostCounter);

                    int x = Convert.ToInt32(ipAddrs[3]);
                    string bin = Convert.ToString(x, 2);
                    hex2 = Convert.ToInt32(bin, 2).ToString("X");
                    if (hex2.Length == 1)
                    {
                        hex2 = hex2.PadLeft(2, '0');
                    }

                    sBytes[24] = "00";
                    sBytes[25] = "00";
                    sBytes[28] = currentIP[0];
                    sBytes[29] = currentIP[1];
                    sBytes[32] = hex1;
                    sBytes[33] = hex2;

                    for (int c = 14; c < 34; c += 2)
                    {
                        ipHeaderSegs[c - 14 - count] = sBytes[c] + sBytes[c + 1];
                        count++;
                    }

                    count = 0;

                    string ipHeaderFull = string.Join(" ", ipHeaderSegs);
                    string checksum = calculateIPHeaderChecksum(ipHeaderFull);

                    sBytes[24] = checksum.Substring(0, 2);
                    sBytes[25] = checksum.Substring(2);

                    //Change the strings into bytes
                    byte[] packet = new byte[sBytes.Length];
                    int i = 0;
                    foreach (string s in sBytes)
                    {
                        packet[i] = Convert.ToByte(s, 16);
                        i++;
                    }

                    try
                    {
                        frmCapture.device.SendPacket(packet); // sends the packet
                    }
                    catch (Exception exp)
                    {

                    }
                    hostCounter++;
                    if (hostCounter == 255)
                    {
                        hostCounter = 0;
                        ipAddrs[2] = "210";
                    }
                }
                else if (ipAddrs[2] == "210")
                {
                    hex1 = "D2";
                    ipAddrs[3] = Convert.ToString(hostCounter);
                    int x = Convert.ToInt32(ipAddrs[3]);
                    string bin = Convert.ToString(x, 2);
                    hex2 = Convert.ToInt32(bin, 2).ToString("X");
                    if (hex2.Length == 1)
                    {
                        hex2 = hex2.PadLeft(2, '0');
                    }

                    sBytes[24] = "00";
                    sBytes[25] = "00";
                    sBytes[28] = currentIP[0];
                    sBytes[29] = currentIP[1];
                    sBytes[32] = hex1;
                    sBytes[33] = hex2;

                    for (int c = 14; c < 34; c += 2)
                    {
                        ipHeaderSegs[c - 14 - count] = sBytes[c] + sBytes[c + 1];
                        count++;
                    }

                    count = 0;

                    string ipHeaderFull = string.Join(" ", ipHeaderSegs);
                    string checksum = calculateIPHeaderChecksum(ipHeaderFull);

                    sBytes[24] = checksum.Substring(0, 2);
                    sBytes[25] = checksum.Substring(2);

                    //Change the strings into bytes
                    byte[] packet = new byte[sBytes.Length];
                    int i = 0;
                    foreach (string s in sBytes)
                    {
                        packet[i] = Convert.ToByte(s, 16);
                        i++;
                    }

                    try
                    {
                        frmCapture.device.SendPacket(packet); // sends the packet
                    }
                    catch (Exception exp)
                    {

                    }
                    hostCounter++;
                    if (hostCounter == 255)
                    {
                        hostCounter = 0;
                        ipAddrs[2] = "211";
                    }
                }
                else if (ipAddrs[2] == "211")
                {
                    hex1 = "D3";
                    ipAddrs[3] = Convert.ToString(hostCounter);
                    int x = Convert.ToInt32(ipAddrs[3]);
                    string bin = Convert.ToString(x, 2);
                    hex2 = Convert.ToInt32(bin, 2).ToString("X");
                    if (hex2.Length == 1)
                    {
                        hex2 = hex2.PadLeft(2, '0');
                    }

                    sBytes[24] = "00";
                    sBytes[25] = "00";
                    sBytes[28] = currentIP[0];
                    sBytes[29] = currentIP[1];
                    sBytes[32] = hex1;
                    sBytes[33] = hex2;

                    for (int c = 14; c < 34; c += 2)
                    {
                        ipHeaderSegs[c - 14 - count] = sBytes[c] + sBytes[c + 1];
                        count++;
                    }

                    count = 0;

                    string ipHeaderFull = string.Join(" ", ipHeaderSegs);
                    string checksum = calculateIPHeaderChecksum(ipHeaderFull);

                    sBytes[24] = checksum.Substring(0, 2);
                    sBytes[25] = checksum.Substring(2);

                    //Change the strings into bytes
                    byte[] packet = new byte[sBytes.Length];
                    int i = 0;
                    foreach (string s in sBytes)
                    {
                        packet[i] = Convert.ToByte(s, 16);
                        i++;
                    }

                    try
                    {
                        frmCapture.device.SendPacket(packet); // sends the packet
                    }
                    catch (Exception exp)
                    {

                    }
                    hostCounter++;
                    if (hostCounter == 255)
                    {
                        hostCounter = 0;
                        ipAddrs[2] = "212";
                    }
                }
                else if (ipAddrs[2] == "212")
                {
                    hex1 = "D4";
                    ipAddrs[3] = Convert.ToString(hostCounter);
                    int x = Convert.ToInt32(ipAddrs[3]);
                    string bin = Convert.ToString(x, 2);
                    hex2 = Convert.ToInt32(bin, 2).ToString("X");
                    if (hex2.Length == 1)
                    {
                        hex2 = hex2.PadLeft(2, '0');
                    }

                    sBytes[24] = "00";
                    sBytes[25] = "00";
                    sBytes[28] = currentIP[0];
                    sBytes[29] = currentIP[1];
                    sBytes[32] = hex1;
                    sBytes[33] = hex2;

                    for (int c = 14; c < 34; c += 2)
                    {
                        ipHeaderSegs[c - 14 - count] = sBytes[c] + sBytes[c + 1];
                        count++;
                    }

                    count = 0;

                    string ipHeaderFull = string.Join(" ", ipHeaderSegs);
                    string checksum = calculateIPHeaderChecksum(ipHeaderFull);

                    sBytes[24] = checksum.Substring(0, 2);
                    sBytes[25] = checksum.Substring(2);

                    //Change the strings into bytes
                    byte[] packet = new byte[sBytes.Length];
                    int i = 0;
                    foreach (string s in sBytes)
                    {
                        packet[i] = Convert.ToByte(s, 16);
                        i++;
                    }

                    try
                    {
                        frmCapture.device.SendPacket(packet); // sends the packet
                    }
                    catch (Exception exp)
                    {

                    }
                    hostCounter++;
                    if (hostCounter == 255)
                    {
                        hostCounter = 0;
                        ipAddrs[2] = "213";
                    }
                }
                else if (ipAddrs[2] == "213")
                {
                    hex1 = "D5";
                    ipAddrs[3] = Convert.ToString(hostCounter);
                    int x = Convert.ToInt32(ipAddrs[3]);
                    string bin = Convert.ToString(x, 2);
                    hex2 = Convert.ToInt32(bin, 2).ToString("X");
                    if (hex2.Length == 1)
                    {
                        hex2 = hex2.PadLeft(2, '0');
                    }

                    sBytes[24] = "00";
                    sBytes[25] = "00";
                    sBytes[28] = currentIP[0];
                    sBytes[29] = currentIP[1];
                    sBytes[32] = hex1;
                    sBytes[33] = hex2;

                    for (int c = 14; c < 34; c += 2)
                    {
                        ipHeaderSegs[c - 14 - count] = sBytes[c] + sBytes[c + 1];
                        count++;
                    }

                    count = 0;

                    string ipHeaderFull = string.Join(" ", ipHeaderSegs);
                    string checksum = calculateIPHeaderChecksum(ipHeaderFull);

                    sBytes[24] = checksum.Substring(0, 2);
                    sBytes[25] = checksum.Substring(2);

                    //Change the strings into bytes
                    byte[] packet = new byte[sBytes.Length];
                    int i = 0;
                    foreach (string s in sBytes)
                    {
                        packet[i] = Convert.ToByte(s, 16);
                        i++;
                    }

                    try
                    {
                        frmCapture.device.SendPacket(packet); // sends the packet
                    }
                    catch (Exception exp)
                    {

                    }
                    hostCounter++;
                    if (hostCounter == 255)
                    {
                        hostCounter = 0;
                        ipAddrs[2] = "214";
                    }
                }
                else if (ipAddrs[2] == "214")
                {
                    hex1 = "D6";
                    ipAddrs[3] = Convert.ToString(hostCounter);
                    int x = Convert.ToInt32(ipAddrs[3]);
                    string bin = Convert.ToString(x, 2);
                    hex2 = Convert.ToInt32(bin, 2).ToString("X");
                    if (hex2.Length == 1)
                    {
                        hex2 = hex2.PadLeft(2, '0');
                    }

                    sBytes[24] = "00";
                    sBytes[25] = "00";
                    sBytes[28] = currentIP[0];
                    sBytes[29] = currentIP[1];
                    sBytes[32] = hex1;
                    sBytes[33] = hex2;

                    for (int c = 14; c < 34; c += 2)
                    {
                        ipHeaderSegs[c - 14 - count] = sBytes[c] + sBytes[c + 1];
                        count++;
                    }

                    count = 0;

                    string ipHeaderFull = string.Join(" ", ipHeaderSegs);
                    string checksum = calculateIPHeaderChecksum(ipHeaderFull);

                    sBytes[24] = checksum.Substring(0, 2);
                    sBytes[25] = checksum.Substring(2);

                    //Change the strings into bytes
                    byte[] packet = new byte[sBytes.Length];
                    int i = 0;
                    foreach (string s in sBytes)
                    {
                        packet[i] = Convert.ToByte(s, 16);
                        i++;
                    }

                    try
                    {
                        frmCapture.device.SendPacket(packet); // sends the packet
                    }
                    catch (Exception exp)
                    {

                    }
                    hostCounter++;
                    if (hostCounter == 255)
                    {
                        hostCounter = 0;
                        ipAddrs[2] = "215";
                    }
                }
                else if (ipAddrs[2] == "215")
                {
                    hex1 = "D7";
                    ipAddrs[3] = Convert.ToString(hostCounter);
                    int x = Convert.ToInt32(ipAddrs[3]);
                    string bin = Convert.ToString(x, 2);
                    hex2 = Convert.ToInt32(bin, 2).ToString("X");
                    if (hex2.Length == 1)
                    {
                        hex2 = hex2.PadLeft(2, '0');
                    }

                    sBytes[24] = "00";
                    sBytes[25] = "00";
                    sBytes[28] = currentIP[0];
                    sBytes[29] = currentIP[1];
                    sBytes[32] = hex1;
                    sBytes[33] = hex2;

                    for (int c = 14; c < 34; c += 2)
                    {
                        ipHeaderSegs[c - 14 - count] = sBytes[c] + sBytes[c + 1];
                        count++;
                    }

                    count = 0;

                    string ipHeaderFull = string.Join(" ", ipHeaderSegs);
                    string checksum = calculateIPHeaderChecksum(ipHeaderFull);


                    sBytes[24] = checksum.Substring(0, 2);
                    sBytes[25] = checksum.Substring(2);

                    //Change the strings into bytes
                    byte[] packet = new byte[sBytes.Length];
                    int i = 0;
                    foreach (string s in sBytes)
                    {
                        packet[i] = Convert.ToByte(s, 16);
                        i++;
                    }

                    try
                    {
                        frmCapture.device.SendPacket(packet); // sends the packet
                    }
                    catch (Exception exp)
                    {

                    }
                    hostCounter++;
                    if (hostCounter == 255)
                    {
                        hostCounter = 0;
                        subnetFinished = true;
                    }
                }
            }
        }

        public int resolveUsers()
        {
            int userCount = 0;
            dbConnect_IT = form.getDatabaseConnection();
            form.dbProgress.Value = 0;
            //form.progressLabel.Visible = true;
            //form.progressLabel.Text = "Resolving users for " + form.roomSelection.Text;
            form.machineList.Invoke(new Action(() => form.machineList.Items.Clear()));
            for (int i = 0; i < ITrooms.Length; i++)
            {

                if (form.roomSelection.Text == ITrooms[i])
                {
                    //form.progressLabel.Text = "Resolving users for " + ITrooms[i];
                    string readData = "select host from machine_info where host like '%" + ITrooms[i] + "X0%' order by host asc";
                    SqlCommand command = new SqlCommand(readData, dbConnect_IT);
                    SqlDataReader dr = command.ExecuteReader();
                    ListViewItem row = null;
                    while (dr.Read())
                    {
                        string str = findCurrentUser(dr["host"].ToString());

                        string user = parseUser(str, ITrooms[i], form); // pass str and current room to parseUser 
                        if (user == "None")
                        {
                            userCount++;
                            row = new ListViewItem();
                            row.ForeColor = Color.Green;
                            row.Text = dr["host"].ToString();
                            form.machineList.Invoke(new Action(() => form.machineList.Items.Add(row).SubItems.Add(user)));
                            form.dbProgress.Value++;
                        }
                        else
                        {
                            userCount++;
                            row = new ListViewItem();
                            row.ForeColor = Color.Red;
                            row.Text = dr["host"].ToString();
                            form.machineList.Invoke(new Action(() => form.machineList.Items.Add(row).SubItems.Add(user)));
                            form.dbProgress.Value++;
                        }
                    }
                }
            }
            //form.progressLabel.Text = "Done resolving users";
            return userCount;
        }

        private static string[] getSubnetHostHex()
        {
            // [0] will hold subnetHex
            // [1] will hold hostHex
            string[] hexVals = new string[2];

            string machineHostName = Dns.GetHostName();
            string IP = Dns.GetHostByName(machineHostName).AddressList[2].ToString();
            string[] ipSegs = IP.Split('.');

            hexVals[0] = Convert.ToString(Int32.Parse(ipSegs[2]), 16);
            hexVals[1] = Convert.ToString(Int32.Parse(ipSegs[3]), 16);
            if (hexVals[1].Length != 2)
            {
                hexVals[1] = hexVals[1].PadLeft(2, '0');
            }

            return hexVals;
        }

        private static string calculateIPHeaderChecksum(string ip_header)
        {
            string[] hexBytes = ip_header.Split(' ');
            string[] hexBin = new string[hexBytes.Length];

            for (int i = 0; i < hexBin.Length; i++)
            {
                hexBin[i] = Convert.ToString(Convert.ToInt32(hexBytes[i], 16), 2).PadLeft(16, '0');
            }

            int sum = 0;
            for (int i = 0; i < hexBin.Length; i++)
            {
                sum += Convert.ToInt32(hexBin[i], 2);
            }

            string sumBinStr = Convert.ToString(sum, 2);
            string strHex = Convert.ToInt32(sumBinStr, 2).ToString("X");
            int decVal = Convert.ToInt32(strHex.Substring(1), 16);
            int firstVal = hexToInt(strHex[0]);
            int finalVal = decVal + firstVal;
            string checksumNoFlip = Convert.ToString(finalVal, 16);

            string y = Convert.ToString(Convert.ToInt32(checksumNoFlip, 16), 2).PadLeft(16, '0');
            string y_flipped = flipBits(y);
            string checkSumFlip = Convert.ToInt32(y_flipped, 2).ToString("X");

            if (checkSumFlip.Length != 4)
            {
                checkSumFlip = checkSumFlip.PadLeft(4, '0');
            }

            hexBytes[5] = checkSumFlip;
            string finalIPHeader = string.Join(" ", hexBytes);

            return checkSumFlip;
        }

        private static int hexToInt(char c)
        {
            int v = Convert.ToInt32(c);
            string hexOutput = String.Format("{0:X}", v);
            int fix = Convert.ToInt32(hexOutput);
            if (fix >= 30 && fix < 40)
            {
                fix -= 30;
            }
            else if (fix >= 40 && fix < 47)
            {
                fix -= 31;
            }
            return fix;
        }

        private static string flipBits(string bin)
        {
            char[] c = bin.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == '0')
                {
                    c[i] = '1';
                }
                else
                {
                    c[i] = '0';
                }
            }

            return new string(c);
        }

        private static string findCurrentUser(string ip)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            //startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = "CMD.exe";
            startInfo.Arguments = "/C wmic.exe /node:" + ip + " ComputerSystem Get UserName";
            process.StartInfo = startInfo;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            Debug.Write(output.Length);
            //process.WaitForExit();
            return output;
        }

        private static string parseUser(string user, string roomNum, frmCapture form)
        {
            // Get number of entries with specified roomNum
            string sql = "select count(*) from machine_info where host like '%" + roomNum + "X0%'";
            SqlCommand command = new SqlCommand(sql, dbConnect_IT);
            //int machineCount = Convert.ToInt32(command.ExecuteScalar());

            // Find index of 'A'... NOTE: searching for AD/[username]
            int index = user.IndexOf("A", StringComparison.CurrentCulture);
            form.dbProgress.Maximum = 100;
            char[] c = user.ToCharArray();


            // Occasional Bug in this if-else... maybe change condition?

            if (c.Length > 29) // Length of 29 means nobody is signed on to that machine
            {
                string currentUser = "";
                for (int i = index; i < c.Length; i++)
                {
                    currentUser += c[i];
                }
                Debug.Write(currentUser);
                //form.dbProgress.Value++;
                return currentUser;
            }
            else
            {
                Debug.Write("None");
                //form.dbProgress.Value++;
                return "None";
            }
        }
    }
}
