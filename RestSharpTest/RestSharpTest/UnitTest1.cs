using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharpTest;

namespace AddressBookRestSharp
{
    [TestClass]
    public class RestSharpTestCase
    {
        RestClient client;

        [TestInitialize]
        public void Setup()
        {
            client = new RestClient("http://localhost:3000");
        }
        private RestResponse getEmployeeList()
        {
            RestRequest request = new RestRequest("/Persons", Method.Get);

            //act

            RestResponse response = (RestResponse)client.Execute(request);
            return response;
        }

        [TestMethod]
        public void onCallingGETApi_ReturnEmployeeList()
        {
            RestResponse response = getEmployeeList();

            //assert
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            List<PersonModel> dataResponse = JsonConvert.DeserializeObject<List<PersonModel>>(response.Content);
            Assert.AreEqual(5, dataResponse.Count);
            foreach (var item in dataResponse)
            {
                System.Console.WriteLine("id: " + item.Id + " First Name: " + item.First_Name + " Last Name: " + item.Last_Name);
            }
        }


        [TestMethod]
        public void givenEmployee_OnPost_ShouldReturnAddedEmployee()
        {
            RestRequest request = new RestRequest("/Persons", Method.Post);
            JObject jObjectbody = new JObject();
            jObjectbody.Add("First_Name", "Pooja");
            jObjectbody.Add("Last_Name", "Sen");
            jObjectbody.Add("Address", "Rajendra nagar");
            jObjectbody.Add("City", "Indore");
            jObjectbody.Add("State", "MP");
            jObjectbody.Add("Zip", "110007");
            jObjectbody.Add("Email", "Abhay@gmail.com");
            jObjectbody.Add("Phone_Num", "8888888888");

            request.AddParameter("application/json", jObjectbody, ParameterType.RequestBody);

            //act
            RestResponse response = (RestResponse)client.Execute(request);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.Created);
            PersonModel dataResponse = JsonConvert.DeserializeObject<PersonModel>(response.Content);
            Assert.AreEqual("Abhay", dataResponse.First_Name);
            Assert.AreEqual("Pandey", dataResponse.Last_Name);
            Assert.AreEqual("Patel Nagar", dataResponse.Address);
            Assert.AreEqual("Ayodhya", dataResponse.City);
            Assert.AreEqual("UP", dataResponse.State);
            Assert.AreEqual(110007, dataResponse.Zip);
            Assert.AreEqual("Abhay@gmail.com", dataResponse.Email);
            Assert.AreEqual("8888888888", dataResponse.Phone_Num);

        }
    }
}