using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
namespace MyManager
{
    public partial class Form1 : Form
    {
        private string path1 = @"C:\";
        private string path2 = @"C:\";
        
            List<string> copyDirectories = new List<string>();
            List<string> copyFiles = new List<string>();
            bool copy; //1 - copy, 0 - cut

            private void saveConfigs()
            {
                XDocument xdoc = new XDocument();
                XElement configs = new XElement("configs");
                xdoc.Add(configs);
                XElement xpath1 = new XElement("path1", path1);
                configs.Add(xpath1);
                XElement xpath2 = new XElement("path2", path2);
                configs.Add(xpath2);
                xdoc.Save("./configs.xml");

            }

            private void getConfigs()
            {
                if (File.Exists("./configs.xml"))
                {
                    XDocument xdoc = XDocument.Load("./configs.xml");
                    path1 = xdoc.Element("configs").Element("path1").Value;
                    if (!Directory.Exists(path1))
                    {
                        path1 = @"C:\";
                    }
                    path2 = xdoc.Element("configs").Element("path2").Value;
                    if (!Directory.Exists(path2))
                    {
                        path2 = @"C:\";
                    }
                }
                
            }
        
        public Form1()
            {
                InitializeComponent();
                getConfigs();
                Refresh(showList1, path1, address1);
                Refresh(showList2, path2, address2);
                

                string[] Drivers = Environment.GetLogicalDrives();
                int n = 0;
                foreach (string k in Drivers)
                {
                    try
                    {
                        TreeNode tn = new TreeNode();
                        tn.Name = k;
                        tn.Text = "Local disk " + k;
                        showTree1.Nodes.Add(tn.Name, tn.Text, 0);
                        string t = "";
                        string[] str2 = Directory.GetDirectories(k);
                        foreach (string s2 in str2)
                        {
                            t = s2.Substring(s2.LastIndexOf('\\') + 1);
                            ((TreeNode)showTree1.Nodes[n]).Nodes.Add(s2, t, 1);
                        }
                    }
                    catch { }
                    n++;
                }

                n = 0;
                foreach (string s in Drivers)
                {
                    try
                    {
                        TreeNode tn = new TreeNode();
                        tn.Name = s;
                        tn.Text = "Local disk " + s;
                        showTree2.Nodes.Add(tn.Name, tn.Text, 0);
                        string t = "";
                        string[] Drivers2 = Directory.GetDirectories(s);
                        foreach (string k2 in Drivers2)
                        {
                            t = k2.Substring(k2.LastIndexOf('\\') + 1);
                            ((TreeNode)showTree2.Nodes[n]).Nodes.Add(k2, t, 1);
                        }
                    }
                    catch { }
                    n++;
                }

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dispose();
        }
        
        private void ShowList1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (showList1.SelectedItems.Count > 0)
            {
                if (showList1.SelectedItems[0].ImageIndex == 0)     //Go to the next folder
                {
                    try { GoToFolder(showList1, ref path1, showList1.SelectedItems[0].Text, address1); }
                    catch { MessageBox.Show("access denied","Error"); }
                }
                else //open file
                {
                    string s = path1 +'\\'+ showList1.SelectedItems[0].Text;
                    try { OpenFile(showList1, s); }
                    catch { MessageBox.Show("access denied", "Error"); }
                }
            }
        }

