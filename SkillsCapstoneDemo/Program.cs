using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;
using System.IO;
using System.Linq;
using System.Collections;
using System.Diagnostics;
using System.Text.RegularExpressions;
namespace ConsoleApp1
{
    public class Program
    {
        public static void Main(String[] args)
        {
            // Question 1.1 Main() function
            Console.WriteLine("---- Q1.1 Main() function and read file into course list");
            // Question 1.2 Question 1.2 Define Course class and read course file into course list
            Course[] coursesList = readCourseFile("Courses.csv"); // Read Excel file into a list
                                                                  // Question 1.3a Query course by subject, level
            var results = qCourses(coursesList, "IEE", 300);
            foreach (var result in results)
            {
                Console.WriteLine($"Course Title: {result.Title} - Instructor: {result.Instructor}");
            }
            // Question 1.3b Course by subject, code in groups, print courses with at least 2 courses
            var groups = qGroups(coursesList); // at least 2 courses
            int n = 2;
            foreach (var subjectGroup in groups)
            {
                var flag = true;
                foreach (var codeGroup in subjectGroup.Groups)
                {
                    if (codeGroup.Courses.Count >= n)
                    {
                        if (flag)
                        {
                            Console.WriteLine($"Subject: {subjectGroup.Subject}");
                            flag = false;
                        }
                        Console.WriteLine($"\tCode: {codeGroup.Code}");
                        foreach (var course in codeGroup.Courses)
                        {
                            Console.WriteLine($"\t\t{course.Title}");
                        }
                    }
                }
            }
            // Question 1.4 Create Instructor file manually and read the file into a list
            List<Instructor> instructorsList = readInstructorFile("Instructors.csv");
            Console.WriteLine(instructorsList.Count);
            // Question 1.5 Find instructor’s email address for each course
            var email_query_results = qNamesEmails(instructorsList, coursesList, 200);
            // Print all courses at the level = 200 - 299
            foreach (var result in email_query_results)
            {
                Console.WriteLine($"{result.Course} - {result.InstructorEmail}");
            }
            // Question 2.1a Retrieve CPI courses of number of 200 or higher
            coursesList = readCourseFile("Courses.csv"); // Read Excel file into a list
            var cpiCourses = qTitleInstructorInXml(coursesList);
            foreach (var course in cpiCourses)
            {
                Console.WriteLine(course);
            }
            // Question 2.1b Get courses in groups: subject 1st level key, code 2nd level key
            var courseGroups = qTitleCodeGroups(coursesList);
            Console.WriteLine(courseGroups);
            // Question 2.2 Deliver the result set in the type IEnumerable<XElement>
            var resultXML = qCourseInstructorEnum(instructorsList, coursesList);
            foreach (XElement courseElement in resultXML)
            {
                Console.WriteLine(courseElement);
            }
        }
        // Question 1.2 Read course file into course list/


        public static Course[] readCourseFile(string fpath)
        {
            int entriesFound = 0;
            Course temp;
            List<Course> dyList = new List<Course>();


            using (var textReader = new System.IO.StreamReader(fpath))
            { //using System.IO
                char _Delimiter = ','; // .csv comma separate values
                string line = textReader.ReadLine();
                int skipCount = 0;
                string[] subjCodeArr;
                while (line != null && skipCount < 1)
                { // skip file header
                    line = textReader.ReadLine();
                    skipCount++;
                }
                while (line != null)
                {
                    string[] columns = line.Split(_Delimiter);
                    entriesFound++;
                    line = textReader.ReadLine();



                    for (int i = 0; i < columns.Length; i++)
                    {
                        if (columns[i].Contains(" "))
                        {
                            columns[i].Replace(" ", " ");
                        }
                    }

                    subjCodeArr = columns[0].Split(' ');

                    temp = new Course(subjCodeArr[0].Trim(), subjCodeArr[1].Trim(), columns[1].Trim(), columns[7].Trim(), columns[3].Trim(), columns[2].Trim());
                    dyList.Add(temp);
                }
            }

            Course[] list = dyList.ToArray();
            return list;
        }
        public static dynamic qCourses(Course[] coursesList, String subject, int level)
        {
            Console.WriteLine("---- Q1.3a Query course by subject, level");
            IEnumerable<Course> results =
                from c in coursesList
                where Int32.Parse(c.Code) >= level && c.Subject == subject
                orderby c.Instructor ascending
                select c;


            return results;
        }

