using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ManageCenter.SvcManageRef;

namespace ManageCenter
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            SvcManageClient client = new SvcManageClient();
            List<DevInfo> devList = new List<DevInfo>(client.GetDeviceList());
            DeviceDgv.DataSource = devList;

        }
    }
}
