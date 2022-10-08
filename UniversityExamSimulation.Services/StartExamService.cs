using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Drawing;
using System.Linq;
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
            CalculateStudentsPoint(students, articles);
            students = students.OrderByDescending(s => s.Point).ToList();
            var universities = universityRepository.GetUniversities();
            AddStudentsToUnivertiy(students, universities);
            return students;
        }

        private void CalculateStudentsPoint(List<Student> students, List<Article> articles)
        {
            var distinctStudentFullNames = GetDistinctStudentFullNames(students);
            var distinctArticleNames = GetDistinctArticleNames(articles); 

            foreach (var (student, index) in distinctStudentFullNames.Select((value, i) => (value, i)))
            {
                var point = 0;

                foreach (var article in distinctArticleNames)
                {
                    //point += article.Count(student.Contains);
                    point += CalculateMatches(article, student);
                }
                students[index].Point = point;
            }
        }

        public void AddStudentsToUnivertiy(List<Student> students, List<University> universities)
        {
            var universityQuota = int.Parse(configuration["AppSettings:UniversityQuota"]);
            int currentQuota = 0;
            int currentUniversityId = 0;
            foreach (var student in students)
            {
                if (currentQuota < universityQuota)
                {
                    student.UniversityId = universities[currentUniversityId].Id;
                    student.University = universities[currentUniversityId];
                    currentQuota++;
                }
                else
                {
                    currentUniversityId++;
                    currentQuota = 0;
                }
            }
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
            var fetchArticleUrl = configuration["AppSettings:FetchArticlesUrl"];
            var dateTimeFormat = configuration["AppSettings:DateTimeFormat"];
            return string.Format(fetchArticleUrl, examDate.ToString(dateTimeFormat));
        }

        private List<string> GetDistinctStudentFullNames(List<Student> students)
        {
            List<string> distinctStudentFullNames = new List<string>();

            foreach (var student in students)
            {
                string stundentFullName = string.Concat(student.Name, student.Surname);
                var distinctStudentsChars = stundentFullName.ToLower().Distinct();
                distinctStudentFullNames.Add(new string(distinctStudentsChars.ToArray()));
            }
            return distinctStudentFullNames;
        }

        private List<string> GetDistinctArticleNames(List<Article> articles)
        {
            List<string> distinctArticleNames = new List<string>();

            foreach (var article in articles)
            {
                var distinctArticleChars = article.Name.ToLower().Distinct();
                distinctArticleNames.Add(new String(distinctArticleChars.ToArray()));
            }
            return distinctArticleNames;
        }

        private int CalculateMatches(string distinctArticleName, string distinctStudentName)
        {
            int matches = 0;
            for (int i = 0; i < distinctArticleName.Length; i++)
            {
                if (distinctStudentName.IndexOf(distinctArticleName[i]) >= 0)
                {
                    matches += 1;
                }
            }
            return matches;
        }

    }
}
