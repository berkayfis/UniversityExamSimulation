using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UniversityExamSimulation.Core.Models;

namespace UniversityExamSimulation.Data
{
    public class DataSeeder
    {
        private readonly UniversityExamDbContext dbContext;
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory httpClientFactory;

        public DataSeeder(UniversityExamDbContext dbContext, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
            this.httpClientFactory = httpClientFactory;
        }

        public void Seed()
        {
            if (!dbContext.Students.Any())
            {
                var url = GetStudentApiUrl();
                var students = FetchStudents(url);
                dbContext.Students.AddRange(students);
                dbContext.SaveChanges();
            }

            if (!dbContext.Universities.Any())
            {
                var url = GetUniversityApiUrl();
                var universities = FetUniversities(url);
                dbContext.Universities.AddRange(universities);
                dbContext.SaveChanges();
            }
        }

        private List<Student> FetchStudents(string url)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            var httpClient = httpClientFactory.CreateClient();
            var httpResponseMessage = httpClient.Send(httpRequestMessage);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var responseMessage = httpResponseMessage.Content.ReadAsStringAsync().Result;
                var jsonObject = JsonConvert.DeserializeObject<RandomUserResponse>(responseMessage);
                if (jsonObject == null) return null;
                return MapRandomUsersToStudents(jsonObject.RandomUsers);
            }
            return null;
        }

        private List<University> FetUniversities(string url)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            var httpClient = httpClientFactory.CreateClient();
            var httpResponseMessage = httpClient.Send(httpRequestMessage);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var responseMessage = httpResponseMessage.Content.ReadAsStringAsync().Result;
                var jsonObject = JsonConvert.DeserializeObject<UniversitiesResponse[]>(responseMessage);
                if (jsonObject == null) return null;
                return MapUniversityResponseToUniversities(jsonObject);
            }
            return null;
        }

        private string GetStudentApiUrl()
        {
            return configuration["AppSettings:FetchStudentsUrl"];
        }

        private string GetUniversityApiUrl()
        {
            return configuration["AppSettings:FetchUniversitiesUrl"];
        }

        private List<Student> MapRandomUsersToStudents(RandomUser[] randomUsers)
        {
            List<Student> students = new List<Student>();
            foreach (var randomUser in randomUsers)
            {
                var student = new Student()
                {
                    Name = randomUser.Name.FirstName,
                    Surname = randomUser.Name.LastName
                };
                students.Add(student);
            }
            return students;
        }

        private List<University> MapUniversityResponseToUniversities(UniversitiesResponse[] universityResponse)
        {
            List<University> universities = new List<University>();
            foreach (var university in universityResponse)
            {
                var univ = new University() { Name = university.Name };
                universities.Add(univ);
            }            
            return universities;
        }
    }
}
