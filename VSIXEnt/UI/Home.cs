using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Apis;
using Newtonsoft.Json;
using VSIXEnt.Models;

namespace VSIXEnt.UI
{
    public partial class Home : Form
    {
        List<Result> results;
        public Home(string exception)
        {
            InitializeComponent();
            string[] queries = exception.Split('\'');
            string searchQuery = "";
            if (queries.Length > 2)
            {
                searchQuery = queries[0];
                searchQuery += queries[2];
            }
            else if (queries.Length == 1)

            {
                searchQuery = queries[0];
            }
            this.searchtextbox.Text = searchQuery;
            results=  seacrhfromgoogle(searchQuery);
            int count = 1;
            foreach(var item in results)
            {
                listBox3.Items.Add("" + count);
                listBox1.Items.Add(item.Title);
                listBox2.Items.Add(item.Link);
                count++;
            }
            
            
        }
         List<Result> seacrhfromgoogle(string words)
        {

            string searchQuery=words; 
           
            string cx = "000107149968631652513:aqg-qkj0rxa";
            string apiKey = "AIzaSyBil3Lg6Q7RZLHBkAcHIOro5w72B8oCck0";
            var request = WebRequest.Create("https://www.googleapis.com/customsearch/v1?key=" + apiKey + "&cx=" + cx + "&q=" + searchQuery);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseString = reader.ReadToEnd();
            dynamic jsonData = JsonConvert.DeserializeObject(responseString);

            List<Result> results = new List<Result>();
            foreach (var item in jsonData.items)
            {
                results.Add(new Result
                {
                    Title = item.title,
                    Link = item.link,
                    Snippet = item.snippet,
                });
                if(results.Count==10)
                {
                    break;
                }
            }

            return results;
        }

        private void listBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.listBox2.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                Process myProcess = new Process();

                try
                {
                    // true is the default, but it is important not to set it to false
                    myProcess.StartInfo.UseShellExecute = true;
                    myProcess.StartInfo.FileName =results[index].Link;
                    myProcess.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string newSearch = this.searchtextbox.Text;
            results = seacrhfromgoogle(newSearch);
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            int count = 1;
            foreach (var item in results)
            {
                listBox3.Items.Add("" + count);
                listBox1.Items.Add(item.Title);
                listBox2.Items.Add(item.Link);
                count++;
            }
        }
    }
}
