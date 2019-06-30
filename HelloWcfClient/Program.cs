using System;
using HelloWcfClient.HelloWcfServiceReference;

namespace HelloWcfClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // http://localhost:8080/HelloWcfService/mex
            SignServiceClient client = 
                new SignServiceClient("BasicHttpBinding_ISignService");

            SignModel model = new SignModel();
            model.Email = "z@z.com";
            model.Password = "xyz";

            var r = client.SignIn(model);

            Console.WriteLine(r);
        }
    }
}
