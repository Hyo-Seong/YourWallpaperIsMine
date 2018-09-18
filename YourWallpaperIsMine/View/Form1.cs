using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YourWallpaperIsMine.Core;
using static YourWallpaperIsMine.Wallpaper;

namespace YourWallpaperIsMine
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            HttpConnection httpConnection = new HttpConnection();
            httpConnection.GetImageURL();
            if (!httpConnection.DownloadImage())
            {
                return;
            }
            Wallpaper.Set(Path.GetTempPath() + "YourWallpaperIsMine\\temp.jpg", Style.Tiled);
        }
    
    
    }
}
