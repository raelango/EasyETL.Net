using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEndpoint;

namespace EasyEndPointSample
{
    class Program
    {
        static void Main(string[] args)
        {
            //https://garden.logixhealth.com/dept/SoftwareandTechnology/TechnologyManagement/Pages/default.aspx
            //https://garden.logixhealth.com/dept/SoftwareandTechnology/TechnologyManagement/IPPR/Forms/Current%20Year.aspx
            //IEasyEndpoint source = new SharepointEasyEndpoint(@"https://garden.logixhealth.com/dept/SoftwareandTechnology/TechnologyManagement/","","", "IPPR");
            //string[] files = source.GetList("*.*");
            //IEasyEndpoint destination = new FileEasyEndpoint(@"C:\Users\aelango\LogixHealth\Technology\SaaS\Copied");

            //IEasyEndpoint source = new FileEasyEndpoint(@"C:\Users\aelango\LogixHealth\Technology\SaaS\");
            //IEasyEndpoint destination = new SharepointEasyEndpoint(@"https://garden.logixhealth.com/dept/SoftwareandTechnology/TechnologyManagement/", "", "", "IPPR");

            IEasyEndpoint source = new FileEasyEndpoint(@"C:\Users\aelango\LogixHealth\Technology\SaaS\Copied");
            IEasyEndpoint destination = new FtpEasyEndpoint(@"ftp://ftp.dlptest.com/", "dlpuser@dlptest.com", "fLDScD4Ynth0p4OJ6bW6qCxjh") { CurrentDirectory = "/", Overwrite = true };

            source.CopyTo(destination, source.GetList("*.*"));
        }
    }
}
