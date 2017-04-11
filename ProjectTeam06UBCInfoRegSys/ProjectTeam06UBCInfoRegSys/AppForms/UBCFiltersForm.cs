using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using ProjectTeam06UBCInfoRegSys.GridView_Classes;  //grid view objects for binding list

namespace ProjectTeam06UBCInfoRegSys.AppForms
{
    public partial class UBCFiltersForm : Form
    {
        //reference fields
        private Options option; //currently selected option
        private UBCInfoRegEntities dbContext;   //database context
        private DataGridView dataGridViewData;  //grid view to apply filters to
        private Label labelShowTotal;   //total number of entries label

        public UBCFiltersForm(Options option, UBCInfoRegEntities dbContext,
            DataGridView dataGridViewData, Label labelShowTotal)
        {
            InitializeComponent();
            this.option = option;
            this.dbContext = dbContext;
            this.dataGridViewData = dataGridViewData;
            this.labelShowTotal = labelShowTotal;

            //modify form according to selected option
            switch(this.option)
            {
                case Options.Courses:
                case Options.Add_Delete_Course:
                    ShowCourseFilters();
                    break;
                case Options.Instructors:
                    ShowInstructorFilters();
                    break;
                case Options.Students:
                    ShowStudentFilters();
                    break;
                case Options.Registrations:
                    ShowRegistrationSectionFilters();
                    break;
                case Options.Add_Delete_Section:
                    ShowRegistrationSectionFilters();
                    break;
            } //end switch
        }

        #region Form modifying methods

        /// <summary>
        /// Show course filter controls method.
        /// </summary>
        private void ShowCourseFilters()
        {
            //hide irrelevant labels and text boxes
            labelFirstName.Visible = false;
            labelLastName.Visible = false;
            labelCourseId.Visible = false;
            textBoxFirstName.Visible = false;
            textBoxLastName.Visible = false;
            textBoxCourseId.Visible = false;

            //reposition groupbox
            groupBoxDepartments.Location = new Point(211, 63);
        }

        /// <summary>
        /// Show instructor filter controls method.
        /// </summary>
        private void ShowInstructorFilters()
        {
            //hide irrelevant label and text box
            labelCourseId.Visible = false;
            textBoxCourseId.Visible = false;

            //rename role label
            labelRole.Text = "Instructor";
        }

        /// <summary>
        /// Show student filter controls method.
        /// </summary>
        private void ShowStudentFilters()
        {
            //hide irrelevant label and text box
            labelCourseId.Visible = false;
            textBoxCourseId.Visible = false;

            //rename role label
            labelRole.Text = "Student";
        }

        /// <summary>
        /// Show section filter controls method.
        /// </summary>
        private void ShowRegistrationSectionFilters()
        {
            //hide irrelevant labels and text boxes
            labelFirstName.Visible = false;
            labelLastName.Visible = false;
            textBoxFirstName.Visible = false;
            textBoxLastName.Visible = false;

            //reposition label and text box
            labelCourseId.Location = new Point(278, 117);
            textBoxCourseId.Location = new Point(357, 116);

            MessageBox.Show("Enter both Department and Course ID"); //hint for filters
        }

        #endregion

        #region Form controls' event handlers

        /// <summary>
        /// Cancel button click event handler.
        /// </summary>
        /// <param name="sender">Object triggered event.</param>
        /// <param name="e">Event arguments.</param>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// Closing form event handler.
        /// </summary>
        /// <param name="sender">Object triggered event.</param>
        /// <param name="e">Event arguments.</param>
        private void UBCFiltersForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            buttonCancel.PerformClick();    //fire cancel button click event
        }

        /// <summary>
        /// Apply button click event handler.
        /// </summary>
        /// <param name="sender">Object triggered event.</param>
        /// <param name="e">Event arguments.</param>
        private void buttonApply_Click(object sender, EventArgs e)
        {
            //apply specific filters
            switch(option)
            {
                case Options.Courses:
                case Options.Add_Delete_Course:
                    FilterCourses();
                    break;
                case Options.Instructors:
                    FilterInstructors();
                    break;
                case Options.Students:
                    FilterStudents();
                    break;
                case Options.Registrations:
                    FilterRegistrations();
                    break;
                case Options.Add_Delete_Section:
                    FilterSections();
                    break;
            } //end switch

            DialogResult = DialogResult.OK;
        }

        #endregion

        #region Filter methods

