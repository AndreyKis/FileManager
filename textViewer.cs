using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO; //for the work with files

namespace MyManager
{
    public partial class textViewer : Form
    {
        string path = "";

        public textViewer()
        {
            InitializeComponent();
        }

        public textViewer(string s)
        {//uploading with the upload oof the text file path
            InitializeComponent();
            LoadFile(s);
        }

        private void LoadFile(string newPath)
        {
            text.Clear(); path = "";
            if (!File.Exists(newPath))
            {
                MessageBox.Show("No such file");
                return;
            }

            string s = newPath.Substring(newPath.LastIndexOf('.') + 1);
            if (s != "txt" && s != "html")
            {
                newPath += " neither txt, nor html";
                MessageBox.Show(newPath, "ERROR");
                return;
            }
            else
            {
                path = newPath;
                StreamReader streamReader = new StreamReader(newPath); //Open the file for reading
                string str = ""; //Initializing the parametr, in which the text will be written from the file

                str = streamReader.ReadLine();
                while (!streamReader.EndOfStream) //The cycle lasts until the end of the file
                {
                    str += "\n"+streamReader.ReadLine(); //Writing the info from file into str
                }

                text.Text = str;
                streamReader.Close();
            }
        }

        private void SaveFile()
        {
            StreamWriter write_text= new StreamWriter(path,false,Encoding.GetEncoding(1251));  //Class for the rewriting into the file
            
            write_text.WriteLine(text.Text); //Writing the text to the file from the text field
            write_text.Close(); // Closing the file
        }

        private void textViewer_Load(object sender, EventArgs e)
        {
            text.Left = 0;
            text.Top = menu.Size.Height;
            text.Size = new Size(this.Size.Width,this.Size.Width-menu.Size.Height);
        }

        
        private void textViewer_SizeChanged(object sender, EventArgs e)
        {
            text.Size = new Size(this.Size.Width, this.Size.Width - menu.Size.Height);
            //text.Size.Height = 12;

        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text.Clear();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void openToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //MessageBox.Show(openFileDialog1.FileName);
                LoadFile(openFileDialog1.FileName);
            }
        }

        private void findLonelyWordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string pathT= path+"~~.txt";
            StreamWriter write_text = new StreamWriter(pathT, false, Encoding.GetEncoding(1251));  //Class for the rewriting into the file

            Dictionary<string,int> map = new Dictionary<string,int>();
            string str= text.Text;
            int i=0; int l= str.Length;

            while (i<l)
            {// Takes the words from the text in the cycle and writing the amount of them into the map
                while (i<l && (str[i]==' ' || str[i]=='\n')) i++;
                if (i<l)
                {
                string s = "";
                int i1 = 0;
                while (i+i1<l && (str[i+i1]!=' ' && str[i+i1]!='\n')) i1++;
                s= str.Substring(i,i1);
                //MessageBox.Show(s);
                if (!map.ContainsKey(s)) map.Add(s, 1);
                else map[s]= 2;
                i+= i1+1;
                }
            }
            
            foreach (KeyValuePair<string,int> s in map)
            {
             if (s.Value==1) write_text.WriteLine(s.Key);
            }
            write_text.Close(); // Closing the file


            var tV2 = new textViewer(pathT);
            tV2.ShowDialog();
            tV2.Dispose();
            File.Delete(pathT);
        }

        private void SimilarWordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileStream fIn = new FileStream(@"D:\Прога_Лаба№1\test_1st.txt", FileMode.Open, FileAccess.Read, FileShare.Read);

            int num = 0;
            string str = "";
            string result = "";
            for (int i = 0; i < text.Text.Length; i++)
            {
                if (text.Text[i] == ' ' || text.Text[i] == '\n')
                    str = "";
                else
                    str += text.Text[i];
            }

            using (StreamReader sr = new StreamReader(fIn))
            {
                string input;
                // While not at the end of the file, read lines from the file. 
                while (sr.Peek() > -1)
                {
                    input = sr.ReadLine();
                    if (input.Length >= str.Length)
                    {
                        string begin = input.Substring(0, str.Length);
                        if (begin == str)
                        {
                            result += input + "\n";
                            num++;
                        }
                    }
                }
            }
            if (num == 0) result = "No such words";
            MessageBox.Show(result, "Similar words in the test_1st file:");
        }

        private void uniteHTMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string file1 = text.Text;
            string file2 = "";
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "html files (*.html)|*.html";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    LoadFile(openFileDialog1.FileName);
                    file2 = text.Text;
                    file1 = file1.Substring(0, file1.IndexOf("</BODY>") - 1);
                    file2 = file2.Substring(file2.IndexOf("<BODY>") + "<BODY>".Length, 
                        file2.Length - file2.IndexOf("<BODY>") - "<BODY>".Length);
                    text.Text = file1 + file2;
                }
        }
    }
}