        public static dynamic qGroups(Course[] coursesList)
        {
            Console.WriteLine("---- Q1.3b Course by subject, code in groups");

            List<Course> temp = new List<Course>();

            for (int i = 0; i < coursesList.Length; i++)
            {
                temp.Add(coursesList[i]);
            }

            var groups =
                from c in coursesList
                group c by c.Subject into g
                select new
                {
                    Subject = g.Key,
                    Groups = from c in g
                             group c by c.Code into codeC
                             select new
                             {
                                 Code = codeC.Key,
                                 Courses = codeC.ToList<Course>()
                             }
                };

            return groups;

        }
        //Question 1.4 Create and read file into instructor list

        public static List<Instructor> readInstructorFile(string fpath)
        {
            Console.WriteLine("---- Q1.4 Read file into instructor list");
            int entriesFound = 0;
            Instructor temp;
            List<Instructor> tmpList = new List<Instructor>();


            using (var textReader = new System.IO.StreamReader(fpath))
            { //using System.IO
                char _Delimiter = ','; // .csv comma separate values
                string line = textReader.ReadLine();
                int skipCount = 0;
                while (line != null && skipCount < 1)
                { // skip file header
                    line = textReader.ReadLine();
                    skipCount++;
                }
                while (line != null)
                {
                    string[] columns = line.Split(_Delimiter);
                    entriesFound++;
                    line = textReader.ReadLine();
                    for (int i = 0; i < columns.Length; i++)
                    {
                        if (columns[i].Contains(" "))
                        {
                            columns[i].Replace(" ", " ");
                        }
                    }

                    temp = new Instructor(columns[0], columns[1], columns[2]);
                    tmpList.Add(temp);
                }
            }



            return tmpList;
        }
        //1.5 Implementation
        public static dynamic qNamesEmails(List<Instructor> instructorsList, Course[] coursesList, int level)
        {
            Console.WriteLine("---- Q1.5 Find instructor’s email address for each course");
            var query =
                from i in instructorsList
                join c in coursesList on i.InstructorName equals c.Instructor
                orderby c.Code ascending
                select new
                {
                    Course = c.Subject + c.Code,
                    InstructorEmail = i.EmailAddress
                };

            return query;
        }
        public static dynamic qTitleInstructorInXml(Course[] coursesList)
        {
            Console.WriteLine("---- Q2.1a Retrieve CPI courses of number of 200 or higher");
             
             
             
            return cpiCourses;
        }
        public static dynamic qTitleCodeGroups(Course[] coursesList)
        {
            Console.WriteLine("---- Q2.1b Get courses in groups: subject 1st level key, code 2nd level key");
             
             
             
            return courseGroups;
        }
        // Question 2.2 Deliver the result set in the type IEnumerable<XElement>
        public static dynamic qCourseInstructorEnum(List<Instructor> instructorsList, Course[] coursesList)
        {
            Console.WriteLine("---- Q2.2 Deliver the result set in the type IEnumerable<XElement>");
             
             
             
            return resultXML;
        }
    }
    public class Course // Question 1.2
    {
        public string Subject, Code, Title, Location, Instructor, CourseID;




        public Course(string Subject, string Code, string Title, string Location, string Instructor, string CourseID)
        {
            this.Subject = Subject;
            this.Code = Code;
            this.Title = Title;
            this.Location = Location;
            this.Instructor = Instructor;
            this.CourseID = CourseID;
        }
        public void setCode(string SubjectCode)
        {
            string[] s = SubjectCode.Split(' ');
            Subject = s[0];
            Code = s[1];
        }

        public void setTitle(string Title)
        {
            this.Title = Title;
        }

        public void setLoc(string Location)
        {
            this.Location = Location;
        }

        public void setIns(string Instructor)
        {
            this.Instructor = Instructor;
        }
        public void setID(string CourseID)
        {
            this.CourseID = CourseID;
        }
        public string getSubjectCode()
        {
            return Subject + " " + Code;
        }


        public string getTitle()
        {
            return Title;
        }

        public string getLoc()
        {
            return Location;
        }

        public string getIn()
        {
            return Instructor;
        }

        public string getID()
        {
            return CourseID;
        }

    }
    public class Instructor // Question 1.4
    {
        public string InstructorName, OfficeNumber, EmailAddress;

        public Instructor(string InstructorName, string OfficeNumber, string EmailAddress)
        {
            this.InstructorName = InstructorName;
            this.OfficeNumber = OfficeNumber;
            this.EmailAddress = EmailAddress;
        }

        public string getInstructorName()
        {
            return InstructorName;
        }

        public string getOfficeNumber()
        {
            return OfficeNumber;
        }

        public string getEmailAddress()
        {
            return EmailAddress;
        }

        public void setInstructorName(string InstructorName)
        {
            this.InstructorName = InstructorName;
        }

        public void setOfficeNumber(string OfficeNumber)
        {
            this.OfficeNumber = OfficeNumber;
        }

        public void setEmailAddress(string EmailAddress)
        {
            this.EmailAddress = EmailAddress;
        }
    }
}