using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;
using ProjectTeam06UBCInfoRegSys.EF_Classes;    //entities
using ProjectTeam06UBCInfoRegSys.GridView_Classes;  //grid view objects for binding list

namespace ProjectTeam06UBCInfoRegSys.AppForms
{
    public partial class UBCDataForm : Form
    {
        private Options option; //currently selected option
        private Users user; //current system user
        private UBCInfoRegEntities dbContext;   //database context

        public UBCDataForm(Options option, Users user)
        {
            InitializeComponent();
            this.option = option;
            this.user = user;
            dbContext = new UBCInfoRegEntities();

            //load all DbSets into dbContext (local copies)
            dbContext.Cities.Load();
            dbContext.Courses.Load();
            dbContext.Departments.Load();
            dbContext.Instructors.Load();
            dbContext.Provinces.Load();
            dbContext.Registrations.Load();
            dbContext.Sections.Load();
            dbContext.Students.Load();

            //modify form according to selected option
            switch(this.option)
            {
                case Options.Departments:
                    ShowDepartments();
                    break;
                case Options.Courses:
                case Options.Add_Delete_Course:
                    ShowCourses();
                    break;
                case Options.Instructors:
                    ShowInstructors();
                    break;
                case Options.Students:
                    ShowStudents();
                    break;
                case Options.Registrations:
                    ShowRegistration();
                    break;
                case Options.Add_Delete_Section:
                    ShowSections();
                    break;
            } //end switch
        }

        #region Form modifying methods

        /// <summary>
        /// Show departments in grid view method.
        /// </summary>
        private void ShowDepartments()
        {
            /*
             * set up data grid view control
             * auto-size columns
             * hide associations
             */
            dataGridViewData.DataSource = dbContext.Departments.Local.ToBindingList();
            dataGridViewData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewData.Columns["Courses"].Visible = false;
            dataGridViewData.Columns["Students"].Visible = false;
            dataGridViewData.Columns["Instructors"].Visible = false;

            //change form and label text
            Text = "UBC - Departments";
            labelGridViewName.Text = "Departments";

            labelShowTotal.Text =  dbContext.Departments.ToList().Count().ToString();   //total number of entries

            buttonFilters.Visible = false;  //no need to filter departments
            buttonShowAll.Visible = false;  //all departments are shown by default
        }

        /// <summary>
        /// Show courses in grid view method.
        /// </summary>
        private void ShowCourses()
        {
            /*
             * set up data grid view control
             * auto-size columns
             */
            dataGridViewData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            //custom query to retrieve department name
            var query = dbContext.Courses
                .OrderBy(c => c.Department.DepartmentName)
                .Select(c => new GridViewCourse
                {
                    CourseId = c.CourseId,
                    DepartmentId = c.DepartmentId,
                    DepartmentName = c.Department.DepartmentName,
                    CourseName = c.CourseName
                });

            //binding list allows to modify grid view after linking LINQ to it
            dataGridViewData.DataSource = new BindingList<GridViewCourse>(query.ToList());
            dataGridViewData.Refresh();

            //change form and label text
            Text = "UBC - Courses";
            labelGridViewName.Text = "Courses";

            labelShowTotal.Text = query.Count().ToString(); //total number of entries

            //enable adding/deleting/editing rows to/from grid view for admin
            if(user == Users.Admin)
            {
                dataGridViewData.AllowUserToAddRows = true;
                dataGridViewData.AllowUserToDeleteRows = true;
                dataGridViewData.ReadOnly = false;
                buttonAdd.Visible = true;    //show add button
                buttonDelete.Visible = true;    //show delete button
            }
        }

        /// <summary>
        /// Show instructors in grid view method.
        /// </summary>
        private void ShowInstructors()
        {
            /*
             * set up data grid view control
             * auto-size columns
             */
            dataGridViewData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            //custom query to retrieve department name
            var query = dbContext.Instructors
                .Select(i => new
                {
                    i.InstructorId,
                    i.InstructorFirstName,
                    i.InstructorLastName,
                    i.Department.DepartmentName
                });

            dataGridViewData.DataSource = query.ToList();
            dataGridViewData.Refresh();

            //change form and label text
            Text = "UBC - Instructors";
            labelGridViewName.Text = "Instructors";

            labelShowTotal.Text = query.Count().ToString(); //total number of entries
        }

