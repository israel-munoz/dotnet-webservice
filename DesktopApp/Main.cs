using Models;
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
using System.Xml.Linq;

namespace DesktopApp
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private XDocument GetXmlMessage(int id)
        {
            //<?xml version="1.0" encoding="utf-8"?>
            //<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
            //  <soap12:Body>
            //    <GetUser xmlns="http://tempuri.org/">
            //      <id>int</id>
            //    </GetUser>
            //  </soap12:Body>
            //</soap12:Envelope>
            XDeclaration declaration = new XDeclaration("1.0", "utf-8", null);
            XNamespace xsi = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance"),
                xsd = XNamespace.Get("http://www.w3.org/2001/XMLSchema"),
                soap12 = XNamespace.Get("http://www.w3.org/2003/05/soap-envelope"),
                app = XNamespace.Get("http://tempuri.org/");
            XDocument doc = new XDocument(declaration);
            XElement root = new XElement(soap12 + "Envelope",
                new XElement(soap12 + "Body",
                    new XElement(app + "GetUser",
                        new XElement(app + "id", id))));
            doc.Add(root);

            return doc;
        }

        private User GetXmlResult(XDocument doc)
        {
            //<?xml version="1.0" encoding="utf-8"?>
            //<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
            //  <soap12:Body>
            //    <GetUserResponse xmlns="http://tempuri.org/">
            //      <GetUserResult>
            //        <Id>int</Id>
            //        <Name>string</Name>
            //      </GetUserResult>
            //    </GetUserResponse>
            //  </soap12:Body>
            //</soap12:Envelope>
            XNamespace xsi = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance"),
                xsd = XNamespace.Get("http://www.w3.org/2001/XMLSchema"),
                soap12 = XNamespace.Get("http://www.w3.org/2003/05/soap-envelope"),
                app = XNamespace.Get("http://tempuri.org/");
            XElement body = doc.Root.Element(soap12 + "Body"),
                response = body.Element(app + "GetUserResponse"),
                result = response.Element(app + "GetUserResult"),
                id,
                name;
            if (result == null)
            {
                return null;
            }
            id = result.Element(app + "Id");
            name = result.Element(app + "Name");
            return new User
            {
                Id = int.Parse(id.Value),
                Name = name.Value
            };
        }

        private void btnWebRequest_Click(object sender, EventArgs e)
        {
            try
            {
                string idString = txtUserId.Text,
                    serviceUrl = "http://localhost:52681/UsersService.asmx";

                int id = int.Parse(idString);

                var request = new WebClient();
                request.Headers.Add("Content-Type", "application/soap+xml; charset=utf-8");
                XDocument xmlMessage = GetXmlMessage(id);
                byte[] data = Encoding.ASCII.GetBytes(xmlMessage.ToString());
                byte[] response = request.UploadData(serviceUrl, "POST", data);

                string resultText = Encoding.ASCII.GetString(response);
                var result = GetXmlResult(XDocument.Parse(resultText));
                if (result == null)
                {
                    throw new Exception("User not found.");
                }
                userName.Text = result.Name;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnServiceReference_Click(object sender, EventArgs e)
        {
            try
            {
                string idString = txtUserId.Text;
                int id = int.Parse(idString);
                User user;
                using (var service = new UsersService.UsersServiceSoapClient())
                {
                    // Service References add it's own classes to match the ones in the Web Service
                    UsersService.User serviceUser = service.GetUser(id);
                    if (serviceUser == null)
                    {
                        throw new Exception("User not found.");
                    }
                    user = new User
                    {
                        Id = serviceUser.Id,
                        Name = serviceUser.Name
                    };
                }
                userName.Text = user.Name;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