        /// <summary>
        /// Apply filters to courses method.
        /// </summary>
        private void FilterCourses()
        {
            IEnumerable<GridViewCourse> query = null;
            if(!CheckBoxesEmpty())
            {
                var checkBoxes = GetSelectedCheckBoxes();

                //custom query with multiple selected departments
                query = dbContext.Courses.AsEnumerable()
                    .OrderBy(c => c.Department.DepartmentName)
                    .Where(c => checkBoxes.Select(ch => ch.Text).Contains(c.Department.DepartmentName))
                    .Select(c => new GridViewCourse
                    {
                        CourseId = c.CourseId,
                        DepartmentId = c.DepartmentId,
                        DepartmentName = c.Department.DepartmentName,
                        CourseName = c.CourseName
                    });
            }
            else
            {
                //query with all records
                query = dbContext.Courses
                    .OrderBy(c => c.Department.DepartmentName)
                    .Select(c => new GridViewCourse
                    {
                        CourseId = c.CourseId,
                        DepartmentId = c.DepartmentId,
                        DepartmentName = c.Department.DepartmentName,
                        CourseName = c.CourseName
                    });
            }

            //binding list allows to modify grid view after linking LINQ to it
            dataGridViewData.DataSource = new BindingList<GridViewCourse>(query.ToList());
            labelShowTotal.Text = query.Count().ToString(); //total number of entries
        }

        /// <summary>
        /// Apply filters to instructors method.
        /// </summary>
        private void FilterInstructors()
        {
            if(!CheckBoxesEmpty())
            {
                var checkBoxes = GetSelectedCheckBoxes();

                //custom query with multiple selected departments and specific first and last names
                var query = dbContext.Instructors.AsEnumerable()
                    .OrderBy(i => i.InstructorId)
                    .Where(i => checkBoxes.Select(ch => ch.Text).Contains(i.Department.DepartmentName) |
                        (textBoxFirstName.Text.Equals(i.InstructorFirstName) && //match both first and last names only
                            textBoxLastName.Text.Equals(i.InstructorLastName)))
                    .Select(i => new
                    {
                        i.InstructorId,
                        i.InstructorFirstName,
                        i.InstructorLastName,
                        i.Department.DepartmentName
                    });

                dataGridViewData.DataSource = query.ToList();
                labelShowTotal.Text = query.Count().ToString(); //total number of entries
            }
            else if(CheckBoxesEmpty() && !TextBoxesEmpty())
            {
                //custom query with specific first and last names
                var query = dbContext.Instructors
                    .OrderBy(i => i.InstructorId)
                    .Where(i => textBoxFirstName.Text.Equals(i.InstructorFirstName) &&
                        textBoxLastName.Text.Equals(i.InstructorLastName))  //match both first and last names only
                    .Select(i => new
                    {
                        i.InstructorId,
                        i.InstructorFirstName,
                        i.InstructorLastName,
                        i.Department.DepartmentName
                    });

                dataGridViewData.DataSource = query.ToList();
                labelShowTotal.Text = query.Count().ToString(); //total number of entries
            }
            else
            {
                //query with all records
                var query = dbContext.Instructors
                    .OrderBy(i => i.InstructorId)
                    .Select(i => new
                    {
                        i.InstructorId,
                        i.InstructorFirstName,
                        i.InstructorLastName,
                        i.Department.DepartmentName
                    });

                dataGridViewData.DataSource = query.ToList();
                labelShowTotal.Text = query.Count().ToString(); //total number of entries
            }
        }

