using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using ProjectTeam06UBCInfoRegSys.EF_Classes;    //entities
using ProjectTeam06UBCInfoRegSys.AppForms;  //application forms

namespace ProjectTeam06UBCInfoRegSys
{
    /// <summary>
    /// Represents users of the system.
    /// </summary>
    public enum Users
    {
        Student,
        Instructor,
        Guest,
        Admin
    }

    public partial class UBCMainForm : Form
    {
        private UBCInfoRegEntities dbContext;   //database context

        public UBCMainForm()
        {
            InitializeComponent();
            dbContext = new UBCInfoRegEntities();

            //open and read files with test data
            //ProcessFiles(); //uncomment this line on initial start of application

            //populate drop down menu with user options
            comboBoxUsers.Items.Add(Users.Student);
            comboBoxUsers.Items.Add(Users.Instructor);
            comboBoxUsers.Items.Add(Users.Guest);
            comboBoxUsers.Items.Add(Users.Admin);

            //select guest user by default
            comboBoxUsers.SelectedItem = Users.Guest;
        }

        /// <summary>
        /// Login button click event handler.
        /// </summary>
        /// <param name="sender">Object triggered event.</param>
        /// <param name="e">Event arguments.</param>
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            //retrieve selected user from drop down menu
            Users user = (Users)Enum.Parse(typeof(Users), comboBoxUsers.SelectedItem.ToString());
            Hide(); //hide main form

            if(new UBCOptionsForm(user).ShowDialog() == DialogResult.OK)    //lock main thread on new form
                Show(); //unhide main form
        }

        #region Files processing

        /// <summary>
        /// Open-read files method.
        /// </summary>
        private void ProcessFiles()
        {
            //add provinces
            StreamReader reader = File.OpenText(@"Data\Provinces.csv");
            List<Province> provinces = new List<Province>();
            while(!reader.EndOfStream)
            {
                string[] fields = reader.ReadLine().Trim().Split(',');
                provinces.Add(new Province
                {
                    ProvinceId = int.Parse(fields[0]),
                    ProvinceName = fields[1],
                    ProvinceDescription = fields[2]
                });
            }
            reader.Close();
            dbContext.Provinces.AddRange(provinces);

            //add cities
            reader = File.OpenText(@"Data\Cities.csv");
            List<City> cities = new List<City>();
            while(!reader.EndOfStream)
            {
                string[] fields = reader.ReadLine().Trim().Split(',');
                cities.Add(new City
                {
                    CityId = int.Parse(fields[0]),
                    CityName = fields[1],
                    ProvinceId = int.Parse(fields[2])
                });
            }
            reader.Close();
            dbContext.Cities.AddRange(cities);

            //add departments
            reader = File.OpenText(@"Data\Departments.csv");
            List<Department> departments = new List<Department>();
            while(!reader.EndOfStream)
            {
                string[] fields = reader.ReadLine().Trim().Split(',');
                departments.Add(new Department
                {
                    DepartmentId = int.Parse(fields[0]),
                    DepartmentName = fields[1],
                    DepartmentDescription = fields[2]
                });
            }
            reader.Close();
            dbContext.Departments.AddRange(departments);

            //add courses
            reader = File.OpenText(@"Data\Courses.csv");
            List<Course> courses = new List<Course>();
            while(!reader.EndOfStream)
            {
                string[] fields = reader.ReadLine().Trim().Split(',');
                courses.Add(new Course
                {
                    CourseId = int.Parse(fields[0]),
                    DepartmentId = int.Parse(fields[1]),
                    CourseName = fields[2]
                });
            }
            reader.Close();
            dbContext.Courses.AddRange(courses);

            //add instructors
            reader = File.OpenText(@"Data\Instructors.csv");
            List<Instructor> instructors = new List<Instructor>();
            while(!reader.EndOfStream)
            {
                string[] fields = reader.ReadLine().Trim().Split(',');
                instructors.Add(new Instructor
                {
                    InstructorId = int.Parse(fields[0]),
                    InstructorFirstName = fields[1],
                    InstructorLastName = fields[2],
                    InstructorAddress = fields[3],
                    CityId = int.Parse(fields[4]),
                    DepartmentId = int.Parse(fields[5])
                });
            }
            reader.Close();
            dbContext.Instructors.AddRange(instructors);

            //add students
            reader = File.OpenText(@"Data\Students.csv");
            List<Student> students = new List<Student>();
            while(!reader.EndOfStream)
            {
                string[] fields = reader.ReadLine().Trim().Split(',');
                students.Add(new Student
                {
                    StudentId = int.Parse(fields[0]),
                    StudentFirstName = fields[1],
                    StudentLastName = fields[2],
                    StudentAddress = fields[3],
                    CityId = int.Parse(fields[4]),
                    DepartmentId = int.Parse(fields[5])
                });
            }
            reader.Close();
            dbContext.Students.AddRange(students);

            //add sections
            reader = File.OpenText(@"Data\Sections.csv");
            List<Section> sections = new List<Section>();
            while(!reader.EndOfStream)
            {
                string[] fields = reader.ReadLine().Trim().Split(',');
                sections.Add(new Section
                {
                    SectionId = int.Parse(fields[0]),
                    CourseId = int.Parse(fields[1]),
                    DepartmentId = int.Parse(fields[2]),
                    InstructorId = int.Parse(fields[3])
                });
            }
            reader.Close();
            dbContext.Sections.AddRange(sections);

            //add students and sections to registrations
            List<Registration> registrations = new List<Registration>();
            IEnumerator<Student> iter = students.GetEnumerator();   //keeps track of position
            for(int i = 0; i < sections.Count; i++)
            {
                int count = 0;
                while(count != 5 && iter.MoveNext())
                {
                    //add 5 students and sections to each registration
                    registrations.Add(new Registration
                    {
                        StudentId = iter.Current.StudentId,
                        DepartmentId = sections[i].DepartmentId,
                        CourseId = sections[i].CourseId,
                        SectionId = sections[i].SectionId
                    });
                    count++;
                }
            }
            dbContext.Registrations.AddRange(registrations);

            dbContext.SaveChanges();
        }

        #endregion
    }
}