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
            //sort student
            //addst
            return students;
        }

        private void CalculateStudentsPoint(List<Student> students, List<Article> articles)
        {
            List<string> distinctStudentFullNames = new List<string>();
            List<string> distinctArticleNames = new List<string>();

            foreach (var student in students)
            {
                string stundentFullName = String.Concat(student.Name, student.Surname);
                var distinctStudentsChars = stundentFullName.ToLower().Distinct();
                distinctStudentFullNames.Add(new String(distinctStudentsChars.ToArray()));
            }
            foreach (var article in articles)
            {
                var distinctArticleChars = article.Name.ToLower().Distinct();
                distinctArticleNames.Add(new String(distinctArticleChars.ToArray()));
            }

            int i = 0;
            foreach (var (student, index) in distinctStudentFullNames.Select((value, i) => (value, i)))
            {
                var point = 0;

                foreach (var article in distinctArticleNames)
                {
                    //point += article.Count(student.Contains);
                    point += count(article, student);
                }
                students[index].Point = point;
            }
        }

        public void AddStudentsToUnivertiy(List<Student> students, List<University> universities)
        {
            var universityQuota = configuration["AppSettings:UniversityQuota"];
            foreach (var student in students)
            {
                //student.UniversityId = universities[i].Id;
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

        private int count(string str1, string str2)
        {
            int c = 0, j = 0;

            // Traverse the string 1 char by char
            for (int i = 0; i < str1.Length; i++)
            {

                // This will check if str1[i]
                // is present in str2 or not
                // str2.find(str1[i]) returns -1 if not found
                // otherwise it returns the starting occurrence
                // index of that character in str2
                if (str2.IndexOf(str1[i]) >= 0)
                {
                    c += 1;
                }
            }
            return c;
        }

    }
}