        /// <summary>
        /// Apply filters to students method.
        /// </summary>
        private void FilterStudents()
        {
            if(!CheckBoxesEmpty())
            {
                var checkBoxes = GetSelectedCheckBoxes();

                //custom query with multiple selected departments and specific first and last names
                var query = dbContext.Students.AsEnumerable()
                    .OrderBy(s => s.StudentId)
                    .Where(s => checkBoxes.Select(ch => ch.Text).Contains(s.Department.DepartmentName) |
                        (textBoxFirstName.Text.Equals(s.StudentFirstName) && //match both first and last names only
                            textBoxLastName.Text.Equals(s.StudentLastName)))
                    .Select(s => new
                    {
                        s.StudentId,
                        s.StudentFirstName,
                        s.StudentLastName,
                        s.Department.DepartmentName
                    });

                dataGridViewData.DataSource = query.ToList();
                labelShowTotal.Text = query.Count().ToString(); //total number of entries
            }
            else if(CheckBoxesEmpty() && !TextBoxesEmpty())
            {
                //custom query with specific first and last names
                var query = dbContext.Students
                    .OrderBy(s => s.StudentId)
                    .Where(s => textBoxFirstName.Text.Equals(s.StudentFirstName) &&
                        textBoxLastName.Text.Equals(s.StudentLastName))  //match both first and last names only
                    .Select(s => new
                    {
                        s.StudentId,
                        s.StudentFirstName,
                        s.StudentLastName,
                        s.Department.DepartmentName
                    });

                dataGridViewData.DataSource = query.ToList();
                labelShowTotal.Text = query.Count().ToString(); //total number of entries
            }
            else
            {
                //query with all records
                var query = dbContext.Students
                    .OrderBy(s => s.StudentId)
                    .Select(s => new
                    {
                        s.StudentId,
                        s.StudentFirstName,
                        s.StudentLastName,
                        s.Department.DepartmentName
                    });

                dataGridViewData.DataSource = query.ToList();
                labelShowTotal.Text = query.Count().ToString(); //total number of entries
            }
        }

        /// <summary>
        /// Apply filters to registrations method.
        /// </summary>
        private void FilterRegistrations()
        {
            //evaluate course id text box input
            int courseId = 0;
            try { courseId = int.Parse(textBoxCourseId.Text); }
            catch(FormatException) { MessageBox.Show("Invalid Course ID"); }

            IEnumerable<GridViewRegistration> query = null;
            if(!CheckBoxesEmpty())
            {
                var checkBoxes = GetSelectedCheckBoxes();

                //custom query with multiple selected departments and specific course id
                query = dbContext.Registrations.AsEnumerable()
                    .Where(r => checkBoxes.Select(ch => ch.Text).Contains(r.Department.DepartmentName) &
                        courseId == r.CourseId)   //both filter options must be applied
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
            }
            else
            {
                //query with all records
                query = dbContext.Registrations
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
            }

            //binding list allows to modify grid view after linking LINQ to it
            dataGridViewData.DataSource = new BindingList<GridViewRegistration>(query.ToList());
            labelShowTotal.Text = query.Count().ToString(); //total number of entries
        }

        /// <summary>
        /// Apply filters to sections method.
        /// </summary>
        private void FilterSections()
        {
            //evaluate course id text box input
            int courseId = 0;
            try { courseId = int.Parse(textBoxCourseId.Text); }
            catch(FormatException) { MessageBox.Show("Invalid Course ID"); }

            IEnumerable<GridViewSection> query = null;
            if(!CheckBoxesEmpty())
            {
                var checkBoxes = GetSelectedCheckBoxes();

                //custom query with multiple selected departments and specific course id
                query = dbContext.Sections.AsEnumerable()
                    .OrderBy(s => s.Department.DepartmentName)
                    .Where(s => checkBoxes.Select(ch => ch.Text).Contains(s.Department.DepartmentName) &
                        courseId == s.CourseId) //both filter options must be applied
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
            }
            else
            {
                //query with all records
                query = dbContext.Sections
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
            }

            //binding list allows to modify grid view after linking LINQ to it
            dataGridViewData.DataSource = new BindingList<GridViewSection>(query.ToList());
            labelShowTotal.Text = query.Count().ToString(); //total number of entries
        }

        #endregion

        #region Controls' input check methods

        /// <summary>
        /// Check for empty check boxes method.
        /// </summary>
        /// <returns>True if all check boxes are empty, false otherwise</returns>
        private bool CheckBoxesEmpty()
        {
            bool allEmpty = true;
            foreach(CheckBox ch in groupBoxDepartments.Controls)
            {
                //break if any is checked
                if(ch.Checked)
                {
                    allEmpty = false;
                    break;
                }
            }
            return allEmpty;
        }

        /// <summary>
        /// Check for empty text boxes method.
        /// </summary>
        /// <returns>True if either of text boxes is empty, false otherwise</returns>
        private bool TextBoxesEmpty()
        {
            return textBoxFirstName.Text.Equals("") || textBoxLastName.Text.Equals("");
        }

        #endregion

        /// <summary>
        /// Retrieve selected check boxes method.
        /// </summary>
        /// <returns>IEnumerable object which contains selected check boxes.</returns>
        private IEnumerable<CheckBox> GetSelectedCheckBoxes()
        {
            return groupBoxDepartments.Controls.OfType<CheckBox>()
                    .Where(ch => ch.Checked);
        }
    }
}