        private void ShowList2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (showList2.SelectedItems.Count > 0)
            {
                if (showList2.SelectedItems[0].ImageIndex == 0)     //Go to the next folder
                {
                    try { GoToFolder(showList2, ref path2, showList2.SelectedItems[0].Text, address2); }
                    catch { MessageBox.Show("access denied", "Error"); }

                }
                else //open file
                {
                    string s = path2 + '\\' + showList2.SelectedItems[0].Text;
                    try { OpenFile(showList2, s); }
                    catch { MessageBox.Show("access denied", "Error"); }
                }
            }
        }

        private void showTree1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            path1 = e.Node.Name;
            Refresh(showList1, path1, address1);
        }

        private void showTree1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            showTree1.SelectedImageIndex = e.Node.ImageIndex;

        }

        private void showTree1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            //MessageBox.Show(e.Node.Name);
            TreeNodeCollection daughter = e.Node.Nodes;
            foreach (TreeNode tn in daughter)
            {
                //MessageBox.Show(tn.Name);
                tn.Nodes.Clear();
                try
                {
                    string s = tn.Name;

                    string[] str2 = Directory.GetDirectories(s);
                    string t;
                    foreach (string s2 in str2)
                    {
                        t = s2.Substring(s2.LastIndexOf('\\') + 1);
              //          MessageBox.Show(s2);
                        tn.Nodes.Add(s2, t, 1);
                    }
                }
                catch { }
            }

        }

        private void showTree2_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            path2 = e.Node.Name;
            Refresh(showList2, path2, address2);
        }

        private void showTree2_AfterSelect(object sender, TreeViewEventArgs e)
        {
            showTree2.SelectedImageIndex = e.Node.ImageIndex;

        }

        private void showTree2_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            //MessageBox.Show(e.Node.Name);
            TreeNodeCollection daughter = e.Node.Nodes;
            foreach (TreeNode tn in daughter)
            {
                //MessageBox.Show(tn.Name);
                tn.Nodes.Clear();
                try
                {
                    string s = tn.Name;

                    string[] str2 = Directory.GetDirectories(s);
                    string t;
                    foreach (string s2 in str2)
                    {
                        t = s2.Substring(s2.LastIndexOf('\\') + 1);
                        //          MessageBox.Show(s2);
                        tn.Nodes.Add(s2, t, 1);
                    }
                }
                catch { }
            }

        }
      
        private void BackButton1Click(object sender, EventArgs e)
        {
            Back(showList1, ref path1, address1);
        }

        private void Address1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string s;
                s = address1.Text;
                if (!ChangeDir(showList1, ref path1, s, address1))
                {
                    address1.Text = path1;
                    MessageBox.Show("Incorrect path");
                }
            }
        }

        private void NewDirButton1Click(object sender, EventArgs e)
        { //создание новой папки
            newFolder f2 = new newFolder();
            f2.Owner = this;
            f2.ShowDialog();
            if (f2.folderName != "") CreateDirectory(path1, f2.folderName);
            Refresh(showList1, path1, address1);
            f2.Dispose();
        }

        private void NewFileButton1Click(object sender, EventArgs e)
        {//создать новый файл
            newFolder f2 = new newFolder(true);
            f2.Owner = this;
            f2.ShowDialog();
            if (f2.folderName != "") CreateFile(path1, f2.folderName);
            Refresh(showList1, path1, address1);
            f2.Dispose();
        }

        private void RefreshButton1Click(object sender, EventArgs e)
        {
            Refresh(showList1, path1, address1);
        }

        private void BackButton2Click(object sender, EventArgs e)
        {
            Back(showList2, ref path2, address2);
        }

        private void Address2_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void NewDirButton2Click(object sender, EventArgs e)
        { //создание новой папки
            newFolder f2 = new newFolder();
            f2.Owner = this;
            f2.ShowDialog();
            if (f2.folderName != "") CreateDirectory(path2, f2.folderName);
            MessageBox.Show(path2);
            Refresh(showList2, path2, address2);
            f2.Dispose();
        }

        private void NewFileButton2Click(object sender, EventArgs e)
        {//создать новый файл
            newFolder f2 = new newFolder(true);
            f2.Owner = this;
            f2.ShowDialog();
            if (f2.folderName != "") CreateFile(path2, f2.folderName);
            Refresh(showList2, path2, address2);
            f2.Dispose();
        }

        private void RefreshButton2Click(object sender, EventArgs e)
        {
            Refresh(showList2, path2, address2);
        }

        private void PressOpenItem(object sender, EventArgs e)
        {
            if (showList1.SelectedItems.Count > 0) ShowList1_MouseDoubleClick(this, null);
            if (showList2.SelectedItems.Count > 0) ShowList2_MouseDoubleClick(this, null);
        }

        private void PressDeleteItem(object sender, EventArgs e)
        {
            if (showList1.SelectedItems.Count > 0) Delete(showList1, path1);
            if (showList2.SelectedItems.Count > 0) Delete(showList2, path2);

            Refresh(showList1, path1, address1);
            Refresh(showList2, path2, address2);
        }

        private void PressCopyItem(object sender, EventArgs e)
        {
            if (showList1.SelectedItems.Count > 0) AddToBuffer(showList1, path1, true);
            if (showList2.SelectedItems.Count > 0) AddToBuffer(showList2, path2, true);
        }

        private void PressCutItem(object sender, EventArgs e)
        {
            if (showList1.SelectedItems.Count > 0)
            {
                AddToBuffer(showList1, path1, false);
                foreach (int n in showList1.SelectedIndices)
                {
                    showList1.Items[n].ForeColor= Color.Gray;
                }


            }
            if (showList2.SelectedItems.Count > 0) AddToBuffer(showList2, path2, false);

        }

        private void PressPasteItem(object sender, EventArgs e)
        {
            if (MousePosition.X < PointToScreen(showList2.Location).X)
            {
                try { Paste(path1); }
                catch { MessageBox.Show("Error", "ERROR"); }
                Refresh(showList1, path1, address1);
            }
            else
            {
                try { Paste(path2); }
                catch { MessageBox.Show("Error", "ERROR"); }
                Refresh(showList2, path2, address2);
            }
            MessageBox.Show("Complete", "Info");
        }

        private void Refresh(ListView sender, string path, ToolStripTextBox address)
            {
                string[] files = Directory.GetFiles(path);
                string[] directories = Directory.GetDirectories(path);

                address.Text = path;

                sender.SmallImageList = imageList1;
                sender.Items.Clear();
                {
                    string dirName = "";
                    for (int i = 0; i < directories.Length; i++)
                    {
                        ListViewItem lvi = new ListViewItem();
                        dirName = directories[i].Substring(directories[i].LastIndexOf('\\') + 1);
                        lvi.Text = dirName;
                        lvi.ImageIndex = 0;
                        sender.Items.Add(lvi);                       
                    }

                    string fileName = "";
                    for (int i = 0; i < files.Length; i++)
                    {
                        ListViewItem lvi = new ListViewItem();
                        fileName = files[i].Substring(files[i].LastIndexOf('\\') + 1);
                        lvi.Text = fileName;
                        lvi.ImageIndex = 1;
                        sender.Items.Add(lvi);
                    }

                    sender.View = View.List;
                }

            }
        
        private bool ChangeDir(ListView sender, ref string path, string newPath, ToolStripTextBox address)
        {
            if (newPath.LastIndexOf('\\') == -1) return false;
            if (Directory.Exists(newPath))
            {
             path= newPath;
             Refresh(sender,path,address);
             return true;
            }
            return false;
        }

        private void Back(ListView sender, ref string path, ToolStripTextBox address)
        {
            while (path.LastIndexOf('\\') == path.Length - 1) path = path.Substring(0, path.LastIndexOf('\\'));
            if (path.LastIndexOf('\\') == -1) { path += '\\'; System.Console.Beep(3000, 300); return; }
            else
            {
                path = path.Substring(0, path.LastIndexOf('\\'))+'\\';
                Refresh(sender, path, address);
            }
        }

        private void GoToFolder(ListView sender, ref string path, string fName, ToolStripTextBox address)
        {
            if (path.LastIndexOf('\\') != path.Length - 1) path += '\\';
            path += fName;
            Refresh(sender, path, address);
        }

        private void OpenFile(ListView sender, string path)
        {// 
            string s = path.Substring(path.LastIndexOf(".") + 1);
            if (s != "txt" && s != "html") { MessageBox.Show("Problems with opening file"); }
            else
            {
             textViewer tV1= new textViewer(path);
             tV1.Show();
            }
        }

        private void CreateDirectory(string path, string fName)
        {
            if (!Directory.Exists(path + '\\' + fName))
                Directory.CreateDirectory(path + '\\' + fName);
            else
                MessageBox.Show("Folder created", "Error");
        }

        private void CreateFile(string path, string fileName)
        {
            if (!File.Exists(path + '\\' + fileName))
                File.Create(path + '\\' + fileName);
            else
                MessageBox.Show("File created", "Error");
        }

        private void Delete(ListView sender, string path)
        {
            if (sender.SelectedItems.Count > 0)
            {
                path += '\\';
                foreach (int n in sender.SelectedIndices)
                {
                    if (sender.Items[n].ImageIndex == 0) // folder
                        Directory.Delete(path + sender.Items[n].Text, true); //deletes with the thing consisted
                    else // file
                        File.Delete(path + showList1.Items[n].Text);
                }
            }

        }

        private void AddToBuffer(ListView sender, string path, bool flag)
        {
            if (sender.SelectedItems.Count > 0)
            {
                copyDirectories.Clear();
                copyFiles.Clear();
                copy = flag;
                path += '\\';
                foreach (int n in sender.SelectedIndices)
                {
                    if (sender.Items[n].ImageIndex == 0) // folder
                        copyDirectories.Add(path + sender.Items[n].Text);
                    else // file
                        copyFiles.Add(path + sender.Items[n].Text);
                }
            }
        }

        private void CopyFile(string from, string to, bool overwrite)
        {
            {
                int i;
                for (i = from.Length; from[i - 1] == '\\'; i--) ;
                from = from.Substring(0, i);
            }
            string fileName = from.Substring(from.LastIndexOf('\\'));
            File.Copy(from, to + '\\' + fileName, overwrite);
        }

        private void CopyFolder(string from, string to)
        {// copyFolder("D:\1\2","C:\") result "C:\2"
            {
                int i;
                for (i= from.Length; from[i-1]=='\\'; i--);
                from= from.Substring(0,i);
            }

            string fName= from.Substring(from.LastIndexOf('\\')+1);
            to+= '\\'+fName;
            if (!Directory.Exists(to)) Directory.CreateDirectory(to);

            string[] files = Directory.GetFiles(from);
            foreach (string s in files)
                CopyFile(s, to, true);

            string[] folders = Directory.GetDirectories(from);
            foreach (string s in folders)
                CopyFolder(s, to);
         }

        private void Paste(string path)
        {
            {
                string t = "";
                t += path[0];
                for (int i = 1; i < path.Length; i++)
                    if (path[i] != '\\' || path[i - 1] != '\\') t += path[i];
                path = t;
            }

            foreach (string s in copyDirectories)
            {// check for the copy into it`s own catalogue
                string t = "";
                t += s[0];
                for (int i = 1; i < s.Length; i++)
                {
                    if (s[i] != '\\' || s[i - 1] != '\\') t += s[i];
                }
                if (path.Contains(t)) { MessageBox.Show("Oh, very funny "+path+t, " Error"); return; }
                
            }

            foreach (string s in copyFiles) // copy with rewriting
                CopyFile(s, path, true);

            foreach (string s in copyDirectories)
                CopyFolder(s, path);

            if (!copy)
            {//if moving, delete in the base place
                foreach (string s in copyFiles)
                    File.Delete(s);

                foreach (string s in copyDirectories)
                    Directory.Delete(s, true); //delete with the subfolders
            }

            copyFiles.Clear(); copyDirectories.Clear(); copy = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            saveConfigs();
            Dispose();
        }

        private void keyWordsSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchHTML();
        }

        private void SearchHTML()
        {
            Form2 f = new Form2();
            f.Owner = this;
            f.ShowDialog();
            if (f.searchRes != "")
            {
                try
                {
                    string s = address1.Text;
                    string[] dirs = Directory.GetFiles(s, "*.html", SearchOption.AllDirectories);
                    List<string> need = new List<string>();

                    bool exxx = false;
                    foreach (string dir in dirs)
                    {
                        bool ex = false;
                        string[] readText = File.ReadAllLines(dir);
                        foreach (string str in readText)
                        {
                            if (str.Contains(f.searchRes))
                            {
                                ex = true; exxx = true;
                            }
                        }
                        if (ex)
                        {
                            //MessageBox.Show(dir); 
                            need.Add(dir);
                        }
                    }
                    if (!exxx) MessageBox.Show("Ничего не найдено");

                    //------
                    showList1.SmallImageList = imageList1;
                    showList1.Items.Clear();

                    string fileName = "";
                    for (int i = 0; i < need.Count; i++)
                    {
                        ListViewItem lvi = new ListViewItem();
                        fileName = need[i].Substring(address1.Text.Length + 1);

                        lvi.Text = fileName;
                        lvi.ImageIndex = 1;

                        showList1.Items.Add(lvi);
                    }

                    showList1.View = View.List;

                    //-----

                }

                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
            else MessageBox.Show("Параметры поиска не заданы", "Ошибка");
            f.Dispose();
        }
    }

}