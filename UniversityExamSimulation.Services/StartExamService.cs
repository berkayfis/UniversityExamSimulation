using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using UniversityExamSimulation.Core.Models;
using UniversityExamSimulation.Core.Repositories;
using UniversityExamSimulation.Core.Services;

namespace UniversityExamSimulation.Services
{
    public class StartExamService : IStartExamService
    {
        private readonly IStudentRepository studentRepository;
        private readonly IUniversityRepository universityRepository;
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory httpClientFactory;

        public StartExamService(IStudentRepository studentRepository, IUniversityRepository universityRepository, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            this.studentRepository = studentRepository;
            this.universityRepository = universityRepository;
            this.configuration = configuration;
            this.httpClientFactory = httpClientFactory;
        }

        public List<Student> GetExamResult(DateTime examDate)
        {
            var url = GetClientUrl(examDate);
            var articles = FetchArticles(url);
            if (articles == null) return null;
            var students = studentRepository.GetStudents();
            CalculateStudentsPoint(students);
            return students;
        }

        private void CalculateStudentsPoint(List<Student> students)
        {
            students[0].Point = 50;
        }

        private List<Article> FetchArticles(string url)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            var httpClient = httpClientFactory.CreateClient();
            var httpResponseMessage = httpClient.Send(httpRequestMessage);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var responseMessage = httpResponseMessage.Content.ReadAsStringAsync().Result;
                var jsonObject = JsonConvert.DeserializeObject<WikipediaResponse>(responseMessage);
                if (jsonObject == null) return null;
                return jsonObject.Items[0].Articles.ToList();
            }
            return null;
        }

        private string GetClientUrl(DateTime examDate)
        {
            var fetchArticleUrl = configuration["AppSettings:FetchArticleUrl"];
            var dateTimeFormat = configuration["AppSettings:DateTimeFormat"];
            return string.Format(fetchArticleUrl, examDate.ToString(dateTimeFormat));
        }

    }
}
