using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Stock
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var url = "https://www.alahlitadawul.com/GTrade/trading?-44adc335%3A1703db68122%3A4bb9";
            string postData = "SourceOnly=true";
            string responseFromServer = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            byte[] bytes;
            bytes = System.Text.Encoding.ASCII.GetBytes(postData);
            request.ContentType = "text/xml; encoding='utf-8'";
            request.ContentLength = bytes.Length;
            request.Method = "POST";
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            HttpWebResponse response;
            response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream responseStream = response.GetResponseStream();
                responseFromServer = new StreamReader(responseStream).ReadToEnd();
            }
            XmlDocument xmlDoc = new XmlDocument();
            var xDoc = XDocument.Parse(responseFromServer);
            var pkmode = xDoc.Descendants("pk_mod").Single().Value;
            // Close the response.  
            response.Close();

            var result = getotp(pkmode,textBox1.Text,textBox2.Text);
            textBox3.Text = result;
        }



        public string getotp(string key,string uname,string pass)
        {
            string responseFromServer = string.Empty;

            // Create a request using a URL that can receive a post.   
            WebRequest request = WebRequest.Create("https://www.alahlitadawul.com/GTrade/trading");
            // Set the Method property of the request to POST.  
            request.Method = "POST";

            // Create POST data and convert it to a byte array.  
            string postData = "NameXsl=FirstStepLogin&XSLCONTEXT=HARIARAB&SourceOnly=true&Bank=NCBC&UserID="+uname+"&Password=" + key + "&DEVICETYPE=PC&_history_=exclude&nocache=1582446459958";
            //string postData = "SourceOnly=true";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            // Set the ContentType property of the WebRequest.  
            request.ContentType = "text/xml; encoding='utf-8'";//"application/x-www-form-urlencoded";
            // Set the ContentLength property of the WebRequest.  
            request.ContentLength = byteArray.Length;

            // Get the request stream.  
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.  
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.  
            dataStream.Close();

            // Get the response.  
            WebResponse response = request.GetResponse();
            // Display the status.  
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            // Get the stream containing content returned by the server.  
            // The using block ensures the stream is automatically closed.
            using (dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.  
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.  
                responseFromServer = reader.ReadToEnd();
                // Display the content.  
                Console.WriteLine(responseFromServer);
            }
            //XmlDocument xmlDoc = new XmlDocument();
            //var xDoc = XDocument.Parse(responseFromServer);
            //var pkmode = xDoc.Descendants("pk_mod").Single().Value;
            // Close the response.  
            response.Close();
            return responseFromServer;
        }
    }
}