        /// <summary>
        /// Show students in grid view method.
        /// </summary>
        private void ShowStudents()
        {
            /*
             * set up data grid view control
             * auto-size columns
             */
            dataGridViewData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            //custom query to retrieve department name
            var query = dbContext.Students
                .Select(s => new
                {
                    s.StudentId,
                    s.StudentFirstName,
                    s.StudentLastName,
                    s.Department.DepartmentName
                });

            dataGridViewData.DataSource = query.ToList();
            dataGridViewData.Refresh();

            //change form and label text
            Text = "UBC - Students";
            labelGridViewName.Text = "Students";

            labelShowTotal.Text = query.Count().ToString(); //total number of entries
        }

        /// <summary>
        /// Show registrations in grid view method.
        /// </summary>
        private void ShowRegistration()
        {
            /*
             * set up data grid view control
             * auto-size columns
             */
            dataGridViewData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            //custom query to retrieve department and course names
            var query = dbContext.Registrations
                .OrderBy(r => r.RegistrationId)
                .Select(r => new GridViewRegistration
                {
                    RegistrationId = r.RegistrationId,
                    StudentId = r.StudentId,
                    StudentFirstName = r.Student.StudentFirstName,
                    StudentLastName = r.Student.StudentLastName,
                    DepartmentId = r.DepartmentId,
                    DepartmentName = r.Department.DepartmentName,
                    CourseId = r.CourseId,
                    SectionId = r.SectionId,
                    CourseName = r.Course.CourseName
                });

            //binding list allows to modify grid view after linking LINQ to it
            dataGridViewData.DataSource = new BindingList<GridViewRegistration>(query.ToList());
            dataGridViewData.Refresh();

            //change form and label text
            Text = "UBC - Registrations";
            labelGridViewName.Text = "Registrations";

            labelShowTotal.Text = query.Count().ToString(); //total number of entries

            //enable adding/editing rows to grid view for students
            if(user == Users.Student)
            {
                dataGridViewData.AllowUserToAddRows = true;
                dataGridViewData.ReadOnly = false;
                buttonAdd.Text = "Register";    //change button name
                buttonAdd.Visible = true;    //show register button
            }
        }

        /// <summary>
        /// Show sections in grid view method.
        /// </summary>
        private void ShowSections()
        {
            /*
             * set up data grid view control
             * auto-size columns
             * enable adding/deleting/editing rows to/from grid view for admin
             */
            dataGridViewData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewData.AllowUserToAddRows = true;
            dataGridViewData.AllowUserToDeleteRows = true;
            dataGridViewData.ReadOnly = false;

            //custom query to retrieve department, instructor's first and last names
            var query = dbContext.Sections
                .OrderBy(s => s.Department.DepartmentName)
                .Select(s => new GridViewSection
                {
                    SectionId = s.SectionId,
                    CourseId = s.CourseId,
                    DepartmentId = s.DepartmentId,
                    DepartmentName = s.Department.DepartmentName,
                    InstructorId = s.Instructor.InstructorId,
                    InstructorFirstName = s.Instructor.InstructorFirstName,
                    InstructorLastName = s.Instructor.InstructorLastName
                });

            //binding list allows to modify grid view after linking LINQ to it
            dataGridViewData.DataSource = new BindingList<GridViewSection>(query.ToList());
            dataGridViewData.Refresh();

            //change form and label text
            Text = "UBC - Sections";
            labelGridViewName.Text = "Sections";

            labelShowTotal.Text = query.Count().ToString(); //total number of entries

            buttonAdd.Visible = true;    //show add button
            buttonDelete.Visible = true;    //show delete button
        }

        #endregion

        #region Form controls' event handlers

        /// <summary>
        /// Back button click event handler.
        /// </summary>
        /// <param name="sender">Object triggered event.</param>
        /// <param name="e">Event arguments.</param>
        private void buttonBack_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Closing form event handler.
        /// </summary>
        /// <param name="sender">Object triggered event.</param>
        /// <param name="e">Event arguments.</param>
        private void UBCDataForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            buttonBack.PerformClick();  //fire back button click event
        }

        /// <summary>
        /// Show all button click event handler.
        /// </summary>
        /// <param name="sender">Object triggered event.</param>
        /// <param name="e">Event arguments.</param>
        private void buttonShowAll_Click(object sender, EventArgs e)
        {
            //show all data in specific grid view based on selected option
            switch(option)
            {
                case Options.Courses:
                case Options.Add_Delete_Course:
                    ShowCourses();
                    break;
                case Options.Instructors:
                    ShowInstructors();
                    break;
                case Options.Students:
                    ShowStudents();
                    break;
                case Options.Registrations:
                    ShowRegistration();
                    break;
                case Options.Add_Delete_Section:
                    ShowSections();
                    break;
            } //end switch
        }

