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
        // Question 1.2 Read course file into course list
        public static Course[] readCourseFile(string fpath)
        {
            Console.WriteLine("---- Q1.2 Read file into course list");
             
 
 
            return tmpList.ToArray<Course>();
        }
        public static dynamic qCourses(Course[] coursesList, String subject, int level)
        {
            Console.WriteLine("---- Q1.3a Query course by subject, level");

            return results;
        }
        public static dynamic qGroups(Course[] coursesList)
        {
            Console.WriteLine("---- Q1.3b Course by subject, code in groups");
             
             
             
            return groups;
        }
        //Question 1.4 Create and read file into instructor list
        public static List<Instructor> readInstructorFile(string fpath)
        {
            Console.WriteLine("---- Q1.4 Read file into instructor list");
             
             
             
            return tmpList;
        }
        public static dynamic qNamesEmails(List<Instructor> instructorsList, Course[] coursesList, int level)
        {
            Console.WriteLine("---- Q1.5 Find instructor’s email address for each course");
             
             
             
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
         
    }
    public class Instructor // Question 1.4
    {
         
    }
}