        /// <summary>
        /// Register button click event handler.
        /// </summary>
        /// <param name="sender">Object triggered event.</param>
        /// <param name="e">Event arguments.</param>
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            //modify grid view based on selected option
            switch(option)
            {
                case Options.Registrations:
                    AddRegistrations();
                    break;
                case Options.Add_Delete_Course:
                    AddCourses();
                    break;
                case Options.Add_Delete_Section:
                    AddSections();
                    break;
            } //end switch
        }

        /// <summary>
        /// Delete button click event handler.
        /// </summary>
        /// <param name="sender">Object triggered event.</param>
        /// <param name="e">Event arguments.</param>
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            //modify grid view based on selected option
            switch(option)
            {
                case Options.Add_Delete_Course:
                    DeleteCourses();
                    break;
                case Options.Add_Delete_Section:
                    DeleteSections();
                    break;
            } //end switch
        }

        /// <summary>
        /// Filters button click event handler.
        /// </summary>
        /// <param name="sender">Object triggered event.</param>
        /// <param name="e">Event arguments.</param>
        private void buttonFilters_Click(object sender, EventArgs e)
        {
            //lock main thread on new form
            if(new UBCFiltersForm(option, dbContext,
                dataGridViewData, labelShowTotal).ShowDialog() == DialogResult.OK)
            {
                //update data in controls
                dataGridViewData.Refresh();
                labelShowTotal.Refresh();
            }
        }

        #endregion

        #region Registrations, Courses, Sections adding/deleting methods

        /// <summary>
        /// Add registrations to database and grid view method.
        /// </summary>
        private void AddRegistrations()
        {
            //make sure rows are selected
            if(dataGridViewData.SelectedRows.Count == 0) return;

            bool hasRegistered = false; //flag to show successful registration message
            try
            {
                //check for existing entries in database to prevent duplicate insertions
                foreach(DataGridViewRow row in dataGridViewData.SelectedRows)
                {
                    if(dbContext.Registrations.ToList().Any(r => r.StudentId == (int)row.Cells[1].Value
                        && r.CourseId == (int)row.Cells[6].Value && r.DepartmentId == (int)row.Cells[4].Value))
                    {
                        //unselect duplicate row entry
                        row.Selected = false;
                        MessageBox.Show("Student already registered for the course");
                    }
                }

                //add registrations to database
                foreach(DataGridViewRow row in dataGridViewData.SelectedRows)
                {
                    dbContext.Registrations.Add(new Registration
                    {
                        StudentId = (int)row.Cells[1].Value,
                        SectionId = (int)row.Cells[7].Value,
                        CourseId = (int)row.Cells[6].Value,
                        DepartmentId = (int)row.Cells[4].Value
                    });
                    hasRegistered = true;
                }

                try
                {
                    //save changes to database
                    dbContext.SaveChanges();
                    if(hasRegistered) MessageBox.Show("Registered successfully");
                }
                catch(Exception) { MessageBox.Show("Registration update error"); }
            }
            catch(Exception) { MessageBox.Show("Registration error"); }
            finally { buttonShowAll.PerformClick(); }   //fire show all button click event
        }

        /// <summary>
        /// Add courses to database and grid view method.
        /// </summary>
        private void AddCourses()
        {
            //make sure rows are selected
            if(dataGridViewData.SelectedRows.Count == 0) return;

            bool hasAdded = false; //flag to show successful course update message
            try
            {
                //check for existing entries in database to prevent duplicate insertions
                foreach(DataGridViewRow row in dataGridViewData.SelectedRows)
                {
                    if(dbContext.Courses.ToList().Any(c => c.CourseId == (int)row.Cells[0].Value
                        && c.DepartmentId == (int)row.Cells[1].Value))
                    {
                        //unselect duplicate row entry
                        row.Selected = false;
                        MessageBox.Show("Course already exists");
                    }
                }

                //add courses to database
                foreach(DataGridViewRow row in dataGridViewData.SelectedRows)
                {
                    dbContext.Courses.Add(new Course
                    {
                        CourseId = (int)row.Cells[0].Value,
                        DepartmentId = (int)row.Cells[1].Value,
                        CourseName = (string)row.Cells[3].Value
                    });
                    hasAdded = true;
                }

                try
                {
                    //save changes to database
                    dbContext.SaveChanges();
                    if(hasAdded) MessageBox.Show("Course added successfully");
                }
                catch(Exception) { MessageBox.Show("Courses update error"); }
            }
            catch(Exception) { MessageBox.Show("Courses error"); }
            finally { buttonShowAll.PerformClick(); }   //fire show all button click event
        }

        /// <summary>
        /// Add sections to database and grid view method.
        /// </summary>
        private void AddSections()
        {
            //make sure rows are selected
            if(dataGridViewData.SelectedRows.Count == 0) return;

            bool hasAdded = false; //flag to show successful section update message
            try
            {
                //check for existing entries in database to prevent duplicate insertions
                foreach(DataGridViewRow row in dataGridViewData.SelectedRows)
                {
                    if(dbContext.Sections.ToList().Any(s => s.SectionId == (int)row.Cells[0].Value
                        && s.CourseId == (int)row.Cells[1].Value && s.DepartmentId == (int)row.Cells[2].Value))
                    {
                        //unselect duplicate row entry
                        row.Selected = false;
                        MessageBox.Show("Section already exists");
                    }
                }

                //add sections to database
                foreach(DataGridViewRow row in dataGridViewData.SelectedRows)
                {
                    dbContext.Sections.Add(new Section
                    {
                        SectionId = (int)row.Cells[0].Value,
                        CourseId = (int)row.Cells[1].Value,
                        DepartmentId = (int)row.Cells[2].Value,
                        InstructorId = (int)row.Cells[4].Value
                    });
                    hasAdded = true;
                }

                try
                {
                    //save changes to database
                    dbContext.SaveChanges();
                    if(hasAdded) MessageBox.Show("Section added successfully");
                }
                catch(Exception) { MessageBox.Show("Sections update error"); }
            }
            catch(Exception) { MessageBox.Show("Sections error"); }
            finally { buttonShowAll.PerformClick(); }   //fire show all button click event
        }

        /// <summary>
        /// Delete courses from database and grid view method.
        /// </summary>
        private void DeleteCourses()
        {
            //make sure rows are selected
            if(dataGridViewData.SelectedRows.Count == 0) return;

            bool sectionsExist = false; //flag to show successful course delete message
            try
            {
                //check for existing sections of the course
                foreach(DataGridViewRow row in dataGridViewData.SelectedRows)
                {
                    if(dbContext.Courses.ToList().Any(c => c.Sections.Any()))
                    {
                        //unselect course row with active sections
                        row.Selected = false;
                        sectionsExist = true;
                        MessageBox.Show("Can not delete, active sections exist");
                    }
                }

                //delete courses from database
                List<Course> coursesToDelete = new List<Course>();
                foreach(DataGridViewRow row in dataGridViewData.SelectedRows)
                    coursesToDelete.Add(dbContext.Courses.Find(row.Cells[0].Value, row.Cells[1].Value));
                dbContext.Courses.RemoveRange(coursesToDelete);

                try
                {
                    //save changes to database
                    dbContext.SaveChanges();
                    if(!sectionsExist) MessageBox.Show("Course deleted successfully");
                }
                catch(Exception) { MessageBox.Show("Courses delete error"); }
            }
            catch(Exception) { MessageBox.Show("Courses error"); }
            finally { buttonShowAll.PerformClick(); }   //fire show all button click event
        }

        /// <summary>
        /// Delete sections from database and grid view method.
        /// </summary>
        private void DeleteSections()
        {
            //make sure rows are selected
            if(dataGridViewData.SelectedRows.Count == 0) return;

            bool studentsRegistered = false;    //flag to show successful section delete message
            try
            {
                //check for students registered for the section
                foreach(DataGridViewRow row in dataGridViewData.SelectedRows)
                {
                    if(dbContext.Sections.ToList().Any(s => s.Registrations.Any()))
                    {
                        //unselect section row with registered students
                        row.Selected = false;
                        studentsRegistered = true;
                        MessageBox.Show("Can not delete, active registrations exist");
                    }
                }

                //delete sections from database
                List<Section> sectionsToDelete = new List<Section>();
                foreach(DataGridViewRow row in dataGridViewData.SelectedRows)
                    sectionsToDelete.Add(dbContext.Sections.Find(row.Cells[0].Value, row.Cells[1].Value,
                        row.Cells[2].Value));
                dbContext.Sections.RemoveRange(sectionsToDelete);

                try
                {
                    //save changes to database
                    dbContext.SaveChanges();
                    if(!studentsRegistered) MessageBox.Show("Section deleted successfully");
                }
                catch(Exception) { MessageBox.Show("Sections delete error"); }
            }
            catch(Exception) { MessageBox.Show("Sections error"); }
            finally { buttonShowAll.PerformClick(); }   //fire show all button click event
        }

        #endregion
    }
}